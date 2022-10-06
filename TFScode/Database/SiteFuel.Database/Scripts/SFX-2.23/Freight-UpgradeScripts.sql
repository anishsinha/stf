GO
-- =============================================
-- Author:		<Jigar Patel>
-- Create date: <04/27/2022>
-- Description:	<Update Demand SiteId Information>
-- =============================================
CREATE OR ALTER PROCEDURE usp_Update_Demands_SiteId_Information
	@SiteID nvarchar(500),
	@PreviousSiteID nvarchar(500)
AS
BEGIN
	SET NOCOUNT ON;
	IF(@PreviousSiteID <>'')
	BEGIN
		Update Demands set SiteId=@SiteID where SiteId=@PreviousSiteID AND Level=0 AND Ullage=0 AND GrossVolume=0
	END
	ELSE
	BEGIN
		Update Demands set SiteId=@SiteID where SiteId IS NULL AND Level=0 AND Ullage=0 AND GrossVolume=0
	END
END
GO

DROP PROCEDURE IF EXISTS [dbo].[usp_InsertIntoDemands]
GO

DROP TYPE IF EXISTS [dbo].[DemandsTypes]
GO

/****** Object:  UserDefinedTableType [dbo].[DemandsTypes]    Script Date: 4/27/2022 10:22:38 PM ******/
CREATE TYPE [dbo].[DemandsTypes] AS TABLE(
	[SiteId] [nvarchar](256) NULL,
	[TankId] [nvarchar](256) NULL,
	[StorageId] [nvarchar](256) NULL,
	[Level] [decimal](18, 2) NULL,
	[Ullage] [real] NULL,
	[GrossVolume] [real] NULL,
	[NetVolume] [real] NULL,
	[WaterNetLevel] [real] NULL,
	[WaterGrossLevel] [real] NULL,
	[CaptureTime] [datetime] NULL,
	[ProductName] [nvarchar](max) NULL,
	[DataSourceTypeId] [int] NULL,
	[SupplierId] [int] NULL,
	[SourceFileId] [bigint] NULL,
	[IsProcessed] [bit] NULL,
	[DipTestValue] [real] NULL,
	[DipTestUoM] [int] NULL
)
GO

CREATE   PROCEDURE [dbo].[usp_InsertIntoDemands]
 @TableDemands DemandsTypes READONLY
AS
BEGIN
	SET NOCOUNT ON
	insert into Demands select * from @TableDemands 
	select @@ROWCOUNT
END
GO