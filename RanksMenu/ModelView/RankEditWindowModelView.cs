using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Windows;

namespace RanksMenu
{
	public class RankEditWindowModelView : CatalogEditModelView
	{
		public RankEditWindowModelView() : base()
		{
			Tittle = "Новое звание";
			Description = "Наименование звания";
		}

		public RankEditWindowModelView(RankModel rankModel) : base(rankModel)
		{
			Tittle = "Редактирование звания";
			Description = "Наименование звания";
			Name = rankModel.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				RankModel rankModel = new RankModel()
				{
					Name = Name,
				};

				Database.Add(rankModel);
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

				RankModel rankModel = new RankModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(rankModel);
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
