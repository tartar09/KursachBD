using System.Collections.Generic;

namespace DatabaseManagers
{
	public class BookCatalogModel : DataModel
	{
		public int Count { get; set; }
		public int AvailableCount { get; set; }

		public int BookTypeId { get; set; }

		public virtual BookTypeModel BookType { get; set; }
		public virtual ICollection<BookModel> Books { get; set; }
		public virtual ICollection<AuthorsBooks> AuthorsBooks { get; set; }
	}
}
