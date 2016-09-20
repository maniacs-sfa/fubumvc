using System.Linq;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Resources.Conneg;
using FubuMVC.Core.Runtime;
using HtmlTags;
using NUnit.Framework;
using Shouldly;

namespace FubuMVC.Tests.Registration.Conventions
{
    [TestFixture]
    public class HtmlTagAndDocumentHandlerConventionTester
    {
        [SetUp]
        public void SetUp()
        {
            graph = BehaviorGraph.BuildFrom(x => { x.Actions.IncludeType<TagController>(); });
        }

        private BehaviorGraph graph;

        [Test]
        public void action_that_returns_HtmlDocument_should_output_to_html()
        {
            var outputNode =
                graph.ChainFor<TagController>(x => x.BuildDoc()).Outputs.First().ShouldBeOfType<OutputNode>();
            outputNode
                .ResourceType.ShouldBe(typeof(HtmlDocument));

            outputNode.Writes(MimeType.Html).ShouldBeTrue();
        }

        [Test]
        public void action_that_returns_HtmlTag_should_output_to_html()
        {
            var outputNode =
                graph.ChainFor<TagController>(x => x.BuildTag()).Outputs.First().ShouldBeOfType<OutputNode>();
            outputNode.Writes(MimeType.Html).ShouldBeTrue();
            outputNode.ResourceType.ShouldBe(typeof(HtmlTag));
        }
    }

    public class TagController
    {
        public string GoSomewhere()
        {
            return null;
        }

        public HtmlTag BuildTag()
        {
            return null;
        }

        public HtmlDocument BuildDoc()
        {
            return null;
        }
    }
}