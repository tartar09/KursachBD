using ModelViewSystem;
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
using System.Windows.Shapes;

namespace UniversityLibraryProject
{
	/// <summary>
	/// Логика взаимодействия для CatalogEditWindow.xaml
	/// </summary>
	public partial class CatalogEditWindow : Window
	{
		public CatalogEditWindow(CatalogEditModelView modelView)
		{
			InitializeComponent();
			DataContext = modelView;
		}
	}
}
