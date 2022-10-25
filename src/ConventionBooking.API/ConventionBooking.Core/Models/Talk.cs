namespace ConventionBooking.Core.Models
{
	public class Talk
	{
		public Talk(Guid id, Guid talkerID, DateTime createdAt, string title, string? description) {
			this.ID = id;
			this.Talker = talkerID;
			this.CreatedAt = createdAt;
			this.Title = title;
			this.Description = description;
		}

		 public Guid ID { get; }
		 public Guid Talker {get;}
		 public DateTime CreatedAt {get;}
		 public string Title {get;}
		 public string? Description {get;}
	}
}