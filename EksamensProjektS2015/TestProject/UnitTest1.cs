using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Input;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EksamensProjektS2015;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        /*[TestMethod]
        public void ButtonClick()
        {
            EksamensProjektS2015.Button b = new EksamensProjektS2015.Button();
            
            b.Clicked = false;

            bool result = b.Clicked;

            Assert.AreEqual(false, result);
        }*/
        [TestMethod]
        public void InputTest()
        {
            bool shiftIsDown = Input.TypingKeyboard.ShiftDown(Input.Modifiers.Shift);
            Assert.AreEqual(false, shiftIsDown);
        }
    }
}
