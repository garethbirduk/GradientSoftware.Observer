using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;

namespace TestObserver
{
    [TestClass]
    public class TestSubjectStates
    {
        public Observer<int> Observer { get; set; }

        [TestMethod]
        public void TestToggleState()
        {
            var s = Subject<int>.Create(0);
            Assert.AreEqual(0, ValueTimes10);
            Observer = new Observer<int>(s, () => OnNotify10());

            // notifying
            s.Replace(1);
            Assert.AreEqual(10, ValueTimes10);

            s.ToggleState();
            // not notifying
            s.Replace(2);
            Assert.AreEqual(10, ValueTimes10);

            s.ToggleState();
            // notifying
            Assert.AreEqual(10, ValueTimes10);

            s.Replace(4);
            Assert.AreEqual(40, ValueTimes10);

            s.ToggleState();
            // not notifying
            s.Replace(5);
            Assert.AreEqual(40, ValueTimes10);

            s.ToggleState(true);
            // notifying
            Assert.AreEqual(50, ValueTimes10);

        }

        public int ValueTimes10 { get; set; } = 0;
        
        public void OnNotify10()
        {
            ValueTimes10 = 10 * Observer.Data;
        }

    }
}
