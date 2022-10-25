using MySql.Data.MySqlClient;
using ConventionBooking.Core.Models;

namespace ConventionBooking.Core.Repositories {

    public class MySqlRepository {

        public List<Venue> GetAllVenues()
        {
            List<Venue> allVenues = new List<Venue>();

            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = "select BIN_TO_UUID(id) id, external_source_id, created_at, name, address from venues";
            using var cmd = new MySqlCommand(stmt, con);

            using var rdr = cmd.ExecuteReader();

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

    }

}