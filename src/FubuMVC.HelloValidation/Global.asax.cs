using System;
using System.Web.Routing;
using FubuMVC.Core;
using FubuMVC.StructureMap;

namespace FubuMVC.HelloValidation
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            FubuApplication
                .For<HelloValidationFubuRegistry>()
                .StructureMapObjectFactory(configure => configure.Scan(s =>
                                                                           {
                                                                               s.TheCallingAssembly();
                                                                               s.WithDefaultConventions();
                                                                           }))
                .Bootstrap(RouteTable.Routes);
        }
    }
}