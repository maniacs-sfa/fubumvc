using System;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Tests.Registration.Conventions;
using Shouldly;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace FubuMVC.Tests.Registration.DSL
{

    [TestFixture]
    public class when_adding_an_iconfigurationaction_without_specifying_configuration_type
    {
        [Test]
        public void it_should_default_to_policy()
        {
            FakePolicy.Count = 0;
            BehaviorGraph.BuildFrom(graph => 
                graph.Policies.Local.Add(new FakePolicy()));

            FakePolicy.Count.ShouldBe(1);
        }
    }

    [TestFixture]
    public class when_adding_an_action_of_policy_without_specifying_configuration_type
    {
        [Test]
        public void it_should_default_to_policy()
        {
            FakePolicy.Count = 0;
            BehaviorGraph.BuildFrom(graph => 
                graph.Policies.Local.Add<FakePolicy>(x => x.NoOp()));

            FakePolicy.Count.ShouldBe(1);
        }
    }

    public class OrderingPolicyController
    {
        [WrapWith(typeof(OPWrapper2), typeof(OPWrapper3))]
        public void M1(){}
        public void M2(){}
        public void M3(){}
        public void M4(){}
        public void M5(){}
    }

    public class OPWrapper1 : IActionBehavior
    {
        public Task Invoke()
        {
            throw new NotImplementedException();
        }

        public Task InvokePartial()
        {
            throw new NotImplementedException();
        }
    }

    public class OPWrapper2 : IActionBehavior
    {
        public Task Invoke()
        {
            throw new NotImplementedException();
        }

        public Task InvokePartial()
        {
            throw new NotImplementedException();
        }
    }

    public class OPWrapper3 : IActionBehavior
    {
        public Task Invoke()
        {
            throw new NotImplementedException();
        }

        public Task InvokePartial()
        {
            throw new NotImplementedException();
        }
    }

    public class FakePolicy : IConfigurationAction
    {
        public static int Count;

        public void Configure(BehaviorGraph graph)
        {
            Count++;
        }

        public void NoOp() { }
    }
}