using FluentValidation;
using FubuCore;
using FubuTestingSupport;
using NUnit.Framework;
using StructureMap;

namespace FubuValidation.FluentValidation.Tests
{
    [TestFixture]
    public class EndToEndSmokeTests
    {
        [Test]
        public void should_validate_through_fluent_validation_source()
        {
            var container = new Container(configure =>
                                              {
                                                  configure
                                                      .Scan(x =>
                                                                {
                                                                    x.TheCallingAssembly();
                                                                    x.AddAllTypesOf<global::FluentValidation.IValidator>();
                                                                    x.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                                                                });

                                                  configure
                                                      .For<IValidationSource>()
                                                      .Add<FluentValidationSource>();

                                                  configure
                                                      .Scan(x =>
                                                                {
                                                                    x.AssemblyContainingType<INotificationFiller>();
                                                                    x.WithDefaultConventions();
                                                                });

                                                  configure
                                                      .For<ITypeResolver>()
                                                      .Use<TypeResolver>();

                                                  configure
                                                      .For<IValidator>()
                                                      .Use<Validator>();
                                              });

            var model = new SampleFluentModel();
            container
                .GetInstance<IValidator>()
                .Validate(model)
                .AllMessages
                .ShouldHaveCount(1);
        }
    }
}