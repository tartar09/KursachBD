
namespace DatabaseManagers
{
	public class LibraryCard : DataModel
	{
		public int BookId { get; set; }
		public int ReaderId { get; set; }

		public virtual BookModel Book { get; set; }
		public virtual ReaderModel Reader { get; set; }
	}
}
