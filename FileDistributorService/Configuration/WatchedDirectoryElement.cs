using System.Configuration;
using FileDistributorService.Configuration.Validation;

namespace FileDistributorService.Configuration
{
    internal class WatchedDirectoryElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        //[CallbackValidator(Type = typeof(ValidationHelper), CallbackMethodName = "IsDirectoryExists")]
        //[DirectoryExistsValidator]
        public string Value
        {
            get => (string)this["value"];
        }
    }
}
