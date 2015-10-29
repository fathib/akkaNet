using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace UnitTestProject1.Labyrinthe
{
    public class Labyrinthe
    {
        private readonly int _width;//x
        private readonly int _height;//y
        private readonly int _nbMoveToReachTheExit;

        public Node InitialPosition { get; private set; }
        public Node ControlRoom { get; private set; }
        
        public Node CurrentNode { get; private set; }
        private bool _goBack = false;


        private List<Node> _roadBack; 

        private Node[,] _map;
        public Node[,] Map { get { return _map;} private set { _map = value; } }
        

        public Labyrinthe(int width, int height, int nbMoveToReachTheExit)
        {
            _width = width;//x
            _height = height;//y
            _nbMoveToReachTheExit = nbMoveToReachTheExit;
            _map= new Node[width,height];
            ControlRoom = null;
            
        }


     

        public void LoadLineForWall(int rank, string content)
        {
            for (int x = 0; x < _width; x++)
            {
                char c = content[x];
                
                //first time
                if (_map[x, rank] == null)
                    _map[x, rank] = new Node(x,rank,c);
                else if (_map[x, rank].Content == NodeContent.Unknown )//we know the content
                    _map[x, rank].SetContent(c);

                if (_map[x, rank].Content == NodeContent.KirkPosition)

                {
                    InitialPosition = _map[x, rank];
                    if (CurrentNode == null)
                    {
                        CurrentNode = InitialPosition;
                    }
                }

                if (_map[x, rank].Content == NodeContent.CommandRoom)
                    ControlRoom = _map[x, rank];
                
            }
        }

        public string DoSomething()
        {
            CurrentNode.State =NodeState.Closed;
            var cNode = CurrentNode;

            //go back to initial room
            if (CurrentNode.Content == NodeContent.CommandRoom)
                _goBack = true;

            Node nextPosition = _goBack? GoBack(cNode): Walk(cNode);

            var direction = GetDirection(CurrentNode, nextPosition);
            
            CurrentNode = nextPosition;
            return GetOrder(direction);
        }


        private string GetOrder(Direction direction)
        {
            if (direction == Direction.DOWN)
                return "DOWN";
            else if (direction == Direction.RIGHT)
                return "RIGHT";
            else if (direction == Direction.LEFT)
                return "LEFT";
            else 
                return "UP";
        }

        private Node GoBack(Node currentPosition)
        {
            //todo astar for the shortest road
            if (_roadBack == null)
            {
                var road = FindRoad(currentPosition);

                if (road.Count <= _nbMoveToReachTheExit)
                    _roadBack = road;
            }
            else
            {
                //perform an AStart strategy!

            }

            Node nexMove = _roadBack.First();
            _roadBack.RemoveAt(0);
            return nexMove;

        }

     

        private List<Node> FindRoad(Node currentPosition)
        {
            List<Node> road = new List<Node>();
            Node node = currentPosition;
            while (node != InitialPosition)
            {
                node = node.ParentNode;
                road.Add(node);
            }
           

            return road;
        }

        public Node Walk(Node currentPosition)
        {
            var adjacentNodes = GetAdjacentWalkableNodes(currentPosition);
            Node nextPosition = null;

            if (adjacentNodes != null && adjacentNodes.Count > 0)
            {
                var commandRoom = adjacentNodes.FirstOrDefault(n => n.Content == NodeContent.CommandRoom);
                if (commandRoom == null)
                {
                    nextPosition = adjacentNodes[0];
                }
                else
                {
                    nextPosition = commandRoom;
                    ControlRoom = nextPosition;
                }
            }
            else
            {
                //go Back
                nextPosition = currentPosition.ParentNode;
            }
            
            return nextPosition;
        }



        public Direction GetDirection(Node from, Node to)
        {
            if(from.Position.X > to.Position.X)
                return  Direction.LEFT;
            else if(from.Position.X < to.Position.X)
                return Direction.RIGHT;
            else if (from.Position.Y > to.Position.Y)
                return Direction.UP;
            else //if (from.Position.Y < to.Position.Y)
                return Direction.DOWN;

        }


        public Direction TurnRight(Direction direction)
        {
            if (direction == Direction.DOWN)
                return Direction.LEFT;
            else if (direction == Direction.LEFT)
                return Direction.UP;
            else if (direction == Direction.UP)
                return Direction.RIGHT;
            else 
                return Direction.DOWN;
        }

        public List<Node> GetAdjacentWalkableNodes(Node currentPosition)
        {
            var nextLocations = GetAdjacentNodes(currentPosition);
            List<Node> walkableNodes = new List<Node>();

            foreach (var node in nextLocations)
            {
              
                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    node.ParentNode = currentPosition;
                    node.DistanceFromStart = GetTraversalCost(InitialPosition.Position, node.Position);
                    walkableNodes.Add(node);
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = currentPosition;
                    node.State = NodeState.Open;
                    node.DistanceFromStart = GetTraversalCost(InitialPosition.Position, node.Position);
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes.OrderByDescending(n => n.DistanceFromStart).ToList();
        }


        public static float GetTraversalCost(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        }

        public List<Node> GetAdjacentNodes(Node currentPosition)
        {
            var x = currentPosition.Position.X;
            var y = currentPosition.Position.Y;
            var retVal = new List<Node>();

            if(x<_width-1)
                retVal.Add(_map[x+1,y]);
            if (x > 0)
                retVal.Add(_map[x - 1, y]);
            if (y > 0)
                retVal.Add(_map[x , y-1]);
            if (y < _height-1)
                retVal.Add(_map[x, y + 1]);
            return retVal;
        }

    }

    
}