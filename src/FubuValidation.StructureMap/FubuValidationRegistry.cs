using System;
using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace FubuValidation.StructureMap
{
	// SAMPLE: ValidationBootsrapping
    public class FubuValidationRegistry : Registry
    {
        public FubuValidationRegistry()
        {
            For<ITypeResolver>().Use<TypeResolver>();
            For<IValidator>().Use<Validator>();
            For<IValidationSource>().Add<FieldRuleSource>();

	        For<IServiceLocator>().Use<StructureMapServiceLocator>();

            ForSingletonOf<ITypeDescriptorCache>().Use<TypeDescriptorCache>();

            ForSingletonOf<ValidationGraph>();
            ForSingletonOf<IFieldRulesRegistry>().Use<FieldRulesRegistry>();
            ForSingletonOf<IFieldValidationQuery>().Use<FieldValidationQuery>();
        }
    }
	// ENDSAMPLE

	public class StructureMapServiceLocator : IServiceLocator
	{
		private readonly IContainer _container;

		public StructureMapServiceLocator(IContainer container)
		{
			_container = container;
		}

		public object GetInstance(Type type)
		{
			return _container.GetInstance(type);
		}

		public IContainer Container { get { return _container; } }


		public TService GetInstance<TService>()
		{
			return _container.GetInstance<TService>();
		}

		public TService GetInstance<TService>(string name)
		{
			return _container.GetInstance<TService>(name);
		}
	}
}