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
		   ('2022-12-02','210','300','acc51bef-12d2-4933-ba98-646927663579','7122f67c-d670-47d4-8afb-0fd14b76cec4','1','0'),
		   ('2022-12-01','150','210','7d1b0307-a5f2-494e-935c-828bdf39afe2','7122f67c-d670-47d4-8afb-0fd14b76cec4','7','0'),
		   ('2022-12-01','300','360','7d1b0307-a5f2-494e-935c-828bdf39afe2','7122f67c-d670-47d4-8afb-0fd14b76cec4','7','0')
GO


insert GymWorkHours (DayOfWeekNumber, GymId,StartTime,EndTime)
values 
(0,	1, 540,	1200)
,(0, 2, 600, 1320)
,(0, 3,	600, 1260)
,(0, 4,	480, 1260)
,(1, 1,	480, 1320)
,(1, 2,	480, 1380)
,(1, 3,	420, 1320)
,(1, 4,	360, 1260)
,(2, 1,	480, 1320)
,(2, 2,	480, 1380)
,(2, 3,	420, 1320)
,(2, 4,	360, 1260)
,(3, 1,	480, 1320)
,(3, 2,	480, 1380)
,(3, 3,	420, 1320)
,(3, 4,	360, 1260)
,(4, 1,	480, 1320)
,(4, 2,	480, 1380)
,(4, 3,	420, 1320)
,(4, 4,	360, 1260)
,(5, 1,	480, 1320)
,(5, 2,	480, 1380)
,(5, 3,	420, 1320)
,(5, 4,	360, 1260)
,(6, 1,	480, 1200)
,(6, 2,	540, 1320)
,(6, 3,	540, 1260)
,(6, 4,	600, 1320)
go


insert TrainerWorkHours (TrainerId, StartTime,EndTime,GymWorkHoursId)
values
('acc51bef-12d2-4933-ba98-646927663579', 540, 960,	1)
,('acc51bef-12d2-4933-ba98-646927663579', 480, 900,	5)
,('acc51bef-12d2-4933-ba98-646927663579', 480, 900,	9)
,('acc51bef-12d2-4933-ba98-646927663579', 840, 1260, 13)
,('acc51bef-12d2-4933-ba98-646927663579', 840, 1260, 17)
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 540, 900,	1)
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 480, 1020, 5)
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 480, 1020, 9)
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 540, 900,	17)
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 540, 900,	21)
,('5f91c6cf-7708-4b35-a070-99ed79752302', 480, 960,	5)
,('5f91c6cf-7708-4b35-a070-99ed79752302', 480, 960,	9)
,('5f91c6cf-7708-4b35-a070-99ed79752302', 480, 960,	13)
,('5f91c6cf-7708-4b35-a070-99ed79752302', 480, 960,	17)
,('5f91c6cf-7708-4b35-a070-99ed79752302', 480, 960,	21)
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 900, 1320, 5)
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 900, 1320, 9)
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 900, 1320, 13)
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 900, 1320, 17)
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 900, 1320, 21)
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 600, 900, 2)
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 480, 960, 6)
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 480,	960, 10)
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 960,	1380, 14)
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 960,	1380, 18)
,('0a329324-af6f-4afa-ad33-384a9f3af275', 600,	900, 2)
,('0a329324-af6f-4afa-ad33-384a9f3af275', 480,	960, 6)
,('0a329324-af6f-4afa-ad33-384a9f3af275', 480,	960, 10)
,('0a329324-af6f-4afa-ad33-384a9f3af275', 960,	1380, 14)
,('0a329324-af6f-4afa-ad33-384a9f3af275', 960,	1380, 18)
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 840,	1320, 6)
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 840,	1320, 10)
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 840,	1320, 14)
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 840,	1320, 18)
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 840,	1260, 22)
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 900,	1260, 6)
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 900,	1260, 10)
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 900,	1260, 14)
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 480,	960, 22)
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 540,	960, 26)
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 600,	960, 3)
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 420,	900, 7)
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 420,	900, 11)
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 840,	1260, 15)
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 840,	1260, 19)
,('83192926-4d5b-4a27-93f1-a2958c69f069', 600,	900, 3)
,('83192926-4d5b-4a27-93f1-a2958c69f069', 480,	1020, 7)
,('83192926-4d5b-4a27-93f1-a2958c69f069', 480,	1020, 11)
,('83192926-4d5b-4a27-93f1-a2958c69f069', 540,	900, 19)
,('83192926-4d5b-4a27-93f1-a2958c69f069', 540,	900, 23)
,('f6b480ef-aa5c-467f-8382-5199a395d585', 840,	1320, 7)
,('f6b480ef-aa5c-467f-8382-5199a395d585', 840,	1320, 11)
,('f6b480ef-aa5c-467f-8382-5199a395d585', 840,	1320, 15)
,('f6b480ef-aa5c-467f-8382-5199a395d585', 840,	1320, 19)
,('f6b480ef-aa5c-467f-8382-5199a395d585', 840, 1200, 23)
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 420,	900, 7)
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 420,	960, 11)
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 420,	960, 15)
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 420,	900, 19)
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 420,	900, 23)
,('08b2b361-1115-44db-97fa-ce9cd34366d7', 480,	840, 4)
,('08b2b361-1115-44db-97fa-ce9cd34366d7', 420,	1020, 8)
,('08b2b361-1115-44db-97fa-ce9cd34366d7', 420,	1020, 12)
,('08b2b361-1115-44db-97fa-ce9cd34366d7', 420,	900, 20)
,('08b2b361-1115-44db-97fa-ce9cd34366d7', 420,	900, 24)
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 480,	960, 8)
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 480,	960, 12)
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 480,	960, 16)
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 480,	960, 20)
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 480,	960, 24)
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 600,	960, 28)
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 900,	1260, 8)
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 900,	1260, 12)
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 900,	1260, 16)
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 480,	960, 20)
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 540,	960, 24)
go



  insert UserSubscriptions (UserId, GymSubscriptionId, TrainerId, StartDate, EndDate)
  values('189cb89d-f5c1-4dc8-9a1b-67715f6b6fcf', '47','7d1b0307-a5f2-494e-935c-828bdf39afe2', '2022-12-01', '2022-12-31')
  ,('871dd4aa-19dd-4749-86ec-c2d902c290e3', '48','7d1b0307-a5f2-494e-935c-828bdf39afe2', '2022-12-10', '2023-01-09')
  ,('afb2a4c0-7ff4-400c-98c1-448908b39e46', '50','7d1b0307-a5f2-494e-935c-828bdf39afe2', '2022-12-15', '2023-01-14')
  go
