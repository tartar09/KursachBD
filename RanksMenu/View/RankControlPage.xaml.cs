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

namespace RanksMenu
{
	/// <summary>
	/// Логика взаимодействия для RankControlPage.xaml
	/// </summary>
	public partial class RankControlPage : Page
	{
		public RankControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new RankControlPageModelView(
				new Button[5] { null, AddButton, EditButton, DeleteButton, LogOutButton },
				MainTable,
				accessInfo
			);
		}
	}
}
