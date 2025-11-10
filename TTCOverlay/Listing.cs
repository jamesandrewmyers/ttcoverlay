using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCOverlay
{
	public class Listing
	{
		public string Rarity { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Trait { get; set; }
		public int Vouchers { get; set; }
		public int Level { get; set; }
		public string Location { get; set; }
		public string Guild { get; set; }
		public int UnitPrice { get; set; }
		public int Quantity { get; set; }
		public int TotalPrice { get; set; }
		public int MinutesOld { get; set; }
	}
}
