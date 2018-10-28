using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileManagment
{
    internal class FileSystemWatcherComposite : IDisposable
    {
        private readonly List<FileSystemWatcher> _watchers;

        private FileSystemWatcherComposite()
        {
            _watchers = new List<FileSystemWatcher>();
        }

        public FileSystemWatcherComposite(string watchedDirectoryPath)
            : this()
        {
            _watchers.Add(new FileSystemWatcher(watchedDirectoryPath));
        }

        public FileSystemWatcherComposite(IEnumerable<string> watchedDirectoryPathes)
            : this()
        {
            foreach (var directoryPath in watchedDirectoryPathes)
            {
                _watchers.Add(new FileSystemWatcher(directoryPath));
            }

            if (_watchers == null)
                throw new ArgumentException("Parameter should has at least one directory path.", nameof(watchedDirectoryPathes));
        }

        public event FileSystemEventHandler Created
        {
            add
            {
                foreach (var watcher in _watchers)
                    watcher.Created += value;
            }
            remove
            {
                foreach (var watcher in _watchers)
                    watcher.Created -= value;
            }
        }

        public bool EnableRaisingEvents
        {
            get => _watchers[0].EnableRaisingEvents;
            set
            {
                foreach (var watcher in _watchers)
                    watcher.EnableRaisingEvents = value;
            }
        }

        public IEnumerable<string> Pathes => _watchers.Select(watcher => watcher.Path);

        public void Dispose()
        {
            foreach (var watcher in _watchers)
            {
                watcher.Dispose();
            }
        }
    }
}
