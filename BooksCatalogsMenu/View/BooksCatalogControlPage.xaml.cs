using DatabaseManagers;
using System.Windows.Controls;
using System.Windows.Data;

namespace BooksCatalogsMenu
{
	/// <summary>
	/// Логика взаимодействия для BookControlPage.xaml
	/// </summary>
	public partial class BooksCatalogControlPage : Page
	{
		public BooksCatalogControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new BooksCatalogControlPageModelView(new Button[] { null, AddButton, EditButton, DeleteButton, LogOutButton, GiveButton, FindButton, ExportButton },
				MainTable,
				accessInfo
			);

			Binding binding = new Binding("BookName");
			binding.Source = DataContext;
			binding.Mode = BindingMode.TwoWay;
			BookName.SetBinding(TextBox.TextProperty, binding);
		}
	}
}
