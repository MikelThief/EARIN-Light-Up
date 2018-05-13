using System;
using System.Drawing;
using System.Numerics;
using Console = Colorful.Console;

namespace EARIN_Light_Up.Algorithm
{
    public class DFS
    {
        private uint _numberOfFields { get; set; }
        private Board Board { get; set; }
		private BigInteger Visits { get; set; }

        public void Perform(Board srcboard, uint depth, uint numberOfFields)
        {
			Console.WriteLine();
	        Console.WriteLine();
	        Console.WriteLine();
			_numberOfFields = numberOfFields;
            Board = srcboard;

	        Search(0);
	        Console.WriteLine("DFS traversed whole tree.", Color.Aqua);
        }

        private void Search(uint depth)
        {
	        if (Board.ValidateSolution())
	        {
				Console.WriteLine("Visits:" + ++Visits, Color.GreenYellow);
		        Board.Draw();
				Console.WriteLine();
				return;
	        }

	        for (uint counter = depth; counter < _numberOfFields; ++counter)
	        {
		        if (Board.ValidateMove(counter))
		        {
					Board.PutBulb(counter);
			        Visits += 1;
					Search(1+counter);
					Board.RemoveBulb(counter);
			        Visits += 1;
				}
	        }
		}
    }
}
