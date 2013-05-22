using System.Collections.Generic;
using System.Reflection;
using FubuCore;
using FubuValidation.Fields;

namespace FubuMVC.Validation
{
    public class ListFieldValidationSource : IFieldValidationSource
    {
        public IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            if (property.PropertyType.Closes(typeof(IList<>)))
            {
                yield return new ListValidationRule();
            }
        }

        public void AssertIsValid()
        {

        }
    }
}