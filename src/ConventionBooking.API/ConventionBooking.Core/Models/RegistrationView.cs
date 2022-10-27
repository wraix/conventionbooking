namespace ConventionBooking.Core.Models
{
    public class RegistrationView : ConventionEventView {

        public RegistrationView(
            int isConventionRegistration,
            int isSeatReservation,
            Guid seatingReservationID,
            Guid id,
            Guid conventionID, string conventionName,
            Guid talkID, Guid talkerID, string talkTitle, string talkDescription,
            Guid venueID, string venueName, string venueAddress,
            DateTime createdAt, DateTime? cancelledAt, DateTime startsAt, DateTime endsAt, int numberOfSeats)
            : base(id, conventionID, conventionName, talkID, talkerID, talkTitle, talkDescription, venueID, venueName, venueAddress, createdAt, cancelledAt, startsAt, endsAt, numberOfSeats)
        {
            this.IsConventionRegistration = isConventionRegistration;
            this.IsSeatReservation = isSeatReservation;
            this.SeatingReservationID = seatingReservationID;
		}

        public int IsConventionRegistration {get; set;}
        public int IsSeatReservation {get; set;}
        public Guid SeatingReservationID {get; set;}
    }
}