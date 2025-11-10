using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTCOverlay
{
	public class Suggestion
	{
		private const string LABEL_FIELD = "label";
		private const string ID_FIELD = "ItemID";

		public string Label { get; set; }
		public string ID { get; set; }

		public Suggestion(Dictionary<string,object> input)
		{
			this.Label = input[LABEL_FIELD].ToString();
			this.ID = input[ID_FIELD].ToString();
		}
	}
}
