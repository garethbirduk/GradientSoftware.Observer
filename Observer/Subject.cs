using Gradient.ObjectHistory;
using System.Collections.Generic;
using System.Linq;

namespace Observer
{
    public class Subject<T> : ISubject<T>
    {
        private readonly List<Observer<T>> Observers = new();

        protected Subject(T data)
        {
            History = History<T>.Create(data, NullCondition.AllowAll, OnNullError.Ignore);
        }
        public static Subject<T> Create(T data)
        {
            var subject = new Subject<T>(data);
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

        public void Undo(bool suppressNotify = false)
        {
            History.Undo();
            if (!suppressNotify)
                NotifyObservers();
        }

        public void Redo(bool suppressNotify = false)
        {
            History.Redo();
            if (!suppressNotify)
                NotifyObservers();
        }

        public T Data
        {
            get
            {
                return History.Value;
            }
            protected set
            {
                if (History.AddValue(value))
                {
                    NotifyObservers();
                }
            }
        }

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
            var previous = History.Value;
            var copy = previous.DeepCopy();

            History.InsertBefore(copy);

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
