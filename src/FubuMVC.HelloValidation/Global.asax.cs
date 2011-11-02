using System;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuValidation.StructureMap;
using StructureMap.Configuration.DSL;

namespace FubuMVC.HelloValidation
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication
                .For<HelloValidationFubuRegistry>()
                .StructureMapObjectFactory(configure => configure.AddRegistry<HelloValidationRegistry>())
                .Bootstrap();
        }
    }

    public class HelloValidationRegistry : Registry
    {
        public HelloValidationRegistry()
        {
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.WithDefaultConventions();
                     });

            this.FubuValidation();
        }
    }
}