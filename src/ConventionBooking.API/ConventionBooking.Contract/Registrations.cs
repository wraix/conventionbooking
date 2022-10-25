namespace ConventionBooking.Contract
{
#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class Registrations
	{
		public Participant participant {get;set;}
        public List<ConventionSignup> SignedUpForConventions {get;set;}
        public List<SeatReservation> ReservedSeatsAtTalks {get; set;}
	}
#pragma warning restore CS8618
}