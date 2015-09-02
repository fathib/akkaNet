using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class SuiteDeConway
    {
        [TestMethod]
        public void TestMethod1()
        {


            int initialValue = 1;
            int depth = 1;

            string chain = GetConwaySuite(initialValue, depth);

            Assert.AreEqual("1 1", chain);


        }


        [TestMethod]
        public void TestMethod2()
        {


            int initialValue = 1;
            int depth = 2;

            string chain = GetConwaySuite(initialValue, depth);

            Assert.AreEqual("2 1", chain);


        }


        [TestMethod]
        public void TestMethod3()
        {


            int initialValue = 1;
            int depth = 3;

            string chain = GetConwaySuite(initialValue, depth);

            Assert.AreEqual("1 2 1 1", chain);


        }


        [TestMethod]
        public void TestMethod4()
        {


            int initialValue = 1;
            int depth = 10;

            string chain = GetConwaySuite(initialValue, depth);

            Assert.AreEqual("1 1 1 3 1 2 2 1 1 3 3 1 1 2 1 3 2 1 1 3 2 1 2 2 2 1", chain);


        }

        


        public string GetConwaySuite(int initialValue, int depth)
        {
            string chain = initialValue.ToString();
            for (int i = 0; i < depth-1; i++)
            {
                chain = CalculateNextLine(chain);
            }

            return chain;
        }

        public string CalculateNextLine(string chain)
        {
            int count = 0;
            string value = "";
            string retVal =String.Empty;
            var values = chain.Split(' ');
            foreach (var c in values)
            {
                if (c == value)
                    count++;
                else
                {
                    if(value!= String.Empty)
                        retVal += count + " " + value+" ";

                    count = 1;
                    value = c;
                }
            }

            retVal += count + " " + value;


            return retVal;

        }

    }
}
