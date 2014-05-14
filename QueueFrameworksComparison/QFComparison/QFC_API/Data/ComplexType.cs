using System.Collections.Generic;

namespace QFC.Contracts.Data
{
	public class ComplexType
	{
		public int Id { get; set; }
		public string Street { get; set; }
		public List<string> Names { get; set; }
	}
}
