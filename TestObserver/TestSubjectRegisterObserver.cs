using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;

namespace TestObserver
{
    [TestClass]
    public class TestSubjectRegisterObserver
    {
        public Observer<int> Observer1 { get; set; }
        public Observer<int> Observer2 { get; set; }

        [TestMethod]
        public void TestInt1()
        {
            var s = Subject<int>.Create(0);
            Assert.AreEqual(0, ValueTimes10);
            Assert.AreEqual(0, ValueTimes100);
            Observer1 = new Observer<int>(s, () => OnNotify10());
            Observer2 = new Observer<int>(s, () => OnNotify100());

            s.Replace(1);
            Assert.AreEqual(10, ValueTimes10);
            Assert.AreEqual(100, ValueTimes100);

            s.UnregisterObserver(Observer2);
            s.Replace(2);
            Assert.AreEqual(20, ValueTimes10);
            Assert.AreEqual(100, ValueTimes100);

            s.RegisterObserver(Observer2);
            s.Replace(3);
            Assert.AreEqual(30, ValueTimes10);
            Assert.AreEqual(300, ValueTimes100);

        }

        public int ValueTimes10 { get; set; } = 0;
        public int ValueTimes100 { get; set; } = 0;

        public bool OnNotify10()
        {
            ValueTimes10 = 10 * Observer1.Data;
            return true;
        }
        public void OnNotify100()
        {
            ValueTimes100 = 100 * Observer2.Data;
        }
    }
}
