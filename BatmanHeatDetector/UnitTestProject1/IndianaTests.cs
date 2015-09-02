using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class IndianaTests

    {
        [TestMethod]
        public void TestMethod1()
        {


            int W = 2;
            int H = 4;


            SquareRoom[,] board = new SquareRoom[W,H];

            board[0, 0] = SquareRoom.RefereRooms[4];
            board[1, 0] = SquareRoom.RefereRooms[3];

            board[0, 1] = SquareRoom.RefereRooms[12];
            board[1, 1] = SquareRoom.RefereRooms[10];

            board[0, 2] = SquareRoom.RefereRooms[11];
            board[1, 2] = SquareRoom.RefereRooms[5];

            board[0, 3] = SquareRoom.RefereRooms[2];
            board[1, 3] = SquareRoom.RefereRooms[3];

            int Exit = 1; //x Position


            int Xpos = 1;
            int Ypos = 0;
            string InDirection = "TOP";
            Direction inputDir = Convert(InDirection);


            string line = "1 2 3";


            var currentRoom = board[Xpos, Ypos];
            var way = currentRoom.RoomPaths.FirstOrDefault(p => p.Entry == inputDir);
            if (way != null)
            {
                switch (way.OutDirection)
                {
                    case Direction.Down:
                        Ypos++;
                        break;
                    case Direction.Left:
                        Xpos--;
                        break;
                    case Direction.Top:
                        Xpos++;
                        break;
                }
            }
        }


        public Direction Convert(string direction)
        {
            if(direction =="TOP")
                return Direction.Top;
            else if(direction =="DOWN")
                return Direction.Down;
            else if (direction == "LEFT")
                return Direction.Left;
            else 
                return Direction.Right;

        }
    }




    public class SquareRoom
    {
        private static SquareRoom[] refereRooms;


        public static SquareRoom[] RefereRooms
        {
            get
            {
                if (refereRooms == null)
                    LoadRefRooms();

                return refereRooms;
            }
        }


        private static void LoadRefRooms()
        {
            refereRooms = new SquareRoom[14];
            refereRooms[1] = new SquareRoom(1, new []
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Down},
                new Path() { Entry = Direction.Left, OutDirection = Direction.Down},
                new Path() { Entry = Direction.Right, OutDirection = Direction.Down}
            });

            refereRooms[2] = new SquareRoom(2, new[]
            {
                new Path() { Entry = Direction.Left, OutDirection = Direction.Right},
                new Path() { Entry = Direction.Right, OutDirection = Direction.Left}
            });

            refereRooms[3] = new SquareRoom(3, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Down}
            });

            refereRooms[4] = new SquareRoom(4, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Left},
                new Path() { Entry = Direction.Right, OutDirection = Direction.Down}
            });

            refereRooms[5] = new SquareRoom(5, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Right},
                new Path() { Entry = Direction.Left, OutDirection = Direction.Down}
            });

            refereRooms[6] = new SquareRoom(6, new[]
            {
                new Path() { Entry = Direction.Left, OutDirection = Direction.Right},
                new Path() { Entry = Direction.Right, OutDirection = Direction.Left}
            });

            refereRooms[7] = new SquareRoom(7, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Down},
                new Path() { Entry = Direction.Right, OutDirection = Direction.Down}
            });

            refereRooms[8] = new SquareRoom(8, new[]
            {
                new Path() { Entry = Direction.Left, OutDirection = Direction.Down},
                new Path() { Entry = Direction.Right, OutDirection = Direction.Down}
            });

            refereRooms[9] = new SquareRoom(9, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Down},
                new Path() { Entry = Direction.Left, OutDirection = Direction.Down}
            });

            refereRooms[10] = new SquareRoom(10, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Left},
            });

            refereRooms[11] = new SquareRoom(11, new[]
            {
                new Path() { Entry = Direction.Top, OutDirection = Direction.Right},
            });

            refereRooms[12] = new SquareRoom(12, new[]
            {
                new Path() { Entry = Direction.Right, OutDirection = Direction.Down},
            });

            refereRooms[13] = new SquareRoom(13, new[]
            {
                new Path() { Entry = Direction.Left, OutDirection = Direction.Down},
            });
            

        }

        public SquareRoom(int type, Path[] roomPaths)
        {
            Type = type;
            RoomPaths = roomPaths;
        }

        public int Type { get; private set; }
        public Path[] RoomPaths { get; private set; }
        
    }


    public class Path
    {
        public Direction Entry { get; set; }
        public Direction OutDirection { get; set; }

    }
    
    public enum Direction
    {
        Top,
        Down,
        Left,
        Right
    }







}
