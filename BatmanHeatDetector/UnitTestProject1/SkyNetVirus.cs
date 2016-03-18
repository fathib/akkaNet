using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{


    [TestClass]
    public class SkyNetPathFinderTest
    {
        [TestMethod]
        public void SimpleExemple()
        {
            int nbNodes = 8;
            
            int nbGateway = 2;

            
            List<string> links = new List<string> {
            "6 2",
            "7 3",
            "6 3",
            //"5 3",
            //"3 4",
            "7 1",
            "2 0",
            "0 1",
            "0 3",
            "1 3",
            "2 3",
            "7 4"
            //"6 5"
             };
            int nbLinks = links.Count;

            var nodes = new List<SkyNetCell>();

            for (int i = 0; i < nbNodes; i++)
            {
                nodes.Add(new SkyNetCell(i));
            }

            foreach (var link in links)
            {
                var inputs = link.Split(' ');
                
                int cellId1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
                int cellId2 = int.Parse(inputs[1]);
                var node1 = nodes.First(x => x.Id == cellId1);
                var node2 = nodes.First(x => x.Id == cellId2);

                node1.Neighbors.Add(node2);
                node2.Neighbors.Add(node1);
            }

            //mark gateway
           
            var gateway = nodes.First(x => x.Id == 4);
            gateway.IsGateWay = true;

            gateway = nodes.First(x => x.Id == 5);
            gateway.IsGateWay = true;

            

            //Loop( nodes, 0);
            //Loop(nodes, 3);
            //Loop(nodes, 6);
            Loop(nodes, 3);
        }

        private static void Loop(List<SkyNetCell> nodes, int idInitial)
        {
            SkyNet sn = new SkyNet(nodes, idInitial );
            var path = sn.FindPath();
            int n1 = path[0].Id;
            int n2 = path[1].Id;
            
            var c1 = nodes.FirstOrDefault(n => n.Id == n1);
            var c2 = nodes.FirstOrDefault(n => n.Id == n2);

            c1.Neighbors.Remove(c2);
            c2.Neighbors.Remove(c1);
        }
    }


    public class SkyNetPathFinder
    {

        static void Main(string[] args)
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            int N = int.Parse(inputs[0]); // the total number of nodes in the level, including the gateways
            int L = int.Parse(inputs[1]); // the number of links
            int E = int.Parse(inputs[2]); // the number of exit gateways


            var nodes = new List<SkyNetCell>();

            for (int i = 0; i < N; i++)
            {
                nodes.Add(new SkyNetCell(i));
            }

            //link cells
            for (int i = 0; i < L; i++)
            {
                var t = Console.ReadLine();
                inputs = t.Split(' ');
                Console.Error.WriteLine(t);
                int cellId1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
                int cellId2 = int.Parse(inputs[1]);
                var node1 = nodes.First(x => x.Id == cellId1);
                var node2 = nodes.First(x => x.Id == cellId2);

                node1.Neighbors.Add(node2);
                node2.Neighbors.Add(node1);
            }
            
            //mark gateway
            for (int i = 0; i < E; i++)
            {
                int EI = int.Parse(Console.ReadLine()); // the index of a gateway node
                Console.Error.WriteLine("EI-->" + EI);
                var gateway = nodes.First(x => x.Id == EI);
                gateway.IsGateWay = true;
            }

            //game loop
            while (true)
            {
                int IdInitial = int.Parse(Console.ReadLine()); // The index of the node on which the Skynet agent is positioned this turn
                
                SkyNet sn = new SkyNet(nodes, IdInitial);

                var path = sn.FindPath();
                
                int n1 = path[0].Id;
                int n2 = path[1].Id;
                string pathToCut = n1 +" "+n2;

                var c1 = nodes.FirstOrDefault(n => n.Id == n1);
                var c2 = nodes.FirstOrDefault(n => n.Id == n2);

                c1.Neighbors.Remove(c2);
                c2.Neighbors.Remove(c1);

                Console.WriteLine(pathToCut);

            }
        }
        
    }


    public class SkyNet
    {
        private readonly List<SkyNetCell> _nodes;
        private readonly SkyNetCell _startNode;
        private SkyNetCell _endNode;

        public SkyNet(List<SkyNetCell> nodes, int InitialId)
        {

            ResetNodes(nodes);
            _nodes = nodes;

            _startNode = _nodes.First(x => x.Id == InitialId);
            
            _endNode = null;
        }



        public List<SkyNetCell> FindPath()
        {
            // The start node is the first entry in the 'open' list
            
            List<SkyNetCell> path = new List<SkyNetCell>();

            Walk(_startNode);

            var gate = _nodes.Where(n => n.IsGateWay && n.Parent != null).OrderBy(c => c.DistanceFromStart).FirstOrDefault();

            //récuperer les gates 
            //trier par distance et parent commun
            //couper

            var parent = gate.Parent;
            path.Add(parent);
            path.Add(gate);
            
            return path;
        }

        private void ResetNodes(List<SkyNetCell> nodes)
        {
            foreach (var skyNetCell in nodes)
            {
                skyNetCell.Parent = null;
                skyNetCell.Status = SkyNetCellStatus.Untested;
            }
        }

        private List<SkyNetCell> GetAdjacentWalkableNodes(SkyNetCell currentPosition)
        {
            var nextLocations = currentPosition.Neighbors;
            var walkableNodes = new List<SkyNetCell>();

            foreach (var node in nextLocations)
            {
                // Ignore already-closed nodes
                if (node.Status == SkyNetCellStatus.Close)
                    continue;
                // Already-open nodes are only added to the list if their G-value is lower going via this route.

                if (node.Parent == null)
                {
                    node.Parent = currentPosition;
                    node.DistanceFromStart = currentPosition.DistanceFromStart + 1;
                    node.Status = SkyNetCellStatus.Open;
                    walkableNodes.Add(node);
                }
                else 
                {
                    if (node.DistanceFromStart > currentPosition.DistanceFromStart + 1)
                    {
                        node.Parent = currentPosition;
                        node.DistanceFromStart = currentPosition.DistanceFromStart + 1;
                        node.Status = SkyNetCellStatus.Open;
                        walkableNodes.Add(node);

                    }
                }
                
            }

            return walkableNodes.OrderByDescending(n => n.DistanceFromStart).ToList();
        }


        private void Walk(SkyNetCell currentPosition)
        {
            var borderCells = new List<SkyNetCell>();
            borderCells.Add(currentPosition);
            
            while (borderCells.Count>0)
            {
                var currentCell = borderCells.First();

                currentCell.Status = SkyNetCellStatus.Close;
                borderCells.RemoveAt(0);

                foreach (var neighbor in currentCell.Neighbors)
                {
                    if(neighbor.Status ==  SkyNetCellStatus.Close)
                        continue;
                    else if (neighbor.Status == SkyNetCellStatus.Untested)
                    {
                        neighbor.Parent = currentCell;
                        neighbor.Status = SkyNetCellStatus.Open;
                        neighbor.DistanceFromStart = currentCell.DistanceFromStart + 1;
                        borderCells.Add(neighbor);
                    }
                    else
                    {
                        if (neighbor.DistanceFromStart > currentCell.DistanceFromStart + 1)
                        {
                            neighbor.Parent = currentCell;
                            neighbor.DistanceFromStart = currentCell.DistanceFromStart + 1;
                        }
                    }
                    
                }

                borderCells = borderCells.Where(c => c.Status != SkyNetCellStatus.Close).OrderBy(d => d.DistanceFromStart).ToList();
                
            }

        }
        
    }

    public class SkyNetCell
    {

        public SkyNetCell(int id)
        {
            this.Id = id;
            IsGateWay = false;
            Status = SkyNetCellStatus.Untested;
            Neighbors = new List<SkyNetCell>();
        }

        public int Id { get; set; }
        public List<SkyNetCell> Neighbors { get; set; } 

        public bool IsGateWay { get; set; }
        
        public SkyNetCellStatus Status { get; set; }

        public SkyNetCell Parent { get; set; }

        public int DistanceFromStart { get; set; }


        public int nbConectedGateway
        {
            get { return Neighbors.Where(n => n.IsGateWay).Count(); }
        }
        
    }

    public enum SkyNetCellStatus
    {
        Open,
        Close,
        Untested
    }

}

