using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DatabaseManagers;
using ModelViewSystem;

namespace AccessControlMenu
{
	public class AccessControlPageModelView : TableEditPageModelView
	{
		/// <summary>
		/// ModelView страницы редактирования таблицы доступа к меню
		/// </summary>
		/// <param name="buttons"> [view, add, edit, delete, logOut] </param>
		/// <param name="dataGrid"> Основная таблица для вывода данных </param>
		/// <param name="access"> Параметры допуска </param>
		public AccessControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{ }

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new AccessEditWindowModelView();
				WindowEvents.OpenWindow(typeof(AccessEditWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void Edit(object obj)
		{
			try
			{ 
				base.Edit(obj);
				var item = Database.GetMenuAccessList().Find(a => a.Id == SelectedItem.Id);
				var modelView = new AccessEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(AccessEditWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void Delete(object obj) 
		{
			try
			{
				base.Delete(obj);
				var item = Database.GetMenuAccessList().Find(a => a.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch(Exception ex) 
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();
			var accessList = Database.GetMenuAccessList();
			Items = new ObservableCollection<DataModel>(accessList);
		}

	}
}
