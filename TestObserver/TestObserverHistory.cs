using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System.Linq;

namespace TestObserver
{
    [TestClass]
    public class TestObserverHistory
    {
        public Observer<MyClass> Observer { get; set; }

        [TestMethod]
        public void TestItem()
        {
            var s = Subject<MyClass>.Create(null);
            Assert.IsNull(s.Data);

            s.Replace(new MyClass("Andy", 123));
            Assert.AreEqual("Andy", s.Data.Name);

            s.Undo();
            Assert.IsNull(s.Data);

            s.Replace(new MyClass("Bob", 456));
            Assert.AreEqual("Bob_456", s.Data.NameValue);

            s.Undo();
            Assert.IsNull(s.Data);

            s.Redo();
            Assert.AreEqual("Bob_456", s.Data.NameValue);
        }

        [TestMethod]
        public void TestList1()
        {
            var s = ListSubject<MyClass>.Create();
            Assert.AreEqual(0, s.Data.Count);

            s.Add(new MyClass("Bob", 123));
            Assert.AreEqual("Bob", s.Data.First().Name);
            Assert.AreEqual("Bob", s.Data.Last().Name);

            s.Add(new MyClass("Andy", 456));
            Assert.AreEqual("Bob", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

            s.Undo();
            Assert.AreEqual("Bob", s.Data.First().Name);
            Assert.AreEqual("Bob", s.Data.Last().Name);

            s.Redo();
            Assert.AreEqual("Bob", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);
        }

        [TestMethod]
        public void TestList2()
        {
            var s = ListSubject<MyClass>.Create();
            Assert.AreEqual(0, s.Data.Count);

            s.Add(new MyClass("Andy", 123));
            Assert.AreEqual(1, s.Data.Count);
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

            s.Modify(x =>
            {
                var item = x.First();
                item.Name = "Bob";
            });
            Assert.AreEqual("Bob", s.Data.First().Name);
            Assert.AreEqual("Bob", s.Data.Last().Name);

            s.Undo();
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

            s.Undo();
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

            s.Redo();
            Assert.AreEqual("Bob", s.Data.First().Name);
            Assert.AreEqual("Bob", s.Data.Last().Name);

            s.Undo();
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

            s.Add(new MyClass("Charlie", 789));
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Charlie", s.Data.Last().Name);

            s.Redo();
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Charlie", s.Data.Last().Name);

            s.Undo();
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

            s.Modify(x =>
            {
                var item = x.First();
                item.Name = "Danny";
            });
            Assert.AreEqual("Danny", s.Data.First().Name);
            Assert.AreEqual("Danny", s.Data.Last().Name);

            s.Redo();
            Assert.AreEqual("Danny", s.Data.First().Name);
            Assert.AreEqual("Danny", s.Data.Last().Name);

            s.Undo();
            Assert.AreEqual("Andy", s.Data.First().Name);
            Assert.AreEqual("Andy", s.Data.Last().Name);

        }
    }
}
