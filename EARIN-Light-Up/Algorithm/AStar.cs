using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using Priority_Queue;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace EARIN_Light_Up.Algorithm
{
	class AStar
	{
		private List<Board> solutions;
		private uint _numberOfFields { get; set; }
		private Board Board { get; set; }
		private Board PlainBoard { get; set; }
		private BigInteger Visits { get; set; }
		private long MaxProfit { get; set; }
		private SimplePriorityQueue<Board> openSet;	// Priority in list is Board.CurrentProfit
		private SimplePriorityQueue<Board> closedSet;
		private SimplePriorityQueue<List<int>> openSetexp;
		private List<List<int>> closedSetexp;

		public AStar(Board board)
		{
			this.solutions = new List<Board>();
			this.Board = new Board(board);
			this.PlainBoard = new Board(board);
			this.MaxProfit = Board.GetProfit();
			//openSet = new SimplePriorityQueue<Board>();
			//closedSet = new SimplePriorityQueue<Board>();
			openSetexp = new SimplePriorityQueue<List<int>>();
			closedSetexp = new List<List<int>>();

		}

		public async void Perform()
		{
			Console.WriteLine("\tStarted A*-based solver.");

			// add root as a first element (frontier)
			openSetexp.Enqueue(PlainBoard.GetBulbsLayer(), 0);

			while (openSetexp.Count > 0)
			{
				var currentBulbLayer = openSetexp.Dequeue();

				var currentBoard = new Board(PlainBoard);
				currentBoard.PutBulbsLayer(currentBulbLayer);

                if (currentBoard.ValidateSolution())
				{
					currentBoard.Visits = this.Visits;
					solutions.Add(currentBoard);
					break;
				}

				Visits += 1;
				var successors = currentBoard.GetSuccessors();

                foreach (var successorBulbLayer in successors)
                {
                    var successorBoard = new Board(PlainBoard);
                    successorBoard.PutBulbsLayer(successorBulbLayer);

					openSetexp.Enqueue(successorBulbLayer, (float) (successorBoard.GetProfit() + 0.01 * successorBoard.GetNumberOfLitFields()));
				}
            }

			foreach (var board in solutions)
			{
				Console.WriteLine("A* solution: ", Color.Orchid);
				Console.WriteLine("Visits:" + board.Visits, Color.Orchid);
				board.Draw();
            }
		}
	}
}
