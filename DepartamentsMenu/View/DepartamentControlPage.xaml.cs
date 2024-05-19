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
using DatabaseManagers;
using ModelViewSystem;

namespace DepartamentsMenu
{
	/// <summary>
	/// Логика взаимодействия для DepartamentControlPage.xaml
	/// </summary>
	public partial class DepartamentControlPage : Page
	{
		public DepartamentControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new DepartmentControlPageModelView(new Button[5] { null, AddButton, EditButton, DeleteButton, LogOutButton },
				MainTable,
				accessInfo
				);
		}
	}
}
