namespace FubuMVC.Validation.Tests
{
    public class SampleInputModel
    {
        public string Field { get; set; }

        public SampleInputModel Test()
        {
            return this;
        }

        public SampleInputModel Test(string input)
        {
            return this;
        }

        public SampleInputModel Test(int input)
        {
            return this;
        }
    }
}