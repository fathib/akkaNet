using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.GreatEscape
{
    [TestClass]
    public class GreatEscapeTest
    {
        [TestMethod]
        public void NoWall_Next_Move_Should_Be_Right()
        {
            GreatEscapeGame game = new GreatEscapeGame(9, 9, 2, 0);

            game.LoadPlayersPositions(0,4,0);

            var order = game.GetNextOrder();
            Assert.AreEqual("RIGHT",order);
            
        }


        [TestMethod]
        public void Wall_Next_Move_Should_Be_Top_or_Down()
        {
            GreatEscapeGame game = new GreatEscapeGame(9, 9, 2, 0);
            game.LoadWall(1,4,"V");
            game.LoadPlayersPositions(0, 4, 0);

            var order = game.GetNextOrder();
            Assert.AreNotEqual("RIGHT", order);
            Assert.AreNotEqual("LEFT", order);
        }


        [TestMethod]
        public void Wall_should_be_esquived()
        {
            GreatEscapeGame game = new GreatEscapeGame(9, 9, 2, 0);

            game.LoadPlayersPositions(4, 7, 0);
            game.LoadWall(5, 7, "V");
            var order = game.GetNextOrder();
            Assert.AreNotEqual("RIGHT", order);

        }
    }
}
