using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    public class MarsLander
    {
        private static int EPSILON = 5;
        private static int MAX_VERTICAL_SPEED = 40;
        private static int MAX_HORIZONTAL_SPEED = 20;
        private static double GRAVITY = 3.711;
        private static int SECURITY_DISTANCE_FROM_FLAT_GROUND = 50;


        static void Main(string[] args)
        {
            string[] inputs;
            int flatGroundLeftX = -1;
            int flatGroundRightX = -1;
            int flatGroundY = -1;
            int previousPointX = -1;
            int previousPointY = -1;

            int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
            for (int i = 0; i < surfaceN; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
                int landY = int.Parse(inputs[1]); // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.

                if (previousPointY == landY)
                {
                    flatGroundLeftX = previousPointX;
                    flatGroundRightX = landX;
                    flatGroundY = landY;
                }
                else
                {
                    previousPointX = landX;
                    previousPointY = landX;
                }
            }

            // game loop
            while (true)
            {
                inputs = Console.ReadLine().Split(' ');
                int X = int.Parse(inputs[0]);
                int Y = int.Parse(inputs[1]);
                int hSpeed = int.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
                int vSpeed = int.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
                int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
                int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
                int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                if (isMarsLanderFlyingOverFlatGround(X, flatGroundLeftX, flatGroundRightX))
                {
                    if (isMarsLanderAboutToLand(Y, flatGroundY))
                    {
                        rotate = 0;
                        power = 3;
                    }
                    else if (areMarsLanderSpeedLimitsSatisfied(hSpeed, vSpeed))
                    {
                        rotate = 0;
                        power = 2;
                    }
                    else
                    {
                        rotate = calculateRotationToSlowDownMarsLander(hSpeed, vSpeed);
                        power = 4;
                    }
                }
                else
                {

                    if (isMarsLanderFlyingInTheWrongDirection(X, hSpeed, flatGroundLeftX, flatGroundRightX) 
                        || isMarsLanderFlyingTooFastTowardsFlatGround(hSpeed))
                    {
                        rotate = calculateRotationToSlowDownMarsLander(hSpeed, vSpeed);
                        power= 4;
                    }
                    else if (isMarsLanderFlyingTooSlowTowardsFlatGround(hSpeed))
                    {
                        rotate = calculateRotationToSpeedUpMarsLanderTowardsFlatGround(X, flatGroundLeftX, flatGroundRightX);
                        power = 4;
                    }
                    else
                    {
                        rotate = 0;
                        power = calculateThrustPowerToFlyTowardsFlatGround(vSpeed);
                    }
                    
                }
                
                Console.WriteLine(rotate + " " + power); // rotate power. rotate is the desired rotation angle. power is the desired thrust power.
            }
        }



        private static bool isMarsLanderFlyingOverFlatGround(int marsLanderX, int flatGroundLeftX, int flatGroundRightX)
        {
            return marsLanderX >= flatGroundLeftX && marsLanderX <= flatGroundRightX;
        }

        private static bool isMarsLanderAboutToLand(int marsLanderY, int flatGroundY)
        {
            return marsLanderY < flatGroundY + SECURITY_DISTANCE_FROM_FLAT_GROUND;
        }

        private static int ToDegrees(double rotationAsRadians)
        {
            return (int) (Math.PI* rotationAsRadians / 180.0);
        }

        private static bool areMarsLanderSpeedLimitsSatisfied(int marsLanderHorizontalSpeed, int marsLanderVerticalSpeed)
        {
            return Math.Abs(marsLanderHorizontalSpeed) <= (MAX_HORIZONTAL_SPEED - EPSILON)
                && Math.Abs(marsLanderVerticalSpeed) <= (MAX_VERTICAL_SPEED - EPSILON);
        }

        private static int calculateRotationToSlowDownMarsLander(int horizontalSpeed, int verticalSpeed)
        {
            double speed = Math.Sqrt(Math.Pow(horizontalSpeed, 2) + Math.Pow(verticalSpeed, 2));
            double rotationAsRadians = Math.Asin((double)horizontalSpeed / speed);
            return ToDegrees(rotationAsRadians);
        }


        private static bool isMarsLanderFlyingInTheWrongDirection(int marsLanderX, int marsLanderHorizontalSpeed, int flatGroundLeftX, int flatGroundRightX)
        {

            if (marsLanderX < flatGroundLeftX && marsLanderHorizontalSpeed < 0)
            {
                return true;
            }

            if (marsLanderX > flatGroundRightX && marsLanderHorizontalSpeed > 0)
            {
                return true;
            }

            return false;
        }

        private static bool isMarsLanderFlyingTooFastTowardsFlatGround(int marsLanderHorizontalSpeed)
        {
            return Math.Abs(marsLanderHorizontalSpeed) > (MAX_HORIZONTAL_SPEED * 4);
        }

        private static bool isMarsLanderFlyingTooSlowTowardsFlatGround(int marsLanderHorizontalSpeed)
        {
            return Math.Abs(marsLanderHorizontalSpeed) < (MAX_HORIZONTAL_SPEED * 2);
        }

        private static int calculateRotationToSpeedUpMarsLanderTowardsFlatGround(int marsLanderX, int flatGroundLeftX, int flatGroundRightX)
        {

            if (marsLanderX < flatGroundLeftX)
            {
                return -ToDegrees(Math.Acos(GRAVITY / 4.0));
            }

            if (marsLanderX > flatGroundRightX)
            {
                return +ToDegrees(Math.Acos(GRAVITY / 4.0));
            }

            return 0;
        }

        private static int calculateThrustPowerToFlyTowardsFlatGround(int marsLanderVerticalSpeed)
        {
            return (marsLanderVerticalSpeed >= 0) ? 3 : 4;
        }
    }



}
