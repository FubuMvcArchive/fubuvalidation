using System;
using FubuMVC.Core.Registration.Nodes;
using FubuValidation;

namespace FubuMVC.Validation
{
    public class ValidationFailure
    {
        private readonly ActionCall _target;
        private readonly Notification _notification;
        private readonly object _inputModel;

        public ValidationFailure(ActionCall target, Notification notification, object inputModel)
        {
            _target = target;
            _notification = notification;
            _inputModel = inputModel;
        }

        public object InputModel
        {
            get { return _inputModel; }
        }

        public Notification Notification
        {
            get { return _notification; }
        }

        public ActionCall Target
        {
            get { return _target; }
        }

        public Type InputType()
        {
            return _target.InputType();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ValidationFailure)) return false;
            return Equals((ValidationFailure) obj);
        }

        public bool Equals(ValidationFailure other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._target, _target);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _target != null ? _target.GetHashCode() : 0;
            }
        }
    }
}