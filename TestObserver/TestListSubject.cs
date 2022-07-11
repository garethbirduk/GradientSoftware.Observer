using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TestObserver
{
    [TestClass]
    public class TestListSubject
    {
        public Observer<List<int>> Observer { get; set; }

        [TestMethod]
        public void TestAdd()
        {
            var s = ListSubject<int>.Create();
            Assert.AreEqual(0, s.Data.Count);
            Assert.AreEqual(-1, Count);

            Observer = new Observer<List<int>>(s, () => OnNotifyGetCount());
            s.Add(1);
            Assert.AreEqual(1, Count);
        }


        [TestMethod]
        public void TestAddRangeNotifying()
        {
            var s = ListSubject<int>.Create();
            Assert.AreEqual(-1, Count);

            Observer = new Observer<List<int>>(s, () => OnNotifyIncrement());

            s.AddRange(new List<int>() { 1, 2, 3 });
            Assert.AreEqual(1, Total);

            s.Add(4);
            Assert.AreEqual(2, Total);

            Assert.AreEqual(4, s.Count);

        }

        [TestMethod]
        public void TestAddRangeNotNotifying()
        {
            var s = ListSubject<int>.Create();
            Assert.AreEqual(-1, Count);

            s.ToggleState();

            Observer = new Observer<List<int>>(s, () => OnNotifyIncrement());

            s.AddRange(new List<int>() { 1, 2, 3 });
            Assert.AreEqual(0, Total);

            s.Add(4);
            Assert.AreEqual(0, Total);

            Assert.AreEqual(4, s.Count);

        }

        [TestMethod]
        public void TestReplaceNotifying()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 10, 11, 12, 13, 14, 15 });
            Assert.AreEqual(-1, Count);

            Observer = new Observer<List<int>>(s, () => OnNotifyIncrement());

            s.Replace(new List<int>() { 1, 2, 3 });
            Assert.AreEqual(1, Total);

            s.Add(4);
            Assert.AreEqual(2, Total);

            Assert.AreEqual(4, s.Count);
        }

        [TestMethod]
        public void TestReplaceNotNotifying()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 10, 11, 12, 13, 14, 15 });
            Assert.AreEqual(-1, Count);

            s.ToggleState();

            Observer = new Observer<List<int>>(s, () => OnNotifyIncrement());

            s.Replace(new List<int>() { 1, 2, 3 });
            Assert.AreEqual(0, Total);

            s.Add(4);
            Assert.AreEqual(0, Total);

            Assert.AreEqual(4, s.Count);
        }


        [TestMethod]
        public void TestGetArrayReference()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });

            Assert.AreEqual(0, s[0]);
            Assert.AreEqual(1, s[1]);
            Assert.AreEqual(2, s[2]);
        }

        [TestMethod]
        public void TestSetArrayReference()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });


            Observer = new Observer<List<int>>(s, () => OnNotifyGetValueAt(1));

            s[1] = 100;
            Assert.AreEqual(100, ValueAt1);
        }


        [TestMethod]
        public void TestClear()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });

            Observer = new Observer<List<int>>(s, () => OnNotifyGetCount());
            Observer.OnNotify();
            Assert.AreEqual(3, Count);
            s.Clear();
            Assert.AreEqual(0, Count);
        }

        [TestMethod]
        public void TestCount()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0 });


            Assert.AreEqual(1, s.Count);
            s.Clear();
            Assert.AreEqual(0, s.Count);
        }

        [TestMethod]
        public void TestContains()
        {
            var item1 = new MyClass();
            var item2 = new MyClass();
            var s = ListSubject<MyClass>.Create();
            s.Add(item1);

            Assert.IsTrue(s.Contains(item1));
            Assert.IsFalse(s.Contains(item2));
        }


        [TestMethod]
        public void TestIndexOf()
        {
            var item1 = new MyClass("Andy", 123);
            var item2 = new MyClass("Bob", 456);

            var s = ListSubject<MyClass>.Create();
            s.Add(item1);
            Assert.AreEqual(0, s.IndexOf(item1));

            s.Add(item2);
            Assert.AreEqual(0, s.IndexOf(item1));
            Assert.AreEqual(1, s.IndexOf(item2));
        }

        [TestMethod]
        public void TestRemove()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });

            Observer = new Observer<List<int>>(s, () => OnNotifyGetCount());
            Observer.OnNotify();
            Assert.AreEqual(3, Count);
            s.Remove(s[1]);
            Assert.AreEqual(2, Count);
            Assert.AreEqual(0, Observer.Data[0]);
            Assert.AreEqual(2, Observer.Data[1]);
        }

        [TestMethod]
        public void TestRemoveAt()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });

            Observer = new Observer<List<int>>(s, () => OnNotifyGetCount());
            Observer.OnNotify();
            Assert.AreEqual(3, Count);
            s.RemoveAt(1);
            Assert.AreEqual(2, Count);
            Assert.AreEqual(0, Observer.Data[0]);
            Assert.AreEqual(2, Observer.Data[1]);
        }

        [TestMethod]
        public void TestInsert()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });

            Observer = new Observer<List<int>>(s, () => OnNotifyGetCount());
            Observer.OnNotify();
            Assert.AreEqual(3, Count);
            s.Insert(1, 5);
            Assert.AreEqual(4, Count);
            Assert.AreEqual(0, Observer.Data[0]);
            Assert.AreEqual(5, Observer.Data[1]);
            Assert.AreEqual(1, Observer.Data[2]);
            Assert.AreEqual(2, Observer.Data[3]);
        }

        [TestMethod]
        public void TestFirst()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 10, 11, 12, 13, 14, 15 });
            Assert.AreEqual(10, s.First());
            Assert.AreEqual(15, s.Last());
        }

        [TestMethod]
        public void TestLast()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 10, 11, 12, 13, 14, 15 });
            Assert.AreEqual(15, s.Last());
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            var s = ListSubject<int>.Create();
            Assert.IsNotNull(s.GetEnumerator());
        }


        private int Count { get; set; } = -1;

        private int ValueAt1 { get; set; } = -1;

        private int Total { get; set; } = 0;
        public void OnNotifyGetCount()
        {
            Count = Observer.Data.Count;
        }

        public void OnNotifyIncrement()
        {
            Total += 1;
        }

        public void OnNotifyGetValueAt(int index)
        {
            ValueAt1 = Observer.Data[index];
        }

        [TestMethod]
        public void TestCopyToArray()
        {
            var s = ListSubject<int>.Create();
            s.AddRange(new List<int>() { 0, 1, 2 });
            int[] array = null;
            Assert.ThrowsException<NotImplementedException>(() => s.CopyTo(array, 0));
        }
    }
}