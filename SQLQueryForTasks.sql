USE [TaskRegistrar]
GO

/****** Object:  Table [dbo].[Task]    Script Date: 27.07.2016 15:04:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Task](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](50) NOT NULL,
	[Amount] [nchar](200) NOT NULL,
	[Start] [smalldatetime] NOT NULL,
	[End] [smalldatetime] NOT NULL,
	[Status] [int] NOT NULL,
	[Executor] [int] NULL,
 CONSTRAINT [PK__Task__3214EC07271B636F] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Task]  WITH CHECK ADD  CONSTRAINT [FK__Task__Executor__1273C1CD] FOREIGN KEY([Executor])
REFERENCES [dbo].[Executor] ([Id])
GO

ALTER TABLE [dbo].[Task] CHECK CONSTRAINT [FK__Task__Executor__1273C1CD]
GO


