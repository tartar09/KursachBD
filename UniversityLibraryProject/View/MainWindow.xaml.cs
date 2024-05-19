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
using DatabaseManagers;
using ModelViewSystem;

namespace UniversityLibraryProject
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainWindowModelView _viewModel;

		public MainWindow(UserModel user)
		{
			InitializeComponent();
			_viewModel = new MainWindowModelView(user);
			DataContext = _viewModel;
			LoadMenu();
			WindowEvents.OnLoadMenuPage += LoadMenuPage;
			WindowEvents.OnLogOut += () => this.Hide();
			WindowEvents.OnLogOut += UnsubscribeFromEvents;
		}

		/// <summary>
		/// Загрузка контекстного меню
		/// </summary>
		private void LoadMenu()
		{
			List<CustomMenuItem> list = _viewModel.LoadMenu();

			foreach (MenuItem item in list)
				MainMenu.Items.Add(item);
		}

		/// <summary>
		/// Загрузка страницы
		/// </summary>
		private void LoadMenuPage(Page page)
		{
			ContentControl contentControl = new ContentControl();
			contentControl.Content = page.Content;
			contentControl.HorizontalAlignment = HorizontalAlignment.Stretch;
			contentControl.VerticalAlignment = VerticalAlignment.Stretch;

			StackPanel stackPanel = new StackPanel();
			DockPanel.SetDock(contentControl, Dock.Top);

			if (MainMenu.Parent is StackPanel panel)
				panel.Children.Remove(MainMenu);

			MainGrid.Children.Remove(MainMenu);

			stackPanel.Children.Add(MainMenu);
			stackPanel.Children.Add(contentControl);

			this.Content = stackPanel;
		}

		/// <summary>
		/// Обнуление делегатов
		/// </summary>
		private void UnsubscribeFromEvents()
		{
			WindowEvents.OnLogOut -= () => this.Hide();
			WindowEvents.OnLoadMenuPage -= LoadMenuPage;
		}

		private void Window_Closed(object sender, System.EventArgs e)
		{
			UnsubscribeFromEvents();
			WindowEvents.UserLogOut();
		}
	}
}
