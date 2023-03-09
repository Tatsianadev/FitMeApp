use FitMeDB
go


insert into Gyms (Name, Address, Phone)
values 
('BigRock', '25 MainStreet, 378-94, Gdansk', '(048)586-48-58' ),
('GoldSection', '124 RedAvenue, 391-31, Gdansk', '(048)789-53-26' ),
('Caliostro', '8 BakerStreet, 380-92, Gdansk', '(048)185-03-74' ),
('FitProfit', '97 BlueStreet, 379-45, Gdansk', '(048)358-09-08' )
go


insert TrainerWorkLicenses (TrainerId,SubscriptionId,ContractNumber,GymId,StartDate,EndDate, ConfirmationDate)
  values  ('08b2b361-1115-44db-97fa-ce9cd34366d7',0, '123456_3', 4, '2023-03-07', '2024-03-07', '2023-03-07')
  ,('0a329324-af6f-4afa-ad33-384a9f3af275',0, '123456_4', 2, '2023-02-11', '2024-03-07', '2023-02-13')
  ,('5f91c6cf-7708-4b35-a070-99ed79752302',0, '123456_5', 1, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('69e23278-6b00-4f40-a555-02ef12ffd09b',0, '123456_6', 2, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('763a257a-7b4b-45d5-a5ac-caa75c51bae9',0, '123456_7', 3, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('7c3689b7-2aea-4ed1-990f-9622954d575a',0, '123456_8', 2, '2023-02-11', '2024-03-07', '2023-02-13')
  ,('7d1b0307-a5f2-494e-935c-828bdf39afe2',0, '123456_9', 4, '2023-02-11', '2024-03-07', '2023-02-13')
  ,('83192926-4d5b-4a27-93f1-a2958c69f069',0, '123456_10', 3, '2023-02-11', '2024-03-07', '2023-02-13')
  ,('85ffe910-0a85-40ba-82fa-192d445ab5c5',0, '123456_11', 3, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1',0, '123456_12', 1, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('acc51bef-12d2-4933-ba98-646927663579',0, '123456_13', 1, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('d51f7f69-c30f-4842-8a61-b4a9890f00ed',0, '123456_14', 2, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('e26bcd6e-49bd-42d6-acca-905ba29cddbc',0, '123456_15', 2, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('e985d062-69f4-40fd-aa35-ff35dc1ca911',0, '123456_16', 4, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('f6b480ef-aa5c-467f-8382-5199a395d585',0, '123456_17', 4, '2022-11-18', '2023-11-18', '2022-11-20')
  ,('f90f2e2c-821e-4651-91d7-105d56ddbee7',0, '123456_18', 1, '2022-11-18', '2023-11-18', '2022-11-20')
  go


--(Fill WorkLicenseId from TrainerWorkLicenses table (Id))
insert into Trainers (Id, Specialization, GymId, WorkLicenseId)
values
('acc51bef-12d2-4933-ba98-646927663579', 'universal',1,0),
('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 'personal',2,0),
('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 'universal',3,0),
('08b2b361-1115-44db-97fa-ce9cd34366d7', 'group',4,0),
('e985d062-69f4-40fd-aa35-ff35dc1ca911', 'universal',4,0),
('f90f2e2c-821e-4651-91d7-105d56ddbee7', 'universal',1,0),
('0a329324-af6f-4afa-ad33-384a9f3af275', 'group',2,0),
('83192926-4d5b-4a27-93f1-a2958c69f069', 'universal',3,0),
('5f91c6cf-7708-4b35-a070-99ed79752302', 'group',1,0),
('7d1b0307-a5f2-494e-935c-828bdf39afe2', 'group',4,0),
('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'personal',2,0),
('f6b480ef-aa5c-467f-8382-5199a395d585', 'personal',3,0),
('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 'personal',1,0),
('69e23278-6b00-4f40-a555-02ef12ffd09b', 'personal',2,0),
('85ffe910-0a85-40ba-82fa-192d445ab5c5', 'personal',3,0)
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




insert into Subscriptions (ValidDays, GroupTraining, DietMonitoring, WorkAsTrainer)
values 
(1,0,0,0)
,(1,1,0,0)
,(1,0,1,0)
,(1,1,1,0)
,(30,0,0,0)
,(30,1,0,0)
,(30,0,1,0)
,(30,1,1,0)
,(183,0,0,0)
,(183,1,0,0)
,(183,0,1,0)
,(183,1,1,0)
,(256,0,0,0)
,(256,1,0,0)
,(256,0,1,0)
,(256,1,1,0)
,(30,1,1,1)
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
,(1,17,100)

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
,(2,17,110)

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
,(3,17,115)

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
,(4,17,120)
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
		   ('2022-12-01','300','360','7d1b0307-a5f2-494e-935c-828bdf39afe2','7122f67c-d670-47d4-8afb-0fd14b76cec4','7','0'),
		   ('2023-02-07','900','960','f6b480ef-aa5c-467f-8382-5199a395d585','7122f67c-d670-47d4-8afb-0fd14b76cec4','8','1'),
		    ('2023-02-07','990','1050','f6b480ef-aa5c-467f-8382-5199a395d585','7122f67c-d670-47d4-8afb-0fd14b76cec4','8','0'),
			 ('2023-02-09','1110','1170','f6b480ef-aa5c-467f-8382-5199a395d585','7122f67c-d670-47d4-8afb-0fd14b76cec4','8','1')
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



  insert UserSubscriptions (UserId, GymSubscriptionId, StartDate, EndDate)
  values('189cb89d-f5c1-4dc8-9a1b-67715f6b6fcf', '47', '2022-12-01', '2022-12-31')
  ,('871dd4aa-19dd-4749-86ec-c2d902c290e3', '48', '2022-12-10', '2023-01-09')
  ,('afb2a4c0-7ff4-400c-98c1-448908b39e46', '50', '2022-12-15', '2023-01-14')
  go



insert AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
values ('410c528f-e262-404c-80bf-ab4fb5014cc5', 'user', 'USER', 'ae7eb6ef-9961-49a8-8414-dc9ba8e7b5e9')
, ('aa25d694-f274-49b9-8ddc-ab40b1b9044d', 'blocked', 'BLOCKED', 'a2b7170e-25d8-42f6-bd79-8dd22a547757')
,('b5cb8169-ea31-4f9a-becd-df56b1d53756', 'trainer', 'TRAINER', '76bd3fda-2511-47fc-8eca-104e667f1002')
,('c1073fb7-a6de-4530-a285-68bfc4652624', 'admin', 'ADMIN', '23e5da34-2b66-49ed-a8a7-e2af8d9a11f1')
go



insert AspNetUserRoles (UserId, RoleId)
values ('08b2b361-1115-44db-97fa-ce9cd34366d7', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('0a329324-af6f-4afa-ad33-384a9f3af275', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('5f91c6cf-7708-4b35-a070-99ed79752302', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('83192926-4d5b-4a27-93f1-a2958c69f069', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('acc51bef-12d2-4933-ba98-646927663579', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('f6b480ef-aa5c-467f-8382-5199a395d585', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 'b5cb8169-ea31-4f9a-becd-df56b1d53756')
,('7122f67c-d670-47d4-8afb-0fd14b76cec4', 'c1073fb7-a6de-4530-a285-68bfc4652624')
go



insert AspNetUsers(Id, Year,Gender,UserName,NormalizedUserName, Email, NormalizedEmail, 
EmailConfirmed, PasswordHash,SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed,
TwoFactorEnabled, LockoutEnd, LockoutEnabled,AccessFailedCount, FirstName,LastName,AvatarPath)
values ('08b2b361-1115-44db-97fa-ce9cd34366d7', 1983, 'male', 'terry@gmail.com', 'TERRY@GMAIL.COM', 
'terry@gmail.com', 'TERRY@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEA+yeMUHnLSvqEPa/2tW4OCpKz/NRnrpKncstTkv6eqM6E7Za0RG/xJ26dq8zxcbng==',
'HVVHMKWCU4EVWODRXS2ZDSFBL3UDGQDL', 'c629f12c-6cd7-43f5-b814-edcfdb5ae297', '(111)222666', 0, 0, NULL, 1, 0,
'Terry', 'Caesar', '/Content/Upload/08b2b361-1115-44db-97fa-ce9cd34366d7/AvatarPath/47116290.jpg')
,('0a329324-af6f-4afa-ad33-384a9f3af275', 1990, 'female', 'wanda@gmail.com', 'WANDA@GMAIL.COM', 
'wanda@gmail.com', 'WANDA@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEOI2k0sg9OcZH+54JmQpea4cYU6vbC0J089VLTcLRrTlMs5+mhZQpqDIo5nu4jJg5A==',
'4Q5MKZBSJ5H3KUQYYQMPJ4NRMCD2TW6L', '2bfd0876-ebd0-412c-9df9-6d56cc445681', '(111)222999', 0, 0, NULL, 1, 0,
'Wanda', 'Maximoff', '/Content/Upload/0a329324-af6f-4afa-ad33-384a9f3af275/AvatarPath/15214905.jpg')
,('5f91c6cf-7708-4b35-a070-99ed79752302', 1993, 'female', 'boxis@gmail.com', 'BOXIS@GMAIL.COM', 
'boxis@gmail.com', 'BOXIS@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEImDrRKtvqv8ONnEXW3w9c+JzNtCCYv0TFIkK7oHTTITkfb+DQk1BheZQBbfAzAGLg==',
'7LDCRMFQ7RCE7RZ2G3QYRD4PN4HCIYRH', 'ff1404e5-c937-46f3-a9d4-443b490b8526', '(111)333222', 0, 0, NULL, 1, 0,
'Boxis', 'Strong', '/Content/Upload/5f91c6cf-7708-4b35-a070-99ed79752302/AvatarPath/15815369.jpg')
,('69e23278-6b00-4f40-a555-02ef12ffd09b', 1994, 'male', 'tor@gmail.com', 'BTOR@GMAIL.COM', 
'tor@gmail.com', 'TOR@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEIvzr/RaN3KoaY8f57Qp8OAHbz0WOjK5VNz3EB63YJEzm96H5oTulT23imns/W7W8A==',
'2Z7PE4EXQ7KXCNXWNGOIQJTFXZSUN2ZI', '9367fa06-dbad-4a1b-bd38-ce9b606d30a2', '(111)333888', 0, 0, NULL, 1, 0,
'Tor', 'Asgaard', '/Content/Upload/69e23278-6b00-4f40-a555-02ef12ffd09b/AvatarPath/29393465.jpg')
,('763a257a-7b4b-45d5-a5ac-caa75c51bae9', 1980, 'male', 'gunnar@gmail.com', 'GUNNAR@GMAIL.COM', 
'gunnar@gmail.com', 'GUNNAR@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEMRuB4NuCPgKod1N4JVwD1Bz2DnSx3CI5TfCZdoL6Nhzc3NZpTyyUBam8ky6oc4iQA==',
'CKAZB2ZRTXRYRX7ZXXFLSWYSV4R7RCTX', 'c4cf0542-9d1b-4ba0-92b7-af7b68d250f8', '(111)222444', 0, 0, NULL, 1, 0,
'Gunnar', 'Jensen', '/Content/Upload/763a257a-7b4b-45d5-a5ac-caa75c51bae9/AvatarPath/46110404.jpg')
,('7d1b0307-a5f2-494e-935c-828bdf39afe2', 1987, 'male', 'bruce@gmail.com', 'BRUCE@GMAIL.COM', 
'bruce@gmail.com', 'BRUCE@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEPRU3W8JisO0dbCDkfR9RyQn/ol2mhpiBj1+30nsniCxaa3BeGB5ufPv2XWo0x9G/w==',
'RIL42QCPUX7P753ATWIKH36FSRN3X6BE', '07771852-170e-499a-a07c-9f7f1091bdc5', '(111)333445', 0, 0, NULL, 1, 0,
'Bruce', 'Lee', '/Content/Upload/7d1b0307-a5f2-494e-935c-828bdf39afe2/AvatarPath/66430989.jpg')
,('83192926-4d5b-4a27-93f1-a2958c69f069', 1988, 'female', 'gamora@gmail.com', 'GAMORA@GMAIL.COM', 
'gamora@gmail.com', 'GAMORA@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEPg3WsSBXu3hC48dwZGMhu0tV//9HTWOXb67xNyoDze9S10K9mVhQ7GLquub01C+uA==',
'NRNGUOBZSCCO4YV2RHDO7JUSMCL7PQI7', 'a8db310a-39e1-48dd-aa9f-fe08a117c4c8', '(111)333111', 0, 0, NULL, 1, 0,
'Gamora', 'Gamorak', '/Content/Upload/83192926-4d5b-4a27-93f1-a2958c69f069/AvatarPath/31061944.jpg')
,('85ffe910-0a85-40ba-82fa-192d445ab5c5', 1990, 'male', 'witcher@gmail.com', 'WITCHER@GMAIL.COM', 
'witcher@gmail.com', 'WITCHER@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEJOKnEzFOES/m3EhJ6Y4j61Bn/KI22WdzUtA0kO8t8rLkpQFw5bZwbz4N2LiwNh7VA==',
'6RFV2PXZBDAXYAWHPLIFQNH3EVK2ZMRA', '096988b2-23de-4cff-a385-7dc2c202d92f', '(111)333999', 0, 0, NULL, 1, 0,
'Witcher', 'Moon', '/Content/Upload/85ffe910-0a85-40ba-82fa-192d445ab5c5/AvatarPath/45475037.jpg')
,('8d4176f7-f8f4-49fb-ada1-6134f58e8ff1', 1988, 'female', 'supwom@gmail.com', 'SUPWOM@GMAIL.COM', 
'supwom@gmail.com', 'SUPWOM@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEBaxJxjmio9Etn9Viiyf63GNqrp1NCdSaEAoDaJsF5LEYXwcLNgSrCRuvmStysXo8Q==',
'IEKZGTTWCKP45GVC42DVSROHMTRLX7CS', '348cba9d-9a4a-4368-a1d6-ea54d4c5df4c', '(111)333777', 0, 0, NULL, 1, 0,
'Supwom', 'Nanual', '/Content/Upload/8d4176f7-f8f4-49fb-ada1-6134f58e8ff1/AvatarPath/11450641.jpg')
,('acc51bef-12d2-4933-ba98-646927663579', 1975, 'male', 'barney@gmail.com', 'BARNEY@GMAIL.COM', 
'barney@gmail.com', 'BARNEY@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEBBH0aE0iUVjQxZ8y9RMIq+9/AOL6v602ZJSRV6/EREO6sGyw/ckM8+9hnMN5XHpTg==',
'HJBCZRJMBH2ZKXERMQ37BBZ6FHWD4AHH', '8477de83-3ae5-4917-8a4a-91d2b0978aee', '(111)222333', 0, 0, NULL, 1, 0,
'Barney', 'Ross', '/Content/Upload/acc51bef-12d2-4933-ba98-646927663579/AvatarPath/28308356.jpg')
,('d51f7f69-c30f-4842-8a61-b4a9890f00ed', 1977, 'male', 'lee@gmail.com', 'LEE@GMAIL.COM', 
'lee@gmail.com', 'LEE@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEGXMLKxzT5tFWwpdr2oKz3Z6W9+Cm2mhXypeH0ftnF6TuD5nCYEQtX/tgzZw7bNN0A==',
'FCRGP2MUZW6KEAS57OLGFQYMCHPOCDPO', '20d97599-c9da-4eb6-9596-a47702a74089', '(111)333444', 0, 0, NULL, 1, 0,
'Lee', 'Christmas', '/Content/Upload/d51f7f69-c30f-4842-8a61-b4a9890f00ed/AvatarPath/30839641.jpg')
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 1995, 'female', 'marvel@gmail.com', 'MARVEL@GMAIL.COM', 
'marvel@gmail.com', 'MARVEL@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEGJXXBGlmpnb8KXC0XF7EkegUA5ZA5vM0gkdBrjHDceSFicGA1R+ng8a5IsNb6VTTA==',
'5GOJUFPU6CMUKLV5X74GXNVA2AVZKVPG', '0613c5b1-2c47-4cbc-a113-bdef69c76da4', '(111)333555', 0, 0, NULL, 1, 0,
'Marvel', 'Levram', '/Content/Upload/e26bcd6e-49bd-42d6-acca-905ba29cddbc/AvatarPath/17783116.jpg')
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 1973, 'male', 'jean@gmail.com', 'JEAN@GMAIL.COM', 
'jean@gmail.com', 'JEAN@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAECP6q8P1xItlmEJwzj/N9+NWEWygRA5a1Xg2PjKe62a76V+DbJ6qC9i8YKaFl7gb0g==',
'6K2XAPAZK23QUU7F6C3WVNPV2ETHNW6F', '2d6619ae-ba5d-480d-ab70-f2092d40362d', '(111)222777', 0, 0, NULL, 1, 0,
'Jean', 'Vilain', '/Content/Upload/e985d062-69f4-40fd-aa35-ff35dc1ca911/AvatarPath/31745316.jpg')
,('f6b480ef-aa5c-467f-8382-5199a395d585', 1996, 'female', 'sonya@gmail.com', 'SONYA@GMAIL.COM', 
'sonya@gmail.com', 'SONYA@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEJGVQR1xOIkuVO0GuAwT3ifjmPW2gfbTyUC08v1pa776uK+p9bUIqGL7Hx5t/bTuUA==',
'2UB75R4WOUPEWQHVJLBFNCQAQVBSSNNK', '70c79250-b38f-4a15-8fe9-da9828518fb6', '(111)333666', 0, 0, NULL, 1, 0,
'Sonya', 'Night', '/Content/Upload/f6b480ef-aa5c-467f-8382-5199a395d585/AvatarPath/28860078.jpg')
,('f90f2e2c-821e-4651-91d7-105d56ddbee7', 1986, 'female', 'natalia@gmail.com', 'NATALIA@GMAIL.COM', 
'natalia@gmail.com', 'NATALIA@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEDB3p/38dQ13/aLBd/ifgzSMQUSwng5gDEPL5h3ohpq1gqRkas5PHiUomNZ4VWjV8A==',
'WLHNXMZLEYVJ7PHBKU2LB7RM537N6VXY', '6614037c-5cac-49f0-b9f8-92302104afb5', '(111)222888', 0, 0, NULL, 1, 0,
'Natalia', 'Romanoff', '/Content/Upload/f90f2e2c-821e-4651-91d7-105d56ddbee7/AvatarPath/59464845.jpg')
,('7122f67c-d670-47d4-8afb-0fd14b76cec4', 1990, 'female', 'user1@gmail.com', 'USER1@GMAIL.COM', 
'user1@gmail.com', 'USER1@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEHi4aY3vkEzvdZ45s6vDMYK/8HSpx9M5MiGOHqYbLPRBc2Cxu+ccfbidqhg2SitjLQ==',
'ST4TBI57WWUH2BB6AB2TED6D3ME6QOXD', '8d2e8f75-a142-40e1-abbc-9f7a5d9c6c19', '(123)456789', 0, 0, NULL, 1, 0,
'User1', 'Admin1', '/Content/images/defaultAvatar.jpg')
go




insert ChatMessages (SenderId, ReceiverId, Message, Date)
values
('f6b480ef-aa5c-467f-8382-5199a395d585', 'e985d062-69f4-40fd-aa35-ff35dc1ca911', 'Hi, Jean!', '2023-01-11 10:53:00')
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 'f6b480ef-aa5c-467f-8382-5199a395d585', 'Hi, Sonya!', '2023-01-11 10:58:20')
,('f6b480ef-aa5c-467f-8382-5199a395d585', 'e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'Hi, Marwel!', '2023-01-11 12:33:00')
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'f6b480ef-aa5c-467f-8382-5199a395d585', 'Hi, Sonya!', '2023-01-11 12:49:25')
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'f6b480ef-aa5c-467f-8382-5199a395d585', 'How are you?',  '2023-01-11 13:49:00')
,('f6b480ef-aa5c-467f-8382-5199a395d585', 'e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'I am totally great! What about you?',  '2023-01-11 13:58:00')
go



insert ChatContacts (UserId, InterlocutorId)
values
('f6b480ef-aa5c-467f-8382-5199a395d585', 'e985d062-69f4-40fd-aa35-ff35dc1ca911')
,('f6b480ef-aa5c-467f-8382-5199a395d585', 'e26bcd6e-49bd-42d6-acca-905ba29cddbc')
,('e985d062-69f4-40fd-aa35-ff35dc1ca911', 'f6b480ef-aa5c-467f-8382-5199a395d585')
,('e26bcd6e-49bd-42d6-acca-905ba29cddbc', 'f6b480ef-aa5c-467f-8382-5199a395d585')
go



insert GymImages (GymId, ImagePath)
values (1, '/Content/BigRock/1.jpg')
,(1, '/Content/BigRock/2.jpg')
,(1, '/Content/BigRock/3.jpg')
,(1, '/Content/BigRock/4.jpg')
,(1, '/Content/BigRock/5.jpg')
,(2, '/Content/Caliostro/1.jpg')
,(2, '/Content/Caliostro/2.jpg')
,(2, '/Content/Caliostro/3.jpg')
,(2, '/Content/Caliostro/4.jpg')
,(2, '/Content/Caliostro/5.jpg')
,(3, '/Content/FitProfit/1.jpg')
,(3, '/Content/FitProfit/2.jpg')
,(3, '/Content/FitProfit/3.jpg')
,(3, '/Content/FitProfit/4.jpg')
,(3, '/Content/FitProfit/5.jpg')
,(3, '/Content/FitProfit/6.jpg')
,(4, '/Content/GoldSection/1.jpg')
,(4, '/Content/GoldSection/2.jpg')
,(4, '/Content/GoldSection/3.jpg')
,(4, '/Content/GoldSection/4.jpg')
go