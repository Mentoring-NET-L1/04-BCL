using System.Configuration;
using FileDistributorService.Configuration.Validation;

namespace FileDistributorService.Configuration
{
    internal class FileDistributorServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("culture", IsRequired = false)]
        public CultureElement Culture
        {
            get => (CultureElement)this["culture"];
        }

        [ConfigurationProperty("defaultDirectory", IsRequired = true)]
        public DefaultDirectoryElement DefaultDirectory
        {
            get => (DefaultDirectoryElement)this["defaultDirectory"];
        }

        [ConfigurationProperty("watchedDirectories", IsRequired = true)]
        [CollectionElementsCountValidator(1)]
        public WatchedDirectoryCollection WatchedDirectories
        {
            get => (WatchedDirectoryCollection)this["watchedDirectories"];
        }

        [ConfigurationProperty("mapRules", IsRequired = false)]
        public MapRuleCollection MapRules
        {
            get => (MapRuleCollection)this["mapRules"];
        }
    }
}
