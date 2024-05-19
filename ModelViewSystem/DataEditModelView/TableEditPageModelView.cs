using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using DatabaseManagers;
using ModelViewSystem;

namespace ModelViewSystem
{
	/// <summary>
	/// Баовый ModelView для редактирования таблицы из базы данных
	/// </summary>
	public class TableEditPageModelView : BaseUIModelView
	{
		public ButtonCommand AddCommand { get; protected set; }
		public ButtonCommand EditCommand { get; protected set; }
		public ButtonCommand DeleteCommand { get; protected set; }
		public ButtonCommand ViewCommand { get; protected set; }

		public ButtonCommand LogOutCommand { get; private set; }

		protected DataGrid _table;

		private ObservableCollection<DataModel> _items;
		private DataModel _selectedItem;

		/// <summary>
		/// Элементы таблицы
		/// </summary>
		public ObservableCollection<DataModel> Items
		{
			get { return _items; }
			set
			{
				_items = value;
				OnPropertyChanged(nameof(Items));
			}
		}
		/// <summary>
		/// Выделенный элемент таблицы
		/// </summary>
		public DataModel SelectedItem
		{
			get { return _selectedItem; }
			set
			{
				_selectedItem = value;
				OnPropertyChanged(nameof(SelectedItem));
			}
		}

		/// <summary>
		/// База данных
		/// </summary>
		protected DatabaseManager Database => DatabaseManager.GetInstance();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="buttons"> [view, add, edit, delete, logOut] </param>
		/// <param name="dataGrid"> Основная таблица для вывода данных </param>
		/// <param name="access"> Параметры допуска </param>
		public TableEditPageModelView(Button[] buttons, DataGrid dataGrid, AccessInfo access) : base()
		{
			LogOutCommand = new ButtonCommand(LogOut);
			AddCommand = new ButtonCommand(Add);
			EditCommand = new ButtonCommand(Edit);
			DeleteCommand = new ButtonCommand(Delete);
			ViewCommand = new ButtonCommand(View);

			_table = dataGrid;

			LoadModelView(buttons);
			SetBinding();
			SetSettings();

			LoadAccessModel(buttons, access);
			LoadDataTable();
		}

		protected virtual void Add(object obj)
		{ }

		protected virtual void Edit(object obj)
		{
			if (SelectedItem == null)
				throw new Exception("Строка не выделена");
		}

		protected virtual void Delete(object obj) 
		{
			if (SelectedItem == null)
				throw new Exception("Строка не выделена");
		}

		protected virtual void View(object obj) 
		{
			if (SelectedItem == null)
				throw new Exception("Строка не выделена");
		}	

		private void LogOut(object obj) => WindowEvents.UserLogOut();

		#region SETTINGS

		/// <summary>
		/// Установка параметров
		/// </summary>
		protected virtual void SetSettings()
		{
			_table.AutoGenerateColumns = false;
			_table.SelectionMode = DataGridSelectionMode.Single;
			_table.IsReadOnly = true;
			_table.CanUserResizeColumns = false;
			_table.CanUserDeleteRows = false;
			_table.CanUserAddRows = false;
			_table.CanUserReorderColumns = false;
		}

		/// <summary>
		/// Формирование связей для ModelView (MVVM)
		/// </summary>
		protected virtual void SetBinding()
		{
			Binding itemsBinding = new Binding("Items");
			itemsBinding.Source = this;

			Binding selectedItemBinding = new Binding("SelectedItem");
			selectedItemBinding.Mode = BindingMode.TwoWay;
			selectedItemBinding.Source = this;

			_table.AutoGenerateColumns = false;
			_table.SelectionMode = DataGridSelectionMode.Single;
			_table.IsReadOnly = true;

			_table.SetBinding(DataGrid.SelectedItemProperty, selectedItemBinding);
			_table.SetBinding(DataGrid.ItemsSourceProperty, itemsBinding);
		}

		/// <summary>
		/// Загрузка ModelView (MVVM)
		/// </summary>
		protected void LoadModelView(Button[] buttons)
		{
			buttons[1].Command = AddCommand;
			buttons[2].Command = EditCommand;
			buttons[3].Command = DeleteCommand;

			if (buttons[0] != null)
				buttons[0].Command = ViewCommand;

			buttons[4].Command = LogOutCommand;

			SetBinding();
			SetSettings();
		}

		/// <summary>
		/// Доступ к кнопкам, в зависимости от доступа
		/// </summary>
		protected virtual void LoadAccessModel(Button[] buttons, AccessInfo access)
		{
			if (buttons[0] != null)
				buttons[0].IsEnabled = access.Read;

			buttons[1].IsEnabled = access.Add;
			buttons[2].IsEnabled = access.Edit;
			buttons[3].IsEnabled = access.Delete;
		}

		#endregion

		/// <summary>
		/// Загрузка таблицы данных
		/// </summary>
		protected virtual void LoadDataTable()
		{
			SelectedItem = null;
		}

	}
}
