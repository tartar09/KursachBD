using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FacultiesMenu
{
	public class FacultyControlPageModelView : TableEditPageModelView
	{
		public FacultyControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{ }

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new FacultyEditWindowModelView();
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
				var item = Database.GetFacultiesList().Find(f => f.Id == SelectedItem.Id);
				var modelView = new FacultyEditWindowModelView(item);
				WindowEvents.OpenCatalogEdit(modelView);
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
				var item = Database.GetFacultiesList().Find(f => f.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();
			Items = new ObservableCollection<DataModel>(Database.GetFacultiesList());
		}

	}
}
