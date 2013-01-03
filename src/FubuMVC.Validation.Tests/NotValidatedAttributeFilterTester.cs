using System;
using System.Linq.Expressions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Validation.Tests.UI;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
	[TestFixture]
	public class NotValidatedAttributeFilterTester
	{
		private bool matches<T>(Expression<Func<T, object>> expression)
		{
			var call = ActionCall.For(expression);
			var chain = new BehaviorChain();
			chain.AddToEnd(call);

			return new NotValidatedAttributeFilter().Matches(chain);
		}

		[Test]
		public void filters_models_with_NotValidatedAttribute()
		{
			matches<FormValidationModeEndpoint>(x => x.post_none(null)).ShouldBeFalse();
		}

		[Test]
		public void filters_methods_with_NotValidatedAttribute()
		{
			matches<FormValidationModeEndpoint>(x => x.post_ignored(null)).ShouldBeFalse();
		}

		[Test]
		public void matches_others()
		{
			matches<FormValidationModeEndpoint>(x => x.post_lofi(null)).ShouldBeTrue();
		}
	}
}