using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace BooksMenu
{
	public class BookPublicModel : DataModel
	{
		public string Name { get; set; }
		public string Category { get; set; }
		public string StorageLocation { get; set; }
		public string PublishInfo { get; set; }
		public int AuthorsCount { get; set; }
	}

	public class BookControlPageModelView : TableEditPageModelView
	{
		private string _bookName = "";

		public string BookName
		{ 
			get { return _bookName; }
			set {
				_bookName = value;
				OnPropertyChanged(nameof(BookName));
				LoadDataTable();
			}
		}

		public BookControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{
			buttons[5].Command = new ButtonCommand(GetBook);
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new BookEditWindowModelView();
				WindowEvents.OpenWindow(typeof(BookEditWindow), modelView);
				LoadDataTable();
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
				var item = Database.GetBooksList().Find(b => b.Id == SelectedItem.Id);
				var modelView = new BookEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(BookEditWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void Delete(object obj)
		{
			try
			{
				base.Delete(obj);
				var item = Database.GetBooksList().Find(b => b.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected override void View(object obj)
		{
			try
			{
				base.View(obj);
				var item = Database.GetBooksList().Find(b => b.Id == SelectedItem.Id);
				var modelView = new BookViewWindowModelView(item);
				WindowEvents.OpenWindow(typeof(BookViewWindow), modelView);
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		private void GetBook(object obj)
		{
			try
			{
				var modelView = new GetBookWindowModelView();
				WindowEvents.OpenWindow(typeof(GetBookWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();

			var books = Database.GetBooksList();
			List<BookPublicModel> bookModels = new List<BookPublicModel>(0);

			foreach (var book in books)
			{
				bookModels.Add(new BookPublicModel()
				{
					Id = book.Id,
					Name = book.Name,
					Category = book.Category.Name,
					StorageLocation = $"{book.StorageNumber}-{book.StandNumber}-{book.ShelfNumber}",
					PublishInfo = $"{book.PublishHouseName}, {book.Date}",
					AuthorsCount = Database.GetBooksCatalogsList().Find(c => c.Id == book.CatalogId).AuthorsBooks.Count,
				});
			}

			bookModels = bookModels.Where(b => b.Name.Contains(BookName)).ToList();
			Items = new ObservableCollection<DataModel>(bookModels);
		}
	}
}
