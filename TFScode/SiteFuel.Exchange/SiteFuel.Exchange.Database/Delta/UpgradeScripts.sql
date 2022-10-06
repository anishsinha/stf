
GO
/****** Object:  StoredProcedure [dbo].[usp_GetMarineInvoiceBolList]    Script Date: 05/18/2022 5:22:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- EXEC usp_GetMarineInvoiceBolList 1293, 332588, 0
CREATE OR ALTER       PROCEDURE [dbo].[usp_GetMarineInvoiceBolList] 
	-- Add the parameters for the stored procedure here
	@CompanyId int,
	@invoiceHeaderId int,
	@invoiceId int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@companyId > 0 AND @invoiceheaderId > 0)
	BEGIN
		SELECT 
		INV.InvoiceHeaderId,
		INV.Id AS InvoiceId,
		INVBOL.ID AS InvoiceFtlDetailId,
		ISNULL(DRIUSR.FirstName, '') + ISNULL(DRIUSR.LastName, '') AS Driver,
		INV.DisplayInvoiceNumber AS DisplayNumber,
		CASE WHEN INVBOL.BolNumber IS NULL THEN INVBOL.LiftTicketNumber ELSE INVBOL.BolNumber END AS BolOrLiftNumber,
		ISNULL(INVBOL.DeliveredQuantity,INV.DroppedGallons) AS DroppedQty,
		INV.ConvertedQuantity AS ConvertedQty, 
		INVBOL.GrossQuantity AS GrossQty,
		INVBOL.NetQuantity AS NetQty,
		CONVERT(NVARCHAR(10), INV.DropEndDate, 101) AS DropDate,
		INV.Gravity AS ApiGravity,
		BDR.DensityInVaccum AS Density,
		BDR.ObservedTemperature AS Temperature,
		BDR.SulphurContent AS SulfurContent,
		BDR.Viscosity,
		BDR.FlashPoint,
		FR.UoM,
		ISNULL(TS.DeliveryLevelPO,'') AS DeliveryLevelPO
		FROM Invoices INV
		INNER JOIN InvoiceXBolDetails INVXBOL ON INV.Id = INVXBOL.InvoiceId AND INVXBOL.InvoiceHeaderId = @invoiceheaderId
		INNER JOIN InvoiceFtlDetails INVBOL ON INVXBOL.BolDetailId = INVBOL.Id AND INVBOL.NetQuantity IS NOT NULL AND INVBOL.GrossQuantity IS NOT NULL
		INNER JOIN BDRDetails BDR ON INV.Id = BDR.InvoiceId
		INNER JOIN Orders ORD ON ORD.Id = INV.OrderId
		INNER JOIN FuelRequests FR ON ORD.FuelRequestId = FR.Id
		LEFT JOIN Users DRIUSR ON INV.DriverId = DRIUSR.Id
		LEFT JOIN DeliveryScheduleXTrackableSchedules TS ON INV.TrackableScheduleId = TS.Id  
		WHERE INVXBOL.InvoiceHeaderId = @invoiceheaderId 
				AND INV.IsActive = 1 
				AND INV.InvoiceVersionStatusId = 1 
				AND (INV.ID = @invoiceId OR @invoiceId = 0  )
				
	END
END
Go


GO
/****** Object:  StoredProcedure [dbo].[usp_ValidateBolDeleteRequest]    Script Date: 05/19/2022 4:23:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	 Yash Bagade
-- Create date: 8th March 2022
-- Description:	Validates Bol delete request for DDT/Bol
-- =============================================
 --exec usp_ValidateBolDeleteRequest 342953, 411554, 382928
CREATE OR ALTER           PROCEDURE [dbo].[usp_ValidateBolDeleteRequest]
	-- Add the parameters for the stored procedure here
	@InvoiceHeaderId INT,
	@InvoiceId INT ,
	@InvoiceFtlDetailsId INT
AS
BEGIN
	 SET NOCOUNT ON;
	 DECLARE @Response TABLE (Result BIT,Message nvarchar(1000))
	
	 --do now allow bol deletion if bol was considered for LFV process.
	  DECLARE  @IsLiftFileValidated BIT
	  SET @IsLiftFileValidated = (SELECT IsLiftFileValidated FROM InvoiceFtlDetails where Id = @InvoiceFtlDetailsId) 
	  IF @IsLiftFileValidated = 1
	  BEGIN
	     INSERT INTO @Response VALUES(0,'Cannot delete BOL as it is validated against a Lift file record.')
		 SELECT * FROM @Response
		 RETURN;
	  END

	  DELETE FROM @Response
	  --do not allow bol deletion if invoice is created. 
	  DECLARE  @WaitingFor INT
	  SET @WaitingFor = (SELECT WaitingFor FROM Invoices where Id = @InvoiceId) 
	  IF @WaitingFor = 0
	  BEGIN
	     INSERT INTO @Response VALUES(0,'Cannot delete a BOL after invoice/DDT confirmation')
		 SELECT * FROM @Response
		 RETURN;
	  END

	  DELETE FROM @Response
	  --Validate single bol delete
		    DECLARE @ActiveBolRecordsCount INT
			SET @ActiveBolRecordsCount = (SELECT COUNT(Id) FROM InvoiceFtlDetails WHERE (RecordHistory IS NULL OR RecordHistory = '') 
									AND IsActive =1 and IsDeleted =  0 AND
									Id IN (
										 SELECT BolDetailId FROM InvoiceXBolDetails WHERE InvoiceId = @InvoiceId
										  ) 
		    )
			IF @ActiveBolRecordsCount =1
			BEGIN
				INSERT INTO @Response VALUES(0,'Unable to delete as atleast one BOL is required')
				SELECT * FROM @Response
				RETURN;
			END
	  --INSERT INTO @Response VALUES(1,'Valid Bol delete request.')
	  --SELECT * FROM @Response
	  -- Update InvoiceFtlDetails if no validation error occurred.
	  BEGIN TRY
	    BEGIN TRAN 
	    DECLARE @RecordJson nvarchar(max)
		SET @RecordJson =  CONVERT(nvarchar(MAX), (SELECT BolNumber,LiftTicketNumber,NetQuantity,GrossQuantity,LiftDate FROM InvoiceFtlDetails WHERE Id = @InvoiceFtlDetailsId
		FOR JSON PATH, WITHOUT_ARRAY_WRAPPER,INCLUDE_NULL_VALUES) )

	    UPDATE InvoiceFtlDetails SET 
									BolNumber = NULL,
									LiftTicketNumber = NULL,
									NetQuantity = NULL,
									GrossQuantity = NULL,
									LiftDate = null,
									IsActive = 0,
									IsDeleted = 1,
									RecordHistory = @RecordJson
		WHERE Id = @InvoiceFtlDetailsId
		
		DELETE FROM @Response
		INSERT INTO @Response VALUES (1,'Bol deleted succesfully.')		
		COMMIT TRAN;
		SELECT * FROM @Response
    END TRY
	BEGIN CATCH
	       DELETE FROM @Response
		   INSERT INTO @Response VALUES (0,'Error occurred when processing your request.')
		   ROLLBACK TRAN;
		   SELECT * FROM @Response
	END CATCH
END
Go
Go

ALTER   PROCEDURE [dbo].[usp_GetDispatcherLoads]          
 @CompanyId INT,          
 @States NVARCHAR(MAX),          
 @FromDate DATETIMEOFFSET(7),          
 @ToDate DATETIMEOFFSET(7),          
 @DriverSearch NVARCHAR(512),          
 @Priority INT,          
 @RegionId NVARCHAR(512),          
 @DispatcherId NVARCHAR(MAX),        
 @CustomerId INT,     
 @Carriers NVARCHAR(MAX)  = '',   
 @ShowCarrierManaged BIT = 0, 
 @InventoryCaptureTypeIds NVARCHAR(512),
 @GlobalSearchText VARCHAR(30) = NULL,          
 @SortId INT = 0,          
 @SortDirection VARCHAR(6) = 'desc',          
 @PageSize INT = 99999999,          
 @PageNumber INT = 1,          
 @PoNumSearchTypes [dbo].SearchTypes READONLY,       
 @DR_IDSearchTypes [dbo].SearchTypes READONLY,   
 @NameSearchTypes [dbo].SearchTypes READONLY,          
 @DNameSearchTypes [dbo].SearchTypes READONLY,          
 @CNameSearchTypes [dbo].SearchTypes READONLY,          
 @PckupSearchTypes [dbo].SearchTypes READONLY,          
 @LocSearchTypes [dbo].SearchTypes READONLY,   
 @LTSearchTypes [dbo].SearchTypes READONLY, 
 @PrdtNmSearchTypes [dbo].SearchTypes READONLY,          
 @QtySearchTypes [dbo].SearchTypes READONLY,          
 @LdDateSearchTypes [dbo].SearchTypes READONLY,          
 @StatusSearchTypes [dbo].SearchTypes READONLY ,  
 @DropTicketSearchTypes [dbo].[SearchTypes] READONLY        
        
AS          
BEGIN          
 DECLARE @PoNumSearchTypesValid  INT SET @PoNumSearchTypesValid  = (SELECT COUNT(*) FROM @PoNumSearchTypes)  
 DECLARE @DR_IDSearchTypesValid  INT SET @DR_IDSearchTypesValid  = (SELECT COUNT(*) FROM @DR_IDSearchTypes) 
 DECLARE @NameSearchTypesValid   INT SET @NameSearchTypesValid   = (SELECT COUNT(*) FROM @NameSearchTypes)          
 DECLARE @DNameSearchTypesValid  INT SET @DNameSearchTypesValid  = (SELECT COUNT(*) FROM @DNameSearchTypes)           
 DECLARE @CNameSearchTypesValid  INT SET @CNameSearchTypesValid  = (SELECT COUNT(*) FROM @CNameSearchTypes)          
 DECLARE @PckupSearchTypesValid  INT SET @PckupSearchTypesValid  = (SELECT COUNT(*) FROM @PckupSearchTypes)           
 DECLARE @LocSearchTypesValid    INT SET @LocSearchTypesValid    = (SELECT COUNT(*) FROM @LocSearchTypes)     
 DECLARE @LTSearchTypesValid  INT SET @LTSearchTypesValid  = (SELECT COUNT(*) FROM @LTSearchTypes)
 DECLARE @PrdtNmSearchTypesValid INT SET @PrdtNmSearchTypesValid = (SELECT COUNT(*) FROM @PrdtNmSearchTypes)          
 DECLARE @QtySearchTypesValid    INT SET @QtySearchTypesValid    = (SELECT COUNT(*) FROM @QtySearchTypes)          
 DECLARE @LdDateSearchTypesValid INT SET @LdDateSearchTypesValid = (SELECT COUNT(*) FROM @LdDateSearchTypes)          
 DECLARE @StatusSearchTypesValid INT SET @StatusSearchTypesValid = (SELECT COUNT(*) FROM @StatusSearchTypes)          
 DECLARE @DropTicketSearchTypesValid INT SET @DropTicketSearchTypesValid = (SELECT COUNT(*) FROM @DropTicketSearchTypes)         
 DECLARE @StateList TABLE (Code NVARCHAR(2));          
 INSERT INTO @StateList          
 SELECT VALUE FROM STRING_SPLIT(@States, ',') WHERE LEN(VALUE) > 0;          
          
 DECLARE @IsStateSearch INT = (SELECT COUNT (*) FROM @StateList)          
 DECLARE @IsDriverSearch INT = (LEN(@DriverSearch))          
 SET @DriverSearch = '%'+@DriverSearch + '%';          
   
DECLARE @CarrierList TABLE (Id INT);                  
INSERT INTO @CarrierList                  
SELECT VALUE FROM STRING_SPLIT(@Carriers, ',') WHERE LEN(VALUE) > 0;    
DECLARE @IsCarrierSearch INT = (SELECT COUNT (*) FROM @CarrierList)   

 ;WITH DriverLocations_CTE AS          
 (          
  SELECT DISTINCT          
   USR.Id,          
   USR.FirstName,          
   USR.LastName,          
   USR.PhoneNumber,          
   MAX(LOC.Id) LocationId          
  FROM Users USR          
  JOIN AppLocations LOC ON USR.Id = LOC.UserId AND LOC.IsUserLogout = 0          
  WHERE USR.CompanyId = @CompanyId           
    AND (@IsDriverSearch = 0 OR (USR.FirstName + ' ' + USR.LastName LIKE @DriverSearch) OR USR.PhoneNumber LIKE @DriverSearch)          
    AND (LOC.OrderId IS NOT NULL OR LOC.TrackableScheduleId IS NOT NULL)          
    AND LOC.StatusId IN (1, 11, 12, 18, 20)          
    AND LOC.UpdatedDate >= @FromDate AND LOC.UpdatedDate < @ToDate          
  GROUP BY USR.Id, USR.FirstName, USR.LastName, USR.PhoneNumber          
 )          
          
 ,DriverLocationWithStatus_CTE AS          
 (          
  SELECT DISTINCT          
   DRV.Id,          
   DRV.FirstName,          
   DRV.LastName,          
   DRV.PhoneNumber,          
   LOC.Latitude,          
   LOC.Longitude,          
   ISNULL(DXT.OrderId, LOC.OrderId) OrderId,          
   LOC.TrackableScheduleId,          
   DXT.Quantity,          
   LOC.StatusId,          
   CASE WHEN LOC.StatusId = 1 THEN 'On the way to location'          
     WHEN LOC.StatusId = 11 THEN 'On the way to terminal'          
     WHEN LOC.StatusId = 12 THEN 'Arrived at terminal'          
     WHEN LOC.StatusId = 18 THEN 'Arrived at location'          
     WHEN LOC.StatusId = 20 THEN 'Fuel truck retained'          
     ELSE '' END AS [Status],          
   DXT.FrDeliveryRequestId AS DrId          
  FROM DriverLocations_CTE DRV          
  JOIN AppLocations LOC ON DRV.LocationId = LOC.Id          
  LEFT JOIN Orders ORD ON LOC.OrderId = ORD.Id          
  LEFT JOIN DeliveryScheduleXTrackableSchedules DXT ON LOC.TrackableScheduleId = DXT.Id           
  WHERE LOC.OrderId IS NOT NULL OR LOC.TrackableScheduleId IS NOT NULL          
     AND LOC.StatusId IN (1, 11, 12, 18, 20)          
 )          
           
 ,FinalDriverLocationWithStatus_CTE AS          
 (SELECT DISTINCT          
   COALESCE(DRVR.Id, DLC.Id, 0) Id,          
   ISNULL(DLC.FirstName, DRVR.FirstName) FirstName,          
   ISNULL(DLC.LastName, DRVR.LastName) LastName,          
   ISNULL(DLC.PhoneNumber, DRVR.PhoneNumber) PhoneNumber,          
   DSPTR.FirstName + ' ' + DSPTR.LastName AS Dispatcher,          
   CMP.[Name] AS Customer,          
   CASE WHEN DXT.DeliveryScheduleStatusId IN (7, 8, 9) THEN NULL          
   ELSE DLC.Latitude END Latitude,          
   CASE WHEN DXT.DeliveryScheduleStatusId IN (7, 8, 9) THEN NULL          
   ELSE DLC.Longitude END Longitude,          
   ISNULL(DXT.Quantity, DLC.Quantity) as Quantity,  
   ISNULL(DXT.QuantityTypeId,0) as QuantityTypeId,  
   FRQ.UoM,          
   ORD.PoNumber,          
   FORMAT(DXT.[Date], 'MM/dd/yyyy') AS [Date],          
   MPT.[Name] AS ProductName,          
   CASE WHEN FDL.TerminalId IS NULL THEN 'Location: ' + FDL.SiteName + ', '           
     + FDL.[Address] + ', ' + FDL.City + ', ' + FDL.StateCode + ' ' + FDL.ZipCode          
     ELSE 'Terminal: ' + MET.[Name] END Pickup,          
   JBS.[Address] + ', ' + JBS.City + ', ' + MST.Code + ' ' + JBS.ZipCode AS [Location],          
   JBS.StateId,          
   JBS.Latitude AS JobLatitude,          
   JBS.Longitude AS JobLongitude,          
   DLC.StatusId,  
   CASE WHEN DXT.[Date] >= GETDATE() AND DXT.DeliveryScheduleStatusId = 3 THEN 'Scheduled'          
   WHEN DXT.DeliveryScheduleStatusId IN (7, 8, 9) THEN DSS.[Name]          
   ELSE ISNULL(DLC.[Status], DSS.[Name]) END [Status],          
   DXT.FrDeliveryRequestId AS DrId,          
   CASE WHEN (DXT.AdditionalInfo Is Not null OR DXT.AdditionalInfo != '')           
     THEN JSON_VALUE(DXT.AdditionalInfo, '$.FsPriority')           
     ELSE 1 END AS LdPri,          
   CASE WHEN (DXT.AdditionalInfo Is Not null OR DXT.AdditionalInfo != '')           
     THEN JSON_VALUE(DXT.AdditionalInfo, '$.FsRegionId')           
     ELSE '' END AS RgId,  
	 CASE WHEN (DXT.AdditionalInfo Is Not null OR DXT.AdditionalInfo != '')           
     THEN JSON_VALUE(DXT.AdditionalInfo, '$."UniqueOrderNo"')           
     ELSE '' END AS UniqueOrderNo,
  ISNULL(STUFF((SELECT  ',' + IND.DisplayInvoiceNumber+'##'+CONVERT(VARCHAR(20),IND.InvoiceHeaderId)      
      FROM Invoices IND      
       WHERE  IND.OrderId=DXT.OrderId and IND.InvoiceTypeId IN(6,7) AND IND.TrackableScheduleId=DXT.Id      
      FOR XML PATH('')), 1, 1, ''),'') AS DROPTicketNum,      
	  JBS.InventoryDataCaptureType,
   [TotalCount]= COUNT(1) OVER()          
 FROM DeliveryScheduleXTrackableSchedules DXT     
 LEFT JOIN ScheduleXBrokerOrderDetails SXTS ON DXT.Id = SXTS.TrackableScheduleId  
 JOIN MstDeliveryScheduleStatuses DSS ON DXT.DeliveryScheduleStatusId = DSS.Id          
 JOIN Orders ORD ON (DXT.OrderId = ORD.Id OR SXTS.OrderId = ORD.Id)AND DXT.IsActive = 1    
 JOIN Companies CMP ON ORD.BuyerCompanyId = CMP.Id          
 JOIN FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id          
 JOIN Jobs JBS ON FRQ.JobId = JBS.Id          
 JOIN MstProducts MPD ON FRQ.FuelTypeId = MPD.Id          
 JOIN MstProductTypes MPT ON MPD.ProductTypeId = MPT.Id          
 JOIN MstStates MST ON JBS.StateId = MST.Id          
 LEFT JOIN DriverLocationWithStatus_CTE DLC ON ORD.Id = DLC.OrderId AND (DLC.TrackableScheduleId Is NULL OR DLC.TrackableScheduleId = DXT.Id)          
 LEFT JOIN Users DRVR ON DXT.DriverId = DRVR.Id          
 LEFT JOIN FuelDispatchLocations FDL ON DXT.Id = FDL.TrackableScheduleId AND FDL.LocationType = 1          
 LEFT JOIN MstExternalTerminals MET ON FDL.TerminalId = MET.Id          
 LEFT JOIN DeliveryGroups GRP ON DXT.DeliveryGroupId = GRP.Id          
 LEFT JOIN Users DSPTR ON GRP.CreatedBy = DSPTR.Id          
 LEFT JOIN JobCarrierDetails JC ON JBS.Id = JC.JobId AND JC.IsActive = 1  
 WHERE ORD.AcceptedCompanyId = @CompanyId           
 AND (@DispatcherId IS NULL  OR @DispatcherId ='' OR DSPTR.Id IN ( @DispatcherId))        
 AND (@CustomerId=0 OR CMP.Id=@CustomerId)        
 AND DXT.[Date] >= @FromDate AND DXT.[Date] < @ToDate  
 AND (@InventoryCaptureTypeIds ='' OR JBS.InventoryDataCaptureType IN (SELECT VALUE FROM STRING_SPLIT(@InventoryCaptureTypeIds, ',')))
 AND (@IsStateSearch = 0 OR MST.Code IN (SELECT * FROM @StateList))          
 AND (@IsDriverSearch = 0 OR (DRVR.FirstName + ' ' + DRVR.LastName LIKE @DriverSearch) OR DRVR.PhoneNumber LIKE @DriverSearch)          
 AND (          
  (@Priority = 1           
  AND (DXT.AdditionalInfo IS NUll OR DXT.AdditionalInfo = ''))           
  OR           
  (DXT.AdditionalInfo IS NOT NUll           
  AND DXT.AdditionalInfo != ''           
  AND @Priority = JSON_VALUE(AdditionalInfo,'$.FsPriority'))          
  )          
 AND (          
  (@RegionId IS NULL OR @RegionId = '')           
  OR           
  (DXT.AdditionalInfo IS NOT NUll           
  AND DXT.AdditionalInfo != ''           
 -- AND @RegionId = JSON_VALUE(AdditionalInfo,'$.FsRegionId'))      
  AND JSON_VALUE(AdditionalInfo,'$.FsRegionId') IN (SELECT VALUE FROM STRING_SPLIT(@RegionId, ',')))      
  )  
 AND  
 (  
  @ShowCarrierManaged = 0 OR (  
          @ShowCarrierManaged = 1   
          AND   
          JC.ID > 0   
          AND   
          (  
           @IsCarrierSearch = 0   
           OR   
           JC.CarrierCompanyId IN (SELECT Id FROM @CarrierList)  
          )  
         )  
 )           
 )          
          
 Select *, [FilteredCount]= COUNT(1) OVER()          
 from FinalDriverLocationWithStatus_CTE FD          
 WHERE           
 (@PoNumSearchTypesValid  = 0 OR (@PoNumSearchTypesValid  > 0 AND FD.PoNumber  IN (Select PoNumber from @PoNumSearchTypes   where (PoNumber) like '%' +  SearchVar + '%')))AND   
 (@DR_IDSearchTypesValid  = 0 OR (@DR_IDSearchTypesValid  > 0 AND FD.UniqueOrderNo  IN (Select UniqueOrderNo from @DR_IDSearchTypes   where (UniqueOrderNo) like '%' +  SearchVar + '%')))AND 
  (@NameSearchTypesValid   = 0 OR (@NameSearchTypesValid   > 0 AND FD.FirstName +' '+FD.LastName  IN (Select FirstName+' '+LastName from @NameSearchTypes    where (FirstName+' '+LastName) like '%' +  SearchVar + '%')))     AND      
  (@DNameSearchTypesValid  = 0 OR (@DNameSearchTypesValid  > 0 AND FD.Dispatcher  IN (Select Dispatcher from @DNameSearchTypes   where (Dispatcher) like '%' +  SearchVar + '%')))     AND          
 (@CNameSearchTypesValid  = 0 OR (@CNameSearchTypesValid  > 0 AND FD.Customer  IN (Select Customer from @CNameSearchTypes   where (Customer) like '%' +  SearchVar + '%')))     AND          
 (@PckupSearchTypesValid  = 0 OR (@PckupSearchTypesValid  > 0 AND FD.Pickup  IN (Select Pickup from @PckupSearchTypes   where (Pickup) like '%' +  SearchVar + '%')))     AND          
 (@LocSearchTypesValid    = 0 OR (@LocSearchTypesValid    > 0 AND FD.Location  IN (Select Location from @LocSearchTypes     where (Location) like '%' +  SearchVar + '%')))     AND          
 --(@LTSearchTypesValid    = 0 OR (@LTSearchTypesValid    > 0 AND FD.InventoryDataCaptureType IN (Select InventoryDataCaptureType from @LTSearchTypes   where (InventoryDataCaptureType) like '%' +  SearchVar + '%')))     AND 
 (@PrdtNmSearchTypesValid = 0 OR (@PrdtNmSearchTypesValid > 0 AND FD.ProductName  IN (Select ProductName from @PrdtNmSearchTypes  where (ProductName) like '%' +  SearchVar + '%')))     AND          
 (@QtySearchTypesValid    = 0 OR (@QtySearchTypesValid    > 0 AND FD.Quantity  IN (Select Quantity from @QtySearchTypes     where (Quantity) like '%' +  SearchVar + '%')))     AND          
 (@LdDateSearchTypesValid = 0 OR (@LdDateSearchTypesValid > 0 AND FD.Date  IN (Select Date from @LdDateSearchTypes  where (Date) like '%' +  SearchVar + '%')))     AND          
 (@StatusSearchTypesValid = 0 OR (@StatusSearchTypesValid > 0 AND FD.Status  IN (Select Status from @StatusSearchTypes  where (Status) like '%' +  SearchVar + '%')))    AND  
 (@DropTicketSearchTypesValid = 0 OR (@DropTicketSearchTypesValid > 0 AND FD.DROPTicketNum  IN (Select DROPTicketNum from @DropTicketSearchTypes  where (DROPTicketNum) like '%' +  SearchVar + '%')))           
 AND          
  (          
   @GlobalSearchText IS NULL           
   OR          
   (          
    (FD.PoNumber like '%' + @GlobalSearchText+ '%') 
	OR (FD.UniqueOrderNo like '%' + @GlobalSearchText+ '%')
    OR (FD.FirstName like '%' + @GlobalSearchText+ '%')          
    OR (FD.LastName like '%' + @GlobalSearchText+ '%')          
    OR (FD.Dispatcher like '%' + @GlobalSearchText+ '%')          
    OR (FD.Customer like '%' + @GlobalSearchText+ '%')          
    OR (FD.Pickup like '%' + @GlobalSearchText+ '%')          
    OR (FD.Location like '%' + @GlobalSearchText+ '%')          
    OR (FD.ProductName like '%' + @GlobalSearchText+ '%')          
    OR (FD.Quantity like '%' + @GlobalSearchText+ '%')          
    OR (FD.Date like '%' + @GlobalSearchText+ '%')          
    OR (FD.Status like '%' + @GlobalSearchText+ '%')       
 OR (FD.DROPTicketNum like '%' + @GlobalSearchText+ '%') 
 OR (FD.InventoryDataCaptureType like '%' + @GlobalSearchText+ '%')   
   )          
  )          
ORDER BY          
 CASE          
 WHEN @SortId = 0 AND @SortDirection = 'asc' THEN FD.PoNumber                  
 WHEN @SortId = 7 AND @SortDirection = 'asc' THEN FD.ProductName          
 WHEN @SortId = 9 AND @SortDirection = 'asc' THEN FD.Date   
 WHEN @SortId = 1 AND @SortDirection = 'asc' THEN FD.UniqueOrderNo 
 WHEN @SortId = 2 AND @SortDirection = 'asc' THEN (FD.FirstName + FD.LastName)       
 WHEN @SortId = 3 AND @SortDirection = 'asc' THEN FD.Dispatcher    
 WHEN @SortId = 4 AND @SortDirection = 'asc' THEN FD.Customer    
 WHEN @SortId = 5 AND @SortDirection = 'asc' THEN FD.Pickup          
 WHEN @SortId = 6 AND @SortDirection = 'asc' THEN FD.Location          
 WHEN @SortId = 10 AND @SortDirection = 'asc' THEN FD.Status    
 WHEN @SortId = 11 AND @SortDirection = 'asc' THEN FD.DROPTicketNum    
 END ASC,                      
 CASE             
 WHEN @SortId = 8 AND @SortDirection = 'asc' THEN FD.Quantity        
 END ASC,  
  CASE             
  WHEN @SortId = 12 AND @SortDirection = 'asc' THEN FD.InventoryDataCaptureType         
 END ASC, 
 CASE                       
 WHEN @SortId = 0 AND @SortDirection = 'desc' THEN FD.PoNumber                  
 WHEN @SortId = 7 AND @SortDirection = 'desc' THEN FD.ProductName          
 WHEN @SortId = 9 AND @SortDirection = 'desc' THEN FD.Date   
 WHEN @SortId = 1 AND @SortDirection = 'desc' THEN FD.UniqueOrderNo
 WHEN @SortId = 2 AND @SortDirection = 'desc' THEN (FD.FirstName + FD.LastName)          
 WHEN @SortId = 3 AND @SortDirection = 'desc' THEN FD.Dispatcher    
 WHEN @SortId = 4 AND @SortDirection = 'desc' THEN FD.Customer    
 WHEN @SortId = 5 AND @SortDirection = 'desc' THEN FD.Pickup          
 WHEN @SortId = 6 AND @SortDirection = 'desc' THEN FD.Location          
 WHEN @SortId = 10 AND @SortDirection = 'desc' THEN FD.Status   
  WHEN @SortId = 11 AND @SortDirection = 'desc' THEN FD.DROPTicketNum   
 END DESC,                      
 CASE                
 WHEN @SortId = 8 AND @SortDirection = 'desc' THEN FD.Quantity             
 END DESC,
   CASE             
  WHEN @SortId = 12 AND @SortDirection = 'desc' THEN FD.InventoryDataCaptureType         
 END DESC
 OFFSET (@PageNumber - 1) * @PageSize ROWS           
 FETCH NEXT @PageSize ROWS ONLY           
END   
GO