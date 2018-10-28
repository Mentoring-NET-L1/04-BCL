using System;
using System.Configuration;

namespace FileDistributorService.Configuration.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    class DirectoryExistsValidatorAttribute : ConfigurationValidatorAttribute
    {
        public DirectoryExistsValidatorAttribute()
        {
            ValidatorInstance = new SkipDefaultValueCallbackValidator(typeof(string), ValidationHelper.IsDirectoryExists);
        }

        public override ConfigurationValidatorBase ValidatorInstance { get; }
    }
}
