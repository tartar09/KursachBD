using DatabaseManagers;
using System;
using System.Windows.Controls;

namespace ModelViewSystem
{
	/// <summary>
	/// Класс WindowEvents представляет собой статический класс для определения событий, связанных с окнами приложения.
	/// </summary>
	public class WindowEvents
	{
		/// <summary>
		/// Событие OnLoadMenuPage, которое вызывается при загрузке страницы
		/// </summary>
		public static Action<Page> OnLoadMenuPage { get; set; }

		/// <summary>
		/// Событие OnLogin, которое вызывается при успешном входе пользователя в систему
		/// </summary>
		public static Action<UserModel> OnLogin { get; set; }

		/// <summary>
		/// Событие OnLogOut, которое вызывается при выходе пользователя из системы
		/// </summary>
		public static Action OnLogOut { get; set; }

		/// <summary>
		/// Событие OnOpenWindow, которое вызывается при открытии нового окна и загрузки для него ModelView
		/// </summary>
		public static Action<Type, ViewModelBase> OnOpenWindow { get; set; }

		/// <summary>
		/// Событие OnOpenCatalogEdit, которое вызывается при открытии окна редактирования каталога
		/// </summary>
		public static Action<CatalogEditModelView> OnOpenCatalogEdit { get; set; }

		// Методы для вызова соответствующих событий
		public static void LoadMenuPage(Page page) => OnLoadMenuPage?.Invoke(page);
		public static void NewUserLogin(UserModel userModel) => OnLogin?.Invoke(userModel);
		public static void UserLogOut() => OnLogOut?.Invoke();
		public static void OpenWindow(Type windowType, ViewModelBase viewModel) => OnOpenWindow?.Invoke(windowType, viewModel);
		public static void OpenCatalogEdit(CatalogEditModelView modelView) => OnOpenCatalogEdit?.Invoke(modelView);

	}

}
