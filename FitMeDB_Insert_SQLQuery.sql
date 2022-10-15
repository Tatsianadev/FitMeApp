use FitMeDB
go


insert into Gyms (Name, Address, Phone)
values 
('BigRock', '25 MainStreet, 378-94, Gdansk', '(048)586-48-58' ),
('GoldSection', '124 RedAvenue, 391-31, Gdansk', '(048)789-53-26' ),
('Caliostro', '8 BakerStreet, 380-92, Gdansk', '(048)185-03-74' ),
('FitProfit', '97 BlueStreet, 379-45, Gdansk', '(048)358-09-08' )
go

insert into Trainers (FirstName, LastName, Gender, Picture)
values
('Barney', 'Ross', 'man', 'Stallone.jpg'),
('Lee', 'Christmas', 'man', 'Statham.jpg'),
('Gunnar', 'Jensen', 'man', 'Lundgren.jpg'),
('Terry', 'Caesar', 'man', 'Crews.jpg'),
('Jean', 'Vilain', 'man', 'Damme.jpg'),
('Natalia', 'Romanoff', 'woman', 'Natasha.jpg'),
('Wanda', 'Maximoff', 'woman', 'Wanda.jpg'),
('Gamora', 'Gamorak', 'woman', 'Camora.jpg')
go


insert into TrainerGym (TrainerId, GymId)
values
(1,5),
(1,3),
(2,5),
(3,2),
(3,4),
(4,4),
(5,5),
(5,2),
(5,4),
(6,3),
(6,4),
(7,5),
(7,2),
(8,2),
(8,3)
go


insert into GroupTrainings (Name, Description, TrainerId, GymId)
values
('Yoga', 'Concentrated movements to promote flexibility, tone and strengthen muscles, and align the body', 7, 2),
('Pilates', 'Heavy elements of core focus, with repetitive and small movements of isolated or full body muscle groups', 7,5),
('HIIT', 'High-Intensity Interval Training - alternates short periods of intense exercise movements, followed by less intense “recovery” periods', 6,3),
('Water Aerobics', 'Engaging muscle endurance and strength in a low-impact setting', 4, 4),
('Cycling', 'Cardio workout that relies on a fitness center cycling machine', 4, 4),
('Zumba', 'Series of energetic dance routines by mixing low intensity and high intensity moves', 5,5),
('Kickboxing', 'Great cardiovascular workout, helps build endurance, coordination, tones muscles and core', 3, 2)
go