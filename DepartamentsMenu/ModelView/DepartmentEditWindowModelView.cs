using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Windows;

namespace DepartamentsMenu
{
	public class DepartmentEditWindowModelView : CatalogEditModelView
	{
		public DepartmentEditWindowModelView() : base()
		{
			Tittle = "Новый отдел";
			Description = "Наименование отдела";
		}

		public DepartmentEditWindowModelView(DepartmentModel departmentModel) : base(departmentModel)
		{
			Tittle = "Редактирование отдела";
			Description = "Наименование отдела";
			Name = departmentModel.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				DepartmentModel departmentModel = new DepartmentModel()
				{
					Name = Name,
				};

				Database.Add(departmentModel);
				SuccessMessage("Успешно добавлено");
				WindowVisibility = Visibility.Hidden;
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

				if (Name == "")
					throw new Exception("Наименование - пусто");

				DepartmentModel departmentModel = new DepartmentModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(departmentModel);
				SuccessMessage("Успешно отредактировано");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}
	}
}
