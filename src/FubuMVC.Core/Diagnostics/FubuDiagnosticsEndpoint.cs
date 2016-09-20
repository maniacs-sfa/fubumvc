using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FubuMVC.Core.Diagnostics.Assets;
using FubuMVC.Core.Http;
using FubuMVC.Core.Registration;
using HtmlTags;

namespace FubuMVC.Core.Diagnostics
{
    [Tag("Diagnostics")]
    public class FubuDiagnosticsEndpoint
    {
        private static readonly string[] _styles = {"bootstrap.min.css", "master.css", "bootstrap.overrides.css"};
        private static readonly string[] _scripts = {"jquery.min.js", "typeahead.bundle.min.js", "root.js"};
        private readonly IDiagnosticAssets _assets;
        private readonly IHttpRequest _request;
        private readonly IHttpResponse _response;
        private readonly FubuRuntime _runtime;

        public FubuDiagnosticsEndpoint(IHttpResponse response, IDiagnosticAssets assets, IHttpRequest request, FubuRuntime runtime)
        {
            _response = response;
            _assets = assets;
            _request = request;
            _runtime = runtime;
        }

        public HtmlDocument get__fubu()
        {
            var document = new HtmlDocument
            {
                Title = "FubuMVC Diagnostics"
            };

            writeStyles(document);

            var div = new HtmlTag("div");
            div.Id("diagnostics");
            var foot = new HtmlTag("foot");
            document.Body.Children.Add(div);
            document.Body.Children.Add(foot);
            writeScripts(foot);
            return document;
        }


        private void writeScripts(HtmlTag foot)
        {
            // Do this regardless
            foot.Append(_assets.For("FubuDiagnostics.js").ToEmbeddedScriptTag());

            throw new NotImplementedException("Need to replace the code below somehow");
//            var routeData = _routeWriter.Write("FubuDiagnostics.routes", _routes);
//            foot.Append(routeData);

            var extensionFiles = _assets.JavascriptFiles().Where(x => x.AssemblyName != "FubuMVC.Core");



//            if (_runtime.Mode.InDiagnostics())
//            {
//                var names =
//                    _scripts.Union(extensionFiles.Select(x => x.Name.Split('.').Reverse().Take(2).Reverse().Join(".")));
//                var links = _tags.BuildScriptTags(names.Select(x => "fubu-diagnostics/" + x));
//                links.Each(x => foot.Append(x));
//            }
//            _scripts.Each(name =>
//            {
//                var file = _assets.For(name);
//                foot.Append(file.ToEmbeddedScriptTag());
//            });

            extensionFiles.Each(file => foot.Append(file.ToEmbeddedScriptTag()));
        }

        private void writeStyles(HtmlDocument document)
        {
            _styles.Each(name =>
            {
                var file = _assets.For(name);
                document.Head.Append(file.ToEmbeddedStyleTag());
            });
        }

        [NoDiagnostics]
        public void get__fubu_asset_Version_Name(DiagnosticAssetRequest request)
        {
            var file = _assets.For(request.Name);
            if (file == null)
            {
                _response.StatusCode = (int) HttpStatusCode.NotFound;
                return;
            }

            file.Write(_response);
        }

        [NoDiagnostics]
        public void get__fubu_icon(Standin standin)
        {
            var file = _assets.For("fubumvc.png");
            file.Write(_response);
        }
    }

    public class Standin
    {
    }

    public class DiagnosticAssetRequest
    {
        public string Version { get; set; }
        public string Name { get; set; }
    }

//    public class DiagnosticJavascriptRoutes : JavascriptRouter
//    {
//        public DiagnosticJavascriptRoutes(BehaviorGraph graph)
//        {
//            graph.Chains.OfType<DiagnosticChain>().Where(x => x.Route.AllowedHttpMethods.Any()).Each(Add);
//
//            Get("icon").Action<FubuDiagnosticsEndpoint>(x => x.get__fubu_icon(null));
//        }
//    }
}