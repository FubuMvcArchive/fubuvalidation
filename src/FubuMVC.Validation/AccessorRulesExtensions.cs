using System;
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

        public static void GreaterThanZero(this IAccessorRulesExpression expression)
        {
            expression.Add(new GreaterThanZeroRule());
        }

        public static void GreaterOrEqualToZero(this IAccessorRulesExpression expression)
        {
            expression.Add(new GreaterOrEqualToZeroRule());
        }

        public static void Required(this IAccessorRulesExpression expression)
        {
            expression.Add(new RequiredFieldRule());
        }

        public static void Email(this IAccessorRulesExpression expression)
        {
            expression.Add(new EmailFieldRule());
        }

        public static void MinimumLength(this IAccessorRulesExpression expression, int length)
        {
            expression.Add(new MinimumLengthRule(length));
        }

        public static void MinValue(this IAccessorRulesExpression expression, IComparable bounds)
        {
            expression.Add(new MinValueFieldRule(bounds));
        }

        public static void RangeLength(this IAccessorRulesExpression expression, int min, int max)
        {
            expression.Add(new RangeLengthFieldRule(min, max));
        }

        public static void MaxValue(this IAccessorRulesExpression expression, IComparable bounds)
        {
            expression.Add(new MaxValueFieldRule(bounds));
        } 

    }
}