using System.Collections.Generic;
using FubuMVC.Core;
using NUnit.Framework;
using Shouldly;

namespace FubuMVC.Tests
{
    [TestFixture]
    public class AjaxExtensionsTester
    {
        private readonly IDictionary<string, object> _ajaxRequestInput = new Dictionary<string, object>
        {
            {"X-Requested-With", "XMLHttpRequest"}
        };

        private readonly IDictionary<string, object> _nonAjaxRequestInput = new Dictionary<string, object>
        {
            {"X-Requested-With", "some_value"}
        };

        [Test]
        public void is_dictionary_input_an_ajax_request()
        {
            _ajaxRequestInput.IsAjaxRequest().ShouldBeTrue();
            _nonAjaxRequestInput.IsAjaxRequest().ShouldBeFalse();
        }
    }
}