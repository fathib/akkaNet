using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class CalculMayaTest
    {
        [TestMethod]
        public void TestMethod_Load()
        {

            var refData= new List<string>();
            var chiffreA = new List<string>();
            List<string> chiffreB = new List<string>();
            List<string> result = new List<string>();
            int width = 4;
            int lenght = 4;
            refData.Add(".oo.o...oo..ooo.oooo....o...oo..ooo.oooo____o...oo..ooo.oooo____o...oo..ooo.oooo");
            refData.Add("o..o................____________________________________________________________");
            refData.Add(".oo.........................................____________________________________");
            refData.Add("................................................................________________");
          
            //16
            chiffreA.Add("o...");
            chiffreA.Add("....");
            chiffreA.Add("....");
            chiffreA.Add("....");//1

            chiffreA.Add("____");
            chiffreA.Add("____");
            chiffreA.Add("....");
            chiffreA.Add("....");

            chiffreA.Add("oo..");
            chiffreA.Add("____");
            chiffreA.Add("____");
            chiffreA.Add("____");

            chiffreA.Add("....");
            chiffreA.Add("____");
            chiffreA.Add("....");
            chiffreA.Add("....");
            //4");
           


            chiffreB.Add("oooo");
            chiffreB.Add("....");
            chiffreB.Add("....");
            chiffreB.Add("....");
            chiffreB.Add("ooo.");
            chiffreB.Add("____");
            chiffreB.Add("____");
            chiffreB.Add("____");
            chiffreB.Add("oo..");
            chiffreB.Add("____");
            chiffreB.Add("____");
            chiffreB.Add("....");
            chiffreB.Add("____");
            chiffreB.Add("____");
            chiffreB.Add("....");
            chiffreB.Add("....");
            chiffreB.Add("oo..");
            chiffreB.Add("____");
            chiffreB.Add("____");
            chiffreB.Add("....");


            result.Add("oo..");
            result.Add("____");
            result.Add("....");
            result.Add("....");

            result.Add("oo..");
            result.Add("____");
            result.Add("____");
            result.Add("....");

            result.Add("ooo.");
            result.Add("....");
            result.Add("....");
            result.Add("....");

            result.Add("oo..");
            result.Add("____");
            result.Add("____");
            result.Add("____");

            result.Add("oooo");
            result.Add("....");
            result.Add("....");
            result.Add("....");

            result.Add("oo..");
            result.Add("....");
            result.Add("....");
            result.Add("....");

            result.Add("oo..");
            result.Add("____");
            result.Add("____");
            result.Add("____");

            result.Add(".oo.");
            result.Add("o..o");
            result.Add(".oo.");
            result.Add("....");
            //+");

            CalculMaya cm = new CalculMaya(width, lenght);
            cm.LoadDictionnary(refData);
            var va = cm.LoadValues(chiffreA);
            var vb = cm.LoadValues(chiffreB);
            var vt = va*vb;
            var vr = cm.LoadValues(result);
            var chiffreBInInt = cm.ExecuteOperation("*", va, vb);

            var mayaCodeInt = cm.EncodeToInteger(chiffreBInInt);

            List<string> mayaCode = cm.EncodeToMayaCode(mayaCodeInt);


            Assert.AreEqual(mayaCode[0],result[0]);
            Assert.AreEqual(mayaCode[1],result[1]);
            Assert.AreEqual(mayaCode[2],result[2]);
        }

        [TestMethod]
        public void TestEncode_45()
        {
            int a =45;

            CalculMaya cm = new CalculMaya(4,4);

            var retVal = cm.EncodeToInteger(45);
            Assert.AreEqual(retVal[0], 2);
            Assert.AreEqual(retVal[1],5);
            

        }

        [TestMethod]
        public void Test_Encode_4805()
        {
            int a = 4805;

            CalculMaya cm = new CalculMaya(4, 4);

            var retVal = cm.EncodeToInteger(a);
            Assert.AreEqual(retVal[0], 12);
            Assert.AreEqual(retVal[1], 0);
            Assert.AreEqual(retVal[2], 5);

        }



        [TestMethod]
        public void TestPuissanceMax4805__renvoi_2()
        {
            int a = 4805;

            
            var retVal = CalculMaya.PuissanceMax(a,20);
            Assert.AreEqual(retVal, 2);
            
        }


        [TestMethod]
        public void TestPuissanceMax_5_renvoi_0()
        {
            int a = 5;


            var retVal = CalculMaya.PuissanceMax(a, 20);
            Assert.AreEqual(retVal, 0);

        }


        [TestMethod]
        public void TestPuissanceMax_45_renvoi_1()
        {
            int a = 45;


            var retVal = CalculMaya.PuissanceMax(a, 20);
            Assert.AreEqual(retVal, 1);

        }

    }



    public class CalculMaya
    {
        private readonly int _width;
        private readonly int _lenght;
        private readonly int _nbChiffre = 20;
        private readonly ChiffreMaya[] _referenceChiffreMaya;

        public CalculMaya(int width, int lenght)
        {
            _width = width;
            _lenght = lenght;
            _referenceChiffreMaya = new ChiffreMaya[_nbChiffre];
            for (int i = 0; i < _nbChiffre; i++)
            {
                _referenceChiffreMaya[i] = new ChiffreMaya(i);
            }
        }




        public void LoadDictionnary(List<string> reference)
        {
            for (int ligne = 0; ligne < _width; ligne++)
            {
                for (int lettre = 0; lettre < _nbChiffre; lettre++)
                {
                    int startIndex = lettre*_lenght;
                    
                    string extract = reference[ligne].Substring(startIndex, _lenght);
                    _referenceChiffreMaya[lettre].Lines.Add(extract);
                }
            }
        }


        public int LoadValues(List<string> data)
        {
            int valeur = 0;
            int nbChiffre = data.Count/ _width;
            
            for (int i = 0; i < nbChiffre; i++)
            {
                int startIndex = i*_width;
                int endIndex = (i + 1)*_width;
                List<string> chiffre = new List<string>();
                for(int j= startIndex; j<endIndex; j++)
                {
                    chiffre.Add(data[j]);
                }


                int puissance = nbChiffre - (i+1);
                int valeurMaya = GetChiffre(chiffre);

                valeur = valeur + valeurMaya *((int) Math.Pow(20, puissance)); 

            }

            return valeur;
        }


        

        public int GetChiffre(List<string> data)
        {
            return _referenceChiffreMaya.First(r => r.Match(data)).Valeur;
        }



        public double ExecuteOperation(string mathOperator, int a, int b)
        {
            if (mathOperator.Equals("+"))
                return a + b;
            else if (mathOperator.Equals("-"))
                return a - b;
            else if (mathOperator.Equals("*"))
                return a * b;
            else if (mathOperator.Equals("/"))
                return a / b;
            else
            {
                throw new NotImplementedException();
            }
        }


        public List<int> EncodeToInteger(double value)
        {
            
            List<int> values = new List<int>();
            double valeur = value;
            int puissance = PuissanceMax(value, 20);

            for (int i = puissance; i >= 0; i--)
            {
                double reste = 0;
                if (i > 0)
                {
                    int multiplicateur =(int) (valeur/ Math.Pow(20, i));

                    reste= valeur - multiplicateur*Math.Pow(20, i);
                    values.Add((int)multiplicateur);
                }
                else
                {
                    values.Add((int)valeur);
                }
                
                valeur = reste;
            }

         

            return values;
        }


        public static int PuissanceMax(double valeurX, int puissance)
        {
            double valeurPuissance = Math.Log(valeurX) / Math.Log(puissance);

            return (int)valeurPuissance;
        }


        public List<string> EncodeToMayaCode(List<int> mayaCodeInt)
        {
            List<string> retVal = new List<string>();
            mayaCodeInt.ForEach(m => retVal.AddRange(_referenceChiffreMaya[m].Lines));

            return retVal;
        }
    }


    public class ChiffreMaya 
    {
        public int Valeur { get; set; }
        public List<string> Lines { get; set; }

        public ChiffreMaya(int valeur)
        {
            Valeur = valeur;
            Lines=new List<string>();
        }

        public bool Match(List<string> data)
        {
            bool match = true;
            for (int i = 0; i < Lines.Count; i++)
            {
                if (!Lines[i].Equals(data[i]))
                {
                    match = false;
                    break;
                }
            }
            return match;
        }
     

    }


}
