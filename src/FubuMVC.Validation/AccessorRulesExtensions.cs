using System;
using FubuLocalization;
using FubuMVC.Core.Registration;
using FubuValidation.Fields;

namespace FubuMVC.Validation
{
    // This is all covered by ST
    public static class AccessorRulesExtensions
    {
	    public static void MaximumLength(this IAccessorRulesExpression expression, int length)
        {
            expression.Add(new MaximumLengthRule(length));
        }

		public static void MaximumLength(this IAccessorRulesExpression expression, int length, StringToken message)
		{
			expression.Add(new MaximumLengthRule(length, message));
		}

        public static void GreaterThanZero(this IAccessorRulesExpression expression)
        {
            expression.Add(new GreaterThanZeroRule());
        }

		public static void GreaterThanZero(this IAccessorRulesExpression expression, StringToken message)
		{
			expression.Add(new GreaterThanZeroRule(message));
		}

        public static void GreaterOrEqualToZero(this IAccessorRulesExpression expression)
        {
            expression.Add(new GreaterOrEqualToZeroRule());
        }

		public static void GreaterOrEqualToZero(this IAccessorRulesExpression expression, StringToken message)
		{
			expression.Add(new GreaterOrEqualToZeroRule(message));
		}

        public static void Required(this IAccessorRulesExpression expression)
        {
            expression.Add(new RequiredFieldRule());
        }

		public static void Required(this IAccessorRulesExpression expression, StringToken message)
		{
			expression.Add(new RequiredFieldRule(message));
		}

        public static void Email(this IAccessorRulesExpression expression)
        {
            expression.Add(new EmailFieldRule());
        }

		public static void Email(this IAccessorRulesExpression expression, StringToken message)
		{
			expression.Add(new EmailFieldRule(message));
		}

        public static void MinimumLength(this IAccessorRulesExpression expression, int length)
        {
            expression.Add(new MinimumLengthRule(length));
        }

		public static void MinimumLength(this IAccessorRulesExpression expression, int length, StringToken message)
		{
			expression.Add(new MinimumLengthRule(length, message));
		}

        public static void MinValue(this IAccessorRulesExpression expression, IComparable bounds)
        {
            expression.Add(new MinValueFieldRule(bounds));
        }

		public static void MinValue(this IAccessorRulesExpression expression, IComparable bounds, StringToken message)
		{
			expression.Add(new MinValueFieldRule(bounds, message));
		}

        public static void RangeLength(this IAccessorRulesExpression expression, int min, int max)
        {
            expression.Add(new RangeLengthFieldRule(min, max));
        }

		public static void RangeLength(this IAccessorRulesExpression expression, int min, int max, StringToken message)
		{
			expression.Add(new RangeLengthFieldRule(min, max, message));
		}

        public static void MaxValue(this IAccessorRulesExpression expression, IComparable bounds)
        {
            expression.Add(new MaxValueFieldRule(bounds));
        }

		public static void MaxValue(this IAccessorRulesExpression expression, IComparable bounds, StringToken message)
		{
			expression.Add(new MaxValueFieldRule(bounds, message));
		} 
    }
}