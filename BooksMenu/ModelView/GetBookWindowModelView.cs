using ModelViewSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DatabaseManagers;
using System;

namespace BooksMenu
{
	public class PublicBookModel : ViewModelBase
	{
		private bool _isSelected;

		public int Id { get; set; }
		public string Name { get; set; }
		public string Date { get; set; }
		public string Category { get; set; }
		public string PublishHouseName { get; set; }

		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged(nameof(IsSelected));
				}
			}
		}
	}

	public class GetBookWindowModelView : BaseUIModelView
	{
		private string _bookName;
		private string _booksCount;

		private ObservableCollection<PublicBookModel> _booksList = new ObservableCollection<PublicBookModel>();
		private ObservableCollection<PublicBookModel> _selectedBooks = new ObservableCollection<PublicBookModel>();

		private DatabaseManager Database => DatabaseManager.GetInstance();

		public ObservableCollection<PublicBookModel> BooksList
		{ 
			get { return _booksList; }
			set
			{ 
				_booksList = value;
				OnPropertyChanged(nameof(BooksList));
			}
		}
		public ObservableCollection<PublicBookModel> SelectedBooks
		{
			get { return _selectedBooks; }
			set
			{ 
				_selectedBooks = value; 
				OnPropertyChanged(nameof(SelectedBooks));
			}
		}

		public string BookName
		{
			get { return _bookName; }
			set
			{ 
				_bookName = value;
				OnPropertyChanged(nameof(BookName));
				LoadDataTable();
			}
		}
		public string BooksCount
		{ 
			get { return _booksCount; }
			set
			{ 
				_booksCount = value;
				OnPropertyChanged(nameof(BooksCount));
			}
		}

		public ButtonCommand GetCommand { get; set; }

		public GetBookWindowModelView() : base()
		{
			var booksList = Database.GetBooksList().Where(b => Database.GetBooksReadersList().Any(br => br.BookId == b.Id));
			List<PublicBookModel> books = new List<PublicBookModel>(0);	

			foreach (var book in booksList)
			{
				books.Add(new PublicBookModel()
				{ 
					Id = book.Id,
					Name = book.Name,
					Date = book.Date,
					Category = book.Category.Name,
					PublishHouseName = book.PublishHouseName,
				});
			}

			BooksList = new ObservableCollection<PublicBookModel>(books);
			GetCommand = new ButtonCommand(GetBook);
			BooksCount = "0";
		}

		private void LoadDataTable()
		{ 
			var booksList = Database.GetBooksList().Where(b => Database.GetBooksReadersList().Any(br => br.BookId == b.Id));
			booksList = booksList.Where(b => b.Name.Contains(BookName));

			List<PublicBookModel> books = new List<PublicBookModel>(0);	

			foreach (var book in booksList)
			{
				books.Add(new PublicBookModel()
				{ 
					Id = book.Id,
					Name = book.Name,
					Date = book.Date,
					Category = book.Category.Name,
					PublishHouseName = book.PublishHouseName,
				});
			}

			BooksList = new ObservableCollection<PublicBookModel>(books);
		}

		private void GetBook(object obj)
		{
			try
			{
				var booksReaders = Database.GetBooksReadersList().Where(rb => SelectedBooks.Any(b => rb.BookId == b.Id));

				foreach (var bookReader in booksReaders)
				{
					Database.Delete(bookReader);
					BookReRegistationModel reRegistation = new BookReRegistationModel()
					{
						BookId = bookReader.BookId,
						ReaderCard = null,
						ReaderCardId = null,
						Date = DateTime.Now.ToShortDateString(),
					};
					Database.Add(reRegistation);
				}

				SuccessMessage("Возврат успешно");
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		public void Update() => BooksCount = SelectedBooks.Count.ToString();
	}
}
