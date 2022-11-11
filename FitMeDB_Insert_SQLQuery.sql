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



insert into Trainings (Name, Description)
values
('Yoga', 'Concentrated movements to promote flexibility, tone and strengthen muscles, and align the body'),
('Pilates', 'Heavy elements of core focus, with repetitive and small movements of isolated or full body muscle groups'),
('HIIT', 'High-Intensity Interval Training - alternates short periods of intense exercise movements, followed by less intense “recovery” periods'),
('Water Aerobics', 'Engaging muscle endurance and strength in a low-impact setting'),
('Cycling', 'Cardio workout that relies on a fitness center cycling machine'),
('Zumba', 'Series of energetic dance routines by mixing low intensity and high intensity moves'),
('Kickboxing', 'Great cardiovascular workout, helps build endurance, coordination, tones muscles and core'),
('Personal training','Training under the guidance of a trainer')
go



insert into TrainingTrainer (TrainingId, TrainerId)
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


insert into Subscriptions (ValidDays, GroupTrainingInclude, DietMonitoring)
values 
(1,0,0)
,(1,1,0)
,(1,0,1)
,(1,1,1)
,(30,0,0)
,(30,1,0)
,(30,0,1)
,(30,1,1)
,(183,0,0)
,(183,1,0)
,(183,0,1)
,(183,1,1)
,(256,0,0)
,(256,1,0)
,(256,0,1)
,(256,1,1)
go

insert into GymSubscriptions (GymId,SubscriptionId,Price)
values
(1,1,5)
,(1,2,6)
,(1,3,6)
,(1,4,7)
,(1,5,13)
,(1,6,15)
,(1,7,15)
,(1,8,17)
,(1,9,50)
,(1,10,52)
,(1,11,52)
,(1,12,57)
,(1,13,85)
,(1,14,90)
,(1,15,90)
,(1,16,100)

,(2,1,5)
,(2,2,6)
,(2,3,6)
,(2,4,7)
,(2,5,14)
,(2,6,16)
,(2,7,16)
,(2,8,17)
,(2,9,53)
,(2,10,57)
,(2,11,57)
,(2,12,60)
,(2,13,88)
,(2,14,94)
,(2,15,94)
,(2,16,105)

,(3,1,4)
,(3,2,5)
,(3,3,5)
,(3,4,6)
,(3,5,12)
,(3,6,14)
,(3,7,14)
,(3,8,15)
,(3,9,48)
,(3,10,52)
,(3,11,50)
,(3,12,55)

,(4,1,6)
,(4,2,7)
,(4,5,12)
,(4,6,15)
,(4,7,15)
,(4,8,18)
,(4,9,55)
,(4,10,60)
,(4,11,57)
,(4,12,65)
,(4,13,90)
,(4,14,100)
,(4,15,97)
,(4,16,110)
go