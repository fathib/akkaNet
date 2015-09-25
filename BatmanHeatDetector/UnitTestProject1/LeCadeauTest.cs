using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class LeCadeauTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            //Ligne 1 : le nombre N de Oods participant au cadeau
            //Ligne 2 : le prix C du cadeau
            //N Lignes suivantes : la liste des budgets B des participants.

            List<int> input = new List<int>
            {
            //    3,
            //    100,
                40,
                40,
                40
            };

            List<int> outpout = new List<int>{33,33,34};

            Cadeau c = new Cadeau(3,input,100);
            c.Dispatch();
            var retVal = c.Print();
            Assert.AreEqual(33,retVal[0]);
            Assert.AreEqual(33, retVal[1]);
            Assert.AreEqual(34, retVal[2]);
        }



        [TestMethod]
        public void TestMethod2()
        {

            //Ligne 1 : le nombre N de Oods participant au cadeau
            //Ligne 2 : le prix C du cadeau
            //N Lignes suivantes : la liste des budgets B des participants.

            List<int> input = new List<int>
            {
                100,1,60
            };

            List<int> outpout = new List<int> { 33, 33, 34 };

            Cadeau c = new Cadeau(3, input, 100);
            c.Dispatch();
            var retVal = c.Print();
            Assert.AreEqual(1, retVal[0]);
            Assert.AreEqual(49, retVal[1]);
            Assert.AreEqual(50, retVal[2]);

        }


        [TestMethod]
        public void TestMethod3()
        {

            //Ligne 1 : le nombre N de Oods participant au cadeau
            //Ligne 2 : le prix C du cadeau
            //N Lignes suivantes : la liste des budgets B des participants.

            List<int> input = new List<int>
            {
                20,20,40
            };

            
            Cadeau c = new Cadeau(3, input, 100);
            c.Dispatch();
            var retVal = c.Print();
            Assert.IsNull(retVal);

        }
    }


    class Cadeau
    {
        private Contributor[] _contributors;
        private int _giftPrice;
        private int _totalContribution=0;
        private bool _impossible = false;

        public Cadeau(int nbOods, List<int> contributions, int giftPrice )
        {
            _giftPrice = giftPrice;
            _contributors = new Contributor[nbOods];

            for (int i = 0; i < nbOods; i++)
            {
                _contributors[i] = new Contributor(contributions[i]);
                _totalContribution += contributions[i];
            }
        }


        public void Dispatch()
        {
            
            int money = _totalContribution;
            int totalPrice = 0;
            int amountToDispatch = 1;
            while (!(money == 0 || totalPrice == _giftPrice))
            {
                foreach (var contributor in _contributors)
                {
                    if (contributor.Contribute(amountToDispatch))
                    {
                        money -= amountToDispatch;
                        totalPrice += amountToDispatch;
                    }

                    if (money == 0 || totalPrice == _giftPrice)
                        break;
                }

            }

            _impossible = (totalPrice < _giftPrice);

        }

        public List<int> Print()
        {
            if (_impossible)
                return null;
            else
                return _contributors.OrderBy(c => c.Contribution).ToList().Select(x => x.Contribution).ToList();
        } 



        
    }

    public class Contributor
    {
        private readonly int _maxContrib;

        public int Contribution { get; private set; }

        public Contributor(int maxContrib )
        {
            _maxContrib = maxContrib;
        }


        public bool Contribute(int contributionAmount)
        {
            if (_maxContrib < Contribution + contributionAmount)
            {
                return false;
            }
            else
            {
                Contribution += contributionAmount;
                return true;
            }
        }
    }


}
