USE [master]
GO
/****** Object:  Database [Car_Center]    Script Date: 02-May-16 12:49:37 ******/
CREATE DATABASE [Car_Center]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Car_Center', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Car_Center.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Car_Center_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\Car_Center_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Car_Center] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Car_Center].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Car_Center] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Car_Center] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Car_Center] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Car_Center] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Car_Center] SET ARITHABORT OFF 
GO
ALTER DATABASE [Car_Center] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Car_Center] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Car_Center] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Car_Center] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Car_Center] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Car_Center] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Car_Center] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Car_Center] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Car_Center] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Car_Center] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Car_Center] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Car_Center] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Car_Center] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Car_Center] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Car_Center] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Car_Center] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Car_Center] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Car_Center] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Car_Center] SET  MULTI_USER 
GO
ALTER DATABASE [Car_Center] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Car_Center] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Car_Center] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Car_Center] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Car_Center] SET DELAYED_DURABILITY = DISABLED 
GO
USE [Car_Center]
GO
/****** Object:  Table [dbo].[Center]    Script Date: 02-May-16 12:49:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Center](
	[Center_Id] [int] IDENTITY(1,1) NOT NULL,
	[Town_Id] [int] NULL,
	[Adress] [varchar](50) NULL,
	[Telephone] [nvarchar](50) NULL,
 CONSTRAINT [XPKCenter] PRIMARY KEY CLUSTERED 
(
	[Center_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Condition]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Condition](
	[Condition_Number] [int] NOT NULL,
	[Description] [varchar](20) NULL,
 CONSTRAINT [XPKCondition] PRIMARY KEY CLUSTERED 
(
	[Condition_Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Country]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Country](
	[Country_Id] [varchar](3) NOT NULL,
	[Name] [varchar](20) NULL,
 CONSTRAINT [XPKCountry] PRIMARY KEY CLUSTERED 
(
	[Country_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[Customer_Id] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [varchar](20) NULL,
	[Last_Name] [varchar](20) NULL,
	[Birth_Date] [date] NULL,
 CONSTRAINT [XPKCustomer] PRIMARY KEY CLUSTERED 
(
	[Customer_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employee](
	[Employee_Id] [int] IDENTITY(1,1) NOT NULL,
	[First_Name] [varchar](20) NULL,
	[Last_Name] [varchar](20) NULL,
	[Birth_Date] [date] NULL,
	[Center_Id] [int] NULL,
 CONSTRAINT [XPKEmployee] PRIMARY KEY CLUSTERED 
(
	[Employee_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rental_Car]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rental_Car](
	[Car_Number] [varchar](6) NOT NULL,
	[Firm] [varchar](20) NULL,
	[Model] [varchar](20) NULL,
	[Colour] [varchar](20) NULL,
	[Engine] [varchar](20) NULL,
	[Country] [varchar](20) NULL,
	[Condition_Number] [int] NULL,
	[Center_Id] [int] NULL,
 CONSTRAINT [XPKRental_Car] PRIMARY KEY CLUSTERED 
(
	[Car_Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rental_Contract]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rental_Contract](
	[Contract_Id] [int] IDENTITY(1,1) NOT NULL,
	[Car_Number] [varchar](6) NOT NULL,
	[Customer_Id] [int] NOT NULL,
	[Employee_Id] [int] NOT NULL,
	[Date_Of_Begin] [date] NULL,
	[Date_Of_End] [date] NULL,
	[Number_of_days] [int] NULL,
	[Price_Per_Day] [decimal](20, 2) NOT NULL,
	[Total_Price] [decimal](20, 2) NULL,
 CONSTRAINT [XPKRental_Contract] PRIMARY KEY CLUSTERED 
(
	[Contract_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Car]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sales_Car](
	[Car_Id] [int] IDENTITY(1,1) NOT NULL,
	[Firm] [varchar](20) NULL,
	[Model] [varchar](20) NULL,
	[Colour] [varchar](20) NULL,
	[Engine] [varchar](20) NULL,
	[Country] [varchar](20) NULL,
	[Center_Id] [int] NULL,
 CONSTRAINT [XPKSales_Car] PRIMARY KEY CLUSTERED 
(
	[Car_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Sales_Contract]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales_Contract](
	[Contract_Id] [int] IDENTITY(1,1) NOT NULL,
	[Car_Id] [int] NOT NULL,
	[Employee_Id] [int] NULL,
	[Customer_Id] [int] NULL,
	[Date] [date] NULL,
	[Price] [decimal](20, 2) NOT NULL,
 CONSTRAINT [XPKSales_Contract] PRIMARY KEY CLUSTERED 
(
	[Contract_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Town]    Script Date: 02-May-16 12:49:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Town](
	[Town_Id] [int] NOT NULL,
	[Country_Id] [varchar](3) NULL,
	[Name] [varchar](20) NULL,
 CONSTRAINT [XPKTown] PRIMARY KEY CLUSTERED 
(
	[Town_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Center]  WITH CHECK ADD  CONSTRAINT [R_2] FOREIGN KEY([Town_Id])
REFERENCES [dbo].[Town] ([Town_Id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Center] CHECK CONSTRAINT [R_2]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [R_4] FOREIGN KEY([Center_Id])
REFERENCES [dbo].[Center] ([Center_Id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [R_4]
GO
ALTER TABLE [dbo].[Rental_Car]  WITH CHECK ADD  CONSTRAINT [FK_Rental_Car_Center] FOREIGN KEY([Center_Id])
REFERENCES [dbo].[Center] ([Center_Id])
GO
ALTER TABLE [dbo].[Rental_Car] CHECK CONSTRAINT [FK_Rental_Car_Center]
GO
ALTER TABLE [dbo].[Rental_Car]  WITH CHECK ADD  CONSTRAINT [R_5] FOREIGN KEY([Condition_Number])
REFERENCES [dbo].[Condition] ([Condition_Number])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Rental_Car] CHECK CONSTRAINT [R_5]
GO
ALTER TABLE [dbo].[Rental_Contract]  WITH CHECK ADD  CONSTRAINT [FK_Rental_Contract_Customer] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Customer_Id])
GO
ALTER TABLE [dbo].[Rental_Contract] CHECK CONSTRAINT [FK_Rental_Contract_Customer]
GO
ALTER TABLE [dbo].[Rental_Contract]  WITH CHECK ADD  CONSTRAINT [FK_Rental_Contract_Employee] FOREIGN KEY([Employee_Id])
REFERENCES [dbo].[Employee] ([Employee_Id])
GO
ALTER TABLE [dbo].[Rental_Contract] CHECK CONSTRAINT [FK_Rental_Contract_Employee]
GO
ALTER TABLE [dbo].[Rental_Contract]  WITH CHECK ADD  CONSTRAINT [FK_Rental_Contract_Rental_Car] FOREIGN KEY([Car_Number])
REFERENCES [dbo].[Rental_Car] ([Car_Number])
GO
ALTER TABLE [dbo].[Rental_Contract] CHECK CONSTRAINT [FK_Rental_Contract_Rental_Car]
GO
ALTER TABLE [dbo].[Sales_Car]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Car_Center] FOREIGN KEY([Center_Id])
REFERENCES [dbo].[Center] ([Center_Id])
GO
ALTER TABLE [dbo].[Sales_Car] CHECK CONSTRAINT [FK_Sales_Car_Center]
GO
ALTER TABLE [dbo].[Sales_Contract]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Contract_Sales_Car] FOREIGN KEY([Car_Id])
REFERENCES [dbo].[Sales_Car] ([Car_Id])
GO
ALTER TABLE [dbo].[Sales_Contract] CHECK CONSTRAINT [FK_Sales_Contract_Sales_Car]
GO
ALTER TABLE [dbo].[Sales_Contract]  WITH CHECK ADD  CONSTRAINT [R_7] FOREIGN KEY([Customer_Id])
REFERENCES [dbo].[Customer] ([Customer_Id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Sales_Contract] CHECK CONSTRAINT [R_7]
GO
ALTER TABLE [dbo].[Sales_Contract]  WITH CHECK ADD  CONSTRAINT [R_8] FOREIGN KEY([Employee_Id])
REFERENCES [dbo].[Employee] ([Employee_Id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Sales_Contract] CHECK CONSTRAINT [R_8]
GO
ALTER TABLE [dbo].[Town]  WITH CHECK ADD  CONSTRAINT [R_1] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[Country] ([Country_Id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Town] CHECK CONSTRAINT [R_1]
GO
USE [master]
GO
ALTER DATABASE [Car_Center] SET  READ_WRITE 
GO
