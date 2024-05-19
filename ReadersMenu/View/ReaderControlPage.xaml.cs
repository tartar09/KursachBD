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

namespace ReadersMenu
{
	/// <summary>
	/// Логика взаимодействия для ReaderControlPage.xaml
	/// </summary>
	public partial class ReaderControlPage : Page
	{
		public ReaderControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new ReaderControlPageModelView(new Button[5] { null, AddButton, EditButton, DeleteButton, LogOutButton },
				MainTable,
				accessInfo
				);
		}
	}
}
