namespace ConventionBooking.Contract {

#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class Talk {
		public Guid ID { get; set; }
		public Talker Talker { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
	}
#pragma warning restore CS8618
}