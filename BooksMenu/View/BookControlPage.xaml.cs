using DatabaseManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BooksMenu
{
	/// <summary>
	/// Логика взаимодействия для BookControlPage.xaml
	/// </summary>
	public partial class BookControlPage : Page
	{
		public BookControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new BookControlPageModelView(new Button[6] { ReadButton, AddButton, EditButton, DeleteButton, LogOutButton, GetBookButton },
				MainTable,
				accessInfo
			);
		}
	}
}
