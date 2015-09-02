using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class NetworkCabling
    {
        [TestMethod]
        public void TestMethod1()
        {

            int nBuildings = 3;


            Point[] buildings = new Point[nBuildings];


            //buildings[0] = new Point(-5,-3);
            //buildings[1] = new Point(-9, 2);
            //buildings[2] = new Point(3, -4);


            buildings[0] = new Point(1, 2);
            buildings[1] = new Point(0, 0);
            buildings[2] = new Point(2, 2);



            int widthDistance = buildings.Max(_ => _.X) - buildings.Min(_ => _.X);


            var average = buildings.Average(_ => _.Y);

            var yPos = (long) buildings.Select(p => new Tuple<int, double>(p.Y, Math.Abs(p.Y - average))).OrderBy(_ => _.Item2).First().Item1;
            var dist = buildings.Select(p => Math.Abs((p.Y - yPos))).Sum();
            
             var totalDistance = (long)widthDistance + dist;

            Assert.AreEqual(4,totalDistance);
        }
    }
}
