using System.Diagnostics;
using FubuCore;
using FubuCore.Binding;
using FubuCore.Binding.InMemory;
using FubuCore.Conversion;
using FubuCore.Formatting;
using FubuCore.Logging;
using FubuMVC.Core;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Diagnostics.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.Cookies;
using FubuMVC.Core.Json;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Resources.PathBased;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Runtime.Conditionals;
using FubuMVC.Core.Runtime.Files;
using FubuMVC.Core.Runtime.SessionState;
using FubuMVC.Core.Urls;
using NUnit.Framework;
using StructureMap;

namespace FubuMVC.Tests
{
    [TestFixture]
    public class required_service_registrations
    {
        [Test]
        public void all_default_service_registrations()
        {
            using (var runtime = FubuRuntime.Basic())
            {
                var _ = runtime.Get<IContainer>();


                Debug.WriteLine(_.WhatDoIHave());

                // Model Binding registrations
                _.DefaultRegistrationIs<IBindingContext, BindingContext>();
                _.DefaultRegistrationIs<IObjectResolver, ObjectResolver>();
                _.DefaultRegistrationIs<IBindingLogger, NulloBindingLogger>();
                _.ShouldHaveRegistration<IModelBinder, ResourcePathBinder>();
                _.ShouldHaveRegistration<IConverterFamily, AspNetPassthroughConverter>();


                // Core services
                _.DefaultRegistrationIs<IFubuRequestContext, FubuRequestContext>();
                _.DefaultRegistrationIs<IRequestData, FubuMvcRequestData>();
                _.DefaultRegistrationIs<IExceptionHandlingObserver, ExceptionHandlingObserver>();
                _.DefaultRegistrationIs<ICookies, Cookies>();
                _.DefaultRegistrationIs<IFlash, FlashProvider>();
                _.DefaultRegistrationIs<IOutputWriter, OutputWriter>();
                _.DefaultRegistrationIs<IPartialFactory, PartialFactory>();
                _.DefaultRegistrationIs<IPartialInvoker, PartialInvoker>();
                _.DefaultRegistrationIs<IRequestDataProvider, RequestDataProvider>();
                _.DefaultRegistrationIs<Stringifier, Stringifier>(); // it's goofy, but assert that it exists


                _.DefaultSingletonIs<IChainResolver, ChainResolutionCache>();

                _.DefaultRegistrationIs<IDisplayFormatter, DisplayFormatter>();
                _.DefaultRegistrationIs<IFileSystem, FileSystem>();

                _.ShouldHaveRegistration<ILogModifier, LogRecordModifier>();

                _.DefaultRegistrationIs<IObjectConverter, ObjectConverter>();
                _.DefaultRegistrationIs<ISetterBinder, SetterBinder>();

                _.DefaultRegistrationIs<IFubuApplicationFiles, FubuApplicationFiles>();

                _.DefaultRegistrationIs<IUrlRegistry, UrlRegistry>();
                _.DefaultRegistrationIs<IChainUrlResolver, ChainUrlResolver>();
                _.DefaultRegistrationIs<AppReloaded, AppReloaded>();

                _.DefaultRegistrationIs<ILogger, Logger>();

                // Conneg
                _.DefaultRegistrationIs<IResourceNotFoundHandler, DefaultResourceNotFoundHandler>();


                // Conditionals
                _.DefaultRegistrationIs<IContinuationProcessor, ContinuationProcessor>();
                _.DefaultRegistrationIs<IConditionalService, ConditionalService>();

                // Diagnostics
                _.DefaultSingletonIs<IDiagnosticAssets, DiagnosticAssetsCache>();


                // Json
                _.DefaultRegistrationIs<IJsonSerializer, NewtonSoftJsonSerializer>();
            }
        }
    }
}