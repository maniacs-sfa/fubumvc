using FubuCore;
using FubuCore.Binding;
using FubuCore.Configuration;
using FubuCore.Conversion;
using FubuCore.Dates;
using FubuCore.Formatting;
using FubuCore.Logging;
using FubuCore.Reflection;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Diagnostics.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.Cookies;
using FubuMVC.Core.Json;
using FubuMVC.Core.Registration.Querying;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Runtime.Conditionals;
using FubuMVC.Core.Runtime.SessionState;
using FubuMVC.Core.Urls;

namespace FubuMVC.Core.Registration
{
    /// <summary>
    ///     The core runtime service registry for a FubuMVC application
    /// </summary>
    public class CoreServiceRegistry : ServiceRegistry
    {
        public CoreServiceRegistry()
        {

            SetServiceIfNone<IRequestData, FubuMvcRequestData>();

            For<AppReloaded>().Use(new AppReloaded());

            SetServiceIfNone<IDiagnosticAssets, DiagnosticAssetsCache>().Singleton();

            // HAck alert. Inconsistent usage in the app, so you're stuck with this.
            var stringifier = new Stringifier();
            For<Stringifier>().Use(stringifier);
            For<IStringifier>().Use(stringifier);

            AddService(new TypeDescriptorCache());

            SetServiceIfNone<IConditionalService, ConditionalService>();
            SetServiceIfNone<IOutputWriter, OutputWriter>();

            SetServiceIfNone<IUrlRegistry, UrlRegistry>();
            SetServiceIfNone<IChainUrlResolver, ChainUrlResolver>();

            SetServiceIfNone<IFlash, FlashProvider>();
            SetServiceIfNone<IRequestDataProvider, RequestDataProvider>();

            SetServiceIfNone<IFubuRequest, FubuRequest>();
            SetServiceIfNone<IContinuationProcessor, ContinuationProcessor>();

            SetServiceIfNone<IDisplayFormatter, DisplayFormatter>();
            SetServiceIfNone<IChainResolver, ChainResolutionCache>().Singleton();



            SetServiceIfNone<ITypeDescriptorCache, TypeDescriptorCache>().Singleton();

            SetServiceIfNone<ISessionState, SimpleSessionState>();


            SetServiceIfNone<IFubuRequestContext, FubuRequestContext>();
            SetServiceIfNone<IFileSystem, FileSystem>();


            SetServiceIfNone<IObjectConverter, ObjectConverter>();

            SetServiceIfNone<IResourceNotFoundHandler, DefaultResourceNotFoundHandler>();


            SetServiceIfNone<ILogger, Logger>();
            AddService<ILogModifier, LogRecordModifier>();

            SetServiceIfNone<IClock, Clock>().Singleton();
            SetServiceIfNone<ITimeZoneContext, MachineTimeZoneContext>();
            SetServiceIfNone<ISystemTime, SystemTime>();

            SetServiceIfNone<IExceptionHandlingObserver, ExceptionHandlingObserver>();


            SetServiceIfNone<ICookies, Cookies>();


            SetServiceIfNone<ISettingsProvider, SettingsProvider>();
            AddService<ISettingsSource>(new AppSettingsSettingSource(SettingCategory.environment));


            SetServiceIfNone<IJsonSerializer, NewtonSoftJsonSerializer>();

        }
    }
}