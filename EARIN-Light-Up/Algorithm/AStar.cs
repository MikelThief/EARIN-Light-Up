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
		private FastPriorityQueue<Field> openSet;

		public AStar(Board board)
		{
			this.Board = board;
			this.MaxProfit = Board.GetMaxProfit();
			openSet = new FastPriorityQueue<Field>((int) (board.size * board.size));
		}

		public void Perform(Board srcBoard, uint numberOfField)
		{
		 	
		}
	}
}
