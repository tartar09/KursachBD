using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Windows;

namespace GroupsMenu
{
	public class GroupEditWindowModelView : CatalogEditModelView
	{
		public GroupEditWindowModelView() : base()
		{
			Tittle = "Новая группа";
			Description = "Название группы";
		}

		public GroupEditWindowModelView(GroupModel groupModel) : base(groupModel)
		{
			Tittle = "Редактирование группы";
			Description = "Название группы";
			Name = groupModel.Name;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (Name == "")
					throw new Exception("Наименование - пусто");

				GroupModel groupModel = new GroupModel()
				{
					Name = Name,
				};

				Database.Add(groupModel);
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

				if (Name == "")
					throw new Exception("Наименование - пусто");

				GroupModel groupModel = new GroupModel()
				{
					Id = DataModel.Id,
					Name = Name,
				};

				Database.Edit(groupModel);
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
