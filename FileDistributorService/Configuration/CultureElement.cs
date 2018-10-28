using System.Configuration;
using System.Globalization;

namespace FileDistributorService.Configuration
{
    internal class CultureElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = false, DefaultValue = "en")]
        public CultureInfo Value
        {
            get => (CultureInfo)this["value"];
        }
    }
}
