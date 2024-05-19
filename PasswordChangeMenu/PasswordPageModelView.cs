using System.Linq;
using ModelViewSystem;
using DatabaseManagers;
using System;

namespace PasswordChangeMenu
{
	/// <summary>
	/// ModelView Окна редактирования пароля пользователя
	/// </summary>
	public class PasswordPageModelView : BaseUIModelView
	{
		private string _password;
		private string _login;

		public string Password
		{
			get { return _password; }
			set {
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}
		public string Login
		{
			get { return _login; }
			set
			{ 
				_login = value;
				OnPropertyChanged(nameof(Login));
			}
		}

		public ButtonCommand ButtonCommand { get; private set; }

		public PasswordPageModelView() : base()
		{
			Login = CurrentUser.Login;
			Password = CurrentUser.Password;
			ButtonCommand = new ButtonCommand(ChangePassword);
		}

		private void ChangePassword(object obj)
		{
			try
			{
				if (Password.Length < 4)
					throw new Exception("Пароль слишком короткий");

				var database = DatabaseManager.GetInstance();
				var UserToEdit = database.GetUsersList().First(u => u.Login == Login);

				UserModel user = new UserModel()
				{
					Id = UserToEdit.Id,
					Login = Login,
					Password = Password,
				};

				database.Edit(user);
				SuccessMessage("Пароль успешно изменен");
			}
			catch(Exception ex) 
			{
				ErrorMessage(ex.Message);			
			}
		}
	}
}
