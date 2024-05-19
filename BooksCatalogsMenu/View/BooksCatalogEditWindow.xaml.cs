using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BooksCatalogsMenu
{
	/// <summary>
	/// Логика взаимодействия для BooksCatalogEditWindow.xaml
	/// </summary>
	public partial class BooksCatalogEditWindow : Window
	{
		private BooksCatalogEditWindowModelView _modelView;

		public BooksCatalogEditWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Выделение авторов для книги
		/// </summary>
		private void AuthorsTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var modelView = DataContext as BooksCatalogEditWindowModelView;
			var dataGrid = sender as DataGrid;
			if (dataGrid != null)
			{
				modelView.SelectedAuthors = new ObservableCollection<PublicAuthorModel>(dataGrid.Items.Cast<PublicAuthorModel>().Where(a => a.IsSelected));
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			var modelView = DataContext as BooksCatalogEditWindowModelView;
			_modelView = modelView;
			AuthorsTable.Focus();

			// Ждем, пока DataGrid полностью загрузится
			AuthorsTable.Dispatcher.BeginInvoke(new Action(() =>
			{
				// Выделяем элементы из modelView.SelectedAuthors
				foreach (var item in modelView.SelectedAuthors)
				{
					if (!AuthorsTable.SelectedItems.Contains(item))
					{
						AuthorsTable.SelectedItems.Add(item);
					}
				}

				CountTextBox.IsEnabled = modelView.DataModel == null;
			}), DispatcherPriority.ContextIdle);
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			var author = checkBox.DataContext as PublicAuthorModel;
			if (author != null && _modelView != null)
			{
				 _modelView.SelectedAuthors.Remove(author);
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;
			var author = checkBox.DataContext as PublicAuthorModel;
			if (author != null && _modelView != null)
			{
				_modelView.SelectedAuthors.Add(author);
			}
		}
	}
}
