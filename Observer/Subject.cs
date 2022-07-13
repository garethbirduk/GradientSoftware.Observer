using Gradient.ObjectHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Observer
{
    public class Subject<T> : ISubject<T>
    {
        private readonly List<Observer<T>> Observers = new();

        protected Subject(T data, bool withHistory)
        {
            if (data != null && data.GetType() != typeof(string))
                IsClassOrString = data.GetType().IsClass;

            if (withHistory)
                History = History<T>.Create(data, NullCondition.AllowAll, OnNullError.Ignore);
            else
                Singleton = data;
        }
        public static Subject<T> Create(T data, bool withHistory = true)
        {
            var subject = new Subject<T>(data, withHistory);
            return subject;
        }

        public SubjectState State { get; protected set; } = SubjectState.Notifying;

        internal protected void ToggleState(bool notify = false)
        {
            if (State == SubjectState.Notifying)
                State = SubjectState.NotNotifying;
            else
            {
                State = SubjectState.Notifying;
                if (notify)
                    NotifyObservers();
            }
        }

        protected History<T> History { get; set; }
        protected T Singleton { get; set; }

        public void Undo(bool suppressNotify = false)
        {
            if (History == null)
                throw new NotSupportedException("Cannot undo when there is no history");
            History.Undo();
            if (!suppressNotify)
                NotifyObservers();
        }

        public void Redo(bool suppressNotify = false)
        {
            if (History == null)
                throw new NotSupportedException("Cannot redo when there is no history");
            History.Redo();
            if (!suppressNotify)
                NotifyObservers();
        }

        public T Data
        {
            get
            {
                if (History == null)
                    return Singleton;
                return History.Value;
            }
            protected set
            {
                if (History == null)
                {
                    if (Singleton.Equals(value))
                        return;
                    Singleton = value;
                    NotifyObservers();
                }
                else if (History.AddValue(value))
                {
                    NotifyObservers();
                }
            }
        }

        public bool IsClassOrString { get; }

        public void RegisterObserver(Observer<T> observer)
        {
            Observers.Add(observer);
        }

        public void UnregisterObserver(Observer<T> observer)
        {
            Observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            if (State == SubjectState.Notifying)
            {
                foreach (var observer in Observers.Where(x => x.State == ObserverState.Awake))
                {
                    observer.OnNotify();
                }

                foreach (var observer in Observers.Where(x => x.State == ObserverState.AsleepUntil && x.SleepUntilCondition()))
                {
                    observer.State = ObserverState.Awake;
                    observer.OnNotify();
                }
            }
        }

        public virtual void Modify(ModifyDelegate modifyDelegate)
        {
            if (!IsClassOrString)
            {
                throw new NotSupportedException($"Cannot use Modify({modifyDelegate} if subject is not a class. Use Replace() instead");
            }

            if (History != null)
            {
                var previous = History.Value;
                var copy = previous.DeepCopy();
                History.InsertBefore(copy);
            }

            modifyDelegate(Data);
            NotifyObservers();
        }

        public void Replace(T item)
        {
            Data = item;
        }

        public delegate void ModifyDelegate(T entity);
    }
}
