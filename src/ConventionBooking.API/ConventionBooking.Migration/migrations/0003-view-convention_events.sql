create view v_convention_events as
select ce.id,
       ce.convention_id,
       ce.talk_id,
       ce.venue_id,
       ce.created_at,
       ce.cancelled_at,
       ce.starts_at,
       ce.ends_at,
       ce.number_of_seats,
       c.name as convention_name,
       t.talker as talk_talker,
       t.title as talk_title,
       t.description as talk_description,
       v.external_source_id as venue_external_source_id,
       v.name as venue_name,
       v.address as venue_address
  from convention_events ce
  join convention c
    on c.id = ce.convention_id
  join venues v
    on v.id = ce.venue_id
  join talks t
    on t.id = ce.talk_id
;
