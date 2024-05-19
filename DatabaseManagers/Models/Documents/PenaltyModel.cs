
namespace DatabaseManagers
{
	public class PenaltyModel : DataModel
	{
		public string Date { get; set; }
		public int Time { get; set; }
		public decimal Value { get; set; }
		public string Reason { get; set; }

		public int ReaderCardId { get; set; }
		public virtual ReaderCard Reader { get; set; }
	}
}
