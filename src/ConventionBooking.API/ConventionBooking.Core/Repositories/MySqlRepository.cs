using MySql.Data.MySqlClient;
using ConventionBooking.Core.Models;

namespace ConventionBooking.Core.Repositories {

    public class MySqlRepository {

        public List<Venue> GetAllVenues()
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = @"select
              BIN_TO_UUID(id) id,
              external_source_id,
              created_at, name,
              address
            from venues";
            using var cmd = new MySqlCommand(stmt, con);

            using var rdr = cmd.ExecuteReader();

            List<Venue> allVenues = new List<Venue>();
            while(rdr.Read()) {
               var newVenue = new Venue(
                    Guid.Parse(rdr.GetString("id")),
                    rdr.GetString("external_source_id"),
                    rdr.GetDateTime("created_at"),
                    rdr.GetString("name"),
                    rdr.GetString("address")
               );
               allVenues.Add(newVenue);
            }

            return allVenues;
        }

        public List<Convention> GetAllConventions()
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = "select BIN_TO_UUID(id) id, name from convention";
            using var cmd = new MySqlCommand(stmt, con);

            using var rdr = cmd.ExecuteReader();

            List<Convention> allConventions = new List<Convention>();
            while(rdr.Read()) {
               var newConvention = new Convention(
                    Guid.Parse(rdr.GetString("id")),
                    rdr.GetString("name")
               );
               allConventions.Add(newConvention);
            }

            return allConventions;
        }

        /// <summary>
        /// List all talks scheduled for events
        /// </summary>
        /// <returns>A list of talks scheduled for events.</returns>
        public List<Talk> GetAllTalks()
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = "select BIN_TO_UUID(id) id, BIN_TO_UUID(talker) talker, created_at, title, description from talks";
            using var cmd = new MySqlCommand(stmt, con);

            using var rdr = cmd.ExecuteReader();

            List<Talk> allTalks = new List<Talk>();
            while(rdr.Read()) {
               var newTalk = new Talk(
                    Guid.Parse(rdr.GetString("id")),
                    Guid.Parse(rdr.GetString("talker")),
                    rdr.GetDateTime("created_at"),
                    rdr.GetString("title"),
                    rdr.GetString("description")
               );
               allTalks.Add(newTalk);
            }

            return allTalks;
        }

        public List<ConventionEventView> GetAllConventionEvents()
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = @"select
              BIN_TO_UUID(id) id,
              BIN_TO_UUID(convention_id) convention_id,
              BIN_TO_UUID(talk_id) talk_id,
              BIN_TO_UUID(venue_id) venue_id,
              created_at,
              cancelled_at,
              starts_at,
              ends_at,
              number_of_seats,
              convention_name,
              BIN_TO_UUID(talk_talker) talk_talker,
              talk_title,
              talk_description,
              venue_external_source_id,
              venue_name,
              venue_address
            from v_convention_events";
            using var cmd = new MySqlCommand(stmt, con);

            using var rdr = cmd.ExecuteReader();

            List<ConventionEventView> allConventionEvents = new List<ConventionEventView>();
            while(rdr.Read()) {
               var newConventionEvent = new ConventionEventView(
                    Guid.Parse(rdr.GetString("id")),

                    Guid.Parse(rdr.GetString("convention_id")),
                    rdr.GetString("convention_name"),

                    Guid.Parse(rdr.GetString("talk_id")),
                    Guid.Parse(rdr.GetString("talk_talker")),
                    rdr.GetString("talk_title"),
                    rdr.GetString("talk_description"),

                    Guid.Parse(rdr.GetString("venue_id")),
                    rdr.GetString("venue_name"),
                    rdr.GetString("venue_address"),

                    rdr.GetDateTime("created_at"),
                    rdr.IsDBNull(11) ? rdr.GetDateTime("cancelled_at") : null,
                    rdr.GetDateTime("starts_at"),
                    rdr.GetDateTime("ends_at"),
                    rdr.GetInt32("number_of_seats")
               );
               allConventionEvents.Add(newConventionEvent);
            }

            return allConventionEvents;
        }

    }

}