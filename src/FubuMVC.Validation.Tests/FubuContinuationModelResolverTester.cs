using System;
using FubuMVC.Core.Registration.Nodes;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class FubuContinuationModelResolverTester : InteractionContext<FubuContinuationModelResolver>
    {
        [Test]
        public void should_resolve_model_through_input_model_resolver()
        {
            var context = new ValidationFailure(ActionCall.For<SampleInputModel>(m => m.Test("Hello")),
                                                       Notification.Valid(), "Hello");
            var modelType = typeof (Guid);
            MockFor<IFubuContinuationModelDescriptor>()
                .Expect(d => d.DescribeModelFor(context))
                .Return(modelType);

            var model = new object();
            MockFor<IInputModelResolver>()
                .Expect(r => r.Resolve(modelType, typeof (string), "Hello"))
                .Return(model);

            ClassUnderTest
                .ModelFor(context)
                .ShouldEqual(model);
        }
    }
}