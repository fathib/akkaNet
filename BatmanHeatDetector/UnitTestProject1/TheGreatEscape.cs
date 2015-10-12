using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TheGreatEscapeProject
{
    public class TheGreatEscape
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _playerId;
        private readonly int _playerCount;
        private int[,] _board;
        private List<Wall> walls = new List<Wall>(); 
        private Direction mainDirection;

        private Point[] _players = new Point[3]; 
        
        public TheGreatEscape(int width, int height, int playerId, int playerCount)
        {
            _width = width;
            _height = height;
            _playerId = playerId;
            _playerCount = playerCount;
            _board = new int[width,height];
            if(playerId == 0)
                mainDirection   = Direction.RIGHT;
            else if(playerId == 1)
                mainDirection = Direction.LEFT;
            else
                mainDirection = Direction.DOWN;
        }


        public void AddPlayer(int x, int y, int playerId)
        {
            _players[playerId] = new Point(x,y);
            _board[x,y] = playerId;
        }
        public void AddWall(int x, int y, string orientation)
        {
            walls.Add(new Wall(x,y,orientation));
        }


        public string NextMove(Point currentPosition)
        {
            Wall blockerWall = new Wall(0,0,"");
            Direction nextDirection = mainDirection;
            var directions = PriorityMoves( currentPosition);
            int i = 0;
            while (blockerWall != null)
            {
                nextDirection = directions[i];
                blockerWall = BloquerWall(currentPosition, nextDirection);
                i++;
            }
            
            return nextDirection.ToString();
           
        }


        public string DoSomething()
        {
            string action = String.Empty;
            var myPosition = _players[_playerId];
            var opponentID = _playerId == 0 ? 1 : 0;
            
            var opponentPosition = _players[opponentID];
            var opponentDirection = (opponentID == 0) ? Direction.RIGHT : Direction.LEFT;
            var distanceTOFinish = (opponentDirection == Direction.RIGHT )? _width - opponentPosition.X : opponentPosition.X;


            var myDistanceToFinish = (opponentDirection == Direction.RIGHT) ? _width - myPosition.X : myPosition.X;

            if (myDistanceToFinish > distanceTOFinish)
                action = PutWall(opponentPosition, opponentDirection);
            else
                action = NextMove(myPosition);



            return action;
        }

        private string PutWall(Point opponentPosition, Direction opponentDirection)
        {
            throw new NotImplementedException();
        }

        private List<Direction> PriorityMoves(Point currentPosition)
        {
            List<Direction> movesPriority = new List<Direction>();
            movesPriority.Add(mainDirection);
            if (mainDirection == Direction.RIGHT || mainDirection == Direction.LEFT)
            {
                int midDirection = _height / 2;
                if (currentPosition.Y > midDirection)
                {
                    movesPriority.Add(Direction.UP);
                    movesPriority.Add(Direction.DOWN);
                    movesPriority.Add(mainDirection == Direction.RIGHT? Direction.LEFT:Direction.RIGHT );
                }
                else
                {
                    movesPriority.Add(Direction.DOWN);
                    movesPriority.Add(Direction.UP);
                    movesPriority.Add(mainDirection == Direction.RIGHT ? Direction.LEFT : Direction.RIGHT);
                }
            }
            else
            {
                int midDirection = _width / 2;
                if (currentPosition.X > midDirection)
                {
                    movesPriority.Add(Direction.LEFT);
                    movesPriority.Add(Direction.RIGHT);
                    movesPriority.Add(mainDirection == Direction.UP ? Direction.DOWN : Direction.UP);
                }
                else
                {
                    movesPriority.Add(Direction.RIGHT);
                    movesPriority.Add(Direction.LEFT);
                    movesPriority.Add(mainDirection == Direction.UP ? Direction.DOWN : Direction.UP);
                }
            }

            return movesPriority;
        } 

        private Direction GetAlternateDirection(Point currentPosition, Direction direction)
        {
            if (direction == Direction.RIGHT || direction == Direction.LEFT)
            {
                int midDirection = _height/2;
                if(currentPosition.Y> midDirection)
                    return Direction.UP;
                else
                return Direction.DOWN;
            }
            else
            {
                int midDirection = _width / 2;
                if (currentPosition.X > midDirection)
                    return Direction.LEFT;
                else
                    return Direction.RIGHT;
            }
        }


        public Wall BloquerWall(Point position, Direction direction)
        {
           var blockerWall = walls.FirstOrDefault(w => !w.CanMove(position, direction));
            return blockerWall;
        }
    }


    public class Wall
    {
        private Point _wallPosition;
        private readonly Orientation _orientation;

        public Wall(int x, int y, string orientation)
        {
            _wallPosition = new Point(x,y);
            _orientation = (orientation == "H")? Orientation.Horizontal : Orientation.Vertical;
        }

        public bool CanMove(Point position, Direction direction)
        {
            return CanMove(position.X, position.Y, direction);
        }

        public bool CanMove(int x, int y, Direction direction)
        {
            bool block = false;
            if (_orientation == Orientation.Vertical)
            {
                if (direction == Direction.RIGHT)
                {
                    block = ((x == _wallPosition.X - 1 && y == _wallPosition.Y)
                             || (x == _wallPosition.X - 1 && y == _wallPosition.Y + 1));
                }
                else if (direction == Direction.LEFT)
                {
                    block = ((x == _wallPosition.X && y == _wallPosition.Y)
                             || (x == _wallPosition.X && y == _wallPosition.Y + 1));
                }
                else
                    block = false;
            }
            else
            {
                if (direction == Direction.DOWN)
                {
                    block = ((x == _wallPosition.X  && y == _wallPosition.Y-1)
                             || (x == _wallPosition.X + 1 && y == _wallPosition.Y - 1));
                }
                else if (direction == Direction.UP)
                {
                    block = ((x == _wallPosition.X && y == _wallPosition.Y)
                             || (x == _wallPosition.X+1 && y == _wallPosition.Y));
                }
                else
                    block = false;
            }

            return block;
            
        }


        public bool WallExist(Point point, Orientation orientation)
        {
            if( point.X== _wallPosition.X && point.Y == _wallPosition.Y)
                return true;

            if (orientation == Orientation.Vertical 
                && (point.X == _wallPosition.X && point.Y == _wallPosition.Y+1))
                return true;

            if (orientation == Orientation.Horizontal
                && (point.X == _wallPosition.X+1 && point.Y == _wallPosition.Y))
                return true;
            return false;
        }
    }


    public enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN 
    }

    public enum Orientation
    {
        Vertical,
        Horizontal
    }
}
