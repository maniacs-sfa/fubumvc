using FubuMVC.Core.Runtime;
using OpenQA.Selenium;

namespace Serenity
{
    public static class WebElementExtensions
    {
        public static bool IsCssLink(this IWebElement element)
        {
            return (element.TagName == "link") &&
                   (element.GetMimeType() == MimeType.Css);
        }

        public static string Href(this IWebElement element)
        {
            return element.GetAttribute("href");
        }


        public static MimeType GetMimeType(this IWebElement element)
        {
            return MimeType.MimeTypeByFileName(element.Href());
        }

        public static bool IsHiddenInput(this IWebElement element)
        {
            return (element.TagName == "input") && (element.GetAttribute("type") == "hidden");
        }
    }
}