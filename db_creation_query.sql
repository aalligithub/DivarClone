USE [master]
GO
/****** Object:  Database [DivarClone]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE DATABASE [DivarClone]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DivarClone', FILENAME = N'D:\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DivarClone.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DivarClone_log', FILENAME = N'D:\SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DivarClone_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [DivarClone] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DivarClone].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DivarClone] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DivarClone] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DivarClone] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DivarClone] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DivarClone] SET ARITHABORT OFF 
GO
ALTER DATABASE [DivarClone] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DivarClone] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DivarClone] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DivarClone] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DivarClone] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DivarClone] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DivarClone] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DivarClone] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DivarClone] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DivarClone] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DivarClone] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DivarClone] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DivarClone] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DivarClone] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DivarClone] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DivarClone] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [DivarClone] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DivarClone] SET RECOVERY FULL 
GO
ALTER DATABASE [DivarClone] SET  MULTI_USER 
GO
ALTER DATABASE [DivarClone] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DivarClone] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DivarClone] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DivarClone] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DivarClone] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DivarClone] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DivarClone', N'ON'
GO
ALTER DATABASE [DivarClone] SET QUERY_STORE = ON
GO
ALTER DATABASE [DivarClone] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [DivarClone]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 9/29/2024 3:01:30 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 9/29/2024 3:01:30 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 9/29/2024 3:01:30 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 9/29/2024 3:01:30 AM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
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
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Enrollment]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Enrollment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[Password] [nvarchar](30) NULL,
	[Email] [nvarchar](50) NULL,
	[Phone] [varchar](15) NULL,
	[Username] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Listings]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Listings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Price] [int] NOT NULL,
	[Poster] [nvarchar](max) NOT NULL,
	[Category] [int] NOT NULL,
	[DateTimeOfPosting] [datetime2](7) NOT NULL,
	[ImagePath] [nvarchar](max) NULL,
 CONSTRAINT [PK_Listings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperationLogs]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperationLogs](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[Operation] [nvarchar](100) NULL,
	[Details] [nvarchar](max) NULL,
	[LogDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[PermissionId] [int] NOT NULL,
	[PermissionName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RolePermissions]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissions](
	[RoleId] [int] NOT NULL,
	[PermissionId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] NOT NULL,
	[RoleName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SpecialPermissions]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SpecialPermissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PermissionName] [varchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 9/29/2024 3:01:30 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OperationLogs] ADD  DEFAULT (getdate()) FOR [LogDate]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
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
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD FOREIGN KEY([PermissionId])
REFERENCES [dbo].[Permissions] ([PermissionId])
GO
ALTER TABLE [dbo].[RolePermissions]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[SpecialPermissions]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Enrollment] ([ID])
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Enrollment] ([ID])
GO
/****** Object:  StoredProcedure [dbo].[SP_AddLogToDb]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_AddLogToDb]
@Operation NVARCHAR(100),
@Details NVARCHAR(MAX),
@LogDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO OperationLogs VALUES (@Operation, @Details, @LogDate)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CreateListing]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CreateListing]
    @Name NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @Price INT,
    @Poster NVARCHAR(100),
    @Category INT,
    @DateTimeOfPosting DATETIME,
    @ImagePath NVARCHAR(255)
AS

BEGIN

	INSERT INTO Listings(Name, Description, Price, Poster, Category, DateTimeOfPosting, ImagePath)
	VALUES(@Name, @Description, @Price, @Poster, @Category, @DateTimeOfPosting, @ImagePath)

END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUserListing]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_DeleteUserListing]  
@Id INT
AS
BEGIN
    BEGIN TRY
        -- Log start of the operation
        INSERT INTO OperationLogs (Operation, Details) 
        VALUES ('DeleteUserListing', 'Attempting to delete listing with Id = ' + CAST(@Id AS NVARCHAR(10)));

        -- Perform the deletion
        DELETE FROM Listings WHERE Listings.Id = @Id;

        -- Log successful deletion
        INSERT INTO OperationLogs (Operation, Details) 
        VALUES ('DeleteUserListing', 'Successfully deleted listing with Id = ' + CAST(@Id AS NVARCHAR(10)));
    END TRY
    BEGIN CATCH
        -- Log the error if deletion fails
        DECLARE @ErrorMessage NVARCHAR(MAX) = ERROR_MESSAGE();
        INSERT INTO OperationLogs (Operation, Details) 
        VALUES ('DeleteUserListing', 'Error deleting listing with Id = ' + CAST(@Id AS NVARCHAR(10)) + ': ' + @ErrorMessage);
    END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[SP_EnrollDetail]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_EnrollDetail]
(
    @FirstName varchar(50),
    @Password varchar(50),
    @Email nvarchar(50),
    @Phone varchar(15),
    @Username nvarchar(20),
    @status varchar(15)
)
AS
BEGIN
	IF @status = 'Insert'
		BEGIN
			INSERT INTO Enrollment (FirstName, Password, Email, Phone, Username)
			VALUES (@FirstName, @Password, @Email, @Phone, @Username)
			INSERT INTO [dbo].[UserRoles] (UserId, RoleId) SELECT SCOPE_IDENTITY(), 3;
		END

	--ELSE IF @status = 'Update'
	--	BEGIN
	--		PRINT 'UPDATE CODE HERE'
	--	END
	--ELSE
	--	BEGIN
	--		 Handle an unknown or unsupported status
	--		PRINT 'Invalid status provided. Supported statuses are Insert and Update.'
	--	END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FilterListing]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_FilterListing] 
@category_enum INT
AS
BEGIN
	SELECT * FROM Listings WHERE Listings.Category = @category_enum;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllUsers]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetAllUsers]

AS
BEGIN
	SELECT ID, FirstName, Password, Email, Phone, Username FROM Enrollment;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetEnrollmentDetails]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetEnrollmentDetails]
(
    @Username nvarchar(20)
)
AS
BEGIN
    SELECT * FROM Enrollment WHERE Username = @Username
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetListings]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GetListings]

AS

BEGIN
	SELECT Id, Name, Description, Price, Poster, Category, DateTimeOfPosting, ImagePath FROM Listings
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetRoleFromUserRoles]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetRoleFromUserRoles]
@UserId NVARCHAR(50)

AS
BEGIN
    SELECT Roles.RoleName 
    FROM UserRoles 
    INNER JOIN Roles ON UserRoles.RoleId = Roles.RoleId 
    WHERE UserRoles.UserId = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetSpecialUserPermissions]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetSpecialUserPermissions] 
	@UserId INT
AS
BEGIN

	SET NOCOUNT ON;

    SELECT PermissionName FROM [dbo].[SpecialPermissions] WHERE UserId = @UserId;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetSpecificListing]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetSpecificListing] 
@Id INT
AS
BEGIN

	SELECT * FROM Listings WHERE Listings.Id = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserPermissions]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_GetUserPermissions] 
@UserId int
AS
BEGIN
SELECT 
    --e.ID, 
    --e.Username, 
    --e.Email, 
    --r.RoleName, 
    p.PermissionName
FROM 
    Enrollment e
INNER JOIN 
    UserRoles ur ON e.ID = ur.UserId
INNER JOIN 
    Roles r ON ur.RoleId = r.RoleId
INNER JOIN 
    RolePermissions rp ON r.RoleId = rp.RoleId
INNER JOIN 
    Permissions p ON rp.PermissionId = p.PermissionId
WHERE 
    e.ID = @UserId;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_LogUserIn]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_LogUserIn]
(
    @Password varchar(50),
    @Email nvarchar(50)
)
AS
BEGIN
	SELECT ID, Username, Email, Password FROM Enrollment
	WHERE Email=@Email AND Password=@Password
END
GO
/****** Object:  StoredProcedure [dbo].[SP_SearchListing]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_SearchListing]
@TextToSearch NVARCHAR(MAX)
AS
BEGIN
	SELECT * FROM Listings WHERE Listings.Name LIKE '%' + @TextToSearch + '%';
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ShowUserListings]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ShowUserListings]
@Username NVARCHAR(100)
AS
BEGIN
	SELECT * FROM Listings WHERE Listings.Poster = @Username;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateListing]    Script Date: 9/29/2024 3:01:30 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_UpdateListing]
@Id INT,
@Name NVARCHAR(MAX),
@Description NVARCHAR(MAX),
@Price INT,
@Poster NVARCHAR(MAX),
@Category INT,
@DateTime DATETIME2(7),
@ImagePath NVARCHAR(MAX)
AS
BEGIN
	UPDATE Listings
	SET Name = @Name, Description = @Description, Price = @Price, Poster = @Poster, Category = @Category, DateTimeOfPosting = @DateTime, ImagePath = @ImagePath
	WHERE Id = @Id;

	INSERT INTO OperationLogs VALUES ('UPDATE LISTING SUCCESS','listing updated from sp', @DateTime);
END
GO
USE [master]
GO
ALTER DATABASE [DivarClone] SET  READ_WRITE 
GO
