using FubuMVC.Validation.StoryTeller.Endpoints.Ignored;
using OpenQA.Selenium;
using StoryTeller.Engine;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
	public class IgnoredEndpointScreenFixture : ValidationScreenFixture<IgnoredModel>
	{
        [FormatAs("The text on the screen is: {value}")]
        [return: AliasAs("value")]
		 public string VerifyTheText()
		 {
		     return Driver.FindElement(By.TagName("pre")).Text;
		 }
	}
}