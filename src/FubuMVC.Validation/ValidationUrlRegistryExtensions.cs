using FubuMVC.Core.Urls;
using FubuMVC.Validation.Remote;

namespace FubuMVC.Validation
{
    public static class ValidationUrlRegistryExtensions
    {
        public static string RemoteRule(this IUrlRegistry urls)
        {
            return urls.UrlFor(new ValidateField());
        }
    }
}