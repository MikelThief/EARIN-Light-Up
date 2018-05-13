using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EARIN_Light_Up
{
    public class Field
    {
        public FieldType Type { get; set; }
        public uint Id { get; set; }
        public uint Row { get; set; }
        public uint Column { get; set; }
        public byte Visits { get; set; }
		public byte Profit { get; set; }
		
    }
}
