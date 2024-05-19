using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DegreesMenu
{
	public class DegreeEditWindowModelView : CatalogEditModelView
	{
		public DegreeEditWindowModelView() 
		{
			Tittle = "Степень. Добавление.";
			Description = "Наименование";
		}

		public DegreeEditWindowModelView(DegreeModel degreeModel) : base(degreeModel) 
		{
			Tittle = "Степень. Редактирование.";
			Description = "Наименование";
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				DegreeModel degreeModel = new DegreeModel()
				{
					Name = Name,
				};

				Database.Add(degreeModel);
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

				DegreeModel degreeModel = new DegreeModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(degreeModel);
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
