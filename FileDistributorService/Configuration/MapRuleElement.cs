using System.Configuration;

namespace FileDistributorService.Configuration
{
    internal class MapRuleElement : ConfigurationElement
    {
        [ConfigurationProperty("fileNameRegex", IsKey = true, IsRequired = true)]
        public string FileNameRegex
        {
            get => (string)this["fileNameRegex"];
        }

        [ConfigurationProperty("destDirectory", IsRequired = true)]
        public string DestDirectory
        {
            get => (string)this["destDirectory"];
        }

        [ConfigurationProperty("addSerialNumber", IsRequired = false, DefaultValue = false)]
        public bool AddSerialNumber
        {
            get => (bool)this["addSerialNumber"];
        }

        [ConfigurationProperty("addMoveDate", IsRequired = false, DefaultValue = false)]
        public bool AddMoveDate
        {
            get => (bool)this["addMoveDate"];
        }
    }
}
