using System;
using System.Configuration;

namespace FileDistributorService.Configuration.Validation
{
    class CollectionElementsCountValidator : ConfigurationValidatorBase
    {
        public CollectionElementsCountValidator(int minCount)
        {
            MinElementsCount = minCount;
        }

        public int MinElementsCount { get; }

        public override bool CanValidate(Type type)
        {
            return type.IsSubclassOf(typeof(ConfigurationElementCollection));
        }

        public override void Validate(object value)
        {
            var collection = (ConfigurationElementCollection)value;

            if (collection == null || collection.Count < MinElementsCount)
                throw new ConfigurationErrorsException($"Collection is required and must contain at least one element.");
        }
    }
}
