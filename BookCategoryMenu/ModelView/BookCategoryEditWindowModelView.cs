using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Windows;

namespace BookCategoryMenu
{
	public class BookCategoryEditWindowModelView : CatalogEditModelView
	{
		public BookCategoryEditWindowModelView() : base()
		{
			Tittle = "Новая категория книг";
			Description = "Наименование категории книг";
		}

		public BookCategoryEditWindowModelView(BookCategoryModel bookCategoryModel) : base(bookCategoryModel)
		{
			Tittle = "Редактирование категории книг";
			Description = "Наименование категории книг";
			Name = bookCategoryModel.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				BookCategoryModel bookCategoryModel = new BookCategoryModel()
				{
					Name = Name,
				};

				Database.Add(bookCategoryModel);
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

				BookCategoryModel bookCategoryModel = new BookCategoryModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(bookCategoryModel);
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
