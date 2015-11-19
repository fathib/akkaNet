using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheadsChallenge
{
    public class Solution
    {
        static void Main(string[] args)
        {
            Console.Error.WriteLine("ici");
            int n = int.Parse(Console.ReadLine()); // the number of adjacency relations
            Node[] nodes = new Node[n * 2];
            for (int i = 0; i < n; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                int a = int.Parse(inputs[0]); // the ID of a person which is adjacent to yi
                int b = int.Parse(inputs[1]); // the ID of a person which is adjacent to xi

                Node nodeA = nodes[a];
                Node nodeB = nodes[b];
                if (nodeA == null)
                    nodeA = nodes[a] = new Node();

                if (nodeB == null)
                    nodeB = nodes[b] = new Node();


                nodeA.ChildNodes.Add(nodeB);
                nodeB.ChildNodes.Add(nodeA);
            }


            Console.Error.WriteLine("toto");
            List<Node> endNodes = nodes.Where(n7 => n7 != null && n7.EndOfTree).ToList();
            int iterration = 0;
            while (!Finish(endNodes))
            {
                iterration++;
                List<Node> tmp = new List<Node>();
                foreach (var node in endNodes)
                {
                    var child = node.ChildNodes[0];
                    child.ChildNodes.Remove(node);

                    if (!tmp.Contains(child) && child.EndOfTree) tmp.Add(child);
                }
                endNodes = tmp;
            }

            if (endNodes.Count() == 2)
                iterration++;
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(iterration); // The minimal amount of steps required to completely propagate the advertisement
        }

        public static bool Finish(List<Node> nodes)
        {
            if (nodes.Count() == 1)
                return true;
            else if (nodes.Count() == 2 & nodes[0].ChildNodes.Contains(nodes[1]))
                return true;

            else
                return false;
        }
    }



    public class Node
    {
        public Node()
        {
            ChildNodes = new List<Node>();
        }
        public List<Node> ChildNodes { get; set; }

        public bool EndOfTree
        {
            get { return ChildNodes.Count() == 1; }
        }
    }
}
