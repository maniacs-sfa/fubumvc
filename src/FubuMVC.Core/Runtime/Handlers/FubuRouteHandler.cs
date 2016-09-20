using System;
using System.Web;
using System.Web.Routing;

namespace FubuMVC.Core.Runtime.Handlers
{
    public class FubuRouteHandler : IFubuRouteHandler
    {
        private readonly IBehaviorInvoker _invoker;
        private readonly IHttpHandlerSource _handlerSource;

        public FubuRouteHandler(IBehaviorInvoker invoker, IHttpHandlerSource handlerSource)
        {
            _invoker = invoker;
            _handlerSource = handlerSource;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            throw new NotSupportedException();
        }

        public IBehaviorInvoker Invoker
        {
            get { return _invoker; }
        }
    }
}