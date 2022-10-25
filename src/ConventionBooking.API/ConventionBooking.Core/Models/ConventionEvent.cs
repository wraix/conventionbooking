namespace ConventionBooking.Core.Models
{
	public class ConventionEvent
	{
		public ConventionEvent(Guid id, Guid conventionID, Guid talkID, Guid venueID, DateTime createdAt, DateTime? cancelledAt, DateTime startsAt, DateTime endsAt, int numberOfSeats) {
			this.ID = id;
			this.ConventionID = conventionID;
            this.TalkID = talkID;
            this.VenueID = venueID;
            this.CreatedAt = createdAt;
            this.CancelledAt = cancelledAt;
            this.StartsAt = startsAt;
            this.EndsAt = endsAt;
            this.NumberOfSeats = numberOfSeats;
		}

		 public Guid ID { get; }
         public Guid ConventionID { get; }
         public Guid TalkID { get; }
         public Guid VenueID { get; }
		 public DateTime CreatedAt {get;}
         public DateTime? CancelledAt {get;}
         public DateTime StartsAt {get;}
         public DateTime EndsAt {get;}
         public int NumberOfSeats {get;}
	}
}