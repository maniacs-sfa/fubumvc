using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuCore.Descriptions;
using FubuCore.Util;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Runtime.Formatters;
using StructureMap.Pipeline;

namespace FubuMVC.Core.Resources.Conneg
{
    public class OutputChoice<T>
    {
        public string MimeType;
        public IMediaWriter<T> Writer;
    }

    public class OutputNode : BehaviorNode, IMayHaveResourceType, DescribesItself, IOutputNode
    {
        private readonly Lazy<IEnumerable<IMediaWriter>> _allMedia;
        private readonly Cache<string, object> _choices = new Cache<string, object>();
        private readonly IList<IMediaWriter> _media = new List<IMediaWriter>();
        private ConnegSettings _settings;

        public OutputNode(Type resourceType)
        {
            if (resourceType == typeof(void))
                throw new ArgumentOutOfRangeException("Void is not a valid resource type");

            if (resourceType == null)
                throw new ArgumentNullException("resourceType");

            ResourceType = resourceType;

            _allMedia = new Lazy<IEnumerable<IMediaWriter>>(() =>
            {
                var settings = _settings ?? new ConnegSettings();

                settings.ApplyRules(this);

                return _media;
            });
        }

        public IEnumerable<IMediaWriter> Explicits
        {
            get { return _media; }
        }

        public override BehaviorCategory Category
        {
            get { return BehaviorCategory.Output; }
        }

        public override string Description
        {
            get { return ToString(); }
        }

        #region DescribesItself Members

        void DescribesItself.Describe(Description description)
        {
            description.Title = "Conneg Output";
            description.ShortDescription = "Render the output for resource " + ResourceType.Name;

            description.AddList("Media", _media);
        }

        #endregion

        #region IMayHaveResourceType Members

        Type IMayHaveResourceType.ResourceType()
        {
            return ResourceType;
        }

        #endregion

        public OutputChoice<T> ChooseOutput<T>(string accepts)
        {
            _choices.Fill(accepts, chooseOutput<T>);
            return (OutputChoice<T>) _choices[accepts];
        }

        public void Add(IFormatter formatter)
        {
            var writer = typeof(FormatterWriter<>).CloseAndBuildAs<object>(formatter, ResourceType).As<IMediaWriter>();
            addWriter(writer);
        }

        public void Add(Type mediaWriterType)
        {
            if (!mediaWriterType.IsOpenGeneric() || !mediaWriterType.Closes(typeof(IMediaWriter<>)) ||
                !mediaWriterType.IsConcreteWithDefaultCtor())
                throw new ArgumentOutOfRangeException("mediaWriterType",
                    "mediaWriterType must implement IMediaWriter<T> and have a default constructor");

            var writerType = mediaWriterType.MakeGenericType(ResourceType);


            var writer = Activator.CreateInstance(writerType).As<IMediaWriter>();

            addWriter(writer);
        }

        public void Add(IMediaWriter writer)
        {
            var writerType = typeof(IMediaWriter<>).MakeGenericType(ResourceType);
            if (!writerType.IsInstanceOfType(writer))
                throw new ArgumentOutOfRangeException("writer", "writer must implement " + writerType.GetFullName());

            addWriter(writer);
        }


        public IEnumerable<IMediaWriter> Media()
        {
            return _allMedia.Value;
        }

        public IEnumerable<IMediaWriter<T>> Media<T>()
        {
            return Media().OfType<IMediaWriter<T>>();
        }

        public bool HasView()
        {
            return _media.Any(x => x.Mimetypes.Contains(MimeType.Html.Value));
        }

        public Type ResourceType { get; }

        /// <summary>
        ///     Use this if you want to override the handling for
        ///     the resource not being found on a chain by chain
        ///     basis
        /// </summary>
        public Instance ResourceNotFound { get; set; }

        /// <summary>
        ///     Use the specified type T as the resource not found handler strategy
        ///     for only this chain
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void UseForResourceNotFound<T>() where T : IResourceNotFoundHandler
        {
            ResourceNotFound = new SmartInstance<T>();
        }


        public void ClearAll()
        {
            _media.Clear();
        }

        public IEnumerable<string> MimeTypes()
        {
            return _allMedia.Value.SelectMany(x => x.Mimetypes).Distinct();
        }

        public bool Writes(MimeType mimeType)
        {
            return MimeTypes().Contains(mimeType.ToString());
        }

        public bool Writes(string mimeType)
        {
            return MimeTypes().Contains(mimeType);
        }

        private OutputChoice<T> chooseOutput<T>(string accepts)
        {
            if (!_allMedia.Value.Any()) return null;

            var mimeTypes = new MimeTypeList(accepts);

            foreach (var acceptType in mimeTypes)
            {
                var candidate = _allMedia.Value.FirstOrDefault(x => x.Mimetypes.Contains(acceptType));
                if (candidate != null)
                    return new OutputChoice<T>
                    {
                        MimeType = acceptType,
                        Writer = (IMediaWriter<T>) candidate
                    };
            }

            if (mimeTypes.DefaultIfEmpty("*/*").Contains("*/*"))
            {
                var media = _allMedia.Value.FirstOrDefault();
                return new OutputChoice<T>
                {
                    MimeType = media.Mimetypes.First(),
                    Writer = (IMediaWriter<T>) media
                };
            }

            return null;
        }

        private void addWriter(IMediaWriter writer)
        {
            _media.Add(writer);
        }


        protected override IConfiguredInstance buildInstance()
        {
            var instance = new ConfiguredInstance(typeof(OutputBehavior<>), ResourceType);
            instance.Dependencies.Add(typeof(IOutputNode), this);

            if (ResourceNotFound != null)
                instance.Dependencies.Add(typeof(IResourceNotFoundHandler), ResourceNotFound);

            return instance;
        }

        public override string ToString()
        {
            return _media.Select(x => x.ToString()).Join(", ");
        }

        public void UseSettings(ConnegSettings settings)
        {
            _settings = settings;
        }
    }
}