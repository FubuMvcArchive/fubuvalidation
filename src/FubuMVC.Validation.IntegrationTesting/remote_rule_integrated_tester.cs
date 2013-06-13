using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FubuCore;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core;
using FubuMVC.Core.Ajax;
using FubuMVC.Validation.IntegrationTesting.Ajax;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.IntegrationTesting
{
    [TestFixture]
    public class remote_rule_integrated_tester : ValidationHarness
    {
        private UserService theUserService;
        private ValidateField theField;

        protected override void configure(FubuRegistry registry)
        {
            registry.Actions.IncludeType<CreateUserEndpoint>();
            registry.Import<FubuMvcValidation>();

            theUserService = new UserService();
            registry.Services(r => r.SetServiceIfNone<IUserService>(theUserService));

            var rule = RemoteFieldRule.For(ReflectionHelper.GetAccessor<CreateUser>(x => x.Username), new UniqueUsernameRule());
            theField = new ValidateField { Hash = rule.ToHash(), Value = "joel_arnold" };
            theUserService.AddUser(theField.Value);
        }

        private JsonResponse theContinuation
        {
            get
            {
                var response = endpoints.PostJson(theField);

                try
                {
                    return response.ReadAsJson<JsonResponse>();
                }
                catch
                {
                    Debug.WriteLine(response.ReadAsText());
                    throw;
                }

            }
        }

        [Test]
        public void validation_fails()
        {
            theContinuation.success.ShouldBeFalse();
        }

        [Test]
        public void adds_the_username_in_use_message()
        {
            theContinuation.errors[0].message.ShouldEqual("'{0}' already taken".ToFormat(theField.Value));
        }
    }

    [Remote]
    public class UniqueUsernameRule : IFieldValidationRule
    {
	    public StringToken Token { get; set; }

		public ValidationMode Mode { get; set; }

	    public void Validate(Accessor accessor, ValidationContext context)
        {
            var service = context.Service<IUserService>();
            var username = context.GetFieldValue<string>(accessor);
            if(service.UsernameExists(username))
            {
                context
                    .Notification
                    .RegisterMessage(accessor, StringToken.FromKeyString("Unique", "'{username}' already taken"), TemplateValue.For("username", username));
            }
        }
    }

    public class CreateUserEndpoint
    {
        public AjaxContinuation post_users_create(CreateUser user)
        {
            throw new NotImplementedException();
        }
    }

    public class CreateUser
    {
        [UniqueUsername]
        public string Username { get; set; }
    }

    public class UniqueUsernameAttribute : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new UniqueUsernameRule();
        }
    }

    public interface IUserService
    {
        bool UsernameExists(string username);
    }

    public class UserService : IUserService
    {
        private readonly IList<string> _usernames = new List<string>();

        public void AddUser(string username)
        {
            _usernames.Fill(username);
        }

        public bool UsernameExists(string username)
        {
            return _usernames.Contains(username);
        }
    }
}