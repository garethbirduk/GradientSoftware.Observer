using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System;
using System.Collections.Generic;

namespace TestObserver
{ 
    [TestClass]
    public class TestModify
    {
        [TestMethod]
        public void TestInt()
        {
            var s = Subject<int>.Create(0);
            Assert.ThrowsException<NotSupportedException>(() => s.Modify(x => x = 1));
        }

        [TestMethod]
        public void TestMyClassWithHistory()
        {
            var s = Subject<MyClass>.Create(new MyClass());
            s.Modify(x => x.Name = "Bob");
            Assert.AreEqual("Bob", s.Data.Name);
        }

        [TestMethod]
        public void TestMyClassWithNoHistory()
        {
            var s = Subject<MyClass>.Create(new MyClass(), false);
            s.Modify(x => x.Name = "Bob");
            Assert.AreEqual("Bob", s.Data.Name);
        }

        [TestMethod]
        public void TestBool()
        {
            var s = Subject<bool>.Create(true);
            s.Replace(false);
            Assert.AreEqual(false, s.Data);
            s.Replace(true);
            Assert.AreEqual(true, s.Data);
            Assert.ThrowsException<NotSupportedException>(() => s.Modify(x => x = false));
        }

        [TestMethod]
        public void TestString()
        {
            var s = Subject<string>.Create("A");
            s.Replace("B");
            Assert.AreEqual("B", s.Data);
            Assert.ThrowsException<NotSupportedException>(() => s.Modify(x => x = "C"));
        }

        [TestMethod]
        public void TestIntNoHistory()
        {
            var s = Subject<int>.Create(0, false);
            Assert.ThrowsException<NotSupportedException>(() => s.Modify(x => x = 1));
        }

        [TestMethod]
        public void TestListInt()
        {
            var s = ListSubject<int>.Create();
            s.Add(0);
            Assert.AreEqual(1, s.Count);

            s.Modify(x => x.Add(1));
            Assert.AreEqual(2, s.Count);
        }

        [TestMethod]
        public void TestModifyListIntNoHistory()
        {
            var s = ListSubject<int>.Create(false);
            s.Add(0);
            Assert.AreEqual(1, s.Count);

            s.Modify(x => x.Add(1));
            Assert.AreEqual(2, s.Count);
        }

        [TestMethod]
        public void TestModifyListIntWithHistory()
        {
            var s = ListSubject<int>.Create();
            s.Add(0);
            Assert.AreEqual(1, s.Count);

            s.Modify(x => x.Add(1));
            Assert.AreEqual(2, s.Count);
        }
    }
}