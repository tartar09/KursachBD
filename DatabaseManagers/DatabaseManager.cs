using Microsoft.EntityFrameworkCore;
using System;
using SQLitePCL;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace DatabaseManagers
{
	public class DatabaseManager : DbContext
	{
		// SINGLETON
		#region SINGLETON

		private static DatabaseManager _instance;
		public static DatabaseManager GetInstance()
		{
			if (_instance == null)
				_instance = new DatabaseManager();
			return _instance;
		}

		#endregion

		//Связь с таблицами базы данных
		/// <summary> Пользователи </summary>
		private DbSet<UserModel> Users { get; set; }
		/// <summary> Меню </summary>
		private DbSet<MenuItemModel> MenuItems { get; set; }
		/// <summary> Доступ к меню </summary>
		private DbSet<AccessModel> MenuAccess { get; set; }


		/// <summary> Факультеты </summary>
		private DbSet<FacultyModel> Faculties { get; set; }
		/// <summary> Группы </summary>
		private DbSet<GroupModel> Groups { get; set; }
		/// <summary> Звания </summary>
		private DbSet<RankModel> Ranks { get; set; }
		/// <summary> Степени </summary>
		private DbSet<DegreeModel> Degrees { get; set; }
		/// <summary> Отделы </summary>
		private DbSet<DepartmentModel> Departments { get; set; }
		/// <summary> Должности </summary>
		private DbSet<JobModel> Jobs { get; set; }
		/// <summary> Категории книг </summary>
		private DbSet<BookCategoryModel> BookCategories { get; set; }
		/// <summary> Категории читателей </summary>
		private DbSet<ReaderCategoryModel> ReaderCategories { get; set; }


		/// <summary> Авторы книг </summary>
		private DbSet<AuthorModel> Authors { get; set; }
		/// <summary> Книги </summary>
		private DbSet<BookModel> Books { get; set; }

		/// <summary> Связь книг и авторов </summary>
		private DbSet<AuthorsBooks> AuthorsBooks { get; set; }
		/// <summary> Группы книг </summary>
		private DbSet<BookCatalogModel> BookCatalogs { get; set; }
		/// <summary> Тип книги (чтениеб срок выдачи) </summary>
		private DbSet<BookTypeModel> BookTypes { get; set; }
		/// <summary> Передачи книг </summary>
		private DbSet<BookReRegistationModel> BooksReRegistration { get; set; }

		/// <summary> Читатель </summary>
		public DbSet<ReaderModel> Readers { get; set; }
		/// <summary> Читательские билеты </summary>
		public DbSet<ReaderCard> ReaderCards { get; set; }
		/// <summary> Карточка читателя </summary>
		public DbSet<BooksReaders> BooksReaders { get; set; }


		/// <summary> Штрафы </summary>
		public DbSet<PenaltyModel> Penalties { get; set; }
		/// <summary> Заказы </summary>
		public DbSet<OrderModel> Orders { get; set; }


		#region MODEL_CREATING
		/// <summary>
		/// Переопределенный метод для настройки параметров подключения к базе данных.
		/// </summary>
		/// <param name="optionsBuilder">Построитель опций контекста базы данных.</param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			string connectString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
			string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

			connectString = baseDirectory + "\\" + connectString;

			optionsBuilder.UseSqlite($"Data Source={connectString}");
			optionsBuilder.UseLazyLoadingProxies();
		}

		public DatabaseManager() : base()
		{
			Batteries.Init();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AccessModel>().ToTable("MenuAccess");
			modelBuilder.Entity<AccessModel>().HasKey(e => e.Id);
			modelBuilder.Entity<AccessModel>()
				.HasOne(m => m.MenuItem)
				.WithMany()
				.HasForeignKey(m => m.MenuId);
			modelBuilder.Entity<AccessModel>()
				.HasOne(m => m.User)
				.WithMany()
				.HasForeignKey(m => m.UserId);

			modelBuilder.Entity<BookModel>()
				.HasOne(m => m.Catalog)
				.WithMany()
				.HasForeignKey(m => m.CatalogId);
			modelBuilder.Entity<BookModel>()
				.HasOne(m => m.Category)
				.WithMany()
				.HasForeignKey(m => m.CategoryId);

			modelBuilder.Entity<BookReRegistationModel>()
				.HasOne(m => m.Book)
				.WithMany()
				.HasForeignKey(m => m.BookId);
			modelBuilder.Entity<BookReRegistationModel>()
				.HasOne(m => m.ReaderCard)
				.WithMany()
				.HasForeignKey(m => m.ReaderCardId);

			modelBuilder.Entity<BookCatalogModel>()
				.HasOne(m => m.BookType)
				.WithMany()
				.HasForeignKey(m => m.BookTypeId);
			modelBuilder.Entity<BookCatalogModel>()
				.HasMany(m => m.Books)
				.WithOne(e => e.Catalog)
				.HasForeignKey(e => e.CatalogId);

			modelBuilder.Entity<BookCatalogModel>()
				.HasMany(c => c.AuthorsBooks)
				.WithOne()
				.HasForeignKey(e => e.BookCatalogId);

			modelBuilder.Entity<AuthorsBooks>()
						.HasKey(e => e.Id);

			modelBuilder.Entity<AuthorsBooks>()
				.HasOne(ab => ab.Author)
				.WithMany(a => a.AuthorsBooks)
				.HasForeignKey(ab => ab.AuthorId);

			modelBuilder.Entity<AuthorsBooks>()
				.HasOne(ab => ab.BookCatalog)
				.WithMany(bc => bc.AuthorsBooks)
				.HasForeignKey(ab => ab.BookCatalogId);

			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.Faculty)
				.WithMany()
				.HasForeignKey(m => m.FacultyId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.Group)
				.WithMany()
				.HasForeignKey(m => m.GroupId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.Department)
				.WithMany()
				.HasForeignKey(m => m.DepartamentId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.Job)
				.WithOne()
				.HasForeignKey<ReaderModel>(m => m.JobId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.Degree)
				.WithOne()
				.HasForeignKey<ReaderModel>(m => m.DegreeId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.Rank)
				.WithOne()
				.HasForeignKey<ReaderModel>(m => m.RankId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.ReaderCategory)
				.WithMany()
				.HasForeignKey(m => m.ReaderCategoryId);
			modelBuilder.Entity<ReaderModel>()
				.HasOne(m => m.ReaderCard)
				.WithOne()
				.HasForeignKey<ReaderModel>(m => m.ReaderCardId);

			modelBuilder.Entity<PenaltyModel>()
				.HasOne(m => m.Reader)
				.WithOne()
				.HasForeignKey<PenaltyModel>(m => m.ReaderCardId);

			modelBuilder.Entity<OrderModel>()
				.HasOne(m => m.BookCatalog)
				.WithOne()
				.HasForeignKey<OrderModel>(m => m.BookCatalogId);

			modelBuilder.Entity<OrderModel>()
				.HasOne(m => m.ReaderCard)
				.WithOne()
				.HasForeignKey<OrderModel>(m => m.ReaderCardId);

		}

		#endregion

		// Пользователи
		#region USERS

		public List<UserModel> GetUsersList()
		{
			var userList = Users.ToList();
			return Users.ToList();
		}

		public void Add(UserModel user)
		{

			if (user.Login.Length < 4)
				throw new Exception("Логин слишком короткий");

			if (user.Password.Length < 5)
				throw new Exception("Пароль слишком короткий");

			if (Users.Any(u => u.Login == user.Login))
				throw new Exception($"Пользователь с логином {user.Login} уже существует");

			user.Password = Encryption.EncryptString(user.Password);

			Users.Add(user);
			SaveChanges();

			var defaultAccess = LoadDefaultAccess(user);

			foreach (var access in defaultAccess)
				Add(access);

			SaveChanges();
		}

		public void Edit(UserModel user)
		{
			var userToEdit = Users.Find(user.Id);

			if (user.Password.Length < 5)
				throw new Exception("Пароль слишком короткий");

			user.Password = Encryption.EncryptString(user.Password);

			if (userToEdit == null)
				throw new Exception("Данные для редактирования не найдены");

			Entry(userToEdit).CurrentValues.SetValues(user);
			SaveChanges();
		}

		public void Delete(UserModel user)
		{
			if (!Users.Any(u => u.Id == user.Id))
				throw new Exception("Данного пользователя не существует");

			Users.Remove(user);
			SaveChanges();
		}

		/// <summary>
		/// Авторизация пользователя
		/// </summary>
		public int Authorization(string login, string password)
		{
			password = Encryption.EncryptString(password);

			if (!Users.Any(u => u.Login == login && u.Password == password))
				throw new Exception("Неверный логин или пароль");

			return Users.First(u => u.Login == login && u.Password == password).Id;
		}

		#endregion

		// Меню
		#region MENU_ITEMS

		public List<MenuItemModel> GetMenuList() => MenuItems.ToList();

		#endregion

		// Доступ к меню
		#region ACCESS

		private static readonly int[] DefaultItemsId = new int[] { 3, 22, 24, 25 };

		private List<AccessModel> LoadDefaultAccess(UserModel user)
		{
			List<AccessModel> access = new List<AccessModel>();

			var menuList = GetMenuList().Where(m => DefaultItemsId.Contains(m.Id)).ToList();

			foreach (var menu in menuList)
			{
				Add(new AccessModel()
				{ 
					UserId = user.Id,
					User = user,

					MenuId = menu.Id,
					MenuItem = menu,

					Read = 1,
					Add = 1,
					Edit = 1,
					Delete = 1,
				});
			}

			return access;
		}

		public List<AccessModel> GetMenuAccessList() => MenuAccess.OrderBy(access => access.MenuItem.Order).ToList();

		/// <summary>
		/// Загрузка доступа к родительскому меню
		/// </summary>
		private List<AccessModel> LoadParentMenuAccess() => GetMenuAccessList().Where(a => a.MenuItem.DLL == null || a.MenuItem.Key == null).ToList();
			
		/// <summary>
		/// Загрузка меню для пользователя
		/// </summary>
		public List<AccessModel> LoadItems(UserModel user)
		{
			var userAcess = MenuAccess.Where(a => a.UserId == user.Id).OrderBy(access => access.MenuItem.Order).ToList();

			List<AccessModel> newAccess = LoadParentMenuAccess();

			foreach (var access in newAccess)
			{
				if (!userAcess.Any(a => a.MenuId == access.MenuId))
					userAcess.Add(access);
			}

			userAcess = userAcess.OrderBy(a => a.MenuItem.Order).ToList();
			return userAcess;
		}

		public void Add(AccessModel menuItemAccessModel)
		{
			if (MenuAccess.Any(access => access.MenuId == menuItemAccessModel.MenuId && access.UserId == menuItemAccessModel.UserId))
				throw new Exception("Подобный доступ уже открыт");

			MenuAccess.Add(menuItemAccessModel);
			SaveChanges();
		}

		public void Edit(AccessModel menuItemAccessModel)
		{
			var accessToEdit = MenuAccess.Find(menuItemAccessModel.Id);

			if (accessToEdit == null)
				throw new Exception("Данные для редактирования не найдены");

			Entry(accessToEdit).CurrentValues.SetValues(menuItemAccessModel);
			SaveChanges();

		}

		public void Delete(AccessModel menuItemAccessModel)
		{
			var accessToDelete = MenuAccess.Find(menuItemAccessModel.Id);

			if (DefaultItemsId.Contains(menuItemAccessModel.MenuId))
				throw new Exception("Это меню - выдается по умолчанию");

			if (accessToDelete == null)
				throw new Exception("Данные для редактирования не найдены");

			MenuAccess.Remove(accessToDelete);
			SaveChanges();
		}

		#endregion

		// Факультеты
		#region FACULTY

		public List<FacultyModel> GetFacultiesList() => Faculties.ToList();

		public void Add(FacultyModel facultyModel)
		{
			if (Faculties.Any(f => f.Id != facultyModel.Id && f.Name == facultyModel.Name))
				throw new Exception("Факультет с таким названием уже существует");

			Faculties.Add(facultyModel);
			SaveChanges();
		}

		public void Edit(FacultyModel facultyModel)
		{
			if (Faculties.Any(f => f.Id != facultyModel.Id && f.Name == facultyModel.Name))
				throw new Exception("Факультет с таким названием уже существует");

			var facultyToEdit = Faculties.Find(facultyModel.Id);
			Entry(facultyToEdit).CurrentValues.SetValues(facultyModel);
			SaveChanges();
		}

		public void Delete(FacultyModel facultyModel)
		{
			Faculties.Remove(facultyModel);
			SaveChanges();
		}

		#endregion

		//группы
		#region GROUP

		public List<GroupModel> GetGroupsList() => Groups.ToList();

		public void Add(GroupModel groupModel)
		{
			if (Groups.Any(g => g.Id != groupModel.Id && g.Name == groupModel.Name))
				throw new Exception("Группа с таким названием уже существует");

			Groups.Add(groupModel);
			SaveChanges();
		}

		public void Edit(GroupModel groupModel)
		{
			if (Groups.Any(g => g.Id != groupModel.Id && g.Name == groupModel.Name))
				throw new Exception("Группа с таким названием уже существует");

			var groupToEdit = Groups.Find(groupModel.Id);
			Entry(groupToEdit).CurrentValues.SetValues(groupModel);
			SaveChanges();
		}

		public void Delete(GroupModel groupModel)
		{
			Groups.Remove(groupModel);
			SaveChanges();
		}

		#endregion

		//звание
		#region RANK

		public List<RankModel> GetRanksList() => Ranks.ToList();

		public void Add(RankModel rankModel)
		{
			if (Ranks.Any(r => r.Id != rankModel.Id && r.Name == rankModel.Name))
				throw new Exception("Звание с таким названием уже существует");

			Ranks.Add(rankModel);
			SaveChanges();
		}

		public void Edit(RankModel rankModel)
		{
			if (Ranks.Any(r => r.Id != rankModel.Id && r.Name == rankModel.Name))
				throw new Exception("Звание с таким названием уже существует");

			var rankToEdit = Ranks.Find(rankModel.Id);
			Entry(rankToEdit).CurrentValues.SetValues(rankModel);
			SaveChanges();
		}

		public void Delete(RankModel rankModel)
		{
			Ranks.Remove(rankModel);
			SaveChanges();
		}

		#endregion

		//степень
		#region DEGREE

		public List<DegreeModel> GetDegreesList() => Degrees.ToList();

		public void Add(DegreeModel degreeModel)
		{
			if (Degrees.Any(d => d.Id != degreeModel.Id && d.Name == degreeModel.Name))
				throw new Exception("Степень с таким названием уже существует");

			Degrees.Add(degreeModel);
			SaveChanges();
		}

		public void Edit(DegreeModel degreeModel)
		{
			if (Degrees.Any(d => d.Id != degreeModel.Id && d.Name == degreeModel.Name))
				throw new Exception("Степень с таким названием уже существует");


			var degreeToEdit = Degrees.Find(degreeModel.Id);
			Entry(degreeToEdit).CurrentValues.SetValues(degreeModel);
			SaveChanges();
		}

		public void Delete(DegreeModel degreeModel)
		{
			Degrees.Remove(degreeModel);
			SaveChanges();
		}

		#endregion

		//отделы
		#region DEPARTMENT

		public List<DepartmentModel> GetDepartmentsList() => Departments.ToList();

		public void Add(DepartmentModel departmentModel)
		{
			if (Departments.Any(d => d.Id != departmentModel.Id && d.Name == departmentModel.Name))
				throw new Exception("Отдел с таким названием уже существует");

			Departments.Add(departmentModel);
			SaveChanges();
		}

		public void Edit(DepartmentModel departmentModel)
		{
			if (Departments.Any(d => d.Id != departmentModel.Id && d.Name == departmentModel.Name))
				throw new Exception("Отдел с таким названием уже существует");

			var departmentToEdit = Departments.Find(departmentModel.Id);
			Entry(departmentToEdit).CurrentValues.SetValues(departmentModel);
			SaveChanges();
		}

		public void Delete(DepartmentModel departmentModel)
		{
			Departments.Remove(departmentModel);
			SaveChanges();
		}

		#endregion

		//должности
		#region JOB

		public List<JobModel> GetJobsList() => Jobs.ToList();

		public void Add(JobModel jobModel)
		{
			if (Jobs.Any(j => j.Id != jobModel.Id && j.Name == jobModel.Name))
				throw new Exception("Должность с таким названием уже существует");

			Jobs.Add(jobModel);
			SaveChanges();
		}

		public void Edit(JobModel jobModel)
		{
			if (Jobs.Any(j => j.Id != jobModel.Id && j.Name == jobModel.Name))
				throw new Exception("Должность с таким названием уже существует");

			var jobToEdit = Jobs.Find(jobModel.Id);
			Entry(jobToEdit).CurrentValues.SetValues(jobModel);
			SaveChanges();
		}

		public void Delete(JobModel jobModel)
		{
			Jobs.Remove(jobModel);
			SaveChanges();
		}

		#endregion

		//категории книг
		#region BOOK_CATEGORY

		public List<BookCategoryModel> GetBookCategoriesList() => BookCategories.ToList();

		public void Add(BookCategoryModel bookCategoryModel)
		{
			if (BookCategories.Any(bc => bc.Id != bookCategoryModel.Id && bc.Name == bookCategoryModel.Name))
				throw new Exception("Категория книг с таким названием уже существует");

			BookCategories.Add(bookCategoryModel);
			SaveChanges();
		}

		public void Edit(BookCategoryModel bookCategoryModel)
		{
			if (BookCategories.Any(bc => bc.Id != bookCategoryModel.Id && bc.Name == bookCategoryModel.Name))
				throw new Exception("Категория книг с таким названием уже существует");

			var categoryToEdit = BookCategories.Find(bookCategoryModel.Id);
			Entry(categoryToEdit).CurrentValues.SetValues(bookCategoryModel);
			SaveChanges();
		}

		public void Delete(BookCategoryModel bookCategoryModel)
		{
			BookCategories.Remove(bookCategoryModel);
			SaveChanges();
		}

		#endregion

		//категории читалей
		#region READER_CATEGORY

		public List<ReaderCategoryModel> GetReaderCategoriesList() => ReaderCategories.ToList();

		public void Add(ReaderCategoryModel readerCategory)
		{
			if (ReaderCategories.Any(rc => rc.Id != readerCategory.Id && rc.Name == readerCategory.Name))
				throw new Exception("Категория читателей с таким названием уже существует");

			ReaderCategories.Add(readerCategory);
			SaveChanges();
		}

		public void Edit(ReaderCategoryModel readerCategory)
		{
			if (ReaderCategories.Any(rc => rc.Id != readerCategory.Id && rc.Name == readerCategory.Name))
				throw new Exception("Категория читателей с таким названием уже существует");

			var categoryToEdit = ReaderCategories.Find(readerCategory.Id);
			Entry(categoryToEdit).CurrentValues.SetValues(readerCategory);
			SaveChanges();
		}

		public void Delete(ReaderCategoryModel readerCategory)
		{
			ReaderCategories.Remove(readerCategory);
			SaveChanges();
		}

		#endregion

		//авторы книг
		#region AUTHOR

		public List<AuthorModel> GetAuthorsList() => Authors.ToList();

		public void Add(AuthorModel author)
		{
			Authors.Add(author);
			SaveChanges();
		}

		public void Edit(AuthorModel author)
		{
			var authorToEdit = Authors.Find(author.Id);
			Entry(authorToEdit).CurrentValues.SetValues(author);
			SaveChanges();
		}

		public void Delete(AuthorModel author)
		{
			Authors.Remove(author);
			SaveChanges();
		}

		#endregion

		//книги
		#region BOOKS

		public List<BookModel> GetBooksList() => Books.ToList();

		public void Add(BookModel book)
		{
			Books.Add(book);
			SaveChanges();
		}

		public void Edit(BookModel book)
		{
			var bookToEdit = Books.Find(book.Id);
			Entry(bookToEdit).CurrentValues.SetValues(book);
			SaveChanges();
		}

		public void Delete(BookModel book)
		{
			Books.Remove(book);
			SaveChanges();
		}


		#endregion

		//перерегистрация книг
		#region BOOK_REREGISTRATION

		public List<BookReRegistationModel> GetBookReRegistationsList() => BooksReRegistration.ToList();

		public void Add(BookReRegistationModel registration)
		{
			BooksReRegistration.Add(registration);
			SaveChanges();
		}

		public void Delete(BookReRegistationModel registration)
		{
			BooksReRegistration.Remove(registration);
			SaveChanges();
		}

		#endregion

		//авторы книг - книги
		#region AUTHOR_BOOK

		public List<AuthorsBooks> GetAuthorsBooksList() => AuthorsBooks.ToList();

		public void Add(AuthorsBooks[] authorsBooks)
		{
			AuthorsBooks.AddRange(authorsBooks);
			SaveChanges();
		}

		public void Edit(AuthorsBooks[] authorsBooks)
		{
			int catalogId = authorsBooks[0].BookCatalogId;

			var list = AuthorsBooks.Where(m => m.BookCatalogId == catalogId).ToArray();
			Delete(list);

			SaveChanges();
		}

		public void Delete(AuthorsBooks[] authorsBooks)
		{
			AuthorsBooks.RemoveRange(authorsBooks);
			SaveChanges();
		}


		#endregion

		//группы книг
		#region BOOK_CATALOG

		public List<BookCatalogModel> GetBooksCatalogsList() => BookCatalogs.ToList();

		public void Add(BookCatalogModel bookCatalog, BookModel bookModel)
		{
			if (bookCatalog.Count <= 0)
				throw new Exception("Количество книг не может быть меньше или равно 0");

			BookCatalogs.Add(bookCatalog);
			SaveChanges();

			int count = bookCatalog.Count;
			bookModel.CatalogId = bookCatalog.Id;

			while (count > 0)
			{
				Add(new BookModel()
				{
					StorageNumber = bookModel.StorageNumber,
					StandNumber = bookModel.StandNumber,
					ShelfNumber = bookModel.ShelfNumber,

					Name = bookModel.Name,
					Date = bookModel.Date,
					PublishHouseName = bookModel.PublishHouseName,

					CategoryId = bookModel.CategoryId,
					CatalogId = bookModel.CatalogId,
				});
				count--;
			}

			SaveChanges();
		}

		public void Edit(BookCatalogModel bookCatalog, BookModel bookModel)
		{
			var booksCopy = bookCatalog.Books.ToList();

			foreach (var book in booksCopy)
			{
				var bookToEdit = Books.Find(book.Id);

				BookModel newBookModel = new BookModel()
				{
					Id = bookToEdit.Id,
					Catalog = bookToEdit.Catalog,
					CatalogId = bookToEdit.CatalogId,
					StorageNumber = bookModel.StorageNumber,
					StandNumber = bookModel.StandNumber,
					ShelfNumber = bookModel.ShelfNumber,
					Name = bookModel.Name,
					Date = bookModel.Date,
					PublishHouseName = bookModel.PublishHouseName,
					CategoryId = bookModel.Category.Id,
					Category = bookModel.Category,
				};

				Entry(bookToEdit).CurrentValues.SetValues(newBookModel);
				SaveChanges();
			}
		}

		public void Delete(BookCatalogModel bookCatalog)
		{
			foreach (BookModel bookModel in bookCatalog.Books)
				Books.Remove(bookModel);
			SaveChanges();

			BookCatalogs.Remove(bookCatalog);
			SaveChanges();
		}

		#endregion

		//заказы
		#region ORDERS

		public List<OrderModel> GetOrdersList() => Orders.ToList();

		public void Add(OrderModel order)
		{
			Orders.Add(order);
			SaveChanges();
		}

		public void Add(List<OrderModel> order)
		{
			Orders.AddRange(order);
			SaveChanges();
		}

		public void Edit(OrderModel order)
		{
			var orderToEdit = Orders.Find(order.Id);
			Entry(orderToEdit).CurrentValues.SetValues(order);
			SaveChanges();
		}

		public void Edit(List<OrderModel> orders)
		{
			var reader = orders[0].ReaderCardId;
			var ordersToDelete = Orders.Where(o => o.ReaderCardId == reader);

			Orders.RemoveRange(ordersToDelete);
			Orders.AddRange(orders);
			SaveChanges();
		}

		public void Delete(OrderModel order)
		{
			Orders.Remove(order);
			SaveChanges();
		}

		public void Delete(List<OrderModel> orders)
		{
			foreach (var order in orders)
			{ 
				if(Orders.Contains(order))
					Orders.Remove(order);
			}

			SaveChanges();
		}

		#endregion

		//штрафы
		#region PENALTY

		public List<PenaltyModel> GetPenaltiesList() => Penalties.ToList();

		public void Add(PenaltyModel penaltyModel)
		{
			Penalties.Add(penaltyModel);
			SaveChanges();
		}

		public void Edit(PenaltyModel penaltyModel)
		{
			var penaltyToEdit = Penalties.Find(penaltyModel.Id);
			Entry(penaltyToEdit).CurrentValues.SetValues(penaltyModel);
			SaveChanges();
		}

		public void Delete(PenaltyModel penaltyModel)
		{
			Penalties.Remove(penaltyModel);
			SaveChanges();
		}

		#endregion

		//читальский билет
		#region READER_CARD

		public List<ReaderCard> ReaderCardsList() => ReaderCards.ToList();

		public void Add(ReaderCard readerCard)
		{ 
			ReaderCards.Add(readerCard);
			SaveChanges();
		}

		public void Edit(ReaderCard readerCard)
		{
			var cardToEdit = ReaderCards.Find(readerCard.Id);
			Entry(cardToEdit).CurrentValues.SetValues(readerCard);
			SaveChanges();
		}

		public void Delete(ReaderCard readerCard)
		{
			ReaderCards.Remove(readerCard);
			SaveChanges();
		}

		#endregion

		//читатель
		#region READER

		public List<ReaderModel> GetReadersList() => Readers.ToList();

		private void DataCheck(ReaderModel readerModel)
		{
			if (readerModel.ReaderCategoryId == 1 || readerModel.ReaderCategoryId == 2 || readerModel.ReaderCategoryId == 4)
			{
				if (readerModel.Faculty == null)
					throw new Exception("Факультет не указан");
			}

			if (readerModel.ReaderCategoryId == 1 || readerModel.ReaderCategoryId == 4)
			{
				if (readerModel.Group == null)
					throw new Exception("Группа не указана");
			}

			if (readerModel.ReaderCategoryId == 2 || readerModel.ReaderCategoryId == 3)
			{
				if (readerModel.Job == null)
					throw new Exception("Должность не указана");
			}

			if (readerModel.ReaderCategoryId == 2)
			{ 
				if(readerModel.Degree == null)
					throw new Exception("Степень не указана");

				if(readerModel.Rank == null)
					throw new Exception("Звание не указана");
			}

			if (readerModel.ReaderCategoryId == 3)
			{
				if (readerModel.Department == null)
					throw new Exception("Отдел не указан");
			}
		}

		public void Add(ReaderModel readerModel)
		{
			DataCheck(readerModel);
			Add(readerModel.ReaderCard);
			readerModel.ReaderCardId = readerModel.ReaderCard.Id;
			Readers.Add(readerModel);
			SaveChanges();
		}

		public void Edit(ReaderModel readerModel)
		{
			DataCheck(readerModel);
			Edit(readerModel.ReaderCard);
			var readerToEdit = Readers.Find(readerModel.Id);
			Entry(readerToEdit).CurrentValues.SetValues(readerModel);
			SaveChanges();
		}

		public void Delete(ReaderModel readerModel)
		{
			Readers.Remove(readerModel);
			SaveChanges();
		}

		#endregion

		//связь читателей и книг
		#region BOOKS_READERS

		public List<BooksReaders> GetBooksReadersList() => BooksReaders.ToList();

		public void Add(BooksReaders bookReaderModel)
		{
			BooksReaders.Add(bookReaderModel);
			SaveChanges();
		}

		public void Edit(BooksReaders bookReaderModel)
		{
			var toEdit = BooksReaders.Find(bookReaderModel.Id);
			Entry(toEdit).CurrentValues.SetValues(bookReaderModel);
			SaveChanges();
		}

		public void Delete(BooksReaders bookReaderModel)
		{
			BooksReaders.Remove(bookReaderModel);
			SaveChanges();
		}

		#endregion

		//тип книг (чтение и срок выдачи)
		#region BOOK_TYPES

		public List<BookTypeModel> GetBookTypesList() => BookTypes.ToList();

		public void Add(BookTypeModel bookTypeModel)
		{
			if (bookTypeModel.AccessTime <= 0)
				throw new Exception("Срок выдачи меньше или равен 0");

			BookTypes.Add(bookTypeModel);	
			SaveChanges();
		}

		public void Edit(BookTypeModel bookTypeModel)
		{
			if (bookTypeModel.AccessTime <= 0)
				throw new Exception("Срок выдачи меньше или равен 0");

			var toEdit = BookTypes.Find(bookTypeModel.Id);
			Entry(toEdit).CurrentValues.SetValues(bookTypeModel);
			SaveChanges();
		}

		public void Delete(BookTypeModel bookTypeModel)
		{
			BookTypes.Remove(bookTypeModel);
			SaveChanges();
		}

		#endregion
	}
}
