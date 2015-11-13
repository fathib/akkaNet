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
            Board b = new Board(2, 3);
            b.LoadLine("1.", 0);
            b.LoadLine("..", 1);
            b.LoadLine("1.", 2);

            var n = b.GetVerticalNeighbor(0, 0, +1);

            Assert.IsNotNull(n);

            Assert.AreEqual(0,n.Position.X);
            Assert.AreEqual(2, n.Position.Y);

        }
    }
}
