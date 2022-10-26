namespace ConventionBooking.Contract
{
#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class Event
	{
		 public Guid ID { get; set; }
         public Convention Convention {get; set;}
         public Talk Talk {get;set;}
         public Venue Venue {get;set;}
         public DateTime StartsAt {get;set;}
         public DateTime EndsAt {get;set;}
		 public int NumberOfSeats {get;set;}
	}
#pragma warning restore CS8618
}
