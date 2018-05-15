using System;
using System.Collections.Concurrent;
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
		private Board PlainBoard { get; set; }
		private BigInteger Visits { get; set; }
		private long MaxProfit { get; set; }
		private SimplePriorityQueue<Board> openSet;	// Priority in list is Board.CurrentProfit
		private SimplePriorityQueue<Board> closedSet;
		private SimplePriorityQueue<List<uint>> openSetexp;
		private SimplePriorityQueue<List<uint>> closedSetexp;

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
			this.PlainBoard = new Board(board);
			this.MaxProfit = Board.GetProfit();
			//openSet = new SimplePriorityQueue<Board>();
			//closedSet = new SimplePriorityQueue<Board>();
		}

		public void Perform()
		{
			// add root as a first element (frontier)
			//openSet.Enqueue(Board, Board.CurrentProfit);
			
			openSetexp.Enqueue(PlainBoard.GetBulbsLayerList(), PlainBoard.CurrentProfit);

			while (openSetexp.Count > 0)
			{
				var currentBulbLayer = openSetexp.Dequeue();

				var currentBoard = new Board(PlainBoard);
				currentBoard.PutBulbsLayer(currentBulbLayer);
				//Board currentBoard = openSet.Dequeue();

				if (currentBoard.ValidateSolution())
					solutions.Add(currentBoard);

				var successors = currentBoard.GetSuccessors();
			}
		}
	}
}
