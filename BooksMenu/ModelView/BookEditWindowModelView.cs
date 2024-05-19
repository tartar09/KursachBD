using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BooksMenu
{
	public class BookEditWindowModelView : DataModelEditWindowModelView
	{
		private string _storageNumber;
		private string _standNumber;
		private string _shelfNumber;
		private string _name;
		private string _publishHouseName;

		public string StorageNumber
		{
			get { return _storageNumber; }
			set
			{
				_storageNumber = value;
				OnPropertyChanged(nameof(StorageNumber));
			}
		}
		public string StandNumber
		{
			get { return _standNumber; }
			set
			{
				_standNumber = value;
				OnPropertyChanged(nameof(StandNumber));
			}
		}
		public string ShelfNumber
		{
			get { return _shelfNumber; }
			set
			{
				_shelfNumber = value;
				OnPropertyChanged(nameof(ShelfNumber));
			}
		}
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string PublishHouseName
		{
			get { return _publishHouseName; }
			set
			{
				_publishHouseName = value;
				OnPropertyChanged(nameof(PublishHouseName));
			}
		}

		public BookEditWindowModelView() : base()
		{
			Tittle = "Новая книга";
			StorageNumber = "";
			StandNumber = "";
			ShelfNumber = "";
			Name = "";
			PublishHouseName = "";
			LoadAuthorsTable();
		}

		public BookEditWindowModelView(BookModel bookModel) : base(bookModel)
		{
			Tittle = "Редактирование книги";
			StorageNumber = bookModel.StorageNumber.ToString();
			StandNumber = bookModel.StandNumber.ToString();
			ShelfNumber = bookModel.ShelfNumber.ToString();
			Name = bookModel.Name;
			PublishHouseName = bookModel.PublishHouseName;
			LoadAuthorsTable();
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Название книги не может быть пустым");

				if (!int.TryParse(StorageNumber, out int storageNumber))
					throw new Exception("Номер зала - нерректное значение");

				if (!int.TryParse(StandNumber, out int standNumber))
					throw new Exception("Номер стенда - нерректное значение");

				if (!int.TryParse(ShelfNumber, out int shelfNumber))
					throw new Exception("Номер полки - нерректное значение");

				if(storageNumber <= 0)
					throw new Exception("Номер зала - нерректное значение");

				if (standNumber <= 0)
					throw new Exception("Номер стенда - нерректное значение");

				if (shelfNumber <= 0)
					throw new Exception("Номер полки - нерректное значение");

				var model = ((BookModel)DataModel);

				BookModel bookModel = new BookModel()
				{
					StorageNumber = storageNumber,
					StandNumber = standNumber,
					ShelfNumber = shelfNumber,
					Name = Name,
					Date = model.Date,
					PublishHouseName = PublishHouseName,

					Category = model.Category,
					CategoryId = model.CategoryId,
					Catalog = model.Catalog,
					CatalogId = model.CatalogId,
				};

				var category = Database.GetBooksCatalogsList().Find(c => c.Books.ToList()[0] == bookModel);
				bookModel.CatalogId = category.Id;

				Database.Add(bookModel);
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
					throw new Exception("Название книги не может быть пустым");

				if (!int.TryParse(StorageNumber, out int storageNumber))
					throw new Exception("Номер зала - нерректное значение");

				if (!int.TryParse(StandNumber, out int standNumber))
					throw new Exception("Номер стенда - нерректное значение");

				if (!int.TryParse(ShelfNumber, out int shelfNumber))
					throw new Exception("Номер полки - нерректное значение");

				var model = ((BookModel)DataModel);

				BookModel bookModel = new BookModel()
				{
					Id = DataModel.Id,
					StorageNumber = storageNumber,
					StandNumber = standNumber,
					ShelfNumber = shelfNumber,
					Name = Name,
					Date = model.Date,
					PublishHouseName = PublishHouseName,

					Category = model.Category,
					CategoryId = model.CategoryId,
					Catalog = model.Catalog,
					CatalogId = model.CatalogId,
				};

				Database.Edit(bookModel);
				SuccessMessage("Успешно отредактировано");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected void LoadAuthorsTable()
		{
			var authors = Database.GetAuthorsList();
			List<AuthorModel> authorsModels = new List<AuthorModel>(0);

			foreach (var author in authors)
			{
				authorsModels.Add(new AuthorModel()
				{
					Id = author.Id,
					Name = $"{author.Surname} {author.Name[0]}.{author.Patronymic[0]}",
				});
			}
		}

	}
}
