using System.Collections.Generic;

namespace DatabaseManagers
{
	public class AuthorModel : DataModel
	{
		public string Surname { get; set; }
		public string Name { get; set; }
		public string Patronymic { get; set; }

		public virtual ICollection<AuthorsBooks> AuthorsBooks { get; set; }
	}
}
