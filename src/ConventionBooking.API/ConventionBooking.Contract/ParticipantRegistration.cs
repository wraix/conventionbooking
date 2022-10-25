namespace ConventionBooking.Contract {

#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class ParticipantRegistration {
		public String Name {get;set;}
        public String Address {get;set;}
        public String PhoneNumber {get;set;}
        public String Email {get;set;}
	}
#pragma warning restore CS8618
}
