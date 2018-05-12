using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using EARIN_Light_Up.Misc;
using Console = Colorful.Console;

namespace EARIN_Light_Up.Algorithm
{
    public class DFS
    {
        private uint _numberOfFields { get; set; }
        private Board Board { get; set; }



        public void Perform(Board srcboard, uint depth, uint numberOfFields)
        {
	        Console.WriteLine();
	        Console.WriteLine();
	        Console.WriteLine();
			_numberOfFields = numberOfFields;
            Board = srcboard;

	        Search(0);
		}

        private void Search(uint depth)
        {
	        if (Board.ValidateSolution())
	        {
				Console.WriteLine("Solution Found:", Color.GreenYellow);
		        Board.Draw();
				Console.WriteLine();
				return;
	        }

	        for (uint counter = depth; counter < _numberOfFields; ++counter)
	        {
		        if (Board.ValidateMove(counter))
		        {
					Board.PutBulb(counter);
					Search(1+counter);
					Board.RemoveBulb(counter);
				}
	        }
		}
    }
}
