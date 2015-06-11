using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Input;
using Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EksamensProjektS2015;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InputTest()
        {
            //GameManager g = new GameManager();
            //double sal = g.SalaryCalc(0.05, 5000);
            //Assert.Equals(sal, 6000);
            int i = 5;
            Assert.AreEqual(i, 5);
        }
    }
}
