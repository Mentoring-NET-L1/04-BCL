using System.Configuration;
using System.IO;

namespace FileDistributorService.Configuration.Validation
{
    internal static class ValidationHelper
    {
        public static void IsDirectoryExists(object value)
        {
            var path = value as string;

            if (path == null)
                throw new ConfigurationErrorsException("Directory path must be a string.");

            if (!Directory.Exists(path))
                throw new ConfigurationErrorsException("Directory must exist.");
        }
    }
}
