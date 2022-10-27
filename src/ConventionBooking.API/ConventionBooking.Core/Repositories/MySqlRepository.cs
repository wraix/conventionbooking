using MySql.Data.MySqlClient;
using ConventionBooking.Core.Models;

namespace ConventionBooking.Core.Repositories {

    public class MySqlRepository {

        public PersonReference? LookupPersonReferenceBySub(string sub)
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = @"select
                  BIN_TO_UUID(id) id
             from person
            where sub = ?";
            using var cmd = new MySqlCommand(stmt, con);

            cmd.Parameters.AddWithValue("sub", sub);

            using var rdr = cmd.ExecuteReader();

            PersonReference? personReference = null;
            while(rdr.Read()) {
                personReference = new PersonReference(
                    Guid.Parse(rdr.GetString("id"))
                );
            }
            return personReference;
        }

        public PersonReference CreatePerson(string sub)
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            MySqlTransaction tx = con.BeginTransaction();
            try {
                var stmt = @"insert into person (id, sub, created_at)
                VALUES (UUID_TO_BIN(?), ?, now())";
                using var cmd = new MySqlCommand(stmt, con);

                var newID = Guid.NewGuid();
                cmd.Parameters.AddWithValue("id", newID.ToString());
                cmd.Parameters.AddWithValue("sub", sub);

                cmd.ExecuteNonQuery();
                tx.Commit();

                return new PersonReference(newID);
            }
            catch (MySqlException ex)
            {
                tx.Rollback();
                throw ex;
            }
        }

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

        private List<ConventionEventView> getAllConventionEvents(Guid? id, Guid? talkID, Guid? conventionID)
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
            from v_convention_events
            where 1=1";

            if (id != null) {
                stmt += " and id = UUID_TO_BIN(?)";
            }

            if (talkID != null) {
                stmt += " and talk_id = UUID_TO_BIN(?)";
            }

            if (conventionID != null) {
                stmt += " and convention_id = UUID_TO_BIN(?)";
            }

            //Console.WriteLine($"sql: {stmt} {id.ToString()}");

            using var cmd = new MySqlCommand(stmt, con);

            if (id != null) {
                cmd.Parameters.AddWithValue("id", id.ToString());
            }

            if (talkID != null) {
                cmd.Parameters.AddWithValue("talk_id", talkID.ToString());
            }

            if (conventionID != null) {
                cmd.Parameters.AddWithValue("convention_id", conventionID.ToString());
            }

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
                    rdr.IsDBNull(6) ? rdr.GetDateTime("cancelled_at") : null,
                    rdr.GetDateTime("starts_at"),
                    rdr.GetDateTime("ends_at"),
                    rdr.GetInt32("number_of_seats")
               );
               allConventionEvents.Add(newConventionEvent);
            }

            return allConventionEvents;
        }

        public List<ConventionEventView> GetAllConventionEvents()
        {
            return getAllConventionEvents(null, null, null);
        }

        public ConventionEventView? GetConventionEventViewFromID(Guid id)
        {
            return getAllConventionEvents(id, null, null).FirstOrDefault<ConventionEventView>();
        }

        public ConventionEventView? GetConventionEventViewFromTalkID(Guid talkId)
        {
            return getAllConventionEvents(null, talkId, null).FirstOrDefault<ConventionEventView>();
        }

        public ConventionEventView? GetAtleastOneConventionEventByConventionID(Guid conventionID)
        {
            return getAllConventionEvents(null, null, conventionID).FirstOrDefault<ConventionEventView>();
        }

        public SeatingReservationReference CreateSeatingReservation(ConventionEventReference ce, PersonReference p)
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            MySqlTransaction tx = con.BeginTransaction();
            try {
                var stmt = @"insert into seating_reservations (id, event_id, participant, created_at)
                VALUES (UUID_TO_BIN(?), UUID_TO_BIN(?), UUID_TO_BIN(?), now())";
                using var cmd = new MySqlCommand(stmt, con);

                var newID = Guid.NewGuid();

                cmd.Parameters.AddWithValue("id", newID.ToString());
                cmd.Parameters.AddWithValue("event_id", ce.ID.ToString());
                cmd.Parameters.AddWithValue("participant", p.ID.ToString());

                cmd.ExecuteNonQuery();
                tx.Commit();

                return new SeatingReservationReference(newID);
            }
            catch (MySqlException ex)
            {
                tx.Rollback();
                throw ex;
            }
        }

        public SeatingReservationReference CreateConventionRegistration(ConventionEventReference ce, PersonReference p)
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            MySqlTransaction tx = con.BeginTransaction();
            try {
                var stmt = @"insert into convention_registrations (id, convention_id, participant, created_at)
                VALUES (UUID_TO_BIN(?), UUID_TO_BIN(?), UUID_TO_BIN(?), now())";
                using var cmd = new MySqlCommand(stmt, con);

                var newID = Guid.NewGuid();

                cmd.Parameters.AddWithValue("id", newID.ToString());
                cmd.Parameters.AddWithValue("convention_id", ce.ID.ToString());
                cmd.Parameters.AddWithValue("participant", p.ID.ToString());

                cmd.ExecuteNonQuery();
                tx.Commit();

                return new SeatingReservationReference(newID);
            }
            catch (MySqlException ex)
            {
                tx.Rollback();
                throw ex;
            }
        }

        public List<RegistrationView> GetAllRegistrations(PersonReference p)
        {
            string cs = @"server=localhost;userid=root;password=qwerty1234;database=conventionbooking";

            using var con = new MySqlConnection(cs);
            con.Open();

            var stmt = @"select
                 max(1) is_convention_registration,
                 max(0) is_seating_reservation,
                 '00000000-0000-0000-0000-000000000000' as seating_reservation_id,
                 '00000000-0000-0000-0000-000000000000' as id,
                 BIN_TO_UUID(ce.convention_id) as convention_id,
                 '00000000-0000-0000-0000-000000000000' as talk_id,
                 '00000000-0000-0000-0000-000000000000' as venue_id,
                 now() as created_at,
                 null as cancelled_at,
                 now() as starts_at,
                 now() as ends_at,
                 0 as number_of_seats,
                 ce.convention_name,
                 '00000000-0000-0000-0000-000000000000' as talk_talker,
                 '' as talk_title,
                 '' as talk_description,
                 '' as venue_external_source_id,
                 '' as venue_name,
                 '' as venue_address
            from convention_registrations cr
            join v_convention_events ce
              on ce.convention_id = cr.convention_id
           where cr.participant = UUID_TO_BIN(@part1)
        group by ce.convention_id, ce.convention_name

            UNION ALL

          select 0 is_convention_registration,
                 1 is_seating_reservation,
                 BIN_TO_UUID(sr.id) as seating_reservation_id,
                 BIN_TO_UUID(ce.id) id,
                 BIN_TO_UUID(ce.convention_id) convention_id,
                 BIN_TO_UUID(ce.talk_id) talk_id,
                 BIN_TO_UUID(ce.venue_id) venue_id,
                 ce.created_at,
                 ce.cancelled_at,
                 ce.starts_at,
                 ce.ends_at,
                 ce.number_of_seats,
                 ce.convention_name,
                 BIN_TO_UUID(ce.talk_talker) talk_talker,
                 ce.talk_title,
                 ce.talk_description,
                 ce.venue_external_source_id,
                 ce.venue_name,
                 ce.venue_address
            from seating_reservations sr
            join v_convention_events ce
              on ce.id = sr.event_id
           where sr.participant = UUID_TO_BIN(@part2)
            ";
            using var cmd = new MySqlCommand(stmt, con);

            cmd.Parameters.AddWithValue("@part1", p.ID.ToString());
            cmd.Parameters.AddWithValue("@part2", p.ID.ToString());

            using var rdr = cmd.ExecuteReader();

            List<RegistrationView> _all = new List<RegistrationView>();
            while(rdr.Read()) {

               var newData = new RegistrationView(
                    rdr.GetInt32("is_convention_registration"),
                    rdr.GetInt32("is_seating_reservation"),
                    Guid.Parse(rdr.GetString("seating_reservation_id")),
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
                    rdr.IsDBNull(9) ? rdr.GetDateTime("cancelled_at") : null,
                    rdr.GetDateTime("starts_at"),
                    rdr.GetDateTime("ends_at"),
                    rdr.GetInt32("number_of_seats")
               );
               _all.Add(newData);
            }

            return _all;
        }

    }
}