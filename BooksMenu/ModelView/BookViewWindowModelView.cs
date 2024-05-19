using ModelViewSystem;
using DatabaseManagers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;

namespace BooksMenu
{
	public class PublicRegistrationModel : BookReRegistationModel
	{ 
		public string FullName { get; set; }

		public PublicRegistrationModel(BookReRegistationModel registationModel)
		{ 
			Id = registationModel.Id;
			Date = registationModel.Date;

			if (registationModel.ReaderCard == null)
				FullName = $"Принят в библиотеку";
			else
				FullName = $"{registationModel.ReaderCard.Surname} {registationModel.ReaderCard.Name[0]}. {registationModel.ReaderCard.Patronymic[0]} - {registationModel.ReaderCardId}";
		}
	}

	public class BookViewWindowModelView : BaseUIModelView
	{
		private string _name;
		private string _publishHouseName;
		private string _address;

		private ObservableCollection<BookReRegistationModel> _reRegistationsList;

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
		public string PublishHouseName
		{
			get { return _publishHouseName; }
			set
			{
				_publishHouseName = value;
				OnPropertyChanged(nameof(PublishHouseName));
			}
		}
		public string Address
		{
			get { return _address; }
			set
			{ 
				_address = value;
				OnPropertyChanged(nameof(Address));
			}
		}

		public ObservableCollection<BookReRegistationModel> ReRegistationsList
		{ 
			get { return _reRegistationsList; }
			set
			{ 
				_reRegistationsList = value;
				OnPropertyChanged(nameof(ReRegistationsList));
			}
		}

		public BookViewWindowModelView(BookModel bookModel) : base()
		{
			Address = $"{bookModel.StorageNumber}-{bookModel.StorageNumber}-{bookModel.StandNumber}";
			Name = bookModel.Name;
			PublishHouseName = bookModel.PublishHouseName;

			ReRegistationsList = new ObservableCollection<BookReRegistationModel>(DatabaseManager.GetInstance().GetBookReRegistationsList());
			LoadRegistration(bookModel);
		}

		private void LoadRegistration(BookModel book)
		{
			var list = DatabaseManager.GetInstance().GetBookReRegistationsList().Where(r => r.BookId == book.Id);
			List<PublicRegistrationModel> registrationList = new List<PublicRegistrationModel>(0);

			foreach (var item in list)
				registrationList.Add(new PublicRegistrationModel(item));

			ReRegistationsList = new ObservableCollection<BookReRegistationModel>(registrationList);
		}

	}
}
