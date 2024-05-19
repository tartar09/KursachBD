using DatabaseManagers;
using ModelViewSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using System.Linq;
using System.ComponentModel;

namespace BooksCatalogsMenu
{
	public class PublicAuthorModel : ViewModelBase
	{
		private bool isSelected;

		public int Id { get; set; }
		public string Surname { get; set; }
		public string Name { get; set; }
		public string Patronymic { get; set; }

		public bool IsSelected
		{
			get { return isSelected; }
			set
			{
				if (isSelected != value)
				{
					isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
				}
			}
		}
	}

	public class BooksCatalogEditWindowModelView : DataModelEditWindowModelView
	{
		private string _storageNumber;
		private string _standNumber;
		private string _shelfNumber;
		private string _name;
		private DateTime _date;
		private string _publishHouseName;

		private string _count;

		private ObservableCollection<BookCategoryModel> _categoriesList;
		private BookCategoryModel _selectedCategory;

		private ObservableCollection<PublicAuthorModel> _authorsList;
		private ObservableCollection<PublicAuthorModel> _selectedAuthors = new ObservableCollection<PublicAuthorModel>();


		private ObservableCollection<BookTypeModel> _typesList;
		private BookTypeModel _selectedType;

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
		public DateTime Date
		{
			get { return _date; }
			set
			{
				_date = value;
				OnPropertyChanged(nameof(Date));
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
		public string Count
		{
			get { return _count; }
			set
			{
				_count = value;
				OnPropertyChanged(nameof(Count));
			}
		}

		public ObservableCollection<BookCategoryModel> CategoriesList
		{
			get { return _categoriesList; }
			set
			{
				_categoriesList = value;
				OnPropertyChanged(nameof(CategoriesList));
			}
		}
		public BookCategoryModel SelectedCategory
		{
			get { return _selectedCategory; }
			set
			{
				_selectedCategory = value;
				OnPropertyChanged(nameof(SelectedCategory));
			}
		}

		public ObservableCollection<PublicAuthorModel> AuthorsList
		{
			get { return _authorsList; }
			set
			{
				_authorsList = value;
				OnPropertyChanged(nameof(AuthorsList));
			}
		}
		public ObservableCollection<PublicAuthorModel> SelectedAuthors
		{
			get { return _selectedAuthors; }
			set
			{
				_selectedAuthors = value;
				OnPropertyChanged(nameof(SelectedAuthors));
			}
		}

		public BookTypeModel SelectedType
		{
			get { return _selectedType; }
			set
			{
				_selectedType = value;
				OnPropertyChanged(nameof(SelectedType));
			}
		}
		public ObservableCollection<BookTypeModel> TypesList
		{
			get { return _typesList; }
			set
			{
				_typesList = value;
				OnPropertyChanged(nameof(TypesList));
			}
		}

		public BooksCatalogEditWindowModelView() : base()
		{
			Tittle = "Новая книга";
			StorageNumber = "";
			StandNumber = "";
			ShelfNumber = "";
			Name = "";
			Date = DateTime.Now;
			PublishHouseName = "";
			CategoriesList = new ObservableCollection<BookCategoryModel>(Database.GetBookCategoriesList());
			LoadAuthorsTable();
			SelectedCategory = null;
			Count = "";
			TypesList = new ObservableCollection<BookTypeModel>(Database.GetBookTypesList());
			SelectedType = null;
		}

		public BooksCatalogEditWindowModelView(BookCatalogModel catalogModel) : base(catalogModel)
		{
			Tittle = "Редактирование книги";

			var bookModel = Database.GetBooksList().First(b => b.CatalogId == catalogModel.Id);

			StorageNumber = bookModel.StorageNumber.ToString();
			StandNumber = bookModel.StandNumber.ToString();
			ShelfNumber = bookModel.ShelfNumber.ToString();
			Name = bookModel.Name;
			Date = DateTime.Parse(bookModel.Date);
			PublishHouseName = bookModel.PublishHouseName;

			CategoriesList = new ObservableCollection<BookCategoryModel>(Database.GetBookCategoriesList());
			var authorsList = Database.GetAuthorsBooksList().Where(ab => ab.BookCatalogId == catalogModel.Id).Select(ab => ab.Author).ToList();

			List<PublicAuthorModel> authorModels = new List<PublicAuthorModel>(0);

			foreach (var item in authorsList)
			{
				authorModels.Add(new PublicAuthorModel()
				{
					Id = item.Id,
					Surname = item.Surname,
					Name = item.Name,
					Patronymic = item.Patronymic,
					IsSelected = true,
				});
			}
			SelectedAuthors = new ObservableCollection<PublicAuthorModel>(authorModels);

			Count = catalogModel.Count.ToString();

			TypesList = new ObservableCollection<BookTypeModel>(Database.GetBookTypesList());
			SelectedType = TypesList.First(t => t.Id == catalogModel.Id);

			LoadAuthorsTable();
			SelectedCategory = Database.GetBookCategoriesList().Find(c => c.Id == bookModel.CategoryId);
		}

		private void AuthorsListUpdate(BookCatalogModel bookCatalog)
		{
			List<AuthorModel> authors = Database.GetAuthorsList().Where(a => SelectedAuthors.Any(m => m.Id == a.Id)).ToList();

			if(bookCatalog.AuthorsBooks != null)
				Database.Delete(bookCatalog.AuthorsBooks.ToArray());

			List<AuthorsBooks> contacts = new List<AuthorsBooks>(0);
			foreach (AuthorModel author in authors)
				contacts.Add(new AuthorsBooks { AuthorId = author.Id, BookCatalogId = bookCatalog.Id });
			Database.Add(contacts.ToArray());
		}

		protected override void Add(object obj)
		{
			//try
			//{
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

				if(standNumber <= 0)
					throw new Exception("Номер стенда - нерректное значение");

				if(shelfNumber <= 0)
					throw new Exception("Номер полки - нерректное значение");

				if (!int.TryParse(Count, out int count))
					throw new Exception("Количество - нерректное значение");

				if (SelectedAuthors.Count == 0)
					throw new Exception("Не выбрано ни одного автора");

				if (SelectedType == null)
					throw new Exception("Тип доступа - не выбран");

				BookCatalogModel bookCatalogModel = new BookCatalogModel()
				{
					Count = count,
					AvailableCount = count,
					BookType = SelectedType,
					BookTypeId = SelectedType.Id,
				};

				BookModel bookModel = new BookModel()
				{
					StorageNumber = storageNumber,
					StandNumber = standNumber,
					ShelfNumber = shelfNumber,
					Name = Name,
					Date = Date.ToShortDateString(),
					PublishHouseName = PublishHouseName,
					CategoryId = SelectedCategory.Id,
					Category = SelectedCategory,
				};

				Database.Add(bookCatalogModel, bookModel);
				AuthorsListUpdate(bookCatalogModel);
				SuccessMessage("Успешно добавлено");
				WindowVisibility = Visibility.Hidden;
			//}
			//catch (Exception ex)
			//{
			//	ErrorMessage(ex.Message);
			//}
		}

		protected override void Edit(object obj)
		{
			//try
			//{
				base.Edit(obj);

				if (Name == "")
					throw new Exception("Название книги не может быть пустым");

				if (!int.TryParse(StorageNumber, out int storageNumber))
					throw new Exception("Номер зала - нерректное значение");

				if (!int.TryParse(StandNumber, out int standNumber))
					throw new Exception("Номер стенда - нерректное значение");

				if (!int.TryParse(ShelfNumber, out int shelfNumber))
					throw new Exception("Номер полки - нерректное значение");

				if (SelectedAuthors.Count == 0)
					throw new Exception("Не выбрано ни одного автора");

				if (SelectedType == null)
					throw new Exception("Тип доступа - не выбран");

				BookCatalogModel bookCatalogModel = Database.GetBooksCatalogsList().Find(c => c.Id == DataModel.Id);

				BookModel bookModel = new BookModel()
				{
					StorageNumber = storageNumber,
					StandNumber = standNumber,
					ShelfNumber = shelfNumber,
					Name = Name,
					Date = Date.ToShortDateString(),
					PublishHouseName = PublishHouseName,
					CategoryId = SelectedCategory.Id,
					Category = SelectedCategory,
				};

				Database.Edit(bookCatalogModel, bookModel);
				AuthorsListUpdate(bookCatalogModel);
				SuccessMessage("Успешно отредактировано");
				WindowVisibility = Visibility.Hidden;
			//}
			//catch (Exception ex)
			//{
			//	ErrorMessage(ex.Message);
			//}
		}

		protected void LoadAuthorsTable()
		{
			var authors = Database.GetAuthorsList();
			List<PublicAuthorModel> authorsModels = new List<PublicAuthorModel>(0);

			foreach (var author in authors)
			{
				authorsModels.Add(new PublicAuthorModel()
				{
					Id = author.Id,
					Name = $"{author.Surname} {author.Name[0]}.{author.Patronymic[0]}",
					IsSelected = SelectedAuthors.Any(m => m.Id == author.Id)
				});
			}

			AuthorsList = new ObservableCollection<PublicAuthorModel>(authorsModels);
		}
	}
}
