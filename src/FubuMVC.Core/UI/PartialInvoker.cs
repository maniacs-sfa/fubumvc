﻿using System;
using FubuCore;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;

namespace FubuMVC.Core.UI
{
    public class PartialInvoker : IPartialInvoker
    {
        private readonly IPartialFactory _factory;
        private readonly IFubuRequest _request;
        private readonly IAuthorizationPreviewService _authorization;
        private readonly ITypeResolver _types;
        private readonly IOutputWriter _writer;

        public PartialInvoker(IPartialFactory factory, IFubuRequest request, IAuthorizationPreviewService authorization,
                              ITypeResolver types, IOutputWriter writer)
        {
            _factory = factory;
            _request = request;
            _authorization = authorization;
            _types = types;
            _writer = writer;
        }

        public string Invoke<T>() where T : class
        {
            var output = string.Empty;
            var input = _request.Get<T>();
            if (_authorization.IsAuthorized(input))
            {
                output = invokeWrapped(typeof (T));
            }
            return output;
        }

        public string InvokeObject(object model)
        {
            var output = string.Empty;
            if (_authorization.IsAuthorized(model))
            {
                var requestType = _types.ResolveType(model);
                _request.Set(requestType, model);
                output = invokeWrapped(requestType);
            }
            return output;
        }

        private string invokeWrapped(Type requestType)
        {
            var partial = _factory.BuildPartial(requestType);
            return _writer.Record(partial.InvokePartial).GetText();
        }
    }
}