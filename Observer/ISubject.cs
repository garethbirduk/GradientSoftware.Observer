namespace Observer
{
    public interface ISubject<T>
    {
        void RegisterObserver(Observer<T> observer);
        void UnregisterObserver(Observer<T> observer);
        void NotifyObservers();
    }

}
