USE [FitMeDB]
GO

--alter table AspNetUsers
--add 
--FirstName nvarchar (256),
--LastName nvarchar (256)
--go


--alter table AspNetUsers
--add 
--LastName nvarchar (256)
--go

--alter table AspNetUsers
--add 
--Avatar nvarchar (256)
--go


--alter table Trainers
--drop column FirstName
--go

--alter table Trainers
--drop column LastName
--go

alter table Trainers
drop constraint [DF__Trainers__Pictur__1B9317B3]
go

--alter table Trainers
--drop column Picture
--go

--alter table Trainers
--drop column Gender
--go

--alter table Trainers
--add Status int
--go