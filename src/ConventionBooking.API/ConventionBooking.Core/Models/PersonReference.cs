namespace ConventionBooking.Core.Models
{
	public class PersonReference
	{
		public PersonReference(Guid id) {
			this.ID = id;
		}

		public Guid ID { get; }
	}
}