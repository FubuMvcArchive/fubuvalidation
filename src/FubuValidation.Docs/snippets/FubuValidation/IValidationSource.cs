using System;
using System.Collections.Generic;

namespace FubuValidation
{
	// SAMPLE: IValidationSource
    public interface IValidationSource
    {
        IEnumerable<IValidationRule> RulesFor(Type type);
    }
	// ENDSAMPLE
}