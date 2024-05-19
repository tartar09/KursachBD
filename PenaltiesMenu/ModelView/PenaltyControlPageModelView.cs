using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using DataExport;

namespace PenaltiesMenu
{
	public class PublicPenaltyModel : PenaltyModel
	{ 
		public string FullReaderName { get; set; }
		public string ReaderCategory { get; set; }
	}

	public class PenaltyControlPageModelView : TableEditPageModelView
	{
		public PenaltyControlPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base(buttons, dataGrid, access)
		{
			buttons[5].Command = new ButtonCommand(Export);
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);
				var modelView = new PenaltyEditWindowModelView();
				WindowEvents.OpenWindow(typeof(PenaltyEditWindow), modelView);
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
				var item = Database.GetPenaltiesList().Find(p => p.Id == SelectedItem.Id);
				var modelView = new PenaltyEditWindowModelView(item);
				WindowEvents.OpenWindow(typeof(PenaltyEditWindow), modelView);
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
				var item = Database.GetPenaltiesList().Find(p => p.Id == SelectedItem.Id);
				Database.Delete(item);
				LoadDataTable();
			}
			catch (Exception ex)
			{
				ErrorMessage("Невозможно удалить элемент, так как он связан с другими элементами в базе данных.");
			}
		}

		public void Export(object obj)
		{
			try
			{
				if (SelectedItem == null)
					throw new Exception("Не выбран документ");

				var item = Database.GetPenaltiesList().Find(o => o.Id == SelectedItem.Id);
				DocumentExporter.GetInstance().CreatePenaltyDocument(item);
				SuccessMessage("Документ успешно создан");
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

		protected override void LoadDataTable()
		{
			base.LoadDataTable();
			var list = Database.GetPenaltiesList();
			List<PublicPenaltyModel> penalties = new List<PublicPenaltyModel>();

			foreach (var penalty in list)
			{
				var reader = Database.GetReadersList().Find(r => r.ReaderCardId == penalty.ReaderCardId);
				penalties.Add(new PublicPenaltyModel()
				{
					Id = penalty.Id,
					ReaderCategory = reader.ReaderCategory.Name,
					FullReaderName = $"{reader.ReaderCard.Surname} {reader.ReaderCard.Name[0]}.{reader.ReaderCard.Patronymic[0]}",
					Value = penalty.Value,
					Reason = penalty.Reason,
					Date = penalty.Date,
				});
			}

			Items = new ObservableCollection<DataModel>(penalties);
		}
	}
}
