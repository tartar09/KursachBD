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

namespace BooksMenu
{
	/// <summary>
	/// Логика взаимодействия для GetBookWindow.xaml
	/// </summary>
	public partial class GetBookWindow : Window
	{
		private GetBookWindowModelView _modelView;

		public GetBookWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			_modelView = DataContext as GetBookWindowModelView;
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			var author = checkBox.DataContext as PublicBookModel;
			if (author != null && _modelView != null)
			{
				_modelView.SelectedBooks.Add(author);
				_modelView.Update();
			}
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			var author = checkBox.DataContext as PublicBookModel;
			if (author != null && _modelView != null)
			{
				_modelView.SelectedBooks.Remove(author);
				_modelView.Update();
			}
		}

	}
}
