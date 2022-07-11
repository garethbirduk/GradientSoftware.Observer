namespace Observer
{
    /// <summary>
    /// Observer states.
    /// </summary>
    public enum ObserverState
    {
        /// <summary>
        /// The observer can be notified.
        /// </summary>
        Awake,

        /// <summary>
        /// The observer cannot be notified.
        /// </summary>
        Asleep,

        /// <summary>
        /// The observer cannot be notified until some prescribed condition is met.
        /// </summary>
        AsleepUntil
    }

}
