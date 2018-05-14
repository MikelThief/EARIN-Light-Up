using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Priority_Queue;
using System.Text;
using System.Threading.Tasks;

namespace EARIN_Light_Up.Algorithm
{
	class AStar
	{
		private uint _numberOfFields { get; set; }
		private Board Board { get; set; }
		private BigInteger Visits { get; set; }
		private ulong MaxProfit { get; set; }
		private FastPriorityQueue<Board> openSet;
		private FastPriorityQueue<Board> closedSet;

		// goal node does not exist - it is computed on the fly

		public AStar(Board board)
		{
			//int capacity = default;
			//for (uint counter = 0; counter < Board.size * Board.size; counter++)
			//{
			//	if (Board.GetField(counter).Type == FieldType.Empty)
			//		capacity += 1;

			//}
			this.Board = new Board(board.size);
			this.Board.CopyBoard(board);
			this.MaxProfit = Board.GetMaxProfit();
			openSet = new FastPriorityQueue<Board>(Int32.MaxValue);
			closedSet = new FastPriorityQueue<Board>(Int32.MaxValue);
		}

		public void Perform(Board srcBoard)
		{
			openSet.Enqueue(srcBoard, srcBoard.CurrentProfit);

			while (openSet.Count > 0)
			{
				Board currentBoard = openSet.Dequeue();

				if (currentBoard.ValidateSolution())
				{

				}
			}
		}
	}
}
