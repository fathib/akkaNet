using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.GreatEscape
{
    public class GreatEscapeGame
    {
        
        private readonly int _myId;
        private int _width;
        private int _height;
        private Node[,] _map;
        
        private List<Wall> _walls;
        private Direction _mainDirection;
        private Point[] _playersPositions;
        private Node _endNode;


        public GreatEscapeGame(int width, int height, int playerCount, int myId )
        {
            _width = width;
            _height = height;
            _myId = myId;
            _map = new Node[width,height];
            _walls = new List<Wall>();
            _playersPositions = new Point[playerCount];

            if(myId == 0)
                _mainDirection = Direction.Right;
            else if(myId ==1)
                _mainDirection = Direction.Left;
            else if (myId == 2)
                _mainDirection = Direction.Top;
            else 
                _mainDirection = Direction.Down;

            LoadMap();
        }

        private void LoadMap()
        {
            for (int x=0; x < _width; x++)
            {
                for (int y=0; y < _height; y++)
                {

                    int estimatedCostToEnd = 0;

                    if (_mainDirection == Direction.Right)
                        estimatedCostToEnd = _width - x;
                    else if (_mainDirection == Direction.Left)
                        estimatedCostToEnd = x;
                    else if (_mainDirection == Direction.Down)
                        estimatedCostToEnd = _height- y;
                    else if (_mainDirection == Direction.Top)
                        estimatedCostToEnd = y;


                    _map[x,y] = new Node(x,y,estimatedCostToEnd);
                }
            }
        }
        public void LoadWall(int x, int y, string orientation)
        {
            Point p = new Point(x, y);
            Orientation o = (orientation == "H") ? Orientation.Horizontal : Orientation.Veritcal;
            _walls.Add(new Wall(p, o));
        }

        public void LoadPlayersPositions(int x, int y, int Id)
        {
            _playersPositions[Id] = new Point(x,y);
        }


        public void Clear()
        {
            _walls.Clear();
            _endNode = null;
        }
        public string GetNextOrder()
        {
            string order = String.Empty;
            var path = FindPath();

            var nbMove = path.Count;
            //no available exit
            

            var nextPosition = path[0];
            Point myPosition = _playersPositions[_myId];
            
            if (nextPosition.X > myPosition.X)
                order = "RIGHT";
            else if (nextPosition.X < myPosition.X)
                order = "LEFT";
            if (nextPosition.Y > myPosition.Y)
                order = "DOWN";
            else if (nextPosition.X < myPosition.X)
                order = "UP";

            return order;
            
        }
        private List<Point> FindPath()
        {
            // The start node is the first entry in the 'open' list
            List<Point> path = new List<Point>();

            Point myPosition = _playersPositions[_myId];
            Node myNode = _map[myPosition.X, myPosition.Y];
            
            bool success = Search(myNode);
            if (success)
            {
                // If a path was found, follow the parents from the end node to build a list of locations
                Node node = this._endNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }

                // Reverse the list so it's in the correct order when returned
                path.Reverse();
            }

            return path;
        }

        private bool Search(Node currentNode)
        {
            // Set the current node to Closed since it cannot be traversed more than once
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);

            // Sort by F-value so that the shortest possible routes are considered first
            nextNodes.Sort((node1, node2) => node1.EstimatedCostToEnd.CompareTo(node2.EstimatedCostToEnd));
            foreach (var nextNode in nextNodes)
            {
                // Check whether the end node has been reached
                if ( IsEndLocation(nextNode))
                {
                    _endNode = nextNode;
                    return true;
                }
                else
                {
                    // If not, check the next set of nodes
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                        return true;
                }
            }

            // The method returns false if this path leads to be a dead end
            return false;
        }

        private bool IsEndLocation(Node nextNode)
        {
            
            if (_mainDirection == Direction.Right)
                return nextNode.Location.X == _width - 1;
            else if (_mainDirection == Direction.Left)
                return nextNode.Location.X == 0;
            else if (_mainDirection == Direction.Top)
                return nextNode.Location.Y == 0;
            else 
                return nextNode.Location.Y == _height-1;
        }

        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {

            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Node> nextLocations = GetAdjacentLocations(fromNode);

            foreach (var node in nextLocations)
            {

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    int traversalCost = Node.GetTraversalCost();//1 no transversal cost

                    int costFromStarWithThisParentNode = fromNode.CostFromStart + traversalCost;
                    //we find a shortest path
                    if (costFromStarWithThisParentNode < node.CostFromStart)
                    {
                        node.ParentNode = fromNode;
                        node.CostFromStart = costFromStarWithThisParentNode;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

        private List<Node> GetAdjacentLocations(Node currentNode)
        {
            List<Node> nodes = new List<Node>();

            var y = currentNode.Location.Y;
            var x = currentNode.Location.X;

            if (x < _width-1 && CanMove(currentNode, Direction.Right))
            {
                nodes.Add(_map[x + 1, y]);
            }
            if (y < _height-1 && CanMove(currentNode, Direction.Down))
            {
                nodes.Add(_map[x, y + 1]);
            }
            if (y > 0 && CanMove(currentNode, Direction.Top))
            {
                nodes.Add(_map[x, y - 1]);
            }
            if (x > 0 && CanMove(currentNode, Direction.Left))
            {
                nodes.Add(_map[x - 1, y]);
            }
            
            
            return nodes;
        }


        private bool CanMove(Node currentNode, Direction direction)
        {
            //if (_walls.Count == 0)
            //    return true;
            bool canMove = true;

            List<Point> possibleWallPosition = new List<Point>();
            Orientation wallOrientation = (direction == Direction.Right || direction == Direction.Left)
                ? Orientation.Veritcal
                : Orientation.Horizontal;

            if (direction == Direction.Right)
            {
                //
                //  |
                // x||
                //   |

                possibleWallPosition.Add(new Point(currentNode.Location.X+1, currentNode.Location.Y));
                possibleWallPosition.Add(new Point(currentNode.Location.X+1, currentNode.Location.Y - 1));
            }
            else if (direction == Direction.Left)
            {
                //
                //  |
                //  ||x
                //   |

                possibleWallPosition.Add(new Point(currentNode.Location.X, currentNode.Location.Y));
                possibleWallPosition.Add(new Point(currentNode.Location.X, currentNode.Location.Y - 1));
            }
            else if (direction == Direction.Top)
            {
                possibleWallPosition.Add(new Point(currentNode.Location.X, currentNode.Location.Y));
                possibleWallPosition.Add(new Point(currentNode.Location.X - 1, currentNode.Location.Y));
            }
            else if (direction == Direction.Down)
            {
                possibleWallPosition.Add(new Point(currentNode.Location.X, currentNode.Location.Y + 1));
                possibleWallPosition.Add(new Point(currentNode.Location.X - 1, currentNode.Location.Y + 1));
            }

            var blockerWall = (from w in _walls
                     join y in possibleWallPosition on w.WallPosition equals y
                     where w.WallOrientation == wallOrientation
                     select w).FirstOrDefault();

            return blockerWall == null;

        }


    }



    public class Node
    {
        private Node parentNode;

        /// <summary>
        /// The node's location in the grid
        /// </summary>
        public Point Location { get; private set; }



        /// <summary>
        /// Estimated cost from here to end
        /// </summary>
        public int EstimatedCostToEnd { get; set; }

        /// <summary>
        /// Cost from start to here
        /// </summary>
        public int CostFromStart { get; set; }

        /// <summary>
        /// Estimated total cost (F = G + H)
        /// </summary>
        public float EstimatedTotalCost
        {
            get { return this.CostFromStart + this.EstimatedCostToEnd; }
        }


        /// <summary>
        /// Flags whether the node is open, closed or untested by the PathFinder
        /// </summary>
        public NodeState State { get; set; }


        public Node(int x, int y, int estimatedCostToEnd)
        {
            this.Location = new Point(x, y);
            this.State = NodeState.Untested;

            EstimatedCostToEnd = estimatedCostToEnd;

            CostFromStart = 0;
        }

        public Node ParentNode
        {
            get { return this.parentNode; }
            set { this.parentNode = value; }
        }

        public static int GetTraversalCost()
        {
            return 1;
        }
    }

    public enum NodeState
    {
        /// <summary>
        /// The node has not yet been considered in any possible paths
        /// </summary>
        Untested,
        /// <summary>
        /// The node has been identified as a possible step in a path
        /// </summary>
        Open,
        /// <summary>
        /// The node has already been included in a path and will not be considered again
        /// </summary>
        Closed
    }


    public enum Direction
    {
        Top,
        Down,
        Left,
        Right
    }
    public enum Orientation
    {
        Horizontal,
        Veritcal
    }

    public class Wall
    {
        public Point WallPosition { get; set; }

        public Orientation WallOrientation { get; set; }

        public Wall(Point position, Orientation orientation)
        {
            WallPosition = position;
            WallOrientation = orientation;
        }
    }


}
