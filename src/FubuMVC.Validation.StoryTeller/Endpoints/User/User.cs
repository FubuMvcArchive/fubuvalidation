using FubuValidation;

namespace FubuMVC.Validation.StoryTeller.Endpoints.User
{
    public class User
    {
        [Required, Unique]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}