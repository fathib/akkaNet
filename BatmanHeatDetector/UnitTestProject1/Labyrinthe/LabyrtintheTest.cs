using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.Labyrinthe
{
    [TestClass]
    public class LabyrtintheTest
    {
        //TODO
        //Test with border
        //implement mainDirection strategy 

        [TestMethod]
        public void LoadMap()
        {
            Labyrinthe l = new Labyrinthe(3,3,10);
            l.LoadLineForWall(0,"...");
            l.LoadLineForWall(1, "...");
            l.LoadLineForWall(2, "...");

            Assert.IsNotNull(l.Map[0,0]);
            Assert.IsNotNull(l.Map[1, 1]);
            Assert.IsNotNull(l.Map[1, 2]);

        }


        [TestMethod]
        public void FillSpecificCase()
        {
            Labyrinthe l = new Labyrinthe(5, 5, 10);
            l.LoadLineForWall(0, "#####");
            l.LoadLineForWall(1, "#####");
            l.LoadLineForWall(2, "T...C");
            l.LoadLineForWall(3, "#####");
            l.LoadLineForWall(4, "#####");


            Assert.AreEqual(l.CurrentNode.Position, new Point(0,2) );
            Assert.AreEqual(l.InitialPosition.Position, new Point(0, 2));
            Assert.AreEqual(l.ControlRoom.Position, new Point(4, 2));

        }
        
        [TestMethod]
        public void AllerRetourEnLigneDroite()
        {
            Labyrinthe l = new Labyrinthe(5, 5, 10);
            l.LoadLineForWall(0, "#####");
            l.LoadLineForWall(1, "#####");
            l.LoadLineForWall(2, "T...C");
            l.LoadLineForWall(3, "#####");
            l.LoadLineForWall(4, "#####");

            var move = l.DoSomething();
            //aller
            Assert.AreEqual(move, "RIGHT");
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            //retour
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            Assert.AreEqual(l.CurrentNode.Position, new Point(0, 2));
        }


        [TestMethod]
        public void NeTombePasDansLeVide()
        {
            Labyrinthe l = new Labyrinthe(5, 5, 10);
            l.LoadLineForWall(0, "#####");
            l.LoadLineForWall(1, "#####");
            l.LoadLineForWall(2, "C...T");
            l.LoadLineForWall(3, "#####");
            l.LoadLineForWall(4, "#####");
            
            //retour
            var move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");
            move = l.DoSomething();
            Assert.AreEqual(move, "LEFT");

            //aller
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            move = l.DoSomething();
            Assert.AreEqual(move, "RIGHT");
            Assert.AreEqual(l.CurrentNode.Position, new Point(4, 2));
        }
    }
}