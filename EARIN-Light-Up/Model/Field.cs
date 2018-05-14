using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace EARIN_Light_Up
{
    public class Field
    {
        public FieldType Type { get; set; }
        public uint Id { get; set; }
        public uint Row { get; set; }
        public uint Column { get; set; }
        public byte Visits { get; set; }
		//public int Profit { get; set; }
		public Tuple<int?, int?> priorityPair { get; set; }
    }
}
