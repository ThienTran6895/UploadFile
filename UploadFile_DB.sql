USE [master]
GO
/****** Object:  Database [UploadFile]    Script Date: 8/6/2019 7:39:31 AM ******/
CREATE DATABASE [UploadFile]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'UploadFile', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\UploadFile.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'UploadFile_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\UploadFile_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [UploadFile] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [UploadFile].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [UploadFile] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [UploadFile] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [UploadFile] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [UploadFile] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [UploadFile] SET ARITHABORT OFF 
GO
ALTER DATABASE [UploadFile] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [UploadFile] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [UploadFile] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [UploadFile] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [UploadFile] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [UploadFile] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [UploadFile] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [UploadFile] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [UploadFile] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [UploadFile] SET  DISABLE_BROKER 
GO
ALTER DATABASE [UploadFile] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [UploadFile] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [UploadFile] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [UploadFile] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [UploadFile] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [UploadFile] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [UploadFile] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [UploadFile] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [UploadFile] SET  MULTI_USER 
GO
ALTER DATABASE [UploadFile] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [UploadFile] SET DB_CHAINING OFF 
GO
ALTER DATABASE [UploadFile] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [UploadFile] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [UploadFile] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [UploadFile] SET QUERY_STORE = OFF
GO
USE [UploadFile]
GO
/****** Object:  Table [dbo].[FileUpload]    Script Date: 8/6/2019 7:39:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FileUpload](
	[FileID] [uniqueidentifier] NULL,
	[NameFile] [nvarchar](50) NULL,
	[UploadDate] [datetime] NULL,
	[UploadPerson] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[uploadfile_AddFile]    Script Date: 8/6/2019 7:39:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
--	Name				Date			Description
--	thientc			05/08/2019			Thêm File
-- =============================================
CREATE PROCEDURE [dbo].[uploadfile_AddFile]
	-- Add the parameters for the stored procedure here
    @FileID UNIQUEIDENTIFIER ,
    @NameFile nvarchar(50),		
	@UploadPerson nvarchar(50)
AS
    BEGIN				
        INSERT INTO FileUpload
                ( FileID ,
                  NameFile ,
                  
                  UploadPerson                   
                )
		OUTPUT  Inserted.FileID
        VALUES  ( @FileID ,
                  @NameFile ,
                  
                  @UploadPerson                  
                )				
    END
GO
USE [master]
GO
ALTER DATABASE [UploadFile] SET  READ_WRITE 
GO
