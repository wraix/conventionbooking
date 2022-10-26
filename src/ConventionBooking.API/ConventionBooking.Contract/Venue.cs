namespace ConventionBooking.Contract {

#pragma warning disable CS8618 // Disable  warning CS8618: Non-nullable property, as this is a DTO
	public class Venue {
		public Guid ID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }

		/// <summary>
    	/// Capacity is the number of people allowed on site before fire inspectors will shut it down
    	/// </summary>
		public int Capacity {get; set;}
	}
#pragma warning restore CS8618

}