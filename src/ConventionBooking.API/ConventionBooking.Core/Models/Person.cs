namespace ConventionBooking.Core.Models
{
	public class Person
	{
		public Person(Guid id, string sub, DateTime createdAt, DateTime? deletedAt, string? name, string? address, string? email) {
			this.ID = id;
			this.Sub = sub;
			this.CreatedAt = createdAt;
            this.DeletedAt = deletedAt;
			this.Name = name;
			this.Address = address;
		}

		 public Guid ID { get; }
		 public string Sub {get;}
		 public DateTime CreatedAt {get;}
         public DateTime? DeletedAt {get;}
		 public string? Name {get;}
		 public string? Address {get;}
         public string? Email {get;}
	}
}