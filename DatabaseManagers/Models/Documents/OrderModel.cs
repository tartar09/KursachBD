
namespace DatabaseManagers
{
	public class OrderModel : DataModel
	{
		public int BookCatalogId { get; set; }
		public int ReaderCardId { get; set; }
		public string Date { get; set; }


		public virtual BookCatalogModel BookCatalog { get; set; }
		public virtual ReaderCard ReaderCard { get; set; }
	}
}
