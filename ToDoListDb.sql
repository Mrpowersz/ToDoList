USE [master]
GO
/****** Object:  Database [ToDoListDb]    Script Date: 29.04.2024 09:41:16 ******/
CREATE DATABASE [ToDoListDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ToDoListDb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\ToDoListDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ToDoListDb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\ToDoListDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [ToDoListDb] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ToDoListDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ToDoListDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ToDoListDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ToDoListDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ToDoListDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ToDoListDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [ToDoListDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ToDoListDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ToDoListDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ToDoListDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ToDoListDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ToDoListDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ToDoListDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ToDoListDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ToDoListDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ToDoListDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ToDoListDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ToDoListDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ToDoListDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ToDoListDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ToDoListDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ToDoListDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ToDoListDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ToDoListDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ToDoListDb] SET  MULTI_USER 
GO
ALTER DATABASE [ToDoListDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ToDoListDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ToDoListDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ToDoListDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ToDoListDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ToDoListDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ToDoListDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [ToDoListDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [ToDoListDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 29.04.2024 09:41:16 ******/
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
/****** Object:  Table [dbo].[Statuses]    Script Date: 29.04.2024 09:41:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Statuses](
	[StatusId] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDos]    Script Date: 29.04.2024 09:41:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DueDate] [datetime2](7) NOT NULL,
	[StatusId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_ToDos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ToDos_StatusId]    Script Date: 29.04.2024 09:41:16 ******/
CREATE NONCLUSTERED INDEX [IX_ToDos_StatusId] ON [dbo].[ToDos]
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ToDos]  WITH CHECK ADD  CONSTRAINT [FK_ToDos_Statuses_StatusId] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Statuses] ([StatusId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ToDos] CHECK CONSTRAINT [FK_ToDos_Statuses_StatusId]
GO
USE [master]
GO
ALTER DATABASE [ToDoListDb] SET  READ_WRITE 
GO
