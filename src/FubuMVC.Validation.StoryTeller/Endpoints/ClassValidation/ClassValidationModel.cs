namespace FubuMVC.Validation.StoryTeller.Endpoints.ClassValidation
{
    public class ClassValidationModel
    {
        public int GreaterThanZero { get; set; }
        public int GreaterOrEqualToZero { get; set; }
        public string LongerThanTen { get; set; }
        public string NoMoreThanFiveCharacters { get; set; }
        public string AtLeastFiveButNotTen { get; set; }
        public int GreaterThanFive { get; set; }
        public double LessThanFifteen { get; set; }
        public string Email { get; set; }
        public string Required { get; set; }
        public string Regex { get; set; }

        public string Custom { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string ConfirmEmail { get; set; }
    }
}