using FubuCore;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures;
using Serenity.Fixtures.Handlers;
using StoryTeller.Assertions;

namespace FubuMVC.Validation.Serenity
{
    public class ValidationElementHandler : ElementHandlerWrapper
    {
        private const string ValidationCountKey = "data-validation-count";

        protected override bool WrapperMatches(IWebElement element)
        {
            return element.HasAttribute(ValidationCountKey);
        }

        public override void EnterData(ISearchContext context, IWebElement element, object data)
        {
            var beginingCount = int.Parse(element.GetAttribute(ValidationCountKey));
            EnterDataNested(context, element, data);
            var timedout = !Wait.Until(() => int.Parse(element.GetAttribute(ValidationCountKey)) > beginingCount);
            StoryTellerAssert.Fail(timedout, "Validation for {0} took longer than expected".ToFormat(element));
        }

        public override string GetData(ISearchContext context, IWebElement element)
        {
            return GetDataNested(context, element);
        }
    }
}