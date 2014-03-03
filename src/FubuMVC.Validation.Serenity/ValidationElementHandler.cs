using System;
using System.Diagnostics;
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
            if (GetData(context, element).Equals(data))
            {
                EnterDataNested(context, element, data);
                return;
            }

            var beginningCount = int.Parse(element.GetAttribute(ValidationCountKey));

            var stopwatch = Stopwatch.StartNew();
            EnterDataNested(context, element, data);

            // Trigger validation quicker
            element.SendKeys(Keys.Tab);

            var timedout = !Wait.Until(() => int.Parse(element.GetAttribute(ValidationCountKey)) > beginningCount);

            stopwatch.Stop();
            timedout = timedout || stopwatch.Elapsed > TimeSpan.FromSeconds(10);

            StoryTellerAssert.Fail(timedout, "Validation for {0} either took longer than expected, or did not occur at all".ToFormat(element));
        }

        public override string GetData(ISearchContext context, IWebElement element)
        {
            return GetDataNested(context, element);
        }
    }
}