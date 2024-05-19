using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows;
using ModelViewSystem;
using DatabaseManagers;

namespace UniversityLibraryProject
{
	/// <summary>
	/// Класс, предоставляющий методы для динамической загрузки страниц из сборок.
	/// </summary>
	public class MenuPageLoader
	{
		/// <summary>
		/// Загрузка страницы для главного меню
		/// </summary>
		/// <param name="dllName">Название dll файла</param>
		/// <param name="pageName">Название страницы</param>
		/// <param name="itemAccess">Информация о доступе (Чтение, добавление, редактирование, запись)</param>
		public static void LoadMenuPage(string dllName, string pageName, AccessInfo itemAccess)
		{
			try
			{
				Assembly assembly;
				try
				{
					assembly = Assembly.LoadFrom(dllName + ".dll");
				}
				catch
				{
					throw new Exception($"Файл {dllName} отсутствует или поврежден");
				}

				string fullTypeName = dllName + "." + pageName;
				Type type = assembly.GetType(fullTypeName);

				if (type != null && typeof(Page).IsAssignableFrom(type))
				{
					Page page;
					if (type.GetConstructor(new[] { typeof(AccessInfo) }) != null)
					{
						// Если у страницы есть конструктор с параметром, используем его
						page = (Page)Activator.CreateInstance(type, itemAccess);
					}
					else
					{
						// Иначе, передаем itemAccess в конструктор страницы без параметров
						page = (Page)Activator.CreateInstance(type);
					}
					WindowEvents.LoadMenuPage(page);
				}
				else
				{
					throw new Exception($"{pageName} - не может быть загружен как Page");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

	}
}
