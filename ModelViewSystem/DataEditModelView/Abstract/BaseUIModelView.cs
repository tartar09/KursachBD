using DatabaseManagers;
using System.Windows;

namespace ModelViewSystem
{
	/// <summary>
	/// Базовый ModelView для окно/страниц
	/// </summary>
	public abstract class BaseUIModelView : ViewModelBase
	{
		private Visibility _windowVisibility = Visibility.Visible;

		/// <summary>
		/// Видимость
		/// </summary>
		public Visibility WindowVisibility
		{
			get { return _windowVisibility; }
			set
			{
				_windowVisibility = value;
				OnPropertyChanged(nameof(WindowVisibility));
			}
		}

		/// <summary>
		/// Заголовок для окна/страницы
		/// </summary>
		public string Tittle { get; set; }

		/// <summary>
		/// Текущий пользователь сессии
		/// </summary>
		protected static UserModel CurrentUser { get; set; }

		protected void ErrorMessage(string message) => MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		protected void SuccessMessage(string message) => MessageBox.Show(message, "", MessageBoxButton.OK, MessageBoxImage.Information);
	}
}
