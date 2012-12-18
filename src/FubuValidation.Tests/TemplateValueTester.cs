using System.Collections.Generic;
using FubuLocalization;
using FubuTestingSupport;
using NUnit.Framework;

namespace FubuValidation.Tests
{
    [TestFixture]
    public class TemplateValueTester
    {
        [Test]
        public void equals_other_value()
        {
            var theTemplate = TemplateValue.For("Test", "123");
            theTemplate.ShouldEqual(TemplateValue.For("Test", "123"));
        }

        [Test]
        public void does_not_equal_value()
        {
            var theTemplate = TemplateValue.For("Test", "123");
            theTemplate.ShouldNotEqual(TemplateValue.For("Test2", "1234"));
        }
    }

    [TestFixture]
    public class TemplateValueCollectionTester
    {
        [Test]
        public void gets_the_values()
        {
            var values = new List<TemplateValue>
            {
                TemplateValue.For("FirstName", "Joel"),
                TemplateValue.For("LastName", "Arnold")
            };

            var collection = new TemplateValueCollection(values);
            collection.ShouldHaveTheSameElementsAs(values);
        }

        [Test]
        public void gets_the_values_from_the_static_builder()
        {
            var v1 = TemplateValue.For("FirstName", "Joel");
            var v2 = TemplateValue.For("LastName", "Arnold");

            var collection = TemplateValueCollection.For(v1, v2);
            collection.ShouldHaveTheSameElementsAs(v1, v2);
        }

        [Test]
        public void adds_the_value()
        {
            var v1 = TemplateValue.For("FirstName", "Joel");
            var v2 = TemplateValue.For("LastName", "Arnold");

            var collection = new TemplateValueCollection();
            collection.Add(v1);
            collection.Add(v2);

            collection.ShouldHaveTheSameElementsAs(v1, v2);
        }

        [Test]
        public void only_adds_the_value_once()
        {
            var v1 = TemplateValue.For("FirstName", "Joel");

            var collection = new TemplateValueCollection();
            collection.Add(v1);
            collection.Add(v1);

            collection.ShouldHaveTheSameElementsAs(v1);
        }

        [Test]
        public void equality_check()
        {
            var v1 = TemplateValue.For("FirstName", "Joel");
            var v2 = TemplateValue.For("LastName", "Arnold");

            var collection1 = TemplateValueCollection.For(v1, v2);
            var collection2 = TemplateValueCollection.For(v1, v2);

            collection1.ShouldEqual(collection2);
        }

        [Test]
        public void equality_check_negative()
        {
            var v1 = TemplateValue.For("FirstName", "Joel");
            var v2 = TemplateValue.For("LastName", "Arnold");

            var collection1 = TemplateValueCollection.For(v1, v2);
            var collection2 = TemplateValueCollection.For(v1);

            collection1.ShouldNotEqual(collection2);
        }

        [Test]
        public void builds_the_dictionary()
        {
            var v1 = TemplateValue.For("FirstName", "Joel");
            var v2 = TemplateValue.For("LastName", "Arnold");

            var values = TemplateValueCollection.For(v1, v2).ToDictionary();
            values["FirstName"].ShouldEqual("Joel");
            values["LastName"].ShouldEqual("Arnold");
        }
    }

    [TestFixture]
    public class TemplateTester
    {
        [Test]
        public void renders_the_template()
        {
            var theValue = TemplateValue.For("field", "FirstName");
            var theTemplate = new Template(StringToken.FromKeyString("Test", "{field} is required"), theValue);

            theTemplate.Render().ShouldEqual("FirstName is required");
        }

        [Test]
        public void equality_check()
        {
            var theValue = TemplateValue.For("field", "FirstName");
            var template1 = new Template(StringToken.FromKeyString("Test", "{field} is required"), theValue);

            var template2 = new Template(StringToken.FromKeyString("Test", "{field} is required"));
            template2.Values.Add(theValue);

            template1.ShouldEqual(template2);
        }

        [Test]
        public void equality_check_negative()
        {
            var theValue = TemplateValue.For("field", "FirstName");
            var template1 = new Template(StringToken.FromKeyString("Test", "{field} is required"), theValue);

            var template2 = new Template(StringToken.FromKeyString("Test2", "{field} is invalid"));
            template2.Values.Add(theValue);

            template1.ShouldNotEqual(template2);

            var template3 = new Template(StringToken.FromKeyString("Test3", "{field} is required"));
            template1.ShouldNotEqual(template3);
        }
    }
}