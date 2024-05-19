using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace OrdersMenu
{
	public class PublicReaderModel : ViewModelBase
	{
		private bool _isSelected;

		public int Id { get; set; }
		public string FullName { get; set; }
		public string Category { get; set; }
		public int ReaderCardId { get; set; }

		public string Name { get; set; }

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

	public class PublicBookCatalogModel : ViewModelBase
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


	public class OrderEditWindowModelView : CatalogEditModelView
	{
		private string _readerSurname;
		private string _bookName;

		private ObservableCollection<PublicReaderModel> _readerList;
		private PublicReaderModel _selectedReader;

		private ObservableCollection<PublicBookCatalogModel> _booksList = new ObservableCollection<PublicBookCatalogModel>();
		private ObservableCollection<PublicBookCatalogModel> _selectedBooks = new ObservableCollection<PublicBookCatalogModel>();

		public ObservableCollection<PublicBookCatalogModel> BooksCatalogsList
		{
			get { return _booksList; }
			set {
				_booksList = value;
				OnPropertyChanged(nameof(BooksCatalogsList));
			}
		}
		public ObservableCollection<PublicReaderModel> ReadersList
		{
			get { return _readerList; }
			set
			{
				_readerList = value;
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
		public ObservableCollection<PublicBookCatalogModel> SelectedCatalogs
		{
			get { return _selectedBooks; }
			set
			{
				_selectedBooks = value;
				OnPropertyChanged(nameof(SelectedCatalogs));
			}
		}

		public string ReaderSurname
		{
			get { return _readerSurname; }
			set {
				_readerSurname = value;
				OnPropertyChanged(nameof(ReaderSurname));
				UpdateLists();
			}
		}
		public string BookName
		{
			get { return _bookName; }
			set
			{
				_bookName = value;
				OnPropertyChanged(nameof(BookName));
				UpdateLists();
			}
		}

		private List<OrderModel> _orders;

		public OrderEditWindowModelView() : base()
		{
			LoadDataTables(Database.GetBooksCatalogsList(), Database.GetReadersList());
			_orders = new List<OrderModel>(0);
			SelectedReader = null;
		}

		public OrderEditWindowModelView(OrderModel orderModel) : base(orderModel)
		{
			LoadDataTables(Database.GetBooksCatalogsList(), Database.GetReadersList());
			_orders = new List<OrderModel>(Database.GetOrdersList().Where(o => o.ReaderCardId == orderModel.ReaderCardId));

			SelectedReader = ReadersList.First(r => r.ReaderCardId == orderModel.ReaderCardId);
			SelectedCatalogs = new ObservableCollection<PublicBookCatalogModel>(BooksCatalogsList.Where(b => _orders.Any(o => o.BookCatalogId == b.Id)));
			LoadDataTables(Database.GetBooksCatalogsList(), Database.GetReadersList());
		}

		private void LoadDataTables(List<BookCatalogModel> booksCatalogsList, List<ReaderModel> readersList)
		{
			var readers = new List<PublicReaderModel>(0);
			var catalogs = new List<PublicBookCatalogModel>(0);

			foreach (var reader in readersList)
			{
				var newReader = new PublicReaderModel()
				{
					Id = reader.Id,
					FullName = $"{reader.ReaderCard.Surname} {reader.ReaderCard.Name[0]}.{reader.ReaderCard.Patronymic[0]}",
					Category = $"{reader.ReaderCategory.Name}",
					IsSelected = (SelectedReader != null) && (SelectedReader.Id == reader.Id),
					ReaderCardId = reader.ReaderCardId,
				};
				newReader.Name = $"{newReader.FullName} {newReader.Category}";
				readers.Add(newReader);
			}

			foreach (var catalog in booksCatalogsList)
			{
				var newCatalog = new PublicBookCatalogModel()
				{
					Id = catalog.Id,
					Name = catalog.Books.ToArray()[0].Name,
					Category = catalog.Books.ToArray()[0].Category.Name,
					PublishHouseName = catalog.Books.ToArray()[0].PublishHouseName,
					Date = catalog.Books.ToArray()[0].Date,
					IsSelected = (SelectedCatalogs != null) && (SelectedCatalogs.Any(c => c.Id == catalog.Id)),
				};
				catalogs.Add(newCatalog);
			}

			BooksCatalogsList = new ObservableCollection<PublicBookCatalogModel>(catalogs);
			ReadersList = new ObservableCollection<PublicReaderModel>(readers);
		}

		private void UpdateLists()
		{
			var catalogsList = Database.GetBooksCatalogsList().Where(c => c.Books.ToArray()[0].Name.Contains(BookName ?? "")).ToList();
			var readersList = Database.GetReadersList().Where(r => r.ReaderCard.Surname.Contains(ReaderSurname ?? "")).ToList();

			LoadDataTables(catalogsList, readersList);
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (SelectedCatalogs.Count == 0)
					throw new Exception("Книга - не выбрана");

				if (SelectedReader == null)
					throw new Exception("Читатель - не выбран");

				var reader = Database.GetReadersList().Find(r => r.Id == SelectedReader.Id);
				var orders = new List<OrderModel>(0);

				foreach (var bookModel in SelectedCatalogs)
				{
					var catalog = Database.GetBooksCatalogsList().Find(b => b.Id == bookModel.Id);

					OrderModel orderModel = new OrderModel()
					{
						ReaderCardId = reader.ReaderCard.Id,
						ReaderCard = reader.ReaderCard,
						BookCatalog = catalog,
						BookCatalogId = catalog.Id,
						Date = DateTime.Now.ToShortDateString(),
					};

					orders.Add(orderModel);
				}

				Database.Add(orders);

				SuccessMessage("Заказ создан");
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

				if (SelectedCatalogs.Count == 0)
					throw new Exception("Книга - не выбрана");

				if (SelectedReader == null)
					throw new Exception("Читатель - не выбран");

				_orders.Clear();

				var reader = Database.GetReadersList().Find(r => r.Id == SelectedReader.Id);

				foreach (var catalogModel in SelectedCatalogs)
				{
					var catalog = Database.GetBooksCatalogsList().Find(c => c.Id == catalogModel.Id);

					OrderModel orderModel = new OrderModel()
					{
						ReaderCardId = reader.ReaderCard.Id,
						ReaderCard = reader.ReaderCard,
						BookCatalog = catalog,
						BookCatalogId = catalog.Id,
						Date = DateTime.Now.ToShortDateString(),
					};

					_orders.Add(orderModel);
				}

				Database.Edit(_orders);

				SuccessMessage("Заказ отредактирован");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

	}
}
