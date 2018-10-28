using System.Configuration;

namespace FileDistributorService.Configuration
{
    internal class WatchedDirectoryCollection : ConfigurationElementCollection<WatchedDirectoryElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WatchedDirectoryElement)element).Value;
        }
    }
}
