using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Reflection;
using System.Windows;

namespace UniversityLibraryProject
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{
		#region WINDOWS

		private void OpenWindow(Type windowType, ViewModelBase viewModel)
		{
			Assembly assembly = windowType.Assembly;
			Type type = assembly.GetType(windowType.FullName);

			Window window = (Window)Activator.CreateInstance(type);
			window.DataContext = viewModel;
			window.ShowDialog();
		}

		private void OpenCatalogEditWindow(CatalogEditModelView modelView)
		{
			try
			{
				CatalogEditWindow editWindow = new CatalogEditWindow(modelView);
				editWindow.ShowDialog();
			}
			catch
			{}
		}
		 
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			WindowEvents.OnLogin += (UserModel user) => (new MainWindow(user)).Show();
			WindowEvents.OnLogOut += () => (new LoginWindow()).Show();
			WindowEvents.OnOpenWindow += OpenWindow;
			WindowEvents.OnOpenCatalogEdit += OpenCatalogEditWindow;
		}

		#endregion
	}
}
