
namespace DatabaseManagers
{
	public class MenuItemModel : DataModel
	{
		public int ParentId { get; set; }
		public string Name { get; set; }
		public string DLL { get; set; }
		public string Key { get; set; }
		public int Order { get; set; }
	}
}
