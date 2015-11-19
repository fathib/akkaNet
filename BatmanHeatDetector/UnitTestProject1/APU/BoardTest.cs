using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.APU
{
    [TestClass]
    public class BoardTest
    {
        

        [TestMethod]
        public void Cell_Should_Have_One_Vertical_Neighbor()
        {
            Board b = new Board(2, 2);
            b.LoadLine("1.", 0);
            b.LoadLine("1.", 1);

            var n  = b.GetVerticalNeighbor(0, 0, +1);
            Assert.IsNotNull(n);


            Assert.AreEqual(0, n.Position.X);
            Assert.AreEqual(1, n.Position.Y);
        }


        [TestMethod]
        public void Cell_Should_Have_One_Vertical_Neighbor_With_Blanc()
        {
            Board b = new Board(4, 2);
            b.LoadLine(".4.4", 0);
            b.LoadLine("14.3", 1);

            

        }


        [TestMethod]
        public void SimpleCase()
        {
            Board b = new Board(2, 2);
            b.LoadLine("2.", 0);
            b.LoadLine("42", 1);

            b.DoSomething();

        }



        [TestMethod]
        public void Case_To_Check()
        {
            Board b = new Board(2, 2);
            b.LoadLine("1.", 0);
            b.LoadLine("1.", 1);

            var n = b.GetVerticalNeighbor(0, 0, +1);
            Assert.IsNotNull(n);


            Assert.AreEqual(0, n.Position.X);
            Assert.AreEqual(1, n.Position.Y);
        }



        [TestMethod]
        public void Case_Intermediaire_1()
        {
            Board b = new Board(5, 5);
            b.LoadLine("4.544", 0);
            b.LoadLine(".2...", 1);
            b.LoadLine("..5.4", 2);
            b.LoadLine("..5.4", 3);
            b.LoadLine("332..", 4);

            b.DoSomething();
            
        }


    }
}
