using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using Priority_Queue;
using System.Text;
using System.Threading.Tasks;

namespace EARIN_Light_Up.Algorithm
{
	class AStar
	{
		private List<Board> solutions;
		private uint _numberOfFields { get; set; }
		private Board Board { get; set; }
		private BigInteger Visits { get; set; }
		private long MaxProfit { get; set; }
		private StablePriorityQueue<Board> openSet;	// Priority in list is Board.CurrentProfit
		private StablePriorityQueue<Board> closedSet;

		// goal node does not exist - it is computed on the fly	= ValidateSolution()
		// h() - estimated distance to the goal = CurrentProfit in Board. Should approach 0 for a success

		public AStar(Board board)
		{
			//int capacity = default;
			//for (uint counter = 0; counter < Board.size * Board.size; counter++)
			//{
			//	if (Board.GetField(counter).Type == FieldType.Empty)
			//		capacity += 1;

			//}
			this.solutions = new List<Board>();
			this.Board = new Board(board);
			this.MaxProfit = Board.GetProfit();
			openSet = new StablePriorityQueue<Board>(Int32.MaxValue);
			closedSet = new StablePriorityQueue<Board>(Int32.MaxValue);
		}

		public void Perform(Board srcBoard)
		{
			// add root as a first element (frontier)
			openSet.Enqueue(srcBoard, srcBoard.CurrentProfit); 

			while (openSet.Count > 0)
			{
				Board currentBoard = openSet.Dequeue();

				if (currentBoard.ValidateSolution())
					solutions.Add(currentBoard);

				var successors = new List<Board>();
			}
		}
	}
}
