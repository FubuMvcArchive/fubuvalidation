using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuValidation;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class CreateUser
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CreateUserEndpoint
    {
        private readonly FubuHtmlDocument<CreateUser> _page;

        public CreateUserEndpoint(FubuHtmlDocument<CreateUser> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<CreateUser> get_users_create(CreateUser request)
        {
            _page.Add(new HtmlTag("h1").Text("Create User"));
            _page.Add(createForm());
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_users_create(CreateUser user)
        {
            return FubuContinuation.RedirectTo(new CreateUser(), "GET");
        }

        private HtmlTag createForm()
        {
            var form = _page.FormFor<CreateUser>();
            form.Append(_page.Edit(x => x.Username));
            form.Append(_page.Edit(x => x.Password));
            form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("CreateUser"));
            form.Id("CreateUserForm");

            return form;
        }
    }
}