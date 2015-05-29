using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Database;
using EksamensProjektS2015;
using Input;

namespace TestEksamensProjekt2015
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //bool shiftIsDown = Input.TypingKeyboard.ShiftDown(Input.Modifiers.Shift);
            //Assert.AreEqual(false, shiftIsDown);
            int i = 2 + 5;
            Assert.Equals(7, i);
        }
    }
}
