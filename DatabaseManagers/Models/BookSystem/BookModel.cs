
namespace DatabaseManagers
{
	public class BookModel : DataModel
	{
		public int StorageNumber { get; set; }
		public int StandNumber { get; set; }
		public int ShelfNumber { get; set; }

		public string Name { get; set; }
		public string Date { get; set; }
		public string PublishHouseName { get; set; }

		public int CategoryId { get; set; }
		public int CatalogId { get; set; }

		public virtual BookCategoryModel Category { get; set; }
		public virtual BookCatalogModel Catalog { get; set; }
	}
}
