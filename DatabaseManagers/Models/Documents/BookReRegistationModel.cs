using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagers
{
	public class BookReRegistationModel : DataModel
	{
		public int BookId { get; set; }
		public int? ReaderCardId { get; set; }


		public string Date { get; set; }
		public virtual BookModel Book { get; set; }
		public virtual ReaderCard ReaderCard { get; set; }
	}
}
