using DatabaseManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using ModelViewSystem;
using System.Xml.Linq;

namespace UniversityLibraryProject
{
	/// <summary>
	/// Элемент управления с учетом доступа
	/// </summary>
	public class CustomMenuItem : MenuItem
	{
		public AccessInfo Access { get; set; }
	}

	public class MenuCreator
	{

		public MenuCreator()
		{ }

		/// <summary>
		/// Формирование доступа к меню
		/// </summary>
		/// <param name="item"> Информация о создаваемом пункте меню </param>
		/// <returns></returns>
		private CustomMenuItem ItemStatusUpdate(CustomMenuItem item)
		{
			if (!item.Access.HaveAccess)
				item.Visibility = Visibility.Hidden;

			if (!item.Access.Read)
				item.IsEnabled = false;

			return item;
		}

		/// <summary>
		/// Добавление дочернего пункта к родительскому
		/// </summary>
		/// <param name="menuItem"> Родительский пункт меню </param>
		/// <param name="model"> Информация о создаваемом пункте меню </param>
		/// <returns></returns>
		private CustomMenuItem AddItem(CustomMenuItem menuItem, AccessModel model)
		{
			CustomMenuItem newItem = new CustomMenuItem();
			newItem.Header = model.MenuItem.Name;
			AccessInfo pageAccess = AccessInfo.LoadFromItem(model);
			newItem.Access = pageAccess;
			newItem = ItemStatusUpdate(newItem);

			newItem.Command = (model.MenuItem.DLL == null) ? 
				null :
				new ButtonCommand((object obj) => MenuPageLoader.LoadMenuPage(model.MenuItem.DLL, model.MenuItem.Key, pageAccess));

			if (newItem.Visibility != Visibility.Hidden)
				menuItem.Items.Add(newItem);


			return newItem;
		}

		private bool IsNullMenuItem(CustomMenuItem item)
		{
			bool value = true;
			string header = item.Header as string;

			if (item.Command != null)
				return false;

			foreach (CustomMenuItem menuItem in item.Items)
			{				
				value = value && IsNullMenuItem(menuItem);
			}

			return value;
		}

		/// <summary>
		/// удаляем заглавные меню, которые не имеют дочерние меню
		/// </summary>
		private List<CustomMenuItem> RemoveNullElements(List<CustomMenuItem> items)
		{
			var nullItems = items.Where(i => IsNullMenuItem(i)).ToList();

			for (int i = nullItems.Count - 1; i >= 0; i--)
				items.Remove(nullItems[i]);

			return items;
		}

		/// <summary>
		/// Загрузка итогового контекстого меню пользователя
		/// </summary>
		/// <param name="items"> List модели доступа </param>
		/// <returns></returns>
		public List<CustomMenuItem> GetMenuItems(List<AccessModel> items)
		{
			List<CustomMenuItem> menuItems = new List<CustomMenuItem>(0); // Все пункты меню
			Dictionary<int, CustomMenuItem> menuDict = new Dictionary<int, CustomMenuItem>(); //Словать для доступа к пунктам меню по Id пункта меню из БД

			// Меню на самом верхнем уровне
			var mainItems = items.Where(i => i.MenuItem.ParentId == 0).ToList();

			foreach (var item in mainItems)
			{
				CustomMenuItem menuItem = new CustomMenuItem();
				menuItem.Header = item.MenuItem.Name;
				AccessInfo pageAccess = AccessInfo.LoadFromItem(item);
				menuItem.Access = pageAccess;
				//menuItem.Command = new ButtonCommand((object obj) => MenuPageLoader.LoadMenuPage(item.MenuItem.DLL, item.MenuItem.Key, pageAccess));
				menuItem.Command = null;

				menuItem = ItemStatusUpdate(menuItem);
				menuItems.Add(menuItem);
				menuDict.Add(item.MenuId, menuItem);
			}

			// По циклу присоединяем дочерние пункты меню к родительским
			while (menuItems.Count != items.Count)
			{
				var childs = items.Where(i => menuDict.Keys.Any(id => id == i.MenuItem.ParentId)).ToList();
				childs.RemoveAll(i => menuDict.Keys.Contains(i.MenuId));
				childs = childs.OrderBy(i => i.MenuItem.ParentId).ToList();

				if (childs.Count == 0)
					break;

				foreach (var child in childs)
				{
					var parent = menuDict[child.MenuItem.ParentId];

					var childItem = AddItem(parent, child);
					menuDict.Add(child.MenuId, childItem);
				}
			}

			menuItems = RemoveNullElements(menuItems);
			return menuItems;
		}
	}
}
