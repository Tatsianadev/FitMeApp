USE [master]
GO

/****** Object:  Database [FitMeDB]    Script Date: 11/4/2022 5:56:12 PM ******/
CREATE DATABASE [FitMeDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
--( NAME = N'FitMeDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\FitMeDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
-- LOG ON 
--( NAME = N'FitMeDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\FitMeDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
--GO
( NAME = N'FitMeDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\FitMeDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'FitMeDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\FitMeDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FitMeDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [FitMeDB] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [FitMeDB] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [FitMeDB] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [FitMeDB] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [FitMeDB] SET ARITHABORT OFF 
GO

ALTER DATABASE [FitMeDB] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [FitMeDB] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [FitMeDB] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [FitMeDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [FitMeDB] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [FitMeDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [FitMeDB] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [FitMeDB] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [FitMeDB] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [FitMeDB] SET  ENABLE_BROKER 
GO

ALTER DATABASE [FitMeDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [FitMeDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [FitMeDB] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [FitMeDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [FitMeDB] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [FitMeDB] SET READ_COMMITTED_SNAPSHOT ON 
GO

ALTER DATABASE [FitMeDB] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [FitMeDB] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [FitMeDB] SET  MULTI_USER 
GO

ALTER DATABASE [FitMeDB] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [FitMeDB] SET DB_CHAINING OFF 
GO

ALTER DATABASE [FitMeDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [FitMeDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [FitMeDB] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [FitMeDB] SET QUERY_STORE = OFF
GO

ALTER DATABASE [FitMeDB] SET  READ_WRITE 
GO




--Identity--

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[Year] [int] NOT NULL,
	[Gender] [nvarchar](max) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[FirstName] [nvarchar](256) NULL,
	[LastName] [nvarchar](256) NULL,
	[AvatarPath] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO




SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO




SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO




SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO




SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO



--Business--

Use FitMeDB
go
 

create table Gyms
(
Id int identity primary key not null,
Name nvarchar(128) not null,
Address nvarchar(max) not null,
Phone nvarchar(256) null
)
go



CREATE TABLE [dbo].[Trainers](
	[Id] [nvarchar](256) NOT NULL,
	[Specialization] [nvarchar](250) NOT NULL,
	[GymId] [int] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Trainers]  WITH CHECK ADD FOREIGN KEY([GymId])
REFERENCES [dbo].[Gyms] ([Id])
GO




create table Trainings
(
Id int identity primary key not null,
Name nvarchar(256) not null,
Description nvarchar (max) null
)
go




CREATE TABLE [dbo].[TrainingTrainer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TrainingId] [int] NOT NULL,	
	[TrainerId] [nvarchar](256) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TrainingTrainer]  WITH CHECK ADD FOREIGN KEY([TrainingId])
REFERENCES [dbo].[Trainings] ([Id])
GO

ALTER TABLE [dbo].[TrainingTrainer]  WITH CHECK ADD FOREIGN KEY([TrainerId])
REFERENCES [dbo].[Trainers] ([Id])
GO



--Subscribtions

CREATE TABLE [dbo].[Subscriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ValidDays] [int] NOT NULL,
	[GroupTraining] [bit] NOT NULL,
	[DietMonitoring] [bit] NOT NULL,
	[WorkAsTrainer] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[GymSubscriptions](
	[Id] [int] IDENTITY(1,1) Primary key NOT NULL,
	[GymId] [int] NOT NULL,
	[SubscriptionId] [int] NOT NULL,
	[Price] [int] NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[GymSubscriptions]  WITH CHECK ADD FOREIGN KEY([GymId])
REFERENCES [dbo].[Gyms] ([Id])
GO

ALTER TABLE [dbo].[GymSubscriptions]  WITH CHECK ADD FOREIGN KEY([SubscriptionId])
REFERENCES [dbo].[Subscriptions] ([Id])
GO



CREATE TABLE [dbo].[UserSubscriptions](
	[Id] [int] IDENTITY(1,1) Primary key NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[GymSubscriptionId] [int] NOT NULL,	
	[StartDate] [smalldatetime] NOT NULL,
	[EndDate] [smalldatetime] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserSubscriptions]  WITH CHECK ADD FOREIGN KEY([GymSubscriptionId])
REFERENCES [dbo].[GymSubscriptions] ([Id])
GO

--Schedule--

CREATE TABLE [dbo].[Events](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [date] NOT NULL,
	[StartTime] [int] NOT NULL,
	[EndTime] [int] NOT NULL,
	[TrainerId] [nvarchar](256) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[TrainingId] [int] NOT NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Events]  WITH CHECK ADD FOREIGN KEY([TrainerId])
REFERENCES [dbo].[Trainers] ([Id])
GO



create table GymWorkHours
(
Id int primary key identity not null,
DayOfWeekNumber int not null,
GymId int not null,
StartTime int not null,
EndTime int not null,
foreign key (GymId) references Gyms(Id)
)
go


create table TrainerWorkHours
(
Id int primary key identity not null,
TrainerId nvarchar(450) not null,
StartTime int not null,
EndTime int not null,
GymWorkHoursId int not null,
foreign key (TrainerId) references AspNetUsers(Id),
foreign key (GymWorkHoursId) references GymWorkHours(Id)
)
go


create table ChatMessages
(
Id int identity primary key not null,
SenderId nvarchar(256) not null,
ReceiverId nvarchar(256) not null,
Message nvarchar(max) not null,
Date smalldatetime not null
)
go



create table ChatContacts
(
Id int identity primary key not null,
UserId nvarchar(450) not null,
InterlocutorId nvarchar(450) not null
)
go

ALTER TABLE [dbo].[ChatContacts]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO

ALTER TABLE [dbo].[ChatContacts]  WITH CHECK ADD FOREIGN KEY([InterlocutorId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
