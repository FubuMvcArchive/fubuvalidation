using System;
using System.Linq.Expressions;
using FubuCore.Reflection;
using FubuMVC.Validation.Remote;
using FubuTestingSupport;
using FubuValidation.Fields;
using NUnit.Framework;

namespace FubuMVC.Validation.Tests.Remote
{
    [TestFixture]
    public class RemoteFieldRuleTester
    {
        private Accessor accessorFor(Expression<Func<RemoteFieldModel, object>> expression)
        {
            return expression.ToAccessor();
        }

        [Test]
        public void blows_up_if_the_type_is_not_a_field_validation_rule()
        {
            Exception<ArgumentException>
                .ShouldBeThrownBy(() => new RemoteFieldRule(typeof (string), accessorFor(x => x.Name)));
        }

        [Test]
        public void equality_check()
        {
            var accessor = accessorFor(x => x.Name);

            var r1 = new RemoteFieldRule(typeof (RequiredFieldRule), accessor);
            var r2 = new RemoteFieldRule(typeof(RequiredFieldRule), accessor);

            r1.ShouldEqual(r2);
        }

        [Test]
        public void equality_check_negative_accessor()
        {
            var r1 = new RemoteFieldRule(typeof(RequiredFieldRule), accessorFor(x => x.Name));
            var r2 = new RemoteFieldRule(typeof(RequiredFieldRule), accessorFor(x => x.Test));

            r1.ShouldNotEqual(r2);
        }

        [Test]
        public void equality_check_negative_type()
        {
            var accessor = accessorFor(x => x.Name);

            var r1 = new RemoteFieldRule(typeof(RequiredFieldRule), accessor);
            var r2 = new RemoteFieldRule(typeof(MinimumLengthRule), accessor);

            r1.ShouldNotEqual(r2);
        }

        [Test]
        public void hash_is_repeatable()
        {
            var r1 = new RemoteFieldRule(typeof(RequiredFieldRule), accessorFor(x => x.Name));
            var r2 = new RemoteFieldRule(typeof(RequiredFieldRule), accessorFor(x => x.Name));

            r1.ToHash().ShouldEqual(r2.ToHash());
        }

        public class RemoteFieldModel
        {
            public string Name { get; set; }
            public string Test { get; set; }
        }
    }
}