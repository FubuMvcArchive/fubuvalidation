using System;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class when_resolving_the_fubu_continuation : InteractionContext<FubuContinuationResolver>
    {
        [Test]
        public void should_build_through_func()
        {
            var model = Guid.NewGuid();
            var continuation = FubuContinuation.TransferTo(model);
            Func<object, FubuContinuation> builder = o => continuation;
            Container
                .Configure(x => x.For<Func<object, FubuContinuation>>().Use(builder));

            var failure = new ValidationFailure(ActionCall.For<SampleInputModel>(m => m.Test("Hello")),
                                                Notification.Valid(), "Hello");

            MockFor<IFubuContinuationModelResolver>()
                .Expect(r => r.ModelFor(failure))
                .Return(model);

            ClassUnderTest
                .Resolve(failure)
                .ShouldEqual(continuation);

            VerifyCallsFor<IFubuContinuationModelResolver>();
        }
    }
}