using System;
using System.Windows;
using DatabaseManagers;
using ModelViewSystem;

namespace ReaderCategoryMenu
{
	public class ReaderCategoryEditWindowModelView : CatalogEditModelView
	{
		public ReaderCategoryEditWindowModelView() : base()
		{
			Tittle = "Новая категория читателей";
			Description = "Наименование категории читателей";
		}

		public ReaderCategoryEditWindowModelView(ReaderCategoryModel readerCategory) : base(readerCategory)
		{
			Tittle = "Редактирование категории читателей";
			Description = "Наименование категории читателей";
			Name = readerCategory.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				ReaderCategoryModel readerCategory = new ReaderCategoryModel()
				{
					Name = Name,
				};

				Database.Add(readerCategory);
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

				ReaderCategoryModel readerCategory = new ReaderCategoryModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(readerCategory);
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
