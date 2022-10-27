use FitMeDB
go


insert into Gyms (Name, Address, Phone)
values 
('BigRock', '25 MainStreet, 378-94, Gdansk', '(048)586-48-58' ),
('GoldSection', '124 RedAvenue, 391-31, Gdansk', '(048)789-53-26' ),
('Caliostro', '8 BakerStreet, 380-92, Gdansk', '(048)185-03-74' ),
('FitProfit', '97 BlueStreet, 379-45, Gdansk', '(048)358-09-08' )
go

insert into Trainers (FirstName, LastName, Gender, Picture, Specialization)
values
('Barney', 'Ross', 'man', 'Stallone.jpg','universal'),
('Lee', 'Christmas', 'man', 'Statham.jpg', 'personal'),
('Gunnar', 'Jensen', 'man', 'Lundgren.jpg','universal'),
('Terry', 'Caesar', 'man', 'Crews.jpg', 'group'),
('Jean', 'Vilain', 'man', 'Damme.jpg', 'universal'),
('Natalia', 'Romanoff', 'woman', 'Natasha.jpg','universal'),
('Wanda', 'Maximoff', 'woman', 'Wanda.jpg', 'group'),
('Gamora', 'Gamorak', 'woman', 'Gamora.jpg','universal')
go


insert into TrainerGym (TrainerId, GymId)
values
(1,1),
(1,3),
(2,1),
(3,2),
(3,4),
(4,4),
(5,1),
(5,2),
(5,4),
(6,3),
(6,4),
(7,1),
(7,2),
(8,2),
(8,3)
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
go



insert into GroupClassGym (GroupClassId,GymId, TrainerId)
values
(3,2,7),
(3,3,8),
(4,1,7),
(4,2,7),
(4,3,8),
(5,3,6),
(6,4,4),
(7,4,4),
(1,1,5),
(1,2,5),
(2,1,3),
(2,1,1)
go