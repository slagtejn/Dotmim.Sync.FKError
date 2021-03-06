USE [master]
GO 

-- Server database
if (exists (select * from sys.databases where name = 'ServerDB'))
Begin
	ALTER DATABASE [ServerDB] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [ServerDB]
End
Create database [ServerDB]
Go
-- Client database. No need to create the schema, Dotmim.Sync will do
if (exists (select * from sys.databases where name = 'Client'))
Begin
	ALTER DATABASE [Client] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [Client]
End
Create database [Client]

GO

-- Script for Server database
USE [ServerDB]
GO

CREATE TABLE [dbo].[FilterTable2](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FilterId] [int] NOT NULL,
	CONSTRAINT [PK_FilterTable2] PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (FILLFACTOR = 75)
) ON [PRIMARY]

CREATE TABLE [dbo].[FilterTable1](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FilterTable2_Id] [int] NOT NULL,
	[FilterId] [int] NOT NULL,
	CONSTRAINT [PK_FilterTable1] PRIMARY KEY NONCLUSTERED ([Id] ASC) WITH (FILLFACTOR = 75),
	CONSTRAINT [FK_FilterTable1_FilterTable2] FOREIGN KEY ([FilterTable2_Id]) REFERENCES [dbo].[FilterTable2] ([Id])
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [dbo].[FilterTable2] ON 
INSERT [dbo].[FilterTable2] ([Id], [FilterId]) VALUES (1, 1)
INSERT [dbo].[FilterTable2] ([Id], [FilterId]) VALUES (2, 2)
INSERT [dbo].[FilterTable2] ([Id], [FilterId]) VALUES (3, 1)
SET IDENTITY_INSERT [dbo].[FilterTable2] OFF

GO

SET IDENTITY_INSERT [dbo].[FilterTable1] ON 
INSERT [dbo].[FilterTable1] ([Id], [FilterTable2_Id], [FilterId]) VALUES (1, 2, 1)
INSERT [dbo].[FilterTable1] ([Id], [FilterTable2_Id], [FilterId]) VALUES (2, 1 ,2)
INSERT [dbo].[FilterTable1] ([Id], [FilterTable2_Id], [FilterId]) VALUES (3, 2 ,1)
SET IDENTITY_INSERT [dbo].[FilterTable1] OFF 
