using DatabaseManagers;

namespace ModelViewSystem
{
	/// <summary>
	/// Класс, представляющий ModelView для окна, предназначенного для редактирования строки из таблицы базы данных.
	/// </summary>
	public class DataModelEditWindowModelView : BaseUIModelView
	{
		/// <summary>
		/// Создаваемая/Редактируемая строка
		/// </summary>
		public DataModel DataModel { get; private set; }

		/// <summary>
		/// Команда для основной кнопки
		/// </summary>
		public ButtonCommand ButtonCommand { get; private set; }
		public string ButtonText { get; set; }
		/// <summary>
		/// Класс доступа к базе данных
		/// </summary>
		protected DatabaseManager Database => DatabaseManager.GetInstance();

		public DataModelEditWindowModelView() : base()
		{
			Tittle = "";
			ButtonText = "Запись";
			ButtonCommand = new ButtonCommand(Add);
		}

		public DataModelEditWindowModelView(DataModel dataModel) : base()
		{
			Tittle = "";
			ButtonText = "Редактировать";
			DataModel = dataModel;
			ButtonCommand = new ButtonCommand(Edit);
		}

		/// <summary>
		/// Добавление строки в таблицу
		/// </summary>
		protected virtual void Add(object obj)
		{

		}

		/// <summary>
		/// Редактирование существующей строки
		/// </summary>
		protected virtual void Edit(object obj)
		{

		}

	}
}
