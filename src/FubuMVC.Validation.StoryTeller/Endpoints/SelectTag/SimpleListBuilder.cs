using FubuMVC.Core.UI.Elements;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class SimpleListBuilder : IElementBuilder
    {
        public HtmlTag Build(ElementRequest request)
        {
            return new SelectTag(x =>
            {
                x.Option("Choose an option", "");
                x.Option("One", "1");
                x.Option("Two", "2");
            });
        }
    }
}