using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1.SKyNetStrikeBack
{
    [TestClass]
    public class SkynetTest
    {



        [TestMethod]
        public void TestMethod()
        {
            List<string> links = new List<string>();
            links.Add("5 1");
            links.Add("1 2");
links.Add("2 3");
links.Add("3 4");
links.Add("4 5");
links.Add("0 1");
links.Add("0 2");
links.Add("0 3");
links.Add("0 4");
links.Add("0 5");
links.Add("5 7");
links.Add("1 8");
links.Add("2 9");
links.Add("3 10");
links.Add("10 6");
links.Add("7 8");
links.Add("8 9");
links.Add("3 9");
links.Add("2 8");
links.Add("4 10");
links.Add("7 12");
links.Add("4 14");
links.Add("14 6");
links.Add("13 5");
links.Add("13 6");
links.Add("1 15");
links.Add("6 16");
links.Add("7 17");
links.Add("17 6");
links.Add("10 18");
links.Add("9 19");
links.Add("6 11");
links.Add("12 1");
links.Add("7 20");
links.Add("21 9");
links.Add("21 10");
links.Add("21 3");

            List <int> gateway= new List<int>();
            gateway.Add(11);
            gateway.Add(12);
            gateway.Add(15);
            gateway.Add(16);
            gateway.Add(18);
            gateway.Add(19);
            gateway.Add(20);

            var game = SkyNetGameFactory.GetSkyNetGame(22,links, gateway);


            var linktoCut = game.LinkEverithing(0);
            string s = linktoCut.Item1.Id + " " + linktoCut.Item2.Id;
            game.CutLink(linktoCut.Item1.Id, linktoCut.Item2.Id);

            linktoCut = game.LinkEverithing(5);
            s = linktoCut.Item1.Id + " " + linktoCut.Item2.Id;


        }


    }


    public class SkyNetGameFactory
    {

        public static SkyNetGame GetSkyNetGameFromConsole(int nodeCount , int linkCount, int gateWayCount)
        {
            List<SkyNetCell> nodes = CreateNodeStructure(nodeCount);
            var linkList = new List<string>(linkCount);
            var gatewayIdList = new List<int>(gateWayCount);
            
            for (int i = 0; i < linkCount; i++)
            {
                linkList.Add(Console.ReadLine());
            }
            for (int i = 0; i < gateWayCount; i++)
            {
                gatewayIdList.Add(int.Parse(Console.ReadLine()));
            }

            return LoadBoardStructure(nodes, linkList, gatewayIdList);
        }

        public static SkyNetGame GetSkyNetGame(int nodeCount, List<string> linkList, List<int> gatewayIdList)
        {
            List<SkyNetCell> nodes = CreateNodeStructure(nodeCount);

            return LoadBoardStructure(nodes, linkList, gatewayIdList);
        }

        private static List<SkyNetCell> CreateNodeStructure(int nodeCount)
        {
            var nodes = new List<SkyNetCell>(nodeCount);
            for (var i = 0; i < nodeCount; i++)
            {
                nodes.Add(new SkyNetCell(i));
            }

            return nodes;
        }

        private static SkyNetGame LoadBoardStructure(List<SkyNetCell> nodes, List<string> linkList, List<int> gatewayIdList)
        {
            LoadLinks(linkList,ref nodes);
            LoadGateway(gatewayIdList, ref nodes);

            SkyNetGame game = new SkyNetGame(nodes);

            return game;
        }
      
        private static void LoadLinks(List<string> lines, ref List<SkyNetCell> nodes)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                var t = lines[i];
                var inputs = t.Split(' ');
                Console.Error.WriteLine(t);
                int cellId1 = int.Parse(inputs[0]); // N1 and N2 defines a link between these nodes
                int cellId2 = int.Parse(inputs[1]);
                var node1 = nodes.First(x => x.Id == cellId1);
                var node2 = nodes.First(x => x.Id == cellId2);

                node1.Neighbors.Add(node2);
                node2.Neighbors.Add(node1);
            }
        }

        private static void LoadGateway(List<int> gatewayIdList, ref List<SkyNetCell> nodes)
        {
            for (int i = 0; i < gatewayIdList.Count; i++)
            {
                int EI = gatewayIdList[i]; // the index of a gateway node
                var gateway = nodes.First(x => x.Id == EI);
                gateway.IsGateWay = true;
            }
        }
        
    }
    public class SkyNetGame
    {
        private readonly List<SkyNetCell> _nodes;

        public List<SkyNetCell> Nodes
        {
            get { return _nodes; }
        }


        public SkyNetGame(List<SkyNetCell> nodes)
        {
            _nodes = nodes;
        }

        public void CutLink(int idCell1, int idCell2)
        {
            var c1 = Nodes.FirstOrDefault(n => n.Id == idCell1);
            var c2 = Nodes.FirstOrDefault(n => n.Id == idCell2);

            c1.Neighbors.Remove(c2);
            c2.Neighbors.Remove(c1);
        }


        public Tuple<SkyNetCell, SkyNetCell> LinkEverithing(int IdstartNode)
        {
            ResetNodes();
            var startNode = Nodes.First(n => n.Id == IdstartNode);
            List<SkyNetCell> path = new List<SkyNetCell>();

            Walk(startNode);

     

            var gateWayParents = _nodes.Where(x => x.Neighbors.Any(s=>s.IsGateWay));
            var groups = gateWayParents.GroupBy(p => p.DistanceFromStart).OrderBy(g => g.Key);

            var nearestGroup = groups.First();
            var parent = nearestGroup.OrderByDescending(c => c.nbConectedGateway).First();

            
            path.Add(parent);
            var gate = parent.Neighbors.First(c => c.IsGateWay);


            return new Tuple<SkyNetCell, SkyNetCell>(parent, gate);
            
        }


        private void ResetNodes()
        {
            foreach (var skyNetCell in Nodes)
            {
                skyNetCell.Parent = null;
                skyNetCell.Status = SkyNetCellStatus.Untested;
                skyNetCell.DistanceFromStart = 0;
            }
        }


        private void Walk(SkyNetCell currentPosition)
        {
            var borderCells = new List<SkyNetCell>();
            borderCells.Add(currentPosition);

            while (borderCells.Count > 0)
            {
                var currentCell = borderCells.First();

                currentCell.Status = SkyNetCellStatus.Close;
                borderCells.RemoveAt(0);

                foreach (var neighbor in currentCell.Neighbors)
                {
                    
                    if (neighbor.Status == SkyNetCellStatus.Close)
                        continue;
                    else if (neighbor.Status == SkyNetCellStatus.Untested)
                    {
                        neighbor.Parent = currentCell;
                        neighbor.Status = SkyNetCellStatus.Open;
                        neighbor.DistanceFromStart = currentCell.DistanceFromStart + 1;
                        if(!neighbor.IsGateWay)
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

                //borderCells = borderCells.Where(c => c.Status != SkyNetCellStatus.Close).OrderBy(d => d.DistanceFromStart).ToList();
                
            }

        }

    }


}
