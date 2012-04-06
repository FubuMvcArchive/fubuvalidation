using System;
using System.Collections.Generic;
using FubuCore.Binding;
using FubuCore.Reflection;
using FubuMVC.Core.Runtime;
using FubuTestingSupport;
using FubuValidation;
using NUnit.Framework;
using Rhino.Mocks;
using System.Linq;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ModelBindingErrorsTester : InteractionContext<ModelBindingErrors>
    {
        [Test]
        public void should_place_problems_into_the_notification()
        {
            MockFor<IFubuRequest>().Stub(x => x.ProblemsFor<ValidationTarget>()).Return(problems());

            var notification = new Notification();

            ClassUnderTest.AddAnyErrors<ValidationTarget>(notification);

            notification.MessagesFor<ValidationTarget>(x => x.Number).Single().StringToken.ShouldEqual(ValidationKeys.INVALID_FORMAT);
            notification.MessagesFor<ValidationTarget>(x => x.Time).Single().StringToken.ShouldEqual(ValidationKeys.INVALID_FORMAT);
            
            notification.MessagesFor<ValidationTarget>(x => x.Other)
                .Any().ShouldBeFalse();
        
        }

        private IEnumerable<ConvertProblem> problems()
        {
            yield return new ConvertProblem{Property = ReflectionHelper.GetProperty<ValidationTarget>(x => x.Number)};
            yield return new ConvertProblem{Property = ReflectionHelper.GetProperty<ValidationTarget>(x => x.Time)};
        }

        public class ValidationTarget
        {
            public int Number { get; set; }
            public DateTime Time { get; set; }
            public int Other { get; set; }
        }
    }
}