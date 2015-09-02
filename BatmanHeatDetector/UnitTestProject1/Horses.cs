using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class Horses
    {
        [TestMethod]
        public void TestMethod1()
        {
            //int[] horses = new[] {10, 5, 15, 17, 3, 8, 11, 28, 6, 55, 7};
            int[] horses = new[] { 3,5,8,9};

            var orderedHorses = horses.OrderByDescending(_=>_).ToArray();
            
            int delta = int.MaxValue;
            for (int i = 0; i < orderedHorses.Count()-1; i++)
            {
                delta = Math.Min(Math.Abs(orderedHorses[i] - orderedHorses[i + 1]), delta);
            }


            string s = delta.ToString();
        }
    }
}
