using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using DatabaseManagers;
using DataExport;
using ModelViewSystem;

namespace BooksCatalogsMenu
{
	public class PublicBookCatalog : DataModel
	{ 
		public string Name { get; set; }
		public string CategoryName { get; set; }
		public string PublishInfo { get; set; }
		public int BooksCount { get; set; }
	}

	public class BooksCatalogControlPageModelView : TableEditPageModelView
	{
		private string _bookName = "";

		public string BookName
		{
			get { return _bookName; }
			set
			{
				_bookName = value;
				OnPropertyChanged(nameof(BookName));
			}
		}

		public BooksCatalogControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{
			buttons[5].Command = new ButtonCommand(BookGive);
			buttons[6].Command = new ButtonCommand(Find);
			buttons[7].Command = new ButtonCommand(ExportToTable);
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new BooksCatalogEditWindowModelView();
				WindowEvents.OpenWindow(typeof(BooksCatalogEditWindow), modelView);
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
				var item = Database.GetBooksCatalogsList().Find(b => b.Id == SelectedItem.Id);
				var modelView = new BooksCatalogEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(BooksCatalogEditWindow), modelView);
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
				var item = Database.GetBooksCatalogsList().Find(b => b.Id == SelectedItem.Id);
				Database.Delete(item);
				SuccessMessage("Успешно удалено");
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected void ExportToTable(object obj)
		{
			try
			{
				List<BookCatalogModel> bookCatalogs = Database.GetBooksCatalogsList().Where(b => Items.Any(i => i.Id == b.Id)).ToList();
				DocumentExporter.GetInstance().CreateCatalogsTable(bookCatalogs);
			}
			catch (Exception ex)
			{ 
				SuccessMessage(ex.Message);
			}
		}

		protected void BookGive(object obj)
		{
			try
			{
				base.Edit(obj);
				var item = Database.GetBooksCatalogsList().Find(b => b.Id == SelectedItem.Id);
				var modelView = new BookGiveWindowModelView(item);
				WindowEvents.OpenWindow(typeof(BookGiveWindow), modelView);
			}
			catch (Exception ex) 
			{
				ErrorMessage(ex.Message);
			}
		}

		protected void Find(object obj) => LoadDataTable();

		protected override void LoadDataTable()
		{
			var catalogs = Database.GetBooksCatalogsList();
			List<PublicBookCatalog> bookModels = new List<PublicBookCatalog>(0);

			foreach (var catalog in catalogs)
			{
				var book = catalog.Books.ToList()[0];

				bookModels.Add(new PublicBookCatalog()
				{
					Id = catalog.Id,
					Name = book.Name,
					CategoryName = catalog.Books.ToList()[0].Category.Name,
					PublishInfo = $"{book.PublishHouseName}, {book.Date}",
					BooksCount = catalog.Books.Count,
				});
			}

			bookModels = bookModels.Where(b => b.Name.Contains(BookName)).ToList();
			Items = new ObservableCollection<DataModel>(bookModels);
		}
	}
}
