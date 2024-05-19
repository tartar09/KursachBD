using System;
using System.Windows.Controls;
using ModelViewSystem;
using DatabaseManagers;
using System.Collections.ObjectModel;

namespace DegreesMenu
{
	public class DegreeControlPageModelView : TableEditPageModelView
	{
		public DegreeControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{ }

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new DegreeEditWindowModelView();
				WindowEvents.OpenCatalogEdit(modelView);
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
				var item = Database.GetDegreesList().Find(a => a.Id == SelectedItem.Id);
				var modelView = new DegreeEditWindowModelView(item);
				WindowEvents.OpenCatalogEdit(modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected override void Delete(object obj)
		{
			try
			{
				base.Delete(obj);
				var item = Database.GetDegreesList().Find(a => a.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();
			var accessList = Database.GetDegreesList();
			Items = new ObservableCollection<DataModel>(accessList);
		}
	}
}
