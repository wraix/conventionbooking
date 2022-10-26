namespace ConventionBooking.Core.Models
{
	public class ConventionEventView
	{
		public ConventionEventView(Guid id,
            Guid conventionID, string conventionName,
            Guid talkID, Guid talkerID, string talkTitle, string talkDescription,
            Guid venueID, string venueName, string venueAddress,
            DateTime createdAt, DateTime? cancelledAt, DateTime startsAt, DateTime endsAt, int numberOfSeats) {
			this.ID = id;

			this.ConventionID = conventionID;
            this.ConventionName = conventionName;

            this.TalkID = talkID;
            this.TalkTalkerID = talkerID;
            this.TalkTitle = talkTitle;
            this.TalkDescription = talkDescription;

            this.VenueID = venueID;
            this.VenueName = venueName;
            this.VenueAddress = venueAddress;

            this.CreatedAt = createdAt;
            this.CancelledAt = cancelledAt;
            this.StartsAt = startsAt;
            this.EndsAt = endsAt;
            this.NumberOfSeats = numberOfSeats;
		}

		public Guid ID { get; }

        public Guid ConventionID { get; }
        public string ConventionName { get; }

        public Guid TalkID { get; }
        public Guid TalkTalkerID { get; }
        public string TalkTitle { get; }
        public string TalkDescription { get; }

        public Guid VenueID { get; }
        public string VenueName {get;}
        public string VenueAddress {get;}

		public DateTime CreatedAt {get;}
        public DateTime? CancelledAt {get;}
        public DateTime StartsAt {get;}
        public DateTime EndsAt {get;}
        public int NumberOfSeats {get;}
	}
}