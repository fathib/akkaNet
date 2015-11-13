using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.APU
{
    public class Cell
    {
        public bool IsEmpty { get; private set; }
        public int NbLink { get; private set; }

        public bool CanCross { get; set; }

        public List<Cell> LinkedCells { get; private set; }

        public List<Cell> NeighborCells { get; private set; }
        public Point Position { get; set; }


        public Cell(char c, int x, int y)
        {
            Position = new Point(x,y);
            LinkedCells = new List<Cell>();
            NeighborCells = new List<Cell>();
            if (c == '.')
            {
                NbLink = 0;
                IsEmpty = true;
                CanCross = true;
            }
            else 
            {
                IsEmpty = false;
                NbLink = int.Parse(c.ToString());    
                CanCross = false;
            }
        }


        public int NbLinkToPut
        {
            get { return NbLink - LinkedCells.Count(); }
        }
        
        
        
    }

}

