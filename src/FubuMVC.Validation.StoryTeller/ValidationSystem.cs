using FubuMVC.HelloValidation;
using Serenity;

namespace FubuMVC.Validation.StoryTeller
{
    public class ValidationSystem : SerenitySystem
    {
        public ValidationSystem()
        {
            WebDriverSettings.Current.Browser = BrowserType.Firefox;
            AddApplication<HelloValidationApplication>();
        }
    }
}