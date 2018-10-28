using System;

namespace FileDistributor.Logger
{
    public interface ILogger
    {
        void Info(string message);

        void Error(string message);

        void Error(string message, Exception ex);
    }
}
