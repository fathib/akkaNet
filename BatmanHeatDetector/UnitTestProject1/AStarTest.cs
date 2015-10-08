using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AstarSample
{
    [TestClass]
    public class AStarTest

    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }


    public class Game
    {
        private readonly int _width;//X
        private readonly int _height;//Y
        private Node[,] _map;
        private List<Wall> _walls;

        public Game(int width, int height)
        {
            _width = width;
            _height = height;

            _map = new Node[width, height];
        }


        public void LoadWall(int x, int y, string orientation)
        {
            Point p = new Point(x,y);
            Orientation o = (orientation == "H") ? Orientation.Horizontal : Orientation.Veritcal;
            _walls.Add(new Wall(p, o));
        }

        private List<Node> GetAdjacentWalkableNodes(Node currentNode)
        {
            List<Node> nodes = new List<Node>();

            var distanceFromStart = currentNode.DistanceFromStartNode + 1;
            
            var y = currentNode.Position.Y;
            var x = currentNode.Position.X;

            if (x < _width && CanMove(currentNode, Direction.Right))
            {
                var node = _map[x + 1, y];

                AddNode(currentNode, node, distanceFromStart, nodes);
            }
            if (y < _height && CanMove(currentNode, Direction.Down))
            {
                var node = _map[x, y + 1];
                AddNode(currentNode, node, distanceFromStart, nodes);
            }
            if (x > 0 && CanMove(currentNode, Direction.Left))
            {
                var node = _map[x - 1, y];
                AddNode(currentNode, node, distanceFromStart, nodes);
            }
            if (y > 0 && CanMove(currentNode, Direction.Top))
            {
                var node = _map[x, y - 1];
                AddNode(currentNode, node, distanceFromStart, nodes);
            }

            nodes.ForEach(n => n.DistanceFromStartNode = currentNode.DistanceFromStartNode+1);

            return nodes.OrderBy(n => n.EstimatedTotalDistance).ToList();
        }

        private static void AddNode(Node currentNode, Node node, int distanceFromStart, List<Node> nodes)
        {
            if (node.State == NodeState.Open)
            {
                if (distanceFromStart < node.DistanceFromStartNode)
                {
                    node.DistanceFromStartNode = distanceFromStart;
                    node.ParentNode = currentNode;
                    nodes.Add(node);
                }
            }
        }


        private bool CanMove(Node currentNode, Direction direction)
        {
            bool canMove = true;

            List<Point> possibleWallPosition= new List<Point>();
            Orientation wallOrientation = (direction == Direction.Right || direction == Direction.Left)
                ? Orientation.Veritcal
                : Orientation.Horizontal;

            if (direction == Direction.Right)
            {
                //
                //  |
                // x||
                //   |
                
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y));
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y-1));
            }
            else if (direction == Direction.Left)
            {
                //
                //  |
                //  ||x
                //   |
                
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y));
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y - 1));
            }
            else if (direction == Direction.Top)
            {
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y));
                possibleWallPosition.Add(new Point(currentNode.Position.X-1, currentNode.Position.Y));
            }
            else if (direction == Direction.Down)
            {
                possibleWallPosition.Add(new Point(currentNode.Position.X, currentNode.Position.Y+1));
                possibleWallPosition.Add(new Point(currentNode.Position.X - 1, currentNode.Position.Y+1));
            }

            var q = (from w in _walls
                join y in possibleWallPosition on w.WallPosition equals y
                where w.WallOrientation == wallOrientation
                select w).FirstOrDefault();
            
            return q!=null;
            
        }

        private bool Search(Node currentNode)
        {

            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            
            foreach (var nextNode in nextNodes)
            {
                int x = nextNode.Position.X;
                int y = nextNode.Position.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= _width || y < 0 || y >= _height)
                    continue;


                Node node = _map[x, y];

                if (node.State == NodeState.Closed)
                    continue;


            }
            return false;
        }
    }


    public enum Orientation
    {
        Horizontal,
        Veritcal
    }

    public class Node  
    {

        public NodeState State { get; set; }

        public Node ParentNode { get; set; }
        public Node(int x, int y)
        {
          
            Position= new Point(x,y);
            State = NodeState.Open;
        }

        public Point Position { get;  set; }
        

        public int DistanceFromStartNode { get;  set; }

        public int DistanceToExit { get;  set; }

        public int EstimatedTotalDistance => DistanceFromStartNode + DistanceToExit;
    }

    public class Wall
    {
        public Point WallPosition { get; }
        
        public Orientation WallOrientation { get; }

        public Wall(Point position, Orientation orientation)
        {
            WallPosition = position;
            WallOrientation = orientation;
        }
    }

    public enum Direction
    {
        Top,
        Down,
        Left,
        Right
    }

    public enum NodeState { Untested, Open, Closed }

}
