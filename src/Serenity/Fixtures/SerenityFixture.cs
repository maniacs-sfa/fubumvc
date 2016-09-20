using System;
using FubuCore;
using FubuCore.Dates;
using FubuMVC.Core;
using StoryTeller;

namespace Serenity.Fixtures
{
    public class SerenityFixture : Fixture
    {

        protected FubuRuntime Runtime
        {
            get
            {
                if (Context == null) throw new InvalidOperationException("The Runtime is only available during specification execution");

                return Retrieve<FubuRuntime>();
            }
        }

        protected void ResetTheClock()
        {
            Runtime.Get<IClock>().As<Clock>().Live();
        }


        protected void AdvanceTheClock(TimeSpan timespan)
        {
            var clock = Runtime.Get<IClock>().As<Clock>();
            var time = clock.UtcNow().Add(timespan).ToLocalTime();

            clock.LocalNow(time);
        }
    }
}