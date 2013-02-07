using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Validation.Remote;
using FubuValidation;
using FubuValidation.Fields;

namespace FubuMVC.Validation.StoryTeller
{
    public interface IUserService
    {
        bool UsernameExists(string username);
        void Update(User user);
        IEnumerable<User> All();

        void Clear();
    }

    public class UserService : IUserService
    {
        private readonly IList<User> _users = new List<User>();

        public bool UsernameExists(string username)
        {
            return _users.Any(user => user.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(User user)
        {
            _users.Add(user);
        }

        public IEnumerable<User> All()
        {
            return _users;
        }

        public void Clear()
        {
            _users.Clear();
        }
    }

    public class Unique : FieldValidationAttribute
    {
        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new UniqueUsernameRule();
        }
    }

    public class UniqueUsernameRule : IFieldValidationRule
    {
	    public StringToken Token { get; set; }

	    public void Validate(Accessor accessor, ValidationContext context)
        {
            var username = context.GetFieldValue<string>(accessor);
            var users = context.Service<IUserService>();

            if(users.UsernameExists(username))
            {
                context.Notification.RegisterMessage(accessor, StringToken.FromKeyString("Validation:Username", "Username '{username}' already exists"), TemplateValue.For("username", username));
            }

        }
    }
}