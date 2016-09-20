using System.Linq;
using FubuCore;
using FubuMVC.Core.Runtime;

namespace FubuMVC.Core.Resources.Conneg
{
    public static class OutputNodeExtensions
    {
        public static bool Writes(this IMediaWriter media, MimeType mimeType)
        {
            return media.Mimetypes.Contains(mimeType.Value);
        }

    }
}