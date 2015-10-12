using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1.GreatEscape
{
    public class GreatEscapeGame
    {
        
        private readonly int _playerCount;
        private readonly int _myId;
        private int _width;
        private int _height;
        private Node[,] _nodes;
        private Node _startNode;


        public GreatEscapeGame(int width, int height, int playerCount, int myId )
        {
            _width = width;
            _height = height;
            _playerCount = playerCount;
            _myId = myId;
            _nodes= new Node[width,height];
        }


        
    }
}
