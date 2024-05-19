﻿using DatabaseManagers;
using ModelViewSystem;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AccessControlMenu
{
	public class AccessEditWindowModelView : DataModelEditWindowModelView
	{
		#region FIELDS

		private ObservableCollection<MenuItemModel> _menuList;
		private MenuItemModel _selectedMenu;

		private ObservableCollection<UserModel> _userList;
		private UserModel _selectedUser;

		private string _buttonText;

		private bool _readAccess;
		private bool _addAccess;
		private bool _editAccess;
		private bool _deleteAccess;

		#endregion

		#region PROPERTIES

		public ObservableCollection<MenuItemModel> MenuList
		{
			get { return _menuList; }
			set
			{
				_menuList = value;
				OnPropertyChanged(nameof(MenuList));
			}
		}
		public MenuItemModel SelectedMenu
		{
			get { return _selectedMenu; }
			set
			{
				_selectedMenu = value;
				OnPropertyChanged(nameof(SelectedMenu));
			}
		}

		public ObservableCollection<UserModel> UserList
		{
			get { return _userList; }
			set
			{
				_userList = value;
				OnPropertyChanged(nameof(UserList));
			}
		}
		public UserModel SelectedUser
		{
			get { return _selectedUser; }
			set
			{
				_selectedUser = value;
				OnPropertyChanged(nameof(SelectedUser));
			}
		}

		public string ButtonText
		{
			get { return _buttonText; }
			set
			{
				_buttonText = value;
				OnPropertyChanged(nameof(ButtonText));
			}
		}

		public bool ReadAccess
		{
			get { return _readAccess; }
			set
			{
				_readAccess = value;
				OnPropertyChanged(nameof(ReadAccess));
			}
		}
		public bool AddAccess
		{
			get { return _addAccess; }
			set
			{
				_addAccess = value;
				OnPropertyChanged(nameof(AddAccess));
			}
		}
		public bool EditAccess
		{
			get { return _editAccess; }
			set
			{
				_editAccess = value;
				OnPropertyChanged(nameof(EditAccess));
			}
		}
		public bool DeleteAccess
		{
			get { return _deleteAccess; }
			set
			{
				_deleteAccess = value;
				OnPropertyChanged(nameof(DeleteAccess));
			}
		}

		#endregion

		public AccessEditWindowModelView() : base()
		{
			Tittle = "Новый доступ";
			ButtonText = "Добавить";
			LoadData();
		}

		public AccessEditWindowModelView(AccessModel accessModel) : base(accessModel)
		{
			Tittle = $"Редактирование доступа";
			ButtonText = $"Редактировать";

			LoadData();
			SelectedMenu = MenuList.First(m => m.Id == accessModel.MenuItem.Id);
			SelectedUser = UserList.First(u => u.Id == accessModel.User.Id);


			ReadAccess = accessModel.Read > 0;
			AddAccess = accessModel.Add > 0;
			EditAccess = accessModel.Edit > 0;
			DeleteAccess = accessModel.Delete > 0;
		}

		private void LoadData()
		{
			UserList = new ObservableCollection<UserModel>(Database.GetUsersList());
			MenuList = new ObservableCollection<MenuItemModel>(Database.GetMenuList());

			SelectedUser = null;
			SelectedMenu = null;
		}

		protected override void Add(object obj)
		{
			try
			{
				base.Add(obj);

				if (SelectedUser == null)
					throw new Exception("Пользователь не выбран");

				if (SelectedMenu == null)
					throw new Exception("Меню не выбрано");

				AccessModel accessModel = new AccessModel()
				{
					MenuId = SelectedMenu.Id,
					UserId = SelectedUser.Id,

					Read = ReadAccess ? 1 : 0,
					Add = AddAccess ? 1 : 0,
					Edit = EditAccess ? 1 : 0,
					Delete = DeleteAccess ? 1 : 0
				};

				Database.Add(accessModel);
				SuccessMessage("Доступ создан");
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

				if (SelectedUser == null)
					throw new Exception("Пользователь не выбран");

				if (SelectedMenu == null)
					throw new Exception("Меню не выбрано");

				AccessModel accessModel = new AccessModel()
				{
					Id = DataModel.Id,
					MenuId = SelectedMenu.Id,
					UserId = SelectedUser.Id,

					Read = ReadAccess ? 1 : 0,
					Add = AddAccess ? 1 : 0,
					Edit = EditAccess ? 1 : 0,
					Delete = DeleteAccess ? 1 : 0
				};

				Database.Edit(accessModel);
				SuccessMessage("Успешное редактирование");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}

			LoadData();
		}
	}

}
