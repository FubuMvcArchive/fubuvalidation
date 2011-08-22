using System;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class FubuRequestInputModelResolverTester : InteractionContext<FubuRequestInputModelResolver>
    {
        [Test]
        public void should_resolve_model_through_request()
        {
            var model = Guid.NewGuid();
            var destinationType = typeof (Guid);
            MockFor<IFubuRequest>()
                .Expect(r => r.Get(destinationType))
                .Return(model);

            ClassUnderTest
                .Resolve(destinationType, typeof (string), Guid.NewGuid().ToString());
        }
    }
}