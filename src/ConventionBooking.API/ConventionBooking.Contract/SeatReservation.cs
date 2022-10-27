namespace ConventionBooking.Contract
{
#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class SeatReservation {
		public Guid ID {get;set;}
		public Event Event {get;set;}
		public Participant Participant {get;set;}
	}
#pragma warning restore CS8618
}