using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;
using StructureMap.Configuration.DSL;

namespace FubuValidation.StructureMap
{
    public class FubuValidationRegistry : Registry
    {
        public FubuValidationRegistry()
        {
            For<ITypeResolver>().Use<TypeResolver>();
            For<IValidator>().Use<Validator>();
            For<IValidationSource>().Add<FieldRuleSource>();

            ForSingletonOf<ITypeDescriptorCache>().Use<TypeDescriptorCache>();

            ForSingletonOf<ValidationGraph>();
            ForSingletonOf<IFieldRulesRegistry>().Add<FieldRulesRegistry>();
            ForSingletonOf<IFieldValidationQuery>().Use<FieldValidationQuery>();

            For<IFieldValidationSource>().Add<AttributeFieldValidationSource>();
        }
    }
}