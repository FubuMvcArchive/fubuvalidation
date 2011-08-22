using System;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class LambdaFubuContinuationModelResolverTester
    {
        private Guid _result;
        private LambdaFubuContinuationModelResolver _resolver;

        [SetUp]
        public void before_each()
        {
            _result = Guid.NewGuid();
            _resolver = new LambdaFubuContinuationModelResolver(c => _result);
        }

        [Test]
        public void should_resolve_with_converter()
        {
            _resolver
                .ModelFor(null)
                .ShouldEqual(_result);
        }
    }
}