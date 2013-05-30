using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;
using StructureMap.Configuration.DSL;

namespace FubuValidation.StructureMap
{
	// SAMPLE: ValidationBootsrapping
    public class FubuValidationRegistry : Registry
    {
        public FubuValidationRegistry()
        {
            For<ITypeResolver>().Use<TypeResolver>();
			For<IServiceLocator>().Use<StructureMapServiceLocator>();

			For<IFieldValidationQuery>().Use<FieldValidationQuery>();
			For<IValidator>().Use<Validator>();

            ForSingletonOf<ITypeDescriptorCache>().Use<TypeDescriptorCache>();
            ForSingletonOf<ValidationGraph>();
            ForSingletonOf<IFieldRulesRegistry>().Use<FieldRulesRegistry>();
            
        }
    }
	// ENDSAMPLE
}