using System;
using ModelViewSystem;
using DatabaseManagers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ReadersMenu
{
	public class ReaderEditWindowModelView : DataModelEditWindowModelView
	{
		private ReaderCategoryModel _selectedCategory;
		private ObservableCollection<ReaderCategoryModel> _categoriesList;

		private string _readerCardId;

		private string _surname;
		private string _name;
		private string _patronymic;

		private ReaderCard _readerCard;

		private FacultyModel _selectedFaculty;
		private ObservableCollection<FacultyModel> _facultiesList;
		private GroupModel _selectedGroup;
		private ObservableCollection<GroupModel> _groupsList;

		private DepartmentModel _selectedDepartment;
		private ObservableCollection<DepartmentModel> _departmentsList;
		private JobModel _selectedJob;
		private ObservableCollection<JobModel> _jobsList;
		private DegreeModel _selectedDegree;
		private ObservableCollection<DegreeModel> _degreesList;
		private RankModel _selectedRank;
		private ObservableCollection<RankModel> _ranksList;

		public FacultyModel SelectedFaculty
		{
			get { return _selectedFaculty; }
			set
			{
				_selectedFaculty = value;
				OnPropertyChanged(nameof(SelectedFaculty));
			}
		}
		public ObservableCollection<FacultyModel> FacultiesList
		{
			get { return _facultiesList; }
			set
			{
				_facultiesList = value;
				OnPropertyChanged(nameof(FacultiesList));
			}
		}
		public GroupModel SelectedGroup
		{
			get { return _selectedGroup; }
			set
			{
				_selectedGroup = value;
				OnPropertyChanged(nameof(SelectedGroup));
			}
		}
		public ObservableCollection<GroupModel> GroupsList
		{
			get { return _groupsList; }
			set
			{
				_groupsList = value;
				OnPropertyChanged(nameof(GroupsList));
			}
		}

		public DepartmentModel SelectedDepartment
		{
			get { return _selectedDepartment; }
			set
			{
				_selectedDepartment = value;
				OnPropertyChanged(nameof(SelectedDepartment));
			}
		}
		public ObservableCollection<DepartmentModel> DepartmentsList
		{
			get { return _departmentsList; }
			set
			{
				_departmentsList = value;
				OnPropertyChanged(nameof(DepartmentsList));
			}
		}
		public JobModel SelectedJob
		{
			get { return _selectedJob; }
			set
			{
				_selectedJob = value;
				OnPropertyChanged(nameof(SelectedJob));
			}
		}
		public ObservableCollection<JobModel> JobsList
		{
			get { return _jobsList; }
			set
			{
				_jobsList = value;
				OnPropertyChanged(nameof(JobsList));
			}
		}
		public DegreeModel SelectedDegree
		{
			get { return _selectedDegree; }
			set
			{
				_selectedDegree = value;
				OnPropertyChanged(nameof(SelectedDegree));
			}
		}
		public ObservableCollection<DegreeModel> DegreesList
		{
			get { return _degreesList; }
			set
			{
				_degreesList = value;
				OnPropertyChanged(nameof(DegreesList));
			}
		}
		public RankModel SelectedRank
		{
			get { return _selectedRank; }
			set
			{
				_selectedRank = value;
				OnPropertyChanged(nameof(SelectedRank));
			}
		}
		public ObservableCollection<RankModel> RanksList
		{
			get { return _ranksList; }
			set
			{
				_ranksList = value;
				OnPropertyChanged(nameof(RanksList));
			}
		}

		public string ReaderCardId
		{
			get { return _readerCardId; }
			set 
			{ 
				_readerCardId = value;
				OnPropertyChanged(nameof(ReaderCardId));
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
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
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

		public ReaderCategoryModel SelectedCategory
		{
			get { return _selectedCategory; }
			set
			{
				_selectedCategory = value;
				OnPropertyChanged(nameof(SelectedCategory));
				ChoiseCategory(_selectedCategory);
			}
		}
		public ObservableCollection<ReaderCategoryModel> CategoriesList
		{
			get { return _categoriesList; }
			set
			{
				_categoriesList = value;
				OnPropertyChanged(nameof(CategoriesList));
			}
		}

		/// <summary>
		/// Изменение элементов выбора характеристик в зависимости от категории читателя
		/// </summary>
		public Action<ReaderCategoryModel> OnSelectionsChange { get; set; }

		public ReaderEditWindowModelView() : base() 
		{
			LoadData();
		}

		public ReaderEditWindowModelView(ReaderModel readerModel) : base(readerModel)
		{
			LoadData();
			SelectedFaculty = readerModel.Faculty != null ? FacultiesList.First(f => f.Id == readerModel.FacultyId) : null;
			SelectedGroup = readerModel.Group != null ? GroupsList.First(g => g.Id == readerModel.GroupId) : null;
			SelectedDepartment = readerModel.Department != null ? DepartmentsList.First(d => d.Id == readerModel.DepartamentId) : null;
			SelectedJob = readerModel.Job != null ? JobsList.First(j => j.Id == readerModel.JobId) : null;

			SelectedDegree = readerModel.Degree != null ? DegreesList.First(d => d.Id == readerModel.DegreeId) : null;
			SelectedRank = readerModel.Rank != null ? RanksList.First(r => r.Id == readerModel.RankId) : null;

			_readerCard = readerModel.ReaderCard;

			Surname = _readerCard.Surname;
			Name = _readerCard.Name;
			Patronymic = _readerCard.Patronymic;

			SelectedCategory = CategoriesList.First(c => c.Id == readerModel.ReaderCategoryId);
			ReaderCardId = readerModel.ReaderCardId.ToString();

			ChoiseCategory(SelectedCategory);
		}

		private void LoadData()
		{
			FacultiesList = new ObservableCollection<FacultyModel>(Database.GetFacultiesList());
			GroupsList = new ObservableCollection<GroupModel>(Database.GetGroupsList());
			DepartmentsList = new ObservableCollection<DepartmentModel>(Database.GetDepartmentsList());
			JobsList = new ObservableCollection<JobModel>(Database.GetJobsList());
			DegreesList = new ObservableCollection<DegreeModel>(Database.GetDegreesList());
			RanksList = new ObservableCollection<RankModel>(Database.GetRanksList());
			CategoriesList = new ObservableCollection<ReaderCategoryModel>(Database.GetReaderCategoriesList());

			ReaderCardId = "";
			Surname = "";
			Name = "";
			Patronymic = "";
		}

		private void ChoiseCategory(ReaderCategoryModel readerCategory) => OnSelectionsChange?.Invoke(readerCategory);

		protected override void Add(object obj)
		{
			try
			{
				if (Surname == "")
					throw new Exception("Фамилия - пустое значение");

				if (Name == "")
					throw new Exception("Имя - пустое значение");

				if (Patronymic == "")
					throw new Exception("Отчество - пустое значение");

				if (SelectedCategory == null)
					throw new Exception("Котегория читателя - не выбрана");

				ReaderCard readerCard = new ReaderCard()
				{ 
					Surname = Surname,
					Name = Name,
					Patronymic = Patronymic,
					RegDate = DateTime.Now.ToShortDateString(),
					ReRegDate = DateTime.Now.ToShortDateString(),
				};

				_readerCard = readerCard;

				ReaderModel readerModel = new ReaderModel()
				{
					ReaderCard = _readerCard,

					Faculty = SelectedFaculty,
					FacultyId = SelectedFaculty?.Id,
					Group = SelectedGroup,
					GroupId = SelectedGroup?.Id,
					Department = SelectedDepartment,
					DepartamentId = SelectedDepartment?.Id,
					Job = SelectedJob,
					JobId = SelectedJob?.Id,
					Degree = SelectedDegree,
					DegreeId = SelectedDegree?.Id,
					Rank = SelectedRank,
					RankId = SelectedRank?.Id,
					ReaderCategory = SelectedCategory,
					ReaderCategoryId = SelectedCategory.Id,
				};

				Database.Add(readerModel);
				SuccessMessage("Новый читатель добавлен");
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

				if (Surname == "")
					throw new Exception("Фамилия - пустое значение");

				if (Name == "")
					throw new Exception("Имя - пустое значение");

				if (Patronymic == "")
					throw new Exception("Отчество - пустое значение");

				if (!int.TryParse(ReaderCardId, out int cardId))
					throw new Exception("Читательский билет - некорректное значение");

				if (SelectedCategory == null)
					throw new Exception("Котегория читателя - не выбрана");

				_readerCard = new ReaderCard()
				{
					Id = _readerCard.Id,
					Surname = Surname,
					Name = Name,
					Patronymic = Patronymic,
					RegDate = _readerCard.RegDate,
					ReRegDate = DateTime.Now.ToShortDateString(),
				};

				ReaderModel readerModel = new ReaderModel()
				{
					Id = DataModel.Id,
					ReaderCard = _readerCard,
					ReaderCardId = cardId,

					Faculty = SelectedFaculty,
					FacultyId = SelectedFaculty?.Id,
					Group = SelectedGroup,
					GroupId = SelectedGroup?.Id,
					Department = SelectedDepartment,
					DepartamentId = SelectedDepartment?.Id,
					Job = SelectedJob,
					JobId = SelectedJob?.Id,
					Degree = SelectedDegree,
					DegreeId = SelectedDegree?.Id,
					Rank = SelectedRank,
					RankId = SelectedRank?.Id,
					ReaderCategory = SelectedCategory,
					ReaderCategoryId = SelectedCategory.Id,
				};

				Database.Edit(readerModel);
				SuccessMessage("Информация о читателе изменена");
				WindowVisibility = Visibility.Hidden;
			}
			catch (Exception ex)
			{
				ErrorMessage(ex.Message);
			}
		}
	}
}
