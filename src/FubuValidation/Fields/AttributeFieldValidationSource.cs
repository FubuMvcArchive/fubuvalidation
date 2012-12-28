using System;
using System.Collections.Generic;
using System.Reflection;
using FubuCore.Reflection;
using System.Linq;

namespace FubuValidation.Fields
{
    public class AttributeFieldValidationSource : IFieldValidationSource
    {
        public IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            return property.GetAllAttributes<FieldValidationAttribute>()
                .SelectMany(x => x.RulesFor(property));
        }

        public void AssertIsValid()
        {
            
        }

        public override bool Equals(object obj)
        {
            return obj is AttributeFieldValidationSource;
        }
    }
}