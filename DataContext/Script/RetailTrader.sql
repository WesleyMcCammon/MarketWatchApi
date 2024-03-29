USE [master]
GO
/****** Object:  Database [RetailTrader]    Script Date: 10/4/2022 4:13:31 PM ******/
CREATE DATABASE [RetailTrader]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RetailTrader', FILENAME = N'/var/opt/mssql/data/RetailTrader.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RetailTrader_log', FILENAME = N'/var/opt/mssql/data/RetailTrader_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [RetailTrader] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RetailTrader].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RetailTrader] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RetailTrader] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RetailTrader] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RetailTrader] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RetailTrader] SET ARITHABORT OFF 
GO
ALTER DATABASE [RetailTrader] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RetailTrader] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RetailTrader] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RetailTrader] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RetailTrader] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RetailTrader] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RetailTrader] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RetailTrader] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RetailTrader] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RetailTrader] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RetailTrader] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RetailTrader] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RetailTrader] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RetailTrader] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RetailTrader] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RetailTrader] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RetailTrader] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RetailTrader] SET RECOVERY FULL 
GO
ALTER DATABASE [RetailTrader] SET  MULTI_USER 
GO
ALTER DATABASE [RetailTrader] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RetailTrader] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RetailTrader] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RetailTrader] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RetailTrader] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'RetailTrader', N'ON'
GO
ALTER DATABASE [RetailTrader] SET QUERY_STORE = OFF
GO
USE [RetailTrader]
GO
/****** Object:  Schema [HangFire]    Script Date: 10/4/2022 4:13:31 PM ******/
CREATE SCHEMA [HangFire]
GO
/****** Object:  Table [dbo].[Instrument]    Script Date: 10/4/2022 4:13:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Instrument](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[MarketId] [int] NOT NULL,
	[MarketSectorId] [int] NOT NULL,
 CONSTRAINT [PK_Instrument] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InstrumentDataSource]    Script Date: 10/4/2022 4:13:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstrumentDataSource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InstrumentId] [int] NOT NULL,
	[Source] [nvarchar](50) NOT NULL,
	[Symbol] [nvarchar](50) NOT NULL,
	[IsLive] [bit] NOT NULL,
 CONSTRAINT [PK_InstrumentDataSource] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Market]    Script Date: 10/4/2022 4:13:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Market](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Market] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MarketSector]    Script Date: 10/4/2022 4:13:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MarketSector](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MarketId] [int] NOT NULL,
 CONSTRAINT [PK_MarketSector] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Instrument] ON 

INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (1, N'/ES', N'Emini S&P 500', 0, 2, 3)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (2, N'/NQ', N'E-Mini NASDAQ-100', 0, 2, 3)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (3, N'/YM', N'E-mini Dow', 0, 2, 3)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (4, N'/RTY', N'E-mini Russell 2000', 0, 2, 3)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (5, N'/CL', N'Crude Oil', 0, 2, 4)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (6, N'/GC', N'Gold', 0, 2, 5)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (7, N'EURUSD', N'Euro Dollar', 1, 1, 1)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (8, N'GBPUSD', N'Pound Dollar', 1, 1, 1)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (9, N'USDCHF', N'Dollar Swissy', 1, 1, 1)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (10, N'USDJPY', N'Dollar Yen', 1, 1, 1)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (11, N'AUDUSD', N'Aussie Dollar', 1, 1, 1)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (12, N'NZDUSD', N'Kiwi Dollar', 1, 1, 1)
INSERT [dbo].[Instrument] ([Id], [Symbol], [Description], [IsActive], [MarketId], [MarketSectorId]) VALUES (13, N'USDCAD', N'Dollar Loonie', 1, 1, 1)
SET IDENTITY_INSERT [dbo].[Instrument] OFF
GO
SET IDENTITY_INSERT [dbo].[InstrumentDataSource] ON 

INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (1, 1, N'TOS', N'/ES:XCME', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (2, 2, N'TOS', N'/NQ:XCME', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (3, 3, N'TOS', N'/YM:XCBT', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (4, 4, N'TOS', N'/RTY:XCME', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (5, 5, N'TOS', N'/CL:XNYM', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (6, 6, N'TOS', N'/GC:XCEC', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (7, 7, N'TOS', N'EUR/USD', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (8, 8, N'TOS', N'GBP/USD', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (9, 9, N'TOS', N'USD/CHF', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (10, 10, N'TOS', N'USD/JPY', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (11, 11, N'TOS', N'AUD/USD', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (12, 12, N'TOS', N'NZD/USD', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (13, 13, N'TOS', N'USD/CAD', 1)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (14, 1, N'unknown', N'/ES:XCME', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (15, 2, N'unknown', N'/NQ:XCME', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (16, 3, N'unknown', N'/YM:XCBT', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (17, 4, N'unknown', N'/RTY:XCME', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (18, 5, N'unknown', N'/CL:XNYM', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (19, 6, N'unknown', N'/GC:XCEC', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (20, 7, N'Oanda', N'EUR_USD', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (21, 8, N'Oanda', N'GBP_USD', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (22, 9, N'Oanda', N'USD_CHF', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (23, 10, N'Oanda', N'USD_JPY', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (24, 11, N'Oanda', N'AUD_USD', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (25, 12, N'Oanda', N'NZD_USD', 0)
INSERT [dbo].[InstrumentDataSource] ([Id], [InstrumentId], [Source], [Symbol], [IsLive]) VALUES (26, 13, N'Oanda', N'USD_CAD', 0)
SET IDENTITY_INSERT [dbo].[InstrumentDataSource] OFF
GO
SET IDENTITY_INSERT [dbo].[Market] ON 

INSERT [dbo].[Market] ([Id], [Name]) VALUES (1, N'Forex')
INSERT [dbo].[Market] ([Id], [Name]) VALUES (2, N'Futures')
SET IDENTITY_INSERT [dbo].[Market] OFF
GO
SET IDENTITY_INSERT [dbo].[MarketSector] ON 

INSERT [dbo].[MarketSector] ([Id], [Name], [MarketId]) VALUES (1, N'Major', 1)
INSERT [dbo].[MarketSector] ([Id], [Name], [MarketId]) VALUES (2, N'Minor', 1)
INSERT [dbo].[MarketSector] ([Id], [Name], [MarketId]) VALUES (3, N'Index', 2)
INSERT [dbo].[MarketSector] ([Id], [Name], [MarketId]) VALUES (4, N'Energy', 2)
INSERT [dbo].[MarketSector] ([Id], [Name], [MarketId]) VALUES (5, N'Metal', 2)
SET IDENTITY_INSERT [dbo].[MarketSector] OFF
GO
ALTER TABLE [dbo].[Instrument]  WITH CHECK ADD  CONSTRAINT [FK_Instrument_Market] FOREIGN KEY([MarketId])
REFERENCES [dbo].[Market] ([Id])
GO
ALTER TABLE [dbo].[Instrument] CHECK CONSTRAINT [FK_Instrument_Market]
GO
ALTER TABLE [dbo].[Instrument]  WITH CHECK ADD  CONSTRAINT [FK_Instrument_MarketSector] FOREIGN KEY([MarketSectorId])
REFERENCES [dbo].[MarketSector] ([Id])
GO
ALTER TABLE [dbo].[Instrument] CHECK CONSTRAINT [FK_Instrument_MarketSector]
GO
ALTER TABLE [dbo].[InstrumentDataSource]  WITH CHECK ADD  CONSTRAINT [FK_InstrumentDataSource_Instrument] FOREIGN KEY([InstrumentId])
REFERENCES [dbo].[Instrument] ([Id])
GO
ALTER TABLE [dbo].[InstrumentDataSource] CHECK CONSTRAINT [FK_InstrumentDataSource_Instrument]
GO
ALTER TABLE [dbo].[MarketSector]  WITH CHECK ADD  CONSTRAINT [FK_MarketSector_Market] FOREIGN KEY([MarketId])
REFERENCES [dbo].[Market] ([Id])
GO
ALTER TABLE [dbo].[MarketSector] CHECK CONSTRAINT [FK_MarketSector_Market]
GO
USE [master]
GO
ALTER DATABASE [RetailTrader] SET  READ_WRITE 
GO
