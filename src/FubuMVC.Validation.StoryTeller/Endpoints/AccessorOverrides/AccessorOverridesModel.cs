namespace FubuMVC.Validation.StoryTeller.Endpoints.AccessorOverrides
{
    public class AccessorOverridesModel
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

        public string EmailCustomMessage { get; set; }

        public string Custom { get; set; }

        public int Triggered { get; set; }
    }
}