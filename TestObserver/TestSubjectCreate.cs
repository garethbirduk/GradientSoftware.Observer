using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System;

namespace TestObserver
{
    [TestClass]
    public class TestSubjectCreate
    {
        [TestMethod]
        public void TestCreateWithoutHistory()
        {
            var s = Subject<int>.Create(0, false);
            s.Replace(1);
            Assert.AreEqual(1, s.Data);
            s.Replace(2);
            Assert.AreEqual(2, s.Data);
        }

        [TestMethod]
        public void TestSingletonUndoException()
        {
            var s = Subject<int>.Create(0, false);
            s.Replace(1);
            Assert.ThrowsException<NotSupportedException>(() => s.Undo());
        }

        [TestMethod]
        public void TestSingletonRedoException()
        {
            var s = Subject<int>.Create(0, false);
            s.Replace(1);
            Assert.ThrowsException<NotSupportedException>(() => s.Redo());
        }

        [TestMethod]
        public void TestCreateWithHistory()
        {
            var s = Subject<int>.Create(0);
            s.Replace(1);
            Assert.AreEqual(1, s.Data);
            s.Replace(2);
            Assert.AreEqual(2, s.Data);
            s.Undo();
            Assert.AreEqual(1, s.Data);
        }
    }
}
