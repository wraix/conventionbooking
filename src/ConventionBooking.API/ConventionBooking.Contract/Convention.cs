namespace ConventionBooking.Contract
{
#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class Convention
	{
		 public Guid ID { get; set; }
		 public string Name {get;set;}
		 public Venue Venue {get; set;}
	}
#pragma warning restore CS8618
}
