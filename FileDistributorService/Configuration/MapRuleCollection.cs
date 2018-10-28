using System.Configuration;

namespace FileDistributorService.Configuration
{
    internal class MapRuleCollection : ConfigurationElementCollection<MapRuleElement>
    {
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MapRuleElement)element).FileNameRegex;
        }
    }
}
