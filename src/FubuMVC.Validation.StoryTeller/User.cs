using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class User
    {
        [Required, Unique]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CreateUserEndpoint
    {
        private readonly FubuHtmlDocument<User> _page;
        private readonly IUserService _users;

        public CreateUserEndpoint(FubuHtmlDocument<User> page, IUserService users)
        {
            _page = page;
            _users = users;
        }

        public FubuHtmlDocument<User> get_users_create(User request)
        {
            _page.Add(new HtmlTag("h1").Text("Create User"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_users_create(User user)
        {
            _users.Update(user);
            return FubuContinuation.RedirectTo(new User(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<User>();
            form.Append(_page.Edit(x => x.Username));
            form.Append(_page.Edit(x => x.Password));
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("User"));
            form.Id("CreateUserForm");

            return form;
        }
    }
}