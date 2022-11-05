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
('Gamora', 'Gamorak', 'woman', 'Gamora.jpg','universal',3)
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



insert into GroupClassGymTrainer (GroupClassId,GymId, TrainerId)
values
(1,1,1),
(3,1,6),
(4,1,1),
(7,1,6),
(1,2,7),
(2,2,7),
(6,2,7),
(5,2,7),
(2,3,3),
(4,3,8),
(5,3,3),
(6,3,8),
(5,4,4),
(1,4,4),
(7,4,5)
go