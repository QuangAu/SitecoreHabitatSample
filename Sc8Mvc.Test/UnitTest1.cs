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
    }
}
