using FubuCore.Reflection;
using FubuMVC.Core.Ajax;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;
using Rhino.Mocks;

namespace FubuMVC.Validation.Tests.Remote
{
    [TestFixture]
    public class ValidateFieldEndpointTester : InteractionContext<ValidateFieldEndpoint>
    {
        private RemoteRuleGraph theGraph;
        private AjaxContinuation theContinuation;
        private Notification theNotification;
        private RemoteFieldRule theRemoteRule;
        private ValidateField theInputModel;

        protected override void beforeEach()
        {
            var theAccessor = ReflectionHelper.GetAccessor<ValidateFieldTarget>(x => x.Name);
            
            theGraph = new RemoteRuleGraph();
            theGraph.RegisterRule(theAccessor, new RequiredFieldRule());

            Services.Inject(theGraph);

            theRemoteRule = RemoteFieldRule.For(theAccessor, new RequiredFieldRule());

            theInputModel = new ValidateField {Hash = theRemoteRule.ToHash(), Value = "Test"};

            theNotification = new Notification();
            theContinuation = new AjaxContinuation();

            MockFor<IRuleRunner>().Stub(x => x.Run(theRemoteRule, theInputModel.Value)).Return(theNotification);
            MockFor<IAjaxContinuationResolver>().Stub(x => x.Resolve(theNotification)).Return(theContinuation);
        }

        [Test]
        public void builds_the_continuation()
        {
            ClassUnderTest.Validate(theInputModel).ShouldBeTheSameAs(theContinuation);
        }

        public class ValidateFieldTarget
        {
            public string Name { get; set; }
        }
    }
}