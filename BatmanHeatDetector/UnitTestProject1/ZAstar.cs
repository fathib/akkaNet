using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZAstarNamespace

{
    public class Node
    {
        public Point Location { get; private set; }
        public bool IsWalkable { get; set; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F { get { return this.G + this.H; } }
        public NodeState State { get; set; }
        public Node ParentNode { get; set; }

        public static int GetTraversalCost(Point location, Point point)
        {
            return 1;
        }
    }
    public enum NodeState
    { Untested,
        Open,
        Closed
    }

    public class Parameter
    {
        public Point StartLocation { get; set; }
        public Point EndLocation { get; set; }
        public string[,] Map { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }



    public class Algo
    {
        private int width;
        private int height;
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }

        public Node[,] Nodes { get; set; }

        public Algo(Parameter parameter)
        {
            Nodes = new Node[parameter.Width,parameter.Height];

            for (int x = 0; x < parameter.Width; x++)
            {
                for (int y = 0; x < parameter.Height; y++)
                {
                    
                }
            }

        }

        private List<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            List<Node> walkableNodes = new List<Node>();
            IEnumerable<Point> nextLocations = GetAdjacentLocations(fromNode.Location);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                    continue;

                Node node = this.Nodes[x, y];
                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == NodeState.Closed)
                    continue;

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == NodeState.Open)
                {
                    float traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
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


        private bool Search(Node currentNode)
        {
            currentNode.State = NodeState.Closed;
            List<Node> nextNodes = GetAdjacentWalkableNodes(currentNode);
            nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location == EndNode.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                        return true;
                }
            }

            return false;

        }


        public List<Point> FindPath()
        {
            List<Point> path = new List<Point>();
            
            bool success = Search(StartNode);
            if (success)
            {
                Node node = EndNode;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }
                path.Reverse();
            }
            return path;
        }



        private List<Point> GetAdjacentLocations(Point location)
        {
            List<Point> points = new List<Point>();
            
            points.Add(new Point(location.X+1, location.Y));
            points.Add(new Point(location.X - 1, location.Y));
            points.Add(new Point(location.X , location.Y+1));
            points.Add(new Point(location.X , location.Y-1));
            return points;
        }
    }
}
