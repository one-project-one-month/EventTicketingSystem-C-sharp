-- Sample data for tbl_businessemail
INSERT INTO public.tbl_businessemail (businessemailid, businessemailcode, fullname, phone, email, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('busem001', 'BE000001', 'Alice Smith', '111-222-3333', 'alice.smith@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem002', 'BE000002', 'Bob Johnson', '444-555-6666', 'bob.johnson@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem003', 'BE000003', 'Charlie Brown', '777-888-9999', 'charlie.brown@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem004', 'BE000004', 'Diana Prince', '123-456-7890', 'diana.prince@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem005', 'BE000005', 'Eve Adams', '987-654-3210', 'eve.adams@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem006', 'BE000006', 'Frank White', '101-202-3030', 'frank.white@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem007', 'BE000007', 'Grace Kelly', '303-202-1010', 'grace.kelly@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem008', 'BE000008', 'Harry Potter', '505-606-7070', 'harry.potter@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem009', 'BE000009', 'Ivy Green', '808-707-6060', 'ivy.green@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem010', 'BE000010', 'Jack Black', '909-808-7070', 'jack.black@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem011', 'BE000011', 'Karen Blue', '112-233-4455', 'karen.blue@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem012', 'BE000012', 'Liam Red', '223-344-5566', 'liam.red@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem013', 'BE000013', 'Mia Yellow', '334-455-6677', 'mia.yellow@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem014', 'BE000014', 'Noah Purple', '445-566-7788', 'noah.purple@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem015', 'BE000015', 'Olivia Orange', '556-677-8899', 'olivia.orange@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem016', 'BE000016', 'Peter Grey', '667-788-9900', 'peter.grey@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem017', 'BE000017', 'Quinn Violet', '778-899-0011', 'quinn.violet@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem018', 'BE000018', 'Rachel Indigo', '889-900-1122', 'rachel.indigo@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem019', 'BE000019', 'Sam Teal', '990-011-2233', 'sam.teal@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('busem020', 'BE000020', 'Tina Magenta', '001-122-3344', 'tina.magenta@example.com', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_tickettype
INSERT INTO public.tbl_tickettype (tickettypeid, tickettypecode, eventcode, tickettypename, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('tt001', 'TT000001', 'EV000001', 'VIP', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt002', 'TT000002', 'EV000002', 'General Admission', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt003', 'TT000003', 'EV000003', 'Early Bird', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt004', 'TT000004', 'EV000004', 'Student', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt005', 'TT000005', 'EV000005', 'Child', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt006', 'TT000006', 'EV000006', 'Senior', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt007', 'TT000007', 'EV000007', 'Family Pack', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt008', 'TT000008', 'EV000008', 'Group Discount', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt009', 'TT000009', 'EV000009', 'Premium', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt010', 'TT000010', 'EV000010', 'Standard', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt011', 'TT000011', 'EV000011', 'Backstage Pass', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt012', 'TT000012', 'EV000012', 'Weekend Pass', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt013', 'TT000013', 'EV000013', 'Day Pass', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt014', 'TT000014', 'EV000014', 'Volunteer', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt015', 'TT000015', 'EV000015', 'Exhibitor', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt016', 'TT000016', 'EV000016', 'Press', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt017', 'TT000017', 'EV000017', 'Complimentary', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt018', 'TT000018', 'EV000018', 'Early Access', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt019', 'TT000019', 'EV000019', 'Late Entry', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tt020', 'TT000020', 'EV000020', 'All-Inclusive', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_venuetype
INSERT INTO public.tbl_venuetype (venuetypeid, venuetypecode, venuetypename, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('vt001', 'VT000001', 'Concert Hall', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt002', 'VT000002', 'Stadium', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt003', 'VT000003', 'Theater', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt004', 'VT000004', 'Exhibition Center', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt005', 'VT000005', 'Outdoor Arena', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt006', 'VT000006', 'Auditorium', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt007', 'VT000007', 'Club', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt008', 'VT000008', 'Convention Center', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt009', 'VT000009', 'Art Gallery', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt010', 'VT000010', 'Community Hall', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt011', 'VT000011', 'Sports Complex', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt012', 'VT000012', 'Fairground', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt013', 'VT000013', 'Restaurant/Bar', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt014', 'VT000014', 'Hotel Ballroom', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt015', 'VT000015', 'School Gymnasium', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt016', 'VT000016', 'Library', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt017', 'VT000017', 'Park', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt018', 'VT000018', 'Warehouse', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt019', 'VT000019', 'Private Residence', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('vt020', 'VT000020', 'Online Platform', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_venue
INSERT INTO public.tbl_venue (venueid, venuecode, venuetypecode, venuename, description, address, capacity, facilities, addons, venueimage, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('ven001', 'VE000001', 'VT000001', 'Grand Concert Hall', 'A majestic hall for classical and modern concerts.', '123 Music Lane, Cityville', 5000, 'Orchestra pit, Green rooms, VIP lounges', 'Catering services, Valet parking', 'grand_hall.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven002', 'VE000002', 'VT000002', 'Metropolitan Stadium', 'Large stadium for sports and major events.', '456 Sports Blvd, Townsville', 50000, 'Scoreboards, Concessions, Restrooms', 'Luxury suites, Team facilities', 'metro_stadium.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven003', 'VE000003', 'VT000003', 'Historic Grand Theater', 'An iconic theater for plays and musicals.', '789 Playwright Rd, Villageton', 1500, 'Stage, Dressing rooms, Lobby', 'Costume rental, Prop storage', 'grand_theater.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven004', 'VE000004', 'VT000004', 'City Exhibition Hall', 'Modern venue for trade shows and conventions.', '101 Expo Drive, Metropolis', 10000, 'Booth spaces, Conference rooms, Loading docks', 'Audiovisual equipment, Internet access', 'expo_hall.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven005', 'VE000005', 'VT000005', 'Riverside Outdoor Arena', 'Open-air venue by the river, perfect for summer concerts.', '202 Riverfront Path, Lakeside', 8000, 'Open seating, Food trucks, Portable restrooms', 'Picnic areas, Boat access', 'riverside_arena.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven006', 'VE000006', 'VT000006', 'University Auditorium', 'Lecture and performance space on campus.', '303 Campus Way, University City', 1000, 'Projector, Sound system, Tiered seating', 'Recording services, Green screen', 'uni_auditorium.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven007', 'VE000007', 'VT000007', 'The Underground Club', 'Intimate venue for live music and DJ sets.', '404 Alley Cat, Dancetown', 300, 'Bar, Dance floor, Sound system', 'Light show, VIP section', 'underground_club.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven008', 'VE000008', 'VT000008', 'Global Convention Center', 'Massive center for international conferences.', '505 Conference Plaza, Global City', 20000, 'Multiple halls, Meeting rooms, Business center', 'Translation services, Shuttle service', 'global_cc.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven009', 'VE000009', 'VT000009', 'Contemporary Art Gallery', 'Space for art exhibitions and cultural events.', '606 Canvas Street, Arttown', 200, 'Display walls, Lighting systems, Reception area', 'Art handling, Security', 'art_gallery.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven010', 'VE000010', 'VT000010', 'Harmony Community Hall', 'Multi-purpose hall for local events and gatherings.', '707 Main Street, Communityville', 500, 'Kitchen, Stage, Tables and chairs', 'Sound system, Projector', 'community_hall.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven011', 'VE000011', 'VT000011', 'Victory Sports Complex', 'State-of-the-art facility for various sports.', '808 Arena Way, Sportsville', 10000, 'Indoor courts, Gym, Locker rooms', 'Coaching staff, Equipment rental', 'sports_complex.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven012', 'VE000012', 'VT000012', 'County Fairgrounds', 'Large open space for fairs, carnivals, and outdoor events.', '909 Fairway, Ruraltown', 15000, 'Carnival rides, Food stalls, Livestock pens', 'Ample parking, Camping area', 'fairgrounds.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven013', 'VE000013', 'VT000013', 'The Jazz Bistro', 'Cozy restaurant with live jazz music.', '111 Melodic Mews, Jazzville', 150, 'Full bar, Kitchen, Small stage', 'Table service, Private dining', 'jazz_bistro.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven014', 'VE000014', 'VT000014', 'Grand Imperial Ballroom', 'Elegant ballroom for weddings and formal events.', '222 Grand Hotel, Elite City', 800, 'Catering kitchen, Dance floor, High ceilings', 'Valet service, Wedding planner', 'ballroom.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven015', 'VE000015', 'VT000015', 'Northwood High Gym', 'School gymnasium for local sports and community events.', '333 School Rd, Northwood', 700, 'Basketball court, Bleachers, Changing rooms', 'Scoreboard, PA system', 'high_gym.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven016', 'VE000016', 'VT000016', 'Central City Library', 'Community library hosting book readings and workshops.', '444 Bookworm Blvd, Centerville', 100, 'Meeting rooms, Reading areas, Wi-Fi', 'Projector, Whiteboard', 'city_library.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven017', 'VE000017', 'VT000017', 'Sunset Hills Park', 'Scenic park for outdoor gatherings and picnics.', '555 Green Lane, Hillside', 2000, 'Picnic tables, Grills, Playground', 'Walking trails, Restrooms', 'sunset_park.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven018', 'VE000018', 'VT000018', 'The Loft Warehouse', 'Industrial warehouse converted for art shows and pop-up events.', '666 Industrial Way, Art District', 400, 'Open space, High ceilings, Concrete floors', 'Freight elevator, Loading dock', 'loft_warehouse.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven019', 'VE000019', 'VT000019', 'The Manor Estate', 'Private estate for exclusive, high-end events.', '777 Secluded Path, Countryside', 80, 'Gardens, Grand hall, Guest rooms', 'Private chef, Security', 'manor_estate.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ven020', 'VE000020', 'VT000020', 'Virtual Event Platform', 'Online platform for virtual conferences and webinars.', 'N/A', 100000, 'Video conferencing, Chat, Polling', 'Breakout rooms, Live streaming', 'virtual_platform.jpg', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_admin
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_001', 'ADM-001', 'John Doe', 'johndoe', 'johndoe@example.com', '123-456-7890', 'hashed_password_1', 'images/john_doe.jpg', true, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_002', 'ADM-002', 'Jane Smith', 'janesmith', 'jane.smith@example.com', '987-654-3210', 'hashed_password_2', 'images/jane_smith.png', false, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_003', 'ADM-003', 'Peter Jones', 'peterjones', 'p.jones@example.com', '555-123-4567', 'hashed_password_3', null, true, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_004', 'ADM-004', 'Sarah Miller', 'sarah.m', 'sarah.miller@example.com', '111-222-3333', 'hashed_password_4', 'images/sarah_m.jpeg', false, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_005', 'ADM-005', 'Michael Brown', 'mikeb', 'michael.brown@example.com', '444-555-6666', 'hashed_password_5', null, true, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_006', 'ADM-006', 'Emily Davis', 'emilyd', 'emily.davis@example.com', '777-888-9999', 'hashed_password_6', 'images/emily_davis.webp', false, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_007', 'ADM-007', 'David Wilson', 'davidw', 'david.wilson@example.com', '999-000-1111', 'hashed_password_7', null, true, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_008', 'ADM-008', 'Jessica Lee', 'jessical', 'jessica.lee@example.com', '333-444-5555', 'hashed_password_8', 'images/jessica_l.jpg', false, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_009', 'ADM-009', 'Kevin White', 'kevinw', 'kevin.white@example.com', '666-777-8888', 'hashed_password_9', 'images/kevin_w.png', true, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_010', 'ADM-010', 'Laura Hall', 'laurah', 'laura.hall@example.com', '222-333-4444', 'hashed_password_10', null, false, 'system', '2025-08-07 06:47:33.122499', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('01K21MRD8QVJ0PQSNPV42B5YHH', 'AD000001', 'string', 'string', 'string@gmail.com', '09000000000', 'd03bfc707497c32e1170afd9a4e5128628c96b4db574aa757208b9d67c8c3991', null, true, 'a001', '2025-08-07 13:21:06.976628', null, null, false);
INSERT INTO public.tbl_admin (adminid, admincode, fullname, username, email, phone, password, profileimage, isfirsttime, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES ('admin_000', 'a001', 'Admin', 'admin', 'admin@gmail.com', '00000000000000', 'bf84493836ab9474354629d4d37b2f9756d5fe391c3da15e0542d45608014137', null, false, '1', '2025-08-07 13:06:23.000000', null, null, false);

-- Sample data for tbl_businessowner
INSERT INTO public.tbl_businessowner (businessownerid, businessownercode, fullname, email, phone, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('bo001', 'BO000001', 'Event Pro Inc.', 'contact@eventpro.com', '111-222-1111', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo002', 'BO000002', 'Concert Organizers LLC', 'info@concertorg.com', '222-333-2222', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo003', 'BO000003', 'Exhibition Masters', 'sales@exhibitionmasters.com', '333-444-3333', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo004', 'BO000004', 'Theater Productions', 'support@theaterprod.com', '444-555-4444', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo005', 'BO000005', 'Festival Fun Co.', 'hello@festivalfun.com', '555-666-5555', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo006', 'BO000006', 'Sports Event Group', 'team@sportsevent.com', '666-777-6666', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo007', 'BO000007', 'Art Show Organizers', 'art@artshow.com', '777-888-7777', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo008', 'BO000008', 'Community Connect Events', 'community@connectevents.com', '888-999-8888', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo009', 'BO000009', 'Tech Conference Solutions', 'tech@techconf.com', '999-000-9999', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo010', 'BO000010', 'Wellness Retreats', 'info@wellnessretreats.com', '000-111-0000', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo011', 'BO000011', 'Live Music Promotions', 'events@livemusic.com', '111-000-1111', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo012', 'BO000012', 'Sporting Arena Management', 'manage@arena.com', '222-111-2222', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo013', 'BO000013', 'Broadway Shows Inc.', 'info@broadwayshows.com', '333-222-3333', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo014', 'BO000014', 'Global Expo Group', 'contact@globalexpo.com', '444-333-4444', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo015', 'BO000015', 'Outdoor Adventures Ltd.', 'outdoor@adventures.com', '555-444-5555', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo016', 'BO000016', 'Academic Conferences', 'conf@academic.com', '666-555-6666', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo017', 'BO000017', 'Nightlife Entertainment', 'party@nightlife.com', '777-666-7777', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo018', 'BO000018', 'Summit Organizers', 'info@summit.com', '888-777-8888', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo019', 'BO000019', 'Gallery Events Co.', 'gallery@events.com', '999-888-9999', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('bo020', 'BO000020', 'Local Festivities', 'local@festivities.com', '000-999-0000', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_eventcategory
INSERT INTO public.tbl_eventcategory (eventcategoryid, eventcategorycode, categoryname, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('ec001', 'EC000001', 'Music Concert', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec002', 'EC000002', 'Sports', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec003', 'EC000003', 'Theater Play', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec004', 'EC000004', 'Exhibition', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec005', 'EC000005', 'Festival', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec006', 'EC000006', 'Conference', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec007', 'EC000007', 'Workshop', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec008', 'EC000008', 'Comedy Show', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec009', 'EC000009', 'Art Exhibition', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec010', 'EC000010', 'Community Gathering', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec011', 'EC000011', 'Film Screening', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec012', 'EC000012', 'Dance Performance', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec013', 'EC000013', 'Food Festival', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec014', 'EC000014', 'Fashion Show', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec015', 'EC000015', 'Charity Event', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec016', 'EC000016', 'Tech Meetup', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec017', 'EC000017', 'Literary Reading', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec018', 'EC000018', 'Kids Event', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec019', 'EC000019', 'Science Fair', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ec020', 'EC000020', 'Virtual Event', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_event
INSERT INTO public.tbl_event (eventid, eventcode, venuecode, eventname, eventcategorycode, description, address, startdate, enddate, isactive, eventstatus, businessownercode, totalticketquantity, soldoutcount, uniquename, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('evt001', 'EV000001', 'VE000001', 'Rock Legends Live', 'EC000001', 'An electrifying night with rock legends.', '123 Music Lane, Cityville', '2025-09-10 19:00:00', '2025-09-10 23:00:00', TRUE, 'Active', 'BO000001', 5000, 1500, 'rock-legends-live', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt002', 'EV000002', 'VE000002', 'Annual Soccer Championship', 'EC000002', 'The biggest soccer event of the year.', '456 Sports Blvd, Townsville', '2025-10-05 14:00:00', '2025-10-05 17:00:00', TRUE, 'Active', 'BO000006', 50000, 20000, 'annual-soccer-championship', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt003', 'EV000003', 'VE000003', 'The Phantom of the Opera', 'EC000003', 'Classic musical on a grand stage.', '789 Playwright Rd, Villageton', '2025-11-01 20:00:00', '2025-11-01 23:00:00', TRUE, 'Active', 'BO000004', 1500, 500, 'phantom-opera-show', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt004', 'EV000004', 'VE000004', 'International Tech Expo', 'EC000004', 'Showcasing the latest in technology.', '101 Expo Drive, Metropolis', '2025-11-15 09:00:00', '2025-11-17 17:00:00', TRUE, 'Active', 'BO000009', 10000, 3000, 'international-tech-expo', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt005', 'EV000005', 'VE000005', 'Summer Jazz Festival', 'EC000005', 'A weekend of smooth jazz by the river.', '202 Riverfront Path, Lakeside', '2026-06-20 18:00:00', '2026-06-22 22:00:00', FALSE, 'Upcoming', 'BO000005', 8000, 0, 'summer-jazz-festival', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt006', 'EV000006', 'VE000006', 'Future of AI Conference', 'EC000006', 'Leading experts discuss AI advancements.', '303 Campus Way, University City', '2025-10-20 09:00:00', '2025-10-21 17:00:00', TRUE, 'Active', 'BO000009', 1000, 200, 'ai-conference-2025', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt007', 'EV000007', 'VE000007', 'Local Bands Night', 'EC000001', 'Discover new talent at this energetic club night.', '404 Alley Cat, Dancetown', '2025-09-01 21:00:00', '2025-09-02 02:00:00', TRUE, 'Active', 'BO000002', 300, 100, 'local-bands-night', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt008', 'EV000008', 'VE000008', 'Global Economic Summit', 'EC000006', 'Discussions on global economic trends.', '505 Conference Plaza, Global City', '2025-12-01 09:00:00', '2025-12-03 17:00:00', TRUE, 'Active', 'BO000001', 20000, 5000, 'global-economic-summit', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt009', 'EV000009', 'VE000009', 'Modern Art Showcase', 'EC000009', 'Exhibition of contemporary art pieces.', '606 Canvas Street, Arttown', '2025-09-25 10:00:00', '2025-09-30 18:00:00', TRUE, 'Active', 'BO000007', 200, 50, 'modern-art-showcase', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt010', 'EV000010', 'VE000010', 'Community Fun Day', 'EC000010', 'A day of fun activities for the whole family.', '707 Main Street, Communityville', '2025-10-12 10:00:00', '2025-10-12 16:00:00', TRUE, 'Active', 'BO000008', 500, 0, 'community-fun-day', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt011', 'EV000011', 'VE000011', 'National Basketball Cup', 'EC000002', 'Annual basketball tournament.', '808 Arena Way, Sportsville', '2026-01-15 18:00:00', '2026-01-20 22:00:00', FALSE, 'Upcoming', 'BO000006', 10000, 0, 'national-basketball-cup', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt012', 'EV000012', 'VE000012', 'Harvest Food Festival', 'EC000013', 'Celebrate the harvest with local food and drinks.', '909 Fairway, Ruraltown', '2025-09-20 11:00:00', '2025-09-21 20:00:00', TRUE, 'Active', 'BO000005', 15000, 1000, 'harvest-food-festival', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt013', 'EV000013', 'VE000013', 'Blues Night at The Bistro', 'EC000001', 'Live blues music and gourmet food.', '111 Melodic Mews, Jazzville', '2025-08-05 19:30:00', '2025-08-05 23:00:00', TRUE, 'Active', 'BO000002', 150, 50, 'blues-night-bistro', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt014', 'EV000014', 'VE000014', 'Annual Charity Gala', 'EC000015', 'A formal event to raise funds for charity.', '222 Grand Hotel, Elite City', '2025-12-10 18:00:00', '2025-12-10 23:59:00', FALSE, 'Upcoming', 'BO000001', 800, 0, 'annual-charity-gala', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt015', 'EV000015', 'VE000015', 'School Science Fair', 'EC000019', 'Showcasing young scientific minds.', '333 School Rd, Northwood', '2026-03-01 09:00:00', '2026-03-01 15:00:00', FALSE, 'Upcoming', 'BO000008', 700, 0, 'school-science-fair', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt016', 'EV000016', 'VE000016', 'Local Author Meet & Greet', 'EC000017', 'Meet local authors and get your books signed.', '444 Bookworm Blvd, Centerville', '2025-08-20 14:00:00', '2025-08-20 16:00:00', TRUE, 'Active', 'BO000008', 100, 20, 'author-meet-greet', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt017', 'EV000017', 'VE000017', 'Yoga in the Park', 'EC000007', 'Outdoor yoga session for all levels.', '555 Green Lane, Hillside', '2025-09-05 08:00:00', '2025-09-05 09:30:00', TRUE, 'Active', 'BO000010', 50, 10, 'yoga-in-park', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt018', 'EV000018', 'VE000018', 'Experimental Art Pop-Up', 'EC000009', 'A unique exhibition in an urban setting.', '666 Industrial Way, Art District', '2025-10-01 17:00:00', '2025-10-04 22:00:00', TRUE, 'Active', 'BO000007', 400, 100, 'experimental-art-popup', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt019', 'EV000019', 'VE000019', 'Exclusive Wine Tasting', 'EC000010', 'A refined evening of wine tasting and networking.', '777 Secluded Path, Countryside', '2025-11-20 19:00:00', '2025-11-20 22:00:00', FALSE, 'Upcoming', 'BO000001', 80, 0, 'exclusive-wine-tasting', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('evt020', 'EV000020', 'VE000020', 'Global Virtual Summit', 'EC000020', 'A fully online conference for international participants.', 'N/A', '2026-02-10 09:00:00', '2026-02-12 17:00:00', FALSE, 'Upcoming', 'BO000009', 100000, 0, 'global-virtual-summit', 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_ticketprice
INSERT INTO public.tbl_ticketprice (ticketpriceid, ticketpricecode, tickettypecode, ticketprice, ticketquantity, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('tp001', 'TP000001', 'TT000001', 150.00, 500, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp002', 'TP000002', 'TT000002', 75.00, 4500, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp003', 'TP000003', 'TT000002', 25.00, 40000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp004', 'TP000004', 'TT000009', 50.00, 10000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp005', 'TP000005', 'TT000001', 200.00, 200, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp006', 'TP000006', 'TT000002', 100.00, 1300, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp007', 'TP000007', 'TT000002', 50.00, 10000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp008', 'TP000008', 'TT000003', 60.00, 3000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp009', 'TP000009', 'TT000002', 80.00, 5000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp010', 'TP000010', 'TT000002', 120.00, 1000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp011', 'TP000011', 'TT000002', 25.00, 300, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp012', 'TP000012', 'TT000009', 250.00, 5000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp013', 'TP000013', 'TT000002', 180.00, 15000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp014', 'TP000014', 'TT000002', 30.00, 200, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp015', 'TP000015', 'TT000005', 10.00, 200, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp016', 'TP000016', 'TT000002', 0.00, 300, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE), -- Free entry
('tp017', 'TP000017', 'TT000002', 35.00, 10000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp018', 'TP000018', 'TT000002', 15.00, 15000, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp019', 'TP000019', 'TT000002', 40.00, 150, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tp020', 'TP000020', 'TT000001', 500.00, 800, 'admin', '2025-07-30 14:35:24', NULL, NULL, FALSE);


-- Sample data for tbl_ticket
INSERT INTO public.tbl_ticket (ticketid, ticketcode, ticketpricecode, isused, soldout, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('tic001', 'TC000001', 'TP000001', FALSE, TRUE, 'customer1', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic002', 'TC000002', 'TP000001', FALSE, TRUE, 'customer2', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic003', 'TC000003', 'TP000002', TRUE,  TRUE, 'customer3', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic004', 'TC000004', 'TP000002', FALSE, TRUE,'customer4', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic005', 'TC000005', 'TP000003', FALSE, TRUE,'customer5', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic006', 'TC000006', 'TP000003', TRUE, TRUE,  'customer6', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic007', 'TC000007', 'TP000004', FALSE, TRUE, 'customer7', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic008', 'TC000008', 'TP000005', FALSE, TRUE,  'customer8', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic009', 'TC000009', 'TP000006', FALSE, TRUE,  'customer9', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic010', 'TC000010', 'TP000007', FALSE, TRUE,  'customer10', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic011', 'TC000011', 'TP000008', FALSE, TRUE,  'customer11', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic012', 'TC000012', 'TP000009', TRUE, TRUE,  'customer12', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic013', 'TC000013', 'TP000010', FALSE, TRUE,  'customer13', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic014', 'TC000014', 'TP000011', FALSE, TRUE,  'customer14', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic015', 'TC000015', 'TP000012', FALSE, TRUE,  'customer15', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic016', 'TC000016', 'TP000013', TRUE, TRUE,  'customer16', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic017', 'TC000017', 'TP000014', FALSE, TRUE,  'customer17', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic018', 'TC000018', 'TP000015', FALSE, TRUE, 'customer18', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic019', 'TC000019', 'TP000016', FALSE, TRUE,  'customer19', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('tic020', 'TC000020', 'TP000017', FALSE, TRUE,  'customer20', '2025-07-30 14:35:24', NULL, NULL, FALSE);

-- Sample data for tbl_transaction
INSERT INTO public.tbl_transaction (transactionid, transactioncode, email, eventcode, status, paymenttype, transactiondate, totalamount, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('tran001', 'TR000001', 'customer1@example.com', 'EV000001', 'Completed', 'Credit Card', '2025-07-29 10:00:00', 225.00, 'customer1', '2025-07-29 10:00:00', NULL, NULL, FALSE),
('tran002', 'TR000002', 'customer2@example.com', 'EV000002', 'Completed', 'PayPal', '2025-07-29 10:30:00', 50.00, 'customer2', '2025-07-29 10:30:00', NULL, NULL, FALSE),
('tran003', 'TR000003', 'customer3@example.com', 'EV000003', 'Pending', 'Bank Transfer', '2025-07-29 11:00:00', 300.00, 'customer3', '2025-07-29 11:00:00', NULL, NULL, FALSE),
('tran004', 'TR000004', 'customer4@example.com', 'EV000001', 'Completed', 'Credit Card', '2025-07-29 11:30:00', 150.00, 'customer4', '2025-07-29 11:30:00', NULL, NULL, FALSE),
('tran005', 'TR000005', 'customer5@example.com', 'EV000004', 'Completed', 'Credit Card', '2025-07-29 12:00:00', 50.00, 'customer5', '2025-07-29 12:00:00', NULL, NULL, FALSE),
('tran006', 'TR000006', 'customer6@example.com', 'EV000002', 'Completed', 'PayPal', '2025-07-29 12:30:00', 25.00, 'customer6', '2025-07-29 12:30:00', NULL, NULL, FALSE),
('tran007', 'TR000007', 'customer7@example.com', 'EV000003', 'Completed', 'Credit Card', '2025-07-29 13:00:00', 100.00, 'customer7', '2025-07-29 13:00:00', NULL, NULL, FALSE),
('tran008', 'TR000008', 'customer8@example.com', 'EV000001', 'Completed', 'PayPal', '2025-07-29 13:30:00', 75.00, 'customer8', '2025-07-29 13:30:00', NULL, NULL, FALSE),
('tran009', 'TR000009', 'customer9@example.com', 'EV000005', 'Pending', 'Credit Card', '2025-07-29 14:00:00', 60.00, 'customer9', '2025-07-29 14:00:00', NULL, NULL, FALSE),
('tran010', 'TR000010', 'customer10@example.com', 'EV000006', 'Completed', 'Bank Transfer', '2025-07-29 14:30:00', 120.00, 'customer10', '2025-07-29 14:30:00', NULL, NULL, FALSE),
('tran011', 'TR000011', 'customer11@example.com', 'EV000007', 'Completed', 'Credit Card', '2025-07-29 15:00:00', 25.00, 'customer11', '2025-07-29 15:00:00', NULL, NULL, FALSE),
('tran012', 'TR000012', 'customer12@example.com', 'EV000008', 'Completed', 'PayPal', '2025-07-29 15:30:00', 250.00, 'customer12', '2025-07-29 15:30:00', NULL, NULL, FALSE),
('tran013', 'TR000013', 'customer13@example.com', 'EV000009', 'Pending', 'Bank Transfer', '2025-07-29 16:00:00', 30.00, 'customer13', '2025-07-29 16:00:00', NULL, NULL, FALSE),
('tran014', 'TR000014', 'customer14@example.com', 'EV000010', 'Completed', 'Credit Card', '2025-07-29 16:30:00', 10.00, 'customer14', '2025-07-29 16:30:00', NULL, NULL, FALSE),
('tran015', 'TR000015', 'customer15@example.com', 'EV000011', 'Completed', 'PayPal', '2025-07-29 17:00:00', 35.00, 'customer15', '2025-07-29 17:00:00', NULL, NULL, FALSE),
('tran016', 'TR000016', 'customer16@example.com', 'EV000012', 'Completed', 'Credit Card', '2025-07-29 17:30:00', 15.00, 'customer16', '2025-07-29 17:30:00', NULL, NULL, FALSE),
('tran017', 'TR000017', 'customer17@example.com', 'EV000013', 'Pending', 'Bank Transfer', '2025-07-29 18:00:00', 40.00, 'customer17', '2025-07-29 18:00:00', NULL, NULL, FALSE),
('tran018', 'TR000018', 'customer18@example.com', 'EV000014', 'Completed', 'Credit Card', '2025-07-29 18:30:00', 500.00, 'customer18', '2025-07-29 18:30:00', NULL, NULL, FALSE),
('tran019', 'TR000019', 'customer19@example.com', 'EV000001', 'Completed', 'PayPal', '2025-07-29 19:00:00', 75.00, 'customer19', '2025-07-29 19:00:00', NULL, NULL, FALSE),
('tran020', 'TR000020', 'customer20@example.com', 'EV000002', 'Completed', 'Credit Card', '2025-07-29 19:30:00', 50.00, 'customer20', '2025-07-29 19:30:00', NULL, NULL, FALSE);

-- Sample data for tbl_transactionticket
INSERT INTO public.tbl_transactionticket (transactionticketid, transactionticketcode, transactioncode, ticketcode, qrimage, price, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('trat001', 'TI000001', 'TR000001', 'TC000001', 'qr_tc000001.png', 150.00, 'customer1', '2025-07-29 10:00:00', NULL, NULL, FALSE),
('trat002', 'TI000002', 'TR000001', 'TC000003', 'qr_tc000003.png', 75.00, 'customer1', '2025-07-29 10:00:00', NULL, NULL, FALSE),
('trat003', 'TI000003', 'TR000002', 'TC000005', 'qr_tc000005.png', 25.00, 'customer2', '2025-07-29 10:30:00', NULL, NULL, FALSE),
('trat004', 'TI000004', 'TR000002', 'TC000006', 'qr_tc000006.png', 25.00, 'customer2', '2025-07-29 10:30:00', NULL, NULL, FALSE),
('trat005', 'TI000005', 'TR000003', 'TC000008', 'qr_tc000008.png', 200.00, 'customer3', '2025-07-29 11:00:00', NULL, NULL, FALSE),
('trat006', 'TI000006', 'TR000003', 'TC000009', 'qr_tc000009.png', 100.00, 'customer3', '2025-07-29 11:00:00', NULL, NULL, FALSE),
('trat007', 'TI000007', 'TR000004', 'TC000002', 'qr_tc000002.png', 150.00, 'customer4', '2025-07-29 11:30:00', NULL, NULL, FALSE),
('trat008', 'TI000008', 'TR000005', 'TC000010', 'qr_tc000010.png', 50.00, 'customer5', '2025-07-29 12:00:00', NULL, NULL, FALSE),
('trat009', 'TI000009', 'TR000008', 'TC000004', 'qr_tc000004.png', 75.00, 'customer8', '2025-07-29 13:30:00', NULL, NULL, FALSE),
('trat010', 'TI000010', 'TR000006', 'TC000007', 'qr_tc000007.png', 25.00, 'customer6', '2025-07-29 12:30:00', NULL, NULL, FALSE),
('trat011', 'TI000011', 'TR000011', 'TC000014', 'qr_tc000014.png', 25.00, 'customer11', '2025-07-29 15:00:00', NULL, NULL, FALSE),
('trat012', 'TI000012', 'TR000012', 'TC000015', 'qr_tc000015.png', 250.00, 'customer12', '2025-07-29 15:30:00', NULL, NULL, FALSE),
('trat013', 'TI000013', 'TR000013', 'TC000017', 'qr_tc000017.png', 30.00, 'customer13', '2025-07-29 16:00:00', NULL, NULL, FALSE),
('trat014', 'TI000014', 'TR000014', 'TC000018', 'qr_tc000018.png', 10.00, 'customer14', '2025-07-29 16:30:00', NULL, NULL, FALSE),
('trat015', 'TI000015', 'TR000015', 'TC000020', 'qr_tc000020.png', 35.00, 'customer15', '2025-07-29 17:00:00', NULL, NULL, FALSE),
('trat016', 'TI000016', 'TR000016', 'TC000019', 'qr_tc000019.png', 15.00, 'customer16', '2025-07-29 17:30:00', NULL, NULL, FALSE),
('trat017', 'TI000017', 'TR000017', 'TC000013', 'qr_tc000013.png', 40.00, 'customer17', '2025-07-29 18:00:00', NULL, NULL, FALSE),
('trat018', 'TI000018', 'TR000018', 'TC000016', 'qr_tc000016.png', 500.00, 'customer18', '2025-07-29 18:30:00', NULL, NULL, FALSE),
('trat019', 'TI000019', 'TR000019', 'TC000012', 'qr_tc000012.png', 75.00, 'customer19', '2025-07-29 19:00:00', NULL, NULL, FALSE),
('trat020', 'TI000020', 'TR000020', 'TC000011', 'qr_tc000011.png', 50.00, 'customer20', '2025-07-29 19:30:00', NULL, NULL, FALSE);

-- Sample data for tbl_verification
INSERT INTO public.tbl_verification (verificationid, verificationcode, email, expiredtime, isused, createdby, createdat, modifiedby, modifiedat, deleteflag) VALUES
('ver001', 'VC000001', 'user1@example.com', '2025-07-31 15:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver002', 'VC000002', 'user2@example.com', '2025-07-31 16:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver003', 'VC000003', 'user3@example.com', '2025-07-31 17:00:00', TRUE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver004', 'VC000004', 'user4@example.com', '2025-07-31 18:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver005', 'VC000005', 'user5@example.com', '2025-07-31 19:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver006', 'VC000006', 'user6@example.com', '2025-07-31 20:00:00', TRUE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver007', 'VC000007', 'user7@example.com', '2025-07-31 21:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver008', 'VC000008', 'user8@example.com', '2025-07-31 22:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver009', 'VC000009', 'user9@example.com', '2025-07-31 23:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver010', 'VC000010', 'user10@example.com', '2025-08-01 00:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver011', 'VC000011', 'user11@example.com', '2025-08-01 01:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver012', 'VC000012', 'user12@example.com', '2025-08-01 02:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver013', 'VC000013', 'user13@example.com', '2025-08-01 03:00:00', TRUE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver014', 'VC000014', 'user14@example.com', '2025-08-01 04:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver015', 'VC000015', 'user15@example.com', '2025-08-01 05:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver016', 'VC000016', 'user16@example.com', '2025-08-01 06:00:00', TRUE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver017', 'VC000017', 'user17@example.com', '2025-08-01 07:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver018', 'VC000018', 'user18@example.com', '2025-08-01 08:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver019', 'VC000019', 'user19@example.com', '2025-08-01 09:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE),
('ver020', 'VC000020', 'user20@example.com', '2025-08-01 10:00:00', FALSE, 'system', '2025-07-30 14:35:24', NULL, NULL, FALSE);
