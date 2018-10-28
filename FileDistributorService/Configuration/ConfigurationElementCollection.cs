using System.Collections.Generic;
using System.Configuration;

namespace FileDistributorService.Configuration
{
    public abstract class ConfigurationElementCollection<T> : ConfigurationElementCollection, IEnumerable<T>
        where T : ConfigurationElement, new()
    {
        public T this[int index] => (T)BaseGet(index);

        public new IEnumerator<T> GetEnumerator()
        {
            foreach (T item in (ConfigurationElementCollection)this)
            {
                yield return item;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }
    }
}