using System.Diagnostics;
using FubuCore;
using FubuMVC.Validation.Serenity;
using FubuMVC.Validation.StoryTeller.Endpoints.ValidationElementHandler;
using OpenQA.Selenium;
using Serenity.Fixtures;
using Serenity.Fixtures.Handlers;
using StoryTeller.Assertions;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class ValidationElementHandlerFixture : ScreenFixture<ElementHandler>
    {
        public ValidationElementHandlerFixture()
        {
            Title = "The ValidationElementHandler test screen";
            EditableElementsForAllImmediateProperties();
        }

        protected override void beforeRunning()
        {
            Navigation.NavigateTo(new ElementHandler());
        }

        [FormatAs("The validation element handler {name} matches {matches}")]
        public bool ValidationElementHandlerMatches(string name, [SelectionValues("true", "false")] bool matches)
        {
            var element = Driver.FindElement(By.CssSelector("input[name=\"{0}\"]".ToFormat(name)));
            var actualMatches = ElementHandlers.FindHandler(element) is ValidationElementHandler;
            return actualMatches == matches;
        }

        [FormatAs("Entering text into validation field {name} waits at least {min} milliseconds but not more than {max} milliseconds for validation to complete")]
        public void ValidationElementHandlerEnterDataWithWait(string name, int min, int max)
        {
            var element = Driver.FindElement(By.CssSelector("input[name=\"{0}\"]".ToFormat(name)));
            var stopwatch = Stopwatch.StartNew();
            ElementHandlers.FindHandler(element).EnterData(Driver, element, "Some text");
            stopwatch.Stop();

            StoryTellerAssert.Fail(stopwatch.ElapsedMilliseconds < min, "Handler was faster than expected actual elapsed milliseconds {0}".ToFormat(stopwatch.ElapsedMilliseconds));
            StoryTellerAssert.Fail(stopwatch.ElapsedMilliseconds > max, "Handler was slower than expected actual elapsed milliseconds {0}".ToFormat(stopwatch.ElapsedMilliseconds));
        }

        [FormatAs("Entering text into validation field {name} waits so long that it times out and fails")]
        public void ValidationElementHandlerEnterDataWithWaitFailsWithTimeout(string name)
        {
            try
            {
                var element = Driver.FindElement(By.CssSelector("input[name=\"{0}\"]".ToFormat(name)));
                ElementHandlers.FindHandler(element).EnterData(Driver, element, "Some text");
            }
            catch (StorytellerAssertionException ex)
            {
                StoryTellerAssert.Fail(!ex.Message.Contains("took longer than expected"), "Failed to enter data for some reason other than a timeout") ;
                return;
            }

            StoryTellerAssert.Fail("Did not timeout when waiting for validation (For some reason this will fail on the Storyteller UI but not in the st runner)");
        }
    }
}