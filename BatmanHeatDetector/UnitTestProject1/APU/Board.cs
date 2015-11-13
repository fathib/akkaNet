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
            //TODO

            while (cells.Count()>0)
            {
                var from = cells.First();

                var to = from.NeighborCells.Where(c => c.NbLinkToPut > 0).OrderByDescending(c=>c.NbLinkToPut).First();
                var messageToPrint = LinkCells(from, to);
                retVal.Add(messageToPrint);
                cells = _cellsToLink.Where(c => c.NbLinkToPut > 0).OrderBy(c => c.NbLinkToPut).ToList();
            }
            
            
            // link all nodes -> vérifier que les cells intermediaires ne sont pas tagés
            //les cells intermédiaires doivent être tagés pour ne plus etre utilisées
            //    -> les tags peuvent être l' id des cells liées (from/to)


            //RQ soit on remplit par ordre de priorité (du plus contraint au moins)
            //   soit on réévalue à chaque lien quelle est la célule la plus contrainte
            //   soit on organise un graph liée puis on complète

            return retVal;
        }

        private string LinkCells(Cell from, Cell to)
        {
            from.LinkedCells.Add(to);
            to.LinkedCells.Add(from);

            return from.Position.X + " " + from.Position.Y + " " + to.Position.X + " " + to.Position.Y + " 1";
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