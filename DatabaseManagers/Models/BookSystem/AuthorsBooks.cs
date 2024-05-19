
namespace DatabaseManagers
{
	public class AuthorsBooks : DataModel
	{
		public int AuthorId { get; set; }
		public int BookCatalogId { get; set; }


		public virtual AuthorModel Author { get; set; }
		public virtual BookCatalogModel BookCatalog { get; set; }
	}
}
