using System.Collections.Generic;

namespace FubuValidation.Tests.Models
{
    public class CompositeModel
    {
        public int Id { get; set; }
        [ContinueValidation]
        public ContactModel Contact { get; set; }
        public ContactModel RestrictedContact { get; set; }
        public IList<ContactModel> Contacts { get; set; }
    }
}