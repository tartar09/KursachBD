using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Windows;

namespace AuthorsMenu
{
	public class AuthorEditWindowModelView : DataModelEditWindowModelView
	{
		private string _surname;
		private string _name;
		private string _patronymic;

		public string Name
		{ 
			get { return _name; }
			set
			{ 
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string Surname
		{
			get { return _surname; }
			set
			{ 
				_surname = value;
				OnPropertyChanged(nameof(Surname));
			}
		}
		public string Patronymic
		{
			get { return _patronymic; }
			set
			{
				_patronymic = value;
				OnPropertyChanged(nameof(Patronymic));
			}
		}


		public AuthorEditWindowModelView() : base()
		{
			Tittle = "Новый автор";
			Name = "";
			Surname = "";
			Patronymic = "";
		}

		public AuthorEditWindowModelView(AuthorModel authorModel) : base(authorModel)
		{
			Tittle = "Редактирование автора";
			Surname = authorModel.Surname;
			Name = authorModel.Name;
			Patronymic = authorModel.Patronymic;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Surname == "" || Name == "")
					throw new Exception("Фамилия и Имя не могут быть пустыми");

				AuthorModel authorModel = new AuthorModel()
				{
					Surname = Surname,
					Name = Name,
					Patronymic = Patronymic,
				};

				Database.Add(authorModel);
				SuccessMessage("Успешно добавлено");
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

				if (Surname == "" || Name == "")
					throw new Exception("Фамилия и Имя не могут быть пустыми");

				AuthorModel authorModel = new AuthorModel()
				{
					Id = DataModel.Id,
					Surname = Surname,
					Name = Name,
					Patronymic = Patronymic,
				};

				Database.Edit(authorModel);
				SuccessMessage("Успешно отредактировано");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}

	}
}
