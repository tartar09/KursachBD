﻿using System;
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
	/// Логика взаимодействия для LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		private LoginWindowModelView _modelView;

		public LoginWindow()
		{
			InitializeComponent();
			_modelView = new LoginWindowModelView();
			DataContext = _modelView;
		}

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			_modelView.Password = PasswordBox.Password;
		}

		private void Window_Closed(object sender, System.EventArgs e) => Environment.Exit(0);
	}
}
