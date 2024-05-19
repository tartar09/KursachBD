using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BooksTypes
{
	public class PublicBookType : BookTypeModel
	{
		public bool IsOnlyRead { get; set; }
	}

	public class BookTypeControlPageModelView : TableEditPageModelView
	{
		public BookTypeControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new BookTypeEditWindowModelView();
				WindowEvents.OpenWindow(typeof(BookTypeEditWindow), modelView);
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
				var item = Database.GetBookTypesList().Find(t => t.Id == SelectedItem.Id);
				var modelView = new BookTypeEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(BookTypeEditWindow), modelView);
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
				var item = Database.GetBookTypesList().Find(t => t.Id == SelectedItem.Id);
				Database.Delete(item);
				SuccessMessage("Успешно удалено");
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();

			List<PublicBookType> list = new List<PublicBookType>(0);

			foreach (var item in Database.GetBookTypesList())
			{
				list.Add(new PublicBookType()
				{
					Id = item.Id,
					Name = item.Name,
					AccessTime = item.AccessTime,
					OnlyRead = item.OnlyRead,
					IsOnlyRead = item.OnlyRead > 0,
				});
			}
			Items = new ObservableCollection<DataModel>(list);
		}
	}
}
