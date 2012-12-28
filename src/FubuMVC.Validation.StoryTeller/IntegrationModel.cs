using System;
using FubuCore.Dates;
using FubuValidation;

namespace FubuMVC.Validation.StoryTeller
{
    public class IntegrationModel
    {
        public int Numeric { get; set; }
        
        public Date FubuDate { get; set; }
        public DateTime StandardDate { get; set; }

        [GreaterThanZero]
        public int GreaterThanZero { get; set; }
        [GreaterOrEqualToZero]
        public int GreaterOrEqualToZero { get; set; }
        
        [MinimumStringLength(10)]
        public string LongerThanTen { get; set; }
        [MaximumStringLength(5)]
        public string NoMoreThanFiveCharacters { get; set; }
        [RangeLength(5, 10)]
        public string AtLeastFiveButNotTen { get; set; }
        
        [MinValue(5)]
        public int GreaterThanFive { get; set; }
        [MaxValue(15)]
        public double LessThanFifteen { get; set; }

        [Email]
        public string Email { get; set; }

        [Required]
        public string Required { get; set; }
    }
}