namespace ConventionBooking.Contract {

	public class Venue {
		public Guid ID {get;set;}

		/// <summary>
    	/// Capacity is the number of people allowed on site before fire inspectors will shut it down
    	/// </summary>
		public int Capacity {get; set;}
	}

}