using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace EARIN_Light_Up.Misc
{
    public static class Validator
    {
        public static bool ValidateMove(Board board, uint fieldID)
        {
           // TODO: Write validation rules for a single move
            
            return false;
        }
        public static bool ValidateBoard(Board board)
        {
            return board.ValidateSolution();
        }
    }
}
