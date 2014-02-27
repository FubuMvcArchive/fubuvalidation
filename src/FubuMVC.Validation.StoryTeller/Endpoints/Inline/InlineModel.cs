using FubuValidation;

namespace FubuMVC.Validation.StoryTeller.Endpoints.Inline
{
    public class InlineModel
    {
        [Required]
        public string Name { get; set; }

        [Email]
        public string Email { get; set; }
    }
}