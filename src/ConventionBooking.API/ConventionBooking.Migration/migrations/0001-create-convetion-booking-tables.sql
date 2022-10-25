create table convention (
    id binary(16) primary key, -- compromise store UUID as binary to reduce size and increase query speed
    name text not null
);

create table venues (
    id binary(16) primary key,
    external_source_id varchar(255) not null,
    created_at timestamp not null,
    name text not null,
    address text not null
);

create table talks (
    id binary(16) primary key,
    talker binary(16) not null,
    created_at timestamp not null,
    title text not null,
    description text
    -- foreign key (talker) references person(id) -- if person is different service then we cannot do this
);

-- convention schedules talks at venues
create table convention_events (
    id binary(16) primary key,
    convention_id binary(16) not null,
    talk_id binary(16) not null,
    venue_id binary(16) not null,
    created_at timestamp not null,
    cancelled_at timestamp,
    starts_at timestamp not null,
    ends_at timestamp not null,
    number_of_seats int not null default 0,
    foreign key (convention_id) references convention(id),
    foreign key (talk_id) references talks(id),
    foreign key (venue_id) references venues(id)
);

create table seating_reservations (
    id binary(16) primary key,
    event_id binary(16) not null,
    participant binary(16) not null,
    created_at timestamp not null,
    cancelled_at timestamp,
    foreign key (event_id) references convention_events(id)
    -- foreign key (participant) references person(id) -- if person is different service then we cannot do this
);

create table convention_registrations (
    id binary(16) primary key,
    convention_id binary(16) not null,
    participant binary(16) not null,
    created_at timestamp not null,
    cancelled_at timestamp,
    foreign key (convention_id) references convention(id)
    -- foreign key (participant) references person(id) -- if person is different service then we cannot do this
);

-- TODO: move into seperate database
-- Identities of people authenticated by the IDP.
-- Holds all the GDPR sensitive data, so cleaning can be done by nulling out the fields
-- but keeping the id for referential integrity.
create table person (
    id binary(16) primary key,
    sub varchar(255) not null unique,
    create_at timestamp not null,
    deleted_at timestamp null,
    name text,
    address text,
    email text
);