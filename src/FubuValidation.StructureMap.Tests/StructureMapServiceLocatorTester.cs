using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;
using FubuTestingSupport;

namespace FubuValidation.StructureMap.Tests
{
	[TestFixture]
	public class StructureMapServiceLocatorTester
	{
		[SetUp]
		public void SetUp()
		{
			_mockSecurityContext = MockRepository.GenerateStub<ISecurityContext>();

			container = new Container(x =>
			{
				x.For<ISecurityContext>().Use(_mockSecurityContext);
			});
		}

		private ISecurityContext _mockSecurityContext;
		private IContainer container;

		[Test]
		public void should_resolve_unnamed_instances()
		{
			new StructureMapServiceLocator(container).GetInstance(typeof(ISecurityContext))
				.ShouldBeTheSameAs(_mockSecurityContext);
		}
	}

	public interface ISecurityContext { }
}