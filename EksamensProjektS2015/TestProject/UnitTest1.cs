using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EksamensProjektS2015;
using Database;
using Input;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ButtonClick()
        {
            EksamensProjektS2015.Button b = new EksamensProjektS2015.Button();

            b.Clicked = false;

            bool result = b.Clicked;

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ButtonClick()
        {
            EksamensProjektS2015.Button b = new EksamensProjektS2015.Button();

            b.Pressed = false;

            bool result = b.Clicked;

            Assert.AreEqual(true, result);
        }

    }
}
