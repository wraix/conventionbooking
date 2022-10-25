namespace ConventionBooking.Core.Models
{
	public class Convention
	{
		public Convention(Guid id, string name) {
			this.ID = id;
			this.Name = name;
		}

		 public Guid ID { get; }
		 public string Name {get;}
	}
}