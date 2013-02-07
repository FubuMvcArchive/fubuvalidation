using FubuCore.Descriptions;
using FubuCore.Reflection;
using FubuLocalization;

namespace FubuValidation.Fields
{
    public class ConditionalFieldRule<T> : IFieldValidationRule, DescribesItself
        where T : class
    {
        private readonly IFieldRuleCondition _condition;
        private readonly IFieldValidationRule _inner;

        public ConditionalFieldRule(IFieldRuleCondition condition, IFieldValidationRule inner)
        {
            _condition = condition;
            _inner = inner;
        }

        public void Validate(Accessor accessor, ValidationContext context)
        {
            if(_condition.Matches(accessor, context))
            {
                _inner.Validate(accessor, context);
            }
        }

        public IFieldRuleCondition Condition
        {
            get { return _condition; }
        }

	    public StringToken Token
	    {
			get { return _inner.Token; }
			set { _inner.Token = value; }
	    }

        public IFieldValidationRule Inner
        {
            get { return _inner; }
        }

        public void Describe(Description description)
        {
            description.AddChild("If", Condition);
            description.AddChild("Then", Inner);
        }
    }
}