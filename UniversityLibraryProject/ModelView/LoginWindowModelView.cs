using System;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using ModelViewSystem;
using DatabaseManagers;

namespace UniversityLibraryProject
{
	/// <summary>
	/// Логика для окна авторизации пользователя
	/// </summary>
	public class LoginWindowModelView : BaseUIModelView
	{
		private string _login = "";
		private string _password = "";

		public string Login
		{
			get { return _login; }
			set
			{
				_login = value;
				OnPropertyChanged(nameof(Login));
			}
		}
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}

		public ButtonCommand LogCommand { get; private set; }
		public ButtonCommand RegCommand { get; private set; }

		public LoginWindowModelView()
		{
			LogCommand = new ButtonCommand(NewUserLogin);
			RegCommand = new ButtonCommand(UserReg);
		}

		private void NewUserLogin(object obj)
		{
			try
			{
				int id = DatabaseManager.GetInstance().Authorization(Login, Password);

				UserModel user = new UserModel()
				{
					Id = id,
					Login = Login,
					Password = Password
				};

				Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.Login), null);
				WindowEvents.NewUserLogin(user);

				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		private void UserReg(object obj)
		{
			UserModel user = new UserModel()
			{
				Login = Login,
				Password = Password
			};

			try
			{
				DatabaseManager.GetInstance().Add(user);
				SuccessMessage("Пользователь успешно зарегистрирован");
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}
	}
}
