using FubuMVC.Core.Continuations;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ConfiguredFubuContinuationResolverTester
    {
        [Test]
        public void should_resolve_to_configured_instance()
        {
            var continuation = FubuContinuation.TransferTo(new object());
            new ConfiguredFubuContinuationResolver(continuation)
                .Resolve(null)
                .ShouldEqual(continuation);
        }
    }
}