namespace ConventionBooking.Contract
{
#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class ConventionSignup
	{
        public Participant Participant {get;set;}
		public Convention Convention {get;set;}
	}
#pragma warning restore CS8618
}