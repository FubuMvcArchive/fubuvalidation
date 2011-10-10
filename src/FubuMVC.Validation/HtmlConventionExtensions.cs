using System;
using FubuMVC.Core.UI.Configuration;
using FubuMVC.Core.UI.Tags;
using FubuValidation.Fields;
using HtmlTags;

namespace FubuMVC.Validation
{
    public static class HtmlConventionExtensions
    {
        public static TagFactoryExpression ModifyForRule<TRule>(this TagFactoryExpression tags, Action<HtmlTag> continuation)
            where TRule : IFieldValidationRule
        {
            return tags.ModifyForRule<TRule>((r, t) => continuation(t));
        }

        public static TagFactoryExpression ModifyForRule<TRule>(this TagFactoryExpression tags, Action<TRule, HtmlTag> continuation)
            where TRule : IFieldValidationRule
        {
            return tags.ModifyForRule<TRule>((rule, request, tag) => continuation(rule, tag));
        }

        public static TagFactoryExpression ModifyForRule<TRule>(this TagFactoryExpression tags, Action<TRule, ElementRequest, HtmlTag> continuation)
            where TRule : IFieldValidationRule
        {
            tags
                .Always
                .Modify((request, tag) => request
                                              .Get<IFieldValidationQuery>()
                                              .ForRule<TRule>(request.Accessor, rule => continuation(rule, request, tag)));

            return tags;
        }
    }
}