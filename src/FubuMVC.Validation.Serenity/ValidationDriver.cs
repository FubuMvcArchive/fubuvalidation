using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using FubuCore.Reflection;
using OpenQA.Selenium;
using Serenity;
using Serenity.Fixtures;

namespace FubuMVC.Validation.Serenity
{
    public class ValidationDriver
    {
        private readonly IWebDriver _driver;
        private readonly Func<IWebDriver, IWebElement> _context;

        public ValidationDriver(IWebDriver driver, Func<IWebDriver, IWebElement> context)
        {
            _driver = driver;
            _context = context;
        }

        private IWebElement element
        {
            get
            {
				Wait.Until(() => _context(_driver).FindElement(By.CssSelector(".validation-container")).Displayed);
				Wait.Until(() => _context(_driver).FindElements(By.CssSelector(".validation-summary > li")).Count != 0);
	            return _context(_driver).FindElement(By.CssSelector(".validation-container"));
            }
        }

        public IEnumerable<ValidationMessage> AllMessages()
        {
        	try
        	{
				return element
				.FindElements(By.CssSelector(".validation-summary li"))
				.Select(x => new ValidationMessage
				{
					Property = x.Data("field"),
					Message = x.Text
				}).ToList();
        	}
        	catch(Exception exc)
        	{
				Debug.Write(exc);
        		return new ValidationMessage[0];
        	}
            
        }

		public IEnumerable<ValidationMessage> InlineMessages()
		{
			try
			{
				return _context(_driver)
				.FindElements(By.CssSelector("span.fubu-inline-error"))
				.Select(x => new ValidationMessage
				{
					Property = x.Data("field"),
					Message = x.Text
				}).ToList();
			}
			catch
			{
				return new ValidationMessage[0];
			}

		}

        public bool Visible
        {
            get { return element.Displayed; }
        }

        public bool Hidden
        {
            get { return !Visible; }
        }

        public IEnumerable<ValidationMessage> MessagesFor(string dataField)
        {
            return AllMessages().Where(x => x.Property == dataField);
        }

        public IEnumerable<ValidationMessage> MessagesFor<T>(Expression<Func<T, object>> expression)
        {
            return MessagesFor(expression.ToAccessor().Name);
        }

        public bool HasMessagesFor(string dataField)
        {
            return MessagesFor(dataField).Any();
        }

        public bool HasMessagesFor<T>(Expression<Func<T, object>> expression)
        {
            return HasMessagesFor(expression.ToAccessor().Name);
        }
    }
}