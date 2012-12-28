using System.Collections.Generic;
using FubuValidation.Fields;

namespace FubuValidation
{
    public interface IValidationRegistration
    {
        void RegisterFieldRules(IFieldRulesRegistry registry);
        IEnumerable<IFieldValidationSource> FieldSources();
    }
}