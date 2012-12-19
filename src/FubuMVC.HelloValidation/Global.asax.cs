using System;
using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using StructureMap.Configuration.DSL;

namespace FubuMVC.HelloValidation
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            new HelloValidationApplication()
                .BuildApplication()
                .Bootstrap();

            PackageRegistry.AssertNoFailures();
        }
    }

    public class HelloValidationApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            return FubuApplication
                .For<HelloValidationFubuRegistry>()
                .StructureMapObjectFactory(configure => configure.AddRegistry<HelloValidationRegistry>());
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
        }
    }
}