using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace FileManagment
{
    class ThreadSafeList<T> : IEnumerable<T>
    {
        private LinkedList<T> _mainList;
        private LinkedList<T> _addList;
        private LinkedList<T> _removeList;

        public ThreadSafeList()
        {
            _mainList = new LinkedList<T>();
            _addList = new LinkedList<T>();
            _removeList = new LinkedList<T>();
        }

        public int Count => _mainList.Count;

        public void Add(T value)
        {
            lock (_addList)
            {
                if (Monitor.TryEnter(this, 0))
                {
                    _mainList.AddLast(value);
                    Monitor.Exit(this);
                }
                else
                {
                    _addList.AddLast(value);
                }
            }
        }

        public void PendingAdd(T value)
        {
            lock(_addList)
                _addList.AddLast(value);
        }

        public void Remove(T value)
        {
            lock (_removeList)
            {
                if (Monitor.TryEnter(this, 0))
                {
                    _mainList.Remove(value);
                    Monitor.Exit(this);
                }
                else
                {
                    PendingRemove(value);
                }
            }
        }

        public void PendingRemove(T value)
        {
            lock (_removeList)
                _removeList.AddLast(value);
        }

        public void ApplyPendingActions()
        {
            lock (_addList)
                lock (_removeList)
                    lock (this)
                    {
                        if (_removeList.Count > 0)
                        {
                            foreach (var item in _removeList)
                            {
                                _mainList.Remove(item);
                            }
                            _removeList.Clear();
                        }

                        if (_addList.Count > 0)
                        {
                            _mainList.AddAfter(_mainList.Last, _addList.First);
                            _addList.Clear();
                        }
                    }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _mainList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            lock (_addList)
                lock (_removeList)
                    lock (this)
                    {
                        _mainList.Clear();
                        _removeList.Clear();
                        _addList.Clear();
                    }
        }
    }
}
