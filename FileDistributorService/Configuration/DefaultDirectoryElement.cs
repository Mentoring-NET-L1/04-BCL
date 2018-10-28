using System.Configuration;
using FileDistributorService.Configuration.Validation;

namespace FileDistributorService.Configuration
{
    internal class DefaultDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        //[CallbackValidator(Type = typeof(ValidationHelper), CallbackMethodName = "IsDirectoryExists")]
        public string Value
        {
            get => (string)this["value"];
        }
    }
}
