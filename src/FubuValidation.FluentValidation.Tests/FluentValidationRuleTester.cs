using FluentValidation.Results;
using FubuTestingSupport;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuValidation.FluentValidation.Tests
{
    [TestFixture]
    public class FluentValidationRuleTester : InteractionContext<FluentValidationRule>
    {
        [Test]
        public void should_transform_results_to_notification()
        {
            var validator = new SampleValidator();
            var model = new SampleFluentModel();
            var context = new ValidationContext(null, new Notification(typeof (SampleFluentModel)), model);

            Container.Configure(x => x.For<global::FluentValidation.IValidator>().Add(validator));

            MockFor<INotificationFiller>()
                .Expect(f => f.Fill(Arg<Notification>.Is.Same(context.Notification), Arg<ValidationResult>.Is.NotNull));

            ClassUnderTest.Validate(context);

            VerifyCallsFor<INotificationFiller>();
        }
    }
}