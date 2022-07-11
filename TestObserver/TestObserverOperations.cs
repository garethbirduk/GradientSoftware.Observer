using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;

namespace TestObserver
{
    [TestClass]
    public class TestObserverOperations
    {
        public Observer<MyClass> Observer { get; set; }

        [TestMethod]
        public void TestModify()
        {
            var s = Subject<MyClass>.Create(new MyClass("Andy", 123));
            Observer = new Observer<MyClass>(s, () => OnNotify());
            Observer.Modify(x => x.Name = "Bob");
            Assert.AreEqual("Bob_123", NameValue);
        }

        [TestMethod]
        public void TestReplace()
        {
            var s = Subject<MyClass>.Create(new MyClass("Andy", 123));
            Observer = new Observer<MyClass>(s, () => OnNotify());
            Observer.Replace(new MyClass("Bob", 456));
            Assert.AreEqual("Bob_456", NameValue);
        }

        public string NameValue { get; set; }

        public bool OnNotify()
        {
            NameValue = Observer.Data.NameValue;
            return true;
        }
    }
}
