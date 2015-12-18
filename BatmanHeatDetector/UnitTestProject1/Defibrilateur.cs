using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{

 
    public class Defibrilateur
    {
        static double GetDistance(double alo, double ala, double blo, double bla)
        {
            double x = (blo - alo) * Math.Cos((ala + bla) / 2.0);
            double y = bla - ala;

            return Math.Sqrt(x * x + y * y) * 6371;
        }


        static double DegreeToRadian(string d)
        {
            string[] ds = d.Split(','); 
            double deg = double.Parse(ds[0]) + double.Parse("0." + ds[1]); 

            return Math.PI * deg / 180.0;
        }

        public void Main(string[] args)
        {

            double LON = DegreeToRadian(Console.ReadLine());    
            double LAT = DegreeToRadian(Console.ReadLine());   
            int N = int.Parse(Console.ReadLine());              

            string name = null;                
            double nearest = double.MaxValue;   
            for (int i = 0; i < N; i++)
            {
                string[] DEFIB = Console.ReadLine().Split(';'); 

                double dlo = DegreeToRadian(DEFIB[DEFIB.Length - 2]); 
                double dla = DegreeToRadian(DEFIB[DEFIB.Length - 1]); 
                double dis = GetDistance(LON, LAT, dlo, dla);       

                if (dis < nearest)
                { 
                    name = DEFIB[1];
                    nearest = dis;
                }
            }

            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(name);
        }
    }

}
