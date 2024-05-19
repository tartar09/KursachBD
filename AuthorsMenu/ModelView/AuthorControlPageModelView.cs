using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using DatabaseManagers;
using ModelViewSystem;

namespace AuthorsMenu
{
	public class AuthorPublicModel : AuthorModel
	{ 
		public string FullName { get; set; }
		public int BooksCount { get; set; }
	}

	public class AuthorControlPageModelView : TableEditPageModelView
	{
		public AuthorControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{ }

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new AuthorEditWindowModelView();
				WindowEvents.OpenWindow(typeof(AuthorEditWindow), modelView);
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
				var item = Database.GetAuthorsList().Find(a => a.Id == SelectedItem.Id);
				var modelView = new AuthorEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(AuthorEditWindow), modelView);
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
				var item = Database.GetAuthorsList().Find(a => a.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();

			var authors = Database.GetAuthorsList();
			List<AuthorPublicModel> authorsModels = new List<AuthorPublicModel>(0);

			foreach (var author in authors)
			{
				authorsModels.Add(new AuthorPublicModel()
				{ 
					Id = author.Id,
					FullName = $"{author.Surname} {author.Name[0]}.{author.Patronymic[0]}",
					BooksCount = author.AuthorsBooks == null ? 0 : author.AuthorsBooks.Count,
				});
			}

			Items = new ObservableCollection<DataModel>(authorsModels);
		}

	}
}
