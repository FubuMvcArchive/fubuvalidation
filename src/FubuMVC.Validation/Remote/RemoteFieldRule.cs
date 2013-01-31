using System;
using FubuCore;
using FubuCore.Reflection;
using FubuValidation.Fields;

namespace FubuMVC.Validation.Remote
{
    public class RemoteFieldRule
    {
        private readonly Type _type;
        private readonly Accessor _accessor;

        public RemoteFieldRule(Type type, Accessor accessor)
        {
            if(!type.CanBeCastTo<IFieldValidationRule>())
            {
                throw new ArgumentException("Must be an IFieldValidationRule", "type");
            }

            _type = type;
            _accessor = accessor;
        }

        public Accessor Accessor
        {
            get { return _accessor; }
        }

        public Type Type
        {
            get { return _type; }
        }

        public string ToHash()
        {
            return "RuleType={0}&Type={1}&Accessor={2}".ToFormat(_type.FullName, _accessor.OwnerType.FullName, _accessor.Name).ToHash();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (RemoteFieldRule)) return false;
            return Equals((RemoteFieldRule) obj);
        }

        public bool Equals(RemoteFieldRule other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other._type == _type && Equals(other._accessor, _accessor);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_type.GetHashCode()*397) ^ _accessor.GetHashCode();
            }
        }

        public static RemoteFieldRule For(Accessor accessor, IFieldValidationRule rule)
        {
            return new RemoteFieldRule(rule.GetType(), accessor);
        }
    }
}
