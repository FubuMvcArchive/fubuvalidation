using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using Serenity;

namespace FubuMVC.Validation.StoryTeller.Fixtures
{
    public class ValidationMessage
    {
        public string Property { get; set; }
        public string Message { get; set; }

        public bool Equals(ValidationMessage other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Property, Property) && Equals(other.Message, Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ValidationMessage)) return false;
            return Equals((ValidationMessage)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Property != null ? Property.GetHashCode() : 0) * 397) ^ (Message != null ? Message.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("Property: {0}, Message: {1}", Property, Message);
        }
    }

    public class ValidationSummaryDriver
    {
        private const string ValidationSummarySelector = ".validation-summary";
        private const string DataField = "data-field";
        private readonly IWebDriver _driver;

        public ValidationSummaryDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        public IEnumerable<ValidationMessage> AllValidationMessages()
        {
            Wait.Until(() => _driver.FindElements(By.CssSelector(".validation-container li")).Count > 0);
            var validationSummaryArea = _driver.FindElement(By.CssSelector(ValidationSummarySelector));

            return validationSummaryArea.FindElements(By.TagName("li")).Select(li =>
            {
                return new ValidationMessage
                {
                    Property = li.GetAttribute(DataField),
                    Message = li.Text
                };
            }).ToList();

        }

        public bool ValidationMessageExistsFor(string dataField)
        {
            return AllValidationMessages().Any(x => x.Property == dataField);
        }
    }
}