using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;

namespace ReadersMenu
{
	public class PublicReaderModel : DataModel
	{
		public string FullName { get; set; }
		public int BooksCount { get;set; }
		public int PenaltyCount { get; set; }
		public string ReaderCategory { get; set; }
	}

	public class ReaderControlPageModelView : TableEditPageModelView
	{
		private string _surname = "";
		private string _name = "";
		private string _patronymic = "";


		public string Surname
		{ 
			get { return _surname; } 
			set 
			{
				_surname = value;
				OnPropertyChanged(nameof(Surname));
				LoadDataTable();
			}
		}
		public string Name
		{
			get { return _name; }
			set
			{ 
				_name = value;
				OnPropertyChanged(nameof(Name));
				LoadDataTable();
			}
		}
		public string Patronymic
		{ 
			get { return _patronymic; }
			set 
			{
				_patronymic = value;
				OnPropertyChanged(nameof(Patronymic));
				LoadDataTable();
			}
		}

		public ReaderControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new ReaderEditWindowModelView();
				WindowEvents.OpenWindow(typeof(ReaderEditWindow), modelView);
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
				var item = Database.GetReadersList().Find(r => r.Id == SelectedItem.Id);
				var modelView = new ReaderEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(ReaderEditWindow), modelView);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void View(object obj)
		{
			try
			{
				base.View(obj);
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
				var item = Database.GetReadersList().Find(r => r.Id == SelectedItem.Id);
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
			var list = Database.GetReadersList().Where(r => r.ReaderCard.Name.Contains(Name) && r.ReaderCard.Surname.Contains(Surname) && r.ReaderCard.Patronymic.Contains(Patronymic)).ToList();

			List<PublicReaderModel> readerModels = new List<PublicReaderModel>(0);

			foreach (var item in list)
			{
				readerModels.Add(new PublicReaderModel()
				{ 
					Id = item.Id,
					ReaderCategory = item.ReaderCategory.Name,
					FullName = $"{item.ReaderCard.Surname} {item.ReaderCard.Name[0]}.{item.ReaderCard.Patronymic[0]}",
					PenaltyCount = Database.GetPenaltiesList().Where(p => p.ReaderCardId == item.ReaderCard.Id).Count(),
					BooksCount = Database.GetBooksReadersList().Where(m => m.ReaderCardId == item.ReaderCard.Id).Count(),
				});
			}

			Items = new ObservableCollection<DataModel>(readerModels);
		}
	}
}
