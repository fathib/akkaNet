using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace UnitTestProject1.APU
{

    
    public class Cell
    {
        public bool IsEmpty { get; private set; }
        public int Number { get; private set; }

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
                Number = 0;
                IsEmpty = true;
                CanCross = true;
            }
            else 
            {
                IsEmpty = false;
                Number = int.Parse(c.ToString());    
                CanCross = false;
            }
        }
        
        public int NbLinkToPut
        {
            
            get { return Number - LinkedCells.Count(); }
        }
        
        public int NbLinkToCell(Cell cell)
        {
            
            return LinkedCells.Count(c => c.Position.Equals(cell.Position));
        }
        
    }

    public class CellComparer : IComparer<Cell>
    {
        public int Compare(Cell x, Cell y)
        {
            if (x.NbLinkToPut == y.NbLinkToPut)
            {
                //trouver plus fin
                return x.NeighborCells.Count() - y.NeighborCells.Count();


            }
            else
                return x.NbLinkToPut - y.NbLinkToPut;
        }


    }
}

