using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Labyrinthe
{

    public class Labyrinthe
    {
        private readonly int _width;
        private readonly int _height;
        private readonly int _nbMove;

        private Node _initialPosition;
        private Node _controlRoom;

        private Node[,] _map;


        public Labyrinthe(int width, int height, int nbMove)
        {
            _width = width;
            _height = height;
            _nbMove = nbMove;
            _map= new Node[width,height];
        }


        public void LoadLineForWall(int rank, string content)
        {
            for (int x = 0; x < _width; x++)
            {
                char c = content[x];
                Node n = _map[x, rank];

                //first time
                if(n == null)
                    n = new Node(x,rank,c);
                else if (n.Content == NodeContent.Unknown )//we know the content
                    n.SetContent(c);

                if (n.Content == NodeContent.KirkPosition)
                    _initialPosition = n;

                if (n.Content == NodeContent.CommandRoom)
                    _controlRoom= n;
                
            }
        }


    }

    public class Node
    {

        public Node(int x, int y, char content)
        {
            Position = new Point(x,y);


            SetContent(content);

            State = NodeState.Untested;
        }

        public Node ParentNode { get; set; }

        public NodeState State { get; set; }
        
        public NodeContent Content { get; private set; }

        public void SetContent(char content)
        {
            if (content == '.')
                Content = NodeContent.Empty;
            else if (content == '#')
                Content = NodeContent.Wall;
            else if (content == '?')
                Content = NodeContent.Unknown;
        }

        public Point Position { get; set; }

        public int DistanceFromStart { get; set; }

    }

    public enum NodeContent
    {
        Wall,//#
        Empty,//.
        Unknown,//?
        KirkPosition,//T
        CommandRoom//C
    }

    public enum NodeState
    {
        Untested,
        Open,
        Closed
    }


    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }
}
