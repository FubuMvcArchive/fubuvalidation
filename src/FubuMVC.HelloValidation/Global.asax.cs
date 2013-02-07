using System;
using System.Collections.Generic;
using System.Reflection;
using Bottles;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuMVC.Validation.Remote;
using FubuValidation;
using FubuValidation.Fields;
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
    
    public interface IUserService
    {
        bool UsernameExists(string username);
    }

    public class UserService : IUserService
    {
        public bool UsernameExists(string username)
        {
            return username == "joel-arnold";
        }
    }

    [Remote]
    public class UniqueUsernameRule : IFieldValidationRule
    {
	    public UniqueUsernameRule()
	    {
		    Token = StringToken.FromKeyString("UniqueUser", "'{username}' is already in use");
	    }

	    public StringToken Token { get; set; }

	    public void Validate(Accessor accessor, ValidationContext context)
        {
            var username = context.GetFieldValue<string>(accessor);
            var service = context.Service<IUserService>();

            if(service.UsernameExists(username))
            {
                context.Notification.RegisterMessage(accessor, Token, TemplateValue.For("username", username));
            }
        }
    }

    public class UniqueUsernameAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new UniqueUsernameRule();
        }
    }
}