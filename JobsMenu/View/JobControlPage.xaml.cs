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

namespace JobsMenu
{
	/// <summary>
	/// Логика взаимодействия для JobControlPage.xaml
	/// </summary>
	public partial class JobControlPage : Page
	{
		public JobControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new JobModelControlPageModelView(new Button[5] { null, AddButton, EditButton, DeleteButton, LogOutButton },
				MainTable,
				accessInfo
				);
		}
	}
}
