using System.Linq;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.Conneg;
using NUnit.Framework;
using Shouldly;

namespace FubuMVC.Tests.Resources.Conneg
{
    [TestFixture]
    public class InputOutputNodePlacementIntegrationTester
    {
        private BehaviorGraph theGraph;

        [SetUp]
        public void SetUp()
        {
            var registry = new FubuRegistry();
            registry.Actions.IncludeType<PlacementController>();

            theGraph = BehaviorGraph.BuildFrom(registry);
        }


        [Test]
        public void output_is_last()
        {
            theGraph.ChainFor<PlacementController>(x => x.post_payload(null))
                .Last().ShouldBeOfType<OutputNode>();
        }
    }

    public class PlacementController
    {
        public string SayHello()
        {
            return "hello";
        }

        public PlacementPayload post_payload(PlacementPayload payload)
        {
            return payload;
        } 
    }

    public class PlacementPayload
    {
        
    }
}