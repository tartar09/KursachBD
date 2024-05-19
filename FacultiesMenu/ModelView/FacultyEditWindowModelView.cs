using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Windows;

namespace FacultiesMenu
{
	public class FacultyEditWindowModelView : CatalogEditModelView
	{
		public FacultyEditWindowModelView() : base()
		{
			Tittle = "Новый факультет";
			Description = "Название факультета";
		}

		public FacultyEditWindowModelView(FacultyModel facultyModel) : base(facultyModel)
		{
			Tittle = "Редактирование факультета";
			Description = "Название факультета";
			Name = facultyModel.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				FacultyModel facultyModel = new FacultyModel()
				{ 
					Name = Name,
				};

				Database.Add(facultyModel);
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

				FacultyModel facultyModel = new FacultyModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(facultyModel);
				SuccessMessage("Редактирование - успешно");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

	}
}
