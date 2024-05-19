using DatabaseManagers;
using System.Windows.Controls;

namespace OrdersMenu
{
	/// <summary>
	/// Логика взаимодействия для OrderControlPage.xaml
	/// </summary>
	public partial class OrderControlPage : Page
	{
		public OrderControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new OrdersControlPageModelView(new Button[] { null, AddButton, EditButton, DeleteButton, LogOutButton },
				MainTable,
				accessInfo
			);
		}
	}
}
