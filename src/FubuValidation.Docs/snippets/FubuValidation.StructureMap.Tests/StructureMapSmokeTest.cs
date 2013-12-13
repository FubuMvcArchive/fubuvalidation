using NUnit.Framework;
using StructureMap;
using FubuTestingSupport;

namespace FubuValidation.StructureMap.Tests
{
	// SAMPLE: StructureMapBootstrapping
	[TestFixture]
	public class StructureMapSmokeTest
	{

		private IValidator validator
		{
			get
			{
				var container = new Container();

				ValidationConfiguration.Bootstrap(validation =>
				{
					validation.StructureMap(container);

					validation
						.Registration
						.AddFromAssemblyContaining<TargetDslRules>();
				});

				return container.GetInstance<IValidator>();
			}
		}

		[Test]
		public void validates_the_target()
		{
			var target = new Target();
			var notification = validator.Validate(target);

			notification.MessagesFor<Target>(x => x.Name).ShouldHaveCount(1);
		}

		[Test]
		public void conditional_rule_from_dsl_registration()
		{
			var target = new Target { Name = "TooShort" };
			var notification = validator.Validate(target);

			notification.MessagesFor<Target>(x => x.Name).ShouldHaveCount(1);
		}

		public class Target
		{
			[Required]
			public string Name { get; set; }
		}

		public class TargetDslRules : ClassValidationRules<Target>
		{
			public TargetDslRules()
			{
				Property(x => x.Name)
					.MinimumLength(10)
					.IfValid();
			}
		}
	}
	// ENDSAMPLE
}