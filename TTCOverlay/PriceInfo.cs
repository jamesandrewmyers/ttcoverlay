using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCOverlay
{
	public class PriceInfo
	{
		public String Rarity { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Trait { get; set; }
		public int Vouchers { get; set; }
		public int Level { get; set; }
		public decimal Min { get; set; }
		public decimal Avg { get; set; }
		public decimal Max { get; set; }
		public decimal Low { get; set; }
		public decimal High { get; set; }
		public int Listings { get; set; }
		public int Items { get; set; }
	}
}
