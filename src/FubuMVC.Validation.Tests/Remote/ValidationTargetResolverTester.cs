using FubuCore;
using FubuCore.Binding;
using FubuCore.Reflection;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Remote
{
    [TestFixture]
    public class ValidationTargetResolverTester
    {
        private IObjectResolver theObjectResolver;
        private ValidationTargetResolver theResolver;

        [SetUp]
        public void SetUp()
        {
            theObjectResolver = ObjectResolver.Basic();
            theResolver = new ValidationTargetResolver(theObjectResolver);
        }

        [Test]
        public void target_with_simple_string_property()
        {
            var value = "Test";
            var accessor = ReflectionHelper.GetAccessor<ExampleModel>(x => x.FirstName);
            
            var model = theResolver.Resolve(accessor, value).As<ExampleModel>();
            model.FirstName.ShouldEqual(value);
        }

        [Test]
        public void target_with_simple_int_property()
        {
            var accessor = ReflectionHelper.GetAccessor<ExampleModel>(x => x.Count);

            var model = theResolver.Resolve(accessor, "10").As<ExampleModel>();
            model.Count.ShouldEqual(10);
        }

        [Test]
        public void target_with_value_type()
        {
            var value = "Test";
            var accessor = ReflectionHelper.GetAccessor<ExampleModel>(x => x.Nested);

            var model = theResolver.Resolve(accessor, value).As<ExampleModel>();
            model.Nested.Value.ShouldEqual(value);
        }

        public class ExampleModel
        {
            public string FirstName { get; set; }
            public int Count { get; set; }
            public ExampleValueObject Nested { get; set; }
        }

        public class ExampleValueObject
        {
            public ExampleValueObject(string value)
            {
                Value = value;
            }

            public string Value { get; private set; }
        }
    }
}