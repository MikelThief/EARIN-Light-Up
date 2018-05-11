using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using EARIN_Light_Up.Misc;

namespace EARIN_Light_Up.Algorithm
{
    public class DFS
    {
        private uint _numberOfFields { get; set; }
        private Board Board { get; set; }

		List<Board> solutions = new List<Board>();

        public void Perform(Board srcboard, uint depth, uint numberOfFields)
        {
            _numberOfFields = numberOfFields;
            Board = srcboard;
            Search(depth);
        }

        private void Search(uint depth)
        {
	        if (depth == 0)
	        {
		        solutions.Add(Board);
		        return;
	        }
			for (uint counter = _numberOfFields-depth; counter < _numberOfFields; counter++)
            {
	            if (Validator.ValidateMove(Board, counter))
	            {
		            Board.PutBulb(counter);
		            Search(--depth);
		            Board.RemoveBulb(counter);
	            }
            }
		}
    }
}
