USE [master]
GO

/****** Object:  Database [TaskRegistrar]    Script Date: 27.07.2016 14:58:20 ******/
CREATE DATABASE [TaskRegistrar]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TaskRegistrar', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\TaskRegistrar.mdf' , SIZE = 4288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TaskRegistrar_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\TaskRegistrar_log.ldf' , SIZE = 1072KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [TaskRegistrar] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TaskRegistrar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [TaskRegistrar] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [TaskRegistrar] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [TaskRegistrar] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [TaskRegistrar] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [TaskRegistrar] SET ARITHABORT OFF 
GO

ALTER DATABASE [TaskRegistrar] SET AUTO_CLOSE ON 
GO

ALTER DATABASE [TaskRegistrar] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [TaskRegistrar] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [TaskRegistrar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [TaskRegistrar] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [TaskRegistrar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [TaskRegistrar] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [TaskRegistrar] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [TaskRegistrar] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [TaskRegistrar] SET  ENABLE_BROKER 
GO

ALTER DATABASE [TaskRegistrar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [TaskRegistrar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [TaskRegistrar] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [TaskRegistrar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [TaskRegistrar] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [TaskRegistrar] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [TaskRegistrar] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [TaskRegistrar] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [TaskRegistrar] SET  MULTI_USER 
GO

ALTER DATABASE [TaskRegistrar] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [TaskRegistrar] SET DB_CHAINING OFF 
GO

ALTER DATABASE [TaskRegistrar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [TaskRegistrar] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [TaskRegistrar] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [TaskRegistrar] SET  READ_WRITE 
GO

