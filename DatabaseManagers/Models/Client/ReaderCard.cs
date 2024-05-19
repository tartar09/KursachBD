using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagers
{
	public class ReaderCard: DataModel
	{
		public string Surname { get; set; }
		public string Name { get; set; }
		public string Patronymic { get; set; }

		public string RegDate { get; set; }
		public string ReRegDate { get; set; }
	}
}
