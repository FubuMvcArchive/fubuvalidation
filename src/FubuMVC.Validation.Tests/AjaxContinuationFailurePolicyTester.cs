using FubuMVC.Core.Ajax;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class AjaxContinuationFailurePolicyTester : InteractionContext<AjaxContinuationFailurePolicy>
    {
        [Test]
        public void should_match_ajax_continuations()
        {
            matchesOuputType<AjaxContinuation>()
                .ShouldBeTrue();
        }

        [Test]
        public void should_match_subclasses_of_ajax_continuations()
        {
            matchesOuputType<ChildContinuation>()
                .ShouldBeTrue();
        }

        [Test]
        public void should_not_match_non_ajax_continuations()
        {
            matchesOuputType<string>()
                .ShouldBeFalse();
        }

        [Test]
        public void should_write_ajax_continuation_to_json_writer()
        {
            var continuation = new AjaxContinuation();
            var failure = failureFor<object>();

            MockFor<IAjaxContinuationResolver>()
                .Expect(r => r.Resolve(failure.Notification))
                .Return(continuation);

            MockFor<IJsonWriter>()
                .Expect(w => w.Write(continuation));

            ClassUnderTest
                .Handle(failure);

            VerifyCallsFor<IJsonWriter>();
        }

        private bool matchesOuputType<T>()
            where T : class 
        {
            return ClassUnderTest
                .Matches(failureFor<T>());
        }

        private ValidationFailure failureFor<T>()
            where T : class
        {
            var call = ActionCall.For<in_and_out<T>>(x => x.go(null));
            var notification = new Notification();
            return new ValidationFailure(call, notification, new object());
        }

        public class in_and_out<T>
        {
            public T go(T value)
            {
                return value;
            }
        }

        public class ChildContinuation : AjaxContinuation {}
    }
}