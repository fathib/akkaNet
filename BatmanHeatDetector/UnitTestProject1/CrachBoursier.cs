using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class CrachBoursier
    {
        [TestMethod]
        public void TestMethod1()
        {

            int lenght = 5;
            string vs = "1 2 4 4 5";


            int[] values = new int[lenght];


            var tab = vs.Split(' ');
            for (int i = 0; i < tab.Length; i++)
            {
                var v = tab[i];
                values[i] = int.Parse(tab[i]);
            }


            int result = 0;
            int delta = 0;
            int refPos = 0;

            List<Element> sommets = new List<Element>();
            for (int i = 0; i < lenght; i++)
            {
                if (IsSommet(i, values))
                {

                    if (sommets.Count==0 || sommets[sommets.Count - 1].Value < values[i])
                    {
                        sommets.Add(new Element(i, 0, values[i]));
                    }
                }
                else
                {
                    int value = values[i];
                    sommets.ForEach(k =>
                    {
                        if (k.Value - value > k.Delta)
                        {
                            k.Delta = k.Value - value;
                        }
                    });
                }
            }

            if(sommets.Count>0)
                result = -sommets.Max(s => s.Delta);


           Assert.AreEqual(0,result);

        }


       
        public bool IsSommet(int i, int[] values)
        {
            bool retVal = false;
            if (i == 0 && values[i + 1] < values[i])
                retVal = true;
            else if (i > 0 && i <values.Length-1 && values[i - 1] < values[i] && values[i + 1] < values[i])
                retVal = true;

            return retVal;
        }
    }

    public class Element
    {

        public Element(int index, int delta, int value)
        {
            Index = index;
            Delta = delta;
            Value = value;
        }


        public int Value { get; set; }
        public int Index { get; set; }
        public int Delta { get; set; }

    }
}
