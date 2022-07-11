using System;

namespace Observer
{

    /// <summary>
    /// Observer of a subject whose value is of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Observer<T>
    {
        /// <summary>
        /// The state of the observer.
        /// </summary>
        public ObserverState State { get; set; } = ObserverState.Awake;
        public Func<bool> SleepUntilCondition { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject">The existing subject to observe.</param>
        /// <param name="onNotify">The callback method on notification.</param>
        public Observer(Subject<T> subject, Action onNotify)
        {
            Subject = subject;
            Subject.RegisterObserver(this);
            OnNotify = onNotify;
        }
        /// <summary>
        /// The subject
        /// </summary>
        private Subject<T> Subject { get; set; }

        /// <summary>
        /// The callback method on notification. 
        /// </summary>
        public Action OnNotify { get; set; }

        /// <summary>
        /// Wake up the observer; the observer can now receive notifications. Returns the subject value, which can be optionally updated first.
        /// </summary>
        /// <param name="update">optional: call the observer's own OnNotify method to get the latest subject value.</param>
        /// <returns>The subject value</returns>
        public T Wake(bool update = false)
        {
            State = ObserverState.Awake;
            if (update)
            {
                OnNotify();
            }
            return Data;
        }

        /// <summary>
        /// Set the observer to sleep; the observer no longer receives notifications.
        /// </summary>
        public void Sleep()
        {
            State = ObserverState.Asleep;
        }

        /// <summary>
        /// Set the observer to sleep; the observer no longer receives notifications.
        /// </summary>
        public void SleepUntil(Func<bool> sleepUntilCondition)
        {
            State = ObserverState.AsleepUntil;
            SleepUntilCondition = sleepUntilCondition;
        }

        /// <summary>
        /// Get the value of this observer's subject.
        /// </summary>
        public T Data => Subject.Data;

        /// <summary>
        /// Delegate method to allow the observer's subject to be modified via the observer.
        /// </summary>
        /// <param name="modifyDelegate"></param>
        public void Modify(Subject<T>.ModifyDelegate modifyDelegate)
        {
            Subject.Modify(modifyDelegate);
        }

        /// <summary>
        /// Replace the subject's contents with this item
        /// </summary>
        /// <param name="item"></param>
        public void Replace(T item)
        {
            Subject.Replace(item);
        }
    }

}
