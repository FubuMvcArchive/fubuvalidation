using System.Collections.Generic;
using System.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation.UI
{
    public class ValidationSummarySource : IActionSource
    {
        public class ValidationSummaryController
        {
            public ValidationSummary Summary(ValidationSummary summary)
            {
                return new ValidationSummary();
            }
        }

        public IEnumerable<ActionCall> FindActions(Assembly applicationAssembly)
        {
            yield return ActionCall.For<ValidationSummaryController>(x => x.Summary(null));
        }
    }
}