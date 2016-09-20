using FubuMVC.Core.ServiceBus;
using FubuMVC.Core.ServiceBus.Configuration;
using NUnit.Framework;
using Shouldly;

namespace FubuMVC.Tests.ServiceBus
{
    [TestFixture]
    public class can_establish_a_control_queue
    {
        public class ControlQueueRegistry : FubuTransportRegistry<BusSettings>
        {
            public ControlQueueRegistry()
            {
                AlterSettings<BusSettings>(x =>
                {
                    x.Downstream = "memory://1".ToUri();
                    x.Upstream = "memory://2".ToUri();
                });

                Channel(x => x.Downstream).ReadIncoming();
                Channel(x => x.Upstream).UseAsControlChannel();

                ServiceBus.EnableInMemoryTransport();


                Mode = "Testing";
            }
        }

        [Test]
        public void configure_a_control_queue()
        {
            var registry = new ControlQueueRegistry();

            using (var runtime = registry.ToRuntime())
            {
                var graph = runtime.Get<ChannelGraph>();

                graph.ControlChannel.Uri.ShouldBe("memory://2".ToUri());

                graph.ChannelFor<BusSettings>(x => x.Upstream)
                    .Incoming.ShouldBeTrue();
            }
        }
    }
}