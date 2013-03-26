using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FubuCore.Reflection;
using FubuTestingSupport;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests
{
    [TestFixture]
    public class ListFieldValidationSourceTester : InteractionContext<ListFieldValidationSource>
    {
        [Test]
        public void returns_an_empty_list_for_properties_that_are_not_of_ilist_generic_type()
        {
            ClassUnderTest.RulesFor(getProperty(x => x.GenericEnumerableProperty)).ShouldHaveCount(0);
            ClassUnderTest.RulesFor(getProperty(x => x.RegularEnumerableProperty)).ShouldHaveCount(0);
            ClassUnderTest.RulesFor(getProperty(x => x.RegularListProperty)).ShouldHaveCount(0);
            ClassUnderTest.RulesFor(getProperty(x => x.CollectionProperty)).ShouldHaveCount(0);
            ClassUnderTest.RulesFor(getProperty(x => x.StringProperty)).ShouldHaveCount(0);
            ClassUnderTest.RulesFor(getProperty(x => x.ObjectProperty)).ShouldHaveCount(0);
            ClassUnderTest.RulesFor(getProperty(x => x.RegularListProperty)).ShouldHaveCount(0);
        }

        [Test]
        public void returns_a_ListValidationRule_for_properties_that_are_of_ilist_generic_type()
        {
            ClassUnderTest.RulesFor(getProperty(x => x.GenericListProperty)).ToArray()
                .ShouldHaveCount(1).First()
                .ShouldBeOfType<ListValidationRule>();

            ClassUnderTest.RulesFor(getProperty(x => x.ArrayProperty)).ToArray()
                .ShouldHaveCount(1).First()
                .ShouldBeOfType<ListValidationRule>();
        }

        private PropertyInfo getProperty(Expression<Func<ListFieldValidationSourceTesterModel, object>> expression)
        {
            return ReflectionHelper.GetProperty(expression);
        }
        public class ListFieldValidationSourceTesterModel
        {
            public IList<Model> GenericListProperty { get; set; }
            public Model[] ArrayProperty { get; set; }
            public IEnumerable<Model> GenericEnumerableProperty { get; set; }
            public IEnumerable RegularEnumerableProperty { get; set; }
            public IList RegularListProperty { get; set; }
            public ICollection<Model> CollectionProperty { get; set; }
            public string StringProperty { get; set; }
            public object ObjectProperty { get; set; }
            public class Model { }

        }
    }
}