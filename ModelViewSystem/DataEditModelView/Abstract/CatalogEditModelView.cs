using DatabaseManagers;

namespace ModelViewSystem
{
	/// <summary>
	/// ModelView для страницы редактирования/создания экземпляра справочника
	/// </summary>
	public class CatalogEditModelView : DataModelEditWindowModelView
	{
		private string _name;
		private string _description;

		public string Name
		{
			get { return _name; }
			set 
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}	
		}
		public string Description
		{
			get { return _description; }
			set
			{ 
				_description = value; 
				OnPropertyChanged(nameof(Description));	
			}
		}


		public CatalogEditModelView() : base()
		{
			Description = "";
		}

		public CatalogEditModelView(DataModel dataModel) : base(dataModel)
		{
			Description = "";
		}

		protected override void Add(object obj)
		{ 
			
		}

		protected override void Edit(object obj)
		{ 
					
		}

	}
}
