using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelViewSystem;
using DatabaseManagers;

namespace UniversityLibraryProject
{
	public class MainWindowModelView : BaseUIModelView
	{
		public MainWindowModelView() : base() 
		{ }

		public MainWindowModelView(UserModel user)
		{
			CurrentUser = user;
		}

		public List<CustomMenuItem> LoadMenu()
		{
			try
			{
				var items = DatabaseManager.GetInstance().LoadItems(CurrentUser);

				MenuCreator menuCreator = new MenuCreator();
				return menuCreator.GetMenuItems(items);
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
				return new List<CustomMenuItem>(0);
			}
		}

	}
}
