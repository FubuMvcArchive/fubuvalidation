using System;

namespace FubuMVC.Validation
{
    public interface IFubuContinuationModelDescriptor
    {
        Type DescribeModelFor(ValidationFailure context);
    }
}