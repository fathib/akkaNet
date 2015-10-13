using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        }

        public void LoadMap(List<string> board)
        {
            //todo
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

        public List<Point> FindPath()
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
                return nextNode.Location.X == _width;
            else if (_mainDirection == Direction.Left)
                return nextNode.Location.X == 0;
            else if (_mainDirection == Direction.Top)
                return nextNode.Location.Y == 0;
            else 
                return nextNode.Location.Y == _height;
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

            if (x < _width && CanMove(currentNode, Direction.Right))
            {
                nodes.Add(_map[x + 1, y]);
            }
            if (y < _height && CanMove(currentNode, Direction.Down))
            {
                nodes.Add(_map[x, y + 1]);
            }
            if (x > 0 && CanMove(currentNode, Direction.Left))
            {
                nodes.Add(_map[x - 1, y]);
            }
            if (y > 0 && CanMove(currentNode, Direction.Top))
            {
                nodes.Add(_map[x, y - 1]);
            }
            
            return nodes;
        }


        private bool CanMove(Node currentNode, Direction direction)
        {
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

                possibleWallPosition.Add(new Point(currentNode.Location.X, currentNode.Location.Y));
                possibleWallPosition.Add(new Point(currentNode.Location.X, currentNode.Location.Y - 1));
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

            var q = (from w in _walls
                     join y in possibleWallPosition on w.WallPosition equals y
                     where w.WallOrientation == wallOrientation
                     select w).FirstOrDefault();

            return q != null;

        }


    }
}
