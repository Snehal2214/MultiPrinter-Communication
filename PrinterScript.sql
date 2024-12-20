USE [VideojetAPP]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 18-12-2024 10:18:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginId] [varchar](20) NOT NULL,
	[Password] [varchar](20) NOT NULL,
	[Role] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 18-12-2024 10:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PName] [varchar](100) NOT NULL,
	[IP] [varchar](50) NOT NULL,
	[Port] [int] NOT NULL,
	[FPath] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetAllValues]    Script Date: 18-12-2024 10:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAllValues]
AS
BEGIN
   
    SELECT * 
    FROM Settings;
END;
GO
/****** Object:  StoredProcedure [dbo].[SaveSetting]    Script Date: 18-12-2024 10:18:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveSetting]
    @Id INT = NULL,             -- Optional, used for updating existing records
    @Pname VARCHAR(100),
    @IP VARCHAR(50),
    @Port VARCHAR(25),
    @Fpath VARCHAR(100)
AS
BEGIN
    -- If Id is NULL, we are inserting a new record
    IF @Id IS NULL
    BEGIN
        INSERT INTO Settings (Pname, IP, Port, Fpath)
        VALUES (@Pname, @IP, @Port, @Fpath);
    END
    -- If Id is not NULL, we are updating an existing record
    ELSE
    BEGIN
        UPDATE Settings
        SET Pname = @Pname,
            IP = @IP,
            Port = @Port,
            Fpath = @Fpath
        WHERE Id = @Id;
    END
END;
GO
