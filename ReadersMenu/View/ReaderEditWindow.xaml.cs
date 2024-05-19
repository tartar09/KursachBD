using DatabaseManagers;
using System.Windows;
using System.Windows.Controls;

namespace ReadersMenu
{
	/// <summary>
	/// Логика взаимодействия для ReaderEditWindow.xaml
	/// </summary>
	public partial class ReaderEditWindow : Window
	{

		public ReaderEditWindow()
		{
			InitializeComponent();
		}

		private void LoadDataContext()
		{ 
			var viewModel = DataContext as ReaderEditWindowModelView;
			viewModel.OnSelectionsChange += SelectCategory;

			if(viewModel.SelectedCategory != null)
				SelectCategory(viewModel.SelectedCategory);
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			LoadDataContext();
		}

		private void ComboBoxEnable(ComboBox comboBox, bool value)
		{
			comboBox.SelectedItem = comboBox.IsEnabled && !value ? null : comboBox.SelectedItem;
			comboBox.IsEnabled = value;
		}

		private void SelectCategory(ReaderCategoryModel readerCategory)
		{
			ComboBoxEnable(FacultyComboBox, readerCategory.Id == 1 || readerCategory.Id == 2 || readerCategory.Id == 4);
			ComboBoxEnable(GroupComboBox, readerCategory.Id == 1 || readerCategory.Id == 4);
			ComboBoxEnable(JobComboBox, readerCategory.Id == 2 || readerCategory.Id == 3);
			ComboBoxEnable(DegreeComboBox, readerCategory.Id == 2);
			ComboBoxEnable(RankComboBox, readerCategory.Id == 2);
			ComboBoxEnable(DepartmentComboBox, readerCategory.Id == 3);
		}

    }
}
