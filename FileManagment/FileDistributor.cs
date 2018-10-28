using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using FileDistributor.Logger;
using FileDistributor.Logger.Resources;

namespace FileManagment
{
    public class FileDistributor : IDisposable
    {
        private const double DefaultTimerInterval = 500;

        private FileSystemWatcherComposite _watcher;

        private ThreadSafeList<string> _createdFilePathes;

        private System.Timers.Timer _tryToMoveFilesTimer;

        private FileDistributor(string defaultDirectoryPath)
        {
            DefaultDirectoryPath = defaultDirectoryPath;
            _createdFilePathes = new ThreadSafeList<string>();

            _tryToMoveFilesTimer = new System.Timers.Timer(DefaultTimerInterval);
            _tryToMoveFilesTimer.Elapsed += OnTryToMoveFiles;
            _tryToMoveFilesTimer.AutoReset = true;
        }

        public FileDistributor(string watchedDirectoryPath, string defaultDirectoryPath)
            : this(defaultDirectoryPath)
        {
            _watcher = new FileSystemWatcherComposite(watchedDirectoryPath);
            InitializeWatchers();
        }

        public FileDistributor(IEnumerable<string> watchedDirectoryPathes, string defaultDirectoryPath)
            : this(defaultDirectoryPath)
        {
            _watcher = new FileSystemWatcherComposite(watchedDirectoryPathes);
            InitializeWatchers();
            AddExistingFiles();
        }

        public ILogger Logger { get; set; }

        public string DefaultDirectoryPath { get; set; }

        public double TimerInterval
        {
            get => _tryToMoveFilesTimer.Interval;
            set => _tryToMoveFilesTimer.Interval = value;
        }

        public event Func<string, string> Mappers;

        public void Run()
        {
            Logger?.Info(LogMessages.StartWatching);
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            Logger?.Info(LogMessages.StopWatching);
            _watcher.EnableRaisingEvents = false;
        }

        public void Dispose()
        {
            _watcher.Dispose();
            _tryToMoveFilesTimer.Dispose();
        }

        private void InitializeWatchers()
        {
            _watcher.Created += OnFileCreated;
            _watcher.EnableRaisingEvents = false;
        }

        private void AddExistingFiles()
        {
            foreach (var directoryPath in _watcher.Pathes)
            {
                foreach (var file in Directory.EnumerateFiles(directoryPath))
                {
                    Logger?.Info(string.Format(LogMessages.FileCreated, file));
                    _createdFilePathes.Add(file);
                }
            }
            _tryToMoveFilesTimer.Enabled = true;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs args)
        {
            if (File.Exists(args.FullPath))
            {
                Logger?.Info(string.Format(LogMessages.FileCreated, args.FullPath));

                _createdFilePathes.Add(args.FullPath);
                _tryToMoveFilesTimer.Enabled = true;
            }
        }

        private void OnTryToMoveFiles(object sender, ElapsedEventArgs args)
        {
            try
            {
                _tryToMoveFilesTimer.Enabled = false;

                lock (_createdFilePathes)
                {
                    foreach (var createdFilePath in _createdFilePathes)
                    {
                        var destFilePath = Map(createdFilePath);
                        if (TryMoveFile(createdFilePath, destFilePath))
                        {
                            _createdFilePathes.PendingRemove(createdFilePath);
                        }
                    }
                }

                _createdFilePathes.ApplyPendingActions();
                _tryToMoveFilesTimer.Enabled = (_createdFilePathes.Count > 0);
            }
            catch (Exception ex)
            {
                Logger?.Error(LogMessages.UnexpectedError, ex);
            }
            finally
            {
                _createdFilePathes.Clear();
                AddExistingFiles();
            }
        }

        /// <returns>Value that indicate file should be removed from list or not.</returns>
        private bool TryMoveFile(string sourceFilePath, string destFilePath)
        {
            var destDirectoryPath = Path.GetDirectoryName(destFilePath);
            try
            {
                if (!Directory.Exists(destDirectoryPath))
                {
                    Directory.CreateDirectory(destDirectoryPath);
                    Logger?.Info(string.Format(LogMessages.DestinationDirectoryWasCreated, destDirectoryPath));
                }
            }
            catch (Exception)
            {
                Logger?.Error(string.Format(LogMessages.CannotCreateDirectory, destDirectoryPath));
                return true;
            }

            try
            {
                if (File.Exists(destFilePath))
                    File.Delete(destFilePath);

                File.Move(sourceFilePath, destFilePath);
                Logger?.Info(string.Format(LogMessages.FileWasMovedSuccessfully, sourceFilePath));
            }
            catch (FileNotFoundException)
            {
                Logger?.Error(string.Format(LogMessages.FileWasDeletedBeforTransfering, sourceFilePath));
            }
            catch (DirectoryNotFoundException)
            {
                Logger?.Error(string.Format(LogMessages.CannotFindDestinationDirectory, Path.GetDirectoryName(destFilePath)));
            }
            catch (IOException)  // If source file currently in use
            {
                Logger?.Error(string.Format(LogMessages.FileIsCurrentlyInUse, sourceFilePath));
                return false;
            }
            catch (Exception ex)
            {
                Logger?.Error(string.Format(LogMessages.UnexpectedErrorWithFile, sourceFilePath), ex);
            }

            return true;
        }

        private string Map(string filePath)
        {
            if (Mappers != null)
            {
                foreach (Func<string, string> mapper in Mappers.GetInvocationList())
                {
                    var destFilePath = mapper(filePath);
                    if (destFilePath != null)
                    {
                        Logger?.Info(string.Format(LogMessages.FileWillMoveUsingExistingRule, filePath, Path.GetDirectoryName(destFilePath)));
                        return destFilePath;
                    }
                }
            }

            Logger?.Info(string.Format(LogMessages.FileWillMoveToDefaultDirectory, filePath, DefaultDirectoryPath));
            return Path.Combine(DefaultDirectoryPath, Path.GetFileName(filePath));
        }
    }
}
