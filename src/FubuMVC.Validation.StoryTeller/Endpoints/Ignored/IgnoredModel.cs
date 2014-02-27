using FubuValidation;

namespace FubuMVC.Validation.StoryTeller.Endpoints.Ignored
{
    [NotValidated]
    public class IgnoredModel
    {
        [Required]
        public string Name { get; set; }
    }
}