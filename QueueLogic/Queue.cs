using System;
using System.Collections;
using System.Collections.Generic;

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

        #endregion

        #region prop
        
        public int Count => _size;

        #endregion

        #region ctors

        public Queue() => _arr = new T[_capacity];

        public Queue(int capacity)
        {
            _capacity = capacity;
            _arr = new T[_capacity];
        }

        #endregion

        #region public methods

        public bool IsEmpty() => _tail == _head;

        public void Enqueue(T elem)
        {
            if (Count == _capacity)
            {
                //copy in new array 
                //Array.Resize(ref _arr, _capacity == 0 ? 4 : _capacity * 2);
                //_capacity = _capacity == 0 ? 4 : _capacity * 2;
                //_tail = _head + Count;
                throw new InvalidOperationException();
            }

            _arr[_tail] = elem;
            _size++;
            _tail = (_tail + 1) % _capacity;
        }

        public T Dequeue()
        {
            T ret = Peek();
            _arr[_head] = default(T);
            _size--;
            _head = (_head + 1) % _capacity;
            return ret;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new InvalidOperationException();

            return _arr[_head];
        }

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

        internal T GetElement(int index)
        {
            return _arr[(_head + index) % _capacity];
        }

        #region Enumerator

        private struct QueueIterator : IEnumerator<T>
        {
            private readonly Queue<T> _queue;
            private int _currIndex;
            private T _currElem; // -1 before start; -2 if ended

            public QueueIterator(Queue<T> queue)
            {
                _queue = queue;
                _currIndex = -1;
                _currElem = default(T);
            }

            public void Dispose() { }

            public bool MoveNext()
            {
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
