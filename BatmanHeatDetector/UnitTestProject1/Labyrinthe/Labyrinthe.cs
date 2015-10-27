using System.Collections.Generic;
using System.Linq;

namespace UnitTestProject1.Labyrinthe
{
    public class Labyrinthe
    {
        private readonly int _width;//x
        private readonly int _height;//y
        private readonly int _nbMove;

        public Node InitialPosition { get; private set; }
        public Node ControlRoom { get; private set; }
        private Node _target;
        public Node CurrentNode { get; private set; }
        private bool _goBack = false;


        private List<Node> _road = new List<Node>();
        private Node[,] _map;
        public Node[,] Map { get { return _map;} private set { _map = value; } }
        private Direction _mainDirection;


        public Labyrinthe(int width, int height, int nbMove)
        {
            _width = width;//x
            _height = height;//y
            _nbMove = nbMove;
            _map= new Node[width,height];
            ControlRoom = null;
            _target = null;
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
                        _road.Add(CurrentNode);
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


            if (CurrentNode.Content == NodeContent.CommandRoom)
            {
                _goBack = true;
            }
            Direction direction = Direction.DOWN;

            if (_goBack)
            {
                direction = GoBack(cNode);
            }
            else 
                direction = Walk(cNode);

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

        private Direction GoBack(Node currentPosition)
        {
            currentPosition.State = NodeState.Closed;
            Node nextPosition = _road.Last();
            _road.RemoveAt(_road.Count - 1);
            if (nextPosition.Position == currentPosition.Position)
            {
                nextPosition = _road.Last();
                _road.RemoveAt(_road.Count - 1);
            }

            var d  = GetDirection(currentPosition, nextPosition);
            CurrentNode = nextPosition;
            return d;

        }

        public Direction Walk(Node currentPosition)
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
                _road.Add(nextPosition);
            }
            else
            {
                //go Back
                currentPosition.State=NodeState.Closed;
                nextPosition = _road[_road.Count - 1];
                _road.RemoveAt(_road.Count - 1);
            }


            CurrentNode = nextPosition;
            return GetDirection(currentPosition, nextPosition);
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
                    walkableNodes.Add(node);
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = currentPosition;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;

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