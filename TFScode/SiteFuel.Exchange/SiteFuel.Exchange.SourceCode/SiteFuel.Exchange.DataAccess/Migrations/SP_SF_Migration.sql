CREATE TYPE [dbo].[SearchTypes] AS TABLE(
	[SearchVar] [varchar](100) NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:        Kailash Saini
-- Create date:    13 Sep 2017
-- Description:    Calculates and returns the distance based on Latitude and Longitude
-- =============================================
CREATE FUNCTION [dbo].[usf_CalculateDistance]
(
   @CompanyLatitude    FLOAT,
   @CompanyLongitude    FLOAT,
   @JobLatitude        FLOAT,
   @JobLongitude        FLOAT
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @DegToRad    FLOAT = 57.29577951
    DECLARE @Ans        FLOAT = 0
    DECLARE @Miles        FLOAT = 0
    IF(@CompanyLatitude IS NULL OR @CompanyLatitude = 0 OR @CompanyLongitude IS NULL OR @CompanyLongitude = 0 OR @JobLatitude IS NULL OR @JobLatitude = 0 OR @JobLongitude IS NULL OR @JobLongitude = 0)
    BEGIN
        RETURN (@Miles)
    END
    SET @Ans = SIN(@CompanyLatitude / @DegToRad) * SIN(@JobLatitude / @DegToRad) + COS(@CompanyLatitude / @DegToRad ) * COS( @JobLatitude / @DegToRad ) * COS(ABS(@JobLongitude - @CompanyLongitude ) / @DegToRad)
	
	SET @Ans = ROUND(@Ans, 6)
    
	SET @Miles = ABS(3959 * ATAN(SQRT(1 - SQUARE(@Ans)) / @Ans))
    
	RETURN (@Miles)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rohan Koshti
-- Create date: 21-Mar-2018
-- Description:	Returns total number of STATUSES which are received for invoice
-- =============================================
CREATE FUNCTION [dbo].[usf_CheckInvoiceHasAnyReceivedStatus]
(
	@InvoiceId INT
)
RETURNS INT
AS
BEGIN
	Declare @ReceivedStatusCount INT
	Declare @ReceivedStatus INT
	SET @ReceivedStatus = 2
	select @ReceivedStatusCount = count(*) from Invoices Inv 
	inner join InvoiceXInvoiceStatusDetails InvStatus on Inv.Id = InvStatus.InvoiceId
	where InvStatus.StatusId = @ReceivedStatus 
	and Inv.Id = @InvoiceId
	RETURN @ReceivedStatusCount
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Sravanthi Pirikiti
-- Create date: 16-Sep-2017
-- Description:	Returns true if current user is approval person for that invoice
-- =============================================
CREATE FUNCTION [dbo].[usf_CheckInvoiceWorkflow]
(@UserId	INT,
@IsBuyerAdmin BIT, 
@InvoiceNumberId	INT,
@ApprovalUserId	INT,
@StatusId	INT)
RETURNS BIT
AS
BEGIN
	DECLARE @IsApprovalPerson	BIT = 1

	IF(@StatusId = 8 AND @ApprovalUserId <> @UserId AND @IsBuyerAdmin = 0)
	BEGIN
	SET @IsApprovalPerson = 0
	END
	IF(@StatusId = 4)
	BEGIN
	SET @IsApprovalPerson = CASE WHEN EXISTS(SELECT 1 FROM Invoices I INNER JOIN InvoiceXInvoiceStatusDetails IXS ON I.Id = IXS.InvoiceId WHERE InvoiceNumberId = @InvoiceNumberId AND IXS.StatusId = 2) THEN 1
	ELSE (CASE WHEN @ApprovalUserId <> @UserId AND @IsBuyerAdmin = 0 THEN 0 ELSE 1 END) END  
	END

	RETURN @IsApprovalPerson
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 23-Nov-2017
-- Description:	Returns list of ids of blacklisted company
-- =============================================
CREATE FUNCTION [dbo].[usf_GetBlacklistedCompanyIds]
(
	@CompanyId	INT
)
RETURNS 
@BlacklistedIds TABLE 
(
	CompanyId	INT
)
AS
BEGIN
	INSERT INTO @BlacklistedIds
	SELECT	CompanyId 
	FROM	dbo.CompanyBlacklists
	WHERE	RemovedBy IS NULL
			AND
			AddedByCompanyId = @CompanyId
	--Supplier who blacklisted buyers, so get all these suppliers
	INSERT INTO @BlacklistedIds
	SELECT	AddedByCompanyId 
	FROM	dbo.CompanyBlacklists
	WHERE	RemovedBy IS NULL
			AND
			CompanyId = @CompanyId
	RETURN 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kailash Saini
-- Create date: 14-Sep-2017
-- Description:	Returns companyIds for brokered chain of fuel request
-- =============================================
CREATE FUNCTION [dbo].[usf_GetBrokeredChain]
(
	@FuelRequestId	INT
)
RETURNS 
@BrokeredChain TABLE 
(
	CompanyId	INT
)
AS
BEGIN
	DECLARE @CompanyId	INT

	WHILE @FuelRequestId IS NOT NULL
	BEGIN
		SELECT	@CompanyId = USR.CompanyId, --UXC.CompanyId,
				@FuelRequestId = FRQ.ParentId
		FROM	[dbo].[FuelRequests] FRQ
		INNER JOIN [dbo].[Users] USR ON FRQ.CreatedBy = USR.Id
		--INNER JOIN [dbo].[UserXCompanies] UXC ON USR.Id = UXC.UserId
		WHERE	FRQ.Id = @FuelRequestId

		IF(@CompanyId IS NOT NULL)
		BEGIN
			INSERT INTO @BrokeredChain VALUES(@CompanyId)
		END
	END
	
	RETURN 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 29-Dec-2017
-- Description:	Returns brokered fuel request id of the order
-- =============================================
CREATE FUNCTION [dbo].[usf_GetBrokeredFuelRequestId]
(
	@OrderId		INT,
	@FuelRequestId	INT
)
RETURNS INT
AS
BEGIN
	DECLARE @ChildRequestId INT
	SELECT	@ChildRequestId = CASE WHEN MAX(ORD.Id) IS NULL THEN ISNULL(MAX(FRQ.Id), 0) ELSE 0 END
	FROM
	(
		SELECT	C_FRQ.Id
		FROM	dbo.FuelRequests P_FRQ, dbo.FuelRequests C_FRQ
		WHERE	P_FRQ.Id = C_FRQ.ParentId 
				AND
				C_FRQ.FuelRequestTypeId = 3
				AND
				P_FRQ.Id = @FuelRequestId
	) AS FRQ
	LEFT JOIN
	(
		SELECT	C_ORD.Id, C_ORD.FuelRequestId
		FROM	dbo.Orders P_ORD, dbo.Orders C_ORD
		WHERE	P_ORD.Id = C_ORD.ParentId
				AND
				P_ORD.Id = @OrderId
	) AS ORD
	ON FRQ.Id = ORD.FuelRequestId
	RETURN @ChildRequestId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Rohan Koshti>
-- Create date: <27-03-2018>
-- Description:	<Get Buyer and Supplier Status of latest counter offer for a fuel request>
-- =============================================
CREATE FUNCTION [dbo].[usf_GetCounterOfferStatus]
(	
	@FuelRequestId	INT,
	@SupplierId     INT
)
--RETURNS TABLE 
RETURNS @CounterOfferStatus TABLE 
(
    BuyerStatus int NULL,
	SupplierStatus int NULL
)
AS
BEGIN
	DECLARE @BuyerStatus	INT = 0
	DECLARE @Supplierstatus	INT = 0

	SELECT TOP 1 
			@BuyerStatus = ISNULL(BuyerStatus, 0),
			@SupplierStatus = ISNULL(SupplierStatus, 0)
	FROM	CounterOffers 
	WHERE	OriginalFuelRequestId = @FuelRequestId
			AND
			SupplierId = @SupplierId
	ORDER BY Id DESC, OriginalFuelRequestId DESC

	INSERT INTO @CounterOfferStatus VALUES(@BuyerStatus,@SupplierStatus)
RETURN;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[usf_GetFuelRequequestTotalValue]
(
	@FuelRequestId		INT,
	@GallonsNeeded		FLOAT,
	@FrStartDate		DATETIMEOFFSET,
	@FuelTypeId			INT,
	@PPG				FLOAT,
	@PricingTypeId   INT,
	@RackAvgTypeId   INT,
	@TerminalId      INT
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @returnPrice	FLOAT = 0.0
    DECLARE @rackAgvPrice	FLOAT = 0.0
	DECLARE @externalProductId INT = 0
	DECLARE @MaxEffectiveDate	DATETIMEOFFSET
	IF(@PricingTypeId=1 AND @TerminalId > 0)
	BEGIN
		SELECT @externalProductId=ExternalProductId FROM MstProductMappings WHERE ExternalTerminalId=@TerminalId AND ProductId=@FuelTypeId
		SET @MaxEffectiveDate = (SELECT MAX(EffectiveDate) FROM ExternalPricingData
								WHERE ProductId=@externalProductId AND TerminalId=@TerminalId AND EffectiveDate <= @FrStartDate)
		SET @rackAgvPrice = (SELECT TOP 1 AvgPrice FROM ExternalPricingData
								WHERE ProductId=@externalProductId AND TerminalId=@TerminalId AND EffectiveDate = @MaxEffectiveDate)
		IF(@RackAvgTypeId = 1)
		BEGIN
			SET @returnPrice = @rackAgvPrice + @PPG
		END
		ELSE IF(@RackAvgTypeId = 2)
		BEGIN
			SET @returnPrice = @rackAgvPrice - @PPG
		END
		ELSE IF(@RackAvgTypeId = 3)
		BEGIN
			SET @returnPrice = @rackAgvPrice + (@rackAgvPrice / 100 * @PPG)
		END
		ELSE IF(@RackAvgTypeId = 4)
		BEGIN
			SET @returnPrice = @rackAgvPrice - (@rackAgvPrice / 100 * @PPG)
		END
	END
	ELSE IF(@PricingTypeId=3 AND @TerminalId > 0)
	BEGIN
		DECLARE @TierPricingTypeId		INT = 1;
		DECLARE @TierRackAvgTypeTypeId	INT = 1;
		DECLARE @TierPpg				FLOAT = 1;
		SELECT TOP 1 @TierPricingTypeId = DFR.PricePerGallon, @TierRackAvgTypeTypeId = DFR.RackAvgTypeId, @TierPpg = DFR.PricePerGallon FROM DifferentFuelPrices DFR 
		--INNER JOIN FuelRequestXDifferentFuelPrices FRDRF ON FRDRF.DifferentFuelPriceId=DFR.Id
		WHERE DFR.FuelRequestId = @FuelRequestId --FRDRF.FuelRequestId=@FuelRequestId
		ORDER BY DFR.Id DESC
		SET @rackAgvPrice = @TierPpg;
		IF(@TierPricingTypeId = 1)
		BEGIN
			SELECT @externalProductId=ExternalProductId FROM MstProductMappings WHERE ExternalTerminalId=@TerminalId AND ProductId=@FuelTypeId
			SET @MaxEffectiveDate = (SELECT MAX(EffectiveDate) FROM ExternalPricingData
								WHERE ProductId=@externalProductId AND TerminalId=@TerminalId AND EffectiveDate <= @FrStartDate)
			SET @rackAgvPrice = (SELECT top 1 AvgPrice FROM ExternalPricingData
								WHERE ProductId=@externalProductId AND TerminalId=@TerminalId AND EffectiveDate = @MaxEffectiveDate)
		END
		IF(@TierRackAvgTypeTypeId = 1)
		BEGIN
			SET @returnPrice = @rackAgvPrice + @TierPpg
		END
		ELSE IF(@TierRackAvgTypeTypeId = 2)
		BEGIN
			SET @returnPrice = @rackAgvPrice - @TierPpg
		END
		ELSE IF(@TierRackAvgTypeTypeId = 3)
		BEGIN
			SET @returnPrice = @rackAgvPrice + (@rackAgvPrice / 100 * @TierPpg)
		END
		ELSE IF(@TierRackAvgTypeTypeId = 4)
		BEGIN
			SET @returnPrice = @rackAgvPrice - (@rackAgvPrice / 100 * @TierPpg)
		END
	END
	RETURN (CAST((@returnPrice * @GallonsNeeded) AS DECIMAL(18,2)))
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kailash Saini
-- Create date: 14-Sep-2017
-- Description:	Returns job details for brokered fuel requests
-- =============================================
CREATE FUNCTION [dbo].[usf_GetFuelRequestJob]
(
	-- Add the parameters for the function here
	@FuelRequestId	INT
)
RETURNS 
@JobDetail TABLE 
(
	[JobId]			INT,
	[FuelRequestId] INT,
	[CompanyId]		INT,
	[Address]		NVARCHAR(256),
	[City]			NVARCHAR(64),
	[StateId]		INT,
	[State]			NVARCHAR(64),
	[ZipCode]		NVARCHAR(8),
	[Latitude]		DECIMAL(18,8),
	[Longitude]		DECIMAL(18,8)
)
AS
BEGIN
	DECLARE @JobId		INT
	DECLARE @ParentId	INT = @FuelRequestId

	WHILE @JobId IS NULL AND @ParentId IS NOT NULL
	BEGIN
		SELECT	@JobId = FRQ.JobId,--JXF.JobId,
				@ParentId = ISNULL(FRQ.ParentId, @FuelRequestId)
		FROM	[dbo].[FuelRequests] FRQ
		--LEFT JOIN [dbo].[JobXFuelRequests] JXF ON FRQ.Id = JXF.FuelRequestId
		WHERE	FRQ.Id = @ParentId

		IF(@ParentId IS NOT NULL)
		BEGIN
			SET @FuelRequestId = @ParentId
		END
	END

	INSERT INTO @JobDetail
	SELECT	FRQ.JobId AS JobId,--JXF.JobId,
			FRQ.Id,
			JBS.CompanyId, --CXJ.CompanyId,
			JBS.[Address],
			JBS.City,
			JBS.StateId,
			MST.Code,
			JBS.ZipCode,
			JBS.Latitude,
			JBS.Longitude
	FROM	[dbo].[FuelRequests] FRQ
	--LEFT JOIN [dbo].[JobXFuelRequests] JXF ON FRQ.Id = JXF.FuelRequestId
	INNER JOIN [dbo].[Jobs] JBS ON FRQ.JobId = JBS.Id --JXF.JobId = JBS.Id
	INNER JOIN [dbo].[MstStates] MST ON JBS.StateId = MST.Id
	--INNER JOIN [dbo].[CompanyXJobs] CXJ ON JBS.Id = CXJ.JobId
	WHERE	FRQ.Id = @FuelRequestId

	RETURN 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 08-Jan-2018
-- Description:	Returns total fees applicable to the invoice specified
-- =============================================
CREATE FUNCTION [dbo].[usf_GetInvoiceTotalFees]
(
	@InvoiceId INT
)
RETURNS DECIMAL(18, 8)
AS
BEGIN
	DECLARE @TotalFees DECIMAL(18, 8) = 0
	;WITH ASSETDROP AS
	(
		SELECT	OrderId,
				InvoiceId,
				JobXAssetId,
				SUM(DroppedGallons) AS DroppedGallons,
				SUM(DATEDIFF(S, DropStartDate, DropEndDate)) AS TotalSeconds
		FROM	AssetDrops
		WHERE	InvoiceId = @InvoiceId
		GROUP BY OrderId,
				InvoiceId,
				JobXAssetId
	),
	FEES AS
	(
		SELECT	IXF.InvoiceId,
				FRF.FeeTypeId,
				FRF.FeeSubTypeId,
				(
					CASE WHEN FRF.FeeSubTypeId = 4 THEN AVG(FRF.Fee) * COUNT(ASD.JobXAssetId)
						 WHEN FRF.FeeSubTypeId = 5 THEN (AVG(FRF.Fee) * ISNULL(SUM(TotalSeconds), DATEDIFF(S, MIN(INV.DropStartDate), MIN(INV.DropEndDate))) / 3600)
						 WHEN FRF.FeeTypeId = 1 AND FRF.FeeSubTypeId = 3 THEN 0
						 WHEN FRF.FeeTypeId = 4 OR FRF.FeeSubTypeId = 1 THEN 0
						 ELSE AVG(FRF.Fee) END
				) AS Fee
		FROM	dbo.Invoices INV
		INNER JOIN dbo.InvoiceXFees IXF ON INV.Id = IXF.InvoiceId 
		INNER JOIN dbo.FuelRequestFees FRF ON IXF.FuelRequestFeeId = FRF.Id
		LEFT JOIN ASSETDROP ASD ON IXF.InvoiceId = ASD.InvoiceId AND ASD.DroppedGallons > 0
		WHERE INV.Id = @InvoiceId
		GROUP BY IXF.InvoiceId,
				FRF.FeeTypeId,
				FRF.FeeSubTypeId
	)
	SELECT @TotalFees = ISNULL(SUM(Fee), 0) FROM FEES
	SELECT @TotalFees += ISNULL(SUM(FBQ.Fee), 0)
	FROM	dbo.Invoices INV
	INNER JOIN dbo.InvoiceXFees IXF ON INV.Id = IXF.InvoiceId 
	INNER JOIN dbo.FuelRequestFees FRF ON IXF.FuelRequestFeeId = FRF.Id AND FRF.FeeTypeId = 1 AND FRF.FeeSubTypeId = 3
	--INNER JOIN dbo.InvoiceXFeeByQuantities IXFQ ON INV.Id = IXFQ.InvoiceId
	INNER JOIN dbo.FeeByQuantities FBQ ON FBQ.InvoiceId = INV.Id --IXFQ.FeeByQuantityId = FBQ.Id 
	AND INV.DroppedGallons BETWEEN FBQ.MinQuantity AND ISNULL(FBQ.MaxQuantity, INV.DroppedGallons)
	WHERE INV.Id = @InvoiceId
	GROUP BY IXF.InvoiceId,
			FRF.FeeTypeId,
			FRF.FeeSubTypeId,
			FBQ.Fee
	RETURN @TotalFees
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		sravanthi p
-- Create date: 28-05-2018
-- Description:	Returns order status in case of broker cancel scenario 
-- =============================================
CREATE FUNCTION [dbo].[usf_GetOrderStatusForBuyer]
(
	@Order	INT,
	@OrderStatus INT
)
RETURNS NVARCHAR(64)
AS
BEGIN
	DECLARE @Status NVARCHAR(64)
	DECLARE @FuelRequest INT
	DECLARE @ParentOrder INT
	WHILE @Order IS NOT NULL 
	BEGIN
		SELECT @FuelRequest = FR.Id FROM Orders O INNER JOIN FuelRequests FR ON O.FuelRequestId = FR.ParentId WHERE O.Id = @Order
		IF (@FuelRequest IS NOT NULL)
		BEGIN
		SELECT TOP(1) @ParentOrder = Id FROM Orders WHERE FuelRequestId = @FuelRequest ORDER BY ID DESC
			IF @ParentOrder IS NOT NULL OR @ParentOrder <> @Order 
			BEGIN
				IF EXISTS(SELECT 1 FROM Orders O INNER JOIN OrderXStatuses OXS ON O.Id = OXS.OrderId WHERE O.Id = @ParentOrder AND OXS.IsActive =1 AND OXS.StatusId = 1)
				BEGIN
				SET @Status = 'Open'
				BREAK
				END
				ELSE
				BEGIN
				SET @Order = @ParentOrder
				END
			END
			ELSE
			BREAK;
		END
		ELSE
		BREAK;
	END
	RETURN CASE WHEN @Status IS NOT NULL THEN @Status ELSE (CASE WHEN @OrderStatus = 4 THEN 'Canceled' ELSE 'Closed' END) END
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[usf_GetOriginalParentFuelRequest]
(
	@FuelRequestId	INT
	
)
RETURNS INT
AS
BEGIN
DECLARE @MainParentId INT;
	With FuelRequestRec AS (
select * from FuelRequests Where id = @FuelRequestId
UNION ALL
SELECT FR.*
From FuelRequests FR Inner Join 
FuelRequestRec FRR ON FR.Id = FRR.ParentId   
)
 Select  @MainParentId = Id from FuelRequestRec  Where ParentId IS NULL
 RETURN @MainParentId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		sravanthi pirikiti
-- Create date:	24-05-2018
-- Description:	get price per gallon
-- =============================================
CREATE FUNCTION [dbo].[usf_GetPricePerGallon]
(
    @PricePerGallon	DECIMAL(18,8),
	@PricingTypeId INT,
	@RackAvgTypeId INT
)
RETURNS NVARCHAR(64)
 
AS
BEGIN
	DECLARE @OutPut AS NVARCHAR(64)
	IF @PricingTypeId = 3
	BEGIN
		SELECT @OutPut = 'Tier'
	END
	ELSE IF @PricingTypeId = 2
	BEGIN
		SELECT @OutPut = '$' + FORMAT(@PricePerGallon, 'N2') 
	END
	ELSE
	BEGIN
		SELECT @OutPut = CASE  @RackAvgTypeId WHEN  1 THEN 'Rack + $' +  FORMAT(@PricePerGallon, 'N2') 
			 WHEN 2 THEN 'Rack - $' +  FORMAT(@PricePerGallon, 'N2')  
			 WHEN 3 THEN 'Rack + ' +  FORMAT(@PricePerGallon, 'N2') + '%'
			ELSE 'Rack - ' +  FORMAT(@PricePerGallon, 'N2') + '%'  END
	END
	RETURN @OutPut
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 29-Dec-2017
-- Description:	Returns the qualifications of specified fuel request
-- =============================================
CREATE FUNCTION [dbo].[usf_GetSupplierQualifications]
(
	@FuelRequest	INT
)
RETURNS NVARCHAR(256)
AS
BEGIN
	DECLARE @Qualifications NVARCHAR(256)
	SELECT	@Qualifications = COALESCE(@Qualifications + ', ', '') + SUBSTRING(MSQ.[Name], 1, 1)
	FROM	dbo.FuelRequestXSupplierQualifications  FXSQ
	INNER JOIN dbo.MstSupplierQualifications MSQ ON FXSQ.SupplierQualificationId = MSQ.Id
	WHERE	FuelRequestId = @FuelRequest ORDER BY MSQ.Id
	RETURN @Qualifications
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sandip>
-- Create date: <20 Feb 2018>
-- Description:	<Get time in DD HH mm format>
-- Test : SELECT [dbo].[usf_GetTimeInHhMmFormat](420)
-- =============================================
CREATE FUNCTION [dbo].[usf_GetTimeInHhMmFormat]
(
	@minutes INT
)
RETURNS NVARCHAR(64)
AS
BEGIN
	IF(@minutes=0)
	BEGIN
		RETURN '--'
	END
	-- Declare the return variable here
	DECLARE @MinutePart INT;
	DECLARE @HourPart INT;
	DECLARE @DayPart INT;
	DECLARE @Response NVARCHAR(64) ='';
	SET @MinutePart = @minutes % 60;
	SET @HourPart = @minutes/60;
	SET @DayPart = @minutes/1440; -- 24hrs * 60mins
	IF(@DayPart > 0)
	BEGIN
		SET @Response = CONCAT(@DayPart, 'day(s)')
		SET @HourPart = (@minutes - (@DayPart*1440))/60
	END
	IF(@HourPart > 0)
	BEGIN
		SET @Response = CONCAT(@Response,' ', @HourPart, 'hr(s)')
	END
	IF(@MinutePart > 0)
	BEGIN
		SET @Response = CONCAT(@Response, ' ', @MinutePart, 'min(s)')
	END
	RETURN LTRIM(@Response)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 20-Sep-2017
-- Description:	Returns job-ids of the specified user based on their role
-- =============================================
CREATE FUNCTION [dbo].[usf_GetUserJobIds]
(
	@UserId	INT
)
RETURNS 
@UserJobs TABLE 
(
	JobId	INT
)
AS
BEGIN
	DECLARE @AdminRoleId INT = (SELECT TOP 1 RoleId FROM dbo.UserXRoles WHERE UserId = @UserId AND RoleId = 2)	--Admin
	DECLARE @BuyerRoleId INT = (SELECT TOP 1 RoleId FROM dbo.UserXRoles WHERE UserId = @UserId AND RoleId = 3)	--Buyer
	DECLARE @OnsiteRoleId INT = (SELECT TOP 1 RoleId FROM dbo.UserXRoles WHERE UserId = @UserId AND RoleId = 6)--Onsite
	DECLARE @AccountingRoleId INT = (SELECT TOP 1 RoleId FROM dbo.UserXRoles WHERE UserId = @UserId AND RoleId = 7)--Accounting
	DECLARE @ReportingRoleId INT = (SELECT TOP 1 RoleId FROM dbo.UserXRoles WHERE UserId = @UserId AND RoleId = 8)--Reporting
	
	IF(@AdminRoleId = 2)
	BEGIN
		INSERT INTO @UserJobs
		--SELECT CXJ.JobId FROM dbo.CompanyXJobs CXJ
		--INNER JOIN dbo.UserXCompanies UXC ON CXJ.CompanyId = UXC.CompanyId
		--WHERE UXC.UserId = @UserId
		SELECT JBS.Id FROM dbo.Jobs JBS
		INNER JOIN dbo.Users USR ON JBS.CompanyId = USR.CompanyId
		WHERE USR.Id = @UserId
	END
	ELSE IF(@BuyerRoleId = 3 OR @OnsiteRoleId = 6 OR @AccountingRoleId = 7 OR @ReportingRoleId = 8)
	BEGIN
		INSERT INTO @UserJobs
		SELECT UXJ.JobId FROM dbo.UserXJobs UXJ
		WHERE UXJ.UserId = @UserId
	END
	RETURN 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kailash Saini
-- Create date: 16-Sep-2017
-- Description:	Returns true if counter offer is available for the fuel request
-- =============================================
CREATE FUNCTION [dbo].[usf_IsCounterOfferAvailable]
(
	@FuelRequestId	INT,
	@SupplierId		INT = 0
)
RETURNS BIT
AS
BEGIN
	DECLARE @IsCounterOffer	BIT = 0
	DECLARE @CounterOfferCount	INT
	DECLARE @ParentId INT
	DECLARE @BuyerCompanyId INT = 
	(
		SELECT TOP 1 USR.CompanyId 
		FROM dbo.FuelRequests FRQ
		--INNER JOIN dbo.UserXCompanies BUXC ON FRQ.CreatedBy = BUXC.UserId
		INNER JOIN dbo.Users USR ON FRQ.CreatedBy = USR.Id
		WHERE FRQ.Id = @FuelRequestId
	)
	DECLARE @CounterOffers  TABLE
	(
		CounterOfferId	INT,
		ParentId		INT,
		SupplierId		INT
	)
	WHILE @FuelRequestId IS NOT NULL
	BEGIN
		INSERT INTO @CounterOffers
		SELECT	FR.Id,
				FR.ParentId,
				CO.SupplierId
		FROM	dbo.FuelRequests FR
		INNER JOIN dbo.FuelRequestXStatuses FXS ON FR.Id = FXS.FuelRequestId AND FXS.IsActive = 1
		INNER JOIN dbo.CounterOffers CO ON FR.Id = CO.FuelRequestId
		--INNER JOIN dbo.UserXCompanies SUXC ON CO.SupplierId = SUXC.UserId
		INNER JOIN dbo.Users SUSR ON CO.SupplierId = SUSR.Id
		WHERE	FR.IsActive = 1 AND FXS.StatusId = 2
				AND
				FR.FuelRequestTypeId = 2
				AND 
				FR.ParentId = @FuelRequestId
				AND 
				(	@SupplierId = 0
					OR
					CO.SupplierId = @SupplierId
				)
				AND
				SUSR.CompanyId NOT IN (SELECT CompanyId FROM dbo.usf_GetBlacklistedCompanyIds(@BuyerCompanyId))
		SET @FuelRequestId = (SELECT TOP 1 CounterOfferId FROM @CounterOffers WHERE CounterOfferId > @FuelRequestId)
	END
	SET @CounterOfferCount = (SELECT COUNT(*) FROM @CounterOffers)
	IF(@CounterOfferCount > 0)
	BEGIN
		SET @IsCounterOffer = 1
	END
	RETURN @IsCounterOffer
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rohan Koshti
-- Create date: 25-Jan-2018
-- Description:	Returns true if counter offer is declined by buyer for the fuel request
-- =============================================
CREATE FUNCTION [dbo].[usf_IsCounterOfferDeclinedByBuyer]
(
	@FuelRequestId	INT,
	@SupplierId     INT
)
RETURNS BIT
AS
BEGIN
	DECLARE @IsCounterOfferDeclinedByBuyer	BIT = 0
	DECLARE @LatestCounterOfferStatus	INT
	DECLARE @ParentId INT
	DECLARE @BuyerCompanyId INT = 
	(
		SELECT TOP 1 BUXC.CompanyId 
		FROM dbo.FuelRequests FRQ
		--INNER JOIN dbo.UserXCompanies BUXC ON FRQ.CreatedBy = BUXC.UserId
		INNER JOIN dbo.Users BUXC ON FRQ.CreatedBy = BUXC.Id
		WHERE FRQ.Id = @FuelRequestId
	)
	DECLARE @CounterOffers  TABLE
	(
		CounterOfferId	INT,
		ParentId		INT,
		SupplierId		INT,
		BuyerStatus     INT,
		SupplierStatus  INT
	)
	WHILE @FuelRequestId IS NOT NULL
	BEGIN
		INSERT INTO @CounterOffers
		SELECT	FR.Id,
				FR.ParentId,
				CO.SupplierId,
				CO.BuyerStatus,
				CO.SupplierStatus
		FROM	dbo.FuelRequests FR
		INNER JOIN dbo.FuelRequestXStatuses FXS ON FR.Id = FXS.FuelRequestId AND FXS.IsActive = 1
		INNER JOIN dbo.CounterOffers CO ON FR.Id = CO.FuelRequestId
		--INNER JOIN dbo.UserXCompanies SUXC ON CO.SupplierId = SUXC.UserId
		INNER JOIN dbo.Users SUXC ON CO.SupplierId = SUXC.Id
		WHERE	FR.IsActive = 1 AND FXS.StatusId = 2
				AND
				FR.FuelRequestTypeId = 2
				AND 
				CO.SupplierId = @SupplierId
				AND 
				FR.ParentId = @FuelRequestId
				AND
				SUXC.CompanyId NOT IN (SELECT CompanyId FROM dbo.usf_GetBlacklistedCompanyIds(@BuyerCompanyId))
		SET @FuelRequestId = (SELECT TOP 1 CounterOfferId FROM @CounterOffers WHERE CounterOfferId > @FuelRequestId)
	END
	SET @LatestCounterOfferStatus = (SELECT top 1 BuyerStatus FROM @CounterOffers order by CounterOfferId desc)
	IF(@LatestCounterOfferStatus = 3)
	BEGIN
		SET @IsCounterOfferDeclinedByBuyer = 1
	END
	RETURN @IsCounterOfferDeclinedByBuyer
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sravanthi
-- Create date: 21-Feb-2018
-- Description:	Returns counter offer accepted company id
-- =============================================
CREATE FUNCTION [dbo].[usf_IsSupplierAcceptedCounterOffer]
(
	@FuelRequestId	INT,
	@BuyerId INT,
	@SupplierCompanyId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @CounterOfferId INT
	DECLARE @SupplierId INT
	DECLARE @BuyerCompanyId INT
	DECLARE @Output INT
	SELECT @BuyerCompanyId = U.CompanyId FROM [dbo].Users U 
		--INNER JOIN [dbo].UserXCompanies UXC ON UXC.UserId = U.Id
		WHERE U.Id = @BuyerId
	WHILE EXISTS (SELECT 1 FROM [FuelRequests] FR WHERE  ParentId = @FuelRequestId)
	BEGIN
		SELECT	@CounterOfferId = FRQ.Id
		FROM	[dbo].[FuelRequests] FRQ
		INNER JOIN [dbo].Users U ON U.Id = FRQ.CreatedBy
		--INNER JOIN [dbo].UserXCompanies UXC ON UXC.UserId = U.Id
		INNER JOIN [dbo].Users UXC ON UXC.Id = U.Id
		WHERE	FRQ.ParentId = @FuelRequestId AND UXC.CompanyId IN (@SupplierCompanyId, @BuyerCompanyId)
		SET @FuelRequestId = @CounterOfferId
	END
	SELECT @SupplierId = O.AcceptedCompanyId
	FROM FuelRequests FR JOIN Orders O 
	ON FR.Id = O.FuelRequestId AND FR.FuelRequestTypeId = 2
	WHERE FR.Id = @CounterOfferId 
	
	IF(@SupplierId IS NULL OR @SupplierId <> @SupplierCompanyId)
	BEGIN
		SET @Output = 0
	END
	ELSE
	BEGIN
		SET @Output = 1
	END
	RETURN @Output
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 14-Sep-2016
-- Description:	Returns true if supplier address qualification is matched to the fuel request
-- =============================================
CREATE FUNCTION [dbo].[usf_IsSupplierQualificationMatched]
(
	@FuelRequestId	INT,
	@AddressId		INT
)
RETURNS BIT
AS
BEGIN
	DECLARE @QualificationCount INT
	DECLARE @QualificationMatched BIT = 0

	;WITH Qualifications AS
	(
		SELECT FRXQ.SupplierQualificationId AS QID FROM [dbo].[FuelRequestXSupplierQualifications] FRXQ WHERE FRXQ.FuelRequestId = @FuelRequestId
		EXCEPT
		SELECT SAXQ.QualificationId  AS QID FROM [dbo].[SupplierAddressXQualifications] SAXQ WHERE SAXQ.AddressId = @AddressId
	)
	
	SELECT @QualificationCount = COUNT(*) FROM Qualifications

	IF(@QualificationCount = 0)
	BEGIN
		SET @QualificationMatched = 1
	END

	RETURN @QualificationMatched
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetTerminalPricesForCalculator]  
	@PricingDate		DATETIME= NULL,  
	@ExternalProductId	INT		= 0,  
	@SrcLatitude		FLOAT	= 0,  
	@SrcLongitude		FLOAT	= 0,  
	@StateId			INT		= 0,  
	@RecordCount		INT		= 5  
AS  
BEGIN TRY  
	DECLARE @ErrorMessage NVARCHAR(MAX)
	;WITH PRICING_IDENTITY AS
	(
		SELECT	EPD.AvgPrice,
				EPD.EffectiveDate,
				EPD.TerminalId
		FROM	ExternalPricingData EPD
		WHERE	ProductId = @ExternalProductId
				AND EffectiveDate IN
				(
					SELECT  MAX(EP.EffectiveDate)
					FROM ExternalPricingData EP
					WHERE (EP.EffectiveDate) <= @PricingDate
					AND
					EP.TerminalId = EPD.TerminalId
					AND
					EP.ProductId = @ExternalProductId
					GROUP BY
					EP.ProductId, EP.TerminalId
				)
	)
  
	SELECT TOP (@RecordCount)
		ET.Id AS TerminalId,
		ET.Abbreviation AS Abbreviation,
		ET.[Name] AS [Name],
		ET.City AS City,
		ET.ZipCode AS ZipCode,
		ST.Code AS StateCode,
		PE.AvgPrice AS Price,
		dbo.usf_CalculateDistance(@SrcLatitude, @SrcLongitude, ET.Latitude, ET.Longitude) AS Distance,
		PE.EffectiveDate AS PricingDate
	FROM MstExternalTerminals ET
	INNER JOIN PRICING_IDENTITY PE ON ET.Id = PE.TerminalId
	INNER JOIN MstStates ST ON ET.StateId = ST.Id
	ORDER BY Distance ASC
END TRY
BEGIN CATCH
    SELECT @ErrorMessage = ERROR_MESSAGE()
	RAISERROR (@ErrorMessage, 1, 1)
END CATCH
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Description:	Loads pricing data into ExternalPricingData table of SiteFuel database
-- NOTE: THIS STORED PROCEDURE IS USING LINKED SERVER INSTANCE. SO PLEASE DO REMEMBER
-- =============================================

CREATE PROCEDURE [dbo].[SyncExternalPricingData]
	@StartDate	DATETIME	=	NULL,
	@EndDate	DATETIME	=	NULL
AS
BEGIN

	DECLARE @ReturnValue INT = 0

	--This is for testing purpose only, comment it in actual
	--SET @StartDate = DATEADD(day, -10, GETDATE())

	-- Check that user has provided  @StartDate, If not, then set @StartDate as max date from ExternalPricingData table
	IF(@StartDate IS NULL)
	BEGIN
		SET @StartDate = (SELECT MAX(EffectiveDate) FROM [dbo].[ExternalPricingData])
		IF @StartDate IS NULL OR @StartDate = ''
		BEGIN
			--[SITEFUEL.PRICING.DATA]
			SET @StartDate = (SELECT TOP 1 Effective_Date FROM [SiteFuel-Axxis].[dbo].[Price_History] ORDER BY Effective_Date ASC )
		END
	END

	-- Check that user has provided @EndDate, If not, then set @EndDate as system's current date-time
	IF(@EndDate IS NULL)
	BEGIN
		SET @EndDate = GETDATE()
	END
	
	-- Get last date-time where the pricing data was loaded to start loading afterwards
	DECLARE @LastPricingDate DATETIME = (SELECT MAX(EffectiveDate) FROM [dbo].[ExternalPricingData])

	-- Insert data into ExternalPricingData table
	INSERT INTO	ExternalPricingData([TerminalId],[TerminalAbbreviation],[ProductId],[ProductCode],[AvgPrice],[EffectiveDate])
	SELECT		ET.Id,
				PH.TerminalAbbr,
				PH.ProductID,
				EP.Code,
				AVG(Price_Gross),
				PH.Effective_Date  
	-- MAKE SURE THAT LINKED SERVER [SITEFUEL.PRICING.DATA] NAME IS CORRECT HERE. IF YOU USE OTHER SERVER, THEN CHANGE THE LINKED SERVER NAME HERE
	FROM		[SiteFuel-Axxis].[dbo].[Price_History] PH
	JOIN		MstExternalTerminals ET 
	ON			PH.TerminalAbbr COLLATE DATABASE_DEFAULT = ET.[Abbreviation]
	JOIN		MstExternalProducts EP
	ON			PH.ProductID = EP.Id
	WHERE		PH.Effective_Date > @StartDate AND PH.Effective_Date<=@EndDate
	GROUP BY	ET.Id, PH.TerminalAbbr, PH.ProductID, EP.Code, PH.Effective_Date
	ORDER BY	PH.Effective_Date

	SET @ReturnValue = @@ROWCOUNT
	
	SELECT @ReturnValue 
END


GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TestStoredProc]          
(    
 @PhoneType INT,    
 @PhoneNumber NVARCHAR
)     
AS           

BEGIN
	SELECT 
		--CONCAT('Mobile', '-', @PhoneNumber) AS TypeOfPhone,  
		'Mobile' AS TypeOfPhone,  
		1234 AS Number,
		GETDATE() AS TodaysDate
END







GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 13-Dec-2017
-- Description:	Returns axxis audit report of specified date range
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAxxisAuditReport]
	@StartDate	DATETIME,
	@EndDate	DATETIME
AS
BEGIN
	;WITH INVOICES_CTE AS
	(
		SELECT
			INV.Id AS InvoiceId,
			INV.OrderId,
			INV.TerminalId,
			INV.CreatedDate
	FROM	dbo.Invoices INV
	WHERE	INV.InvoiceVersionStatusId = 1
			AND
			INV.OrderId IS NOT NULL
			AND
			INV.ParentId IS NULL
			AND
			CAST(INV.CreatedDate AS DATETIME) BETWEEN @StartDate AND @EndDate
	)
	SELECT	INV.CreatedDate AS DeliveryDate,
			MET.Code AS TerminalId,
			MET.[Name] AS TerminalName,
			MEP.Code AS ProductId,
			MEP.[Name] AS ProductName
	FROM	INVOICES_CTE INV
	INNER JOIN dbo.Orders ORD ON ORD.Id = INV.OrderId
	INNER JOIN dbo.FuelRequests FRQ ON FRQ.Id = ORD.FuelRequestId
	INNER JOIN dbo.MstExternalTerminals MET ON MET.Id = INV.TerminalId
	INNER JOIN dbo.MstProductMappings MPM ON MPM.ProductId = FRQ.FuelTypeId AND MPM.ExternalTerminalId = INV.TerminalId
	INNER JOIN dbo.MstExternalProducts MEP ON MPM.ExternalProductId = MEP.Id
	ORDER BY INV.CreatedDate DESC
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:  Kailash Saini 
-- Create date: 21-Sep-2017 
-- Modified By: Rohan 
-- Description: Returns all fuel requests of the buyer company 
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetBuyerFuelRequests]
  @CompanyId INT, 
  @UserId    INT, 
  @Broadcast INT = 3, -- All FuelRequests 
  @StatusId  INT = 0, -- All Statuses 
  @JobId     INT = 0, -- All Jobs 
  @StartDate DATETIMEOFFSET(7), 
  @EndDate DATETIMEOFFSET(7), 
  @isCallFromDashboard BIT = 0, 
  @GlobalSearchText    VARCHAR(30) = NULL, 
  @SortId              INT = 0, 
  @SortDirection       VARCHAR(6) = 'desc', 
  @PageSize            INT = 99999999, 
  @PageNumber          INT = 1, 
  @FuelRequestNumberSearchTypes [dbo].SEARCHTYPES Readonly , 
  @JobNameSearchTypes [dbo].SEARCHTYPES Readonly , 
  @AddressSearchTypes [dbo].SEARCHTYPES Readonly , 
  @FuelTypeSearchTypes [dbo].SEARCHTYPES Readonly , 
  @GallonsSearchTypes [dbo].SEARCHTYPES Readonly , 
  @PriceSearchTypes [dbo].SEARCHTYPES Readonly , 
  @ContactSearchTypes [dbo].SEARCHTYPES Readonly , 
  @StatusSearchTypes [dbo].SEARCHTYPES Readonly , 
  @CounterOfferSearchTypes [dbo].SEARCHTYPES Readonly 
AS 
  BEGIN 
    DECLARE @FuelRequestNumberSearchTypesValid INT 
    SET @FuelRequestNumberSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @FuelRequestNumberSearchTypes) 
    DECLARE @JobNameSearchTypesValid INT 
    SET @JobNameSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @JobNameSearchTypes) 
    DECLARE @AddressSearchTypesValid INT 
    SET @AddressSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @AddressSearchTypes) 
    DECLARE @FuelTypeSearchTypesValid INT 
    SET @FuelTypeSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @FuelTypeSearchTypes) 
    DECLARE @GallonsSearchTypesValid INT 
    SET @GallonsSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @GallonsSearchTypes) 
    DECLARE @PriceSearchTypesValid INT 
    SET @PriceSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @PriceSearchTypes) 
    DECLARE @StatusSearchTypesValid INT 
    SET @StatusSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @StatusSearchTypes) 
	DECLARE @ContactSearchTypesValid INT 
    SET @ContactSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @ContactSearchTypes) 
    DECLARE @CounterOfferSearchTypesValid INT 
    SET @CounterOfferSearchTypesValid = 
    ( 
           SELECT Count(*) 
           FROM   @CounterOfferSearchTypes) 
    IF(@isCallFromDashboard=1) 
    BEGIN 
      SET @PageSize = 10 
      SET @GlobalSearchText = 'Open' 
    END ; 
    WITH BuyerFuelRequests AS 
    ( 
               SELECT     FRQ.Id AS FuelRequestId, 
                          FRQ.RequestNumber, 
                          FRQ.CreatedDate, 
                          JBS.[Name] AS JobName, 
                          JBS.Id     AS JobId, 
                          JBS.[Address], 
                          JBS.City, 
                          MST.Code AS StateCode, 
                          JBS.ZipCode, 
                          MST.[Name]      AS StateName, 
                          MPD.[Name]      AS FuelType, 
                          FRQ.MaxQuantity AS Gallons, 
                          dbo.[usf_GetPricePerGallon](FRQ.PricePerGallon, FRQ.PricingTypeId, FRQ.RackAvgTypeId) AS PricePerGallon, 
                          CASE WHEN ISNULL(U.[FirstName] + ' ' + U.[LastName],'') = '' THEN '--' ELSE U.[FirstName] + ' ' + U.[LastName] + ' - '+ U.PhoneNumber+ ' - '+U.Email END AS ContactName, 
                          FXS.StatusId, 
                          FRS.[Name]                                 AS [Status], 
                          dbo.usf_IsCounterOfferAvailable(FRQ.Id, 0) AS IsCounterOfferAvailable
               FROM       dbo.FuelRequests FRQ 
               INNER JOIN dbo.FuelRequestXDeliveryDetails FRD 
               ON         FRQ.Id = FRD.FuelRequestId 
               INNER JOIN dbo.FuelRequestXStatuses FXS 
               ON         FRQ.Id = FXS.FuelRequestId 
               AND        FXS.IsActive = 1 
               INNER JOIN dbo.MstProducts MPD 
               ON         FRQ.FuelTypeId = MPD.Id 
               INNER JOIN dbo.MstFuelRequestStatuses FRS 
               ON         FXS.StatusId = FRS.Id 
               --INNER JOIN dbo.JobXFuelRequests JXF 
               --ON         FRQ.Id = JXF.FuelRequestId 
               INNER JOIN dbo.Jobs JBS 
               ON         FRQ.JobId = JBS.Id 
               --INNER JOIN dbo.CompanyXJobs CXJB 
               --ON         JBS.Id = CXJB.JobId 
               INNER JOIN dbo.MstStates MST 
               ON         JBS.StateId = MST.Id 
               --INNER JOIN dbo.JobXJobBudgets JXB 
               --ON         JBS.Id = JXB.JobId 
               INNER JOIN dbo.JobBudgets JBT 
               ON         JBS.JobBudgetId = JBT.Id 
			   INNER JOIN dbo.usf_GetUserJobIds(@UserId) IDS
			   ON IDS.JobId = JBS.Id
               LEFT JOIN  dbo.JobXOnsiteContactPersons JXC 
               ON         JBS.Id = JXC.JobId 
               LEFT JOIN  dbo.Users U 
               ON         JXC.UserId = U.Id 
               WHERE      FRQ.IsActive = 1 
               AND        JBS.CompanyId = @CompanyId 
               AND        ( ( 
                                                @JobId > 0) 
                          OR         ( 
                                                @JobId = 0 
                                     AND        ( ( 
                                                                      FRD.StartDate >= @StartDate
                                                           AND        FRD.StartDate < @EndDate)
                                                OR         ( 
                                                                      FRQ.CreatedDate >= @StartDate
                                                           AND        FRQ.CreatedDate < @EndDate) ) ) )
               AND        ( ( 
                                                FRQ.FuelRequestTypeId = 2 
                                     AND        FXS.StatusId = 3) 
                          OR         ( 
                                                FRQ.FuelRequestTypeId = 1 
                                     AND        FXS.StatusId != 6) ) 
               AND        ( 
                                     JXC.UserId IS NULL 
                          OR         JXC.UserId IN 
                                     ( 
                                              SELECT   Min(JCP.UserId) 
                                              FROM     dbo.JobXOnsiteContactPersons JCP 
                                              GROUP BY JCP.JobId) ) 
               AND        (  @JobId = 0 
                                     OR         JBS.Id = @JobId)                            
               AND        ( 
                                     @StatusId = 0 
                          OR         FXS.StatusId = @StatusId ) 
               AND        ( 
                                     @Broadcast = 3 
                          OR         FRQ.IsPublicRequest = ( 
                                     CASE 
                                                WHEN @Broadcast = 1 THEN 0 
                                                ELSE 1 
                                     END) ) 
               ) 
    SELECT   *,						  [TotalCount]= COUNT(FuelRequestId) OVER() 
    FROM     BuyerFuelRequests 
    WHERE    ( 
                      @FuelRequestNumberSearchTypesValid = 0 
             OR      ( 
                               @FuelRequestNumberSearchTypesValid > 0 
                      AND      RequestNumber IN 
                               ( 
                                      SELECT RequestNumber 
                                      FROM   @FuelRequestNumberSearchTypes 
                                      WHERE  RequestNumber IN (SearchVar)))) 
    AND      ( 
                      @JobNameSearchTypesValid = 0 
             OR      ( 
                               @JobNameSearchTypesValid > 0 
                      AND      JobName IN 
                               ( 
                                      SELECT JobName 
                                      FROM   @JobNameSearchTypes 
                                      WHERE  JobName IN (SearchVar)))) 
    AND      ( 
                      @AddressSearchTypesValid = 0 
             OR      ( 
                               @AddressSearchTypesValid > 0 
                      AND      ( 
                                        [Address] +' '+ City +' '+ [StateCode] +' '+ ZipCode) IN 
                               ( 
                                      SELECT [Address] +' '+ City +' '+ [StateCode] +' '+ ZipCode
                                      FROM   @AddressSearchTypes 
                                      WHERE  ( 
                                                    [Address] +' '+ City +' '+ [StateCode] +' '+ ZipCode) LIKE '%' + SearchVar + '%')))
    AND      ( 
                      @FuelTypeSearchTypesValid = 0 
             OR      ( 
                               @FuelTypeSearchTypesValid > 0 
                      AND      FuelType IN 
                               ( 
                                      SELECT FuelType 
                                      FROM   @FuelTypeSearchTypes 
                                      WHERE  FuelType IN (SearchVar)))) 
    AND      ( 
                      @GallonsSearchTypesValid = 0 
             OR      ( 
                               @GallonsSearchTypesValid > 0 
                      AND      Gallons IN 
                               ( 
                                      SELECT Gallons 
                                      FROM   @GallonsSearchTypes 
                                      WHERE  Cast(Gallons AS VARCHAR(100)) LIKE '%' + SearchVar + '%')))
    AND      ( 
                      @PriceSearchTypesValid = 0 
             OR      ( 
                               @PriceSearchTypesValid > 0 
                      AND      PricePerGallon IN 
                               ( 
                                      SELECT PricePerGallon 
                                      FROM   @PriceSearchTypes 
                                      WHERE  PricePerGallon IN (SearchVar))))
    AND      ( 
                      @ContactSearchTypesValid = 0 
             OR      ( 
                               @ContactSearchTypesValid > 0 
                      AND      ContactName IN 
                               ( 
                                      SELECT ContactName 
                                      FROM   @ContactSearchTypes 
                                      WHERE  ContactName IN (SearchVar)))) 
    AND      ( 
                      @CounterOfferSearchTypesValid = 0 
             OR      ( 
                               @CounterOfferSearchTypesValid > 0 
                      AND      IsCounterOfferAvailable IN 
                               ( 
                                      SELECT IsCounterOfferAvailable 
                                      FROM   @CounterOfferSearchTypes 
                                      WHERE  IsCounterOfferAvailable LIKE '%' + SearchVar + '%')))
    AND      ( 
                      @StatusSearchTypesValid = 0 
             OR      ( 
                               @StatusSearchTypesValid > 0 
                      AND      [Status] IN 
                               ( 
                                      SELECT [Status] 
                                      FROM   @StatusSearchTypes 
                                      WHERE  [Status] LIKE '%' + SearchVar + '%'))) 
    AND      ( 
                      @GlobalSearchText IS NULL 
             OR       (( 
                                        RequestNumber LIKE '%' + @GlobalSearchText+ '%') 
						OR				( 
                                        JobName LIKE '%' + @GlobalSearchText+ '%')
                      OR       (( 
                                                 [Address] +' '+ City +' '+ [StateCode] +' '+ ZipCode) LIKE '%' + @GlobalSearchText+ '%')
                      OR       ( 
                                        Gallons LIKE '%' + @GlobalSearchText+ '%') 
                      OR       ( 
                                        PricePerGallon LIKE '%' + @GlobalSearchText+ '%') 
					  OR       ( 
                                        FuelType LIKE '%' + @GlobalSearchText+ '%') 
                      OR       ( 
                                        ContactName LIKE '%' + @GlobalSearchText+ '%') 
                      OR       ( 
                                        IsCounterOfferAvailable LIKE '%' + @GlobalSearchText+ '%')
                      OR       ( 
                                        [Status] LIKE '%' + @GlobalSearchText+ '%')) ) 
    ORDER BY 
             CASE 
                      WHEN @SortId = 4 
                      AND      @SortDirection = 'asc' THEN Gallons 
             END ASC, 
			 CASE 
			          WHEN @SortId = 0 
                      AND      @SortDirection = 'asc' THEN RequestNumber 
					  WHEN @SortId = 1 
                      AND      @SortDirection = 'asc' THEN JobName
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'asc' THEN FuelType 
					  WHEN @SortId = 5 
                      AND      @SortDirection = 'asc' THEN PricePerGallon 
                      WHEN @SortId = 6 
                      AND      @SortDirection = 'asc' THEN ContactName 
                      WHEN @SortId = 7 
                      AND      @SortDirection = 'asc' THEN [Status] 
					  WHEN @SortId = 2 
                      AND      @SortDirection = 'asc' THEN ZipCode 
             END ASC, 
			 CASE                       
					  WHEN @SortId = 8 
                      AND      @SortDirection = 'asc' THEN IsCounterOfferAvailable 
			 END ASC, 
             CASE 
                      WHEN @SortId = 4 
                      AND      @SortDirection = 'desc' THEN Gallons 
             END DESC, 
			 CASE
			          WHEN @SortId = 0 
                      AND      @SortDirection = 'desc' THEN RequestNumber
					  WHEN @SortId = 1 
                      AND      @SortDirection = 'desc' THEN JobName 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'desc' THEN FuelType 
					  WHEN @SortId = 5 
                      AND      @SortDirection = 'desc' THEN PricePerGallon 
                      WHEN @SortId = 6 
                      AND      @SortDirection = 'desc' THEN ContactName 
                      WHEN @SortId = 7 
                      AND      @SortDirection = 'desc' THEN [Status] 
					  WHEN @SortId = 2 
                      AND      @SortDirection = 'desc' THEN ZipCode 
             END DESC,
			 CASE 
					  WHEN @SortId = 8 
                      AND      @SortDirection = 'desc' THEN IsCounterOfferAvailable 
			 END DESC OFFSET (@PageNumber -1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY 
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sravanthi Pirikiti
-- Create date: 01-Jun-2018
-- Description:	Returns buyer invoices of specified company
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetBuyerInvoices]
	@CompanyId		INT,
	@UserId			INT,
	@IsBuyerAdmin	BIT,
	@JobId			INT,
	@InvoiceTypeId	INT,
	@InvoiceFilter	INT,
	@StartDate		DATETIMEOFFSET(7),
	@EndDate		DATETIMEOFFSET(7),
	@GlobalSearchText VARCHAR(30) = NULL, 
    @SortId           INT = 0, 
    @SortDirection    VARCHAR(6) = 'desc', 
    @PageSize         INT = 99999999, 
    @PageNumber       INT = 1, 
	@InvoiceNumberSearchTypes [dbo].SEARCHTYPES Readonly,
    @PoNumberSearchTypes [dbo].SEARCHTYPES Readonly, 
	@JobSearchTypes [dbo].SEARCHTYPES Readonly, 
    @SupplierSearchTypes [dbo].SEARCHTYPES Readonly, 
    @FuelTypeSearchTypes [dbo].SEARCHTYPES Readonly, 
    @TerminalSearchTypes [dbo].SEARCHTYPES Readonly,
	@AssetSearchTypes [dbo].SEARCHTYPES Readonly, 
    @AmountSearchTypes [dbo].SEARCHTYPES Readonly, 
    @DropDateSearchTypes [dbo].SEARCHTYPES Readonly, 
    @DropTimeSearchTypes [dbo].SEARCHTYPES Readonly, 
	@InvoiceDateSearchTypes [dbo].SEARCHTYPES Readonly, 
	@DueDateSearchTypes [dbo].SEARCHTYPES Readonly, 
    @StatusSearchTypes [dbo].SEARCHTYPES Readonly
AS
BEGIN
    DECLARE @InvoiceNumberSearchTypesValid INT 
    SET @InvoiceNumberSearchTypesValid = ( SELECT Count(*) FROM   @InvoiceNumberSearchTypes) 
    DECLARE @PoNumberSearchTypesValid INT 
    SET @PoNumberSearchTypesValid = ( SELECT Count(*) FROM   @PoNumberSearchTypes) 
    DECLARE @SupplierSearchTypesValid INT 
    SET @SupplierSearchTypesValid = ( SELECT Count(*)  FROM   @SupplierSearchTypes) 
    DECLARE @JobSearchTypesValid INT 
    SET @JobSearchTypesValid =  ( SELECT Count(*)  FROM   @JobSearchTypes) 
    DECLARE @FuelTypeSearchTypesValid INT 
    SET @FuelTypeSearchTypesValid =  ( SELECT Count(*)  FROM   @FuelTypeSearchTypes) 
    DECLARE @TerminalSearchTypesValid INT 
    SET @TerminalSearchTypesValid =  (  SELECT Count(*)   FROM   @TerminalSearchTypes) 
    DECLARE @DropDateSearchTypesValid INT 
    SET @DropDateSearchTypesValid =  (  SELECT Count(*)   FROM   @DropDateSearchTypes) 
    DECLARE @AssetSearchTypesValid INT 
    SET @AssetSearchTypesValid =   (   SELECT Count(*)   FROM   @AssetSearchTypes) 
    DECLARE @AmountSearchTypesValid INT 
    SET @AmountSearchTypesValid = ( SELECT Count(*)  FROM   @AmountSearchTypes) 
    DECLARE @DropTimeSearchTypesValid INT 
    SET @DropTimeSearchTypesValid = (  SELECT Count(*)  FROM   @DropTimeSearchTypes) 
    DECLARE @StatusSearchTypesValid INT 
    SET @StatusSearchTypesValid =   (   SELECT Count(*)  FROM   @StatusSearchTypes) 
	DECLARE @InvoiceDateSearchTypesValid INT 
    SET @InvoiceDateSearchTypesValid =   (   SELECT Count(*)  FROM   @InvoiceDateSearchTypes) 
	DECLARE @DueDateSearchTypesValid INT 
    SET @DueDateSearchTypesValid =   (   SELECT Count(*)  FROM   @DueDateSearchTypes) 

;WITH BuyerInvoices AS (
	SELECT	INV.Id,
			INV.OrderId,
						INM.Number AS InvoiceNumber,
						JBS.Name AS JobName,
						JBS.Id AS JobId,
						ISNULL(FRQ.ExternalPoNumber, CONVERT(NVARCHAR(16), ORD.PoNumber)) AS PoNumber,
			S.Name AS Supplier,
			CASE WHEN INV.InvoiceTypeId = 5 THEN 'Dry Run Fee' ELSE PRD.[Name] END AS FuelType,			
			(
				INV.BasicAmount + 
				(CASE WHEN INV.InvoiceTypeId = 5 THEN 0 ELSE dbo.usf_GetInvoiceTotalFees(INV.Id) END) +
				(CASE WHEN (INV.InvoiceTypeId = 6 OR INV.InvoiceTypeId = 7) THEN 0 
				 ELSE INV.TotalTaxAmount END)
			)  AS InvoiceAmount,
			CONVERT(NVARCHAR(10), INV.DropEndDate, 101) AS DropDate,
			FORMAT(INV.DropStartDate,'h:mm tt') + ' - ' + FORMAT(INV.DropEndDate,'h:mm tt') AS DropTime,
			CASE WHEN IXS.StatusId = 10 THEN '--' ELSE CONVERT(NVARCHAR(10), INV.CreatedDate, 101) END AS InvoiceDate,
			CONVERT(NVARCHAR(10), INV.PaymentDueDate, 101) AS PaymentDueDate,
			CASE WHEN IXS.StatusId = 8 THEN IST.[Name] + ' - ' + A_USR.FirstName + ' ' + A_USR.LastName ELSE IST.[Name] END AS [Status],
			INV.InvoiceNumberId,
			CASE WHEN INV.TerminalId IS NULL THEN '--' ELSE MET.[Name] END AS TerminalName,
			JXA.AssetId
	FROM	dbo.Invoices INV
	INNER JOIN dbo.InvoiceNumbers INM ON INV.InvoiceNumberId = INM.Id
	INNER JOIN dbo.InvoiceXInvoiceStatusDetails IXS ON INV.Id = IXS.InvoiceId AND IXS.IsActive = 1
	INNER JOIN dbo.MstInvoiceStatuses IST ON IXS.StatusId = IST.Id
	INNER JOIN dbo.Orders ORD ON INV.OrderId = ORD.Id
	INNER JOIN dbo.FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id
	--INNER JOIN JobXFuelRequests JXF ON FRQ.Id = JXF.FuelRequestId
	INNER JOIN Jobs JBS ON FRQ.JobId = JBS.Id
	INNER JOIN dbo.Companies S ON S.Id = ORD.AcceptedCompanyId
	INNER JOIN dbo.usf_GetUserJobIds(@UserId) IDS ON IDS.JobId = FRQ.JobId
	LEFT JOIN JobXApprovalUsers A ON JBS.Id = A.JobId AND A.IsActive =1
	LEFT JOIN dbo.MstProducts PRD ON FRQ.FuelTypeId = PRD.Id
	LEFT JOIN dbo.MstExternalTerminals MET ON INV.TerminalId = MET.Id
	LEFT JOIN dbo.Users A_USR ON A.UserId = A_USR.Id
	LEFT JOIN AssetDrops AD ON INV.Id = AD.InvoiceId
	LEFT JOIN JobXAssets JXA ON AD.JobXAssetId = JXA.Id
	WHERE	INV.InvoiceVersionStatusId = 1 AND IXS.StatusId <> 10
				AND (INV.OrderId IS NOT NULL AND ORD.BuyerCompanyId = @CompanyId)
				AND
				( @JobId = 0 
                          OR         FRQ.JobId = @JobId)
			AND
			(
				(@InvoiceTypeId IN (6, 7) AND INV.InvoiceTypeId IN (6, 7))
				OR
				(@InvoiceTypeId NOT IN (6, 7) AND INV.InvoiceTypeId NOT IN (6, 7))
			)
			AND
			( INV.CreatedDate >= @StartDate AND INV.CreatedDate < @EndDate )
			AND
			(
				CASE	WHEN @InvoiceFilter = 0 THEN 1	--Invoice Filters
						WHEN @InvoiceFilter IN (2, 11, 12, 1, 6, 5, 7, 3) AND IXS.StatusId = @InvoiceFilter THEN 1
						WHEN @InvoiceFilter = 4 AND IXS.StatusId = 4 AND 
						(
							(SELECT COUNT(IXIS.StatusId) FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id) = 1 
							OR 
							2 = ANY(SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id)
						) THEN 1
						WHEN @InvoiceFilter IN (13, 14) AND 
						(
							4 = ANY(SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id AND IXIS.IsActive = 1)
							AND
							2 != ANY(SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id)
						) THEN 1
						WHEN @InvoiceFilter = 15 AND INV.ParentId IS NOT NULL THEN 1
						ELSE 0
				END
			) = 1	
			AND (FRQ.FuelRequestTypeId <> 3	OR IXS.StatusId NOT IN (8, 4) OR dbo.usf_CheckInvoiceWorkflow(@UserId, @IsBuyerAdmin, INV.InvoiceNumberId, A.UserId, IXS.StatusId) = 1)
),
BuyerFinalInvoices AS (
SELECT 
Id,
InvoiceNumber,
OrderId,
JobName,
JobId,
PoNumber,
Supplier,
FuelType,
InvoiceAmount,
DropDate,
DropTime,
InvoiceDate,
PaymentDueDate,
Status,
InvoiceNumberId,
TerminalName,
COUNT(DISTINCT AssetId) AS AssetCount
FROM BuyerInvoices GROUP BY  
Id,
InvoiceNumber,
OrderId,
JobName,
JobId,
PoNumber,
Supplier,
FuelType,
InvoiceAmount,
DropDate,
DropTime,
InvoiceDate,
PaymentDueDate,
Status,
InvoiceNumberId,
TerminalName
)

SELECT *, [TotalCount]= Count(Id) OVER() FROM BuyerFinalInvoices
WHERE    ( 
                      @InvoiceNumberSearchTypesValid = 0 
             OR       ( 
                               @InvoiceNumberSearchTypesValid > 0 
                      AND      InvoiceNumber IN 
                               ( 
                                      SELECT InvoiceNumber 
                                      FROM   @InvoiceNumberSearchTypes 
                                      WHERE  InvoiceNumber IN (SearchVar)))) 
    AND      ( 
                      @PoNumberSearchTypesValid = 0 
             OR       ( 
                               @PoNumberSearchTypesValid > 0 
                      AND      PoNumber IN 
                               ( 
                                      SELECT PoNumber 
                                      FROM   @PoNumberSearchTypes 
                                      WHERE  PoNumber IN (SearchVar)))) 
    AND      ( 
                      @SupplierSearchTypesValid = 0 
             OR       ( 
                               @SupplierSearchTypesValid > 0 
                      AND      Supplier IN 
                               ( 
                                      SELECT Supplier 
                                      FROM   @SupplierSearchTypes 
                                      WHERE  Supplier IN (SearchVar)))) 
    AND      ( 
                      @JobSearchTypesValid = 0 
             OR       ( 
                               @JobSearchTypesValid > 0 
                      AND      JobName IN 
                               ( 
                                      SELECT JobName 
                                      FROM   @JobSearchTypes 
                                      WHERE  JobName IN (SearchVar)))) 
    AND      ( 
                      @FuelTypeSearchTypesValid = 0 
             OR       ( 
                               @FuelTypeSearchTypesValid > 0 
                      AND       FuelType IN 
                                        ( 
                                               SELECT FuelType 
                                               FROM   @FuelTypeSearchTypes 
                                               WHERE  FuelType IN (SearchVar)))) 
             AND      ( 
                               @TerminalSearchTypesValid = 0 
                      OR       ( 
                                        @TerminalSearchTypesValid > 0 
                               AND      TerminalName IN 
                                        ( 
                                               SELECT TerminalName 
                                               FROM   @TerminalSearchTypes 
                                               WHERE  TerminalName IN (SearchVar)))) 
             AND      ( 
                               @DropDateSearchTypesValid = 0 
                      OR       ( 
                                        @DropDateSearchTypesValid > 0 
                               AND      DropDate IN 
                                        ( 
                                               SELECT DropDate 
                                               FROM   @DropDateSearchTypes 
                                               WHERE  DropDate IN (SearchVar)))) 
             AND      ( 
                               @AssetSearchTypesValid = 0 
                      OR       ( 
                                        @AssetSearchTypesValid > 0 
                               AND      AssetCount IN 
                                        ( 
                                               SELECT AssetCount 
                                               FROM   @AssetSearchTypes 
                                               WHERE  AssetCount IN (SearchVar)))) 
             AND      ( 
                               @AmountSearchTypesValid = 0 
                      OR       ( 
                                        @AmountSearchTypesValid > 0 
                               AND      InvoiceAmount IN 
                                        ( 
                                               SELECT InvoiceAmount 
                                               FROM   @AmountSearchTypes 
                                                WHERE  InvoiceAmount IN (REPLACE(SearchVar,'$',''))))) 
             AND      ( 
                               @DropTimeSearchTypesValid = 0 
                      OR       ( 
                                        @DropTimeSearchTypesValid > 0 
                               AND      DropTime IN 
                                        ( 
                                               SELECT DropTime 
                                               FROM   @DropTimeSearchTypes 
                                               WHERE  DropTime IN (SearchVar))))
             AND      ( 
                               @StatusSearchTypesValid = 0 
                      OR       ( 
                                        @StatusSearchTypesValid > 0 
                               AND      [Status] IN 
                                        ( 
                                               SELECT [Status] 
                                               FROM   @StatusSearchTypes 
                                               WHERE  [Status] IN (SearchVar)))) 
			 AND      ( 
                               @InvoiceDateSearchTypesValid = 0 
                      OR       ( 
                                        @InvoiceDateSearchTypesValid > 0 
                               AND      InvoiceDate IN 
                                        ( 
                                               SELECT InvoiceDate 
                                               FROM   @InvoiceDateSearchTypes 
                                               WHERE  InvoiceDate IN (SearchVar)))) 
			AND      ( 
                               @DueDateSearchTypesValid = 0 
                      OR       ( 
                                        @DueDateSearchTypesValid > 0 
                               AND      PaymentDueDate IN 
                                        ( 
                                               SELECT PaymentDueDate 
                                               FROM   @DueDateSearchTypes 
                                               WHERE  PaymentDueDate IN (SearchVar)))) 
             AND      ( 
                               @GlobalSearchText IS NULL 
                      OR       (
									( InvoiceNumber LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 PoNumber LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 JobName LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 Supplier LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 FuelType LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 TerminalName LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 DropDate LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 AssetCount LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 InvoiceAmount LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 DropTime LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 [Status] LIKE '%' + @GlobalSearchText+ '%')
							   OR       ( 
                                                 InvoiceDate LIKE '%' + @GlobalSearchText+ '%')
							   OR       ( 
                                                 PaymentDueDate LIKE '%' + @GlobalSearchText+ '%')
							  ) )
    ORDER BY 
			CASE      WHEN @SortId = 6 
                      AND      @SortDirection = 'asc' THEN AssetCount 					  
					  END ASC,
			CASE      WHEN @SortId = 7
                      AND      @SortDirection = 'asc' THEN InvoiceAmount 					  
					  END ASC,
             CASE 
                      WHEN @SortId = 8 
                      AND      @SortDirection = 'asc' THEN cast(DropDate As Datetime) 
                      WHEN @SortId = 10 
                      AND      @SortDirection = 'asc' THEN cast(InvoiceDate As Datetime) 
					  WHEN @SortId = 11
					  AND      @SortDirection = 'asc' THEN cast(PaymentDueDate As Datetime) 
             END ASC, 
             CASE 
                      WHEN @SortId = 0 
                      AND      @SortDirection = 'asc' THEN InvoiceNumber 
                      WHEN @SortId = 1 
                      AND      @SortDirection = 'asc' THEN PoNumber 
                      WHEN @SortId = 2 
                      AND      @SortDirection = 'asc' THEN JobName 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'asc' THEN Supplier 
					  WHEN @SortId = 4 
                      AND      @SortDirection = 'asc' THEN FuelType
					  WHEN @SortId = 5 
                      AND      @SortDirection = 'asc' THEN TerminalName 
                      WHEN @SortId = 9 
                      AND      @SortDirection = 'asc' THEN DropTime
                      WHEN @SortId = 12 
                      AND      @SortDirection = 'asc' THEN [Status] 
             END ASC, 
			CASE      WHEN @SortId = 6 
                      AND      @SortDirection = 'desc' THEN AssetCount 					  
					  END DESC,
			CASE      WHEN @SortId = 7
                      AND      @SortDirection = 'desc' THEN InvoiceAmount 					  
					  END DESC,
             CASE 
                      WHEN @SortId = 8 
                      AND      @SortDirection = 'desc' THEN cast(DropDate As Datetime) 
                      WHEN @SortId = 10 
                      AND      @SortDirection = 'desc' THEN cast(InvoiceDate As Datetime) 
					  WHEN @SortId = 11
					  AND      @SortDirection = 'desc' THEN cast(PaymentDueDate As Datetime) 
             END DESC, 
             CASE 
                      WHEN @SortId = 0 
                      AND      @SortDirection = 'desc' THEN InvoiceNumber 
                      WHEN @SortId = 1 
                      AND      @SortDirection = 'desc' THEN PoNumber 
                      WHEN @SortId = 2 
                      AND      @SortDirection = 'desc' THEN JobName 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'desc' THEN Supplier 
					  WHEN @SortId = 4 
                      AND      @SortDirection = 'desc' THEN FuelType
					  WHEN @SortId = 5 
                      AND      @SortDirection = 'desc' THEN TerminalName 
                      WHEN @SortId = 9 
                      AND      @SortDirection = 'desc' THEN DropTime
                      WHEN @SortId = 12 
                      AND      @SortDirection = 'desc' THEN [Status] 
             END DESC OFFSET (@PageNumber -1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY 
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:    Sravanthi Pirikiti 
-- Create date: 25-05-2018 
-- Description:  Returns buyer orders of the specified company 
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetBuyerOrders] 
  @CompanyId  INT, 
  @UserId     INT, 
  @JobId      INT = 0,-- All Jobs 
  @OrderId    INT = 0,-- All Orders 
  @Filter     INT = 0,-- All Orders 
  @FuelTypeId INT = 0,-- All Orders 
  @StartDate DATETIMEOFFSET(7), 
  @EndDate DATETIMEOFFSET(7), 
  @GlobalSearchText VARCHAR(30) = NULL, 
  @SortId           INT = 0, 
  @SortDirection    VARCHAR(6) = 'desc', 
  @PageSize         INT = 99999999, 
  @PageNumber       INT = 1, 
  @PoNumberSearchTypes [dbo].SEARCHTYPES Readonly, 
  @SupplierSearchTypes [dbo].SEARCHTYPES Readonly, 
  @EligibilitySearchTypes [dbo].SEARCHTYPES Readonly, 
  @FuelTypeSearchTypes [dbo].SEARCHTYPES Readonly, 
  @AssetsSearchTypes [dbo].SEARCHTYPES Readonly, 
  @QuantitySearchTypes [dbo].SEARCHTYPES Readonly, 
  @PriceSearchTypes [dbo].SEARCHTYPES Readonly, 
  @AmountSearchTypes [dbo].SEARCHTYPES Readonly, 
  @DeliverSearchTypes [dbo].SEARCHTYPES Readonly, 
  @StatusSearchTypes [dbo].SEARCHTYPES Readonly 
AS 
  BEGIN 
    DECLARE @PoNumberSearchTypesValid INT 
    SET @PoNumberSearchTypesValid = ( SELECT Count(*) FROM   @PoNumberSearchTypes) 
    DECLARE @SupplierSearchTypesValid INT 
    SET @SupplierSearchTypesValid = ( SELECT Count(*)  FROM   @SupplierSearchTypes) 
    DECLARE @EligibilitySearchTypesValid INT 
    SET @EligibilitySearchTypesValid =  ( SELECT Count(*)  FROM   @EligibilitySearchTypes) 
    DECLARE @FuelTypeSearchTypesValid INT 
    SET @FuelTypeSearchTypesValid =  ( SELECT Count(*)  FROM   @FuelTypeSearchTypes) 
    DECLARE @AssetsSearchTypesValid INT 
    SET @AssetsSearchTypesValid =  (  SELECT Count(*)   FROM   @AssetsSearchTypes) 
    DECLARE @QuantitySearchTypesValid INT 
    SET @QuantitySearchTypesValid =  (  SELECT Count(*)   FROM   @QuantitySearchTypes) 
    DECLARE @PriceSearchTypesValid INT 
    SET @PriceSearchTypesValid =   (   SELECT Count(*)   FROM   @PriceSearchTypes) 
    DECLARE @AmountSearchTypesValid INT 
    SET @AmountSearchTypesValid = ( SELECT Count(*)  FROM   @AmountSearchTypes) 
    DECLARE @DeliverSearchTypesValid INT 
    SET @DeliverSearchTypesValid = (  SELECT Count(*)  FROM   @DeliverSearchTypes) 
    DECLARE @StatusSearchTypesValid INT 
    SET @StatusSearchTypesValid =   (   SELECT Count(*)  FROM   @StatusSearchTypes) 
   ;
    WITH BuyerOrders AS 
    ( 
               SELECT     O.Id, 
                          ISNULL(FR.ExternalPoNumber, O.PoNumber)                AS PoNumber, 
                          S.NAME                                                 AS Supplier, 
                          ISNULL(dbo.usf_GetSupplierQualifications(FR.Id), '--') AS Eligibility, 
                          P.NAME                                                 AS FuelType, 
                          OT.NAME                                                AS [Type], 
                          CASE 
                                     WHEN FR.PricingTypeId <> 2 THEN NULL 
                                     ELSE (FR.MaxQuantity * FR.PricePerGallon)
                          END                                                                                AS TotalAmount,
                          FR.MaxQuantity                                                      AS Quantity,
                          dbo.[usf_GetPricePerGallon](FR.PricePerGallon, FR.PricingTypeId, FR.RackAvgTypeId) AS PricePerGallon, 
                          Cast( 
                                ( 
                                SELECT ISNULL(( Sum(INV.DroppedGallons) / FR.MaxQuantity * 100 ), 0)
                                FROM   dbo.Invoices INV 
                                WHERE  INV.InvoiceVersionStatusId = 1 
                                AND    INV.IsActive = 1 
                                AND    INV.OrderId = O.Id) AS INT) AS FuelDeliveredPercentage, 
                          J.Id                                     AS JobId, 
                          A.Id                                     AS AssetId, 
                          CASE 
                                     WHEN OXS.StatusId IN ( 4, 
                                                           5 ) THEN dbo.usf_GetOrderStatusForBuyer(o.Id, oxs.StatusId)
                                     ELSE MOS.NAME 
                          END AS [Status] 
               FROM       dbo.Orders O 
               INNER JOIN OrderXStatuses OXS 
               ON         O.Id = OXS.OrderId 
               INNER JOIN MstOrderStatuses MOS 
               ON         OXS.StatusId = MOS.Id 
               INNER JOIN dbo.FuelRequests FR 
               ON         O.FuelRequestId = FR.Id 
               --INNER JOIN JobXFuelRequests JXF 
               --ON         FR.Id = JXF.FuelRequestId 
               INNER JOIN Jobs J 
               ON         J.Id = FR.JobId 
               INNER JOIN Companies S 
               ON         O.AcceptedCompanyId = S.Id 
               INNER JOIN Users U 
               ON         O.AcceptedBy = U.Id 
               INNER JOIN MstProducts P 
               ON         FR.FuelTypeId = P.Id 
               INNER JOIN MstOrderTypes OT 
               ON         FR.OrderTypeId = OT.Id 
			   INNER JOIN dbo.usf_GetUserJobIds(@UserId) IDS
			   ON IDS.JobId = FR.JobId
               LEFT JOIN  JobXAssets JXA 
               ON         JXA.JobId = J.Id 
               LEFT JOIN  Assets A 
               ON         A.Id = JXA.AssetId 
               AND        A.IsActive = 1 
               AND        JXA.RemovedBy IS NULL 
               WHERE      O.BuyerCompanyId = @CompanyId 
               AND        O.IsActive = 1 
               AND        OXS.IsActive = 1 
               AND        O.ParentId IS NULL 
               AND        ( 
                                     @FuelTypeId = 0 
                          OR         FR.FuelTypeId = @FuelTypeId ) 
               AND        ( 
                                     (@JobId = 0 
                          OR         FR.JobId = @JobId)) 
               AND        ( 
                                     @Filter = 0 
                          OR         OXS.StatusId = @Filter ) 
               AND        O.AcceptedDate >= @StartDate 
               AND        O.AcceptedDate <= @EndDate),
    FinalOrders AS 
    ( 
             SELECT   Id, 
                      PoNumber, 
                      Supplier, 
                      Eligibility, 
                      FuelType, 
                      Type, 
                      Count(AssetId) AS AssetsAssigned, 
                      TotalAmount, 
                      Quantity, 
                      PricePerGallon, 
                      FuelDeliveredPercentage, 
                      Status 
             FROM     BuyerOrders 
             GROUP BY Id, 
                      PoNumber, 
                      Supplier, 
                      Eligibility, 
                      FuelType, 
                      Type, 
                      Quantity, 
                      PricePerGallon, 
                      TotalAmount, 
                      FuelDeliveredPercentage, 
                      Status, 
                      JobId ) 
    SELECT   *, 
             [TotalCount]= Count(Id) OVER() 
    FROM     FinalOrders 
    WHERE    ( 
                      @PoNumberSearchTypesValid = 0 
             OR       ( 
                               @PoNumberSearchTypesValid > 0 
                      AND      PoNumber IN 
                               ( 
                                      SELECT PoNumber 
                                      FROM   @PoNumberSearchTypes 
                                      WHERE  PoNumber IN (SearchVar)))) 
    AND      ( 
                      @SupplierSearchTypesValid = 0 
             OR       ( 
                               @SupplierSearchTypesValid > 0 
                      AND      Supplier IN 
                               ( 
                                      SELECT Supplier 
                                      FROM   @SupplierSearchTypes 
                                      WHERE  Supplier IN (SearchVar)))) 
    AND      ( 
                      @EligibilitySearchTypesValid = 0 
             OR       ( 
                               @EligibilitySearchTypesValid > 0 
                      AND      Eligibility IN 
                               ( 
                                      SELECT Eligibility 
                                      FROM   @EligibilitySearchTypes 
                                      WHERE  Eligibility IN (SearchVar)))) 
    AND      ( 
                      @FuelTypeSearchTypesValid = 0 
             OR       ( 
                               @FuelTypeSearchTypesValid > 0 
                      AND       FuelType IN 
                                        ( 
                                               SELECT FuelType 
                                               FROM   @FuelTypeSearchTypes 
                                               WHERE  FuelType IN (SearchVar)))) 
             AND      ( 
                               @AssetsSearchTypesValid = 0 
                      OR       ( 
                                        @AssetsSearchTypesValid > 0 
                               AND      AssetsAssigned IN 
                                        ( 
                                               SELECT AssetsAssigned 
                                               FROM   @AssetsSearchTypes 
                                               WHERE  AssetsAssigned IN (SearchVar)))) 
             AND      ( 
                               @QuantitySearchTypesValid = 0 
                      OR       ( 
                                        @QuantitySearchTypesValid > 0 
                               AND      Quantity IN 
                                        ( 
                                               SELECT Quantity 
                                               FROM   @QuantitySearchTypes 
                                               WHERE  Quantity IN (SearchVar)))) 
             AND      ( 
                               @PriceSearchTypesValid = 0 
                      OR       ( 
                                        @PriceSearchTypesValid > 0 
                               AND      PricePerGallon IN 
                                        ( 
                                               SELECT PricePerGallon 
                                               FROM   @PriceSearchTypes 
                                               WHERE  PricePerGallon IN (SearchVar)))) 
             AND      ( 
                               @AmountSearchTypesValid = 0 
                      OR       ( 
                                        @AmountSearchTypesValid > 0 
                               AND      TotalAmount IN 
                                        ( 
                                               SELECT TotalAmount 
                                               FROM   @AmountSearchTypes 
                                                WHERE  TotalAmount = (CASE WHEN SearchVar = '--' THEN NULL ELSE REPLACE(SearchVar,'$','') END)))) 
             AND      ( 
                               @DeliverSearchTypesValid = 0 
                      OR       ( 
                                        @DeliverSearchTypesValid > 0 
                               AND      FuelDeliveredPercentage IN 
                                        ( 
                                               SELECT FuelDeliveredPercentage 
                                               FROM   @DeliverSearchTypes 
                                               WHERE  FuelDeliveredPercentage LIKE '%' + SearchVar + '%')))
             AND      ( 
                               @StatusSearchTypesValid = 0 
                      OR       ( 
                                        @StatusSearchTypesValid > 0 
                               AND      [Status] IN 
                                        ( 
                                               SELECT [Status] 
                                               FROM   @StatusSearchTypes 
                                               WHERE  [Status] IN (SearchVar)))) 
             AND      ( 
                               @GlobalSearchText IS NULL 
                      OR       (( 
                                                 PoNumber LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 Eligibility LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 Supplier LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 FuelType LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 AssetsAssigned LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 Quantity LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 PricePerGallon LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 FuelDeliveredPercentage LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 TotalAmount LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 [Status] LIKE '%' + @GlobalSearchText+ '%')) )
    ORDER BY 
			CASE      WHEN @SortId = 5 
                      AND      @SortDirection = 'asc' THEN Quantity 
					  WHEN @SortId = 7 
                      AND      @SortDirection = 'asc' THEN TotalAmount 
					  END ASC,
             CASE 
                      WHEN @SortId = 4 
                      AND      @SortDirection = 'asc' THEN AssetsAssigned 
                      WHEN @SortId = 8 
                      AND      @SortDirection = 'asc' THEN FuelDeliveredPercentage 
					  WHEN @SortId = 10
					  AND      @SortDirection = 'asc' THEN Id
             END ASC, 
             CASE 
                      WHEN @SortId = 0 
                      AND      @SortDirection = 'asc' THEN PoNumber 
                      WHEN @SortId = 1 
                      AND      @SortDirection = 'asc' THEN Supplier 
                      WHEN @SortId = 2 
                      AND      @SortDirection = 'asc' THEN Eligibility 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'asc' THEN FuelType 
                      WHEN @SortId = 6 
                      AND      @SortDirection = 'asc' THEN PricePerGallon 
                      WHEN @SortId = 9 
                      AND      @SortDirection = 'asc' THEN [Status] 
             END ASC, 
			CASE      WHEN @SortId = 5 
                      AND      @SortDirection = 'desc' THEN Quantity 
					  WHEN @SortId = 7 
                      AND      @SortDirection = 'desc' THEN TotalAmount 
					  END DESC,
             CASE 
                      WHEN @SortId = 4 
                      AND      @SortDirection = 'desc' THEN AssetsAssigned 
                      WHEN @SortId = 8 
                      AND      @SortDirection = 'desc' THEN FuelDeliveredPercentage 
					  WHEN @SortId = 10
					  AND      @SortDirection = 'desc' THEN Id
             END DESC, 
             CASE 
                      WHEN @SortId = 0 
                      AND      @SortDirection = 'desc' THEN PoNumber 
                      WHEN @SortId = 1 
                      AND      @SortDirection = 'desc' THEN Supplier 
                      WHEN @SortId = 2 
                      AND      @SortDirection = 'desc' THEN Eligibility 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'desc' THEN FuelType 
                      WHEN @SortId = 6 
                      AND      @SortDirection = 'desc' THEN PricePerGallon 
                      WHEN @SortId = 9 
                      AND      @SortDirection = 'desc' THEN [Status] 
             END DESC OFFSET (@PageNumber -1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY 
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetBuyerOrdersForDashboard]
	@CompanyId				INT,
	@JobId					INT,
	@CountOfActiveOrders	INT = 5
AS
BEGIN
	DECLARE @TotalOrders	INT
	DECLARE @OpenOrders		INT
	DECLARE @ClosedOrders	INT
	DECLARE @CancelledOrders INT
	DECLARE @IsActiveStatus INT
	DECLARE @OrderCancelledStatus INT
	DECLARE @OrderClosedStatus INT
	DECLARE @EntityTypeOrder INT
	SET @OpenOrders = 0
	SET @TotalOrders = 0
	SET @ClosedOrders = 0
	SET @CancelledOrders = 0
	SET @IsActiveStatus = 1
	SET @OrderCancelledStatus = 3
	SET @OrderClosedStatus = 2
	SET @EntityTypeOrder = 2
	SELECT ord.id as orderId,BuyerCompanyId,ord.FuelRequestId into #TempBuyerOrders 
	FROM Orders ord
	--inner join JobXFuelRequests jobfr on jobfr.FuelRequestId = ord.FuelRequestId
	inner join FuelRequests jobfr on jobfr.Id = ord.FuelRequestId
	where (@JobId= 0 or jobfr.JobId = @JobId) and BuyerCompanyId = @CompanyId AND ord.ParentId IS NULL
	
	SELECT @TotalOrders = COUNT(*) FROM #TempBuyerOrders
	
	SELECT @OpenOrders = COUNT(*) FROM #TempBuyerOrders ord INNER JOIN 
	[dbo].[OrderXStatuses] ordStatus on ord.orderId = ordStatus.OrderId
	WHERE ordStatus.IsActive = @IsActiveStatus and ordStatus.StatusId = @IsActiveStatus
	
	SELECT @ClosedOrders = COUNT(*) FROM #TempBuyerOrders ord INNER JOIN 
	[dbo].[OrderXStatuses] ordStatus on ord.orderId = ordStatus.OrderId
	WHERE ordStatus.IsActive = @IsActiveStatus and (ordStatus.StatusId = @OrderClosedStatus)
	SELECT @CancelledOrders = COUNT(*) FROM #TempBuyerOrders ord INNER JOIN 
	[dbo].[OrderXStatuses] ordStatus on ord.orderId = ordStatus.OrderId
	WHERE ordStatus.IsActive = @IsActiveStatus and (ordStatus.StatusId = @OrderCancelledStatus)
	
	;WITH Last5ActiveOrdersFROMNeewsFeed AS
	(
		SELECT TOP (@CountOfActiveOrders) EntityId
		FROM Newsfeeds NF
		INNER JOIN #TempBuyerOrders OS on os.orderId=NF.EntityId
		WHERE EntityTypeId = @EntityTypeOrder
		GROUP BY EntityId 
		ORDER BY MAX(ID) DESC
	)
	
	SELECT 0 AS Id,
	'0' as PoNumber,
	'N/A' as Supplier,
	CONVERT(NVARCHAR(10), GETDATE(), 101) AS StartDate,
	0 as InvoiceCOUNT,
	0 as FuelDeliveredPercentage,
	@TotalOrders as TotalOrders,
	@OpenOrders as OpenOrders,
	@ClosedOrders as ClosedOrders,
	@CancelledOrders as CancelledOrders
	UNION ALL
	SELECT	ORD.Id,
			ISNULL(FRQ.ExternalPoNumber, ORD.PoNumber) AS PoNumber,
			CUST.Name AS Customer,
			CONVERT(NVARCHAR(10), FDD.StartDate, 101) AS StartDate,
			(SELECT COUNT(*) FROM dbo.Invoices INV WHERE INV.InvoiceVersionStatusId = 1 AND INV.OrderId = ORD.Id) AS InvoiceCOUNT,
			CAST((SELECT ISNULL((SUM(INV.DroppedGallons)/FRQ.MaxQuantity * 100), 0) FROM dbo.Invoices INV 
			WHERE INV.InvoiceVersionStatusId = @IsActiveStatus AND INV.IsActive = @IsActiveStatus AND INV.OrderId = ORD.Id) AS DECIMAL(18,2)) AS FuelDeliveredPercentage,
			@TotalOrders as TotalOrders,
			@OpenOrders as OpenOrders,
			@ClosedOrders as ClosedOrders,
			@CancelledOrders as CancelledOrders
	FROM	dbo.Orders ORD
	INNER JOIN Last5ActiveOrdersFROMNeewsFeed LAO ON ORD.Id = LAO.EntityId
	INNER JOIN dbo.FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id
	INNER JOIN dbo.FuelRequestXDeliveryDetails FDD ON FRQ.Id = FDD.FuelRequestId
	INNER JOIN dbo.Companies CUST ON ORD.AcceptedCompanyId = CUST.Id
	DROP TABLE IF EXISTS #TempBuyerOrders
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetBuyerPerformanceData]
	@CompanyId	INT
AS
BEGIN
		SELECT Id, FuelRequestId, AcceptedCompanyId, BuyerCompanyId				
		INTO #TempSupplierOrderDetails 
		FROM Orders 
		WHERE AcceptedCompanyId=@CompanyId
		
		--BASIC COUNTS OF ORDERS
		SELECT COMP.Name AS BuyerName,
				MIN(COMP.ID) AS BuyerCompanyId,
				COUNT(distinct ORD.Id) TotalOrders, 
				CAST(ISNULL(SUM(
					CASE 
						WHEN INV.InvoiceTypeId NOT IN (6, 7)
						THEN INV.BasicAmount + INV.TotalTaxAmount + dbo.usf_GetInvoiceTotalFees(INV.Id)
						ELSE 0
						END
					),0) AS numeric(18,2)) AS 'TotalSpend',
				CAST(ISNULL(SUM(
					CASE 
						WHEN INV.InvoiceTypeId NOT IN (6, 7)
						THEN FRS.MaxQuantity
						ELSE 0
						END
				),0) AS numeric(18,2)) as TotalGallonsOrdered,
				CAST(ISNULL(SUM(
				CASE 
						WHEN INV.InvoiceTypeId NOT IN (6, 7)
						THEN INV.DroppedGallons
						ELSE 0
						END
				),0) AS numeric(18,2)) as TotalGallonsDelivered,
				CAST(ISNULL(AVG(INV.PricePerGallon),0) AS numeric(18,2)) as AveragePPG,
				SUM(CASE WHEN INV.InvoiceTypeId = 6 OR INV.InvoiceTypeId = 7 THEN 1 ELSE 0 END) AS 'TotalDDTCount',
				SUM(CASE WHEN INV.InvoiceTypeId != 6 AND INV.InvoiceTypeId != 7 THEN 1 ELSE 0 END) as 'TotalInvoiceCount'
		INTO #TempSupplierOrderInvoiceDetails
		FROM #TempSupplierOrderDetails ORD
		LEFT JOIN Invoices INV ON (ORD.Id=INV.OrderId AND INV.InvoiceVersionStatusId=1 )
		LEFT JOIN Companies COMP ON ORD.BuyerCompanyId=COMP.Id
		LEFT JOIN FuelRequests FRS ON ORD.FuelRequestId=FRS.Id
		GROUP BY COMP.Name
		ORDER BY TotalOrders DESC
		-- COUNT FOR ONLY DDT
		SELECT COMP.Name AS BuyerName,
				MIN(COMP.ID) AS BuyerCompanyId,
				CAST(ISNULL(SUM(
					CASE 
						WHEN INV.InvoiceTypeId IN (6, 7)
						THEN INV.BasicAmount + dbo.usf_GetInvoiceTotalFees(INV.Id)
						ELSE 0
						END
					),0) AS numeric(18,2)) AS 'TotalSpend',
				CAST(ISNULL(SUM(
					CASE 
						WHEN INV.InvoiceTypeId IN (6, 7)
						THEN FRS.MaxQuantity
						ELSE 0
						END
				),0) AS numeric(18,2)) as TotalGallonsOrdered,
				CAST(ISNULL(SUM(
					CASE
						WHEN INV.InvoiceTypeId IN (6, 7)
						THEN INV.DroppedGallons
						ELSE 0
						END
				),0) AS numeric(18,2)) as TotalGallonsDelivered
		INTO #TempSupplierOrderDdtDetails
		FROM #TempSupplierOrderDetails ORD
		LEFT JOIN Invoices INV ON (ORD.Id=INV.OrderId AND INV.InvoiceVersionStatusId=1)
		LEFT JOIN Companies COMP ON ORD.BuyerCompanyId=COMP.Id
		LEFT JOIN FuelRequests FRS ON ORD.FuelRequestId=FRS.Id
		WHERE INV.InvoiceTypeId IN (6,7) AND INV.Id NOT IN (SELECT ParentId FROM Invoices WHERE ParentId IS NOT NULL)
			 AND INV.IsActive=1
		GROUP BY COMP.Name
		SELECT 
			TSOID.BuyerName AS BuyerName,
			MIN(TSOID.BuyerCompanyId) AS BuyerCompanyId,
			SUM(TSOID.TotalOrders) AS 'TotalOrders',
			SUM(ISNULL(TSOID.TotalSpend,0) + ISNULL(TSODD.TotalSpend,0)) AS 'TotalSpend',
			SUM(ISNULL(TSOID.TotalGallonsOrdered,0)) AS 'TotalGallonsOrdered',
			SUM(ISNULL(TSOID.TotalGallonsDelivered,0) + ISNULL(TSODD.TotalGallonsDelivered,0)) AS 'TotalGallonsDelivered',
			SUM(TSOID.AveragePPG) AS 'AveragePPG',
			SUM(TSOID.TotalDDTCount) AS 'TotalDDTCount',
			SUM(TSOID.TotalInvoiceCount) AS 'TotalInvoiceCount'
		FROM #TempSupplierOrderInvoiceDetails TSOID
		LEFT JOIN #TempSupplierOrderDdtDetails TSODD ON TSOID.BuyerCompanyId=TSODD.BuyerCompanyId
		GROUP BY TSOID.BuyerName
		DROP TABLE IF EXISTS #TempSupplierOrderDetails, #TempSupplierOrderDdtDetails, #TempSupplierOrderInvoiceDetails
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		SANDIP SAWANT
-- Create date: 2-2-2018
-- Description:	Get completed deliveries status for specific order
-- =============================================
Create PROCEDURE [dbo].[usp_GetCompletedDeliveriesForOrder] 
	@orderId		INT,
	@PageNo			INT,
	@PageSize		INT,
	@SortId			INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		INV.Id AS InvoiceId,
		INVNO.Number,
		CONVERT(nvarchar(10), TSC.Date, 101)  AS ScheduledDate,
		CONVERT(nvarchar(10), INV.DropEndDate, 101)  AS DropDate,
		CONCAT(CONVERT(nvarchar(15),TSC.StartTime,100), ' - ', CONVERT(nvarchar(15),TSC.EndTime,100)) AS ScheduledTime,
		CONVERT(varchar(15),CAST(INV.DropStartDate AS TIME),100) + '-' + CONVERT(varchar(15),CAST(INV.DropEndDate AS TIME),100) AS DroppedTime,
		CAST(TSC.Quantity AS DECIMAL(18,2)) AS QuantityScheduled,
		CAST(INV.DroppedGallons AS DECIMAL(18,2)) AS QuanityDropped,
		Usr.FirstName + ' ' + Usr.LastName AS ScheduledDriver,
		U.FirstName + ' ' + U.LastName AS Driver,
		DSS.Name AS ScheduleStatus,
		CASE WHEN FR.IsOverageAllowed = 1 THEN 'Yes' ELSE 'No' END AS IsOverageAllowed,
		CASE WHEN CAST(TSC.Date AS date) < CAST(INV.DropEndDate AS date) THEN 1 ELSE 0 END IsDropDateLate,
		CASE WHEN TSC.EndTime < CAST(INV.DropEndDate AS Time) THEN 1 ELSE 0 END IsDropTimeLate,
		CAST(ISNULL(((INV.DroppedGallons - TSC.Quantity)/TSC.Quantity * 100), 0) AS DECIMAL(18))  AS Overage
	FROM 
		Invoices INV 
		INNER JOIN Orders O ON INV.OrderId = O.Id
		INNER JOIN FuelRequests FR ON O.FuelRequestId = FR.Id
		INNER JOIN DeliveryScheduleXTrackableSchedules TSC on INV.Id = TSC.InvoiceId
		LEFT JOIN Users Usr ON TSC.DriverId = Usr.Id 
		INNER JOIN InvoiceNumbers INVNO ON INV.InvoiceNumberId = INVNO.Id
		INNER JOIN MstDeliveryScheduleStatuses DSS ON TSC.DeliveryScheduleStatusId = DSS.Id
		LEFT JOIN Users U ON INV.DriverId = U.Id		
	WHERE 
		INV.OrderId = @orderId AND TSC.OrderId = @orderId AND INV.IsActive = 1
		AND INV.InvoiceVersionStatusId = 1
   
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rupesh Bhadane
-- Create date: 21-2-2018
-- Description:	Get completed deliveries for specific company (or driver)
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetCompletedDeliveriesForSupplier] 
	@CompanyId		INT,
	@DriverIds VARCHAR(512), 
	@StartDate      DATETIMEOFFSET,
	@EndDate        DATETIMEOFFSET,
	@PageNo			INT,
	@PageSize		INT,
	@SortId			INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	    ISNULL(FR.ExternalPoNumber,O.PoNumber) AS PONumber,
		INV.Id AS InvoiceId,
		INVNO.Number,
		O.Id AS OrderId,
		CONVERT(nvarchar(10), ISNULL(TSC.Date, FRD.StartDate), 101) AS ScheduledDate,
		CONVERT(nvarchar(10), INV.DropEndDate, 101)  AS DropDate,
		CONCAT(CONVERT(nvarchar(15),ISNULL(TSC.StartTime, FRD.StartTime),100), ' - ', CONVERT(nvarchar(15),ISNULL(TSC.EndTime,FRD.EndTime),100)) AS ScheduledTime,
		CONVERT(varchar(15),CAST(INV.DropStartDate AS TIME),100) + ' - '+CONVERT(varchar(15),CAST(INV.DropEndDate AS TIME),100) AS DroppedTime,
		CASE WHEN TSC.Id IS NULL THEN CAST((FR.MaxQuantity - ISNULL((SELECT SUM(DroppedGallons) FROM Invoices WHERE OrderId = O.Id AND InvoiceVersionStatusId = 1 AND IsActive = 1 AND Id < INV.Id),0)) AS DECIMAL(18,2))
		 ELSE CAST(TSC.Quantity AS DECIMAL(18,2)) END AS QuantityScheduled,
		CAST(INV.DroppedGallons AS DECIMAL(18,2)) AS QuanityDropped,
		CASE WHEN USR.Id IS NULL THEN (CASE WHEN OD.Id IS NULL THEN '--' ELSE ( OD.FirstName + ' ' + OD.LastName) END) ELSE ( Usr.FirstName + ' ' + Usr.LastName) END AS ScheduledDriver,
		CASE WHEN U.Id IS NULL THEN '--' ELSE U.FirstName + ' ' + U.LastName END AS Driver,
		ISNULL(DSS.Name, 'Completed') AS ScheduleStatus,
		CASE WHEN FR.IsOverageAllowed = 1 THEN 'Yes' ELSE 'No' END AS IsOverageAllowed,
		CASE WHEN TSC.Id IS NOT NULL THEN (CASE WHEN CAST(TSC.Date AS date) < CAST(INV.DropEndDate AS date) THEN 1 ELSE 0 END) ELSE 0 END IsDropDateLate,
		CASE WHEN TSC.Id IS NOT NULL THEN (CASE WHEN TSC.EndTime < CAST(INV.DropEndDate AS Time) THEN 1 ELSE 0 END) ELSE 0 END IsDropTimeLate,
		INV.DriverId,
		CASE WHEN TSC.Id IS NOT NULL THEN 'Yes' ELSE 'No' END As IsDeliverySchedule,
		C.Name AS Customer ,
		CASE WHEN TSC.Id IS NOT NULL THEN (CASE WHEN TSC.DeliveryScheduleStatusId=19 THEN 0 ELSE CAST(ISNULL(((INV.DroppedGallons - TSC.Quantity)/TSC.Quantity * 100), 0) AS DECIMAL(18)) END) ELSE 0 END AS Overage,
		TSC.Id AS TrackableScheduleId,
		ISNULL(DSS.Id,7) AS ScheduleStatusId,
		CASE WHEN INV.InvoiceTypeId IN (1,3,7) THEN 'App' ELSE 'Manual' END AS AppManual
		INTO #DropsCompleted
	FROM 
		Invoices INV 
		INNER JOIN Orders O ON INV.OrderId = O.Id
		INNER JOIN FuelRequests FR ON O.FuelRequestId = FR.Id
		INNER JOIN FuelRequestXDeliveryDetails FRD ON FR.Id = FRD.FuelRequestId
		LEFT JOIN DeliveryScheduleXTrackableSchedules TSC on INV.Id = TSC.InvoiceId
		LEFT JOIN Users Usr ON TSC.DriverId = Usr.Id 
		LEFT JOIN OrderXDrivers OXD ON O.Id = OXD.OrderId AND (OXD.IsActive IS NULL OR OXD.IsActive = 1)
		LEFT JOIN Users OD ON OXD.DriverId = OD.Id 
		INNER JOIN InvoiceNumbers INVNO ON INV.InvoiceNumberId = INVNO.Id
		LEFT JOIN MstDeliveryScheduleStatuses DSS ON TSC.DeliveryScheduleStatusId = DSS.Id
		LEFT JOIN Users U ON INV.DriverId = U.Id		
		--INNER JOIN UserXCompanies UXC ON FR.CreatedBy = UXC.UserId
		INNER JOIN Companies C ON U.CompanyId = C.Id
	WHERE 
		O.AcceptedCompanyId = @CompanyId
		AND CAST(INV.DropStartDate as date) >= @StartDate
		AND CAST(INV.DropEndDate as date) <= @EndDate
		AND INV.ParentId IS NULL
		AND INV.InvoiceVersionStatusId = 1    
	
	UPDATE #DropsCompleted SET Overage =
	     CASE WHEN QuantityScheduled = 0 THEN QuanityDropped 
		 ELSE CAST(ISNULL(((QuanityDropped - QuantityScheduled)/QuantityScheduled * 100), 0) AS DECIMAL(18)) END  
	WHERE TrackableScheduleId IS NULL
	SELECT '-' AS PoNumber, I.Id AS InvoiceId,
	N.Number,
	NULL AS OrderId,
	'--' AS ScheduledDate,
	CONVERT(nvarchar(10), I.DropEndDate, 101) AS DropDate,
	'--' AS ScheduledTime,
	CONVERT(varchar(15),CAST(I.DropStartDate AS TIME),100) + ' - '+CONVERT(varchar(15),CAST(I.DropEndDate AS TIME),100) AS DroppedTime,
	0 AS QuantityScheduled,
	CAST(I.DroppedGallons AS DECIMAL(18,2)) AS QuanityDropped,
	'--' AS ScheduledDriver,
	CASE WHEN D.Id IS NULL THEN '--' ELSE D.FirstName + ' ' + D.LastName END AS Driver,
	'--' AS ScheduleStatus,
	'--' AS IsOverageAllowed,
	0 AS IsDropDateLate,
	0 AS IsDropTimeLate,
	I.DriverId,
	'No' AS IsDeliverySchedule,
	'--' AS Customer,
	0 AS Overage,
	0 AS ScheduleStatusId,
		CASE WHEN I.InvoiceTypeId IN (1,3,7) THEN 'App' ELSE 'Manual' END AS AppManual INTO #TempUnassignedInvoices
	FROM Invoices I 
	--INNER JOIN UserXCompanies UXC ON I.CreatedBy = UXC.UserId
	INNER JOIN Users UXC ON I.CreatedBy = UXC.Id
	INNER JOIN Companies C ON C.Id = UXC.CompanyId
	INNER JOIN InvoiceNumbers N ON I.InvoiceNumberId = N.Id
	LEFT JOIN Users D ON I.DriverId = D.Id
	WHERE OrderId IS NULL AND C.Id = @CompanyId
		AND CAST(I.DropStartDate as date) >= @StartDate
		AND CAST(I.DropEndDate as date) <= @EndDate
		AND I.ParentId IS NULL
		AND I.InvoiceVersionStatusId = 1   
		AND (@DriverIds = '-1' OR DriverId IN (SELECT Value 
                                                      FROM   STRING_SPLIT(@DriverIds, ',')))
	SELECT PoNumber,InvoiceId,
	Number,
	ScheduledDate,
	DropDate,
	ScheduledTime,
	DroppedTime,
	QuantityScheduled,
	QuanityDropped,
	ScheduledDriver,
	Driver,
	ScheduleStatus,
	IsOverageAllowed,
	IsDropDateLate,
	IsDropTimeLate,
	DriverId,
	IsDeliverySchedule,
	Customer,
	Overage,
	OrderId,
	ScheduleStatusId,
	AppManual
	FROM #DropsCompleted
	WHERE @DriverIds = '-1' OR DriverId IN (SELECT Value 
                                                      FROM   STRING_SPLIT(@DriverIds, ',')) 
	UNION 
		SELECT PoNumber,InvoiceId,
	Number,
	ScheduledDate,
	DropDate,
	ScheduledTime,
	DroppedTime,
	QuantityScheduled,
	QuanityDropped,
	ScheduledDriver,
	Driver,
	ScheduleStatus,
	IsOverageAllowed,
	IsDropDateLate,
	IsDropTimeLate,
	DriverId,
	IsDeliverySchedule,
	Customer,
	Overage,
	OrderId,
	ScheduleStatusId,
	AppManual
	FROM #TempUnassignedInvoices
	ORDER BY InvoiceId DESC
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rupesh Bhadane
-- Create date: 09-Jan-2018
-- Description:	Stored procedure to get all the invoices for which delete request has been made
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetDeleteRequestedInvoices]	
	@InvoiceNumber VARCHAR(19)	
AS
BEGIN
	SELECT	INV.Id,
			ISNULL(INV.OrderId, 0) AS OrderId,
			(
				CASE WHEN FRQ.FuelRequestTypeId = 2 THEN (C_COM.[Name] + ' - ' + C_USR.FirstName + ' ' + C_USR.LastName)
					 ELSE (F_COM.[Name] + ' - ' + F_USR.FirstName + ' ' + F_USR.LastName)  END
			) AS Supplier,
			PRD.[Name] AS FuelType,
			INM.Number AS InvoiceNumber,
			ISNULL(FRQ.ExternalPoNumber, CONVERT(NVARCHAR(16), ORD.PoNumber)) AS PoNumber,
			(
				INV.BasicAmount + 
				(CASE WHEN INV.InvoiceTypeId = 5 THEN 0 ELSE dbo.usf_GetInvoiceTotalFees(INV.Id) END) +
				(CASE WHEN INV.InvoiceTypeId = 6 AND INV.InvoiceTypeId = 7 THEN 0 
				 ELSE 
					CASE WHEN INV.TotalTaxAmount > 0 THEN INV.TotalTaxAmount ELSE (INV.StateTax + INV.FedTax + INV.SalesTax) END
				 END)
			) AS InvoiceAmount,
			INV.DropEndDate AS DropDate,
			INV.CreatedDate AS InvoiceDate,
			INV.PaymentDueDate,
			IST.[Name] AS [Status],
			INV.InvoiceNumberId,
			MET.[Name] AS TerminalName,
			D_USR.FirstName AS DriverFName,
			D_USR.LastName AS DriverLName,
			INV.InvoiceTypeId
	FROM	dbo.Invoices INV
	INNER JOIN dbo.InvoiceNumbers INM ON INV.InvoiceNumberId = INM.Id
	INNER JOIN dbo.InvoiceXInvoiceStatusDetails IXS ON INV.Id = IXS.InvoiceId AND IXS.Id = (SELECT TOP 1 Id FROM InvoiceXInvoiceStatusDetails WHERE InvoiceId = INV.Id ORDER BY Id DESC)
	INNER JOIN dbo.MstInvoiceStatuses IST ON IXS.StatusId = IST.Id
	INNER JOIN dbo.Users I_USR ON INV.CreatedBy = I_USR.Id
	--INNER JOIN dbo.UserXCompanies I_UXC ON I_USR.Id = I_UXC.UserId
	INNER JOIN dbo.Companies I_COM ON I_USR.CompanyId = I_COM.Id
	LEFT JOIN dbo.Orders ORD ON INV.OrderId = ORD.Id
	LEFT JOIN dbo.FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id
	LEFT JOIN dbo.MstProducts PRD ON FRQ.FuelTypeId = PRD.Id
	LEFT JOIN dbo.MstExternalTerminals MET ON INV.TerminalId = MET.Id
	LEFT JOIN dbo.Users D_USR ON INV.DriverId = D_USR.Id
	LEFT JOIN dbo.Users F_USR ON FRQ.CreatedBy = F_USR.Id
	--LEFT JOIN dbo.UserXCompanies F_UXC ON F_USR.Id = F_UXC.UserId
	LEFT JOIN dbo.Companies F_COM ON F_USR.CompanyId = F_COM.Id
	LEFT JOIN dbo.CounterOffers COF ON FRQ.Id = COF.FuelRequestId AND COF.BuyerStatus = 2
	LEFT JOIN dbo.Users C_USR ON COF.BuyerId = C_USR.Id
	--LEFT JOIN dbo.UserXCompanies C_UXC ON C_USR.Id = C_UXC.UserId
	LEFT JOIN dbo.Companies C_COM ON C_USR.CompanyId = C_COM.Id
	WHERE	
			 INM.Number = @InvoiceNumber	 
			 AND 
			 INV.InvoiceVersionStatusId = 1		
			 AND 
			 INV.InvoiceTypeId NOT IN (6,7)			
	ORDER BY INV.Id DESC
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetDeliverySchedulesforDrivers] @CompanyId INT, 
                                                           @DriverIds VARCHAR(512), 
                                                           @StartDate DATETIMEOFFSET(7), 
                                                           @EndDate   DATETIMEOFFSET(7),
														   @CurrentDate DATETIMEOFFSET(7)
AS 
 BEGIN 
  SELECT Value INTO #TempDrivers
                                                           FROM   STRING_SPLIT(@DriverIds, ',')
      ; WITH     
      
       PASTSCHEDULES 
           AS (SELECT DISTINCT TS.DeliveryScheduleId                               AS Id, 
                               TS.Id                                               AS [TrackableScheduleId],
                               O.Id                                                AS [OrderId],
                               CASE WHEN TS.Id IS NOT NULL THEN (CASE WHEN TS.DriverId IS NOT NULL THEN TS.DriverId ELSE -1 END) ELSE (CASE WHEN OXD.DriverId IS NULL THEN -1 ELSE OXD.DriverId END) END            AS DriverId, 
                               CASE WHEN TS.Id IS NOT NULL THEN (CASE WHEN TS.DriverId IS NOT NULL THEN U.FirstName + ' ' +U.LastName ELSE '--' END) ELSE (CASE WHEN OXD.DriverId IS NULL THEN '--' ELSE D.FirstName + ' '+ D.LastName END) END               AS DriverName, 
                               COALESCE(U.PhoneNumber, D.PhoneNumber, '--')        AS PhoneNumber, 
                               ISNULL(FR.ExternalPoNumber, O.PoNumber)             AS [PoNumber],
                               FORMAT(ISNULL(TS.Date, FRD.StartDate), 'MM/dd/yyyy')                       AS [Date], 
                               FORMAT(CONVERT(DATETIME, ISNULL(TS.StartTime, FRD.StartTime)), 'hh:mm tt') 
                               + ' - ' 
                               + FORMAT(CONVERT(DATETIME, ISNULL(TS.EndTime, FRD.EndTime)), 'hh:mm tt') AS [DeliveryWindow],
                               MPD.[Name]                                          AS FuelType, 
                               CASE WHEN TS.Id IS NULL THEN CAST((FR.MaxQuantity - ISNULL((SELECT SUM(DroppedGallons) FROM Invoices WHERE OrderId = O.Id AND InvoiceVersionStatusId = 1 AND IsActive = 1),0)) AS DECIMAL(18,2)) ELSE TS.Quantity END AS Quantity, 
                               C.NAME                                              AS [Customer],
                               JBS.[Address] + ', ' + JBS.City + ', ' + MST.Code + ' ' 
                               + JBS.ZipCode                                       AS [Location],
                               CASE 
                                 WHEN TS.Id IS NULL THEN 'Open'
                                 WHEN TS.DeliveryScheduleStatusId = 3 THEN 'Scheduled' 
                                 ELSE SS.NAME 
                               END                                                 AS [Status], 
                               FR.MaxQuantity, 
                               FR.OrderClosingThreshold, 
                               FRD.EndDate                                         AS DeliveryEndDate,
                               JBS.EndDate                                         AS JobEndDate,
                               0                                                   AS ScheduleTypeId,
                               ISNULL(TS.Date, FRD.StartDate)                      AS ScheduleDate,
                               0                                                   AS WeekDayId,
                               1                                                   AS IsPastSchedule,
                               ISNULL(JBS.Latitude, 0)                             AS Latitude,
                               ISNULL(JBS.Longitude, 0)                            AS Longitude,
                               JBS.Id                                              AS JobId,
                                ISNULL(O.ParentId, 0)                              AS ParentId,
                                CASE WHEN TS.Id IS NOT NULL THEN 'Yes' ELSE 'No' END    AS DeliverySchedule,
								JBS.TimeZoneName									AS JobTimeZone,
								ISNULL(TS.EndTime, FRD.EndTime)						AS ScheduleEndTime
               FROM   Orders O 
                      INNER JOIN OrderVersions ORV 
                              ON O.Id = ORV.OrderId 
                                 AND ORV.IsActive = 1 
                      INNER JOIN OrderXStatuses OXS 
                              ON O.Id = OXS.OrderId 
                                 AND OXS.IsActive = 1 
                                 AND OXS.StatusId = 1 
                      LEFT JOIN OrderVersionXDeliverySchedules OVXDS 
                             ON ORV.Id = OVXDS.OrderVersionId 
                      LEFT JOIN DeliveryScheduleXTrackableSchedules TS 
                             ON O.Id = TS.OrderId 
                                AND TS.InvoiceId IS NULL 
                                AND TS.IsActive = 1 
                                AND TS.Date >= @StartDate 
                                AND TS.Date <= @EndDate 
                                AND TS.DeliveryScheduleStatusId IN ( 3, 6, 11, 12 )                            
                      LEFT JOIN OrderXDrivers OXD 
                             ON O.Id = OXD.OrderId 
                                AND OXD.IsActive = 1 
                      LEFT JOIN Users U 
                              ON TS.DriverId = U.Id 
                      LEFT JOIN Users D 
                              ON OXD.DriverId = D.Id 
                      LEFT JOIN MstDeliveryScheduleStatuses SS 
                              ON TS.DeliveryScheduleStatusId = SS.Id
                      INNER JOIN FuelRequests FR 
                              ON FR.Id = O.FuelRequestId 
                      INNER JOIN FuelRequestXDeliveryDetails FRD 
                              ON FRD.FuelRequestId = FR.Id 
                                -- AND FRD.StartDate >= @StartDate 
                                -- AND FRD.StartDate <= @EndDate 
                      --INNER JOIN UserXCompanies UXC 
                      --        ON UXC.UserId = FR.CreatedBy 
					  INNER JOIN Users UXC 
                              ON UXC.Id = FR.CreatedBy 
                      INNER JOIN Companies C 
                              ON C.Id = UXC.CompanyId 
                      INNER JOIN dbo.MstProducts MPD 
                              ON FR.FuelTypeId = MPD.Id 
                      --INNER JOIN dbo.JobXFuelRequests JXF 
                      --        ON FR.Id = JXF.FuelRequestId 
                      INNER JOIN dbo.Jobs JBS 
                              ON FR.JobId = JBS.Id 
                      INNER JOIN dbo.MstStates MST 
                              ON JBS.StateId = MST.Id 
               WHERE   O.AcceptedCompanyId = @CompanyId AND O.IsEndSupplier = 1
                           AND(TS.Id IS NOT NULL OR  FRD.StartDate >= @StartDate 
                                AND FRD.StartDate <= @EndDate )
                                            AND (@DriverIds = '-1' 
                                       OR (TS.Id IS NULL OR TS.DriverId IN (SELECT Value 
                                                         FROM   #TempDrivers))) 
                                                           AND ( @DriverIds = '-1' 
                                       OR (TS.DriverId IS NOT NULL OR OXD.DriverId IN (SELECT Value 
                                                           FROM  #TempDrivers)))), 
           FUTURESCHEDULES 
           AS (SELECT DISTINCT DS.Id, 
                               NULL                                                AS [TrackableScheduleId],
                               O.Id                                                AS [OrderId],
                               ISNULL(DSD.DriverId, -1)                             AS DriverId,
                               CASE WHEN U.Id IS NULL THEN '--' ELSE U.FirstName + ' ' + U.LastName END AS DriverName,  
                               ISNULL(U.PhoneNumber, '--')                          AS PhoneNumber,                     
                               ISNULL(FR.ExternalPoNumber, O.PoNumber)             AS [PoNumber],
                               FORMAT(DS.Date, 'MM/dd/yyyy')                       AS [Date], 
                               FORMAT(CONVERT(DATETIME, DS.StartTime), 'hh:mm tt') 
                               + ' - ' 
                               + FORMAT(CONVERT(DATETIME, DS.EndTime), 'hh:mm tt') AS [DeliveryWindow],
                               MPD.[Name]                                          AS FuelType, 
                               DS.Quantity, 
                               C.NAME                                              AS [Customer],
                               JBS.[Address] + ', ' + JBS.City + ', ' + MST.Code + ' ' 
                               + JBS.ZipCode                                       AS [Location],
                               CASE WHEN DS.StatusId = 6 THEN 'Rescheduled' ELSE 'Scheduled'     END                                    AS [Status], 
                               FR.MaxQuantity, 
                               FR.OrderClosingThreshold, 
                               FRD.EndDate                                         AS DeliveryEndDate,
                               JBS.EndDate                                         AS JobEndDate,
                               DS.Type                                             AS ScheduleTypeId,
                               DS.Date                                             AS ScheduleDate,
                               DS.WeekDayId, 
                               0                                                   AS IsPastSchedule,
                               ISNULL(JBS.Latitude, 0)                             AS Latitude,
                               ISNULL(JBS.Longitude, 0)                            AS Longitude,
                               JBS.Id                                              AS JobId,
                                ISNULL(O.ParentId, 0)                               AS ParentId ,
                                'Yes'                                               AS DeliverySchedule,
								JBS.TimeZoneName									AS JobTimeZone,
								DS.EndTime											AS ScheduleEndTime
               FROM   Orders O 
                      INNER JOIN OrderVersions OV 
                              ON OV.OrderId = O.Id 
                                 AND OV.IsActive = 1 
                      INNER JOIN OrderVersionXDeliverySchedules OVXDS 
                              ON OVXDS.OrderVersionId = OV.Id 
                      INNER JOIN DeliverySchedules DS 
                              ON DS.Id = OVXDS.DeliveryRequestId 
                      INNER JOIN OrderXStatuses OS 
                              ON OS.OrderId = O.Id 
                                 AND OS.IsActive = 1 
                                 AND OS.StatusId = 1 
                      LEFT JOIN DeliveryScheduleXDrivers DSD 
                              ON DSD.DeliveryScheduleId = DS.Id 
                                 AND DSD.IsActive = 1 
                                 AND (@DriverIds = '-1' OR (DSD.DriverId IS NOT NULL
                                        AND 
                                        (DSD.DriverId IN (SELECT [Value]
                                                            FROM   #TempDrivers))))
                      LEFT JOIN Users U 
                              ON DSD.DriverId = U.Id 
                      INNER JOIN FuelRequests FR 
                              ON FR.Id = O.FuelRequestId 
                      INNER JOIN FuelRequestXDeliveryDetails FRD 
                              ON FRD.FuelRequestId = FR.Id 
                      --INNER JOIN UserXCompanies UXC 
                      --        ON UXC.UserId = FR.CreatedBy 
					  INNER JOIN Users UXC 
                              ON UXC.Id = FR.CreatedBy 
                      INNER JOIN Companies C 
                              ON C.Id = UXC.CompanyId 
                      INNER JOIN dbo.MstProducts MPD 
                              ON FR.FuelTypeId = MPD.Id 
                      --INNER JOIN dbo.JobXFuelRequests JXF 
                      --        ON FR.Id = JXF.FuelRequestId 
                      INNER JOIN dbo.Jobs JBS 
                              ON FR.JobId = JBS.Id 
                      INNER JOIN dbo.MstStates MST 
                              ON JBS.StateId = MST.Id 
               WHERE  O.AcceptedCompanyId = @CompanyId AND O.IsEndSupplier = 1 
                      AND ( ( DS.Type <> 4 
                              AND DS.Date <= @EndDate ) 
                             OR ( DS.Date > @CurrentDate 
                                  AND DS.Date <= @EndDate ) )) 
      SELECT * 
      FROM   PASTSCHEDULES 
      UNION 
      SELECT * 
      FROM   FUTURESCHEDULES 
      ORDER  BY OrderId Desc 
  END 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetDeliverySchedulesMapData] 
  @DriverIds VARCHAR(512) 
AS 
  BEGIN 
    SELECT    U.FirstName + ' ' + U.LastName AS NAME, 
              L.Latitude, 
              L.Longitude, 
              U.Id, 
              L.UpdatedDate 
    INTO      #TEMPDRIVERLOCATIONS 
    FROM      Users U 
    LEFT JOIN AppLocations L 
    ON        U.Id = L.UserId 
    AND       L.UserId IN 
              ( 
                     SELECT Value 
                     FROM   STRING_SPLIT(@DriverIds, ',')) 
    WHERE     L.Latitude IS NOT NULL 
    AND       L.Longitude IS NOT NULL 
    ORDER BY  L.UpdatedDate DESC ; 
     
    WITH ORDERED AS 
    ( 
             SELECT   NAME, 
                      Latitude, 
                      Longitude , 
                      ROW_NUMBER() OVER (PARTITION BY Id ORDER BY UpdatedDate DESC) AS rn 
             FROM     #TEMPDRIVERLOCATIONS ) 
    SELECT DISTINCT NAME, 
                    Latitude, 
                    Longitude 
    FROM            ORDERED 
    WHERE           rn = 1; 
   
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_GetDriverOrdersByfilter]
	-- Add the parameters for the stored procedure here
	@DriverId INT = 0,
	@CompanyId INT = 0,
	@Filter nvarchar(30) = 'All'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	    SET NOCOUNT ON;

		IF @Filter IN ('All','Assigned')
		BEGIN
			SELECT  DISTINCT O.Id  from Orders O
			INNER JOIN  OrderVersions OV ON O.Id = OV.OrderId AND OV.Version = (SELECT MAX(Version) FROM OrderVersions WHERE OrderId = O.Id)
			INNER JOIN  OrderVersionXDeliverySchedules OVDS ON OVDS.OrderVersionId = OV.Id
			INNER JOIN  DeliveryScheduleXDrivers DSD ON DSD.DeliveryScheduleId = OVDS.DeliveryRequestId
			WHERE DSD.DriverId = @DriverId AND O.AcceptedCompanyId = @CompanyId
			UNION
			SELECT DISTINCT O.Id FROM Orders O
			INNER JOIN  OrderXDrivers OD ON O.Id = OD.OrderId
			WHERE OD.DriverId = @DriverId AND O.AcceptedCompanyId = @CompanyId AND OD.IsActive =1
		END
		ELSE IF @Filter = 'TodaysScheduled'
		BEGIN
			SELECT DISTINCT O.Id  from Orders O
			INNER JOIN  OrderVersions OV ON O.Id = OV.OrderId AND OV.Version = (SELECT MAX(Version) FROM OrderVersions WHERE OrderId = O.Id)
			INNER JOIN  OrderVersionXDeliverySchedules OVDS ON OVDS.OrderVersionId = OV.Id
			INNER JOIN  DeliveryScheduleXDrivers DSD ON DSD.DeliveryScheduleId = OVDS.DeliveryRequestId
			INNER JOIN DeliverySchedules DS ON DS.Id = DSD.DeliveryScheduleId
			WHERE DSD.DriverId = @DriverId AND CAST(DS.Date AS DATE) = CAST(GETDATE() AS DATE) AND O.AcceptedCompanyId = @CompanyId
			UNION
			SELECT DISTINCT O.Id FROM Orders O
			INNER JOIN  OrderXDrivers OD ON O.Id = OD.OrderId
			INNER JOIN FuelRequestXDeliveryDetails FDD ON O.FuelRequestId = FDD.FuelRequestId
			WHERE OD.DriverId = @DriverId AND O.AcceptedCompanyId = @CompanyId AND CAST(FDD.StartDate AS DATE) = CAST(GETDATE() AS DATE) AND OD.IsActive =1
		END		
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Shubham Chawla
-- Create date: 13-04-2018
-- Description:	Get Exception Logs for Super Admin
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetExceptionLogs] 
 @StartDate varchar(25),  
 @EndDate varchar(25),
 @GlobalSearchText varchar(30) = NULL,
 @SortId int = 0,
 @SortDirection varchar(6) = 'desc',
 @PageSize int = 99999999,
 @PageNumber int = 1, 
 @IDSearchTypes [dbo].SearchTypes  Readonly, 
 @MachineNameSearchTypes [dbo].SearchTypes  Readonly, 
 @LogDateTimeSearchTypes [dbo].SearchTypes  Readonly, 
 @MessageSearchTypes [dbo].SearchTypes  Readonly, 
 @LevelSearchTypes [dbo].SearchTypes  Readonly, 
 @ExceptionSearchTypes [dbo].SearchTypes  Readonly
AS
BEGIN
		SELECT ID,
		MachineName,
		LogDateTime,
		Message,
		Level,
		[TotalCount]= COUNT(ID) OVER()
		FROM [ExceptionLog] 
		WHERE LogDateTime BETWEEN @StartDate AND @EndDate
		  AND (
			  @GlobalSearchText IS NULL 
		  OR (
			  (ID like '%' + @GlobalSearchText+ '%')
			  OR (MachineName like '%' + @GlobalSearchText+ '%')
			  OR (LogDateTime like '%' + @GlobalSearchText+ '%')
			  OR (Message like '%' + @GlobalSearchText+ '%')
			  OR (Level like '%' + @GlobalSearchText+ '%')
			  )
			  )
		ORDER BY
		CASE 
			 WHEN @SortId = 0 AND @SortDirection = 'asc' THEN  ID
		  END ASC,
		  CASE 
			 WHEN @SortId = 2 AND @SortDirection = 'asc' THEN LogDateTime 
		  END ASC,
		  CASE 
			 WHEN @SortId = 1 AND @SortDirection = 'asc' THEN MachineName 
		  WHEN @SortId = 3 AND @SortDirection = 'asc' THEN Message
		  WHEN @SortId = 4 AND @SortDirection = 'asc' THEN Level 
		  END ASC,
			CASE 
			 WHEN @SortId = 0 AND @SortDirection = 'desc' THEN  ID
		  END DESC,
		  CASE 
			WHEN @SortId = 2 AND @SortDirection = 'desc' THEN LogDateTime 
		  END DESC,
		  CASE 
			WHEN @SortId = 1 AND @SortDirection = 'desc' THEN MachineName 
		  WHEN @SortId = 3 AND @SortDirection = 'desc' THEN Message
		  WHEN @SortId = 4 AND @SortDirection = 'desc' THEN Level 
		  END DESC
		 OFFSET ( @PageNumber-1 ) * @PageSize ROWS 
		 FETCH NEXT @PageSize ROWS ONLY 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rupesh Bhadane
-- Create date: 17-Jan-2018
-- Description:	Stored Procedure to get all the Open and Accepted Fuel Requests for Super Admin
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetFuelRequestsSummary]
	@StartDate	DATETIMEOFFSET(7),
	@EndDate	DATETIMEOFFSET(7),
	@PageId INT = 1,
	@PageSize INT = 10,
	@SortId INT = 0
AS
BEGIN
        SELECT  FR.Id AS FuelRequestId,
                FR.RequestNumber, 
                J.ZipCode, 
                FR.MaxQuantity  AS Quantity, 
                FRT.NAME        AS FuelType, 
                FR.CreatedDate, 
                C.NAME  AS CompanyName, 
                FR.PricePerGallon AS Price, 
                MFRS.NAME       AS [Status],
				FR.PricingTypeId,
				FR.RackAvgTypeId 
         FROM   FuelRequests FR 
                --INNER JOIN JobXFuelRequests JFR 
                --        ON FR.Id = JFR.FuelRequestId 
                INNER JOIN Jobs J 
                        ON FR.JobId = J.Id 
                --INNER JOIN CompanyXJobs CJ 
                --        ON J.Id = CJ.JobId 
                INNER JOIN Companies C 
                        ON J.CompanyId = C.Id 
                INNER JOIN Users U 
                        ON FR.CreatedBy = U.Id 
                INNER JOIN MstProducts FRT 
                        ON FR.FuelTypeId = FRT.Id 
                INNER JOIN FuelRequestXStatuses FRS 
                        ON FR.Id = FRS.FuelRequestId 
                           AND FRS.IsActive = 1 
                INNER JOIN MstFuelRequestStatuses MFRS 
                        ON FRS.StatusId = MFRS.Id 
         WHERE  MFRS.NAME IN ( 'Open', 'Accepted' ) 
                AND FR.CreatedDate >= @StartDate 
                AND FR.CreatedDate < @EndDate 
         ORDER BY MFRS.Name DESC
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Author:  Rupesh Bhadane
-- Create date: 24-May-2018  
-- Description: Returns latest received (within 15 seconds) supplier qualified fuel requests  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_GetLatestReceivedFuelRequests] 
 @CompanyId INT,  
 @UserId  INT,  
 @CurrentDate DATETIMEOFFSET(7) 
AS  
BEGIN  

 ;WITH CompanyLocations AS  
 (  
  SELECT SXSS.AddressId,  
    CAD.StateId AS SupplierStateId,  
    SXSS.StateId AS SurvingStateId,  
    CAD.CountryId,  
    SXAS.IsStateWideService,  
    SXAS.Radius,  
    SXAS.IsHedgeOrderAllowed,  
    SXAS.IsOverWaterRefuelingAllowed,  
    CAD.Latitude,  
    CAD.Longitude  
  FROM [dbo].[CompanyAddresses] CAD  
  LEFT JOIN [dbo].[SupplierAddressXServingStates] SXSS ON CAD.Id = SXSS.AddressId  
  LEFT JOIN [dbo].[SupplierAddressXSettings]  SXAS ON CAD.Id = SXAS.AddressId  
  WHERE CAD.IsActive = 1   
    AND  
    CompanyId = @CompanyId     
 ),  
  
 AllFuelRequests AS  
 (  
  SELECT FRQ.Id AS FuelRequestId,  
    FRQ.FuelRequestTypeId, 
    COM.[Name] AS Customer,  
    J.StateId,  
    J.CompanyId, 
	J.Latitude,  
    J.Longitude,   
    MPD.ProductTypeId,  
    FRQ.OrderTypeId,  
    FRQ.MaxQuantity AS GallonsNeeded,  
    FRD.StartDate AS StartDate,  
    FRQ.RequestNumber,  
    FRQ.IsPublicRequest, 
    FXPS.PrivateSupplierListId,  
    USR.CompanyId AS FrCompanyId  
  FROM [dbo].[FuelRequests] FRQ  
  INNER JOIN dbo.FuelRequestXStatuses FXS ON FRQ.Id = FXS.FuelRequestId AND FXS.IsActive = 1  
  INNER JOIN [dbo].[MstFuelRequestStatuses] MFRST ON FXS.StatusId = MFRST.Id  
  INNER JOIN [dbo].[MstProducts] MPD ON FRQ.FuelTypeId = MPD.Id  
  INNER JOIN [dbo].[FuelRequestXDeliveryDetails] FRD ON FRQ.Id = FRD.FuelRequestId  
  INNER JOIN [dbo].[Users] USR ON FRQ.CreatedBy = USR.Id  
  --INNER JOIN [dbo].[UserXCompanies] UXC ON USR.Id = UXC.UserId  
  INNER JOIN [dbo].[Companies] COM ON USR.CompanyId = COM.Id  
  --INNER JOIN [dbo].[JobXFuelRequests] FRJ ON FRQ.Id = FRJ.FuelRequestId  
  INNER JOIN [dbo].[Jobs] J ON J.Id = FRQ.JobId  
  INNER JOIN [dbo].[MstStates] MST ON J.StateId = MST.Id  
  --INNER JOIN [dbo].[CompanyXJobs] CXJ ON J.Id = CXJ.JobId  
  LEFT JOIN  [dbo].[FuelRequestXPrivateSupplierLists] FXPS ON FRQ.Id = FXPS.FuelRequestId  
  LEFT JOIN [dbo].[PrivateSupplierLists] PSLT ON FXPS.PrivateSupplierListId = PSLT.Id  
  LEFT JOIN Orders O ON FRQ.Id = O.FuelRequestId  
  WHERE FRQ.IsActive = 1  
    AND  
    FXS.StatusId = 2 
    AND  
    (FRQ.FuelRequestTypeId != 2  OR (FRQ.FuelRequestTypeId = 2 AND FXS.StatusId = 3 AND O.AcceptedCompanyId = @CompanyId))-- Don't show counter offers  
    AND  
    FRQ.CreatedDate >= DATEADD(SS,-15,@CurrentDate)  AND FRQ.CreatedDate < @CurrentDate
      
 ),  
  
 FuelRequestWithServiceArea AS  
 (  
  SELECT OFRJ.FuelRequestId,  
    OFRJ.FuelRequestTypeId,
    OFRJ.Customer,  
    OFRJ.StateId,  
    OFRJ.CompanyId,  
	OFRJ.ProductTypeId,  
    OFRJ.OrderTypeId,  
    OFRJ.GallonsNeeded,  
    OFRJ.StartDate,  
    OFRJ.RequestNumber,
	OFRJ.IsPublicRequest,  
    OFRJ.PrivateSupplierListId,  
	COML.AddressId,  
	COML.SupplierStateId,  
    COML.SurvingStateId,  
    COML.IsStateWideService,  
    COML.Radius,  
    COML.IsHedgeOrderAllowed,  
    [dbo].[usf_CalculateDistance](COML.Latitude,COML.Longitude,OFRJ.Latitude,OFRJ.Longitude) AS Distance  
  FROM AllFuelRequests OFRJ  
  INNER JOIN CompanyLocations COML ON COML.SurvingStateId = OFRJ.StateId
  WHERE @CompanyId NOT IN (SELECT BLCI.CompanyId FROM [dbo].[usf_GetBlacklistedCompanyIds](FrCompanyId) BLCI)  
    AND  
    FrCompanyId NOT IN (SELECT BLCI.CompanyId FROM [dbo].[usf_GetBlacklistedCompanyIds](@CompanyId) BLCI)  
 ),  
  
 FuelRequestWitninServiceArea AS  
 (  
  SELECT *  
  FROM FuelRequestWithServiceArea  
  WHERE (StateId = SurvingStateId)  
    AND  
    (IsStateWideService = 1 OR Distance <= Radius)  
    AND  
    (IsHedgeOrderAllowed = 1 OR OrderTypeId = 2)  
    AND  
    CompanyId != @CompanyId  
    AND  
    (FuelRequestTypeId <> 3 OR @CompanyId NOT IN (SELECT BRCH.CompanyId FROM [dbo].[usf_GetBrokeredChain](FuelRequestId) BRCH))  
 ),  
  
 QualifiedFuelRequests AS  
 (  
  SELECT FRWSA.*  
  FROM FuelRequestWitninServiceArea FRWSA  
  WHERE ProductTypeId IN (SELECT SAPT.ProductTypeId FROM [dbo].[SupplierAddressXProductTypes] SAPT WHERE SAPT.AddressId = FRWSA.AddressId)  
    AND  
    dbo.usf_IsSupplierQualificationMatched(FRWSA.FuelRequestId, FRWSA.AddressId) = 1  
    AND  
    (  
     FRWSA.IsPublicRequest = 1  
     OR  
     @CompanyId IN (SELECT PSXSC.SupplierCompanyId FROM [dbo].[PrivateSupplierListXSupplierCompanies] PSXSC WHERE PSXSC.PrivateSupplierListId = FRWSA.PrivateSupplierListId)  
    )  
    AND  
    (-- Brokered fuel request (for Red-Dye-Diesel) of Texas should be visible to texas state suppliers only  
     (CASE WHEN (FuelRequestTypeId = 3 AND ProductTypeId = 6 AND FRWSA.StateId = 43)  
     THEN CASE WHEN (FRWSA.SupplierStateId = 43) THEN 1 ELSE 0 END  
     ELSE 1 END) = 1  
    )  
 )

 -- Final SELECTION of columns to return
 SELECT DISTINCT FuelRequestId,  
   Customer,     
   GallonsNeeded,  
   StartDate,  
   RequestNumber  
 FROM QualifiedFuelRequests    
END  
------------------------------------------ Incoming FR alert to Supplier ----End
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rupesh Bhadane
-- Create date: 06-Jun-2017
-- Description:	Stored procedure to get all the missed schedules for an order
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetMissedSchedulesForOrder] 
    @orderId        INT,
    @PageNo            INT,
    @PageSize        INT,
    @SortId            INT
AS
BEGIN
    SET NOCOUNT ON;
     SELECT TSC.Id, 
            CONVERT(NVARCHAR(10), TSC.Date, 101)      ScheduleDate, 
            FORMAT(CAST(TSC.StartTime AS DateTIME),'h:mm tt') + ' - ' + FORMAT(CAST(TSC.EndTime AS DateTIME),'h:mm tt') As DeliveryWindow, 
            U.FirstName + ' ' + U.LastName            DriverName, 
            CASE WHEN TSC.DeliveryScheduleStatusId = 21 THEN 'Canceled' ELSE MDS.NAME END                                 [Status], 
            TSC.OrderId 
     FROM   DeliveryScheduleXTrackableSchedules TSC 
            JOIN MstDeliveryScheduleStatuses MDS 
            ON TSC.DeliveryScheduleStatusId = MDS.Id 
            LEFT JOIN Users U 
            ON TSC.DriverId = U.Id 
     WHERE  TSC.OrderId = @orderId 
            AND TSC.InvoiceId IS NULL 
            AND TSC.Date < Getdate() 
            AND TSC.IsActive = 1 
            AND TSC.DeliveryScheduleStatusId IN ( 21, 11, 12 ) 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 20-Feb-2018
-- Description:	Returns newsfeed messages of specified company
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetNewsfeed]
	-- Add the parameters for the stored procedure here
	@CompanyId		INT,
	@EntityTypeId	INT,
	@EntityId		INT,
	@CurrentPage	INT,
	@PageSize		INT,
	@LatestId		INT = 0,
	@UserId         INT = 0
AS
BEGIN
	DECLARE @SkipCount INT = (CASE WHEN @CurrentPage <= 0 THEN 0 ELSE ((@CurrentPage - 1) * @PageSize) END)
	;WITH UniqueNewsfeedIds AS
	(
	SELECT	Id,
			EventId
	FROM	dbo.Newsfeeds
	WHERE	RecipientCompanyId = @CompanyId
			AND
			EntityTypeId = @EntityTypeId
			AND
			EntityId = @EntityId
			AND
			(@LatestId = 0 OR Id > @LatestId)	
	)
	SELECT	NFD.Id,
			NFD.EventId,
			NFD.EntityId,
			NFD.EntityTypeId,
			NFD.TargetEntityId,
			NFD.RecipientCompanyId,
			NFD.FeedMessage,
			NFD.CreatedBy,
			FORMAT(NFD.CreatedDate, 'MM/dd/yyyy hh:mm tt') AS CreatedDate,
			NFD.IsRead,
			CASE WHEN NFM.TargetUrl IS NULL THEN NULL ELSE REPLACE(NFM.TargetUrl, '[TargetEntityId]', NFD.TargetEntityId) END AS TargetUrl
	FROM	dbo.Newsfeeds NFD
	INNER JOIN UniqueNewsfeedIds UNF ON NFD.Id = UNF.Id
	INNER JOIN dbo.MstNewsfeedMessages NFM ON NFD.EventId = NFM.EventId
	INNER JOIN dbo.MstNewsfeedEvents NFE ON NFD.EventId = NFE.Id
	WHERE	NFD.IsActive = 1
	        AND
	        LOWER(NFE.EventType) NOT LIKE '%message%' 
			OR 
			LOWER(NFE.EventType) LIKE '%message%' AND (SELECT COUNT(1) FROM AppMessageXUserStatuses WHERE UserId = @UserId AND MessageId = NFD.TargetEntityId) > 0 			
			
	ORDER BY NFD.CreatedDate DESC
	OFFSET @SkipCount ROWS FETCH NEXT @PageSize ROWS ONLY
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sravanthi Pirikiti
-- Create date: 15-May-2018
-- Description:	Stored Procedure to get orders by ponumber
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetOrdersByPoNumber]
	@PoNumber	NVARCHAR(256)
AS
BEGIN
        SELECT O.Id AS Id, 
		ISNULL(FR.ExternalPoNumber, O.PoNumber)  AS PoNumber,
                J.Name AS Job, 
                C.Name  AS Customer, 
                S.NAME        AS Supplier, 
				P.Name AS FuelType,
				CONVERT(NVARCHAR(10), FDD.StartDate, 101) AS StartDate,
				FR.MaxQuantity AS Quantity,
				FR.PricePerGallon,
				FR.PricingTypeId,
				FR.RackAvgTypeId,
				OS.Name AS [Status]
         FROM   FuelRequests FR        
		 INNER JOIN FuelRequestXDeliveryDetails FDD ON FR.Id = FDD.FuelRequestId
		 INNER JOIN Orders O ON FR.Id = O.FuelRequestId  
		 --INNER JOIN JobXFuelRequests JXF ON JXF.FuelRequestId = FR.Id
		 INNER JOIN Jobs J ON FR.JobId = J.Id      
		 LEFT JOIN Companies C ON O.BuyerCompanyId = C.Id
		 LEFT JOIN Companies S ON O.AcceptedCompanyId = S.Id
		 LEFT JOIN MstProducts P ON FR.FuelTypeId = P.Id
		 LEFT JOIN OrderXStatuses OXS ON O.Id = OXS.OrderId AND OXS.IsActive =1  
		 LEFT JOIN MstOrderStatuses OS ON OXS.StatusId = OS.Id                      
         WHERE LOWER(FR.ExternalPoNumber) = LOWER(@PoNumber) OR LOWER(O.PoNumber) = LOWER(@PoNumber)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetParticularException] 
	
 @Id int
AS
BEGIN
	select Exception from [ExceptionLog] where ID = @Id
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rupesh Bhadane
-- Create date: 14 - 05 - 2018
-- Description:	Stored procedure to get all the products in given radius for specific Job location
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetProductsInYourArea]
	@JobId INT,
	@Latitude DECIMAL(18,8),
	@Longitude DECIMAL(18,8),
	@Radius DECIMAL(18,8) = 100
AS
BEGIN

DECLARE @StartRadius DECIMAL(18,8) = @Radius - 100

IF @JobId > 0
BEGIN
 SELECT @Latitude = Latitude,@Longitude = Longitude FROM Jobs WHERE Id = @JobId
END


;WITH TERMINALS_CTE AS
(
    SELECT    MET.Id,
            [dbo].[usf_CalculateDistance](MET.Latitude, MET.Longitude, @Latitude, @Longitude) AS Distance
    FROM    MstExternalTerminals MET
),

TERMINAL_WITHINDISTANCE_CTE AS
(
    SELECT * FROM TERMINALS_CTE WHERE Distance > @StartRadius AND Distance <= @Radius
),

PREVIOUS_TERMINALS_CTE AS
(
    SELECT * FROM TERMINALS_CTE WHERE Distance > 0 AND Distance <= @StartRadius
)

SELECT  DISTINCT
        MPD.Id,
        MPD.[Name]
FROM    TERMINAL_WITHINDISTANCE_CTE MET
INNER JOIN MstProductMappings MPM ON MET.Id = MPM.ExternalTerminalId
INNER JOIN MstProducts MPD ON MPM.ProductId = MPD.Id
WHERE MPD.Id NOT IN (  SELECT  DISTINCT
								MPDPrev.Id
						FROM    PREVIOUS_TERMINALS_CTE METPrev
						INNER JOIN MstProductMappings MPMPrev ON METPrev.Id = MPMPrev.ExternalTerminalId
						INNER JOIN MstProducts MPDPrev ON MPMPrev.ProductId = MPDPrev.Id 
					)
ORDER BY MPD.Id
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:    Sravanthi Pirikiti 
-- Create date: 25-05-2018 
-- Description:  Returns supplier calendar future schedules
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetSupplierCalendarFutureSchedules] @FirstDayOfMonth   DATETIME, 
                                                                @LastDayOfMonth    DATETIME, 
                                                                @BuyerCompanyId    INT, 
                                                                @SupplierCompanyId INT, 
                                                                @SelectedOrders    NVARCHAR(MAX)
AS 
  BEGIN ; 
      WITH FutureSchedules 
           AS (SELECT ORD.Id, 
                      DS.Date                                                            AS StartDate,
                      DS.StartTime, 
                      DS.EndTime, 
                      3                                                                  AS StatusId,
                      DSD.DriverId, 
                      D.FirstName                                                        AS DriverFirstName,
                      D.LastName                                                         AS DriverLastName,
                      ISNULL(FRQ.ExternalPoNumber, ORD.PoNumber)                         AS PoNumber,
                      DS.Quantity, 
                      CASE 
                        WHEN COM.CompanyTypeId = 2 
                             AND BR_FRQ.Id IS NOT NULL THEN BR_COM.[Name] 
                        ELSE COM.[Name] 
                      END                                                                AS CompanyName,
                      MST.[Code]                                                         AS JobStateCode, 
                      JBS.ZipCode                                                        AS JobZipCode,
                      JBS.TimeZoneName                                                   AS TimeZone,
                      ISNULL(FDD.EndDate, JBS.EndDate)                                   AS OrderEndDate,
                      ( ISNULL(FRQ.OrderClosingThreshold, 100) * FRQ.MaxQuantity ) / 100 AS OrderQuantity,
                      DS.Type                                                            AS ScheduleType,
                      JC.NAME                                                            AS JobCompanyName,
                      ISNULL(TS.Quantity, 0) 
                      + ISNULL(I.DroppedGallons, 0)                                      AS DroppedGallons
               FROM   Orders ORD 
                      INNER JOIN OrderXStatuses OXS 
                              ON ORD.Id = OXS.OrderId 
                                 AND OXS.IsActive = 1 
                      INNER JOIN MstOrderStatuses OST 
                              ON OXS.StatusId = OST.Id 
                      INNER JOIN FuelRequests FRQ 
                              ON ORD.FuelRequestId = FRQ.Id 
                      INNER JOIN FuelRequestXDeliveryDetails FDD 
                              ON FRQ.Id = FDD.FuelRequestId 
                      --INNER JOIN JobXFuelRequests JXF 
                      --        ON FRQ.Id = JXF.FuelRequestId 
                      INNER JOIN Jobs JBS 
                              ON FRQ.JobId = JBS.Id 
                      --INNER JOIN CompanyXJobs CXJ 
                      --        ON JBS.Id = CXJ.JobId 
                      INNER JOIN Companies JC 
                              ON JBS.CompanyId = JC.Id 
                      INNER JOIN MstStates MST 
                              ON JBS.StateId = MST.Id 
                      INNER JOIN OrderVersions OV 
                              ON OV.OrderId = ORD.Id 
                      INNER JOIN OrderVersionXDeliverySchedules OVXDS 
                              ON OV.Id = OVXDS.OrderVersionId 
                      INNER JOIN DeliverySchedules DS 
                              ON OVXDS.DeliveryRequestId = DS.Id 
                      --INNER JOIN UserXCompanies UXC 
                      --        ON FRQ.CreatedBy = UXC.UserId 
					  INNER JOIN Users UXC 
                              ON FRQ.CreatedBy = UXC.Id 
                      INNER JOIN Companies COM 
                              ON UXC.CompanyId = COM.Id 
                      LEFT JOIN DeliveryScheduleXDrivers DSD 
                             ON DS.Id = DSD.DeliveryScheduleId 
                      LEFT JOIN Users D 
                             ON DSD.DriverId = D.Id 
                      LEFT JOIN FuelRequests BR_FRQ 
                             ON FRQ.Id = BR_FRQ.ParentId 
                      --LEFT JOIN UserXCompanies BR_UXC 
                      --       ON BR_FRQ.CreatedBy = BR_UXC.UserId 
					  LEFT JOIN Users BR_UXC 
                             ON BR_FRQ.CreatedBy = BR_UXC.Id 
                      LEFT JOIN Companies BR_COM 
                             ON BR_UXC.CompanyId = BR_COM.Id 
                      LEFT JOIN Invoices I 
                             ON ORD.Id = I.OrderId AND I.InvoiceVersionStatusId = 1 AND I.IsActive = 1 
                      LEFT JOIN DeliveryScheduleXTrackableSchedules TS 
                             ON ORD.Id = TS.OrderId AND TS.IsActive = 1 
                      AND TS.InvoiceId IS NULL 
                      AND TS.DeliveryScheduleStatusId NOT IN ( 21, 5 )
               WHERE  ORD.AcceptedCompanyId = @SupplierCompanyId 
                      AND ORD.IsEndSupplier = 1 
                      AND OV.IsActive = 1 
                      AND OXS.StatusId = 1 
                      AND ORD.IsActive = 1 
                      AND ( @BuyerCompanyId = 0 
                             OR ORD.BuyerCompanyId = @BuyerCompanyId ) 
                      AND ( @SelectedOrders = '' 
                             OR ORD.Id IN (SELECT VALUE 
                                           FROM   STRING_SPLIT(@SelectedOrders, ',')) ) 
                      AND ( ( DS.Type = 4 
                              AND Cast(DS.Date AS DATE) >= @FirstDayOfMonth 
                              AND Cast(DS.Date AS DATE) <= @LastDayOfMonth ) 
                             OR ( DS.Type <> 4 
                                  AND Cast(DS.Date AS DATE) <= @LastDayOfMonth ) ) 
				) 
      SELECT Id, 
             StartDate, 
             StartTime, 
             EndTime, 
             StatusId, 
             DriverId, 
             DriverFirstName, 
             DriverLastName, 
             PoNumber, 
             Quantity, 
             CompanyName, 
             JobStateCode, 
             JobZipCode, 
             TimeZone, 
             OrderEndDate, 
             OrderQuantity, 
             ScheduleType, 
             JobCompanyName, 
             Sum(DroppedGallons) AS Delivered 
      FROM   FutureSchedules 
      GROUP  BY Id, 
                StartDate, 
                StartTime, 
                EndTime, 
                StatusId, 
                DriverId, 
                DriverFirstName, 
                DriverLastName, 
                PoNumber, 
                Quantity, 
                CompanyName, 
                JobStateCode, 
                JobZipCode, 
                TimeZone, 
                OrderEndDate, 
                OrderQuantity, 
                ScheduleType, 
                JobCompanyName 
	 ORDER BY StartDate
  END 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ============================================= 
-- Author:    Sravanthi Pirikiti 
-- Create date: 25-05-2018 
-- Description:  Returns supplier calendar invoices
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetSupplierCalendarInvoices] 
  @CompanyId  INT, 
  @MonthFirstDate      DATETIME,
  @LastDayVisible    DATETIME ,
  @CustomerCompanyId     INT, 
  @SelectedOrders    NVARCHAR(MAX)
AS 
  BEGIN    
   
               SELECT     I.Id as id, 
                         '#' + INum.Number AS title,
						 CONVERT(NVARCHAR(10), I.PaymentDueDate, 101) AS [start],
						 '#fff' AS textColor,
						 2 AS calendarEventType,
						 CAST (1 AS BIT) AS allDay,
						 0 AS parentStatus
               FROM       dbo.Invoices I 
			   INNER JOIN InvoiceNumbers INum
			   ON I.InvoiceNumberId = INum.Id
               INNER JOIN Orders O
               ON         O.Id = I.OrderId 
               WHERE      I.OrderId IS NOT NULL 
               AND        I.InvoiceVersionStatusId = 1 
               AND        O.IsEndSupplier = 1 
               AND        O.AcceptedCompanyId = @CompanyId 
               AND        (@CustomerCompanyId = 0 OR O.BuyerCompanyId = @CustomerCompanyId)
			   AND CAST(I.PaymentDueDate AS DATE) >= @MonthFirstDate
			   AND CAST(I.PaymentDueDate AS DATE) <= @LastDayVisible 
			   AND ( @SelectedOrders = '' 
                    OR O.Id IN (SELECT VALUE 
                                  FROM   STRING_SPLIT(@SelectedOrders, ',')) ) 
			    ORDER BY PaymentDate		   
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetSupplierDashboardDropAverages] 
    @CompanyId	INT,
	@UserId INT = 0,
	@BuyerCompanyId INT = 0,
	@FuelTypeId INT = 0
AS 
  BEGIN 
    SELECT COUNT(DISTINCT O.Id) AS TotalOrders,
	       COUNT(INV.Id) AS TotalDrops,
	       ISNULL(AVG(INV.PricePerGallon),0) AS AvgPPGPerDrop,
		   ISNULL(AVG(INV.DroppedGallons),0) AS AvgGallonPerDrop
	FROM Orders O
	INNER JOIN FuelRequests FR ON  O.FuelRequestId = FR.Id
	LEFT JOIN Invoices INV ON O.Id = INV.OrderId AND INV.IsActive = 1 AND INV.InvoiceVersionStatusId = 1
	WHERE AcceptedCompanyId = @CompanyId
	  AND (@BuyerCompanyId = 0 OR BuyerCompanyId = @BuyerCompanyId)
	  AND (@FuelTypeId = 0 OR FR.FuelTypeId = @FuelTypeId)
	  AND FR.IsActive = 1   
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Author:  Kailash Saini  
-- Create date: 17-Sep-2017  
-- Description: Returns supplier qualified fuel requests  
-- =============================================  
CREATE PROCEDURE [dbo].[usp_GetSupplierFuelRequests]   
	@CompanyId INT,  
	@UserId  INT,  
	@AddressId INT = 0, -- All Addresses  
	@Broadcast INT = 3,  -- All FuelRequests  
	@StartDate DATETIMEOFFSET(7),  
	@EndDate DATETIMEOFFSET(7),
	@StatusFilter INT = 0,
	@isCallFromDashboard	BIT = 0,
	@GlobalSearchText VARCHAR(30) = NULL,
	@SortId INT = 0,
	@SortDirection VARCHAR(6) = 'desc',
	@PageSize INT = 99999999,
	@PageNumber INT = 1,
	@FuelRequestIdSearchTypes [dbo].SearchTypes READONLY,
	@AddressSearchTypes [dbo].SearchTypes READONLY,
	@FuelTypeSearchTypes [dbo].SearchTypes READONLY,
	@GallonsSearchTypes [dbo].SearchTypes READONLY,
	@PriceSearchTypes [dbo].SearchTypes READONLY,
	@DistanceSearchTypes [dbo].SearchTypes READONLY,
	@StartSearchTypes [dbo].SearchTypes READONLY,
	@StatusSearchTypes [dbo].SearchTypes READONLY
AS  
BEGIN  
	DECLARE @FuelRequestIdSearchTypesValid INT
	SET @FuelRequestIdSearchTypesValid = (SELECT COUNT(*) FROM @FuelRequestIdSearchTypes)

	DECLARE @AddressSearchTypesValid INT
	SET @AddressSearchTypesValid = (SELECT COUNT(*) FROM @AddressSearchTypes)

	DECLARE @FuelTypeSearchTypesValid INT
	SET @FuelTypeSearchTypesValid = (SELECT COUNT(*) FROM @FuelTypeSearchTypes)

	DECLARE @GallonsSearchTypesValid INT
	SET @GallonsSearchTypesValid = (SELECT COUNT(*) FROM @GallonsSearchTypes)

	DECLARE @PriceSearchTypesValid INT
	SET @PriceSearchTypesValid = (SELECT COUNT(*) FROM @PriceSearchTypes)

	DECLARE @DistanceSearchTypesValid INT
	SET @DistanceSearchTypesValid = (SELECT COUNT(*) FROM @DistanceSearchTypes)

	DECLARE @StartSearchTypesValid INT
	SET @StartSearchTypesValid = (SELECT COUNT(*) FROM @StartSearchTypes)

	DECLARE @StatusSearchTypesValid INT
	SET @StatusSearchTypesValid = (SELECT COUNT(*) FROM @StatusSearchTypes)

	IF(@isCallFromDashboard = 1)
	BEGIN
		SET @PageSize = 10
		SET @GlobalSearchText = 'Open'
	END

	DECLARE @BlacklistedBuyers TABLE (CompanyId INT PRIMARY KEY);
	INSERT INTO @BlacklistedBuyers (CompanyId)
	SELECT CompanyId FROM [dbo].[usf_GetBlacklistedCompanyIds](@CompanyId)

	;WITH CompanyLocations AS
	(  
		SELECT SXSS.AddressId,
		CAD.StateId AS SupplierStateId,
		SXSS.StateId AS SurvingStateId,
		CAD.CountryId,
		SXAS.IsStateWideService,
		SXAS.Radius,
		SXAS.IsHedgeOrderAllowed,
		SXAS.IsOverWaterRefuelingAllowed,
		CAD.Latitude,
		CAD.Longitude
		FROM [dbo].[CompanyAddresses] CAD
		LEFT JOIN [dbo].[SupplierAddressXServingStates] SXSS ON CAD.Id = SXSS.AddressId
		LEFT JOIN [dbo].[SupplierAddressXSettings]  SXAS ON CAD.Id = SXAS.AddressId
		WHERE CAD.IsActive = 1
		AND
		CompanyId = @CompanyId
		AND
		(@AddressId = 0 OR CAD.Id = @AddressId)
	),
  
	AllFuelRequests AS
	(
		SELECT FRQ.Id AS FuelRequestId,
		FRQ.FuelRequestTypeId,
		FXS.StatusId,
		FRQ.ParentId,
		COM.[Name] AS Customer,
		J.[Address],
		J.City,
		J.StateId,
		MST.Code AS [State],
		J.ZipCode,
		J.CompanyId,
		J.Latitude,
		J.Longitude,
		ISNULL(FRQ.TerminalId, 0) AS TerminalId,
		MPD.[Name] AS FuelType,
		MPD.ProductTypeId,
		FRQ.FuelTypeId,
		FRQ.OrderTypeId,
		FRQ.MaxQuantity AS GallonsNeeded,
		FRQ.PricePerGallon,
		FRQ.PricingTypeId,
		FRQ.RackAvgTypeId,
		FRD.StartDate,
		FRQ.RequestNumber,
		FRQ.IsPublicRequest,
		FRQ.SpotDroppedGallons + FRQ.HedgeDroppedGallons AS DeliveredTillNow,
		FRQ.CreationTimeRackPPG AS CreationTimeRackPPG,
		CASE WHEN SXDFR.FuelRequestId IS NOT NULL THEN 'Declined'
			WHEN (MFRST.Id = 3 AND O.AcceptedCompanyId <> @CompanyId) OR MFRST.Id = 6 THEN 'Missed'
			ELSE MFRST.Name END AS [StatusName],
		FXPS.PrivateSupplierListId,
		USR.CompanyId AS FrCompanyId
		FROM [dbo].[FuelRequests] FRQ
		INNER JOIN dbo.FuelRequestXStatuses FXS ON FRQ.Id = FXS.FuelRequestId AND FXS.IsActive = 1
		INNER JOIN [dbo].[MstFuelRequestStatuses] MFRST ON FXS.StatusId = MFRST.Id
		INNER JOIN [dbo].[MstProducts] MPD ON FRQ.FuelTypeId = MPD.Id
		INNER JOIN [dbo].[FuelRequestXDeliveryDetails] FRD ON FRQ.Id = FRD.FuelRequestId
		INNER JOIN [dbo].[Users] USR ON FRQ.CreatedBy = USR.Id
		--INNER JOIN [dbo].[UserXCompanies] UXC ON USR.Id = UXC.UserId
		INNER JOIN [dbo].[Companies] COM ON USR.CompanyId = COM.Id
		--INNER JOIN [dbo].[JobXFuelRequests] FRJ ON FRQ.Id = FRJ.FuelRequestId
		INNER JOIN [dbo].[Jobs] J ON J.Id = FRQ.JobId
		INNER JOIN [dbo].[MstStates] MST ON J.StateId = MST.Id
		--INNER JOIN [dbo].[CompanyXJobs] CXJ ON J.Id = CXJ.JobId
		LEFT JOIN  [dbo].[FuelRequestXPrivateSupplierLists] FXPS ON FRQ.Id = FXPS.FuelRequestId
		LEFT JOIN [dbo].[PrivateSupplierLists] PSLT ON FXPS.PrivateSupplierListId = PSLT.Id
		LEFT JOIN Orders O ON FRQ.Id = O.FuelRequestId
		LEFT JOIN SupplierXDeclinedFuelRequests SXDFR ON FRQ.Id = SXDFR.FuelRequestId AND SXDFR.SupplierId = @UserId
		WHERE FRQ.IsActive = 1
		AND
		(FXS.StatusId IN (2, 3, 4, 5) OR (FXS.StatusId = 6 AND [dbo].[usf_IsSupplierAcceptedCounterOffer](FRQ.Id, FRQ.CreatedBy, @CompanyId) = 0))  --Open, Accepted, Canceled, Expired status
		AND
		(FRQ.FuelRequestTypeId != 2  OR (FRQ.FuelRequestTypeId = 2 AND FXS.StatusId = 3 AND O.AcceptedCompanyId = @CompanyId))-- Don't show counter offers
		AND
		(
			(FRD.StartDate >= @StartDate AND FRD.StartDate < @EndDate)
			OR
			(FRQ.CreatedDate >= @StartDate AND FRQ.CreatedDate < @EndDate)
		)
	),

	FuelRequestWithServiceArea AS
	(
		SELECT OFRJ.FuelRequestId,
		OFRJ.FuelRequestTypeId,
		CASE WHEN StatusName = 'Missed' THEN 7 WHEN StatusName = 'Declined' THEN 8 ELSE OFRJ.StatusId END AS StatusId,
		OFRJ.ParentId,
		OFRJ.Customer,
		OFRJ.[Address],
		OFRJ.City,
		OFRJ.StateId,
		OFRJ.[State],
		OFRJ.ZipCode,
		OFRJ.CompanyId,
		OFRJ.TerminalId,
		OFRJ.FuelType,
		OFRJ.FuelTypeId,
		OFRJ.ProductTypeId,
		OFRJ.OrderTypeId,
		OFRJ.GallonsNeeded,
		OFRJ.PricePerGallon,
		OFRJ.PricingTypeId,
		OFRJ.RackAvgTypeId,
		OFRJ.StartDate,
		OFRJ.RequestNumber,
		OFRJ.IsPublicRequest,
		OFRJ.StatusName,
		OFRJ.PrivateSupplierListId,
		OFRJ.DeliveredTillNow,
		OFRJ.CreationTimeRackPPG,
		COML.AddressId,
		COML.SupplierStateId,
		COML.SurvingStateId,
		COML.CountryId,
		COML.IsStateWideService,
		COML.Radius,
		COML.IsHedgeOrderAllowed,
		COML.IsOverWaterRefuelingAllowed,
		[dbo].[usf_CalculateDistance](COML.Latitude,COML.Longitude,OFRJ.Latitude,OFRJ.Longitude) AS Distance
		FROM AllFuelRequests OFRJ
		INNER JOIN CompanyLocations COML ON COML.SurvingStateId = OFRJ.StateId
		WHERE FrCompanyId NOT IN (SELECT BLCI.CompanyId FROM @BlacklistedBuyers BLCI)
	),

	FuelRequestWitninServiceArea AS
	(
		SELECT *
		FROM FuelRequestWithServiceArea
		WHERE (StateId = SurvingStateId)
		AND
		(IsStateWideService = 1 OR Distance <= Radius)
		AND
		(IsHedgeOrderAllowed = 1 OR OrderTypeId = 2)
		AND
		CompanyId != @CompanyId
		AND
		(FuelRequestTypeId <> 3 OR @CompanyId NOT IN (SELECT BRCH.CompanyId FROM [dbo].[usf_GetBrokeredChain](FuelRequestId) BRCH))
	),
  
	QualifiedFuelRequests AS
	(
		SELECT FRWSA.*
		FROM FuelRequestWitninServiceArea FRWSA
		WHERE ProductTypeId IN (SELECT SAPT.ProductTypeId FROM [dbo].[SupplierAddressXProductTypes] SAPT WHERE SAPT.AddressId = FRWSA.AddressId)
		AND
		dbo.usf_IsSupplierQualificationMatched(FRWSA.FuelRequestId, FRWSA.AddressId) = 1
		AND
		(
			FRWSA.IsPublicRequest = 1
			OR
			@CompanyId IN (SELECT PSXSC.SupplierCompanyId FROM [dbo].[PrivateSupplierListXSupplierCompanies] PSXSC WHERE PSXSC.PrivateSupplierListId = FRWSA.PrivateSupplierListId)
		)  
		AND
		(-- Brokered fuel request (for Red-Dye-Diesel) of Texas should be visible to texas state suppliers only
			(CASE WHEN (FuelRequestTypeId = 3 AND ProductTypeId = 6 AND FRWSA.StateId = 43)
			THEN CASE WHEN (FRWSA.SupplierStateId = 43) THEN 1 ELSE 0 END
			ELSE 1 END) = 1
		)
	),

	FinalFuelRequestList AS
	(
		SELECT
		FuelRequestId,
		Customer,
		[Address],
		City,
		[State],
		ZipCode,
		TerminalId,
		FuelType,
		FuelTypeId,
		GallonsNeeded,
		PricePerGallon,
		PricingTypeId,
		RackAvgTypeId,
		StartDate,
		RequestNumber,
		StatusName,
		DeliveredTillNow,
		CreationTimeRackPPG,
		MIN(Distance) AS Distance,
		ISNULL(COUNTEROFFERSTATUS.BuyerStatus, 0) AS BuyerStatus,
		ISNULL(COUNTEROFFERSTATUS.SupplierStatus, 0) AS SupplierStatus
		FROM QualifiedFuelRequests QFR
		--CROSS APPLY [dbo].[usf_GetCounterOfferStatus](FuelRequestId, @UserId) AS COUNTEROFFERSTATUS
		LEFT JOIN 
		(
			SELECT TOP 1 
					BuyerStatus,
					SupplierStatus,
					OriginalFuelRequestId
			FROM	CounterOffers 
			WHERE	SupplierId = @UserId
			ORDER BY Id DESC, OriginalFuelRequestId DESC
		) COUNTEROFFERSTATUS ON QFR.FuelRequestId = COUNTEROFFERSTATUS.OriginalFuelRequestId
		WHERE (@Broadcast = 3 OR IsPublicRequest = (CASE WHEN @Broadcast = 1 THEN 0 ELSE 1 END)) AND (@StatusFilter = 0 OR StatusId = @StatusFilter)
		GROUP BY
		FuelRequestId,
		Customer,
		[Address],
		City,
		[State],
		ZipCode,
		TerminalId,
		FuelType,
		FuelTypeId,
		GallonsNeeded,
		PricePerGallon,
		PricingTypeId,
		RackAvgTypeId,
		StartDate,
		RequestNumber,
		StatusName,
		DeliveredTillNow,
		CreationTimeRackPPG,
		COUNTEROFFERSTATUS.BuyerStatus,
		COUNTEROFFERSTATUS.SupplierStatus
	),
 
	SupplierFinalFuelRequests AS
	(
		SELECT FuelRequestId,
		Customer,
		[Address],
		City,
		[State],
		ZipCode,
		FuelType,
		FuelTypeId,
		GallonsNeeded,
		dbo.[usf_GetPricePerGallon](PricePerGallon, PricingTypeId, RackAvgTypeId) AS PricePerGallon, 
		StartDate,
		RequestNumber,
		Distance,
		StatusName,
		DeliveredTillNow,
		BuyerStatus as CounterOfferBuyerStatus,
		SupplierStatus as CounterOfferSupplierStatus,
		(CASE WHEN BuyerStatus > 0 OR SupplierStatus > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) AS IsCounterOfferAvailable,
		(CASE WHEN BuyerStatus = 3 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END) AS IsCounterOfferDeclinedByBuyer,
		ISNULL(CASE PricingTypeId WHEN 2 THEN CAST((PricePerGallon * GallonsNeeded) AS decimal(18,2)) ELSE CAST((GallonsNeeded * CreationTimeRackPPG) AS decimal(18,2)) END,0) AS FrTotalDollarValue
		FROM FinalFuelRequestList  FRL
	)
	 
	-- Final SELECTION of columns to return
	SELECT FuelRequestId,  
	Customer,  
	[Address],  
	City,  
	[State],  
	ZipCode,  
	FuelType,  
	FuelTypeId,  
	GallonsNeeded,  
	PricePerGallon,    
	StartDate,  
	RequestNumber,  
	Distance,  
	StatusName,  
	DeliveredTillNow,  
	(City +  [State] +  ZipCode) AS [Address],
	[TotalCount]= COUNT(FuelRequestId) OVER(),
	CounterOfferBuyerStatus,
	CounterOfferSupplierStatus,
	IsCounterOfferAvailable,
	IsCounterOfferDeclinedByBuyer,
	FrTotalDollarValue
	FROM SupplierFinalFuelRequests  FRL
	WHERE 
		(@FuelRequestIdSearchTypesValid = 0 OR( @FuelRequestIdSearchTypesValid > 0 AND RequestNumber in (select RequestNumber from @FuelRequestIdSearchTypes where RequestNumber like '%' +  SearchVar + '%')))
		AND
		(@AddressSearchTypesValid = 0 OR( @AddressSearchTypesValid > 0 AND  (City +' '+  [State] +' '+  ZipCode) in (select  City +' '+  [State] +' '+ ZipCode from @AddressSearchTypes where (City +' '+  [State] +' '+  ZipCode) like '%' +  SearchVar + '%')))
		AND
		( @FuelTypeSearchTypesValid = 0 OR( @FuelTypeSearchTypesValid > 0 AND FuelType in (select FuelType from @FuelTypeSearchTypes where FuelType like '%' +  SearchVar + '%')))
		AND
		(@GallonsSearchTypesValid = 0 OR( @GallonsSearchTypesValid > 0 AND GallonsNeeded in (select GallonsNeeded from @GallonsSearchTypes where CAST(GallonsNeeded as varchar(100)) like '%' +  SearchVar + '%')))
		AND
		(@PriceSearchTypesValid = 0 OR( @PriceSearchTypesValid > 0 AND PricePerGallon  in (select PricePerGallon from @PriceSearchTypes where CAST(PricePerGallon  as varchar(100)) like '%' +  SearchVar + '%')))
		AND
		(@DistanceSearchTypesValid = 0 OR( @DistanceSearchTypesValid > 0 AND Distance in (select Distance from @DistanceSearchTypes where Distance like '%' +  SearchVar + '%')))
		AND
		(@StartSearchTypesValid = 0 OR( @StartSearchTypesValid > 0 AND  StartDate in (select StartDate from @StartSearchTypes where StartDate like '%' +  SearchVar + '%')))
		AND
		(@StatusSearchTypesValid = 0 OR( @StatusSearchTypesValid > 0 AND StatusName in (select StatusName from @StatusSearchTypes where StatusName like '%' +  SearchVar + '%')))
		AND
		(
			@GlobalSearchText IS NULL 
			OR
			(
				(RequestNumber like '%' + @GlobalSearchText+ '%')
				OR (FuelType like '%' + @GlobalSearchText+ '%')
				OR ((City +' '+  [State] +' '+ ZipCode) like '%' + @GlobalSearchText+ '%')
				OR (GallonsNeeded like '%' + @GlobalSearchText+ '%')
				OR (PricePerGallon like '%' + @GlobalSearchText+ '%')
				OR (Distance like '%' + @GlobalSearchText+ '%')
				OR (StartDate like '%' + @GlobalSearchText+ '%')
				OR (StatusName like '%' + @GlobalSearchText+ '%')
			)
		)
	ORDER BY
	CASE
	WHEN @SortId = 4 AND @SortDirection = 'asc' THEN GallonsNeeded
	WHEN @SortId = 6 AND @SortDirection = 'asc' THEN Distance
	END ASC,

	CASE
	WHEN @SortId = 7 AND @SortDirection = 'asc' THEN cast(StartDate As Datetime)
	END ASC,

	CASE
	WHEN @SortId = 2 AND @SortDirection = 'asc' THEN  ZipCode
	WHEN @SortId = 3 AND @SortDirection = 'asc' THEN FuelType
	WHEN @SortId = 0 AND @SortDirection = 'asc' THEN RequestNumber
	WHEN @SortId = 5 AND @SortDirection = 'asc' THEN PricePerGallon
	WHEN @SortId = 8 AND @SortDirection = 'asc' THEN StatusName
	END ASC,

	CASE
	WHEN @SortId = 4 AND @SortDirection = 'desc' THEN GallonsNeeded
	WHEN @SortId = 6 AND @SortDirection = 'desc' THEN Distance
	END DESC,

	CASE
	WHEN @SortId = 7 AND @SortDirection = 'desc' THEN cast(StartDate As Datetime)
	END DESC,

	CASE 
	WHEN @SortId = 2 AND @SortDirection = 'desc' THEN    ZipCode
	WHEN @SortId = 3 AND @SortDirection = 'desc' THEN FuelType 
	WHEN @SortId = 0 AND @SortDirection = 'desc' THEN RequestNumber 
	WHEN @SortId = 5 AND @SortDirection = 'desc' THEN PricePerGallon
	WHEN @SortId = 8 AND @SortDirection = 'desc' THEN StatusName 
	END DESC

	OFFSET (@PageNumber - 1) * @PageSize ROWS 
	FETCH NEXT @PageSize ROWS ONLY 
END  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
----------------------------------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[usp_GetSupplierFuelRequestStatForDashboard]   
	@CompanyId		INT,
	@UserId		INT,
	@FuelTypeId	INT = 0
AS  
BEGIN
	DECLARE @AcceptedFrCount	INT = 0;
	DECLARE @ExpiredFrCount		INT = 0;
	DECLARE @MissedFrCount		INT = 0;
	DECLARE @DeclinedFrCount	INT = 0;
	DECLARE @CounterOfferCount	INT = 0;
	DECLARE @TotalFrCount		INT = 0;
	DECLARE @BusinessYouWon		DECIMAL(18,2) = 0.0;
	DECLARE @BusinessYouMissed	DECIMAL(18,2) = 0.0;
	DECLARE @BusinessInYourArea	DECIMAL(18,2) = 0.0;
	DECLARE @TotalRequestedGallons	DECIMAL(18,2) = 0.0;
	DECLARE @TotalAcceptedGallons	DECIMAL(18,2) = 0.0;
	DECLARE @TotalDeliveredGallons	DECIMAL(18,2) = 0.0;
	DECLARE @TotalMissedGallons	DECIMAL(18,2) = 0.0;
	DECLARE @TotalExpiredGallons	DECIMAL(18,2) = 0.0;
	;WITH LastLevelCounterOffer_CTE AS 
	(
		SELECT dbo.usf_GetOriginalParentFuelRequest(FRS.ID) AS OriginalParent, FuelRequestId
		FROM CounterOffers CO
		INNER JOIN FuelRequests FRS ON FRS.Id = CO.FuelRequestId AND IsActive = 1
		--INNER JOIN UserXCompanies UXC ON UXC.CompanyId = @CompanyId AND UXC.UserId = CO.SupplierId
		INNER JOIN Users UXC ON UXC.CompanyId = @CompanyId AND UXC.Id = CO.SupplierId
		WHERE (BuyerStatus > 1  AND BuyerStatus < 5) OR  (SupplierStatus > 1 AND SupplierStatus  < 5)
	),
	OriginalFuelRequest_CTE AS
	(
		SELECT  dbo.usf_GetOriginalParentFuelRequest(FuelRequestId) AS originalFR 
		FROM CounterOffers CO
		INNER JOIN FuelRequests FR ON FR.Id = CO.FuelRequestId AND IsActive = 1
		--INNER JOIN UserXCompanies UXC ON UXC.UserId = CO.SupplierId AND  UXC.CompanyId = @CompanyId 
		INNER JOIN Users UXC ON UXC.Id = CO.SupplierId AND  UXC.CompanyId = @CompanyId 
		WHERE  (BuyerStatus = 1  AND SupplierStatus IS NULL ) OR  (SupplierStatus = 1  AND BuyerStatus IS NULL)
	)
  
	SELECT @CounterOfferCount = Count(DISTINCT originalFR) 
	FROM OriginalFuelRequest_CTE
	WHERE originalFR NOT IN (SELECT OriginalParent FROM LastLevelCounterOffer_CTE);
	WITH CompanyLocations AS
	(
		SELECT SXSS.AddressId,
		CAD.StateId AS SupplierStateId,
		SXSS.StateId AS SurvingStateId,
		CAD.CountryId,
		SXAS.IsStateWideService,
		SXAS.Radius,
		SXAS.IsHedgeOrderAllowed,
		SXAS.IsOverWaterRefuelingAllowed,
		CAD.Latitude,
		CAD.Longitude
		FROM [dbo].[CompanyAddresses] CAD
		LEFT JOIN [dbo].[SupplierAddressXServingStates] SXSS ON CAD.Id = SXSS.AddressId
		LEFT JOIN [dbo].[SupplierAddressXSettings]  SXAS ON CAD.Id = SXAS.AddressId
		WHERE CAD.IsActive = 1
		AND
		CompanyId = @CompanyId
	),
	AllFuelRequests AS
	(
		SELECT FRQ.Id AS FuelRequestId,
		FRQ.FuelRequestTypeId,
		FXS.StatusId,
		FRQ.ParentId,
		COM.[Name] AS Customer,
		J.[Address],
		J.City,
		J.StateId,
		MST.Code AS [State],
		J.ZipCode,
		J.CompanyId,
		J.Latitude,
		J.Longitude,
		CASE WHEN FRQ.TerminalId IS NULL THEN 0 ELSE FRQ.TerminalId END AS TerminalId,
		MPD.[Name] AS FuelType,
		MPD.ProductTypeId,
		FRQ.FuelTypeId,
		FRQ.OrderTypeId,
		FRQ.MaxQuantity AS GallonsNeeded,
		FRQ.PricePerGallon AS PricePerGallon,
		FRQ.PricingTypeId,
		FRQ.RackAvgTypeId,
		FRD.StartDate AS StartDate,
		FRQ.RequestNumber,
		FRQ.IsPublicRequest,
		FRQ.SpotDroppedGallons + FRQ.HedgeDroppedGallons AS DeliveredTillNow,
		FRQ.CreationTimeRackPPG AS CreationTimeRackPPG,
		O.AcceptedCompanyId,
		CASE WHEN FRQ.Id IN (SELECT SXDFR.FuelRequestId FROM [dbo].[SupplierXDeclinedFuelRequests] SXDFR WHERE SXDFR.SupplierId = @UserId) THEN 'Declined'
			WHEN (MFRST.Id = 3 AND O.AcceptedCompanyId <> @CompanyId) OR MFRST.Id = 6 THEN 'Missed'
			ELSE MFRST.Name END AS [StatusName],
		FXPS.PrivateSupplierListId,
		USR.CompanyId AS FrCompanyId
		FROM [dbo].[FuelRequests] FRQ
		INNER JOIN dbo.FuelRequestXStatuses FXS ON FRQ.Id = FXS.FuelRequestId AND FXS.IsActive = 1
		INNER JOIN [dbo].[MstFuelRequestStatuses] MFRST ON FXS.StatusId = MFRST.Id
		INNER JOIN [dbo].[MstProducts] MPD ON FRQ.FuelTypeId = MPD.Id
		INNER JOIN [dbo].[FuelRequestXDeliveryDetails] FRD ON FRQ.Id = FRD.FuelRequestId
		INNER JOIN [dbo].[Users] USR ON FRQ.CreatedBy = USR.Id
		--INNER JOIN [dbo].[UserXCompanies] UXC ON USR.Id = UXC.UserId
		INNER JOIN [dbo].[Companies] COM ON USR.CompanyId = COM.Id
		--INNER JOIN [dbo].[JobXFuelRequests] FRJ ON FRQ.Id = FRJ.FuelRequestId
		INNER JOIN [dbo].[Jobs] J ON J.Id = FRQ.JobId
		INNER JOIN [dbo].[MstStates] MST ON J.StateId = MST.Id 
		--INNER JOIN [dbo].[CompanyXJobs] CXJ ON J.Id = CXJ.JobId
		LEFT JOIN  [dbo].[FuelRequestXPrivateSupplierLists] FXPS ON FRQ.Id = FXPS.FuelRequestId
		LEFT JOIN [dbo].[PrivateSupplierLists] PSLT ON FXPS.PrivateSupplierListId = PSLT.Id
		LEFT JOIN Orders O ON FRQ.Id = O.FuelRequestId
		WHERE FRQ.IsActive = 1
		AND
		(FXS.StatusId IN (2, 3, 4, 5) OR (FXS.StatusId = 6 AND [dbo].[usf_IsSupplierAcceptedCounterOffer](FRQ.Id, FRQ.CreatedBy, @CompanyId) = 0))  --Open, Accepted, Canceled, Expired status
		AND
		(FRQ.FuelRequestTypeId != 2  OR (FRQ.FuelRequestTypeId = 2 AND FXS.StatusId = 3 AND O.AcceptedCompanyId = @CompanyId))-- Don't show counter offers
	),
	FuelRequestWithServiceArea AS
	(  
		SELECT OFRJ.FuelRequestId,
		OFRJ.FuelRequestTypeId,
		OFRJ.StatusId,
		OFRJ.ParentId,
		OFRJ.Customer,
		OFRJ.[Address],
		OFRJ.City,
		OFRJ.StateId,
		OFRJ.[State],
		OFRJ.ZipCode,
		OFRJ.CompanyId,
		OFRJ.TerminalId,
		OFRJ.FuelType,
		OFRJ.FuelTypeId,
		OFRJ.ProductTypeId,
		OFRJ.OrderTypeId,
		OFRJ.GallonsNeeded,
		OFRJ.PricePerGallon,
		OFRJ.PricingTypeId,
		OFRJ.RackAvgTypeId,
		OFRJ.StartDate,
		OFRJ.RequestNumber,
		OFRJ.IsPublicRequest,
		OFRJ.StatusName,
		OFRJ.PrivateSupplierListId,
		OFRJ.DeliveredTillNow,
		OFRJ.CreationTimeRackPPG,
		COML.AddressId,
		COML.SupplierStateId,
		COML.SurvingStateId,
		COML.CountryId,
		COML.IsStateWideService,
		COML.Radius,
		COML.IsHedgeOrderAllowed,
		COML.IsOverWaterRefuelingAllowed,
		OFRJ.AcceptedCompanyId,
		[dbo].[usf_CalculateDistance](COML.Latitude,COML.Longitude,OFRJ.Latitude,OFRJ.Longitude) AS Distance
		FROM AllFuelRequests OFRJ
		INNER JOIN CompanyLocations COML  ON OFRJ.StateId = COML.SurvingStateId
		--CROSS APPLY CompanyLocations COML  --ON OFRJ.StateId = COML.SurvingStateId
		WHERE @CompanyId NOT IN (SELECT BLCI.CompanyId FROM [dbo].[usf_GetBlacklistedCompanyIds](FrCompanyId) BLCI)
		AND
		FrCompanyId NOT IN (SELECT BLCI.CompanyId FROM [dbo].[usf_GetBlacklistedCompanyIds](@CompanyId) BLCI)
	),  
  
	FuelRequestWitninServiceArea AS  
	(  
		SELECT *  
		FROM FuelRequestWithServiceArea  
		WHERE (StateId = SurvingStateId)  
		AND  
		(IsStateWideService = 1 OR Distance <= Radius)  
		AND  
		(IsHedgeOrderAllowed = 1 OR OrderTypeId = 2)  
		AND  
		CompanyId != @CompanyId  
		AND  
		(FuelRequestTypeId <> 3 OR @CompanyId NOT IN (SELECT BRCH.CompanyId FROM [dbo].[usf_GetBrokeredChain](FuelRequestId) BRCH))  
	),
	QualifiedFuelRequests AS
	(  
		SELECT FRWSA.*
		FROM FuelRequestWitninServiceArea FRWSA
		WHERE ProductTypeId IN (SELECT SAPT.ProductTypeId FROM [dbo].[SupplierAddressXProductTypes] SAPT WHERE SAPT.AddressId = FRWSA.AddressId)
		AND
		dbo.usf_IsSupplierQualificationMatched(FRWSA.FuelRequestId, FRWSA.AddressId) = 1
		AND
		(
			FRWSA.IsPublicRequest = 1
			OR
			@CompanyId IN (SELECT PSXSC.SupplierCompanyId FROM [dbo].[PrivateSupplierListXSupplierCompanies] PSXSC WHERE PSXSC.PrivateSupplierListId = FRWSA.PrivateSupplierListId)
		)
		AND
		(-- Brokered fuel request (for Red-Dye-Diesel) of Texas should be visible to texas state suppliers only  
			(CASE WHEN (FuelRequestTypeId = 3 AND ProductTypeId = 6 AND FRWSA.StateId = 43)
			THEN CASE WHEN (FRWSA.SupplierStateId = 43) THEN 1 ELSE 0 END
			ELSE 1 END) = 1
		)
	)
	-- Final SELECTION of columns to return
	SELECT FuelRequestId,  
	FuelType,  
	FuelTypeId,  
	GallonsNeeded,  
	PricePerGallon,  
	PricingTypeId,  
	RackAvgTypeId,  
	StatusName,  
	DeliveredTillNow,
	AcceptedCompanyId,
	ISNULL(CASE PricingTypeId WHEN 2 THEN CAST((PricePerGallon * GallonsNeeded) AS decimal(18,2)) ELSE CAST((GallonsNeeded * CreationTimeRackPPG) AS decimal(18,2)) END,0) AS FrTotalDollarValue
	INTO #SupplierFrStat
	FROM QualifiedFuelRequests  FRL
	WHERE @FuelTypeId = 0 OR FuelTypeId = @FuelTypeId
	GROUP BY 
	FuelRequestId,  
	FuelType,  
	FuelTypeId,  
	GallonsNeeded,  
	PricePerGallon,  
	PricingTypeId,  
	RackAvgTypeId,  
	StatusName,  
	DeliveredTillNow,
	AcceptedCompanyId,
	CreationTimeRackPPG

	SELECT @AcceptedFrCount = COUNT(FuelRequestId), @BusinessYouWon = SUM(PricePerGallon * GallonsNeeded), @TotalAcceptedGallons = SUM(GallonsNeeded) FROM #SupplierFrStat WHERE StatusName = 'Accepted'
	SELECT @MissedFrCount = COUNT(FuelRequestId), @BusinessYouMissed = SUM(PricePerGallon * GallonsNeeded), @TotalMissedGallons = SUM(GallonsNeeded) FROM #SupplierFrStat WHERE StatusName = 'Missed'
	SELECT @ExpiredFrCount = COUNT(FuelRequestId), @TotalExpiredGallons = SUM(GallonsNeeded) FROM #SupplierFrStat WHERE StatusName = 'Expired'
	SELECT @DeclinedFrCount = COUNT(FuelRequestId) FROM #SupplierFrStat WHERE StatusName = 'Declined'
	
	SELECT @TotalFrCount = COUNT(FuelRequestId), @BusinessInYourArea = SUM(FrTotalDollarValue), @TotalRequestedGallons = SUM(GallonsNeeded) FROM #SupplierFrStat
	SELECT @TotalDeliveredGallons = SUM(DeliveredTillNow), @TotalFrCount = COUNT(FuelRequestId) FROM #SupplierFrStat WHERE AcceptedCompanyId = @CompanyId
	--SELECT FuelType, FuelTypeId FROM #SupplierFrStat
 
	SELECT DISTINCT FuelType, FuelTypeId, 
	ISNULL(@AcceptedFrCount,0) AS AcceptedFrCount, 
	ISNULL(@MissedFrCount, 0) AS MissedFrCount, 
	ISNULL(@ExpiredFrCount,0) AS ExpiredFrCount, 
	ISNULL(@DeclinedFrCount,0) AS DeclinedFrCount, 
	ISNULL(@CounterOfferCount,0) AS CounterOfferCount,
	ISNULL(@TotalRequestedGallons,0) AS TotalRequestedGallons, 
	ISNULL(@TotalAcceptedGallons,0) AS TotalAcceptedGallons, 
	ISNULL(@TotalDeliveredGallons,0) AS TotalDeliveredGallons, 
	ISNULL(@TotalMissedGallons,0) AS TotalMissedGallons, 
	ISNULL(@TotalExpiredGallons,0) AS TotalExpiredGallons,
	ISNULL(@BusinessYouWon,0) AS BusinessYouWon, 
	ISNULL(@BusinessYouMissed,0) AS BusinessYouMissed, 
	ISNULL(@BusinessInYourArea,0) AS BusinessInYourArea, 
	ISNULL(@TotalFrCount,0) AS TotalFrCount 
	FROM #SupplierFrStat
	DROP TABLE IF EXISTS #SupplierFrStat
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 09-Jan-2018
-- Description:	Returns supplier invoices of specified company
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetSupplierInvoices]
	@CompanyId		INT,
	@OrderId		INT,
	@InvoiceTypeId	INT,
	@InvoiceFilter	INT,
	@ViewInvoices	INT,
	@StartDate		DATETIMEOFFSET(7),
	@EndDate		DATETIMEOFFSET(7),
	@GlobalSearchText VARCHAR(30) = NULL, 
    @SortId           INT = 0, 
    @SortDirection    VARCHAR(6) = 'desc', 
    @PageSize         INT = 99999999, 
    @PageNumber       INT = 1, 
	@InvoiceNumberSearchTypes [dbo].SEARCHTYPES Readonly,
    @PoNumberSearchTypes [dbo].SEARCHTYPES Readonly, 
    @SupplierSearchTypes [dbo].SEARCHTYPES Readonly, 
    @LocationSearchTypes [dbo].SEARCHTYPES Readonly, 
    @FuelTypeSearchTypes [dbo].SEARCHTYPES Readonly, 
    @TerminalSearchTypes [dbo].SEARCHTYPES Readonly, 
    @AmountSearchTypes [dbo].SEARCHTYPES Readonly, 
    @DropDateSearchTypes [dbo].SEARCHTYPES Readonly, 
    @DropTimeSearchTypes [dbo].SEARCHTYPES Readonly, 
    @PriceSearchTypes [dbo].SEARCHTYPES Readonly, 
	@InvoiceDateSearchTypes [dbo].SEARCHTYPES Readonly, 
	@DueDateSearchTypes [dbo].SEARCHTYPES Readonly, 
	@DriverSearchTypes [dbo].SEARCHTYPES Readonly, 
    @StatusSearchTypes [dbo].SEARCHTYPES Readonly
AS
BEGIN
    DECLARE @InvoiceNumberSearchTypesValid INT 
    SET @InvoiceNumberSearchTypesValid = ( SELECT Count(*) FROM   @InvoiceNumberSearchTypes) 
    DECLARE @PoNumberSearchTypesValid INT 
    SET @PoNumberSearchTypesValid = ( SELECT Count(*) FROM   @PoNumberSearchTypes) 
    DECLARE @SupplierSearchTypesValid INT 
    SET @SupplierSearchTypesValid = ( SELECT Count(*)  FROM   @SupplierSearchTypes) 
    DECLARE @LocationSearchTypesValid INT 
    SET @LocationSearchTypesValid =  ( SELECT Count(*)  FROM   @LocationSearchTypes) 
    DECLARE @FuelTypeSearchTypesValid INT 
    SET @FuelTypeSearchTypesValid =  ( SELECT Count(*)  FROM   @FuelTypeSearchTypes) 
    DECLARE @TerminalSearchTypesValid INT 
    SET @TerminalSearchTypesValid =  (  SELECT Count(*)   FROM   @TerminalSearchTypes) 
    DECLARE @DropDateSearchTypesValid INT 
    SET @DropDateSearchTypesValid =  (  SELECT Count(*)   FROM   @DropDateSearchTypes) 
    DECLARE @PriceSearchTypesValid INT 
    SET @PriceSearchTypesValid =   (   SELECT Count(*)   FROM   @PriceSearchTypes) 
    DECLARE @AmountSearchTypesValid INT 
    SET @AmountSearchTypesValid = ( SELECT Count(*)  FROM   @AmountSearchTypes) 
    DECLARE @DropTimeSearchTypesValid INT 
    SET @DropTimeSearchTypesValid = (  SELECT Count(*)  FROM   @DropTimeSearchTypes) 
    DECLARE @StatusSearchTypesValid INT 
    SET @StatusSearchTypesValid =   (   SELECT Count(*)  FROM   @StatusSearchTypes) 
	DECLARE @InvoiceDateSearchTypesValid INT 
    SET @InvoiceDateSearchTypesValid =   (   SELECT Count(*)  FROM   @InvoiceDateSearchTypes) 
	DECLARE @DueDateSearchTypesValid INT 
    SET @DueDateSearchTypesValid =   (   SELECT Count(*)  FROM   @DueDateSearchTypes) 
	DECLARE @DriverSearchTypesValid INT 
    SET @DriverSearchTypesValid =   (   SELECT Count(*)  FROM   @DriverSearchTypes) 

;WITH SupplierInvoices AS (
	SELECT	INV.Id,
			ISNULL(INV.OrderId, 0) AS OrderId,
			CASE WHEN INV.OrderId IS NULL THEN '--' ELSE (
				CASE WHEN I_USR.CompanyId != @CompanyId THEN I_COM.[Name] 
					 WHEN FRQ.FuelRequestTypeId = 2 THEN C_COM.[Name]
					 ELSE F_COM.[Name]  END
			) END AS Supplier,
			CASE WHEN INV.OrderId IS NULL THEN '--' WHEN INV.InvoiceTypeId = 5 THEN 'Dry Run Fee' ELSE PRD.[Name] END AS FuelType,
			INM.Number AS InvoiceNumber,
			CASE WHEN INV.OrderId IS NULL THEN '--' ELSE ISNULL(FRQ.ExternalPoNumber, CONVERT(NVARCHAR(16), ORD.PoNumber)) END AS PoNumber,
			CASE WHEN INV.OrderId IS NULL THEN 0 ELSE (
				INV.BasicAmount + 
				(CASE WHEN INV.InvoiceTypeId = 5 THEN 0 ELSE dbo.usf_GetInvoiceTotalFees(INV.Id) END) +
				(CASE WHEN INV.InvoiceTypeId = 6 AND INV.InvoiceTypeId = 7 THEN 0 
				 ELSE 
					CASE WHEN INV.TotalTaxAmount > 0 THEN INV.TotalTaxAmount ELSE (INV.StateTax + INV.FedTax + INV.SalesTax) END
				 END)
			) END AS InvoiceAmount,
			CONVERT(NVARCHAR(10), INV.DropEndDate, 101) AS DropDate,
			FORMAT(INV.DropStartDate,'h:mm tt') + ' - ' + FORMAT(INV.DropEndDate,'h:mm tt') AS DropTime,
			CASE WHEN IXS.StatusId = 10 THEN '--' ELSE CONVERT(NVARCHAR(10), INV.CreatedDate, 101) END AS InvoiceDate,
			CONVERT(NVARCHAR(10), INV.PaymentDueDate, 101) AS PaymentDueDate,
			IST.[Name] AS [Status],
			INV.InvoiceNumberId,
			MET.[Name] AS TerminalName,
			CASE WHEN D_USR.Id IS NULL THEN '--' ELSE D_USR.FirstName + ' ' + SUBSTRING(D_USR.LastName,1,1) END AS DriverName,
			JBS.City + ', ' + MST.Code + ' ' + JBS.ZipCode  AS [Location],
			INV.PricePerGallon 
	FROM	dbo.Invoices INV
	INNER JOIN dbo.InvoiceNumbers INM ON INV.InvoiceNumberId = INM.Id
	INNER JOIN dbo.InvoiceXInvoiceStatusDetails IXS ON INV.Id = IXS.InvoiceId AND IXS.IsActive = 1
	INNER JOIN dbo.MstInvoiceStatuses IST ON IXS.StatusId = IST.Id
	INNER JOIN dbo.Users I_USR ON INV.CreatedBy = I_USR.Id
	--INNER JOIN dbo.UserXCompanies I_UXC ON I_USR.Id = I_UXC.UserId
	INNER JOIN dbo.Companies I_COM ON I_USR.CompanyId = I_COM.Id
	LEFT JOIN dbo.Orders ORD ON INV.OrderId = ORD.Id
	LEFT JOIN dbo.FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id
	--INNER JOIN JobXFuelRequests JXF ON FRQ.Id = JXF.FuelRequestId
	INNER JOIN Jobs JBS ON FRQ.JobId = JBS.Id
	INNER JOIN MstStates MST ON JBS.StateId = MST.Id
	LEFT JOIN dbo.MstProducts PRD ON FRQ.FuelTypeId = PRD.Id
	LEFT JOIN dbo.MstExternalTerminals MET ON INV.TerminalId = MET.Id
	LEFT JOIN dbo.Users D_USR ON INV.DriverId = D_USR.Id
	LEFT JOIN dbo.Users F_USR ON FRQ.CreatedBy = F_USR.Id
	--LEFT JOIN dbo.UserXCompanies F_UXC ON F_USR.Id = F_UXC.UserId
	LEFT JOIN dbo.Companies F_COM ON F_USR.CompanyId = F_COM.Id
	LEFT JOIN dbo.CounterOffers COF ON FRQ.Id = COF.FuelRequestId AND COF.BuyerStatus = 2
	LEFT JOIN dbo.Users C_USR ON COF.BuyerId = C_USR.Id
	--LEFT JOIN dbo.UserXCompanies C_UXC ON C_USR.Id = C_UXC.UserId
	LEFT JOIN dbo.Companies C_COM ON C_USR.CompanyId = C_COM.Id
	WHERE	(INV.InvoiceVersionStatusId = 1 OR IXS.StatusId=11)
			AND
			(
				(INV.OrderId IS NULL AND I_USR.CompanyId = @CompanyId)
				OR
				(INV.OrderId IS NOT NULL AND ORD.AcceptedCompanyId = @CompanyId)
				OR
				(FRQ.FuelRequestTypeId = 3 AND ORD.BuyerCompanyId = @CompanyId)
			)
			AND
			(
				(@InvoiceTypeId IN (6, 7) AND INV.InvoiceTypeId IN (6, 7))
				OR
				(@InvoiceTypeId NOT IN (6, 7) AND INV.InvoiceTypeId NOT IN (6, 7))
			)
			AND
			(
				INV.OrderId = @OrderId
				OR
				(
					@OrderId = 0 AND INV.CreatedDate >= @StartDate AND INV.CreatedDate < @EndDate
				)
			)
			AND
			(
				CASE	WHEN @InvoiceFilter = 0 THEN 1	--Invoice Filters
						WHEN @InvoiceFilter = 2 AND IXS.StatusId = 2 THEN 1
						WHEN @InvoiceFilter = 3 AND IXS.StatusId = 3 THEN 1
						WHEN @InvoiceFilter = 4 AND IXS.StatusId = 4 AND 
						(
							(SELECT COUNT(IXIS.StatusId) FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id) = 1 
							OR 
							2 = ANY(SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id)
						) THEN 1
						WHEN @InvoiceFilter = 6 AND IXS.StatusId = 6 THEN 1
						WHEN @InvoiceFilter = 10 AND IXS.StatusId = 8 THEN 1
						WHEN @InvoiceFilter IN (13, 14) AND 
						(
							4 = ANY(SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id AND IXIS.IsActive = 1)
							AND
							2 != ANY(SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id)
						) THEN 1
						WHEN @InvoiceFilter = 15 THEN 1
						WHEN @InvoiceFilter = 16 AND INV.OrderId IS NOT NULL AND ORD.AcceptedCompanyId != @CompanyId THEN 1
						WHEN @InvoiceFilter = 17 AND (INV.OrderId IS NULL OR ORD.AcceptedCompanyId = @CompanyId) THEN 1
						ELSE 0
				END
			) = 1
			AND
			(
				@ViewInvoices NOT IN (2, 3)
				OR
				(@ViewInvoices = 2 AND INV.OrderId IS NOT NULL AND ORD.AcceptedCompanyId != @CompanyId)
				OR
				(@ViewInvoices = 3 AND (INV.OrderId IS NULL OR ORD.AcceptedCompanyId = @CompanyId))
			)
)

SELECT *, [TotalCount]= Count(Id) OVER() FROM SupplierInvoices
WHERE    ( 
                      @InvoiceNumberSearchTypesValid = 0 
             OR       ( 
                               @InvoiceNumberSearchTypesValid > 0 
                      AND      InvoiceNumber IN 
                               ( 
                                      SELECT InvoiceNumber 
                                      FROM   @InvoiceNumberSearchTypes 
                                      WHERE  InvoiceNumber IN (SearchVar)))) 
    AND      ( 
                      @PoNumberSearchTypesValid = 0 
             OR       ( 
                               @PoNumberSearchTypesValid > 0 
                      AND      PoNumber IN 
                               ( 
                                      SELECT PoNumber 
                                      FROM   @PoNumberSearchTypes 
                                      WHERE  PoNumber IN (SearchVar)))) 
    AND      ( 
                      @SupplierSearchTypesValid = 0 
             OR       ( 
                               @SupplierSearchTypesValid > 0 
                      AND      Supplier IN 
                               ( 
                                      SELECT Supplier 
                                      FROM   @SupplierSearchTypes 
                                      WHERE  Supplier IN (SearchVar)))) 
    AND      ( 
                      @LocationSearchTypesValid = 0 
             OR       ( 
                               @LocationSearchTypesValid > 0 
                      AND      [Location] IN 
                               ( 
                                      SELECT [Location] 
                                      FROM   @LocationSearchTypes 
                                      WHERE  REPLACE([Location],',','') IN (SearchVar)))) 
    AND      ( 
                      @FuelTypeSearchTypesValid = 0 
             OR       ( 
                               @FuelTypeSearchTypesValid > 0 
                      AND       FuelType IN 
                                        ( 
                                               SELECT FuelType 
                                               FROM   @FuelTypeSearchTypes 
                                               WHERE  FuelType IN (SearchVar)))) 
             AND      ( 
                               @TerminalSearchTypesValid = 0 
                      OR       ( 
                                        @TerminalSearchTypesValid > 0 
                               AND      TerminalName IN 
                                        ( 
                                               SELECT TerminalName 
                                               FROM   @TerminalSearchTypes 
                                               WHERE  TerminalName IN (SearchVar)))) 
             AND      ( 
                               @DropDateSearchTypesValid = 0 
                      OR       ( 
                                        @DropDateSearchTypesValid > 0 
                               AND      DropDate IN 
                                        ( 
                                               SELECT DropDate 
                                               FROM   @DropDateSearchTypes 
                                               WHERE  DropDate IN (SearchVar)))) 
             AND      ( 
                               @PriceSearchTypesValid = 0 
                      OR       ( 
                                        @PriceSearchTypesValid > 0 
                               AND      PricePerGallon IN 
                                        ( 
                                               SELECT PricePerGallon 
                                               FROM   @PriceSearchTypes 
                                               WHERE  PricePerGallon IN (REPLACE(SearchVar,'$',''))))) 
             AND      ( 
                               @AmountSearchTypesValid = 0 
                      OR       ( 
                                        @AmountSearchTypesValid > 0 
                               AND      InvoiceAmount IN 
                                        ( 
                                               SELECT InvoiceAmount 
                                               FROM   @AmountSearchTypes 
                                                WHERE  InvoiceAmount IN (REPLACE(SearchVar,'$',''))))) 
             AND      ( 
                               @DropTimeSearchTypesValid = 0 
                      OR       ( 
                                        @DropTimeSearchTypesValid > 0 
                               AND      DropTime IN 
                                        ( 
                                               SELECT DropTime 
                                               FROM   @DropTimeSearchTypes 
                                               WHERE  DropTime IN (SearchVar))))
             AND      ( 
                               @StatusSearchTypesValid = 0 
                      OR       ( 
                                        @StatusSearchTypesValid > 0 
                               AND      [Status] IN 
                                        ( 
                                               SELECT [Status] 
                                               FROM   @StatusSearchTypes 
                                               WHERE  [Status] IN (SearchVar)))) 
			 AND      ( 
                               @InvoiceDateSearchTypesValid = 0 
                      OR       ( 
                                        @InvoiceDateSearchTypesValid > 0 
                               AND      InvoiceDate IN 
                                        ( 
                                               SELECT InvoiceDate 
                                               FROM   @InvoiceDateSearchTypes 
                                               WHERE  InvoiceDate IN (SearchVar)))) 
			AND      ( 
                               @DueDateSearchTypesValid = 0 
                      OR       ( 
                                        @DueDateSearchTypesValid > 0 
                               AND      PaymentDueDate IN 
                                        ( 
                                               SELECT PaymentDueDate 
                                               FROM   @DueDateSearchTypes 
                                               WHERE  PaymentDueDate IN (SearchVar)))) 
			 AND      ( 
                               @DriverSearchTypesValid = 0 
                      OR       ( 
                                        @DriverSearchTypesValid > 0 
                               AND      DriverName IN 
                                        ( 
                                               SELECT DriverName 
                                               FROM   @DriverSearchTypes 
                                               WHERE  DriverName IN (SearchVar)))) 
             AND      ( 
                               @GlobalSearchText IS NULL 
                      OR       (
									( InvoiceNumber LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 PoNumber LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 REPLACE([Location],',','') LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 Supplier LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 FuelType LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 TerminalName LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 DropDate LIKE '%' + @GlobalSearchText+ '%') 
                               OR       ( 
                                                 PricePerGallon LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 InvoiceAmount LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 DropTime LIKE '%' + @GlobalSearchText+ '%')
                               OR       ( 
                                                 [Status] LIKE '%' + @GlobalSearchText+ '%')
							   OR       ( 
                                                 InvoiceDate LIKE '%' + @GlobalSearchText+ '%')
							   OR       ( 
                                                 PaymentDueDate LIKE '%' + @GlobalSearchText+ '%')
							   OR       ( 
                                                 DriverName LIKE '%' + @GlobalSearchText+ '%')) )
    ORDER BY 
			CASE      WHEN @SortId = 6 
                      AND      @SortDirection = 'asc' THEN InvoiceAmount 
					  WHEN @SortId = 9 
                      AND      @SortDirection = 'asc' THEN PricePerGallon 
					  END ASC,
             CASE 
                      WHEN @SortId = 7 
                      AND      @SortDirection = 'asc' THEN cast(DropDate As Datetime) 
                      WHEN @SortId = 10 
                      AND      @SortDirection = 'asc' THEN cast(InvoiceDate As Datetime) 
					  WHEN @SortId = 11
					  AND      @SortDirection = 'asc' THEN cast(PaymentDueDate As Datetime) 
             END ASC, 
             CASE 
                      WHEN @SortId = 0 
                      AND      @SortDirection = 'asc' THEN InvoiceNumber 
                      WHEN @SortId = 1 
                      AND      @SortDirection = 'asc' THEN PoNumber 
                      WHEN @SortId = 2 
                      AND      @SortDirection = 'asc' THEN Supplier 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'asc' THEN [Location] 
					  WHEN @SortId = 4 
                      AND      @SortDirection = 'asc' THEN FuelType
					  WHEN @SortId = 5 
                      AND      @SortDirection = 'asc' THEN TerminalName 
                      WHEN @SortId = 8 
                      AND      @SortDirection = 'asc' THEN DropTime
					  WHEN @SortId = 12 
                      AND      @SortDirection = 'asc' THEN DriverName 
                      WHEN @SortId = 13 
                      AND      @SortDirection = 'asc' THEN [Status] 
             END ASC, 
			CASE      WHEN @SortId = 6 
                      AND      @SortDirection = 'desc' THEN InvoiceAmount 
					  WHEN @SortId = 9 
                      AND      @SortDirection = 'desc' THEN PricePerGallon 
					  END DESC,
             CASE 
                      WHEN @SortId = 7 
                      AND      @SortDirection = 'desc' THEN cast(DropDate As Datetime) 
                      WHEN @SortId = 10 
                      AND      @SortDirection = 'desc' THEN cast(InvoiceDate As Datetime) 
					  WHEN @SortId = 11
					  AND      @SortDirection = 'desc' THEN cast(PaymentDueDate As Datetime) 
             END DESC, 
             CASE 
                      WHEN @SortId = 0 
                      AND      @SortDirection = 'desc' THEN InvoiceNumber 
                      WHEN @SortId = 1 
                      AND      @SortDirection = 'desc' THEN PoNumber 
                      WHEN @SortId = 2 
                      AND      @SortDirection = 'desc' THEN Supplier 
                      WHEN @SortId = 3 
                      AND      @SortDirection = 'desc' THEN [Location] 
					  WHEN @SortId = 4 
                      AND      @SortDirection = 'desc' THEN FuelType
					  WHEN @SortId = 5 
                      AND      @SortDirection = 'desc' THEN TerminalName 
                      WHEN @SortId = 8 
                      AND      @SortDirection = 'desc' THEN DropTime
					  WHEN @SortId = 12 
                      AND      @SortDirection = 'desc' THEN DriverName 
                      WHEN @SortId = 13 
                      AND      @SortDirection = 'desc' THEN [Status] 
             END DESC OFFSET (@PageNumber -1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY 
  END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rohan Koshti
-- Create date: 21-03-2018
-- Description:	GET INVOICE AND DROP TICKET DETAILS FOR SUPPLIER DASHBOARD
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetSupplierInvoicesForDashboard]
	@CompanyId				INT,
	@AllowedInvoiceType	INT = 0
AS
BEGIN
	DECLARE @TotalInvoices INT
	DECLARE @InvoicesFromDDTCount INT
	DECLARE @UnconfirmedInvoiceCount INT
	DECLARE @ApprovedInvoiceCount INT
	DECLARE @ReceivedInvoiceCount INT
	DECLARE @NotApprovedInvoiceCount INT
	DECLARE @CreatedInvoiceCount INT
	DECLARE @BrokeredRequest INT
	DECLARE @IsActiveStatus INT
	DECLARE @DDTManualInvoiceType INT
	DECLARE @DDTMobileInvoiceType INT
	DECLARE @UnconfirmedInvoiceStatus INT
	DECLARE @ApprovedInvoiceStatus INT
	DECLARE @RejectedInvoiceStatus INT
	SET @BrokeredRequest = 3
	SET @IsActiveStatus = 1
	SET @DDTManualInvoiceType = 6
	SET @DDTMobileInvoiceType = 7
	SET @UnconfirmedInvoiceStatus = 6
	SET @ApprovedInvoiceStatus = 3
	SET @RejectedInvoiceStatus = 4
	
	SET @TotalInvoices = 0
	SET @InvoicesFromDDTCount = 0
	SET @UnconfirmedInvoiceCount = 0
	SET @ApprovedInvoiceCount = 0
	SET @ReceivedInvoiceCount = 0
	SET @NotApprovedInvoiceCount = 0
	SET @CreatedInvoiceCount = 0
	
	SELECT Inv.Id AS InvoiceId, 
	Inv.OrderId AS InvoiceOrderId,
	Inv.ParentId AS ParentId,
	Inv.InvoiceTypeId AS InvoiceTypeId,
	InvStatus.StatusId AS InvoiceCurrentStatus,
	Ord.BuyerCompanyId AS OrderBuyerCompanyId,
	Ord.AcceptedCompanyId AS OrderAcceptedCompanyId,
	com.Id AS InvoiceCreatedByCompanyId,
	FR.FuelRequestTypeId AS FuelRequestTypeId 
	INTO #TempInvoices
	FROM Invoices Inv
	LEFT JOIN Orders Ord ON Inv.OrderId = Ord.Id
	LEFT JOIN FuelRequests FR ON Ord.FuelRequestId = FR.Id
	INNER JOIN Users USR ON Inv.CreatedBy = USR.Id
	--INNER JOIN UserXCompanies UXC ON USR.Id = UXC.UserId
	INNER JOIN Companies COM ON USR.CompanyId = COM.Id
	INNER JOIN InvoiceXInvoiceStatusDetails InvStatus ON (Inv.Id = InvStatus.InvoiceId AND InvStatus.IsActive = @IsActiveStatus)
	WHERE
	Inv.InvoiceVersionStatusId = @IsActiveStatus AND
	(
		(Inv.OrderId IS NULL and com.Id = @CompanyId) OR
		(Inv.OrderId IS NOT NULL and Ord.AcceptedCompanyId = @CompanyId) OR
		(FR.FuelRequestTypeId = @BrokeredRequest and Ord.BuyerCompanyId = @CompanyId)
	)
	SELECT @InvoicesFromDDTCount = COUNT(InvoiceId) 
	FROM #TempInvoices 
	WHERE 
	ParentId IS NOT NULL
	IF(@AllowedInvoiceType = 0)
	BEGIN
		DELETE FROM #TempInvoices WHERE InvoiceTypeId = @DDTManualInvoiceType or InvoiceTypeId = @DDTMobileInvoiceType
	END
	ELSE
	BEGIN
		DELETE FROM #TempInvoices WHERE InvoiceTypeId != @DDTManualInvoiceType and InvoiceTypeId != @DDTMobileInvoiceType
	END
	SELECT @TotalInvoices = COUNT(InvoiceId) FROM #TempInvoices
	SELECT @UnconfirmedInvoiceCount = COUNT(*) 
	FROM #TempInvoices 
	WHERE 
	InvoiceCurrentStatus = @UnconfirmedInvoiceStatus
	SELECT @ApprovedInvoiceCount = COUNT(InvoiceId) 
	FROM #TempInvoices 
	WHERE 
	InvoiceCurrentStatus = @ApprovedInvoiceStatus
	SELECT @ReceivedInvoiceCount = COUNT(InvoiceId) 
	FROM #TempInvoices 
	WHERE 
	InvoiceOrderId IS NOT NULL AND 
	FuelRequestTypeId = @BrokeredRequest and OrderBuyerCompanyId = @CompanyId
	
	SELECT @NotApprovedInvoiceCount = COUNT(InvoiceId) 
	FROM #TempInvoices 
	WHERE 
	InvoiceCurrentStatus = @RejectedInvoiceStatus and [dbo].[usf_CheckInvoiceHasAnyReceivedStatus](InvoiceId) > 0
	SELECT @CreatedInvoiceCount = COUNT(InvoiceId) FROM #TempInvoices 
	WHERE 
	(
		(InvoiceOrderId IS NULL and InvoiceCreatedByCompanyId = @CompanyId) OR
		(InvoiceOrderId IS NOT NULL and OrderAcceptedCompanyId = @CompanyId)
	)
	
	SELECT @TotalInvoices AS Total, @UnconfirmedInvoiceCount AS Unconfirmed, @ApprovedInvoiceCount AS Approved,
	@ReceivedInvoiceCount AS Received, @NotApprovedInvoiceCount AS NotApproved, @CreatedInvoiceCount AS Created, 
	@InvoicesFromDDTCount AS InvoicesFromDDT
	DROP TABLE IF EXISTS #TempInvoices
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetSupplierOrderCalenderEvents] @FirstDayOfMonth   DATETIME, 
                                                           @LastDayOfMonth    DATETIME, 
                                                           @BuyerCompanyId    INT, 
                                                           @SupplierCompanyId INT, 
                                                           @SelectedOrders    NVARCHAR(MAX) 
AS 
  BEGIN 
      SELECT ORD.Id, 
             FRQ.Id                                     AS FuelRequestId, 
             FRQ.CreatedBy                              AS Buyer, 
             ISNULL(FRQ.ExternalPoNumber, ORD.PoNumber) AS PoNumber, 
             FRQ.MaxQuantity                            AS Quantity, 
             OST.[Name]                                 AS OrderStatus, 
             OXS.StatusId, 
             MST.[Code]                                 AS JobStateCode, 
             JBS.ZipCode                                AS JobZipCode, 
             JBS.TimeZoneName                           AS TimeZone, 
			 JBS.CompanyId, 
             FDD.StartDate, 
             FDD.EndTime, 
             FDD.StartTime, 
             FDD.DeliveryTypeId 
      INTO   #TempSupplierOrders 
      FROM   Orders ORD 
             INNER JOIN OrderXStatuses OXS 
                     ON ORD.Id = OXS.OrderId 
                        AND OXS.IsActive = 1 
             INNER JOIN MstOrderStatuses OST 
                     ON OXS.StatusId = OST.Id 
             INNER JOIN FuelRequests FRQ 
                     ON ORD.FuelRequestId = FRQ.Id 
             INNER JOIN FuelRequestXDeliveryDetails FDD 
                     ON FRQ.Id = FDD.FuelRequestId 
             --INNER JOIN JobXFuelRequests JXF 
             --        ON FRQ.Id = JXF.FuelRequestId 
             INNER JOIN Jobs JBS 
                     ON FRQ.JobId = JBS.Id 
             INNER JOIN MstStates MST 
                     ON JBS.StateId = MST.Id 
      WHERE  ORD.IsActive = 1 
             AND ORD.AcceptedCompanyId = @SupplierCompanyId 
             AND ORD.IsEndSupplier = 1 
             AND ( @BuyerCompanyId = 0 
                    OR ORD.BuyerCompanyId = @BuyerCompanyId ) 
             AND ( @SelectedOrders = '' 
                    OR ORD.Id IN (SELECT VALUE 
                                  FROM   STRING_SPLIT(@SelectedOrders, ',')) ); 

      WITH SingleDeliverySchedules 
           AS (SELECT TMP.Id, 
                      INV.Id                                                 AS InvoiceId, 
                      TMP.StartDate, 
                      TMP.StartTime, 
                      TMP.EndTime, 
                      TMP.DeliveryTypeId, 
                      TMP.StatusId, 
                      TMP.OrderStatus, 
                      OXD.DriverId, 
                      DR_USR.FirstName                                       AS DriverFirstName,
                      DR_USR.LastName                                        AS DriverLastName, 
                      TMP.PoNumber, 
                      TMP.Quantity, 
                      ISNULL((SELECT Sum(DroppedGallons) 
                              FROM   Invoices INV 
                                     INNER JOIN InvoiceXInvoiceStatusDetails ISD 
                                             ON INV.Id = ISD.InvoiceId 
                                                AND ISD.IsActive = 1 
                                                AND ISD.StatusId NOT IN ( 9, 11 ) 
                              WHERE  INV.OrderId = TMP.Id 
                                     AND INV.InvoiceVersionStatusId = 1), 0) AS Delivered, 
                      CASE 
                        WHEN COM.CompanyTypeId = 2 
                             AND BR_FRQ.Id IS NOT NULL THEN BR_COM.[Name] 
                        ELSE COM.[Name] 
                      END                                                    AS CompanyName, 
                      TMP.JobStateCode, 
                      TMP.JobZipCode, 
                      TMP.TimeZone, 
                      1                                                      AS CalendarEventType
               FROM   #TempSupplierOrders TMP 
                      --INNER JOIN UserXCompanies UXC 
                      --        ON TMP.Buyer = UXC.UserId 
					  INNER JOIN Users UXC 
                              ON TMP.Buyer = UXC.Id 
                      INNER JOIN Companies COM 
                              ON UXC.CompanyId = COM.Id 
                      LEFT JOIN FuelRequests BR_FRQ 
                             ON TMP.FuelRequestId = BR_FRQ.ParentId 
                      --LEFT JOIN UserXCompanies BR_UXC 
                      --       ON BR_FRQ.CreatedBy = BR_UXC.UserId
					  LEFT JOIN Users BR_UXC 
                             ON BR_FRQ.CreatedBy = BR_UXC.Id  
                      LEFT JOIN Companies BR_COM 
                             ON BR_UXC.CompanyId = BR_COM.Id 
                      LEFT JOIN OrderXDrivers OXD 
                             ON TMP.Id = OXD.OrderId 
                                AND OXD.IsActive = 1 
                      LEFT JOIN Users DR_USR 
                             ON OXD.DriverId = DR_USR.Id 
                      LEFT JOIN Invoices INV 
                             ON TMP.Id = INV.OrderId 
                                AND INV.InvoiceVersionStatusId = 1 
                                AND INV.IsActive = 1 
                                AND TMP.DeliveryTypeId = 1 
               WHERE Cast(TMP.StartDate AS DATE) >= @FirstDayOfMonth 
                      AND Cast(TMP.StartDate AS DATE) <= @LastDayOfMonth 
			), 
           PastSchedules 
           AS (SELECT TMP.Id, 
		              TS.InvoiceId,
					  TS.Date     AS StartDate, 
					  TS.StartTime, 
					  TS.EndTime,
					  TMP.DeliveryTypeId,
					  CASE 
                        WHEN ( TS.DeliveryScheduleStatusId = 6 
                               AND TS.IsActive = 1 ) THEN 3 
                        ELSE TS.DeliveryScheduleStatusId 
                      END         AS StatusId,
					  TMP.OrderStatus, 
					  TS.DriverId, 
                      D.FirstName AS DriverFirstName, 
                      D.LastName  AS DriverLastName,  
                      TMP.PoNumber, 
                      TS.Quantity,                      
                      0           AS Delivered, 
                      C.NAME      AS CompanyName, 
                      JobStateCode, 
                      JobZipCode, 
                      TMP.TimeZone, 
                      3                                                      AS CalendarEventType
               FROM   DeliveryScheduleXTrackableSchedules TS 
                      INNER JOIN #TempSupplierOrders TMP 
                              ON TS.OrderId = TMP.Id 
                      --INNER JOIN CompanyXJobs CXJ 
                      --        ON TMP.JobId = CXJ.JobId 
                      INNER JOIN Companies C 
                              ON TMP.CompanyId = C.Id 
                      LEFT JOIN Users D 
                             ON TS.DriverId = D.Id 
               WHERE  ( TMP.StatusId = 1 
                         OR TS.InvoiceId IS NOT NULL ) 
                      AND ( Cast(TS.Date AS DATE) >= @FirstDayOfMonth 
                            AND Cast(TS.Date AS DATE) <= @LastDayOfMonth ) 
                      AND ( TS.IsActive = 1 
                             OR TS.DeliveryScheduleStatusId IN ( 11, 12, 20, 21, 
                                                                 6, 5 ) ))
		
	  SELECT * 
      FROM   SingleDeliverySchedules 
      UNION ALL 
      SELECT * 
      FROM   PastSchedules
	  ORDER BY StartDate
  END 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kailash Saini
-- Create date: 30-Dec-2017
-- Modified By: Shubham Chawla
-- MOdified Date: 29-May-2018
-- Description:	Returns supplier orders of the specified company
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetSupplierOrders]
	@CompanyId	INT,
	@StartDate	DATETIMEOFFSET(7),
	@EndDate	DATETIMEOFFSET(7),
	@FilterType int = 0,
	@GlobalSearchText varchar(30) = NULL,
	@SortId int = 0,
	@SortDirection varchar(6) = 'desc',
	@PageSize int = 99999999,
	@PageNumber int = 1, 
	@PoNumberSearchTypes [dbo].SearchTypes  Readonly, 
	@CustomerSearchTypes [dbo].SearchTypes  Readonly, 
	@DBESearchTypes [dbo].SearchTypes  Readonly, 
	@GallonsOrderedSearchTypes [dbo].SearchTypes  Readonly, 
	@TotalAmountSearchTypes [dbo].SearchTypes  Readonly, 
	@DeliveryDateSearchTypes [dbo].SearchTypes  Readonly,
	@InvoiceCountSearchTypes [dbo].SearchTypes  Readonly,
	@OrderCompletedSearchTypes [dbo].SearchTypes  Readonly, 
	@StatusSearchTypes [dbo].SearchTypes  Readonly
AS
BEGIN
	 DECLARE @PoNumberSearchTypesValid INT 
		SET @PoNumberSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @PoNumberSearchTypes) 
		DECLARE @CustomerSearchTypesValid INT 
		SET @CustomerSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @CustomerSearchTypes) 
		DECLARE @DBESearchTypesValid INT 
		SET @DBESearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @DBESearchTypes) 
		DECLARE @GallonsOrderedSearchTypesValid INT 
		SET @GallonsOrderedSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @GallonsOrderedSearchTypes)
		DECLARE @TotalAmountSearchTypesValid INT 
		SET @TotalAmountSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @TotalAmountSearchTypes) 
		DECLARE @DeliveryDateSearchTypesValid INT 
		SET @DeliveryDateSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @DeliveryDateSearchTypes) 
		DECLARE @InvoiceCountSearchTypesValid INT 
		SET @InvoiceCountSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @InvoiceCountSearchTypes) 
		DECLARE @OrderCompletedSearchTypesValid INT 
		SET @OrderCompletedSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @OrderCompletedSearchTypes) 
		DECLARE @StatusSearchTypesValid INT 
		SET @StatusSearchTypesValid = 
		( 
			   SELECT Count(*) 
			   FROM   @StatusSearchTypes) 
	;
	 WITH OrderGridValues AS 
    ( 	
	SELECT	ORD.Id,
			ISNULL(FRQ.ExternalPoNumber, ORD.PoNumber) AS PoNumber,
			ISNULL(dbo.usf_GetSupplierQualifications(FRQ.Id), '--')  AS Eligibility,
			(CASE WHEN FRQ.FuelRequestTypeId != 3 THEN JCOM.[Name] ELSE FCOM.[Name] END) AS Supplier,
			FRQ.MaxQuantity AS Quantity,
			CONVERT(NVARCHAR(10), FDD.StartDate, 101) AS StartDate,
			(SELECT COUNT(*) FROM dbo.Invoices INV WHERE INV.InvoiceVersionStatusId = 1 AND INV.OrderId = ORD.Id) AS InvoiceCount,
			Cast(
                               (SELECT ISNULL((SUM(INV.DroppedGallons)/FRQ.MaxQuantity * 100), 0) FROM dbo.Invoices INV 
			WHERE INV.InvoiceVersionStatusId = 1 AND INV.IsActive = 1 AND INV.OrderId = ORD.Id) AS INT)  AS FuelDeliveredPercentage,
			dbo.usf_GetBrokeredFuelRequestId(ORD.Id, FRQ.Id) AS BrokerFuelRequestId,
			ISNULL(ORD.ParentId, 0) AS ParentId,
			CASE WHEN FRQ.PricingTypeId <> 2 THEN null ELSE FORMAT((FRQ.MaxQuantity * FRQ.PricePerGallon),'0.00') END AS TotalAmount,
			CASE WHEN OXS.StatusId = 4 THEN 'Canceled' WHEN OXS.StatusId = 5 THEN 'Closed' ELSE MOS.NAME END AS [Status],
			CASE WHEN OXS.StatusId = 4 THEN 3 WHEN OXS.StatusId = 5 THEN 2 ELSE OXS.StatusId END AS [StatusId]
	FROM	dbo.Orders ORD
	INNER JOIN dbo.OrderVersions ORV ON ORD.Id = ORV.OrderId AND ORV.IsActive = 1
	INNER JOIN dbo.OrderXStatuses OXS ON ORD.Id = OXS.OrderId AND OXS.IsActive = 1
	INNER JOIN dbo.MstOrderStatuses MOS ON OXS.StatusId = MOS.Id
	INNER JOIN dbo.FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id
	INNER JOIN dbo.FuelRequestXDeliveryDetails FDD ON FRQ.Id = FDD.FuelRequestId
	--INNER JOIN dbo.JobXFuelRequests JXF ON FRQ.Id = JXF.FuelRequestId
	INNER JOIN dbo.Jobs JXF ON FRQ.JobId = JXF.Id
	--INNER JOIN dbo.CompanyXJobs CXJ ON JXF.JobId = CXJ.JobId
	INNER JOIN dbo.Companies JCOM ON JXF.CompanyId = JCOM.Id
	--INNER JOIN dbo.UserXCompanies UXC ON FRQ.CreatedBy = UXC.UserId
	INNER JOIN dbo.Users UXC ON FRQ.CreatedBy = UXC.Id
	INNER JOIN dbo.Companies FCOM ON UXC.CompanyId = FCOM.Id
	WHERE	ORD.AcceptedCompanyId = @CompanyId
			AND
			ORD.AcceptedDate >= @StartDate
			AND
			ORD.AcceptedDate < @EndDate	
	)

	SELECT *INTO #TempOrders FROM OrderGridValues OXS WHERE OXS.ParentId = 0 OR OXS.ParentId NOT IN (SELECT Id from OrderGridValues)
	
	CREATE TABLE #SupplierOrders(Id INT, PoNumber NVARCHAR(256),Eligibility NVARCHAR(256), Supplier NVARCHAR(256), Quantity DECIMAL(18,2), StartDate NVARCHAR(10), InvoiceCount INT, FuelDeliveredPercentage INT,
	BrokerFuelRequestId INT, ParentId INT,TotalAmount DECIMAL(18,2),[Status] NVARCHAR(256), [StatusId] INT)

	IF @FilterType IN (1,2,3)
	BEGIN
	 INSERT INTO #SupplierOrders SELECT * FROM #TempOrders WHERE StatusId = @FilterType
	END
	ELSE IF @FilterType = 4
	BEGIN
	INSERT INTO #SupplierOrders SELECT * FROM #TempOrders WHERE InvoiceCount > 0
	END
	ELSE IF @FilterType = 5
	BEGIN
	INSERT INTO #SupplierOrders SELECT * FROM #TempOrders WHERE FuelDeliveredPercentage > 50
	END
	ELSE IF @FilterType = 0
	BEGIN
	INSERT INTO #SupplierOrders SELECT * FROM #TempOrders
	END

	SELECT *,  [TotalCount]= COUNT(Id) OVER() 
	FROM #SupplierOrders
	WHERE    ( 
							@PoNumberSearchTypesValid = 0 
					OR      ( 
									@PoNumberSearchTypesValid > 0 
							AND      PoNumber IN 
									( 
											SELECT PoNumber 
											FROM   @PoNumberSearchTypes
											WHERE  PoNumber LIKE '%' + SearchVar + '%'))) 
		AND      ( 
							@CustomerSearchTypesValid = 0 
					OR      ( 
									@CustomerSearchTypesValid > 0 
							AND      Supplier IN 
									( 
											SELECT Supplier 
											FROM   @CustomerSearchTypes 
											WHERE  Supplier LIKE '%' + SearchVar + '%')))
		AND      ( 
							@DBESearchTypesValid = 0 
					OR      ( 
									@DBESearchTypesValid > 0 
							AND      Eligibility IN 
									( 
											SELECT Eligibility 
											FROM   @DBESearchTypes 
											WHERE  Eligibility LIKE '%' + SearchVar + '%'))) 
		AND      ( 
							@GallonsOrderedSearchTypesValid = 0 
					OR      ( 
									@GallonsOrderedSearchTypesValid > 0 
							AND      Quantity IN 
									( 
											SELECT Quantity 
											FROM   @GallonsOrderedSearchTypes 
											WHERE  Cast(Quantity AS VARCHAR(100)) LIKE '%' + SearchVar + '%')))
		AND      ( 
							@TotalAmountSearchTypesValid = 0 
					OR      ( 
									@TotalAmountSearchTypesValid > 0 
							AND      TotalAmount  IN 
									( 
											SELECT TotalAmount 
											FROM   @TotalAmountSearchTypes 
											WHERE  TotalAmount = (CASE WHEN SearchVar = '--' THEN NULL ELSE REPLACE(SearchVar,'$','') END) )))
		AND      ( 
							@DeliveryDateSearchTypesValid = 0 
					OR      ( 
									@DeliveryDateSearchTypesValid > 0 
							AND      StartDate IN 
									( 
											SELECT StartDate 
											FROM   @DeliveryDateSearchTypes 
											WHERE  StartDate  LIKE '%' + SearchVar + '%'))) 
		AND      ( 
							@InvoiceCountSearchTypesValid = 0 
					OR      ( 
									@InvoiceCountSearchTypesValid > 0 
							AND      InvoiceCount IN 
									( 
											SELECT InvoiceCount 
											FROM   @InvoiceCountSearchTypes 
											WHERE  InvoiceCount LIKE '%' + SearchVar + '%'))) 
		AND      ( 
							@OrderCompletedSearchTypesValid = 0 
					OR      ( 
									@OrderCompletedSearchTypesValid > 0 
							AND      FuelDeliveredPercentage IN 
									( 
											SELECT FuelDeliveredPercentage 
											FROM   @OrderCompletedSearchTypes 
											WHERE  FuelDeliveredPercentage LIKE '%' + SearchVar + '%')))
		AND      ( 
							@StatusSearchTypesValid = 0 
					OR      ( 
									@StatusSearchTypesValid > 0 
							AND      [Status] IN 
									( 
											SELECT [Status] 
											FROM   @StatusSearchTypes 
											WHERE  [Status] LIKE '%' + SearchVar + '%'))) 
		AND      ( 
							@GlobalSearchText IS NULL 
					OR      (		
									 ( 
											PoNumber LIKE '%' + @GlobalSearchText+ '%') 
							OR		 ( 
											Supplier LIKE '%' + @GlobalSearchText+ '%')
							OR       ( 
											Eligibility LIKE '%' + @GlobalSearchText+ '%') 
							OR       ( 
											Quantity LIKE '%' + @GlobalSearchText+ '%') 
							OR       ( 
											TotalAmount LIKE '%' + @GlobalSearchText+ '%') 
							OR       ( 
											StartDate LIKE '%' + @GlobalSearchText+ '%') 
							OR       ( 
											InvoiceCount LIKE '%' + @GlobalSearchText+ '%')
							OR       ( 
											FuelDeliveredPercentage LIKE '%' + @GlobalSearchText+ '%')
							OR       ( 
											Status LIKE '%' + @GlobalSearchText+ '%')) ) 
		ORDER BY 
					CASE 
							
							WHEN @SortId = 6 
							AND      @SortDirection = 'asc' THEN InvoiceCount 
							WHEN @SortId = 7 
							AND      @SortDirection = 'asc' THEN FuelDeliveredPercentage 
							WHEN @SortId = 3 
							AND      @SortDirection = 'asc' THEN Quantity 
							WHEN @SortId = 4 
							AND      @SortDirection = 'asc' THEN TotalAmount
					END ASC,
					CASE
							WHEN @SortId = 0 
							AND      @SortDirection = 'asc' THEN PoNumber 
							WHEN @SortId = 1 
							AND      @SortDirection = 'asc' THEN Supplier 
							WHEN @SortId = 2 
							AND      @SortDirection = 'asc' THEN Eligibility 
							WHEN @SortId = 8 
							AND      @SortDirection = 'asc' THEN Status 
					END ASC,
					CASE
							WHEN @SortId = 5 
							AND      @SortDirection = 'asc' THEN cast(StartDate As Datetime)
					END ASC,
					
					CASE
							WHEN @SortId = 6 
							AND      @SortDirection = 'desc' THEN InvoiceCount 
							WHEN @SortId = 7 
							AND      @SortDirection = 'desc' THEN FuelDeliveredPercentage 
							WHEN @SortId = 3
							AND      @SortDirection = 'desc' THEN Quantity 
							WHEN @SortId = 4
							AND      @SortDirection = 'desc' THEN TotalAmount
							WHEN @SortId = 10
							AND      @SortDirection = 'desc' THEN Id
					END DESC,
					CASE	
							WHEN @SortId = 0 
							AND      @SortDirection = 'desc' THEN PoNumber 
							WHEN @SortId = 1 
							AND      @SortDirection = 'desc' THEN Supplier 
							WHEN @SortId = 2 
							AND      @SortDirection = 'desc' THEN Eligibility 
							WHEN @SortId = 8 
							AND      @SortDirection = 'desc' THEN Status 
					END DESC,
					CASE
							WHEN @SortId = 5 
							AND      @SortDirection = 'desc' THEN cast(StartDate As Datetime) 
					 
					END DESC OFFSET (@PageNumber -1) * @PageSize ROWS 
		FETCH NEXT @PageSize ROWS ONLY 

		END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rohan Koshti
-- Create date: 20-03-2018
-- Description:	Get last active orders as per NewsFeed + total open closed orders count
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetSupplierOrdersForDashboard]
	@CompanyId				INT,
	@CountOfActiveOrders	INT = 5
AS
BEGIN
	DECLARE @TotalOrders int
	DECLARE @OpenOrders int
	DECLARE @ClosedOrders int
	DECLARE @TotalDrops int
	DECLARE @MoreThanFifty int
	DECLARE @IsActiveStatus INT
	DECLARE @OrderClosedStatus INT
	DECLARE @OrderPartiallyClosedStatus INT
	DECLARE @EntityTypeOrder INT
	SET @OpenOrders = 0
	SET @TotalOrders = 0
	SET @ClosedOrders = 0
	SET @TotalDrops = 0
	SET @MoreThanFifty = 0
	SET @IsActiveStatus = 1
	SET @OrderClosedStatus = 2
	SET @OrderPartiallyClosedStatus = 2
	SET @EntityTypeOrder = 2
	SELECT id as orderId,AcceptedCompanyId,FuelRequestId into #TempOrders 
	FROM Orders 
	WHERE AcceptedCompanyId = @CompanyId AND ParentId IS NULL
	
	SELECT @TotalOrders = COUNT(*) FROM #TempOrders
	SELECT @OpenOrders = COUNT(*) FROM #TempOrders ord INNER JOIN 
	[dbo].[OrderXStatuses] ordStatus on ord.orderId = ordStatus.OrderId
	WHERE ordStatus.IsActive = @IsActiveStatus and ordStatus.StatusId = @IsActiveStatus
	
	SELECT @ClosedOrders = COUNT(*) FROM #TempOrders ord INNER JOIN 
	[dbo].[OrderXStatuses] ordStatus on ord.orderId = ordStatus.OrderId
	WHERE ordStatus.IsActive = @IsActiveStatus and (ordStatus.StatusId = @OrderClosedStatus or ordStatus.StatusId = @OrderPartiallyClosedStatus)
	
	SELECT @TotalDrops = COUNT(*) FROM #TempOrders ord INNER JOIN 
	[dbo].[Invoices] inv on inv.OrderId = ord.orderId
	WHERE inv.IsActive = @IsActiveStatus and inv.InvoiceVersionStatusId = @IsActiveStatus
	
	SELECT @MoreThanFifty = COUNT(*)	
	FROM #TempOrders ord 
	INNER JOIN FuelRequests FR on ord.FuelRequestId = FR.Id
	WHERE ((FR.SpotDroppedGallons + FR.HedgeDroppedGallons)/(CASE WHEN FR.MaxQuantity = 0 THEN 1 ELSE FR.MaxQuantity END) * 100) > 50
	
	;WITH Last5ActiveOrdersFROMNeewsFeed AS
	(
		SELECT TOP (@CountOfActiveOrders) EntityId
		FROM Newsfeeds NF
		INNER JOIN #TempOrders OS on os.orderId=NF.EntityId
		WHERE EntityTypeId = @EntityTypeOrder
		GROUP BY EntityId 
		ORDER BY MAX(ID) DESC
	)
	
	SELECT 0 AS Id,
	'0' as PoNumber,
	'N/A' as Customer,
	CONVERT(NVARCHAR(10), GETDATE(), 101) AS StartDate,
	0 as InvoiceCOUNT,
	0 as FuelDeliveredPercentage,
	@TotalOrders as TotalOrders,
	@OpenOrders as OpenOrders,
	@ClosedOrders as ClosedOrders,
	@TotalDrops as TotalDrops,
	@MoreThanFifty as FiftyPlusPercentDelivered
	UNION ALL
	SELECT	ORD.Id,
			ISNULL(FRQ.ExternalPoNumber, ORD.PoNumber) AS PoNumber,
			CUST.Name AS Customer,
			CONVERT(NVARCHAR(10), FDD.StartDate, 101) AS StartDate,
			(SELECT COUNT(*) FROM dbo.Invoices INV WHERE INV.InvoiceVersionStatusId = 1 AND INV.OrderId = ORD.Id) AS InvoiceCOUNT,
			CAST((SELECT ISNULL((SUM(INV.DroppedGallons)/FRQ.MaxQuantity * 100), 0) FROM dbo.Invoices INV 
			WHERE INV.InvoiceVersionStatusId = @IsActiveStatus AND INV.IsActive = @IsActiveStatus AND INV.OrderId = ORD.Id) AS DECIMAL(18,2)) AS FuelDeliveredPercentage,
			@TotalOrders as TotalOrders,
			@OpenOrders as OpenOrders,
			@ClosedOrders as ClosedOrders,
			@TotalDrops as TotalDrops,
			@MoreThanFifty as FiftyPlusPercentDelivered
	FROM	dbo.Orders ORD
	INNER JOIN Last5ActiveOrdersFROMNeewsFeed LAO ON ORD.Id = LAO.EntityId
	INNER JOIN dbo.FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id
	INNER JOIN dbo.FuelRequestXDeliveryDetails FDD ON FRQ.Id = FDD.FuelRequestId
	INNER JOIN dbo.Companies CUST ON ORD.BuyerCompanyId = CUST.Id
	DROP TABLE IF EXISTS #TempOrders
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GetSupplierPerformanceData]
	@CompanyId	INT
AS
BEGIN
		SELECT Id, FuelRequestId, AcceptedCompanyId, BuyerCompanyId				
		INTO #TempOrderDetails 
		FROM Orders 
		WHERE BuyerCompanyId=@CompanyId
		
		--BASIC COUNTS OF ORDERS
		SELECT COMP.Name AS SupplierName,
				MIN(COMP.ID) AS SupplierCompanyId,
				COUNT(distinct ORD.Id) TotalOrders,
				CAST(ISNULL(SUM(
					INV.BasicAmount + dbo.usf_GetInvoiceTotalFees(INV.Id) +
					CASE WHEN INV.InvoiceTypeId NOT IN (6, 7) THEN INV.TotalTaxAmount ELSE 0 END
					),0) AS numeric(18,2)) AS TotalOrderValue,
				CAST(ISNULL(SUM(INV.DroppedGallons),0) AS numeric(18,2)) AS GallonsDelivered,
				ISNULL(COUNT(distinct INV.Id),0) AS TotalDeliveries
		INTO #TempOrderValues
		FROM #TempOrderDetails ORD
		LEFT JOIN Invoices INV ON (ORD.Id=INV.OrderId AND INV.InvoiceVersionStatusId=1 AND INV.IsActive=1)
		LEFT JOIN Companies COMP ON ORD.AcceptedCompanyId=COMP.Id
		GROUP BY COMP.Name
		--MISSED DELIVERY COUNT OF MULTIPLE DELIVERY
		SELECT	COMP.Name AS SupplierName,
				MIN(COMP.Id) SupplierCompanyId,
				COUNT(ts.Id) AS MissedDelivries
		INTO #TempMissedDeliveriesOfMultiple
		FROM #TempOrderDetails TOD 
		LEFT JOIN Companies COMP ON COMP.Id=TOD.AcceptedCompanyId
		LEFT JOIN DeliveryScheduleXTrackableSchedules ts ON TOD.Id=ts.OrderId
		WHERE ts.DeliveryScheduleStatusId in (11,12,20,21)
		GROUP BY COMP.Name
		--MISSED DELIVERY COUNT OF SINGLE DELIVERY
		SELECT	COMP.Name AS SupplierName,
				MIN(COMP.Id) SupplierCompanyId,
				COUNT(TOD.Id) AS MissedDelivries
		INTO #TempMissedDeliveriesofSingle
		FROM #TempOrderDetails TOD 
		LEFT JOIN OrderXStatuses ORDSTATUS ON (TOD.Id = ORDSTATUS.OrderId AND ORDSTATUS.IsActive = 1 AND ORDSTATUS.StatusId=1)
		LEFT JOIN Companies COMP ON COMP.Id=TOD.AcceptedCompanyId
		LEFT JOIN FuelRequests FR ON TOD.FuelRequestId = FR.Id
		LEFT JOIN FuelRequestXDeliveryDetails FRDD ON (FR.Id=FRDD.FuelRequestId AND FRDD.DeliveryTypeId=1)
		--LEFT JOIN JobXFuelRequests JFR ON FR.Id = JFR.FuelRequestId
		LEFT JOIN Jobs JOB ON FR.JobId=JOB.Id
		WHERE 
			CONVERT(DATE, FRDD.StartDate) < CONVERT(DATE,(GETDATE() AT TIME ZONE JOB.TimeZoneName)) 
			OR
			(
				CONVERT(DATE, FRDD.StartDate) = CONVERT(DATE,(GETDATE() AT TIME ZONE JOB.TimeZoneName)) 
				AND
				FRDD.EndTime < CONVERT(TIME(0), CURRENT_TIMESTAMP AT TIME ZONE JOB.TimeZoneName)
			)
		GROUP BY COMP.Name
		--MISSED DELIVERIES OF BOTH DELIVERY TYPES
		SELECT 
			COMBINED.SupplierName, 
			COMBINED.SupplierCompanyId, 
			SUM(COMBINED.MissedDelivries) AS MissedDeliveries
		INTO #TempMissedDeliveriesCombined
		FROM 
		(SELECT * FROM #TempMissedDeliveriesOfMultiple
		UNION
		SELECT * FROM #TempMissedDeliveriesofSingle) AS COMBINED
		GROUP BY COMBINED.SupplierName, COMBINED.SupplierCompanyId
		
		--LATE DELIVERY COUNT
		SELECT COMP.Name AS SupplierName,
				MIN(COMP.Id) AS SupplierCompanyId,
				COUNT(DTS.Id) AS LateDelivries
		INTO #TempLateDeliveries
		FROM #TempOrderDetails TOD 
		LEFT JOIN Companies COMP ON TOD.AcceptedCompanyId=COMP.Id
		LEFT JOIN Invoices INV ON INV.OrderId=TOD.Id
		LEFT JOIN DeliveryScheduleXTrackableSchedules DTS ON DTS.InvoiceId=INV.ID
		WHERE DTS.DeliveryScheduleStatusId in (8,10)
		GROUP BY COMP.Name
		--Delivery overages
		SELECT	COMP.Name AS SupplierName,
				MIN(COMP.Id) AS SupplierCompanyId,
				COUNT(INV.Id) AS DeliveryOverages
		INTO #TempDeliveriesOverages
		FROM #TempOrderDetails TOD 
		LEFT JOIN Companies COMP ON TOD.AcceptedCompanyId=COMP.Id
		LEFT JOIN FuelRequests FR ON FR.Id=TOD.FuelRequestId
		LEFT JOIN Invoices INV ON INV.OrderId=TOD.Id
		WHERE INV.DroppedGallons > FR.MaxQuantity
		GROUP BY COMP.Name
		HAVING SUM(INV.DroppedGallons) > SUM(FR.MaxQuantity)
		SELECT 
			TOV.SupplierCompanyId,
			TOV.SupplierName,
			TOV.TotalOrders,
			CAST(TOV.TotalOrderValue AS numeric(18,2)) AS TotalOrderValue,
			CAST(TOV.GallonsDelivered AS numeric(18,2)) AS GallonsDelivered,
			TOV.TotalDeliveries,
			ISNULL(TMD.MissedDeliveries,0) AS MissedDeliveries,
			ISNULL(TLD.LateDelivries,0) AS LateDelivries,
			ISNULL(TDO.DeliveryOverages,0) AS DeliveryOverages
		FROM #TempOrderValues TOV
		LEFT JOIN #TempMissedDeliveriesCombined TMD ON TOV.SupplierCompanyId=TMD.SupplierCompanyId 
		LEFT JOIN #TempLateDeliveries TLD ON TOV.SupplierCompanyId = TLD.SupplierCompanyId
		LEFT JOIN #TempDeliveriesOverages TDO ON TOV.SupplierCompanyId = TDO.SupplierCompanyId
		ORDER BY TOV.TotalOrders DESC
		DROP TABLE IF EXISTS #TempOrderDetails, #TempOrderValues, #TempMissedDeliveriesCombined, 
				#TempLateDeliveries, #TempDeliveriesOverages, #TempMissedDeliveriesofSingle,
				#TempMissedDeliveriesOfMultiple, #TempMissedDeliveriesofSingle
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Rohan Koshti
-- Create date: 26-Apr-2018
-- Description:	Returns All Licenses of a company
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetTaxExemptLicenses]
	@CompanyId	INT,
	@IsBuyer BIT
AS
BEGIN
SELECT	
	Licenses.Id,
	Licenses.IDCode, 
	Licenses.LicenseNumber, 
	SubTypes.Jurisdiction + ' - ' + SubTypes.Name as BusinessSubType, 
	Stat.Name as Status,
	Usr.FirstName + ' ' + Usr.LastName as AddedBy,
	IsAssignedToAnyJob = 
						CASE WHEN @IsBuyer = 1
						THEN 
							CASE 
								WHEN
								(SELECT COUNT(*) FROM JobXTaxExemptLicenses WHERE TaxExemptLicenseId = Licenses.Id) > 0
								THEN CONVERT(bit, 1)
								ELSE CONVERT(bit, 0)
								END
						ELSE
								CASE 
								WHEN
								(SELECT COUNT(*) FROM OrderXTaxExemptLicenses WHERE TaxExemptLicenseId = Licenses.Id) > 0
								THEN CONVERT(bit, 1)
								ELSE CONVERT(bit, 0)
								END
						END
	FROM dbo.TaxExemptLicenses Licenses
	INNER JOIN dbo.MstBusinessSubTypes SubTypes ON Licenses.BusinessSubType = SubTypes.Id
	INNER JOIN dbo.MstTaxExemptLicenseStatuses Stat ON Stat.Id = Licenses.Status
	INNER JOIN dbo.Users Usr ON Usr.Id = Licenses.UpdatedBy
	WHERE	Licenses.CompanyId = @CompanyId
	AND Licenses.IsActive = 1
	ORDER BY Licenses.Id DESC
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sandip Sawant
-- Create date: 24-Jan-2018
-- Description:	Returns all drivers time card action summary
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetTimeCardActionSummaryForAllDrivers]
	@CompanyId		INT,
	@DriverIds		VarChar(512),
	@StartDate		DATETIMEOFFSET(7),
	@EndDate		DATETIMEOFFSET(7),
	@PageNo			INT,
	@PageSize		INT,
	@SortId			INT
AS
BEGIN
	SELECT 
		TC.DriverId,
		--CONCAT(U.FirstName, ' ' ,U.LastName) AS [DriverName],
		CONCAT(UC.FirstName, ' ' ,UC.LastName) AS [DriverName],
		CONVERT(NVARCHAR(10), MIN(TC.ActionStartDate), 101) AS [ActionDate],
		FORMAT(MIN(TC.ActionStartDate), 'hh:mm tt') AS [ClockIn],
		MAX (CASE WHEN TC.ActionId=2 THEN FORMAT(TC.ActionEndDate, 'MM/dd/yyyy hh:mm tt') ELSE NULL END) AS [ClockOut],
		[dbo].[usf_GetTimeInHhMmFormat](SUM (CASE WHEN TC.ActionId=7 THEN DATEDIFF(MI, TC.ActionStartDate, TC.ActionEndDate) ELSE 0 END)) AS [FuelDropTime],
		[dbo].[usf_GetTimeInHhMmFormat](SUM (CASE WHEN TC.ActionId=9 THEN DATEDIFF(MI, TC.ActionStartDate, TC.ActionEndDate) ELSE 0 END)) AS [TransitTime],
		[dbo].[usf_GetTimeInHhMmFormat](SUM (CASE WHEN TC.ActionId=3 or TC.ActionId = 5 THEN DATEDIFF(MI, TC.ActionStartDate, TC.ActionEndDate) ELSE 0 END)) AS [BreakTime],
		[dbo].[usf_GetTimeInHhMmFormat](ISNULL(DATEDIFF(MI, MIN(TC.ActionStartDate), MAX(TC.ActionEndDate)), 0)) AS [TotalTime],
		[dbo].[usf_GetTimeInHhMmFormat](CASE WHEN MAX(TC.ActionEndDate) IS NOT NULL THEN (DATEDIFF(MI, MIN(TC.ActionStartDate), MAX(TC.ActionEndDate))) ELSE 0 END) AS [TotalShiftTime]
	FROM TimeCardEntries TC
			--INNER JOIN UserXCompanies UC ON TC.DriverId = UC.UserId AND TC.DriverId IN (SELECT Value 
		   --                                           FROM   STRING_SPLIT(@DriverIds, ',')) 
		   INNER JOIN Users UC ON TC.DriverId = UC.Id AND TC.DriverId IN (SELECT Value 
                                                      FROM   STRING_SPLIT(@DriverIds, ',')) 
			--INNER JOIN Users U ON U.Id = UC.UserId
	WHERE UC.CompanyId = @CompanyId AND TC.CreatedDate >= @StartDate AND TC.CreatedDate < @EndDate
	GROUP BY DriverId, UC.FirstName, UC.LastName, ActionGroup
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SaveDebugLog] (
  @machineName nvarchar(200),
  @siteName nvarchar(200),
  @logDateTime datetime,
  @level varchar(5),
  @userName nvarchar(200),
  @message nvarchar(max),
  @logger nvarchar(300),
  @properties nvarchar(max),
  @serverName nvarchar(200),
  @port nvarchar(100),
  @url nvarchar(2000),
  @https bit,
  @serverAddress nvarchar(100),
  @remoteAddress nvarchar(100),
  @callSite nvarchar(300)
) AS
BEGIN
  INSERT INTO [dbo].[DebugLog] (
    [MachineName],
    [SiteName],
    [LogDateTime],
    [Level],
    [UserName],
    [Message],
    [Logger],
    [Properties],
    [ServerName],
    [Port],
    [Url],
    [Https],
    [ServerAddress],
    [RemoteAddress],
    [CallSite]
  ) VALUES (
    @machineName,
    @siteName,
    @logDateTime,
    @level,
    @userName,
    @message,
    @logger,
    @properties,
    @serverName,
    @port,
    @url,
    @https,
    @serverAddress,
    @remoteAddress,
    @callSite
  );
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_SaveExceptionLog] (
  @machineName nvarchar(200),
  @siteName nvarchar(200),
  @logDateTime datetime,
  @level varchar(5),
  @userName nvarchar(200),
  @message nvarchar(max),
  @logger nvarchar(300),
  @properties nvarchar(max),
  @serverName nvarchar(200),
  @port nvarchar(100),
  @url nvarchar(2000),
  @https bit,
  @serverAddress nvarchar(100),
  @remoteAddress nvarchar(100),
  @callSite nvarchar(300),
  @exception nvarchar(max)
) AS
BEGIN
  INSERT INTO [dbo].[ExceptionLog] (
    [MachineName],
    [SiteName],
    [LogDateTime],
    [Level],
    [UserName],
    [Message],
    [Logger],
    [Properties],
    [ServerName],
    [Port],
    [Url],
    [Https],
    [ServerAddress],
    [RemoteAddress],
    [CallSite],
    [Exception]
  ) VALUES (
    @machineName,
    @siteName,
    @logDateTime,
    @level,
    @userName,
    @message,
    @logger,
    @properties,
    @serverName,
    @port,
    @url,
    @https,
    @serverAddress,
    @remoteAddress,
    @callSite,
    @exception
  );
END
GO
