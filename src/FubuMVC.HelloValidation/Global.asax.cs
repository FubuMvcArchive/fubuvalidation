using System;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuValidation;
using FubuValidation.FluentValidation;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace FubuMVC.HelloValidation
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            ObjectFactory
                .Initialize(x => x.AddRegistry<HelloValidationStructureMapRegistry>());

            FubuApplication
                .For<HelloValidationFubuRegistry>()
                .StructureMapObjectFactory()
                .Bootstrap(RouteTable.Routes);
        }
    }

    public class HelloValidationStructureMapRegistry : Registry
    {
        public HelloValidationStructureMapRegistry()
        {
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.WithDefaultConventions();
                         x.AddAllTypesOf<FluentValidation.IValidator>();
                     });

            Scan(x =>
                     {
                         x.AssemblyContainingType<INotificationFiller>();
                         x.WithDefaultConventions();
                     });

            For<IValidationSource>()
                .Add<FluentValidationSource>();
        }
    }
}