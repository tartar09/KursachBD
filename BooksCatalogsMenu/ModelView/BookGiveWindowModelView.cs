using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BooksCatalogsMenu
{
	public class PublicReaderModel : ReaderModel
	{
		public string FullName { get; set; }
		public string Category { get; set; }

		public string Name { get; set; }

	}

	public class BookGiveWindowModelView : BaseUIModelView
	{
		private string _name;
		private string _publishHouseName;

		private PublicReaderModel _selectedReader;
		private ObservableCollection<PublicReaderModel> _readersList;

		private BookCatalogModel _bookCatalog;

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

		public ObservableCollection<PublicReaderModel> ReadersList
		{
			get { return _readersList; }
			set
			{
				_readersList = value;
				OnPropertyChanged(nameof(ReadersList));
			}
		}
		public PublicReaderModel SelectedReader
		{
			get { return _selectedReader; }
			set
			{
				_selectedReader = value;
				OnPropertyChanged(nameof(SelectedReader));
			}
		}
		public BookCatalogModel BookCatalog
		{
			get { return _bookCatalog; }
			set
			{
				_bookCatalog = value;
				OnPropertyChanged(nameof(BookCatalog));
			}
		}

		public ButtonCommand ButtonCommand { get; private set; }

		protected DatabaseManager Database => DatabaseManager.GetInstance();

		public BookGiveWindowModelView(BookCatalogModel bookCatalog) : base()
		{
			_bookCatalog = bookCatalog;
			ButtonCommand = new ButtonCommand(Entry);

			var bookInfo = bookCatalog.Books.ToArray()[0];
			Name = bookInfo.Name;
			PublishHouseName = bookInfo.PublishHouseName;

			LoadData();
		}

		private void LoadData()
		{
			List<PublicReaderModel> readerModels = new List<PublicReaderModel>(0);

			foreach (var reader in Database.GetReadersList())
			{
				readerModels.Add(new PublicReaderModel()
				{ 
					Id = reader.Id,
					FullName = $"{reader.ReaderCard.Surname} {reader.ReaderCard.Name[0]}.{reader.ReaderCard.Patronymic[0]}",
					ReaderCard = reader.ReaderCard,
					ReaderCardId = reader.ReaderCardId,
					ReaderCategory = reader.ReaderCategory,
					Category = reader.ReaderCategory.Name,
					Name = $"{reader.ReaderCard.Surname} {reader.ReaderCard.Name[0]}.{reader.ReaderCard.Patronymic[0]} {reader.ReaderCategory.Name}",
				});
			}

			ReadersList = new ObservableCollection<PublicReaderModel>(readerModels);
		}

		private void Entry(object obj)
		{
			try
			{
				if (SelectedReader == null)
					throw new Exception("Читатель не выбран");

				var reader = Database.GetReadersList().Find(r => r.Id == SelectedReader.Id);

				if (_bookCatalog.AvailableCount == 0)
					throw new Exception("Нет доступных книг");

				var BooksReadersList = Database.GetBooksReadersList();
				var book = _bookCatalog.Books.First(b => !BooksReadersList.Any(br => br.BookId == b.Id));

				var bookType = Database.GetBookTypesList().Find(t => t.Id == _bookCatalog.BookTypeId);

				if (bookType.OnlyRead > 0)
					throw new Exception("Книга предназначена только для чтения");

				if (SelectedReader.ReaderCategory.CanBorrow == 0)
					throw new Exception("Категория читателей не позволяет брать книгу");

				var thisCatalogOrder = Database.GetOrdersList().FirstOrDefault(o => o.BookCatalog.Id == _bookCatalog.Id && o.ReaderCard.Id == SelectedReader.ReaderCardId);

				if (thisCatalogOrder != null)
					Database.Delete(thisCatalogOrder);

				BookReRegistationModel bookReRegistation = new BookReRegistationModel()
				{
					Book = book,
					BookId = book.Id,
					ReaderCard = reader.ReaderCard,
					ReaderCardId = reader.ReaderCardId,
					Date = DateTime.Now.ToShortDateString(),
				};

				BooksReaders booksReaders = new BooksReaders()
				{
					BookId = book.Id,
					ReaderCardId = reader.ReaderCardId,
					Date = DateTime.Now.ToShortDateString(),
				};

				Database.Add(booksReaders);
				Database.Add(bookReRegistation);

				SuccessMessage("Запись успешно выполнена");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}
	}
}
