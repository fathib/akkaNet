using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class NumeroTelephone
    {
        [TestMethod]
        public void TestMethod1()
        {

            
            Agenda a = new Agenda();
            a.AddNumber("0123456789");
            a.AddNumber("1123456789");
            
            int retVal = a.NbCells;

            Assert.AreEqual(20, retVal);
        }




    }

    public class Agenda
    {
        
        private Cell _storage;
        private int _nbCells =0;

        public Agenda()
        {
            _storage = new Cell() {Value = -1};
        }

        public int NbCells
        {
            get { return _nbCells; }
        }

        public void AddNumber(string phoneNumber)
        {
            _nbCells = _storage.AddPhoneNumber(phoneNumber, _nbCells);
        }


    }

    public class Cell
    {
        public Cell()
        {
            ChildCells = new List<Cell>();
        }
        public int AddPhoneNumber(string number, int addedCells)
        {
            if (string.IsNullOrEmpty(number))
                return addedCells;
            else
            {
                
                int firstValue = int.Parse(number[0].ToString());
                var child = ChildCells.FirstOrDefault(_ => _.Value == firstValue);
                if (child == null)
                { 
                    child = new Cell() {Value = firstValue};
                    addedCells++;
                    ChildCells.Add(child);  
                }
                return child.AddPhoneNumber(number.Substring(1, number.Length-1), addedCells);
            }
        }
        public int Value { get; set; }

        public List<Cell> ChildCells { get; set; } 
    }
}
