using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BooksTypes
{
	public class BookTypeEditWindowModelView : DataModelEditWindowModelView
	{
		private string _name;
		private string _accessTime;
		private bool _isOnlyRead;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string AccessTime
		{
			get { return _accessTime; }
			set
			{
				_accessTime = value;
				OnPropertyChanged(nameof(AccessTime));
			}
		}
		public bool IsOnlyRead {
			get { return _isOnlyRead; }
			set
			{ 
				_isOnlyRead = value;
				OnPropertyChanged(nameof(IsOnlyRead));
			}
		}

		public BookTypeEditWindowModelView() : base()
		{
			Name = "";
			AccessTime = "";
			IsOnlyRead = false;
		}

		public BookTypeEditWindowModelView(BookTypeModel bookType) : base(bookType) 
		{
			Name = bookType.Name;
			AccessTime = bookType.AccessTime.ToString();
			IsOnlyRead = bookType.OnlyRead > 0;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (!int.TryParse(AccessTime, out int time))
					throw new Exception("Время - некорректное значение");

				if (Name == "")
					throw new Exception("Наименоение - пусто");

				BookTypeModel bookType = new BookTypeModel()
				{ 
					Name = Name,
					AccessTime = time,
					OnlyRead = IsOnlyRead ? 1 : 0,
				};

				Database.Add(bookType);
				SuccessMessage("Тип успешно добавлен");
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

				if (Name == "")
					throw new Exception("Наименоение - пусто");

				if (!int.TryParse(AccessTime, out int time))
					throw new Exception("Время - некорректное значение");

				BookTypeModel bookType = new BookTypeModel()
				{
					Id = DataModel.Id,
					Name = Name,
					AccessTime = time,
					OnlyRead = IsOnlyRead ? 1 : 0,
				};

				Database.Edit(bookType);
				SuccessMessage("Информация изменена");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

	}
}
