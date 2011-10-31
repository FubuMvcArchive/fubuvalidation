using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;
using StructureMap.Configuration.DSL;

namespace FubuValidation.StructureMap
{
    public static class ValidationBootstrapExtensions
    {
        public static void FubuValidation(this Registry registry)
        {
            registry.ForSingletonOf<ITypeDescriptorCache>().Use<TypeDescriptorCache>();
            registry.For<ITypeResolver>().Use<TypeResolver>();
            registry.For<IValidator>().Use<Validator>();
            registry.ForSingletonOf<IFieldRulesRegistry>().Add<FieldRulesRegistry>();
            registry.For<IValidationSource>().Add<FieldRuleSource>();
            registry.For<IValidationQuery>().Use<ValidationQuery>();
            registry.For<IFieldValidationQuery>().Use<FieldValidationQuery>();

            registry.For<IFieldValidationSource>().Add<AttributeFieldValidationSource>();
        }
    }
}