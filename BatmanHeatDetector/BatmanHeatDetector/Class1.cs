using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatmanHeatDetector
{
    public class Class1
    {

        public void Foo()
        {
            string bombPos = "UR";
            int xPos = 0;
            int yPos = 0;

            if (bombPos.Contains("U"))
                yPos--;
            else if(bombPos.Contains("D"))
                yPos++;


            if (bombPos.Contains("L"))
                xPos--;
            else if (bombPos.Contains("R"))
                xPos++;
        }
    }
}
