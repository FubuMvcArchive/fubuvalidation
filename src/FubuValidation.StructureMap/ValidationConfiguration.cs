using System;
using System.Collections.Generic;
using System.Reflection;
using FubuCore;
using StructureMap;

namespace FubuValidation.StructureMap
{
    public class ValidationConfiguration
    {
        public static void Bootstrap(Action<ValidationConfigurationExpression> configure)
        {
            var expression = new ValidationConfigurationExpression();
            configure(expression);

            var container = expression.As<IValidationConfigurator>().BakeIntoContainer();

            runRegistrations(container);
        }

        private static void runRegistrations(IContainer container)
        {
            container.GetInstance<ValidationRegistrationRunner>().Run();
        }

        public interface IValidationConfigurator
        {
            IContainer BakeIntoContainer();
        }

        public class ValidationConfigurationExpression : IValidationConfigurator
        {
            private IContainer _container;
            private readonly IList<Assembly> _assemblies = new List<Assembly>();

            public void StructureMap(IContainer container)
            {
                _container = container;
            }

            public void StructureMapObjectFactory()
            {
                StructureMap(ObjectFactory.Container);
            }

            public ValidationRegistrationScanExpression Registration
            {
                get { return new ValidationRegistrationScanExpression(_assemblies); }
            }


            IContainer IValidationConfigurator.BakeIntoContainer()
            {
                if (_container == null)
                {
                    throw new InvalidOperationException("You must specify a container to configure");
                }

                _container.Configure(x =>
                {
                    x.AddRegistry<FubuValidationRegistry>();

                    x.Scan(scan =>
                    {
                        _assemblies.Each(scan.Assembly);
                        //scan.Include(t => _registrationTypes.Contains(t));
                        scan.AddAllTypesOf<IValidationRegistration>();
                    });
                });

                return _container;
            }
        }

        public class ValidationRegistrationScanExpression
        {
            private readonly IList<Assembly> _assemblies;

            public ValidationRegistrationScanExpression(IList<Assembly> assemblies)
            {
                _assemblies = assemblies;
            }

            public void AddFromAssemblyContaining<T>()
            {
                _assemblies.Add(typeof (T).Assembly);
            }

            public void AddAllAvailable()
            {
                _assemblies.Fill(AppDomain.CurrentDomain.GetAssemblies());
            }
        }
    }
}