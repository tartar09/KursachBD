using System.Windows;
using System.Windows.Controls;

namespace OrdersMenu
{
	/// <summary>
	/// Логика взаимодействия для OrderEditWindow.xaml
	/// </summary>
	public partial class OrderEditWindow : Window
	{
		private OrderEditWindowModelView _modelView;

		public OrderEditWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_modelView = DataContext as OrderEditWindowModelView;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			var author = checkBox.DataContext as PublicBookCatalogModel;
			if (author != null && _modelView != null)
			{
				_modelView.SelectedCatalogs.Add(author);
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			var author = checkBox.DataContext as PublicBookCatalogModel;
			if (author != null && _modelView != null)
			{
				_modelView.SelectedCatalogs.Remove(author);
			}
		}
	}
}
