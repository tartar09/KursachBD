
namespace DatabaseManagers
{
	public class BookTypeModel : DataModel
	{
		public string Name { get; set; }
		public int AccessTime { get; set; }
		public int OnlyRead { get; set; }
	}
}
