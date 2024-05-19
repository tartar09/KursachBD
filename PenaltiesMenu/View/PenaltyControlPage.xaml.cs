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

namespace PenaltiesMenu
{
	/// <summary>
	/// Логика взаимодействия для PenaltyControlPage.xaml
	/// </summary>
	public partial class PenaltyControlPage : Page
	{
		public PenaltyControlPage(AccessInfo accessInfo)
		{
			InitializeComponent();
			DataContext = new PenaltyControlPageModelView(new Button[] { null, AddButton, EditButton, DeleteButton, LogOutButton, ExportButton },
				MainTable,
				accessInfo
			);
		}
	}
}
