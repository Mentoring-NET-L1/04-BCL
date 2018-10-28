using System;
using FileDistributor.Logger;
using FileDistributor.Logger.Resources;

namespace FileDistributorService
{
    internal sealed class ConsoleLogger : ILogger
    {
        private static readonly Lazy<ConsoleLogger> _instance
            = new Lazy<ConsoleLogger>(() => new ConsoleLogger());

        public static ConsoleLogger Instance => _instance.Value;

        private ConsoleLogger()
        {
        }

        public void Error(string message)
        {
            Log(message, LogMessages.ErrorLevel, ConsoleColor.Red);
        }

        public void Error(string message, Exception exception)
        {
            Log(message, LogMessages.ErrorLevel, ConsoleColor.Red, $"\n{exception.ToString()}");
        }

        public void Info(string message)
        {
            Log(message, LogMessages.InfoLevel, Console.ForegroundColor);
        }

        private void Log(string message, string logLevel, ConsoleColor color, string exceptionMessage = "")
        {
            var prevColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now}: {logLevel}: {message}.{exceptionMessage}");

            Console.ForegroundColor = prevColor;
        }
    }
}
