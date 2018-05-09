﻿using System;
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
        public BigInteger Visits { get; private set; }
        public BigInteger UniqueNodesVisited { get; private set; }
        public bool Solved { get; private set; }
        private uint _numberOfFields { get; set; }
        private Board Board { get; set; }

        public void Perform(Board board, uint depth, uint numberOfFields)
        {
            _numberOfFields = numberOfFields;
            Search(depth);
            this.Visits = board.Visits;
            this.UniqueNodesVisited = Board.UniqueNodesVisited;
        }

        private void Search(uint depth)
        {
            if (Validator.ValidateBoard(Board) || depth == 0)
                return;
            for(uint counter = _numberOfFields-depth; counter < _numberOfFields; counter++)
            {
                if (Validator.ValidateMove(Board, counter))
                {
                    Board.PutBulb(counter);
                    Search(depth - 1);
                    Board.RemoveBulb(counter);
                }
            }
        }
    }
}