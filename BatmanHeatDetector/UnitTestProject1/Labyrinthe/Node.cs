using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.Labyrinthe
{
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
            else if (content == 'T')
                Content = NodeContent.KirkPosition;
            else if (content == 'C')
                Content = NodeContent.CommandRoom;
        }

        public Point Position { get; set; }

        public int DistanceFromStart { get; set; }

        public bool IsWalkable
        {
            get { return Content != NodeContent.Wall; }
            
        }
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
