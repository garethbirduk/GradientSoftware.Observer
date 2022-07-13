using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Observer
{
    public class ListSubject<T> : Subject<List<T>>, IList<T>
    {
        protected ListSubject(List<T> data, bool withHistory) : base(data, withHistory) { }

        public static ListSubject<T> Create(bool withHistory = true)
        {
            return new ListSubject<T>(new List<T>(), withHistory);
        }

        public T this[int index]
        {
            get
            {
                return Data[index];
            }
            set
            {
                Modify(x => x[index] = value);
            }
        }

        public int Count
        {
            get
            {
                return Data.Count;
            }
        }


        public void Add(T item)
        {
            if (History != null)
            {
                var previous = History.Value.ToList();
                var last = previous.LastOrDefault();
                if (last != null && !last.Equals(item))
                {
                    var copy = previous.DeepCopy();
                    History.InsertBefore(copy);
                }
            }
            Data.Add(item);
            NotifyObservers();
        }

        /// <summary>
        /// Adds a range of values. Will notify zero or one time according to SubjectState.
        /// </summary>
        /// <param name="list"></param>
        public void AddRange(IEnumerable<T> list)
        {
            var state = State;
            if (state == SubjectState.Notifying)
                ToggleState();
            foreach (var item in list)
                Add(item);
            if (state == SubjectState.Notifying)
                ToggleState(true);
        }


        /// <summary>
        /// Replaces list with a range of values. Will notify zero or one time according to SubjectState.
        /// </summary>
        /// <param name="list"></param>
        public void Replace(IEnumerable<T> list)
        {
            var state = State;
            if (state == SubjectState.Notifying)
                ToggleState();
            Clear();
            foreach (var item in list)
                Add(item);
            if (state == SubjectState.Notifying)
                ToggleState(true);
        }

        public void Clear()
        {
            Modify(x => x.Clear());
        }

        public bool Contains(T item)
        {
            return Data.Contains(item);
        }

        public int IndexOf(T item)
        {
            if (History != null)
                return History.Value.IndexOf(item);
            return Singleton.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Modify(x => x.Insert(index, item));
        }

        public bool Remove(T item)
        {
            var result = Data.Remove(item);
            if (result)
                NotifyObservers();
            return result;
        }

        public void RemoveAt(int index)
        {
            Modify(x => x.RemoveAt(index));
        }


        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }


        public override void Modify(ModifyDelegate modifyDelegate)
        {
            if (History != null)
            {
                var previous = History.Value.ToList();
                var copy = previous.DeepCopy();
                History.InsertBefore(copy);
            }
            modifyDelegate(Data);
            NotifyObservers();
        }

        public void CopyTo(T[] array, int index)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

}
