insert into convention (id, name)
values
(UUID_TO_BIN(UUID()), "Let's Talk About Beer"),
(UUID_TO_BIN(UUID()), "The Dark Notes"),
(UUID_TO_BIN(UUID()), "Taste N' Rate"),
(UUID_TO_BIN(UUID()), "A Day in the Life of a Brew Master"),
(UUID_TO_BIN(UUID()), "The History Of Beer")
;

insert into venues (id, external_source_id, created_at, name, address)
values
(UUID_TO_BIN(UUID()), 'carlsberg-copenhagen-qh', now(), 'Queens Hall', 'Carlsberg, Copenhagen, Denmark')
;

insert into talks (id, talker, created_at, title, description)
values
(UUID_TO_BIN(UUID()), UUID_TO_BIN(UUID()), now(), 'History of Pale Ale', 'I will tell you the story of how pale ale came to be.'),
(UUID_TO_BIN(UUID()), UUID_TO_BIN(UUID()), now(), 'History of Stout', 'It is stuck now what?'),
(UUID_TO_BIN(UUID()), UUID_TO_BIN(UUID()), now(), 'Waking up', 'What i do to prepare for the next batch'),
(UUID_TO_BIN(UUID()), UUID_TO_BIN(UUID()), now(), 'Confessions of an alcoholic', 'How my life got eaten by the brewski')
;

insert into convention_events (id, convention_id, talk_id, venue_id, created_at, starts_at, ends_at, number_of_seats)
values
(UUID_TO_BIN(UUID()),
  (select id from convention where name = 'The History Of Beer'),
  (select id from talks where title = 'History of Pale Ale'),
  (select id from venues where external_source_id = 'carlsberg-copenhagen-qh'), now(), now() + interval 10 DAY, now() + interval 11 day, 30),
(UUID_TO_BIN(UUID()),
  (select id from convention where name = 'The History Of Beer'),
  (select id from talks where title = 'History of Stout'),
  (select id from venues where external_source_id = 'carlsberg-copenhagen-qh'), now(), now() + interval 30 DAY, now() + interval 31 day, 30)
;