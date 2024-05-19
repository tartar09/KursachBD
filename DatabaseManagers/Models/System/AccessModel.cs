
namespace DatabaseManagers
{
	/// <summary>
	/// Информация о доступе к меню
	/// </summary>
	public struct AccessInfo
	{
		public bool Read { get; private set; }
		public bool Add { get; private set; }
		public bool Edit { get; private set; }
		public bool Delete { get; private set; }

		public AccessInfo(bool r, bool w, bool e, bool d)
		{
			Read = r;
			Add = w;
			Edit = e;
			Delete = d;
		}

		public static AccessInfo LoadFromItem(AccessModel aM) => new AccessInfo(aM.Read > 0, aM.Add > 0, aM.Edit > 0, aM.Delete > 0);

		public bool HaveAccess => Read || Add || Edit || Delete;
	}


	public class AccessModel : DataModel
	{
		public int UserId { get; set; }
		public int MenuId { get; set; }


		public int Read { get; set; }
		public int Add { get; set; }
		public int Edit { get; set; }
		public int Delete { get; set; }

		// Связанные данные
		public virtual UserModel User { get; set; }
		public virtual MenuItemModel MenuItem { get; set; }
	}
}
