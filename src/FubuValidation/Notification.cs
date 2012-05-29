using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuLocalization;
using System.Linq;

namespace FubuValidation
{
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(string field, string message)
			: this(field, field, message)
        {
            
        }

		public ValidationError(string field, string label, string message)
		{
			this.field = field;
			this.label = label;
			this.message = message;
		}

        public string field { get; set; }
        public string message { get; set; }
    	public string label { get; set; }

    	public bool Equals(ValidationError other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.field, field) && Equals(other.message, message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ValidationError)) return false;
            return Equals((ValidationError) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((field != null ? field.GetHashCode() : 0)*397) ^ (message != null ? message.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return string.Format("field: {0}, message: {1}", field, message);
        }
    }

    [Serializable]
    public class Notification
    {
        private readonly IList<NotificationMessage> _messages = new List<NotificationMessage>();

        public Notification()
        {
        }

        public Notification(Type targetType)
            : this()
        {
            TargetType = targetType;
        }

        public ValidationError[] ToValidationErrors()
        {
            return AllMessages.SelectMany(x => x.ToValidationErrors()).ToArray();
        }

        public Type TargetType { get; private set; }

        public IEnumerable<NotificationMessage> AllMessages
        {
            get { return _messages; }
        }

        public NotificationMessage RegisterMessage<T>(Expression<Func<T, object>> property, StringToken message)
        {
            return RegisterMessage(property.ToAccessor(), message);
        }

        public NotificationMessage RegisterMessage(Accessor accessor, StringToken notificationMessage)
        {
            var message = new NotificationMessage(notificationMessage);
            RegisterMessage(accessor, message);

            return message;
        }

        public NotificationMessage RegisterMessage(PropertyInfo property, StringToken notificationMessage)
        {
            return RegisterMessage(new SingleProperty(property), notificationMessage);
        }

        public void RegisterMessage(Accessor accessor, NotificationMessage notificationMessage)
        {
            notificationMessage.AddAccessor(accessor);
            _messages.Fill(notificationMessage);
        }

        public void AddChild(Accessor accessor, Notification childNotification)
        {
            _messages.AddRange(childNotification.AllMessages.Select(x => x.Prepend(accessor)));
        }

        public IEnumerable<NotificationMessage> MessagesFor(Accessor accessor)
        {
            return _messages.Where(x => x.Accessors.Contains(accessor));
        }

        public IEnumerable<NotificationMessage> MessagesFor<T>(Expression<Func<T, object>> property)
        {
            return MessagesFor(property.ToAccessor());
        }

        public bool IsValid()
        {
            return !AllMessages.Any();
        }

        public static Notification Valid()
        {
            return new Notification();
        }

        public static Notification Invalid()
        {
            var notification = new Notification();
            notification.RegisterMessage(ValidationKeys.REQUIRED);

            return notification;
        }


        public void RegisterMessage(NotificationMessage message)
        {
            _messages.Add(message);
        }

        public NotificationMessage RegisterMessage(StringToken stringToken)
        {
            var message = new NotificationMessage(stringToken);
            RegisterMessage(message);

            return message;
        }
    }
}