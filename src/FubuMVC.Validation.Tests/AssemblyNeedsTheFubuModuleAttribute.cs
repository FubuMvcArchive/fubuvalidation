using System.Linq;
using FubuMVC.Core;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AssemblyNeedsTheFubuModuleAttribute
    {
        [Test]
        public void the_attribute_exists()
        {
            var assembly = typeof(FubuMvcValidation).Assembly;

            assembly.GetCustomAttributes(typeof(FubuModuleAttribute), true)
                .Any().ShouldBeTrue();
        }
    }
}