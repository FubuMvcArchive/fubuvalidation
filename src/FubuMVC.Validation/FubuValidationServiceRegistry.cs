using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Validation.Remote;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation
{
    public class FubuValidationServiceRegistry : ServiceRegistry
    {
        public FubuValidationServiceRegistry()
        {
            SetServiceIfNone<ITypeResolver, TypeResolver>();
            SetServiceIfNone<IValidator, Validator>();

            AddService<IFieldValidationSource, AccessorRulesFieldSource>();

            setSingleton<ValidationGraph, ValidationGraph>();
            setSingleton<IFieldRulesRegistry, FieldRulesRegistry>();
            setSingleton<RemoteRuleGraph, RemoteRuleGraph>();
        }

        private void setSingleton<TPlugin, TConcrete>()
        {
            var def = SetServiceIfNone(typeof(TPlugin), typeof(TConcrete));
            def.IsSingleton = true;
        }
    }
}