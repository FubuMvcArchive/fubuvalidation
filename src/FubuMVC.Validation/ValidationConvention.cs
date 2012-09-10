using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Validation
{
    public class ValidationConvention : IConfigurationAction
    {
        private readonly Func<ActionCall, bool> _predicate;

        public ValidationConvention(Func<ActionCall, bool> predicate)
        {
            _predicate = predicate;
        }


        public void Configure(BehaviorGraph graph)
        {
            graph
                .Actions()
                .Where(call => _predicate(call))
                .Each(call =>
                          {
                              call.WrapWithValidation();
                          });
        }
    }
}