using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FubuCore.Descriptions;
using FubuCore.Util;

namespace FubuMVC.Core.Http.Owin
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class OwinSettings : DescribesItself
    {
        /// <summary>
        ///     A list of keys and their associated values that will be injected by the host into each OWIN request environment.
        /// </summary>
        public readonly Cache<string, object> EnvironmentData = new Cache<string, object>();

        /// <summary>
        ///     Provides granular control over the headers that are written by the host
        /// </summary>
        public readonly OwinHeaderSettings Headers = new OwinHeaderSettings();

        /// <summary>
        ///     Key value pairs to control or alter the behavior of the underlying host
        /// </summary>
        public readonly IDictionary<string, object> Properties = new Dictionary<string, object>();

        public OwinSettings()
        {
            EnvironmentData.Fill(OwinConstants.HeaderSettings, Headers);
        }

        public void Describe(Description description)
        {
            description.Title = "OWIN Settings";
            description.ShortDescription =
                "Governs the attachment and ordering of OWIN middleware plus OWIN host properties";
            Properties.Each(x => description.Properties[x.Key] = x.Value.ToString());

            EnvironmentData.Each((key, value) => description.Properties[key] = value.ToString());

            var middleware = new BulletList {Name = "Middleware", Label = "Middleware"};

            description.BulletLists.Add(middleware);
        }
    }
}