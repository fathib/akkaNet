using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class BenderTest
    {
        [TestMethod]
        public void TestMethod1()
        {

            int lines = 10;
            int columns = 10;
            List<string> input = new List<string>
            {
                "##########",
                "#        #",
                "#  S   W #",
                "#        #",
                "#  $     #",
                "#        #",
                "#@       #",
                "#        #",
                "#E     N #",
                "##########"
            };

            Point initialPosition= new Point(0,0);
            string[,] board = new string[columns,lines];
            List<Point> teleTransporters = new List<Point>();
            for (int y = 0; y < lines; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    board[x, y] = input[y][x].ToString();

                    if (board[x, y]=="@")
                    {
                        initialPosition.X = x;
                        initialPosition.Y = y;
                    }

                    if (board[x, y] == "T")
                    {
                        teleTransporters.Add(new Point(x,y));
                    }
                }
            }

            BenderGame bg = new BenderGame();
            bg.Board = board;
            bg.Bender.Position = initialPosition;
            bg.TeleTransporters = teleTransporters;
            var orders = bg.Move();

            Assert.AreEqual(20, orders.Count);
            Assert.AreEqual("SOUTH",orders[0]);
            Assert.AreEqual("SOUTH", orders[1]);
            Assert.AreEqual("EAST", orders[2]);
            Assert.AreEqual("EAST", orders[3]);
            Assert.AreEqual("EAST", orders[4]);
            Assert.AreEqual("EAST", orders[5]);
            Assert.AreEqual("EAST", orders[6]);
            Assert.AreEqual("EAST", orders[7]);
            Assert.AreEqual("NORTH", orders[8]);
            Assert.AreEqual("NORTH", orders[9]);
            Assert.AreEqual("NORTH", orders[10]);
            Assert.AreEqual("NORTH", orders[11]);
            Assert.AreEqual("NORTH", orders[12]);
            Assert.AreEqual("NORTH", orders[13]);
            Assert.AreEqual("WEST", orders[14]);
            Assert.AreEqual("WEST", orders[15]);
            Assert.AreEqual("WEST", orders[16]);
            Assert.AreEqual("WEST", orders[17]);
            Assert.AreEqual("SOUTH", orders[18]);
            Assert.AreEqual("SOUTH", orders[19]);
        }


        [TestMethod]
        public void TestMethod3()
        {

            int lines = 8;
            int columns =8;
            List<string> input = new List<string>
            {
                "########",
                "#     $#",
                "#      #",
                "#      #",
                "#  @   #",
                "#      #",
                "#      #",
                "########"};

            Point initialPosition = new Point(0, 0);
            string[,] board = new string[columns, lines];
            List<Point> teleTransporters = new List<Point>();
            for (int y = 0; y < lines; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    board[x, y] = input[y][x].ToString();

                    if (board[x, y] == "@")
                    {
                        initialPosition.X = x;
                        initialPosition.Y = y;
                    }

                    if (board[x, y] == "T")
                    {
                        teleTransporters.Add(new Point(x, y));
                    }
                }
            }

            BenderGame bg = new BenderGame();
            bg.Board = board;
            bg.Bender.Position = initialPosition;
            bg.TeleTransporters = teleTransporters;
            var orders = bg.Move();

            Assert.AreEqual(10, orders.Count);
            Assert.AreEqual("SOUTH", orders[0]);
            Assert.AreEqual("SOUTH", orders[1]);
            Assert.AreEqual("EAST", orders[2]);
            Assert.AreEqual("EAST", orders[3]);
            Assert.AreEqual("EAST", orders[4]);
            Assert.AreEqual("NORTH", orders[5]);
            Assert.AreEqual("NORTH", orders[6]);
            Assert.AreEqual("NORTH", orders[7]);
            Assert.AreEqual("NORTH", orders[8]);
            Assert.AreEqual("NORTH", orders[9]);
        }



        [TestMethod]
        public void TestModeCasseur()
        {

            int lines = 10;
            int columns = 10;
            List<string> input = new List<string>
            {
                "##########",
                "# @      #",
                "# B      #",
                "#XXX     #",
                "# B      #",
                "#    BXX$#",
                "#XXXXXXXX#",
                "#        #",
                "#        #",
                "##########"};

            Point initialPosition = new Point(0, 0);
            string[,] board = new string[columns, lines];
            List<Point> teleTransporters = new List<Point>();
            for (int y = 0; y < lines; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    board[x, y] = input[y][x].ToString();

                    if (board[x, y] == "@")
                    {
                        initialPosition.X = x;
                        initialPosition.Y = y;
                    }

                    if (board[x, y] == "T")
                    {
                        teleTransporters.Add(new Point(x, y));
                    }
                }
            }

            BenderGame bg = new BenderGame();
            bg.Board = board;
            bg.Bender.Position = initialPosition;
            bg.TeleTransporters = teleTransporters;
            var orders = bg.Move();

            Assert.AreEqual(10, orders.Count);
            Assert.AreEqual("SOUTH", orders[0]);
            Assert.AreEqual("SOUTH", orders[1]);
            Assert.AreEqual("SOUTH", orders[2]);
            Assert.AreEqual("SOUTH", orders[3]);
            Assert.AreEqual("EAST", orders[4]);
            Assert.AreEqual("EAST", orders[5]);
            Assert.AreEqual("EAST", orders[6]);
            Assert.AreEqual("EAST", orders[7]);
            Assert.AreEqual("EAST", orders[8]);
            Assert.AreEqual("EAST", orders[9]);
        }

        [TestMethod]
        public void TestModeInverseur()
        {

            int lines = 10;
            int columns = 10;
            List<string> input = new List<string>
            {
                "##########",
                "#    I   #",
                "#        #",
                "#       $#",
                "#       @#",
                "#        #",
                "#       I#",
                "#        #",
                "#        #",
                "##########"};

            Point initialPosition = new Point(0, 0);
            string[,] board = new string[columns, lines];
            List<Point> teleTransporters = new List<Point>();
            for (int y = 0; y < lines; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    board[x, y] = input[y][x].ToString();

                    if (board[x, y] == "@")
                    {
                        initialPosition.X = x;
                        initialPosition.Y = y;
                    }

                    if (board[x, y] == "T")
                    {
                        teleTransporters.Add(new Point(x, y));
                    }
                }
            }

            BenderGame bg = new BenderGame();
            bg.Board = board;
            bg.Bender.Position = initialPosition;
            bg.TeleTransporters = teleTransporters;
            var orders = bg.Move();

            Assert.AreEqual(27, orders.Count);
            Assert.AreEqual("SOUTH", orders[0]);
            Assert.AreEqual("SOUTH", orders[1]);
            Assert.AreEqual("SOUTH", orders[2]);
            Assert.AreEqual("SOUTH", orders[3]);
            Assert.AreEqual("WEST", orders[4]);
            Assert.AreEqual("WEST", orders[5]);
            Assert.AreEqual("WEST", orders[6]);
            Assert.AreEqual("WEST", orders[7]);
            Assert.AreEqual("WEST", orders[8]);
            Assert.AreEqual("WEST", orders[9]);
            Assert.AreEqual("WEST", orders[10]);
            Assert.AreEqual("NORTH", orders[11]);
            Assert.AreEqual("NORTH", orders[12]);
            Assert.AreEqual("NORTH", orders[13]);
            Assert.AreEqual("NORTH", orders[14]);
            Assert.AreEqual("NORTH", orders[15]);
            Assert.AreEqual("NORTH", orders[16]);
            Assert.AreEqual("NORTH", orders[17]);
            Assert.AreEqual("EAST", orders[18]);
            Assert.AreEqual("EAST", orders[19]);
            Assert.AreEqual("EAST", orders[20]);
            Assert.AreEqual("EAST", orders[21]);
            Assert.AreEqual("EAST", orders[22]);
            Assert.AreEqual("EAST", orders[23]);
            Assert.AreEqual("EAST", orders[24]);
            Assert.AreEqual("SOUTH", orders[25]);
            Assert.AreEqual("SOUTH", orders[26]);
        }

    }





    public class BenderGame
    {
        public List<Point> TeleTransporters { get; set; }
        public BenderGame()
        {
            Bender = new Robot();
            TeleTransporters = new List<Point>();
        }
        public Robot Bender { get;set;}

        public string[,] Board { get; set; }

        
        
        public List<string> Move()
        {
            List<string> trace=new List<string>();
            List<Point> traject = new List<Point>();
            traject.Add(Bender.Position);

            while (true)
            {
                string currentCase = Board[Bender.Position.X, Bender.Position.Y];
                //Handle Teleportation
                if (currentCase == "T")
                {
                    //get realcase 
                    var p = TeleTransporters.First(t => t.X != Bender.Position.X && t.Y != Bender.Position.Y);
                    
                    Bender.Position = p;
                }//handle Casseur mode
                else if (currentCase == "B")
                {
                    //get realcase 
                    Bender.IsCasseur = !Bender.IsCasseur;
                }//exit
                else if(currentCase == "I")
                {
                    Bender.InvertDirection();
                }
                else if (currentCase == "$")
                {
                    break;
                }




                currentCase = Board[Bender.Position.X, Bender.Position.Y];

                var policy = GetDirectionPolicy(currentCase);


                //try to move
                int nextX = Bender.Position.X;
                int nextY = Bender.Position.Y;

                string order = GetNextMove(policy,ref nextY,ref nextX);
                trace.Add(order);
                //loop
                if (traject.Count>Board.Length * Board.Rank)
                {
                    trace.Clear();
                    trace.Add("LOOP");
                    break;
                }

                traject.Add(new Point(nextX,nextY));
                //Handle special case
                if (Board[nextX, nextY] == "X")
                {
                    traject.Clear();
                    Board[nextX, nextY] = " ";
                }

                //then move!
                Bender.Position= new Point(nextX, nextY);
            }

            return trace;
        }

        private string GetNextMove(List<RobotDirection> policy,ref int nextY,ref int nextX)
        {
           
            string order = string.Empty;
            foreach (var nextMove in policy)
            {
                int tmpX = nextX;
                int tmpY = nextY;
                order = nextMove.ToString();
                if (nextMove == RobotDirection.SOUTH)
                    tmpY = nextY+1;
                else if (nextMove == RobotDirection.NORTH)
                    tmpY = nextY-1;
                else if (nextMove == RobotDirection.EAST)
                    tmpX = nextX + 1;
                else if (nextMove == RobotDirection.WEST)
                    tmpX = nextX - 1;

                string potentialPosition = Board[tmpX, tmpY];

                if (PositionIsReachable(potentialPosition))
                {
                    Bender.OverRideDirection = nextMove;
                    nextX = tmpX;
                    nextY = tmpY;
                    break;
                }
            }

            return order;
        }

        private List<RobotDirection> GetDirectionPolicy(string currentCase)
        {
            List<RobotDirection> policy = new List<RobotDirection>();
            if (currentCase == "S")
            {
                Bender.OverRideDirection = RobotDirection.SOUTH;
                policy.Add(RobotDirection.SOUTH);
            }
            else if (currentCase == "N")
            {
                Bender.OverRideDirection = RobotDirection.NORTH;
                policy.Add(RobotDirection.NORTH);
            }
            else if (currentCase == "E")
            {
                Bender.OverRideDirection = RobotDirection.EAST;
                policy.Add(RobotDirection.EAST);
            }
            else if (currentCase == "W")
            {
                Bender.OverRideDirection = RobotDirection.WEST;
                policy.Add(RobotDirection.WEST);
            }
            
            else if(Bender.OverRideDirection != RobotDirection.None)
            {
                policy.Add(Bender.OverRideDirection);
            }

            policy.AddRange(Bender.PriorityDirections);
            return policy;
        }


        public bool PositionIsReachable(string cellValue)
        {
            if (cellValue == "#" || (cellValue == "X" && !Bender.IsCasseur))
                return false;
            else
                return true;
        }

    }


    public class Robot
    {
        public RobotDirection[] PriorityDirections { get; private set; }

        public RobotDirection OverRideDirection { get; set; }

        public bool IsCasseur { get; set; }

        public Robot()
        {
            OverRideDirection = RobotDirection.None;
            PriorityDirections = new []{ RobotDirection.SOUTH, RobotDirection.EAST, RobotDirection.NORTH, RobotDirection.WEST };
            IsCasseur = false;
        }

        public Point Position { get; set; }
        public void InvertDirection()
        {
            if (PriorityDirections[0] == RobotDirection.SOUTH)
            {
                PriorityDirections[0] = RobotDirection.WEST;
                PriorityDirections[1] = RobotDirection.NORTH;
                PriorityDirections[2] = RobotDirection.EAST;
                PriorityDirections[3] = RobotDirection.SOUTH;
            }
            else
            {
                PriorityDirections[0] = RobotDirection.SOUTH;
                PriorityDirections[1] = RobotDirection.EAST;
                PriorityDirections[2] = RobotDirection.NORTH;
                PriorityDirections[3] = RobotDirection.WEST;
            }
        }
    }



    public enum RobotDirection
    {
        NORTH, SOUTH, EAST, WEST, None
    }


}
