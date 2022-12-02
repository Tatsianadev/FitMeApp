use FitMeDB
go


insert into Gyms (Name, Address, Phone)
values 
('BigRock', '25 MainStreet, 378-94, Gdansk', '(048)586-48-58' ),
('GoldSection', '124 RedAvenue, 391-31, Gdansk', '(048)789-53-26' ),
('Caliostro', '8 BakerStreet, 380-92, Gdansk', '(048)185-03-74' ),
('FitProfit', '97 BlueStreet, 379-45, Gdansk', '(048)358-09-08' )
go

insert into Trainers (Id, FirstName, LastName, Gender, Picture, Specialization, GymId)
values
('acc51bef-12d2-4933-ba98-646927663579', 'Barney', 'Ross', 'man', 'Stallone.jpg','universal',1),
('d51f7f69-c30f-4842-8a61-b4a9890f00ed','Lee', 'Christmas', 'man', 'Statham.jpg', 'personal',2),
('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 'Gunnar', 'Jensen', 'man', 'Lundgren.jpg','universal',3),
('08b2b361-1115-44db-97fa-ce9cd34366d7', 'Terry', 'Caesar', 'man', 'Crews.jpg', 'group',4),
('e985d062-69f4-40fd-aa35-ff35dc1ca911', 'Jean', 'Vilain', 'man', 'Damme.jpg', 'universal',4),
('f90f2e2c-821e-4651-91d7-105d56ddbee7', 'Natalia', 'Romanoff', 'woman', 'Natasha.jpg','universal',1),
('0a329324-af6f-4afa-ad33-384a9f3af275', 'Wanda', 'Maximoff', 'woman', 'Wanda.jpg', 'group',2),
('83192926-4d5b-4a27-93f1-a2958c69f069', 'Gamora', 'Gamorak', 'woman', 'Gamora.jpg','universal',3),
('5f91c6cf-7708-4b35-a070-99ed79752302', 'Boxis', 'Strong', 'woman', 'Boxis.jpg','group',1),
('7d1b0307-a5f2-494e-935c-828bdf39afe2', 'Bruce', 'Lee', 'man', 'Bruce.jpg','group',4),
('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'Marvel', 'Levram', 'woman', 'Marvel.jpg','personal',2),
('f6b480ef-aa5c-467f-8382-5199a395d585', 'Sonya', 'Night', 'woman', 'Sonya.jpg','personal',3),
('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 'Supwom', 'Nanual', 'woman', 'Supwom.jpg','personal',1),
('69e23278-6b00-4f40-a555-02ef12ffd09b', 'Tor', 'Asgaard', 'man', 'Tor.jpg','personal',2),
('85ffe910-0a85-40ba-82fa-192d445ab5c5', 'Witcher', 'Moon', 'man', 'Witcher.jpg','personal',3)
go


insert into TrainingTrainer (TrainingId, TrainerId)
values
(1,'acc51bef-12d2-4933-ba98-646927663579'),
(3,'f90f2e2c-821e-4651-91d7-105d56ddbee7'),
(4,'acc51bef-12d2-4933-ba98-646927663579'),
(7,'f90f2e2c-821e-4651-91d7-105d56ddbee7'),
(1,'0a329324-af6f-4afa-ad33-384a9f3af275'),
(2,'0a329324-af6f-4afa-ad33-384a9f3af275'),
(6,'0a329324-af6f-4afa-ad33-384a9f3af275'),
(5,'0a329324-af6f-4afa-ad33-384a9f3af275'),
(2,'763a257a-7b4b-45d5-a5ac-caa75c51bae9'),
(4,'83192926-4d5b-4a27-93f1-a2958c69f069'),
(5,'763a257a-7b4b-45d5-a5ac-caa75c51bae9'),
(6,'83192926-4d5b-4a27-93f1-a2958c69f069'),
(5,'08b2b361-1115-44db-97fa-ce9cd34366d7'),
(1,'08b2b361-1115-44db-97fa-ce9cd34366d7'),
(7,'e985d062-69f4-40fd-aa35-ff35dc1ca911'),
(7,'5f91c6cf-7708-4b35-a070-99ed79752302'),
(7,'7d1b0307-a5f2-494e-935c-828bdf39afe2'),
(8,'acc51bef-12d2-4933-ba98-646927663579'),
(8,'d51f7f69-c30f-4842-8a61-b4a9890f00ed'),
(8,'763a257a-7b4b-45d5-a5ac-caa75c51bae9'),
(8,'e985d062-69f4-40fd-aa35-ff35dc1ca911'),
(8,'f90f2e2c-821e-4651-91d7-105d56ddbee7'),
(8,'83192926-4d5b-4a27-93f1-a2958c69f069'),
(8,'e26bcd6e-49bd-42d6-acca-905ba29cddbc'),
(8,'f6b480ef-aa5c-467f-8382-5199a395d585'),
(8,'8d4176f7-f8f4-49fb-ada1-6134f58e8ff1'),
(8,'69e23278-6b00-4f40-a555-02ef12ffd09b'),
(8,'85ffe910-0a85-40ba-82fa-192d445ab5c5')
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





insert into Subscriptions (ValidDays, GroupTraining, DietMonitoring)
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



INSERT INTO [dbo].[Events]
           ([Date]
           ,[StartTime]
           ,[EndTime]
           ,[TrainerId]
           ,[UserId]
           ,[TrainingId]
           ,[Status])
     VALUES
           ('2022-11-29','210','270','08b2b361-1115-44db-97fa-ce9cd34366d7','7122f67c-d670-47d4-8afb-0fd14b76cec4','5','1'),
		   ('2022-11-29','360','450','0a329324-af6f-4afa-ad33-384a9f3af275','7122f67c-d670-47d4-8afb-0fd14b76cec4','6','1'),
		   ('2022-12-02','150','210','7d1b0307-a5f2-494e-935c-828bdf39afe2','7122f67c-d670-47d4-8afb-0fd14b76cec4','7','0'),
		   ('2022-12-02','300','360','7d1b0307-a5f2-494e-935c-828bdf39afe2','7122f67c-d670-47d4-8afb-0fd14b76cec4','7','0'),
		   ('2022-12-02','510','570','acc51bef-12d2-4933-ba98-646927663579','7122f67c-d670-47d4-8afb-0fd14b76cec4','8','0'),
		   ('2022-12-02','210','300','acc51bef-12d2-4933-ba98-646927663579','7122f67c-d670-47d4-8afb-0fd14b76cec4','1','0')
GO


insert GymWorkHours (DayOfWeekNumber, GymId,StartTime,EndTime)
values ('0','1','270','630')
,('0','2','240','630')
,('0','3','300','660')
,('0','4','210','600')
go