using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using FubuCore.Reflection;
using FubuLocalization;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Continuations;
using FubuMVC.Core.UI;
using FubuValidation;
using FubuValidation.Fields;
using HtmlTags;

namespace FubuMVC.Validation.StoryTeller
{
    public class ElementHandler
    {
        [Required, Throttled(0)]
        public string AjaxValueFast { get; set; }
        [Required, Throttled(3)]
        public string AjaxValueSlow { get; set; }
        [Required, Throttled(10)]
        public string AjaxValueReallySlow { get; set; }
        [Required]
        public string SynchronousValue { get; set; }
    }

    public class Throttled : FieldValidationAttribute
    {
        private readonly int _throttleSeconds;

        public Throttled(int throttleSeconds)
        {
            _throttleSeconds = throttleSeconds;
        }

        public override IEnumerable<IFieldValidationRule> RulesFor(PropertyInfo property)
        {
            yield return new ThrottledRule(TimeSpan.FromSeconds(_throttleSeconds));
        }
    }

    public class ThrottledRule : IFieldValidationRule
    {
        private readonly TimeSpan _throttle;

        public ThrottledRule(TimeSpan throttle)
        {
            _throttle = throttle;
        }

        public StringToken Token { get; set; }
        public ValidationMode Mode { get; set; }
        public void Validate(Accessor accessor, ValidationContext context)
        {
            Thread.Sleep(_throttle);
        }
    }

    public class ElementHandlerEndpoint
    {
        private readonly FubuHtmlDocument<ElementHandler> _page;

        public ElementHandlerEndpoint(FubuHtmlDocument<ElementHandler> page)
        {
            _page = page;
        }

        public FubuHtmlDocument<ElementHandler> get_elementHandlers_create(ElementHandler request)
        {
            _page.Add(new HtmlTag("h1").Text("ValidationElementHandler test cases"));
            _page.Add(createForm());
            _page.Add(new TextboxTag().Attr("name", "NoValidation"));
            _page.Add(_page.WriteScriptTags());
            return _page;
        }

        public FubuContinuation post_elementHandlers_create(ElementHandler user)
        {
            return FubuContinuation.RedirectTo(new ElementHandler());
        }

        private HtmlTag createForm()
        {
            return _page.FormFor<ElementHandler>(form =>
            {
              form.Append(_page.Edit(x => x.AjaxValueFast));
              form.Append(_page.Edit(x => x.AjaxValueSlow));
              form.Append(_page.Edit(x => x.AjaxValueReallySlow));
              form.Append(_page.Edit(x => x.SynchronousValue));
              form.Append(new HtmlTag("input").Attr("type", "submit").Attr("value", "Submit").Id("ElementHandler"));
              form.Id("ElementHandlerForm");
            });
        }
    }
}