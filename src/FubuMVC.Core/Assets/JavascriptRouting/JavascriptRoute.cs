using System;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Querying;

namespace FubuMVC.Core.Assets.JavascriptRouting
{
    public class JavascriptRoute
    {
        public Func<IChainResolver, BehaviorChain> Finder;
        public string Method;
        public string Name;

        public RoutedChain FindChain(IChainResolver resolver)
        {
            var chain = Finder(resolver) as RoutedChain;

            if (chain == null)
                throw new Exception("Unable to find a routed chain for a Javascript route named " + Name);
            return chain;
        }
    }
}