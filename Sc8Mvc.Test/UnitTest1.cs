using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sc8MVC;

namespace Sc8Mvc.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod, Timeout(2000)]
        public void TestMethod1()
        {
            DuplicateItems item = new DuplicateItems();
            item.TestThis();
            Assert.AreEqual("abc", "abc", "test message");
        }

        [TestMethod, Timeout(500)]
        public void TestDevide()
        {
            DuplicateItems item = new DuplicateItems();
            double a = item.divide(5, 5);
            Assert.AreEqual(1, a, "test devide message");
        }

        [TestMethod, Timeout(500)]
        public void TestDothing()
        {
            DuplicateItems item = new DuplicateItems();
            item.doTheThing();
            Assert.AreEqual(1, 1, "test devide message");
        }
    }
}
