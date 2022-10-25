namespace ConventionBooking.Core.Models
{
	public class Venue
	{
		public Venue(Guid id, string externalSourceId, DateTime createdAt, string name, string address) {
			this.ID = id;
			this.ExternalSourceID = externalSourceId;
			this.CreatedAt = createdAt;
			this.Name = name;
			this.Address = address;
		}

		 public Guid ID { get; }
		 public string ExternalSourceID {get;}
		 public DateTime CreatedAt {get;}
		 public string Name {get;}
		 public string Address {get;}
	}
}