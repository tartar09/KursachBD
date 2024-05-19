using System;
using ModelViewSystem;
using DatabaseManagers;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows;
using System.Runtime.Remoting.Messaging;
using System.Linq;

namespace PenaltiesMenu
{
	public class PublicReaderModel : DataModel
	{
		public string FullName { get; set; }
		public string Category { get; set; }
	}

	public class PenaltyEditWindowModelView : DataModelEditWindowModelView
	{
		private string _reason;
		private string _value;

		private PublicReaderModel _selectedReader;
		private ObservableCollection<PublicReaderModel> _readersList;

		private string _time;

		public string Reason
		{
			get { return _reason; }
			set
			{
				_reason = value;
				OnPropertyChanged(nameof(Reason));
			}
		}
		public string Value
		{
			get { return _value; }
			set
			{ 
				_value = value;
				OnPropertyChanged(nameof(Value));
			}
		}
		public string Time
		{
			get { return _time; }
			set
			{ 
				_time = value;
				OnPropertyChanged(nameof(Time));
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

		public PenaltyEditWindowModelView() : base() 
		{
			LoadData();
		}

		public PenaltyEditWindowModelView(PenaltyModel penaltyModel) : base(penaltyModel) 
		{
			LoadData();
			SelectedReader = ReadersList.First(r => r.Id == penaltyModel.ReaderCardId);
		}

		private void LoadData()
		{
			var list = Database.GetReadersList();
			List<PublicReaderModel> readers = new List<PublicReaderModel>(0);

			foreach (var reader in list) 
			{
				readers.Add(new PublicReaderModel()
				{ 
					Id = reader.Id,
					FullName = $"{reader.ReaderCard.Surname} {reader.ReaderCard.Name[0]}.{reader.ReaderCard.Patronymic[0]}",
					Category = reader.ReaderCategory.Name,
				});
			}
			_readersList = new ObservableCollection<PublicReaderModel>(readers);
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (!int.TryParse(Time, out int time))
					throw new Exception("Время - некорректное значение");

				if (!decimal.TryParse(Value, out decimal value))
					throw new Exception("Значение - некорректное значение");

				if (SelectedReader == null)
					throw new Exception("Читатель - не выбран");

				if(Reason == "")
					throw new Exception("Причина штрафа - не указаны");

				if (time <= 0)
					throw new Exception("Время - некоректное значение");

				if (value <= 0)
					throw new Exception("Значение - некорректное значние");

				PenaltyModel penaltyModel = new PenaltyModel()
				{ 
					Reader = Database.GetReadersList().Find(r => r.Id == SelectedReader.Id).ReaderCard,
					Reason = Reason,

					Value = value,
					Time = time,

					Date = DateTime.Now.ToShortDateString(),
				};

				Database.Add(penaltyModel);
				SuccessMessage("Штраф добавлен");
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

				if (!int.TryParse(Time, out int time))
					throw new Exception("Время - некорректное значение");

				if (!decimal.TryParse(Value, out decimal value))
					throw new Exception("Значение - некорректное значение");

				if (SelectedReader == null)
					throw new Exception("Читатель - не выбран");

				if (Reason == "")
					throw new Exception("Причина штрафа - не указаны");

				PenaltyModel penaltyModel = new PenaltyModel()
				{
					Reader = Database.GetReadersList().Find(r => r.Id == SelectedReader.Id).ReaderCard,
					Reason = Reason,

					Value = value,
					Time = time,

					Date = DateTime.Now.ToShortDateString(),
				};

				Database.Edit(penaltyModel);
				SuccessMessage("Инфомация о штрафе изменена");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

	}
}
