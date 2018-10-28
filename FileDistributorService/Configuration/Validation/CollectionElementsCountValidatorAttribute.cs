using System;
using System.Configuration;

namespace FileDistributorService.Configuration.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class CollectionElementsCountValidatorAttribute : ConfigurationValidatorAttribute
    {
        public CollectionElementsCountValidatorAttribute(int minElementCount)
        {
            ValidatorInstance = new CollectionElementsCountValidator(minElementCount);
        }

        public override ConfigurationValidatorBase ValidatorInstance { get; }
    }
}
