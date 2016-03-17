using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.SKyNetStrikeBack
{


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
                nodes[i] = new SkyNetCell(i);
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
        
        private List<SkyNetCell> _nodes; 

        public SkyNetGame(List<SkyNetCell> nodes)
        {
            _nodes = nodes;
        }
    }
}
