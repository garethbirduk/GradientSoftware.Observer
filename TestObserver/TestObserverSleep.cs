using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;

namespace TestObserver
{
    [TestClass]
    public class TestObserverSleep
    {
        public Observer<bool> Observer { get; set; }
        public Observer<int> Observer1 { get; set; }
        public Observer<int> Observer2 { get; set; }

        [TestMethod]
        public void TestWake()
        {
            var s = Subject<bool>.Create(false);
            Assert.AreEqual(false, LocalValue);
            Observer = new Observer<bool>(s, () => OnNotify());

            Observer.Sleep();
            s.Replace(true);
            Assert.AreEqual(false, LocalValue);

            Observer.Wake();
            s.Replace(false);

            Assert.AreEqual(false, LocalValue);

            s.Replace(true);
            Assert.AreEqual(true, LocalValue);
        }

        [TestMethod]
        public void TestWakeNotify()
        {
            var s = Subject<bool>.Create(false);
            Assert.AreEqual(false, LocalValue);
            Observer = new Observer<bool>(s, () => OnNotify());

            Observer.Sleep();
            s.Replace(true);
            Assert.AreEqual(false, LocalValue);

            Observer.Wake(true);
            Assert.AreEqual(true, LocalValue);
        }

        public bool LocalValue { get; set; }

        public bool OnNotify()
        {
            LocalValue = Observer.Data;
            return true;
        }

        [TestMethod]
        public void TestSleepUntil()
        {
            var s = Subject<int>.Create(0);
            Assert.AreEqual(0, ValueTimes10);
            Observer1 = new Observer<int>(s, () => OnNotify10());

            s.Replace(2);
            Assert.AreEqual(20, ValueTimes10);

            Observer1.SleepUntil(() => Observer1.Data > 10);

            s.Replace(5);
            Assert.AreEqual(20, ValueTimes10);
            Assert.AreEqual(ObserverState.AsleepUntil, Observer1.State);

            s.Replace(10);
            Assert.AreEqual(20, ValueTimes10);
            Assert.AreEqual(ObserverState.AsleepUntil, Observer1.State);

            s.Replace(20);
            Assert.AreEqual(200, ValueTimes10);
            Assert.AreEqual(ObserverState.Awake, Observer1.State);
        }



        public int ValueTimes10 { get; set; } = 0;
        public int ValueTimes100 { get; set; } = 0;

        public bool OnNotify10()
        {
            ValueTimes10 = 10 * Observer1.Data;
            return true;
        }
        public void OnNotify10b()
        {
            ValueTimes10 = 10 * Observer1.Data;
        }

        public bool OnNotify100()
        {
            ValueTimes100 = 100 * Observer1.Data;
            return true;
        }
    }
}
