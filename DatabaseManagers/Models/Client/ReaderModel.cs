
namespace DatabaseManagers
{
	public class ReaderModel : DataModel
	{
		public int ReaderCategoryId { get; set; }
		public int ReaderCardId { get; set; }

		public int? FacultyId { get; set; }
		public int? GroupId { get; set; }

		public int? DepartamentId { get; set; }
		public int? JobId { get; set; }
		public int? DegreeId { get; set; }
		public int? RankId { get; set; }

		public virtual FacultyModel Faculty { get; set; }
		public virtual GroupModel Group { get; set; }

		public virtual DepartmentModel Department { get; set; }
		public virtual JobModel Job { get; set; }
		public virtual DegreeModel Degree { get; set; }
		public virtual RankModel Rank { get; set; }


		public virtual ReaderCategoryModel ReaderCategory { get; set; }
		public virtual ReaderCard ReaderCard { get; set; }
	}
}
