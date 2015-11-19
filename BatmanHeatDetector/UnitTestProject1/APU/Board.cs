using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1.APU
{

    public class Board
    {
        private readonly int _width;
        private readonly int _height;
        public Cell[,] Map { get; set; }
        private List<Cell> _cellsToLink = new List<Cell>(); 

        public Board(int width, int height)
        {
            _width = width;
            _height = height;
            Map = new Cell[_width,_height];
        }

        public void LoadLine(string line, int y)
        {
            for (int x = 0; x < line.Count(); x++)
            {
                Map[x,y]= new Cell(line[x],x,y);
                if(!Map[x,y].IsEmpty)
                    _cellsToLink.Add(Map[x, y]);
            }
        }
        
        public List<string> DoSomething()
        {
            var retVal = new List<string>();
            //DONE 
            //load map
            //store cells in list

            //get neighbord (Nord, sud, est, west)
            LoadNeighbors();

            //order by neighbors

            var cells = _cellsToLink.Where(c => c.NbLinkToPut > 0).OrderBy(c => c.NbLinkToPut).ToList();
            
            
            while (cells.Count()>0)
            {
                Cell from = null;
                //get cells with no choice
                from = cells.FirstOrDefault(c => c.NbLinkToPut == 1 && c.NeighborCells.Count==1);
                if(from == null)
                    from = cells.First();

                LinkSelectedCellToNeighbor(from, retVal);
                cells = _cellsToLink.Where(c => c.NbLinkToPut > 0).OrderBy(c => c.NbLinkToPut).ToList();
            }
            
         

            return retVal;
        }

        private void LinkSelectedCellToNeighbor(Cell from, List<string> retVal)
        {
            var messageToPrint = string.Empty;
            var orderedNeighbors = from.NeighborCells.Where(c => c.NbLinkToPut > 0).OrderByDescending(c => c.NbLinkToPut).ToList();
            foreach (var neighbor in orderedNeighbors)
            {
                if (from.NbLinkToCell(neighbor) < 2)
                {
                    messageToPrint = LinkCells(from, neighbor);
                    break;
                }
            }


            if (!string.IsNullOrEmpty(messageToPrint))
            {
                retVal.Add(messageToPrint);
                Console.WriteLine(messageToPrint);
            }
            //else -> put a bool to say that the cell is not available

        }

        private string LinkCells(Cell from, Cell to)
        {
            from.LinkedCells.Add(to);
            to.LinkedCells.Add(from);

            GetIntermediateCells(from, to);
        
            return from.Position.X + " " + from.Position.Y + " " + to.Position.X + " " + to.Position.Y + " 1";
        }

        


        public void GetIntermediateCells(Cell from, Cell to)
        {
            
            if (from.Position.X == to.Position.X)
            {
                int min = Math.Min(from.Position.Y, to.Position.Y);
                int max = Math.Max(from.Position.Y, to.Position.Y);
                //get vertical cells
                for (int y = min+1; y<max; y++ )
                {
                    var middleNode = Map[from.Position.X, y];

                    var left = GetHorizontalNeighbor(middleNode.Position.X, middleNode.Position.Y, -1);
                    var right = GetHorizontalNeighbor(middleNode.Position.X, middleNode.Position.Y, +1);
                    if (left != null && right != null)
                    {
                        //cut nodes link
                        left.NeighborCells.Remove(left.NeighborCells.FirstOrDefault(c => c.Position == right.Position));
                        right.NeighborCells.Remove(right.NeighborCells.FirstOrDefault(c => c.Position == left.Position));
                    }
                    
                }

            }
            else if (from.Position.Y == to.Position.Y)
            {
                int min = Math.Min(from.Position.X, to.Position.X);
                int max = Math.Max(from.Position.X, to.Position.X);

                //get vertical cells
                for (int x = min; x < max; x++)
                {
                    var middleNode= Map[x,from.Position.Y];

                    var top = GetVerticalNeighbor(middleNode.Position.X, middleNode.Position.Y, +1);
                    var bot = GetVerticalNeighbor(middleNode.Position.X, middleNode.Position.Y, -1);
                    if (top != null && bot != null)
                    {
                        //cut nodes link
                        top.NeighborCells.Remove(top.NeighborCells.FirstOrDefault(c => c.Position == bot.Position));
                        bot.NeighborCells.Remove(bot.NeighborCells.FirstOrDefault(c => c.Position == top.Position));
                    }
                    
                }
            }
            
        }

        private void LoadNeighbors()
        {
            foreach (var cell in _cellsToLink)
            {
                var right = GetHorizontalNeighbor(cell.Position.X, cell.Position.Y, +1);
                if(right!= null)
                    cell.NeighborCells.Add(right);
                var left = GetHorizontalNeighbor(cell.Position.X, cell.Position.Y, -1);
                if (left != null)
                    cell.NeighborCells.Add(left);
                var top = GetVerticalNeighbor(cell.Position.X, cell.Position.Y, +1);
                if (top != null)
                    cell.NeighborCells.Add(top);
                var down = GetVerticalNeighbor(cell.Position.X, cell.Position.Y, -1);
                if (down != null)
                    cell.NeighborCells.Add(down);
            }
        }
        
        public Cell GetHorizontalNeighbor(int x, int y, int direction)
        {
            var newXPosition = x + direction;
            //direction 
            //  +1 --> right
            //  -1 --> left
            if (newXPosition < _width && newXPosition  >= 0)
            {
                if (!Map[newXPosition, y].IsEmpty)
                    return Map[newXPosition, y];
                else
                    return GetHorizontalNeighbor(newXPosition, y, direction);
            }
            else
                return null;
        }

        public Cell GetVerticalNeighbor(int x, int y, int direction)
        {
            var newYPosition = y + direction;
            //direction 
            //  +1 --> down
            //  -1 --> up

            if (newYPosition < _height && newYPosition >= 0)
            {
                if (!Map[x, newYPosition].IsEmpty)
                    return Map[x, newYPosition];
                else
                    return GetVerticalNeighbor(x, newYPosition, direction);
            }
            else
                return null;
        }


      
    }

}