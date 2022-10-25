namespace ConventionBooking.Core.Models
{
	public class SeatingReservation
	{
		public SeatingReservation(Guid id, Guid eventID, Guid participantID, DateTime createdAt, DateTime? cancelledAt) {
			this.ID = id;
			this.EventID = eventID;
            this.ParticipantID = participantID;
			this.CreatedAt = createdAt;
            this.CancelledAt = cancelledAt;
		}

		 public Guid ID { get; }
		 public Guid EventID {get;}
         public Guid ParticipantID { get; }
		 public DateTime CreatedAt {get;}
         public DateTime? CancelledAt {get;}
	}
}