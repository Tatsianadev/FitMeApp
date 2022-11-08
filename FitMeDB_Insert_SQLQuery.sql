use FitMeDB
go


insert into Gyms (Name, Address, Phone)
values 
('BigRock', '25 MainStreet, 378-94, Gdansk', '(048)586-48-58' ),
('GoldSection', '124 RedAvenue, 391-31, Gdansk', '(048)789-53-26' ),
('Caliostro', '8 BakerStreet, 380-92, Gdansk', '(048)185-03-74' ),
('FitProfit', '97 BlueStreet, 379-45, Gdansk', '(048)358-09-08' )
go

insert into Trainers (FirstName, LastName, Gender, Picture, Specialization, GymId)
values
('Barney', 'Ross', 'man', 'Stallone.jpg','universal',1),
('Lee', 'Christmas', 'man', 'Statham.jpg', 'personal',2),
('Gunnar', 'Jensen', 'man', 'Lundgren.jpg','universal',3),
('Terry', 'Caesar', 'man', 'Crews.jpg', 'group',4),
('Jean', 'Vilain', 'man', 'Damme.jpg', 'universal',4),
('Natalia', 'Romanoff', 'woman', 'Natasha.jpg','universal',1),
('Wanda', 'Maximoff', 'woman', 'Wanda.jpg', 'group',2),
('Gamora', 'Gamorak', 'woman', 'Gamora.jpg','universal',3),
('Boxis', 'Strong', 'woman', 'Boxis.jpg','group',1),
('Bruce', 'Lee', 'man', 'Bruce.jpg','group',4),
('Marvel', 'Levram', 'woman', 'Marvel.jpg','personal',2),
('Sonya', 'Night', 'woman', 'Sonya.jpg','personal',3),
('Supwom', 'Nanual', 'woman', 'Supwom.jpg','personal',1),
('Tor', 'Asgaard', 'man', 'Tor.jpg','personal',2),
('Witcher', 'Moon', 'man', 'Witcher.jpg','personal',3)
go



insert into GroupClasses (Name, Description)
values
('Yoga', 'Concentrated movements to promote flexibility, tone and strengthen muscles, and align the body'),
('Pilates', 'Heavy elements of core focus, with repetitive and small movements of isolated or full body muscle groups'),
('HIIT', 'High-Intensity Interval Training - alternates short periods of intense exercise movements, followed by less intense “recovery” periods'),
('Water Aerobics', 'Engaging muscle endurance and strength in a low-impact setting'),
('Cycling', 'Cardio workout that relies on a fitness center cycling machine'),
('Zumba', 'Series of energetic dance routines by mixing low intensity and high intensity moves'),
('Kickboxing', 'Great cardiovascular workout, helps build endurance, coordination, tones muscles and core')
('Personal training','Training under the guidance of a trainer')
go



insert into ClassTrainer (ClassId, TrainerId)
values
(1,1),
(3,6),
(4,1),
(7,6),
(1,7),
(2,7),
(6,7),
(5,7),
(2,3),
(4,8),
(5,3),
(6,8),
(5,4),
(1,4),
(7,5),
(7,9),
(7,10),
(8,1),
(8,2),
(8,3),
(8,5),
(8,6),
(8,8),
(8,11),
(8,12),
(8,13),
(8,14),
(8,15)
go


insert into SubscriptionTypes (LifePeriod, GroupClassInclude, DietMonitoring, GymId, Price)
values 
('One time', 0, 0, 1, 7),
('One time', 1, 0, 1, 9),
('Month', 0, 0, 1, 50),
('Month', 1, 0, 1, 60),
('Month', 1, 1, 1, 65),

('One time', 0, 0, 2, 8),
('One time', 1, 0, 2, 10),
('Month', 0, 0, 2, 55),
('Month', 1, 0, 2, 65),
('Month', 1, 1, 2, 70),

('One time', 0, 0, 3, 7),
('One time', 1, 0, 3, 9),
('Month', 0, 0, 3, 53),
('Month', 1, 0, 3, 62),
('Month', 1, 1, 3, 66),

('One time', 0, 0, 4, 6),
('One time', 1, 0, 4, 8),
('Month', 0, 0, 4, 48),
('Month', 1, 0, 4, 52),
('Month', 1, 1, 4, 60),

('Month', 0, 1, 1, 55),
('Month', 0, 1, 2, 58),
('Month', 0, 1, 3, 60),
('Month', 0, 1, 4, 53)
go