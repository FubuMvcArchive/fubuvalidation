namespace FubuMVC.Validation.Serenity
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
}