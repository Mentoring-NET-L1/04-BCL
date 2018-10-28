using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using FileDistributor.Logger.Resources;
using FileDistributorService.Configuration;

namespace FileDistributorService
{
    internal class Program
    {
        private static void SetAppCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        private static void Main()
        {
            var fileDistributor = default(FileManagment.FileDistributor);
            try
            {
                var config = (FileDistributorServiceSection)ConfigurationManager.GetSection("fileDistributorServiceSection");
                SetAppCulture(config.Culture.Value);

                fileDistributor = new FileManagment.FileDistributor(
                    config.WatchedDirectories.Select(wd => wd.Value),
                    config.DefaultDirectory.Value);

                fileDistributor.Logger = ConsoleLogger.Instance;
                foreach (var rule in config.MapRules)
                {
                    var pathMapper = new PathMapper(rule);
                    fileDistributor.Mappers += pathMapper.Map;
                }
                fileDistributor.Run();

                var exitEvent = new AutoResetEvent(false);
                Console.CancelKeyPress += (sender, args) => 
                {
                    args.Cancel = true;
                    exitEvent.Set();
                };
                exitEvent.WaitOne();
            }
            catch (ConfigurationException)
            {
                ConsoleLogger.Instance.Error(LogMessages.ConfigurationFileError);
            }
            catch (Exception)
            {
                ConsoleLogger.Instance.Error(LogMessages.UnexpectedError);
            }
            finally
            {
                fileDistributor?.Dispose();
            }
        }
    }
}
