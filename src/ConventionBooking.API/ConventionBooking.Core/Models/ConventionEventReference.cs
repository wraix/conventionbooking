namespace ConventionBooking.Core.Models
{
	public class ConventionEventReference
	{
		public ConventionEventReference(Guid id) {
			this.ID = id;
		}

	    public Guid ID { get; }
	}
}