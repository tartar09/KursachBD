using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManagers
{
	public class BooksReaders : DataModel
	{
		public string Date { get; set; }
		public int BookId { get; set; }
		public int ReaderCardId { get; set; }

		public virtual ReaderCard ReaderCard { get; set; }
	}
}
