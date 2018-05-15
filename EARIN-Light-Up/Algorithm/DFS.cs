using System;
using System.Collections.Generic;
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
	    private List<Board> solutions;

	    public DFS(Board board)
	    {
			this.solutions = new List<Board>();
		    this.Board = new Board(board);
			this._numberOfFields = board.size * board.size;
	    }
        public void Perform()
        {
			Console.WriteLine();
	        Console.WriteLine();
	        Console.WriteLine();

	        Search(0);
	        foreach (var solution in solutions)
	        {
		        Console.WriteLine("Visits:" + solution.Visits, Color.Green);
                solution.Draw();
	        }
	        Console.WriteLine("DFS traversed whole tree.", Color.Aqua);
	        Console.WriteLine();
        }

        private void Search(uint depth)
        {
	        if (Board.ValidateSolution())
	        {
				var tempBoard = new Board(Board);
		        tempBoard.Visits = this.Visits;
				solutions.Add(tempBoard);
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
