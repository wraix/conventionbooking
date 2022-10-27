namespace ConventionBooking.Core.Models
{
	public class SeatingReservationReference
	{
		public SeatingReservationReference(Guid id) {
			this.ID = id;
		}

        public Guid ID { get; }
	}
}