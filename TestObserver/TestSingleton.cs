using Microsoft.VisualStudio.TestTools.UnitTesting;
using Observer;
using System.Linq;
using System.Collections.Generic;
using System;

namespace TestObserver
{
    [TestClass]
    public class TestSingleton
    {
        [TestMethod]
        public void TestSameNull()
        {
            var s = Subject<MyClass>.Create(null);
            Assert.IsNull(s.Data);
            s.Replace(null);
        }

        [TestMethod]
        public void TestSame()
        {
            var myClass = new MyClass();
            var s = Subject<MyClass>.Create(myClass);
            Assert.IsNotNull(s.Data);
            s.Replace(myClass);
            Assert.IsNotNull(s.Data);
        }

        [TestMethod]
        public void TestNotSame()
        {
            var s = Subject<MyClass>.Create(new MyClass());
            s.Replace(new MyClass());
            Assert.IsNotNull(s.Data);
        }
    }
}