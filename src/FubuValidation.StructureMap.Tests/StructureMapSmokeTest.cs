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
			get { 
				var container = new Container(x => x.AddRegistry<FubuValidationRegistry>());
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
		
 		public class Target
 		{
			[Required]
			public string Name { get; set; }
 		}
	}
	// ENDSAMPLE
}