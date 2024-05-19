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

namespace BookCategoryMenu
{
	/// <summary>
	/// Логика взаимодействия для BookCategoryControlPage.xaml
	/// </summary>
	public partial class BookCategoryControlPage : Page
	{
		public BookCategoryControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new BookCategoryControlPageModelView(new Button[5] { null, AddButton, EditButton, DeleteButton, LogOutButton },
				MainTable,
				accessInfo
				);
		}
	}
}
