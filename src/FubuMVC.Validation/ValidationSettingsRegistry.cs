using System;
using System.Collections.Generic;
using FubuCore;
using FubuMVC.Core.Registration.Policies;

namespace FubuMVC.Validation
{
	public class ValidationSettingsRegistry
	{
		private readonly IList<ValidationNodeModification> _modifications = new List<ValidationNodeModification>(); 

		public void ForChainsMatching<T>(Action<ValidationNode> action)
			where T : IChainFilter, new()
		{
			ForChainsMatching(new T(), action);
		}

		public void ForChainsMatching(IChainFilter filter, Action<ValidationNode> action)
		{
			addModification(new ValidationNodeModification(filter, action));
		}

		public void ForInputType<T>(Action<ValidationNode> action)
		{
			ForChainsMatching(new InputTypeIs<T>(), action);
		}
		
		public void ForInputTypesMatching(Func<Type, bool> filter, Action<ValidationNode> action)
		{
			var chainFilter = new LambdaChainFilter(chain => chain.InputType() != null && filter(chain.InputType()));
			ForChainsMatching(chainFilter, action);
		}

		protected void addModification(ValidationNodeModification modification)
		{
			_modifications.Add(modification);
		}

		public IEnumerable<ValidationNodeModification> Modifications { get { return _modifications; } }
	}
}