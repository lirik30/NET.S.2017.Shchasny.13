using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QueueLogic
{
    public class Queue<T> : IEnumerable<T>
    {
        #region private fields

        private T[] _arr;
        private int _capacity;
        private int _head; 
        private int _tail;
        private int _size;
        private int _version;

        #endregion

        #region prop
        
        /// <summary>
        /// Number of elements in the queue
        /// </summary>
        public int Count => _size;

        #endregion

        #region ctors

        /// <summary>
        /// Create new zero-size queue 
        /// </summary>
        public Queue() => _arr = new T[_capacity];

        /// <summary>
        /// Creates a new queue of a specific capacity
        /// </summary>
        /// <param name="capacity">Capacity of the queue</param>
        public Queue(int capacity)
        {
            _capacity = capacity;
            _arr = new T[_capacity];
        }

        /// <summary>
        /// Create new queue based on the some collection
        /// </summary>
        /// <param name="collection">Collection of the T elements</param>
        public Queue(IEnumerable<T> collection)
        {
            if(ReferenceEquals(collection, null))
                throw new ArgumentNullException($"{nameof(collection)} must be not null");

            _arr = new T[collection.Count()];
            foreach (var element in collection)
                Enqueue(element);
        }
        #endregion

        #region public methods
        
        public bool IsEmpty() => _size == 0;

        public bool Contains(T elem) => _arr.Contains(elem);

        /// <summary>
        /// Adds element in the ending of queue
        /// </summary>
        /// <param name="elem">Element for adding</param>
        public void Enqueue(T elem)
        {
            if (Count == _capacity)
                Resize();

            _arr[_tail] = elem;
            _size++;
            _version++;
            _tail = (_tail + 1) % _capacity;
        }

        /// <summary>
        /// Removes next element in the queue and returns it
        /// </summary>
        /// <returns>Removed element</returns>
        public T Dequeue()
        {
            T ret = Peek();
            _arr[_head] = default(T);
            _size--;
            _version++;
            _head = (_head + 1) % _capacity;
            return ret;
        }

        /// <summary>
        /// Returns next element in the queue but not removes it
        /// </summary>
        /// <returns>Next element</returns>
        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException();

            return _arr[_head];
        }

        #endregion

        #region private methods

        /// <summary>
        /// Resize queue when it is full
        /// </summary>
        private void Resize()
        {
            int new_capacity = _capacity == 0 ? 4 : _capacity * 2;
            T[] new_arr = new T[new_capacity];
            if (_head < _tail)
                Array.Copy(_arr, _head, new_arr, 0, Count);
            else
            {
                Array.Copy(_arr, _head, new_arr, 0, Count - _head);
                Array.Copy(_arr, 0, new_arr, Count - _head, _tail);
            }

            _capacity = new_capacity;
            _arr = new_arr;
            _head = 0;
            _tail = Count;
        }
        
        internal T GetElement(int index) => _arr[(_head + index) % _capacity];

        internal int GetVersion() => _version;
        #endregion

        #region GetEnumeratorы

        public IEnumerator<T> GetEnumerator()
        {
            return new QueueIterator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Enumerator

        private struct QueueIterator : IEnumerator<T>
        {
            private readonly Queue<T> _queue;
            private int _currIndex;
            private int _version;
            private T _currElem; // -1 before start; -2 if ended

            public QueueIterator(Queue<T> queue)
            {
                _queue = queue;
                _currIndex = -1;
                _version = queue.GetVersion();
                _currElem = default(T);
            }

            public void Dispose() { }

            public bool MoveNext()
            {
                if(_version != _queue.GetVersion()) throw new InvalidOperationException("The collection was changed. Iterator is not valid.");

                if (_currIndex == -2)
                    return false;

                _currIndex++;

                if (_currIndex == _queue.Count)
                {
                    _currIndex = -2;
                    _currElem = default(T);
                    return false;
                }

                _currElem = _queue.GetElement(_currIndex);

                return true;
            }

            public void Reset()
            {
                if (_version != _queue.GetVersion()) throw new InvalidOperationException("The collection was changed. Iterator is not valid.");

                _currIndex = -1;
                _currElem = default(T);
            }

            public T Current
            {
                get
                {
                    if (_currIndex < 0)
                        throw new InvalidOperationException();
                    return _currElem;
                }
            }

            object IEnumerator.Current => Current;
        }

        #endregion
    }
}
