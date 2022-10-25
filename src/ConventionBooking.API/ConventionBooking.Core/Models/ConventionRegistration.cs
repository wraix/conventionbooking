namespace ConventionBooking.Core.Models
{
	public class ConventionRegistration
	{
		public ConventionRegistration(Guid id, Guid conventionID, Guid participantID, DateTime createdAt, DateTime? cancelledAt) {
			this.ID = id;
			this.ConventionID = conventionID;
            this.ParticipantID = participantID;
            this.CreatedAt = createdAt;
            this.CancelledAt = cancelledAt;
		}

		 public Guid ID { get; }
         public Guid ConventionID { get; }
         public Guid ParticipantID { get; }
		 public DateTime CreatedAt {get;}
         public DateTime? CancelledAt {get;}
	}
}