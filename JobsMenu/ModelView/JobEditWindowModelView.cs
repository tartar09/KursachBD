using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DatabaseManagers;
using ModelViewSystem;
using System.Xml.Linq;

namespace JobsMenu
{
	public class JobEditWindowModelView : CatalogEditModelView
	{
		public JobEditWindowModelView() : base()
		{
			Tittle = "Новая должность";
			Description = "Наименование должности";
		}

		public JobEditWindowModelView(JobModel jobModel) : base(jobModel)
		{
			Tittle = "Редактирование должности";
			Description = "Наименование должности";
			Name = jobModel.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				JobModel jobModel = new JobModel()
				{
					Name = Name,
				};

				Database.Add(jobModel);
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

				JobModel jobModel = new JobModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(jobModel);
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
