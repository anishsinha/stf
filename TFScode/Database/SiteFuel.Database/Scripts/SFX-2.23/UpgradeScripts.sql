
GO
/****** Object:  StoredProcedure [dbo].[usp_GetLiftFileAccrualReport]    Script Date: 04/25/2022 3:35:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER    PROCEDURE [dbo].[usp_GetLiftFileAccrualReport]  
 -- Add the parameters for the stored procedure here  
 @CompanyId INT ,  
 @FromDate datetimeoffset =NULL,  
 @ToDATE datetimeoffset = NULL,  
 @ProductTypeIds nvarchar(500) = '',  
 @GlobalSearchText VARCHAR(30) = NULL,            
 @SortId INT = 0,            
 @SortDirection VARCHAR(6) = 'desc',            
 @PageSize INT = 99999999,            
 @PageNumber INT = 1,          
 @CallIdSearchTypes [dbo].SearchTypes READONLY,            
 @BOLSearchTypes [dbo].SearchTypes READONLY,            
 @TerminalNameSearchTypes [dbo].SearchTypes READONLY,      
 @TerminalsSearchTypes [dbo].SearchTypes READONLY,  
 @TerminalItemCodeSearchTypes [dbo].SearchTypes READONLY,            
 @ProductTypeSearchTypes [dbo].SearchTypes READONLY,     
 @correctedQuantitySearchTypes [dbo].SearchTypes READONLY,      
 @LoadDateSearchTypes [dbo].SearchTypes READONLY,            
 @CarrierIDSearchTypes [dbo].SearchTypes READONLY,            
 @CarrierNameSearchTypes [dbo].SearchTypes READONLY,            
 @FileNameSearchTypes [dbo].SearchTypes READONLY,  
 @RecordStatusSearchTypes [dbo].SearchTypes READONLY,  
 @ReasonSearchTypes [dbo].SearchTypes READONLY,  
 @RecordDateSearchTypes [dbo].SearchTypes READONLY,  
 @UsernameSearchTypes [dbo].SearchTypes READONLY,  
 @ModifiedDateSearchTypes [dbo].SearchTypes READONLY,  
 @ReasonCodeSearchTypes [dbo].SearchTypes READONLY,  
 @ReasonCategorySearchTypes [dbo].SearchTypes READONLY  
 AS  
BEGIN  
   
  DECLARE @CallIdSearchTypesValid  INT SET @CallIdSearchTypesValid   = (SELECT COUNT(*) FROM @CallIdSearchTypes)     
  DECLARE @BOLSearchTypesValid   INT SET @BOLSearchTypesValid   = (SELECT COUNT(*) FROM @BOLSearchTypes)      
  DECLARE @TerminalNameSearchTypesValid  INT SET @TerminalNameSearchTypesValid  = (SELECT COUNT(*) FROM @TerminalNameSearchTypes)      
  DECLARE @TerminalsSearchTypesValid  INT SET @TerminalsSearchTypesValid  = (SELECT COUNT(*) FROM @TerminalsSearchTypes)  
  DECLARE @TerminalItemCodeSearchTypesValid  INT SET @TerminalItemCodeSearchTypesValid  = (SELECT COUNT(*) FROM @TerminalItemCodeSearchTypes)             
  DECLARE @ProductTypeSearchTypesValid    INT SET @ProductTypeSearchTypesValid    = (SELECT COUNT(*) FROM @ProductTypeSearchTypes)   
  DECLARE @correctedQuantitySearchTypesValid    INT SET @correctedQuantitySearchTypesValid    = (SELECT COUNT(*) FROM @correctedQuantitySearchTypes)   
  DECLARE @LoadDateSearchTypesValid INT SET @LoadDateSearchTypesValid = (SELECT COUNT(*) FROM @LoadDateSearchTypes)            
  DECLARE @CarrierIDSearchTypesValid    INT SET @CarrierIDSearchTypesValid    = (SELECT COUNT(*) FROM @CarrierIDSearchTypes)            
  DECLARE @CarrierNameSearchTypesValid INT SET @CarrierNameSearchTypesValid = (SELECT COUNT(*) FROM  @CarrierNameSearchTypes)            
  DECLARE @FileNameSearchTypesValid INT SET @FileNameSearchTypesValid = (SELECT COUNT(*) FROM @FileNameSearchTypes)   
  DECLARE @RecordDateSearchTypesValid INT SET @RecordDateSearchTypesValid = (SELECT COUNT(*) FROM @RecordDateSearchTypes)   
  DECLARE @ReasonSearchTypesValid INT SET @ReasonSearchTypesValid  = (SELECT COUNT(*) FROM @ReasonSearchTypes)  
  DECLARE @RecordStatusSearchTypesValid INT SET @RecordStatusSearchTypesValid  = (SELECT COUNT(*) FROM @RecordStatusSearchTypes)  
  DECLARE @UsernameSearchTypesValid INT SET @UsernameSearchTypesValid  = (SELECT COUNT(*) FROM @UsernameSearchTypes)  
  DECLARE @ModifiedDateSearchTypesValid INT SET @ModifiedDateSearchTypesValid  = (SELECT COUNT(*) FROM @ModifiedDateSearchTypes)   
  DECLARE @ReasonCodeSearchTypesValid INT SET @ReasonCodeSearchTypesValid  = (SELECT COUNT(*) FROM @ReasonCodeSearchTypes)    
   DECLARE @ReasonCategorySearchTypesValid INT SET @ReasonCategorySearchTypesValid  = (SELECT COUNT(*) FROM @ReasonCategorySearchTypes)   
  ;WITH LFVRecords AS           
    (  
    SELECT           
     LFR.Id as LiftFileRecordId,    
     LFR.LiftFileId as CallId,  
     CASE WHEN (LFR.BOL ='' OR LFR.BOL IS NULL) THEN '--' ELSE LFR.BOL END AS bol,          
     CASE WHEN (LFR.TerminalCode ='' OR LFR.TerminalCode IS NULL) THEN '--' ELSE LFR.TerminalCode END AS TerminalName,  
     dbo.usf_GetTerminalNameByTerminalCodeForLFV(LFR.TerminalCode, @CompanyId) as Terminals,   
     LFR.CorrectedQty AS correctedQuantity,          
     CASE WHEN (LFR.TermItemCode ='' OR LFR.TermItemCode IS NULL) THEN '--' ELSE LFR.TermItemCode END AS TerminalItemCode,          
     CASE WHEN (MstProd.Name ='' OR MstProd.Name IS NULL) THEN '--' ELSE MstProd.Name END AS ProductType,  
     MstProd.Id as ProductTypeId,  
     CASE WHEN (LFR.LoadDate ='' OR LFR.LoadDate IS NULL) THEN '--' ELSE LFR.LoadDate END AS LoadDate,          
     CASE WHEN (LFR.AddedDate ='' OR LFR.AddedDate IS NULL) THEN '--' ELSE CONVERT(NVARCHAR(10), LFR.AddedDate, 101) END  AS RecordDate,                  
     CASE WHEN LFR.Status = 1 THEN 'Clean'   
    WHEN LFR.Status = 2 THEN 'Pending'   
    WHEN LFR.Status= 3 THEN 'No Match'  
    WHEN LFR.Status = 4 THEN 'Ignore Match'  
    WHEN LFR.Status = 5 THEN 'Un-Matched'  
    WHEN LFR.Status = 6 THEN 'Active Exceptions'  
    WHEN LFR.Status = 7 THEN 'Reprocess Submitted'  
    WHEN LFR.Status = 8 THEN 'Duplicate'  
    WHEN LFR.Status = 9 THEN 'PartialMatch'  
    WHEN LFR.Status = 10 THEN 'Forced Ignore Match'  
    ELSE 'Pending' END AS RecordStatus,    
     CASE WHEN ((LFR.Reason = '' OR LFR.Reason IS NULL) AND (RCD.Description ='' OR RCD.Description IS NULL)) THEN '--'   
          ELSE ISNULL(RCD.Description,LFR.Reason) END AS Reason,      
     CASE WHEN(LFR.CarrierID ='' OR LFR.CarrierID IS NULL) THEN '--' ELSE LFR.CarrierID END AS CarrierID,   
     ISNULL (dbo.usf_GetCarrierNameForLFVByTerminalCodeAndCarrierId(LFR.CarrierID,LFR.TerminalCode, @CompanyId),LFR.CarrierName) AS CarrierName,     
     CASE WHEN (LFR.Filename = '' OR LFR.Filename IS NULL) THEN '--' ELSE LFR.Filename END AS Filename,  
     CASE WHEN (USR.FirstName IS NULL OR USR.FirstName = '') THEN '--'  
       ELSE  CONCAT(USR.FirstName, ' ', USR.LastName)   
       END AS [Username],  
               CASE WHEN (LFR.UpdatedDate IS NULL OR LFR.UpdatedDate = '') THEN '--'  
            ELSE  FORMAT(LFR.UpdatedDate, 'MM/dd/yyyy HH:mm:ss') END  AS ModifiedDate,  
                ISNULL(RCD.ReasonCode , '--') AS ReasonCode,  
    ISNULL(RC.Name,'--') AS ReasonCategory,  
	dbo.usf_GetDateDiffInDayHrMin(LFD.AddedDate, LFR.StatusChangedDate) AS LFVResolutionTime,
	  CASE WHEN (LFR.InvoiceFtlDetailId IS NOT NULL AND LFR.InvoiceFtlDetailId <>'' 
             AND LFR.LoadDate IS NOT NULL AND LFR.LoadDate <> '' AND LFR.EndTime IS NOT NULL AND LFR.EndTime <> '')  
			 THEN dbo.usf_GetDateDiffInDayHrMin(IFD.CreatedDate ,CONCAT (SUBSTRING(LFR.LoadDate, 1, 2) + '/' +   SUBSTRING(LFR.LoadDate, 3, 2) + '/' + SUBSTRING(LFR.LoadDate, 5, 4),
				' ',SUBSTRING(LFR.EndTime,1,2), ':',SUBSTRING(LFR.EndTime,3,4)) )
		ELSE '--'
		END AS TimeToBol, 
      TotalCount= COUNT(LFR.Id) OVER()        
    FROM LiftFileValidationRecords LFR         
     INNER JOIN LiftFileDetails LFD ON LFD.Id = LFR.LiftFileId          
     LEFT JOIN TerminalItemCodeMappings TICM ON LFR.TermItemCode = TICM.ItemCode AND TICM.IsActive = 1 AND TICM.CompanyId = LFD.CompanyId      
     LEFT JOIN MstTerminalItemDescriptions MstTICM ON TICM.ItemDescriptionId = MstTICM.Id AND MstTICM.IsActive = 1      
     LEFT JOIN MstProductTypes MstProd ON MstProd.Id =MstTICM.ProductTypeId     
     LEFT JOIN USERS USR ON USR.Id =  LFR.UpdatedBy  
     LEFT JOIN ReasonCodeDetails RCD ON RCD.Id = LFR.ReasonCodeId   
     LEFT JOIN ReasonCategories RC ON  RC.Id = RCD.CategoryId
	 LEFT JOIN InvoiceFtlDetails IFD ON IFD.Id =LFR.InvoiceFtlDetailId 
    WHERE LFD.CompanyId = @CompanyId AND LFD.IsActive = 1 AND  
          LFR.IsActive = 1 AND LFR.IsRecordPushedToExternalApi = 0 AND   
    (  
      (@FromDate IS NULL AND CONVERT(date, substring(LoadDate,5,4) + '-' + substring(LoadDate,1,2) + '-' + substring(LoadDate,3,2)) <= CAST(@ToDATE as date))  
      OR   
      ( CONVERT(date, substring(LoadDate,5,4) + '-' + substring(LoadDate,1,2) + '-' + substring(LoadDate,3,2)) BETWEEN CAST(@FromDate AS date) AND CAST(@ToDATE AS date))  
    )  
    AND  
    (  
      (@ProductTypeIds = ''OR @ProductTypeIds IS NULL)   
      OR  
      (  
        ( MstProd.Id IN (SELECT VALUE FROM STRING_SPLIT(@ProductTypeIds, ',')))  
      )  
    ) 
	
 )  
    SELECT * INTO #TempLFVRecords FROM LFVRecords ORDER BY LiftFileRecordId DESC   
 Select *, [FilteredCount]= COUNT(1) OVER()  FROM #TempLFVRecords  
 WHERE  (           
    @CallIdSearchTypesValid  = 0           
    OR      (           
    @CallIdSearchTypesValid  > 0           
    AND      CallId IN           
     (           
       SELECT CallId           
       FROM   @CallIdSearchTypes         
       WHERE  Cast(CallId AS VARCHAR(100)) COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))    
   AND       
         (       
     
    @BOLSearchTypesValid  = 0           
    OR      (           
    @BOLSearchTypesValid  > 0           
    AND      bol IN           
     (           
       SELECT CAST(bol AS varchar(1000))              
       FROM   @BOLSearchTypes         
       WHERE  bol  LIKE '%' + SearchVar + '%')))   
   AND       
         (           
     @TerminalNameSearchTypesValid = 0           
    OR      (           
     @TerminalNameSearchTypesValid > 0           
    AND      TerminalName IN           
     (           
       SELECT CAST(TerminalName AS varchar(1000))    
       FROM   @TerminalNameSearchTypes          
       WHERE  TerminalName COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))   
    AND                        
         (           
     @TerminalsSearchTypesValid = 0           
    OR      (           
     @TerminalsSearchTypesValid > 0           
    AND      Terminals IN           
     (           
       SELECT CAST(Terminals AS varchar(1000))    
       FROM   @TerminalsSearchTypes          
       WHERE  Terminals COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))   
  
   AND       
         (           
    @ProductTypeSearchTypesValid = 0           
    OR      (           
    @ProductTypeSearchTypesValid > 0           
    AND      ProductType IN           
     (           
       SELECT  CAST(ProductType AS varchar(1000))                                             
       FROM   @ProductTypeSearchTypes         
       WHERE  ProductType COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND       
         (           
    @correctedQuantitySearchTypesValid = 0           
    OR      (           
    @correctedQuantitySearchTypesValid  > 0           
    AND      correctedQuantity IN           
     (           
       SELECT correctedQuantity           
       FROM   @correctedQuantitySearchTypes          
       WHERE  Cast(CorrectedQuantity AS VARCHAR(100)) COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))   
   AND       
         (           
    @LoadDateSearchTypesValid = 0           
    OR      (           
    @LoadDateSearchTypesValid > 0           
    AND      LoadDate IN           
     (           
       SELECT CAST(LoadDate  AS varchar(1000))                                      
       FROM   @LoadDateSearchTypes          
       WHERE  LoadDate COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND       
         (           
    @RecordDateSearchTypesValid = 0           
    OR      (           
    @RecordDateSearchTypesValid > 0           
    AND      RecordDate IN           
     (           
       SELECT  CAST(RecordDate  AS varchar(1000))   
       FROM   @RecordDateSearchTypes          
       WHERE  RecordDate COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND       
         (           
    @CarrierIdSearchTypesValid = 0           
    OR      (           
    @CarrierIdSearchTypesValid > 0           
    AND      CarrierId IN           
     (           
       SELECT CAST(CarrierId  AS varchar(1000))                                 
       FROM   @CarrierIdSearchTypes          
       WHERE  CarrierId COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND       
         (           
    @CarrierNameSearchTypesValid = 0           
    OR      (           
    @CarrierNameSearchTypesValid > 0           
    AND    CarrierName IN           
     (           
       SELECT CAST(CarrierName AS varchar(1000))                                             
       FROM   @CarrierNameSearchTypes         
       WHERE  CarrierName COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
      AND       
         (           
    @FileNameSearchTypesValid = 0           
    OR      (           
    @FileNameSearchTypesValid > 0           
    AND    Filename IN           
     (           
       SELECT CAST(Filename AS varchar(1000))                                     
       FROM   @FileNameSearchTypes         
       WHERE  Filename  COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND       
         (           
    @RecordDateSearchTypesValid = 0           
    OR      (           
    @RecordDateSearchTypesValid > 0           
    AND    RecordDate IN           
     (           
       SELECT CAST(RecordDate AS varchar(1000))                                           
       FROM   @RecordDateSearchTypes        
       WHERE  RecordDate COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
     AND       
         (           
    @ReasonSearchTypesValid  = 0           
    OR      (           
    @ReasonSearchTypesValid  > 0           
    AND    Reason IN           
     (           
       SELECT CAST(Reason AS varchar(1000))                                                
       FROM   @ReasonSearchTypes         
       WHERE  Reason COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
     AND       
         (           
    @RecordStatusSearchTypesValid  = 0           
    OR      (           
    @RecordStatusSearchTypesValid  > 0           
    AND    recordStatus IN           
     (           
       SELECT CAST(recordStatus AS varchar(1000))      
       FROM   @RecordStatusSearchTypes        
       WHERE  recordStatus COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND  
   (  
      @ModifiedDateSearchTypesValid = 0           
    OR      (           
    @ModifiedDateSearchTypesValid > 0           
    AND    ModifiedDate IN           
     (           
       SELECT CAST(ModifiedDate AS varchar(1000))                                           
       FROM   @ModifiedDateSearchTypes        
       WHERE  ModifiedDate COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
            AND  
     (  
      @UsernameSearchTypesValid = 0           
     OR      (           
     @UsernameSearchTypesValid > 0           
     AND    Username IN           
      (           
        SELECT CAST(Username AS varchar(1000))                                     
        FROM   @UsernameSearchTypes         
        WHERE  Username  COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND  
     (  
      @ReasonCodeSearchTypesValid= 0           
     OR      (           
     @ReasonCodeSearchTypesValid > 0           
     AND    ReasonCode IN           
      (           
        SELECT CAST(ReasonCode AS varchar(1000))                                     
        FROM   @ReasonCodeSearchTypes         
        WHERE  ReasonCode  COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
           AND  
     (  
      @ReasonCategorySearchTypesValid= 0           
     OR      (           
     @ReasonCategorySearchTypesValid > 0           
     AND    ReasonCategory IN           
      (           
        SELECT CAST(ReasonCategory AS varchar(1000))                                     
        FROM   @ReasonCategorySearchTypes         
        WHERE  ReasonCategory  COLLATE DATABASE_DEFAULT LIKE '%' + SearchVar + '%')))  
   AND  
   (  
    @GlobalSearchText IS NULL           
    OR   
    (            
     ( bol LIKE '%' + @GlobalSearchText+ '%')    
     OR  
     ( TerminalName LIKE '%' + @GlobalSearchText+ '%')  
     OR  
     ( Terminals LIKE '%' + @GlobalSearchText+ '%')  
     OR  
     ( correctedQuantity LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( TerminalItemCode LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( ProductType LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( LoadDate LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( RecordDate LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( CarrierID LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( Reason LIKE '%' + @GlobalSearchText+ '%')   
     OR  
     ( CarrierName LIKE '%'+ @GlobalSearchText+ '%')  
     OR  
     ( CallId LIKE '%'+ @GlobalSearchText+ '%')  
        OR  
     ( recordStatus LIKE '%'+ @GlobalSearchText+ '%')  
     OR  
     ( Filename LIKE '%'+ @GlobalSearchText+ '%')  
     OR  
     ( Username LIKE '%'+ @GlobalSearchText+ '%')  
     OR  
     ( ModifiedDate LIKE '%'+ @GlobalSearchText+ '%')  
     OR  
     ( ReasonCode LIKE '%'+ @GlobalSearchText+ '%')  
     OR  
     ( ReasonCategory LIKE '%'+ @GlobalSearchText+ '%')  
    )  
   )  
  ORDER BY           
  CASE     
     WHEN @SortId = 7         
     AND      @SortDirection = 'asc' THEN correctedQuantity   
     WHEN @SortId = 0          
     AND      @SortDirection = 'asc' THEN CallId      
   END ASC,          
   CASE    
     WHEN @SortId = 2           
     AND      @SortDirection = 'asc' THEN bol           
     WHEN @SortId = 3          
     AND      @SortDirection = 'asc' THEN TerminalName    
     WHEN @SortId = 4         
     AND      @SortDirection = 'desc' THEN Terminals  
     WHEN @SortId = 5          
     AND      @SortDirection = 'asc' THEN TerminalItemCode           
     WHEN @SortId = 6         
     AND      @SortDirection = 'asc' THEN ProductType           
     WHEN @SortId =  8          
     AND      @SortDirection = 'asc' THEN LoadDate          
     WHEN @SortId = 9           
     AND      @SortDirection = 'asc' THEN CarrierID          
     WHEN @SortId = 10         
     AND      @SortDirection = 'asc' THEN CarrierName    
     WHEN @SortId = 11          
     AND      @SortDirection = 'asc' THEN Filename    
     WHEN @SortId = 12          
     AND      @SortDirection = 'asc' THEN RecordStatus    
     WHEN @SortId = 18         
     AND      @SortDirection = 'asc' THEN Reason    
     WHEN @SortId = 1        
     AND      @SortDirection = 'asc' THEN RecordDate   
     WHEN @SortId = 13         
     AND      @SortDirection = 'asc' THEN Username  
      WHEN @SortId = 14         
     AND      @SortDirection = 'asc' THEN ModifiedDate   
     WHEN @SortId = 15        
     AND      @SortDirection = 'asc' THEN ReasonCode   
     WHEN @SortId = 16        
     AND      @SortDirection = 'asc' THEN ReasonCode   
   END ASC,          
   CASE          
     WHEN @SortId = 7          
     AND      @SortDirection = 'desc' THEN correctedQuantity   
     WHEN @SortId = 0           
     AND      @SortDirection = 'desc' THEN CallId      
   END DESC,          
   CASE     
     WHEN @SortId = 2           
     AND      @SortDirection = 'desc' THEN bol           
     WHEN @SortId = 3         
     AND      @SortDirection = 'desc' THEN TerminalName    
     WHEN @SortId = 4         
     AND      @SortDirection = 'desc' THEN Terminals  
     WHEN @SortId = 5          
     AND      @SortDirection = 'desc' THEN TerminalItemCode           
     WHEN @SortId = 6          
     AND      @SortDirection = 'desc' THEN ProductType           
     WHEN @SortId = 8           
     AND      @SortDirection = 'desc' THEN LoadDate          
     WHEN @SortId = 9           
     AND      @SortDirection = 'desc' THEN CarrierID          
     WHEN @SortId = 10          
     AND      @SortDirection = 'desc' THEN CarrierName    
     WHEN @SortId = 11          
     AND      @SortDirection = 'desc' THEN Filename    
     WHEN @SortId = 12         
     AND      @SortDirection = 'desc' THEN RecordStatus    
     WHEN @SortId = 18        
     AND      @SortDirection = 'desc' THEN Reason    
     WHEN @SortId = 1        
     AND      @SortDirection = 'desc' THEN RecordDate   
     WHEN @SortId = 13        
     AND      @SortDirection = 'desc' THEN Username    
     WHEN @SortId = 14        
     AND      @SortDirection = 'desc' THEN ModifiedDate   
      WHEN @SortId = 15        
     AND      @SortDirection = 'desc' THEN RecordDate   
      WHEN @SortId = 16       
     AND      @SortDirection = 'desc' THEN ReasonCategory   
   END DESC          
   OFFSET (@PageNumber -1) * @PageSize ROWS           
  FETCH NEXT @PageSize ROWS ONLY   
 DROP TABLE IF EXISTS #TempLFVRecords  
END  
Go 

GO
/****** Object:  StoredProcedure [dbo].[usp_GetLiftFileRecordsByDateTimeWindow]    Script Date: 04/25/2022 3:51:26 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================  
-- Author:  yash  
-- Create date: 3rd Feb 2021  
-- Description: Fetch lift file validation records between given date time span   
-- =============================================  
 --exec usp_GetLiftFileRecordsByDateTimeWindow 6742,'04/22/2022','4/25/2022'  
CREATE OR ALTER    PROCEDURE [dbo].[usp_GetLiftFileRecordsByDateTimeWindow]   
 -- Add the parameters for the stored procedure here  
 @CompanyId int,  
 @StartDate date,         
    @EndDate date   
AS  
BEGIN  
   SELECT       
  LFD.Id as CallId,      
  CASE WHEN (LFR.BOL ='' OR LFR.BOL IS NULL) THEN '--' ELSE LFR.BOL END AS bol,      
  CASE WHEN (LFR.TerminalCode ='' OR LFR.TerminalCode IS NULL) THEN '--' ELSE LFR.TerminalCode END AS TerminalName,      
  LFR.CorrectedQty AS correctedQuantity,      
  CASE WHEN (LFR.TermItemCode ='' OR LFR.TermItemCode IS NULL) THEN '--' ELSE LFR.TermItemCode END AS TerminalItemCode,      
  CASE WHEN (MstProd.Name ='' OR MstProd.Name IS NULL) THEN '--' ELSE MstProd.Name END AS ProductType,      
  CASE WHEN (LFR.LoadDate ='' OR LFR.LoadDate IS NULL)   
  THEN '--' ELSE SUBSTRING(lfr.loaddate, 1, 2) + '/' +   SUBSTRING(lfr.loaddate, 3, 2) + '/' + SUBSTRING(lfr.loaddate, 5, 4) END AS LoadDate,            
  CASE WHEN Status = 1 THEN 'Clean'   
   WHEN Status = 2 THEN 'Pending'   
   WHEN Status= 3 THEN 'No Match'  
   WHEN Status = 4 THEN 'Ignore Match'  
   WHEN Status = 5 THEN 'Un-Matched'  
   WHEN Status = 6 THEN 'Active Exceptions'  
   WHEN Status = 7 THEN 'Reprocess Submitted'  
   WHEN Status = 8 THEN 'Duplicate'  
   WHEN Status = 9 THEN 'PartialMatch'  
   WHEN Status = 10 THEN 'Forced Ignore Match'  
   ELSE 'Pending' END AS recordStatus,  
   CASE WHEN (LFR.Reason ='' OR LFR.Reason IS NULL) THEN '--' ELSE LFR.Reason END AS Reason,  
  CASE WHEN(LFR.CarrierID ='' OR LFR.CarrierID IS NULL) THEN '--' ELSE LFR.CarrierID END AS CarrierID,  
  CASE WHEN (LFD.AddedDate ='' OR LFD.AddedDate IS NULL) THEN '--' ELSE CONVERT(NVARCHAR(10), LFD.AddedDate, 101) END  AS RecordDate,  
   CASE WHEN (LFR.StatusChangedDate ='' OR LFR.StatusChangedDate IS NULL) THEN '--' ELSE FORMAT(dbo.ConvertUTCToTargetTimeZoneDateTime(LFR.StatusChangedDate,'Mountain Standard Time'),'MM/dd/yyyy hh:mm:s tt') END  AS statusChangeDate,  
  --CASE WHEN(LFR.CarrierName ='' OR LFR.CarrierName IS NULL) THEN '--' ELSE LFR.CarrierName END AS CarrierName,  
  ISNULL (dbo.usf_GetCarrierNameForLFVByTerminalCodeAndCarrierId(LFR.CarrierID,LFR.TerminalCode, @CompanyId),LFR.CarrierName) AS CarrierName, -- impediment fix    
  CASE WHEN(LFR.Filename ='' OR LFR.Filename IS NULL) THEN '--' ELSE LFR.Filename END AS Filename,  
  LFR.Status,  
  CASE WHEN (LFR.Reason ='' OR LFR.Reason IS NULL) THEN '--' ELSE LFR.Reason END AS IgnoredReason,  
  CASE WHEN(LFR.ReasonCodeId IS NULL) THEN CASE WHEN LFR.[Status]=10 THEN LFR.Reason ELSE '--' END ELSE LFR.Reason END AS ForcedIgnoreReason,  
  CASE WHEN(LFR.ReasonCodeId IS NULL) THEN '--' ELSE ISNULL((Select Top 1 RCD.ReasonCode from ReasonCodeDetails RCD where RCD.Id=LFR.ReasonCodeId ),'--') END AS ReasonCode,  
  CASE WHEN(LFR.ReasonCodeId IS NULL) THEN '--' ELSE ISNULL((Select Top 1 RC.Name from ReasonCodeDetails RCD INNER JOIN ReasonCategories RC ON RC.Id=RCD.CategoryId where RCD.Id=LFR.ReasonCodeId),'--') END AS ReasonCategory,  
  CASE WHEN(LFR.UpdatedBy IS NULL) THEN '--' ELSE ISNULL((Select Top 1 US.FirstName+' '+US.LastName  from Users US where US.Id=LFR.UpdatedBy and US.IsActive=1 and US.IsDeleted=0),'--') END AS Username,  
  CASE WHEN (LFR.UpdatedDate IS NULL OR LFR.UpdatedDate = '') THEN '--'  
      ELSE FORMAT(dbo.ConvertUTCToTargetTimeZoneDateTime(LFR.UpdatedDate,'Mountain Standard Time'),'MM/dd/yyyy') END  AS ModifiedDate,  
  CASE WHEN (LFR.UpdatedDate IS NULL OR LFR.UpdatedDate = '') THEN '--'  
      ELSE FORMAT(dbo.ConvertUTCToTargetTimeZoneDateTime(LFR.UpdatedDate,'Mountain Standard Time'),'MM/dd/yyyy hh:mm:s tt') END AS LFVCarrierPerModifiedDate ,
	  	dbo.usf_GetDateDiffInDayHrMin(LFD.AddedDate, LFR.StatusChangedDate) AS LFVResolutionTime,
		 CASE WHEN (LFR.InvoiceFtlDetailId IS NOT NULL AND LFR.InvoiceFtlDetailId <>'' 
             AND LFR.LoadDate IS NOT NULL AND LFR.LoadDate <> '' AND LFR.EndTime IS NOT NULL AND LFR.EndTime <> '')  
			 THEN dbo.usf_GetDateDiffInDayHrMin(IFD.CreatedDate ,CONCAT (SUBSTRING(LFR.LoadDate, 1, 2) + '/' +   SUBSTRING(LFR.LoadDate, 3, 2) + '/' + SUBSTRING(LFR.LoadDate, 5, 4),
				' ',SUBSTRING(LFR.EndTime,1,2), ':',SUBSTRING(LFR.EndTime,3,4)) )
		ELSE '--'
		END AS TimeToBol
  FROM LiftFileDetails LFD       
  INNER JOIN LiftFileValidationRecords LFR ON LFD.Id = LFR.LiftFileId      
  LEFT JOIN TerminalItemCodeMappings TICM ON LFR.TermItemCode = TICM.ItemCode AND TICM.IsActive = 1   
   AND TICM.CompanyId = LFD.CompanyId  
  LEFT JOIN MstTerminalItemDescriptions MstTICM ON TICM.ItemDescriptionId = MstTICM.Id AND MstTICM.IsActive = 1  
  LEFT JOIN MstProductTypes MstProd ON MstProd.Id = MstTICM.ProductTypeId 
  LEFT JOIN InvoiceFtlDetails IFD ON IFD.Id = LFR.InvoiceFtlDetailId
  WHERE LFD.CompanyId = @CompanyId   
    --AND   CAST(LFR.AddedDate AS date) = @StartDate --AND CAST (LFR.AddedDate AS date) <= @EndDate   
    AND (  
    (CAST(LFR.AddedDate AS date) BETWEEN @StartDate AND @EndDate  AND LFR.IsActive = 1)  
    OR   
    (CAST(LFR.StatusChangedDate AS date) = @EndDate AND LFR.Status = 5)  
   )  
  AND LFD.IsActive =1  
  Order by  LFR.Id desc  
END  
Go

GO
/****** Object:  StoredProcedure [dbo].[usp_LiftFileRecordsByStatus]    Script Date: 04/25/2022 4:23:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --exec usp_LiftFileRecordsByStatus 6742,3,0,null,null,''
CREATE OR ALTER      PROCEDURE [dbo].[usp_LiftFileRecordsByStatus]              
    @CompanyId int,            
    @RecordStatus int,            
    @CallId int  ,      
    @StartDate Datetimeoffset = NULL,        
    @EndDate Datetimeoffset = NULL  ,    
  @carrierIds nvarchar(1000)     
 AS            
  BEGIN          
  Declare @CarrierList Table ([Name] NVARCHAR(256))   
  INSERT INTO @CarrierList SELECT VALUE FROM STRING_SPLIT(@carrierIds, ',') ORDER BY VALUE     
 SELECT  
  LFR.Id as LiftFileRecordId,            
  CASE WHEN (LFR.BOL ='' OR LFR.BOL IS NULL) THEN '--' ELSE LFR.BOL END AS bol,            
  CASE WHEN (LFR.TerminalCode ='' OR LFR.TerminalCode IS NULL) THEN '--' ELSE LFR.TerminalCode END AS TerminalName,    
   dbo.usf_GetTerminalNameByTerminalCodeForLFV(LFR.TerminalCode, @CompanyId) as Terminals,   
  LFR.CorrectedQty AS correctedQuantity,            
  CASE WHEN (LFR.TermItemCode ='' OR LFR.TermItemCode IS NULL) THEN '--' ELSE LFR.TermItemCode END AS TerminalItemCode,            
  CASE WHEN (MstProd.Name ='' OR MstProd.Name IS NULL) THEN '--' ELSE MstProd.Name END AS ProductType,            
  CASE WHEN (LFR.LoadDate ='' OR LFR.LoadDate IS NULL)   
  THEN '--' ELSE SUBSTRING(lfr.loaddate, 1, 2) + '/' +   SUBSTRING(lfr.loaddate, 3, 2) + '/' + SUBSTRING(lfr.loaddate, 5, 4) END AS LoadDate,            
  CASE WHEN (LFR.AddedDate ='' OR LFD.AddedDate IS NULL) THEN '--' ELSE CONVERT(NVARCHAR(10), LFD.AddedDate, 101) END  AS RecordDate, --Record Date             
  CASE WHEN (LFR.StatusChangedDate ='' OR LFR.StatusChangedDate IS NULL OR @RecordStatus != 1) THEN '--' ELSE CONVERT(NVARCHAR(10), LFR.StatusChangedDate, 101) END  AS statusChangeDate,--Record Date                
  LFR.InvoiceFtlDetailId AS InvFtlDetailId,          
  LFR.Status AS Status,        
  --LFR.Reason,      
  ISNULL(RCD.ReasonCode,'--') AS ReasonCode,    
  ISNULL(RC.Name,'--') AS ReasonCategory,    
  ISNULL(RCD.Description,LFR.Reason) AS Reason,    
  CASE WHEN(LFR.CarrierID ='' OR LFR.CarrierID IS NULL) THEN '--' ELSE LFR.CarrierID END AS CarrierID,     
  LFR.IsRecordPushedToExternalApi,    
  LFR.Gross AS GrossQuantity,    
  CASE WHEN(LFR.CIN ='' OR LFR.CIN IS NULL) THEN '--' ELSE LFR.CIN END AS CIN,    
  CASE     
      WHEN (dbo.usf_GetCarrierNameForLFVByTerminalCodeAndCarrierId(LFR.CarrierID,LFR.TerminalCode, @CompanyId) IS NULL OR     
         dbo.usf_GetCarrierNameForLFVByTerminalCodeAndCarrierId(LFR.CarrierID,LFR.TerminalCode, @CompanyId) = '')      
      AND (LFR.CarrierName IS NULL OR LFR.CarrierName = '') THEN '--'    
   ELSE  ISNULL (dbo.usf_GetCarrierNameForLFVByTerminalCodeAndCarrierId(LFR.CarrierID,LFR.TerminalCode, @CompanyId),LFR.CarrierName)     
   END AS CarrierName,    
  CASE WHEN (USR.FirstName IS NULL OR USR.FirstName = '') THEN '--'    
       ELSE  CONCAT(USR.FirstName, ' ', USR.LastName)     
    END AS [Username],    
  CASE WHEN (LFR.UpdatedDate IS NULL OR LFR.UpdatedDate = '') THEN '--'    
      ELSE FORMAT(dbo.ConvertUTCToTargetTimeZoneDateTime(LFR.UpdatedDate,'Mountain Standard Time'),'MM/dd/yyyy') END  AS ModifiedDate,
	dbo.usf_GetDateDiffInDayHrMin(LFD.AddedDate, LFR.StatusChangedDate) AS LFVResolutionTime,
  CASE WHEN (LFR.InvoiceFtlDetailId IS NOT NULL AND LFR.InvoiceFtlDetailId <>'' 
             AND LFR.LoadDate IS NOT NULL AND LFR.LoadDate <> '' AND LFR.EndTime IS NOT NULL AND LFR.EndTime <> '')  
			 THEN dbo.usf_GetDateDiffInDayHrMin(IFD.CreatedDate ,CONCAT (SUBSTRING(LFR.LoadDate, 1, 2) + '/' +   SUBSTRING(LFR.LoadDate, 3, 2) + '/' + SUBSTRING(LFR.LoadDate, 5, 4),
				' ',SUBSTRING(LFR.EndTime,1,2), ':',SUBSTRING(LFR.EndTime,3,4)) )
		ELSE '--'
		END AS TimeToBol       
  FROM LiftFileDetails LFD             
  INNER JOIN LiftFileValidationRecords LFR ON LFD.Id = LFR.LiftFileId            
  LEFT JOIN TerminalItemCodeMappings TICM ON LFR.TermItemCode = TICM.ItemCode AND TICM.IsActive = 1         
   AND TICM.CompanyId = LFD.CompanyId        
  LEFT JOIN MstTerminalItemDescriptions MstTICM ON TICM.ItemDescriptionId = MstTICM.Id AND MstTICM.IsActive = 1        
  LEFT JOIN MstProductTypes MstProd ON MstProd.Id = MstTICM.ProductTypeId        
  LEFT JOIN Users USR ON USR.Id = LFR.UpdatedBy    
  LEFT JOIN ReasonCodeDetails RCD ON RCD.Id = LFR.ReasonCodeId    
  LEFT JOIN ReasonCategories RC ON RC.Id =RCD.CategoryId 
  LEFT JOIN InvoiceFtlDetails IFD ON IFD.Id =LFR.InvoiceFtlDetailId
  WHERE LFD.CompanyId = @CompanyId AND LFD.IsActive = 1 AND LFR.IsActive = 1 AND             
  LFR.Status =@RecordStatus AND            
 ( @CallId IS NULL OR  @CallId =0 OR LFR.LiftFileId = @CallId ) AND      
 (@StartDate IS NULL OR CONVERT(Date, LFD.AddedDate) >= CONVERT(DATE , @StartDate))  AND      
 (@EndDate IS NULL OR CONVERT(Date, LFD.AddedDate) <= CONVERT(DATE ,@EndDate)) AND (@carrierIds IS NULL OR @carrierIds =''  OR   LFR.CarrierID IN (Select [Name] from @CarrierList) )      
  Order by  LFR.Id desc            
END  
Go

GO
/****** Object:  UserDefinedFunction [dbo].[usf_GetDateDiffInDayHrMin]    Script Date: 04/25/2022 4:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 --SELECT [dbo].[usf_GetDateDiffInDayHrMin] ('2022-04-06 03:56:00.6084935 +00:00','2022-04-06 05:36:23.0000000 +00:00')
  --=========================
  
CREATE OR ALTER   FUNCTION [dbo].[usf_GetDateDiffInDayHrMin]
(
	@StartDate	DATETIMEOFFSET(7),
	@EndDate	DATETIMEOFFSET(7)
)
RETURNS NVARCHAR(256)
AS
BEGIN
	DECLARE @Result NVARCHAR(256)

	SELECT @Result = 
	CASE WHEN ABS(DATEDIFF(MINUTE, @StartDate, @EndDate)) < 60 THEN CONCAT(ABS(DATEDIFF(MINUTE, @StartDate, @EndDate)), ' Min')
			WHEN ABS(DATEDIFF(MINUTE, @StartDate, @EndDate)) <= 1440 THEN CONCAT(CONCAT( DATEDIFF(MINUTE, @StartDate, @EndDate)/60, ' Hr '), CONCAT(DATEDIFF(MINUTE, @StartDate, @EndDate)%60, ' Min'))
			ELSE CONCAT(CONCAT(ABS(DATEDIFF(MINUTE, @StartDate, @EndDate))/1440, ' Day '), CONCAT((ABS (DATEDIFF(MINUTE, @StartDate, @EndDate)%1440))/24, ' Hr')) END

	RETURN @Result
END
Go



GO
/****** Object:  StoredProcedure [dbo].[usp_GetSupplierInvoices]    Script Date: 04/26/2022 2:56:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER       PROCEDURE [dbo].[usp_GetSupplierInvoices]   
	@CompanyId int,        
	@OrderId int,        
	@InvoiceTypeId int,        
	@InvoiceFilter int,        
	@ViewInvoices int,        
	@StartDate datetimeoffset(7),        
	@EndDate datetimeoffset(7),        
	@CountryId int = 0,        
	@CurrencyType int = 0,        
	@GroupIds nvarchar(100),         
	@CustomerIds nvarchar(max) = '', 
	@LocationIds nvarchar(max) = '', 
	@VesselIds nvarchar(max) = '',   
    @IsMarine BIT = 0,
	@GlobalSearchText varchar(30) = NULL,        
	@SortId int = 0,        
	@SortDirection varchar(6) = 'desc',        
	@PageSize int = 99999999,        
	@PageNumber int = 1,        
	@InvoiceNumberSearchTypes [dbo].SEARCHTYPES READONLY,    
	@PoNumberSearchTypes [dbo].SEARCHTYPES READONLY,
	@SourcingRequestSearchTypes [dbo].SEARCHTYPES READONLY,
	@QBInvoiceNumberSearchTypes [dbo].SEARCHTYPES READONLY,
	@BDRNumberSearchTypes [dbo].SEARCHTYPES READONLY,
	@BolNumberSearchTypes [dbo].SEARCHTYPES READONLY,
	@DeliveryLevelPOSearchTypes [dbo].SEARCHTYPES READONLY,
	@LiftDateSearchTypes [dbo].SEARCHTYPES READONLY,   
	@CarrierSearchTypes [dbo].SEARCHTYPES READONLY,   
	@BadgeNumberSearchTypes [dbo].SEARCHTYPES READONLY,   
	@SupplierSearchTypes [dbo].SEARCHTYPES READONLY,        
	@LocationSearchTypes [dbo].SEARCHTYPES READONLY,        
	@FuelTypeSearchTypes [dbo].SEARCHTYPES READONLY,        
	@DroppedGallonsSearchTypes [dbo].SEARCHTYPES READONLY,        
	@PrePostValuesSearchTypes [dbo].SEARCHTYPES READONLY,  
	@TerminalSearchTypes [dbo].SEARCHTYPES READONLY,        
	@AmountSearchTypes [dbo].SEARCHTYPES READONLY,        
	@DropDateSearchTypes [dbo].SEARCHTYPES READONLY,        
	@DropTimeSearchTypes [dbo].SEARCHTYPES READONLY,        
	@PriceSearchTypes [dbo].SEARCHTYPES READONLY,        
	@InvoiceDateSearchTypes [dbo].SEARCHTYPES READONLY,        
	@PaymentDueDateSearchTypes [dbo].SEARCHTYPES READONLY, 
	@DriverSearchTypes [dbo].SEARCHTYPES READONLY,    
	@DropTicketNumberSearchTypes [dbo].SEARCHTYPES READONLY,    
	@CreationMethodSearchTypes [dbo].SEARCHTYPES READONLY,       
	@PickupAddressSearchTypes [dbo].SEARCHTYPES READONLY,      
	--@PDIDetailsDateSearchTypes [dbo].SEARCHTYPES READONLY,
	@PDIOrderIdSearchTypes [dbo].SEARCHTYPES READONLY,
	@ExternalPDIExceptionSearchTypes [dbo].SEARCHTYPES READONLY,
	@TotalNetQuantitySearchTypes [dbo].SEARCHTYPES READONLY,
	@TotalGrossQuantitySearchTypes [dbo].SEARCHTYPES READONLY,
	@HasAttachmentsSearchTypes [dbo].SEARCHTYPES READONLY, 
	@VesselNameSearchTypes [dbo].SEARCHTYPES READONLY,
	@StatusSearchTypes [dbo].SEARCHTYPES READONLY,
	@TimeToInvoiceSearchTypes [dbo].SEARCHTYPES READONLY
	--@LiftTicketNumberSearchTypes [dbo].SEARCHTYPES READONLY, 
	
AS        
BEGIN
  DECLARE @InvoiceNumberSearchTypesValid int = (SELECT COUNT(*) FROM @InvoiceNumberSearchTypes);        
  DECLARE @PoNumberSearchTypesValid int = (SELECT COUNT(*) FROM @PoNumberSearchTypes);  
  DECLARE @SourcingRequestSearchTypesValid int = (SELECT COUNT(*) FROM @SourcingRequestSearchTypes);  
  DECLARE @QBInvoiceNumberSearchTypesValid INT = (SELECT Count(*) FROM @QBInvoiceNumberSearchTypes);  
  DECLARE @BolNumberSearchTypesValid int = (SELECT COUNT(*) FROM @BolNumberSearchTypes);  
  DECLARE @CarrierSearchTypesValid int = (SELECT COUNT(*) FROM @CarrierSearchTypes);  
  DECLARE @LiftDateSearchTypesValid int = (SELECT COUNT(*) FROM @LiftDateSearchTypes);  
  DECLARE @BadgeNumberSearchTypesValid int = (SELECT COUNT(*) FROM @BadgeNumberSearchTypes);  
  DECLARE @SupplierSearchTypesValid int = (SELECT COUNT(*) FROM @SupplierSearchTypes);        
  DECLARE @LocationSearchTypesValid int = (SELECT COUNT(*) FROM @LocationSearchTypes);        
  DECLARE @FuelTypeSearchTypesValid int = (SELECT COUNT(*) FROM @FuelTypeSearchTypes);        
  DECLARE @TerminalSearchTypesValid int = (SELECT COUNT(*) FROM @TerminalSearchTypes);        
  --DECLARE @LiftTicketNumberSearchTypesValid int = (SELECT COUNT(*) FROM @LiftTicketNumberSearchTypes);       
  DECLARE @DropTicketNumberSearchTypesValid int = (SELECT COUNT(*) FROM @DropTicketNumberSearchTypes);        
  DECLARE @CreationMethodSearchTypesValid int = (SELECT COUNT(*) FROM @CreationMethodSearchTypes);        
  DECLARE @DropDateSearchTypesValid int = (SELECT COUNT(*) FROM @DropDateSearchTypes);        
  DECLARE @PriceSearchTypesValid int = (SELECT COUNT(*) FROM @PriceSearchTypes);        
  DECLARE @AmountSearchTypesValid int = (SELECT COUNT(*) FROM @AmountSearchTypes);        
  DECLARE @DropTimeSearchTypesValid int = (SELECT COUNT(*) FROM @DropTimeSearchTypes);        
  DECLARE @StatusSearchTypesValid int = (SELECT COUNT(*) FROM @StatusSearchTypes);        
  DECLARE @InvoiceDateSearchTypesValid int = (SELECT COUNT(*) FROM @InvoiceDateSearchTypes);        
  DECLARE @PaymentDueDateSearchTypesValid int = (SELECT COUNT(*) FROM @PaymentDueDateSearchTypes);        
  DECLARE @DriverSearchTypesValid int = (SELECT COUNT(*) FROM @DriverSearchTypes);        
  DECLARE @PDIOrderIdSearchTypesValid int = (SELECT COUNT(*) FROM @PDIOrderIdSearchTypes);
  DECLARE @ExternalPDIExceptionSearchTypesValid int = (SELECT COUNT(*) FROM @ExternalPDIExceptionSearchTypes);
  DECLARE @DroppedGallonsSearchTypesValid int = (SELECT COUNT(*) FROM @DroppedGallonsSearchTypes);
  DECLARE @PickupAddressSearchTypesValid int = (SELECT COUNT(*)FROM @PickupAddressSearchTypes);
  DECLARE @TotalNetQuantitySearchTypesValid int = (SELECT COUNT(*) FROM @TotalNetQuantitySearchTypes);
  DECLARE @TotalGrossQuantitySearchTypesValid int = (SELECT COUNT(*) FROM @TotalGrossQuantitySearchTypes);
  DECLARE @HasAttachmentsSearchTypesValid int= (SELECT COUNT(*) FROM @HasAttachmentsSearchTypes); 
  DECLARE @VesselNameSearchTypesValid int= (SELECT COUNT(*) FROM @VesselNameSearchTypes);
  DECLARE @BDRNumberSearchTypesValid int= (SELECT COUNT(*) FROM @BDRNumberSearchTypes);
  DECLARE @DeliveryLevelPOSearchTypesValid int= (SELECT COUNT(*) FROM @DeliveryLevelPOSearchTypes);  
  DECLARE @TimeToInvoiceSearchTypesValid int = (SELECT COUNT(*) FROM @TimeToInvoiceSearchTypes);

  DECLARE @TblGroupCompanyIds TABLE (
	CompanyId int        
  );  
  INSERT INTO @TblGroupCompanyIds  SELECT * FROM usf_GetGroupCompanyIds(@CompanyId, @GroupIds);
  
  DECLARE @TblCustomerCompanyIds TABLE (Id int);
  DECLARE @TblLocationIds TABLE (Id int);
  DECLARE @TblVesselIds TABLE (Id int);

  INSERT INTO @TblCustomerCompanyIds SELECT VALUE FROM STRING_SPLIT(@CustomerIds ,',')
  INSERT INTO @TblLocationIds SELECT VALUE FROM STRING_SPLIT(@LocationIds ,',');
  INSERT INTO @TblVesselIds SELECT VALUE FROM STRING_SPLIT(@VesselIds ,',');

  --#TempBOLDetails
  SELECT BD.*, INV.DisplayInvoiceNumber AS DisplayInvoiceNumber
  --INV.DisplayInvoiceNumber
  INTO #TempBOLDetails
  FROM dbo.[usf_getBolDetailsForInvoiceV2](@CompanyId, @GroupIds, @InvoiceTypeId, @OrderId, @StartDate, @EndDate) BD
	INNER JOIN dbo.InvoiceHeaderDetails(Nolock) IHD ON BD.InvoiceHeaderID = IHD.ID AND IHD.IsActive = 1
	INNER JOIN dbo.Invoices(nolock) INV ON IHD.Id = INV.InvoiceHeaderId  AND INV.IsActive = 1
	INNER JOIN dbo.InvoiceXInvoiceStatusDetails(Nolock) IXS ON INV.Id = IXS.InvoiceId AND IXS.IsActive = 1;
 
	--#TblBOLDetails
	SELECT
		DISTINCT IH.InvoiceHeaderID  ,   
		BN.BOLNo,				--1 [BOLNo]
		BGN.BadgeNumber,		--2 [BadgeNumber]
		TN.TerminalName,		--3 [TerminalName]
		PA.PickupAddress,		--4 [PickupAddress]
		PG.PricePerGallon,		--5 [PricePerGallon]
		PO.PONumber,			--6 [PoNumber]
		FT.FuelType,			--7 [FuelType] 
		OI.OrderId,				--8 [OrderId]
		II.InvoiceId,			--9 [InvoiceId]
		DG.DroppedGallons,		--10 [DroppedGallons]
		CG.ConvertedQuantity,	--11 [ConvertedQuantity]
		DD.DropDate,			--12 [DropDate]
		DT.DropTime,			--13 [DropTime]
		DTN.DropTicketNumber,	--14 [DropTicketNumber]
		CA.Carrier,				--15 [Carrier],
		LD.LiftDate,			--16 [LiftDate]
		QB.QbInvoiceNumber,		--17 [QbInvoiceNumber]
		DN.DriverName,			--18 [DriverName]
		TQ.TotalNetQty,			--19 [TotalNetQty]
		TGQ.TotalGrossQty,		--20 [TotalGrossQty]
		HA.HasAttachments,		--21 [HasAttachments]
		PV.PrePostValues,		--22 [PrePostValues]
		ID.DisplayInvoiceNumber,--23 [DisplayInvoiceNumber]
		INVDT.InvoiceDate,		--24 [InvoiceDate]
		CM.CreationMethod,		--25 [CreationMethod]
		PAYDT.PaymentDueDate,	--26 [PaymentDueDate]
		IHSN.[StatusName],	    --27 [Status Name]  
		TTI.TimeToInvoice		--28 TimeToInvoice
	INTO #TblBOLDetails
	FROM  #TempBOLDetails IH
	LEFT JOIN (
				SELECT TD.InvoiceHeaderID, TD.DisplayInvoiceNumber,
						ROW_NUMBER() OVER (
							PARTITION BY TD.InvoiceHeaderID
							ORDER BY TD.DisplayInvoiceNumber DESC
						) row_num
					FROM #TempBOLDetails TD
					INNER JOIN Invoices INVS ON TD.InvoiceHeaderId = INVS.InvoiceHeaderId
					WHERE
					(@InvoiceTypeId IN (6, 7) AND INVS.InvoiceTypeId IN (6, 7))          
					OR (@InvoiceTypeId NOT IN (6, 7)  AND INVS.InvoiceTypeId NOT IN (6, 7)) 
				) ID ON IH.InvoiceHeaderID = ID.InvoiceHeaderID AND row_num = 1
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.BOLNo,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.BOLNo),'--')  AS BolNo
				FROM (SELECT DISTINCT InvoiceHeaderID, BOLNo FROM #TempBOLDetails ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) BN ON IH.InvoiceHeaderID = BN.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.BadgeNumber,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.BadgeNumber),'--')  AS BadgeNumber
				FROM (SELECT DISTINCT InvoiceHeaderID, BadgeNumber FROM #TempBOLDetails)  TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) BGN ON IH.InvoiceHeaderID = BGN.InvoiceHeaderID 
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.TerminalName,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.TerminalName),'--')  AS TerminalName
				FROM (SELECT DISTINCT InvoiceHeaderID, TerminalName FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) TN ON IH.InvoiceHeaderID = TN.InvoiceHeaderID 
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.PickupAddress, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.PickupAddress),'--') AS PickupAddress
				FROM (SELECT DISTINCT InvoiceHeaderID, PickupAddress FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) PA ON IH.InvoiceHeaderID = PA.InvoiceHeaderID 
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(TBOL.PricePerGallon, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.PricePerGallon)  AS PricePerGallon
				FROM (SELECT DISTINCT InvoiceHeaderID, PricePerGallon FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) PG ON IH.InvoiceHeaderID = PG.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.PONumber, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.PONumber),'--')  AS PONumber
				FROM (SELECT DISTINCT InvoiceHeaderID, PONumber FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) PO ON IH.InvoiceHeaderID = PO.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.FuelType, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.FuelType),'--')  AS FuelType
				FROM (SELECT DISTINCT InvoiceHeaderID, FuelType FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) FT ON IH.InvoiceHeaderID = FT.InvoiceHeaderID
			 
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.OrderId, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.OrderId),'--')  AS OrderId
				FROM (SELECT DISTINCT InvoiceHeaderID, OrderId FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) OI ON IH.InvoiceHeaderID = OI.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.InvoiceId, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,TBOL.InvoiceId),'--')  AS InvoiceId
				FROM (SELECT DISTINCT InvoiceHeaderID, InvoiceId FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) II ON IH.InvoiceHeaderID = II.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(DroppedGallons, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, DroppedGallons,TBOL.InvoiceId)  AS DroppedGallons
				FROM (SELECT DISTINCT InvoiceHeaderID, DroppedGallons, InvoiceId FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) DG ON IH.InvoiceHeaderID = DG.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(ConvertedQuantity, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,ConvertedQuantity)  AS ConvertedQuantity
				FROM (SELECT DISTINCT InvoiceHeaderID, ConvertedQuantity FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) CG ON IH.InvoiceHeaderID = CG.InvoiceHeaderID
					/*FROM   Invoices INV               
     WHERE  INV.InvoiceHeaderId = IH.id    AND ( INV.OrderId = @OrderId   OR @OrderId = 0)  
	 */
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(DropDate, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,DropDate)  AS DropDate
				FROM (SELECT DISTINCT InvoiceHeaderID, DropDate FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) DD ON IH.InvoiceHeaderID = DD.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(DropTime, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,DropTime)  AS DropTime
				FROM (SELECT DISTINCT InvoiceHeaderID, DropTime FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) DT ON IH.InvoiceHeaderID = DT.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(DropTicketNumber, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,DropTicketNumber)  AS DropTicketNumber
				FROM (SELECT DISTINCT InvoiceHeaderID, DropTicketNumber FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) DTN ON IH.InvoiceHeaderID = DTN.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(Carrier, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,Carrier),'--')  AS Carrier
				FROM (SELECT DISTINCT InvoiceHeaderID, Carrier FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) CA ON IH.InvoiceHeaderID = CA.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(LiftDate, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,LiftDate),'--')  AS LiftDate
				FROM (SELECT DISTINCT InvoiceHeaderID, LiftDate FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) LD ON IH.InvoiceHeaderID = LD.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(QbInvoiceNumber, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,QbInvoiceNumber)  AS QbInvoiceNumber
				FROM (SELECT DISTINCT InvoiceHeaderID, QbInvoiceNumber FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) QB ON IH.InvoiceHeaderID = QB.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					STRING_AGG(DriverName, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,DriverName)  AS DriverName
				FROM (SELECT DISTINCT InvoiceHeaderID, DriverName FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) DN ON IH.InvoiceHeaderID = DN.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					CAST(CAST(SUM(ISNULL(NetQuantity, 0)) AS NUMERIC(18,2)) AS NVARCHAR(100)) AS TotalNetQty
				FROM (SELECT DISTINCT InvoiceHeaderID, NetQuantity,InvoiceId FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) TQ ON IH.InvoiceHeaderID = TQ.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					CAST(CAST(SUM(ISNULL(GrossQuantity, 0)) AS NUMERIC(18,2)) AS NVARCHAR(100)) AS TotalGrossQty
				FROM (SELECT DISTINCT InvoiceHeaderID, GrossQuantity,InvoiceId FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) TGQ ON IH.InvoiceHeaderID = TGQ.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					CASE WHEN MIN(HasAttachments) < 1 THEN 'No' ELSE 'Yes' END AS HasAttachments
				FROM (SELECT DISTINCT InvoiceHeaderID, HasAttachments FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) HA ON IH.InvoiceHeaderID = HA.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(PrePostValues, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,PrePostValues),'--')  AS PrePostValues
				FROM (SELECT DISTINCT InvoiceHeaderID, PrePostValues FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) PV ON IH.InvoiceHeaderID = PV.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(InvoiceDate, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,InvoiceDate),'--')  AS InvoiceDate
				FROM (SELECT DISTINCT InvoiceHeaderID, InvoiceDate FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) INVDT ON IH.InvoiceHeaderID = INVDT.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(CreationMethod, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,CreationMethod),'--')  AS CreationMethod
				FROM (SELECT DISTINCT InvoiceHeaderID, CreationMethod FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) CM ON IH.InvoiceHeaderID = CM.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(PaymentDueDate, ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,PaymentDueDate),'--')  AS PaymentDueDate
				FROM (SELECT DISTINCT InvoiceHeaderID, PaymentDueDate FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) PAYDT ON IH.InvoiceHeaderID = PAYDT.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG([StatusName], ';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID,[StatusName]),'--')  AS [StatusName]
				FROM (SELECT DISTINCT InvoiceHeaderID, [StatusName] FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) IHSN ON IH.InvoiceHeaderID = IHSN.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					Avg(TimeToInvoice) AS TimeToInvoice
				FROM (SELECT DISTINCT InvoiceHeaderID, TimeToInvoice FROM #TempBOLDetails) TBOL 
				GROUP BY TBOL.InvoiceHeaderID			   
					) TTI ON IH.InvoiceHeaderID = TTI.InvoiceHeaderID;
  
  --#SupplierInvoices
	SELECT --INV.Id,
	DISTINCT        
		(BolDtls.InvoiceId) AS Id,        
		-- ISNULL(INV.OrderId, 0) AS OrderId,                  
		ISNULL(BolDtls.OrderId, '0') AS OrderId,        
		CASE        
		  WHEN INV.OrderId IS NULL THEN '--'        
		  ELSE (        
			CASE        
			  WHEN I_USR.CompanyId NOT IN (SELECT        
				  CompanyId        
				FROM @TblGroupCompanyIds) THEN I_COM.[Name]        
			  WHEN FRQ.FuelRequestTypeId = 2 THEN C_COM.[Name]        
			  ELSE F_COM.[Name]        
			END        
			)        
		END AS Supplier,        
		-- CASE WHEN INV.OrderId IS NULL THEN '--' WHEN INV.InvoiceTypeId = 5 THEN 'Dry Run Fee' WHEN INV.InvoiceTypeId = 9 THEN 'Balance Invoice' WHEN INV.InvoiceTypeId = 10 THEN 'Tank Rental Invoice' ELSE ISNULL(TFXP.NAME,PRD.Name) END AS FuelType,         
		BolDtls.DisplayInvoiceNumber AS InvoiceNumber,        
		BolDtls.FuelType AS FuelType,        
		-- CASE WHEN INV.OrderId IS NULL THEN '--' ELSE INV.PoNumber END AS PoNumber,                
		BolDtls.PoNumber AS PoNumber,        
		--  CASE WHEN IFD.PickupLocation = 2 THEN ISNULL(IFD.LiftTicketNumber,'--') ELSE ISNULL(IFD.BolNumber, '--') END AS BolNumber,                        
		BolDtls.BOLNo AS BolNumber,  
		BolDtls.BadgeNumber AS BadgeNumber,  
		BolDtls.Carrier AS Carrier,  
		BolDtls.LiftDate AS LiftDate,  
		CASE        
		  WHEN INV.OrderId IS NULL THEN 0        
		  ELSE (        
			IH.TotalBasicAmount - IH.TotalDiscountAmount +        
			(CASE        
			  WHEN INV.InvoiceTypeId = 5 THEN 0        
			  ELSE ISNULL(IH.TotalFeeAmount, 0)        
			END) +        
			(CASE        
			  WHEN INV.InvoiceTypeId = 6 AND        
				INV.InvoiceTypeId = 7 THEN 0        
			  ELSE IH.TotalTaxAmount        
			END)        
			)        
		END AS InvoiceAmount,        
		--CASE WHEN INV.InvoiceTypeId = 9 THEN '-' ELSE CONVERT(NVARCHAR(10), INV.DropEndDate, 101) END AS DropDate,                 
		BolDtls.DropDate AS DropDate,        
		--CASE WHEN INV.InvoiceTypeId = 9 THEN '-' ELSE FORMAT(INV.DropStartDate,'h:mm tt') + ' - ' + FORMAT(INV.DropEndDate,'h:mm tt') END AS DropTime,                 
		BolDtls.DropTime AS DropTime,        
		BolDtls.InvoiceDate AS InvoiceDate,        
		BolDtls.PaymentDueDate AS PaymentDueDate,        
		BolDtls.[StatusName] AS [Status],        
		IH.InvoiceNumberId,        
		--ISNULL(MET.[Name],'--') AS TerminalName,           
		BolDtls.TerminalName AS TerminalName,        
		BolDtls.DriverName,        
		CASE        
		  --WHEN INV.OrderId IS NULL THEN '--'        
		  --ELSE CASE        
		  --    WHEN JBS.LocationType = 3 THEN MST.Code        
		  --    ELSE JBS.City + ', ' + MST.Code + ' ' + JBS.ZipCode        
		  --  END END      
		WHEN JBS.Id IS NULL THEN '--' ELSE JBS.Name END AS [Location],        
		ISNULL(BolDtls.PricePerGallon, '-') AS PricePerGallon,        
		--   INV.DropEndDate,                        
		-- INV.PaymentDueDate AS sPaymentDueDate,            
		--CASE WHEN IXS.StatusId = 10 THEN NULL ELSE INV.CreatedDate END AS CreatedDate,                        
		--   INV.DroppedGallons,                
		BolDtls.DroppedGallons, 
		(CASE WHEN BolDtls.ConvertedQuantity IS NULL THEN NULL Else BolDtls.ConvertedQuantity + ' UOM' END) AS ConvertedQuantity,		
		CASE WHEN JBS.IsMarine IS NULL THEN CAST(0 AS BIT) ELSE CAST(JBS.IsMarine AS BIT) END AS IsMarineLocation,   
		--CASE WHEN  FRQ.UoM IS NULL THEN  0 ELSE FRQ.UoM END AS UoM,  
		--(CASE WHEN FRQ.UoM = 2 THEN 'Litres' WHEN FRQ.UoM = 3 THEN 'Barrel' WHEN FRQ.UoM = 4 THEN 'MT' ELSE 'Gallons' END) AS UnitOfMeasurement,
		INV.WaitingFor,        
		BolDtls.CreationMethod AS CreationMethod,        
		--ISNULL(IAD.DropTicketNumber, '--') AS DropTicketNumber,        
		BolDtls.DropTicketNumber AS DropTicketNumber,      
		--ISNULL(IFD.LiftTicketNumber,'--') AS LiftTicketNumber,                        
		--CASE WHEN IFD.PickupLocation = 2 THEN  CASE WHEN IFD.SiteName IS NULL THEN 'Bulk Plant: ' ELSE IFD.SiteName + ': ' END + ISNULL(IFD.Address + ', ' + IFD.City+ ', ' + MST.Code + ' ' + IFD.ZipCode,'--') ELSE ISNULL(IFD.Address + ', ' + IFD.City+ ', ' 
		--   +        
		--MST.Code + ' ' + IFD.ZipCode,'--') END AS [PickupAddress]                        
		BolDtls.PickupAddress AS [PickupAddress],   
		BolDtls.PrePostValues AS [PrePostValues],   
		IH.Id As [InvoiceHeaderId],  
		ISNULL(FORMAT(IAD.DeliverySentToPDIOn,'MM-dd-yyyy hh:mm tt'),'--') As PDIDetailsDate,  
		ISNULL(IAD.PDIDeliveryOrderNo, '--') AS PDIOrderId,
		--ISNULL(INV.QbInvoiceNumber,'--') AS QbInvoiceNumber  
		BolDtls.QbInvoiceNumber AS QbInvoiceNumber,
		BolDtls.TotalNetQty AS TotalNetQuantity,
		BolDtls.TotalGrossQty AS TotalGrossQuantity,
		BolDtls.HasAttachments,  
		--Added below Case-When for PDI exceptions  as a fix for Impediment 31804 -- yash
		CASE WHEN ((IAD.ExceptionMessage IS NOT NULL AND IAD.ExceptionMessage <> '') 
		           AND (PDIDeliveryOrderNo IS NULL OR PDIDeliveryOrderNo =''))  THEN CONCAT('Exception:',IAD.ExceptionMessage) 
		     ELSE '--' END AS ExternalPDIException,
	    ISNULL(LR.DisplayRequestID, '--') as SourcingRequestId,
		(CASE WHEN AST.IsMarine = 1 THEN AST.[Name] ELSE '--' END) As [VesselName],
		(CASE WHEN JBS.IsMarine = 1 THEN BDR.[BDRNumber] ELSE '--' END) As [BDRNumber],
		 COALESCE(STUFF((Select ',' + DST.DeliveryLevelPO From DeliveryScheduleXTrackableSchedules DST
						INNER JOIN Invoices IV
							On IV.TrackableScheduleId=DST.ID and IV.InvoiceHeaderId=IH.Id Where IV.TrackableScheduleId IS NOT NULL
								AND DST.DeliveryLevelPO <>''
							FOR XML PATH(''), TYPE).value('text()[1]','nvarchar(500)')
						   , 1, LEN(','), ''),'--'
						   ) AS DeliveryLevelPO,
	CASE	WHEN BolDtls.TimeToInvoice < 0 THEN CONCAT (0 , ' Min')
	        WHEN BolDtls.TimeToInvoice < 60 THEN CONCAT(BolDtls.TimeToInvoice, ' Min')
			WHEN  BolDtls.TimeToInvoice <= 1440 THEN CONCAT(CONCAT(BolDtls.TimeToInvoice/60, 'Hr '), CONCAT(BolDtls.TimeToInvoice%60, ' Min'))
			ELSE CONCAT(CONCAT(BolDtls.TimeToInvoice/1440, ' Day '), CONCAT((BolDtls.TimeToInvoice%1440)/24, ' Hr'))
			END
			AS TimeToInvoice
  INTO #SupplierInvoices
  FROM #TblBOLDetails BolDtls
		INNER JOIN dbo.InvoiceHeaderDetails(Nolock) IH ON IH.Id = BolDtls.InvoiceHeaderId AND IH.IsActive = 1
		INNER JOIN dbo.Invoices(nolock) INV ON IH.Id = INV.InvoiceHeaderId AND INV.IsActive = 1
		INNER JOIN vw_FilterUnAssignInvoicesForApiException(Nolock) EDDT ON IH.Id = EDDT.InvoiceHeaderId
		INNER JOIN dbo.InvoiceXInvoiceStatusDetails(Nolock) IXS ON INV.Id = IXS.InvoiceId AND IXS.IsActive = 1
		INNER JOIN dbo.MstInvoiceStatuses(Nolock) IST ON IXS.StatusId = IST.Id
		INNER JOIN dbo.Users(Nolock) I_USR ON INV.CreatedBy = I_USR.Id
		INNER JOIN dbo.Companies(Nolock) I_COM ON I_USR.CompanyId = I_COM.Id        
		LEFT JOIN dbo.Orders(Nolock) ORD ON INV.OrderId = ORD.Id        
		LEFT JOIN dbo.FuelRequests(Nolock) FRQ ON ORD.FuelRequestId = FRQ.Id        
		LEFT JOIN dbo.Jobs(Nolock) JBS ON FRQ.JobId = JBS.Id        
		LEFT JOIN dbo.MstStates(Nolock) MST ON JBS.StateId = MST.Id        
		--LEFT JOIN dbo.MstProducts PRD ON FRQ.FuelTypeId = PRD.Id                        
		--LEFT JOIN dbo.MstTfxProducts TFXP  ON TFXP.Id = PRD.TfxProductId                        
		--LEFT JOIN InvoiceXBolDetails INVBOL ON INV.Id= INVBOL.InvoiceId                        
		--LEFT JOIN InvoiceFtlDetails IFD ON IFD.Id = INVBOL.BolDetailId                        
		--LEFT JOIN MstStates MSTDL ON MSTDL.Id = IFD.StateId                        
		--LEFT JOIN dbo.MstExternalTerminals MET ON IFD.TerminalId = MET.Id
		LEFT JOIN dbo.Users(Nolock) D_USR ON INV.DriverId = D_USR.Id        
		LEFT JOIN dbo.Users(Nolock) F_USR ON FRQ.CreatedBy = F_USR.Id        
		LEFT JOIN dbo.Companies(Nolock) F_COM ON F_USR.CompanyId = F_COM.Id        
		LEFT JOIN dbo.CounterOffers(Nolock) COF ON FRQ.Id = COF.FuelRequestId AND COF.BuyerStatus = 2        
		LEFT JOIN dbo.Users(Nolock) C_USR ON COF.BuyerId = C_USR.Id        
		LEFT JOIN dbo.Companies(Nolock) C_COM ON C_USR.CompanyId = C_COM.Id        
		LEFT JOIN InvoiceXAdditionalDetails(Nolock) IAD ON INV.Id = IAD.InvoiceId
		LEFT JOIN dbo.LeadRequests(Nolock) LR ON ORD.LeadRequestId = LR.Id
	    LEFT JOIN JobXAssets(Nolock) JXA ON  JXA.OrderId = ORD.Id AND JXA.RemovedBy IS NULL 
		LEFT JOIN Assets(Nolock) AST ON JXA.AssetId = AST.Id AND AST.IsActive = 1
		LEFT JOIN BDRDetails(NOLOCK) BDR ON BDR.InvoiceId = INV.Id AND INV.isActive = 1 AND BDR.IsActive = 1
		LEFT JOIN DeliveryScheduleXTrackableSchedules(NOLOCK) DST ON DST.Id = INV.TrackableScheduleId  AND  INV.isActive = 1
        
  WHERE (INV.InvoiceVersionStatusId = 1 OR IXS.StatusId = 11)    
  AND INV.WaitingFor != 9 -- Next marine drop    
  AND (          
		  (INV.OrderId IS NULL AND I_USR.CompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds))          
		  OR (INV.OrderId IS NOT NULL AND ORD.AcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds) )          
		  OR ((FRQ.FuelRequestTypeId = 3 OR [dbo].[usf_GetParentFuelRequestTypeId](FRQ.Id) = 3)          
				AND ORD.BuyerCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds) )        
		  OR (
				(FRQ.FuelRequestTypeId = 7  OR 
					[dbo].[usf_GetParentFuelRequestTypeId](FRQ.Id) = 7) AND ORD.BuyerCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)          
		  )          
	)          
  AND (          
		  (@InvoiceTypeId IN (6, 7) AND INV.InvoiceTypeId IN (6, 7))          
		  OR (@InvoiceTypeId NOT IN (6, 7)  AND INV.InvoiceTypeId NOT IN (6, 7))          
  )          
  AND (          
		( INV.OrderId = @OrderId OR @OrderId = 0 ) AND ( INV.CreatedDate >= @StartDate AND INV.CreatedDate < @EndDate )          
  )          
  AND (          
  CASE          
    WHEN @InvoiceFilter = 0 THEN 1 --Invoice Filters                          
    WHEN @InvoiceFilter = 2 AND IXS.StatusId = 2 THEN 1          
    WHEN @InvoiceFilter = 3 AND  IXS.StatusId = 3 THEN 1          
    WHEN @InvoiceFilter = 4 AND   IXS.StatusId = 4 AND          
      ((SELECT COUNT(IXIS.StatusId) FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id) = 1
		OR          
		2 = ANY (SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id)          
      ) THEN 1          
    WHEN @InvoiceFilter = 6 AND IXS.StatusId = 6 THEN 1          
    WHEN @InvoiceFilter = 10 AND IXS.StatusId = 8 THEN 1          
    WHEN @InvoiceFilter IN (13, 14) AND          
		  (          
		  4 = ANY (SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS          
					WHERE IXIS.InvoiceId = INV.Id AND IXIS.IsActive = 1) AND          
		  2 != ANY (SELECT IXIS.StatusId FROM InvoiceXInvoiceStatusDetails IXIS WHERE IXIS.InvoiceId = INV.Id)          
      ) THEN 1          
    WHEN @InvoiceFilter = 15 THEN 1          
    WHEN @InvoiceFilter = 16 AND INV.OrderId IS NOT NULL AND IXS.StatusId != 10 AND          
		ORD.AcceptedCompanyId NOT IN (SELECT CompanyId  FROM @TblGroupCompanyIds) THEN 1          
    WHEN @InvoiceFilter = 17 AND  (INV.OrderId IS NULL OR ORD.AcceptedCompanyId IN (SELECT CompanyId  FROM @TblGroupCompanyIds)          
      ) THEN 1          
    WHEN @InvoiceFilter = 19 AND INV.WaitingFor = 1 THEN 1          
    ELSE 0          
	END          
  ) = 1          
  AND ( @ViewInvoices NOT IN (2, 3)  
		OR (@ViewInvoices = 2 AND INV.OrderId IS NOT NULL AND ORD.AcceptedCompanyId NOT IN (SELECT CompanyId FROM @TblGroupCompanyIds))          
		OR (@ViewInvoices = 3 AND (INV.OrderId IS NULL OR ORD.AcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)))          
	  )          
  AND ((@CountryId = 0 OR @CurrencyType = 0)  OR 
	   (INV.Currency = @CurrencyType AND (JBS.CountryId IS NULL OR JBS.CountryId = @CountryId)))
  AND (@CustomerIds = '' OR (ORD.BuyerCompanyId IN (Select Id from @TblCustomerCompanyIds) AND JBS.IsMarine = @IsMarine))
  AND (@LocationIds = '' OR (JBS.Id IN (Select Id from @TblLocationIds) AND JBS.IsMarine = @IsMarine))
  AND (@VesselIds = '' OR 0 < (SELECT Count(*) FROM AssetDrops AD INNER JOIN JobXAssets AJXA 
								ON AJXA.Id = AD.JobXAssetId 
									WHERE AJXA.AssetId IN (select Id from @TblVesselIds) AND AD.InvoiceId = INV.Id)
		);
  
	--#SupplierInvoicesFinal
	SELECT DISTINCT ID, OrderID 
		,Supplier,
		InvoiceNumber, FuelType,PoNumber, BolNumber, BadgeNumber, Carrier, LiftDate, InvoiceAmount, DropDate, DropTime, InvoiceDate, PaymentDueDate
		, [Status]
		,InvoiceNumberId
		,TerminalName, DriverName
		,JL.[Location]
		,PricePerGallon,DroppedGallons, ConvertedQuantity
		,IsMarineLocation
		,WF.WaitingFor
		,CreationMethod,DropTicketNumber, PickupAddress, PrePostValues
		,SI.[InvoiceHeaderId]
		,PDD.PDIDetailsDate
		,POI.PDIOrderId
		,QbInvoiceNumber,TotalNetQuantity,TotalGrossQuantity,HasAttachments
		,ExternalPDIException
		,SourcingRequestId
		,VN.[VesselName]
		,BDRN.[BDRNumber]
		,DLPO.DeliveryLevelPO
		,TimeToInvoice
	INTO #SupplierInvoicesFinal
	FROM #SupplierInvoices SI
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					Max(TBOL.WaitingFor)  AS WaitingFor
				FROM (SELECT DISTINCT InvoiceHeaderID, WaitingFor FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) WF ON SI.InvoiceHeaderID = WF.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.PDIOrderId,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.PDIOrderId),'--')  AS PDIOrderId
				FROM (SELECT DISTINCT InvoiceHeaderID, PDIOrderId FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) POI ON SI.InvoiceHeaderID = POI.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.PDIDetailsDate,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.PDIDetailsDate),'--')  AS PDIDetailsDate
				FROM (SELECT DISTINCT InvoiceHeaderID, PDIDetailsDate FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) PDD ON SI.InvoiceHeaderID = PDD.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.Location,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.Location),'--')  AS Location
				FROM (SELECT DISTINCT InvoiceHeaderID, Location FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) JL ON SI.InvoiceHeaderID = JL.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.[VesselName],';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.[VesselName]),'--')  AS [VesselName]
				FROM (SELECT DISTINCT InvoiceHeaderID, [VesselName] FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) VN ON SI.InvoiceHeaderID = VN.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.[BDRNumber],';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.[BDRNumber]),'--')  AS [BDRNumber]
				FROM (SELECT DISTINCT InvoiceHeaderID, [BDRNumber] FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) BDRN ON SI.InvoiceHeaderID = BDRN.InvoiceHeaderID
	LEFT JOIN (
				SELECT TBOL.InvoiceHeaderID, 
					ISNULL(STRING_AGG(TBOL.DeliveryLevelPO,';') WITHIN GROUP (ORDER BY TBOL.InvoiceHeaderID, TBOL.DeliveryLevelPO),'--')  AS DeliveryLevelPO
				FROM (SELECT DISTINCT InvoiceHeaderID, DeliveryLevelPO FROM #SupplierInvoices ) TBOL GROUP BY TBOL.InvoiceHeaderID			   
					) DLPO ON SI.InvoiceHeaderID = DLPO.InvoiceHeaderID;
					
	--SELECT InvoiceHeaderId, COUNT(*) FROM #SupplierInvoices GROUP BY InvoiceHeaderId HAVING COUNT(*) > 1;
	--SELECT InvoiceHeaderId, COUNT(*) FROM #SupplierInvoicesFinal GROUP BY InvoiceHeaderId HAVING COUNT(*) > 1;
	
  SELECT *, [TotalCount] = COUNT(Id) OVER ()
  FROM #SupplierInvoicesFinal          
  WHERE (          
			@InvoiceNumberSearchTypesValid = 0 OR 
			( @InvoiceNumberSearchTypesValid > 0  
				AND InvoiceNumber IN (SELECT  InvoiceNumber FROM @InvoiceNumberSearchTypes
										WHERE InvoiceNumber LIKE '%' + SearchVar + '%'))
		)          
	  AND (
		@PoNumberSearchTypesValid = 0 OR ( @PoNumberSearchTypesValid > 0 
											AND PoNumber IN (SELECT PoNumber FROM @PoNumberSearchTypes
											WHERE PoNumber LIKE '%' + SearchVar + '%'))
	  )  
	  AND (
		@SourcingRequestSearchTypesValid = 0 OR ( @SourcingRequestSearchTypesValid > 0 
											AND SourcingRequestId IN (SELECT SourcingRequestId FROM @SourcingRequestSearchTypes
											WHERE SourcingRequestId LIKE '%' + SearchVar + '%'))
	  )  
	  AND (          
		@BolNumberSearchTypesValid = 0 OR ( @BolNumberSearchTypesValid > 0 AND BolNumber IN (SELECT CAST(BolNumber AS varchar(1000))
																							FROM @BolNumberSearchTypes WHERE BolNumber LIKE '%' + SearchVar + '%'))
		)    
	  AND(    
		@CarrierSearchTypesValid = 0 OR ( @CarrierSearchTypesValid > 0 AND Carrier IN (SELECT CAST(Carrier AS varchar(1000))
																						FROM @CarrierSearchTypes WHERE Carrier LIKE '%' + SearchVar + '%'))
		)    
	  AND (    
		@LiftDateSearchTypesValid = 0 OR ( @LiftDateSearchTypesValid > 0 
											AND LiftDate IN (SELECT LiftDate FROM @LiftDateSearchTypes WHERE LiftDate LIKE '%' + SearchVar + '%'))
		)    
	  AND (          
		@BadgeNumberSearchTypesValid = 0 OR ( @BadgeNumberSearchTypesValid > 0
												AND BadgeNumber IN ( SELECT CAST(BadgeNumber AS varchar(1000)) FROM @BadgeNumberSearchTypes WHERE BadgeNumber LIKE '%' + SearchVar + '%'))
		)       
	  AND (          
			@SupplierSearchTypesValid = 0          
		  OR (          
		  @SupplierSearchTypesValid > 0          
		  AND Supplier IN (SELECT Supplier          
						  FROM @SupplierSearchTypes          
						  WHERE Supplier LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
		  @LocationSearchTypesValid = 0          
		  OR (          
		  @LocationSearchTypesValid > 0          
		  AND [Location] IN (SELECT [Location]          
							  FROM @LocationSearchTypes          
							  WHERE REPLACE([Location], ',', '') LIKE '%' + SearchVar + '%')          
	  ))      
	  AND(    
		 @QBInvoiceNumberSearchTypesValid = 0     
		 OR (     
		 @QBInvoiceNumberSearchTypesValid > 0     
		 AND      QbInvoiceNumber IN ( SELECT QbInvoiceNumber     
									 FROM   @QBInvoiceNumberSearchTypes     
									 WHERE  QbInvoiceNumber IN (SearchVar))))    
	  AND (          
		  @FuelTypeSearchTypesValid = 0          
		  OR (          
		  @FuelTypeSearchTypesValid > 0          
		  AND FuelType IN (SELECT FuelType          
						FROM @FuelTypeSearchTypes          
						WHERE FuelType LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
		  @TerminalSearchTypesValid = 0          
		  OR (          
		  @TerminalSearchTypesValid > 0          
		  AND TerminalName IN (SELECT TerminalName          
							  FROM @TerminalSearchTypes          
							  WHERE TerminalName LIKE '%' + SearchVar + '%')          
	  ))          
		--AND      (                           
		-- @LiftTicketNumberSearchTypesValid = 0                           
		-- OR (                           
		--     @LiftTicketNumberSearchTypesValid > 0                           
		--		AND LiftTicketNumber IN                           
		--            (                 
		--              SELECT LiftTicketNumber                           
		--              FROM   @LiftTicketNumberSearchTypes                           
		--              WHERE  LiftTicketNumber like '%' +SearchVar+ '%')))                          
	  AND (          
		  @PickupAddressSearchTypesValid = 0          
		  OR (          
		  @PickupAddressSearchTypesValid > 0          
		  AND [PickupAddress] IN (SELECT  [PickupAddress]          
								  FROM @PickupAddressSearchTypes          
								  WHERE [PickupAddress] LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
		  @DropTicketNumberSearchTypesValid = 0          
		  OR (          
		  @DropTicketNumberSearchTypesValid > 0          
		  AND DropTicketNumber IN (SELECT DropTicketNumber          
								  FROM @DropTicketNumberSearchTypes          
								  WHERE DropTicketNumber LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
			@CreationMethodSearchTypesValid = 0          
			OR (          
			@CreationMethodSearchTypesValid > 0          
			AND CreationMethod IN (SELECT CreationMethod          
									FROM @CreationMethodSearchTypes          
									WHERE CreationMethod LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
			@DropDateSearchTypesValid = 0          
			OR (          
			@DropDateSearchTypesValid > 0          
			AND DropDate IN (SELECT DropDate          
							FROM @DropDateSearchTypes          
							WHERE DropDate LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
			  @PriceSearchTypesValid = 0          
			  OR (          
			  @PriceSearchTypesValid > 0          
			  AND PricePerGallon IN (SELECT CAST(PricePerGallon AS varchar(1000))          
									  FROM @PriceSearchTypes          
									  WHERE PricePerGallon LIKE '%' + REPLACE(SearchVar, '$', '') + '%')          
	  ))          
	  AND (          
		  @AmountSearchTypesValid = 0          
		  OR (          
		  @AmountSearchTypesValid > 0          
		  AND InvoiceAmount IN (SELECT InvoiceAmount          
								  FROM @AmountSearchTypes          
								  WHERE InvoiceAmount LIKE '%' + REPLACE(SearchVar, '$', '') + '%')          
	  ))          
	  AND (          
		  @DropTimeSearchTypesValid = 0          
		  OR (          
		  @DropTimeSearchTypesValid > 0          
		  AND DropTime IN (SELECT  DropTime          
						  FROM @DropTimeSearchTypes          
						  WHERE DropTime LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
		  @StatusSearchTypesValid = 0          
		  OR (          
		  @StatusSearchTypesValid > 0          
		  AND [Status] IN (SELECT [Status]          
						  FROM @StatusSearchTypes          
						  WHERE [Status] LIKE '%' + SearchVar + '%')          
	  ))          
	  AND (          
		  @InvoiceDateSearchTypesValid = 0          
		  OR (          
			  @InvoiceDateSearchTypesValid > 0          
			  AND InvoiceDate IN (SELECT InvoiceDate          
								  FROM @InvoiceDateSearchTypes          
								  WHERE InvoiceDate LIKE '%' + SearchVar + '%')          
		  ))          
	  AND (          
		  @PaymentDueDateSearchTypesValid = 0          
		  OR (          
		  @PaymentDueDateSearchTypesValid > 0          
		  AND PaymentDueDate IN (SELECT PaymentDueDate          
								  FROM @PaymentDueDateSearchTypes          
								  WHERE PaymentDueDate LIKE '%' + SearchVar + '%')          
		  ))          
	  AND (          
		  @DriverSearchTypesValid = 0          
		OR (          
			  @DriverSearchTypesValid > 0          
			  AND DriverName IN (SELECT DriverName
								FROM @DriverSearchTypes
								WHERE DriverName LIKE '%' + SearchVar + '%')          
		  ))      
		AND (          
		 @PDIOrderIdSearchTypesValid = 0        
		  OR (        
			@PDIOrderIdSearchTypesValid > 0        
			AND PDIOrderId IN (
				SELECT PDIOrderId        
				FROM @PDIOrderIdSearchTypes
				WHERE ((PDIOrderId LIKE '%' + SearchVar + '%') OR (ExternalPDIException LIKE '%' + SearchVar + '%' )))        
		  ))
	  AND (          
		 @ExternalPDIExceptionSearchTypesValid = 0        
		  OR (        
			@ExternalPDIExceptionSearchTypesValid > 0        
			AND ExternalPDIException IN (
				SELECT ExternalPDIException        
				FROM @ExternalPDIExceptionSearchTypes
				WHERE ExternalPDIException LIKE '%' + SearchVar + '%')   
			--TotalNetQuantity = '1500.00'
		  ))
	  AND (          
	  @DroppedGallonsSearchTypesValid = 0          
	  OR (          
		  @DroppedGallonsSearchTypesValid > 0          
		  AND DroppedGallons IN ( SELECT CAST(DroppedGallons AS varchar(1000))          
								  FROM @DroppedGallonsSearchTypes          
								  WHERE DroppedGallons LIKE '%' + SearchVar + '%')          
	  ))     
    
	  AND (          
			  @TotalNetQuantitySearchTypesValid = 0          
			  OR (
			  --
			  @TotalNetQuantitySearchTypesValid > 0          
						AND TotalNetQuantity IN (	SELECT CAST(TotalNetQuantity AS varchar(1000))          
													FROM @TotalNetQuantitySearchTypes          
													WHERE TotalNetQuantity LIKE '%' + SearchVar + '%')          
	  ))  
  
	  AND (          
	  @TotalGrossQuantitySearchTypesValid = 0 
		OR (  @TotalGrossQuantitySearchTypesValid > 0 
			AND TotalGrossQuantity IN (	SELECT CAST(TotalGrossQuantity AS varchar(1000))
										FROM @TotalGrossQuantitySearchTypes 
										WHERE TotalGrossQuantity LIKE '%' + SearchVar + '%')          
		))  
  
	  AND (          
			@HasAttachmentsSearchTypesValid = 0          
			OR (          
				@HasAttachmentsSearchTypesValid > 0          
				AND HasAttachments IN (	SELECT CAST(HasAttachments AS varchar(1000))          
										FROM @HasAttachmentsSearchTypes          
										WHERE HasAttachments LIKE '%' + SearchVar + '%')          
	  ))  
	  
	  AND (          
			@VesselNameSearchTypesValid = 0          
		  OR (          
		  @VesselNameSearchTypesValid > 0          
		  AND [VesselName] IN (SELECT [VesselName]          
						  FROM @VesselNameSearchTypes          
						  WHERE [VesselName] LIKE '%' + SearchVar + '%')          
	  ))  
	  AND (
		  @BDRNumberSearchTypesValid = 0 
		  OR (
		  @BDRNumberSearchTypesValid > 0 
		  AND BDRNumber IN (SELECT BDRNumber
							FROM @BDRNumberSearchTypes
							WHERE BDRNumber LIKE '%' + SearchVar + '%')
		  ))
	   AND (
		  @DeliveryLevelPOSearchTypesValid = 0 
		  OR (
		  @DeliveryLevelPOSearchTypesValid > 0 
		  AND DeliveryLevelPO IN (SELECT DeliveryLevelPO
							FROM @DeliveryLevelPOSearchTypes
							WHERE DeliveryLevelPO LIKE '%' + SearchVar + '%')
		  ))
		AND
		(            
			@TimeToInvoiceSearchTypesValid = 0 OR   
			( @TimeToInvoiceSearchTypesValid > 0    
			AND TimeToInvoice IN (SELECT  TimeToInvoice FROM @TimeToInvoiceSearchTypes  
					WHERE TimeToInvoice LIKE '%' + SearchVar + '%'))  
		)
	  AND ( @GlobalSearchText IS NULL          
			OR ( (InvoiceNumber LIKE '%' + @GlobalSearchText + '%')          
			  OR ( PoNumber LIKE '%' + @GlobalSearchText + '%')  
			  OR ( SourcingRequestId LIKE '%' + @GlobalSearchText + '%')
			  OR ( QbInvoiceNumber LIKE '%' + @GlobalSearchText+ '%')     
			  OR ( BolNumber LIKE '%' + @GlobalSearchText + '%')         
			  OR ( BadgeNumber LIKE '%' + @GlobalSearchText + '%')      
			  OR ( Carrier LIKE '%' + @GlobalSearchText + '%')    
			  OR( LiftDate LIKE '%' + @GlobalSearchText + '%')    
			  OR ( REPLACE([Location], ',', '') LIKE '%' + @GlobalSearchText + '%')          
			  OR ( Supplier LIKE '%' + @GlobalSearchText + '%')          
			  OR ( FuelType LIKE '%' + @GlobalSearchText + '%')          
			  OR ( DroppedGallons LIKE '%' + @GlobalSearchText + '%')          
			  OR ( TerminalName LIKE '%' + @GlobalSearchText + '%')          
			  OR ( DropTicketNumber LIKE '%' + @GlobalSearchText + '%')          
			  OR ( CreationMethod LIKE '%' + @GlobalSearchText + '%')          
			  --OR ( LiftTicketNumber LIKE '%' + @GlobalSearchText+ '%')                          
			  OR ( DropDate LIKE '%' + @GlobalSearchText + '%')          
			  OR ( PricePerGallon LIKE '%' + @GlobalSearchText + '%')          
			  OR ( InvoiceAmount LIKE '%' + @GlobalSearchText + '%')          
			  OR ( DropTime LIKE '%' + @GlobalSearchText + '%')          
			  OR ( [Status] LIKE '%' + @GlobalSearchText + '%')          
			  OR ( InvoiceDate LIKE '%' + @GlobalSearchText + '%')          
			  OR ( PaymentDueDate LIKE '%' + @GlobalSearchText + '%')          
			  OR ( PickupAddress LIKE '%' + @GlobalSearchText + '%')          
			  OR ( PDIOrderId LIKE '%' + @GlobalSearchText + '%') 
			  OR ( ExternalPDIException LIKE '%' + @GlobalSearchText + '%') 
			  OR ( TotalNetQuantity LIKE '%' + @GlobalSearchText + '%')   
			  OR ( TotalGrossQuantity LIKE '%' + @GlobalSearchText + '%')   
			  OR ( HasAttachments LIKE '%' + @GlobalSearchText + '%')
			  OR ( [VesselName] LIKE '%' + @GlobalSearchText + '%') 
			  OR ( DriverName LIKE '%' + @GlobalSearchText + '%')  
			  OR ( BDRNumber LIKE '%' + @GlobalSearchText + '%')
			  OR ( DeliveryLevelPO LIKE '%' + @GlobalSearchText + '%')  
			  OR (TimeToInvoice LIKE '%' + @GlobalSearchText + '%')
			)
	)            
	  ORDER BY CASE          
		WHEN @SortId = 15 AND          
		  @SortDirection = 'asc' THEN InvoiceAmount       
	  END ASC,          
     
	  CASE                
		WHEN @SortId = 0 AND @SortDirection = 'asc' THEN InvoiceNumber          
		WHEN @SortId = 1 AND @SortDirection = 'asc' THEN PoNumber 
		WHEN @SortId = 2 AND @SortDirection = 'asc' THEN SourcingRequestId
		WHEN @SortId = 3 AND @SortDirection = 'asc' THEN QbInvoiceNumber
		WHEN @SortId = 4 AND @SortDirection = 'asc' THEN BDRNumber
		WHEN @SortId = 5 AND @SortDirection = 'asc' THEN BolNumber    
		WHEN @SortId = 6 AND @SortDirection = 'asc' THEN DeliveryLevelPO    
		WHEN @SortId = 7 AND @SortDirection = 'asc' THEN LiftDate    
		WHEN @SortId = 8 AND  @SortDirection = 'asc' THEN Carrier    
		WHEN @SortId = 9 AND @SortDirection = 'asc' THEN BadgeNumber        
		WHEN @SortId = 10 AND @SortDirection = 'asc' THEN Supplier          
		WHEN @SortId = 11 AND @SortDirection = 'asc' THEN [Location]          
		WHEN @SortId = 12 AND @SortDirection = 'asc' THEN FuelType          
		WHEN @SortId = 13 AND @SortDirection = 'asc' THEN DroppedGallons    
		WHEN @SortId = 14 AND @SortDirection = 'asc' THEN PrePostValues          
		WHEN @SortId = 15 AND @SortDirection = 'asc' THEN TerminalName                 
		WHEN @SortId = 17 AND @SortDirection = 'asc' THEN DropDate          
		WHEN @SortId = 18 AND @SortDirection = 'asc' THEN DropTime          
		WHEN @SortId = 19 AND @SortDirection = 'asc' THEN PricePerGallon          
		WHEN @SortId = 20 AND @SortDirection = 'asc' THEN InvoiceDate  --createddate            
		WHEN @SortId = 21 AND @SortDirection = 'asc' THEN PaymentDueDate          
		WHEN @SortId = 22 AND @SortDirection = 'asc' THEN DriverName      
		WHEN @SortId = 23 AND @SortDirection = 'asc' THEN DropTicketNumber         
		WHEN @SortId = 24 AND @SortDirection = 'asc' THEN CreationMethod    
		WHEN @SortId = 25 AND @SortDirection = 'asc' THEN PickupAddress     
		WHEN @SortId = 26 AND @SortDirection = 'asc' THEN PDIOrderId 
		WHEN @SortId = 27 AND @SortDirection = 'asc' THEN ExternalPDIException 
		WHEN @SortId = 28 AND @SortDirection = 'asc' THEN TotalNetQuantity   
		WHEN @SortId = 29 AND @SortDirection = 'asc' THEN TotalGrossQuantity   
		WHEN @SortId = 30 AND @SortDirection = 'asc' THEN HasAttachments
		WHEN @SortId = 31 AND @SortDirection = 'asc' THEN [VesselName]
		WHEN @SortId = 32 AND @SortDirection = 'asc' THEN [Status] 
		WHEN @SortId = 33 AND @SortDirection = 'asc' THEN TimeToInvoice      
	  END ASC,
	  CASE               
		--Whenever a new column is added into sorting, make sure this id is greater than the max sortid given to new column and is changed in C# for DDT/Invoice Grid
		WHEN (@SortId > 33) AND          
			@SortDirection = 'asc' THEN InvoiceHeaderId        
	  END ASC,
	  CASE            
		WHEN @SortId = 16 AND @SortDirection = 'desc' THEN InvoiceAmount        
	  END DESC, 
	  
	  CASE            
		
		WHEN @SortId = 0 AND @SortDirection = 'desc' THEN InvoiceNumber            
		WHEN @SortId = 1 AND @SortDirection = 'desc' THEN PoNumber   
		WHEN @SortId = 2 AND @SortDirection = 'desc' THEN SourcingRequestId    
		WHEN @SortId = 3 AND @SortDirection = 'desc' THEN QbInvoiceNumber
		WHEN @SortId = 4 AND @SortDirection = 'desc' THEN BDRNumber
		WHEN @SortId = 5 AND @SortDirection = 'desc' THEN BolNumber       
		WHEN @SortId = 6 AND @SortDirection = 'desc' THEN DeliveryLevelPO   
		WHEN @SortId = 7 AND @SortDirection = 'desc' THEN LiftDate   
		WHEN @SortId = 8 AND @SortDirection = 'desc' THEN Carrier   
		WHEN @SortId = 9 AND @SortDirection = 'desc' THEN BadgeNumber      
		WHEN @SortId = 10 AND @SortDirection = 'desc' THEN Supplier            
		WHEN @SortId = 11 AND @SortDirection = 'desc' THEN [Location]            
		WHEN @SortId = 12 AND @SortDirection = 'desc' THEN FuelType            
		WHEN @SortId = 13 AND @SortDirection = 'desc' THEN DroppedGallons      
		WHEN @SortId = 14 AND @SortDirection = 'desc' THEN PrePostValues      
		WHEN @SortId = 15 AND @SortDirection = 'desc' THEN TerminalName        
		WHEN @SortId = 17 AND @SortDirection = 'desc' THEN DropDate            
		WHEN @SortId = 18 AND @SortDirection = 'desc' THEN DropTime            
		WHEN @SortId = 19 AND @SortDirection = 'desc' THEN PricePerGallon            
		WHEN @SortId = 20 AND @SortDirection = 'desc' THEN InvoiceDate  --createddate                
		WHEN @SortId = 21 AND @SortDirection = 'desc' THEN PaymentDueDate            
		WHEN @SortId = 22 AND @SortDirection = 'desc' THEN DriverName            
		WHEN @SortId = 23 AND @SortDirection = 'desc' THEN DropTicketNumber          
		WHEN @SortId = 24 AND @SortDirection = 'desc' THEN CreationMethod      
		WHEN @SortId = 25 AND @SortDirection = 'desc' THEN PickupAddress      
		WHEN @SortId = 26 AND @SortDirection = 'desc' THEN PDIOrderId
		WHEN @SortId = 27 AND @SortDirection = 'desc' THEN ExternalPDIException 
		WHEN @SortId = 28 AND @SortDirection = 'desc' THEN TotalNetQuantity   
		WHEN @SortId = 29 AND @SortDirection = 'desc' THEN TotalGrossQuantity   
		WHEN @SortId = 30 AND @SortDirection = 'desc' THEN HasAttachments 
		WHEN @SortId = 31 AND @SortDirection = 'desc' THEN [VesselName]
		WHEN @SortId = 32 AND @SortDirection = 'desc' THEN [Status]  
		WHEN @SortId = 33 AND @SortDirection = 'desc' THEN TimeToInvoice   
	  END DESC,  
	 CASE            
          --Whenever a new column is added into sorting, make sure this id is greater than the max sortid given to new column and is changed in C# for DDT/Invoice Grid
	 WHEN (@SortId > 33) AND  -- 32 - DDT Grid and 33-Invoice Grid - Id column          
		  @SortDirection = 'desc' THEN InvoiceHeaderId        
	  END DESC OFFSET (@PageNumber - 1) * @PageSize ROWS            
	  FETCH NEXT @PageSize ROWS ONLY  
END
GO

--------------------------------------------------------------------------

CREATE OR ALTER PROCEDURE [dbo].[Usp_GetOrdersForJobOfCustomerAndSupplier]
@BuyerCompanyId INT = 0,
@IsEndSupplier BIT = 1,
@SupplierCompanyId INT,
@SkipMarineConversion BIT,
@JobIds SearchIntTypes READONLY,
@FavFuelTypeIds SearchIntTypes READONLY,  
@FavProductTypeIds SearchIntTypes READONLY,
@ProductsToExclude SearchIntTypes READONLY
AS
BEGIN
	DECLARE @IsFavFuelExists BIT = 0, @FavProductTypeExists BIT = 0, @IsProductsToExcludeExists BIT = 0;
	IF EXISTS (SELECT 1 FROM @FavFuelTypeIds)
	BEGIN
		SET @IsFavFuelExists = 1;
	END
	IF EXISTS (SELECT 1 FROM @FavProductTypeIds)
	BEGIN
	SET @FavProductTypeExists = 1;
	END
	IF EXISTS (SELECT 1 FROM @ProductsToExclude)
	BEGIN
		SET @IsProductsToExcludeExists = 1;		
	END
	SELECT
			DISTINCT MappedToProductTypeId AS Id INTO  #ProductsToExclude
			FROM ProductTypeCompatibilityMappings C WITH (NOLOCK)
			INNER JOIN @ProductsToExclude P ON C.ProductTypeId = P.Id
		INSERT INTO #ProductsToExclude 
				SELECT Id FROM @ProductsToExclude
	--#Temp_Orders
	SELECT
		O.Id AS OrderId,	O.AcceptedCompanyId,	PoNumber,	O.[Name] AS OrderName,
		JB.DisplayJobID AS SiteId,
		JobId,	O.BuyerCompanyId,	C.[Name] AS BuyerCompanyName,
		(JB.[Name] + ' - ' + JB.[Address]+', '+JB.City+', '+JB.ZipCode+', '+S.Code) AS JobName,
		FR.FuelTypeId,	FRD.IsDispatchRetainedByCustomer AS IsDispatchRetained,		IsEndSupplier,
		P.ProductTypeId,	PT.[Name] AS ProductType,	ISNULL(P.DisplayName,	P.[Name]) AS FuelType,
		(CASE WHEN @SkipMarineConversion = 1 THEN DefaultUom ELSE FR.UoM END) AS UoM,
		O.TerminalId,	TruckLoadTypeId,	T.[Name]  AS TerminalName,
		BD.BadgeNo1,	BD.BadgeNo2,	BD.BadgeNo3,
		ISNULL(DRNotes, '') AS DRNote
	INTO #Temp_Orders
	FROM Orders O WITH (NOLOCK)
		INNER JOIN OrderXStatuses OXS WITH (NOLOCK) ON O.Id = OXS.OrderId
		INNER JOIN FuelRequests FR WITH (NOLOCK) ON FR.Id = O.FuelRequestId
		INNER JOIN FuelRequestDetails FRD WITH (NOLOCK) ON FR.Id = FRD.FuelRequestId
		INNER JOIN Jobs JB WITH (NOLOCK) ON JB.Id = FR.JobId
		INNER JOIN @JobIds J ON JB.Id = J.Id
		INNER JOIN Companies C WITH (NOLOCK) ON C.Id = O.BuyerCompanyId
		INNER JOIN MstStates S WITH (NOLOCK) ON S.Id = JB.StateId
		LEFT JOIN MstCountries CN WITH (NOLOCK) ON CN.Id = JB.CountryId 
		INNER JOIN MstProducts P WITH (NOLOCK) ON P.Id = FR.FuelTypeId
		INNER JOIN MstProductTypes PT WITH (NOLOCK) ON PT.Id = P.ProductTypeId
		LEFT JOIN MstExternalTerminals T WITH (NOLOCK) ON T.Id = O.TerminalId
		LEFT JOIN OrderAdditionalDetails OAD WITH (NOLOCK) ON O.Id = OAD.OrderId
		LEFT JOIN OrderBadgeDetails BD WITH (NOLOCK) ON O.Id = BD.OrderId AND BD.IsActive = 1 AND BD.IsCommonBadge = 1
	WHERE O.AcceptedCompanyId = @SupplierCompanyId AND (@BuyerCompanyId = 0 OR O.BuyerCompanyId = @BuyerCompanyId)
		AND (
				(@IsEndSupplier =1 AND (O.IsEndSupplier = 1 OR FRD.IsDispatchRetainedByCustomer = 1)) 
				OR 
				(@IsEndSupplier = 0 AND (O.IsEndSupplier = 0 OR FRD.IsDispatchRetainedByCustomer = 0))
			)
		AND O.IsActive = 1 AND OXS.IsActive = 1 AND OXS.StatusId = 1 AND JB.IsActive = 1
		AND FR.ProductDisplayGroupId <> 7
		AND (@IsProductsToExcludeExists = 0 OR P.ProductTypeId NOT IN (SELECT Id FROM #ProductsToExclude))
		AND (TfxProductId IS NULL OR @IsFavFuelExists = 0 OR TfxProductId IN (SELECT Id FROM @FavFuelTypeIds))
		AND (@FavProductTypeExists = 0 OR P.ProductTypeId IN (SELECT Id FROM @FavProductTypeIds))
		--CTE_PickupInfo
	;WITH CTE_PickupInfo AS(
		SELECT D.OrderId,  D.LocationType, 	   D.[Address],	   D.City,	   D.SiteName,	   D.StateCode,	   D.StateId,	   D.CountryCode,	   D.TerminalId,
			   T.[Name] AS TerminalName,	   D.ZipCode,	 D.Latitude,	D.Longitude,	   D.CountyName
		FROM	FuelDispatchLocations D WITH (NOLOCK) 
				LEFT JOIN MstExternalTerminals T WITH (NOLOCK) ON D.TerminalId = T.Id
		WHERE D.Id IN (
					SELECT Max(P.Id)
					FROM #Temp_Orders O
						INNER JOIN FuelDispatchLocations P WITH (NOLOCK) ON O.OrderId = P.OrderId
					WHERE P.IsActive = 1 AND IsSkipped = 0
							AND DeliveryScheduleId IS NULL AND TrackableScheduleId IS NULL AND LocationType = 1 
					GROUP BY P.OrderId 
					HAVING Count(P.Id) > 0)
					)           
	SELECT DISTINCT O.Id AS OrderId, COALESCE(T_O.OrderName,T_O.PoNumber,'') + '('+T_O.FuelType+')' AS PoNumber,
			COALESCE(P.TerminalId, T_O.TerminalId,0) AS TerminalId,
			COALESCE(P.TerminalName, T_O.TerminalName,'') AS TerminalName, TruckLoadTypeId, DRNote,
			COALESCE(B.BadgeNo1,T.BadgeNo1,T_O.BadgeNo1,'') AS Badge1,
			COALESCE(B.BadgeNo2,T.BadgeNo2,T_O.BadgeNo2,'') AS Badge2,
			COALESCE(B.BadgeNo3,T.BadgeNo3, T_O.BadgeNo3,'') AS Badge3,
			(CASE WHEN SiteName IS NOT NULL AND TRIM(SiteName) != '' THEN 2 ELSE 1 END) AS PickupLocationType,
			SiteName AS BulkplantName, P.[Address],P.City,P.StateCode,P.StateId,P.CountryCode,P.ZipCode,P.CountyName,
			ISNULL(BP.Id, 0) AS SiteId, 
			ISNULL( P.Latitude, 0) AS Latitude,ISNULL( P.Longitude,0) AS Longitude
	FROM Orders O INNER JOIN
	#Temp_Orders T_O ON O.Id = T_O.OrderId 
		LEFT JOIN CTE_PickupInfo P ON O.Id = P.OrderId
		LEFT JOIN OrderBadgeDetails T WITH (NOLOCK) ON O.Id = T.OrderId AND ISNULL(P.TerminalId, O.TerminalId) = T.TerminalId
		LEFT JOIN OrderBadgeDetails B WITH (NOLOCK) ON O.Id = B.OrderId AND P.SiteName IS NOT NULL AND P.SiteName != '' AND B.BulkPlantId IS NOT NULL AND B.BulkPlantId IS NOT NULL
		LEFT JOIN BulkPlantLocations BP WITH (NOLOCK) ON B.BulkPlantId = BP.Id AND LOWER(P.SiteName) = LOWER(BP.Name) 
	WHERE 
		(B.Id IS NULL OR (B.IsActive = 1 AND B.IsCommonBadge = 0 ))
		AND (T.Id IS NULL OR (T.IsActive = 1 AND T.IsCommonBadge = 0))
	SELECT DISTINCT O.OrderId, SiteId, CAST (O.JobId AS BIGINT) JobId, JobName, O.BuyerCompanyId, BuyerCompanyName,
			ProductType, ProductTypeId, FuelTypeId,IsEndSupplier,
			IsDispatchRetained, COALESCE(OrderName,PoNumber,'') + '('+FuelType+')' AS PoNumber, 
			(CASE WHEN  UoM = 2 THEN 'Litres' WHEN UoM = 3 THEN 'Barrel' WHEN UoM = 4 THEN 'MetricTons' ELSE 'Gallons' END) AS UoM,
			1 AS  [Priority], FuelType +' - '+ COALESCE(OrderName, PoNumber, '') AS ProductName,
			COALESCE(S.SequenceNumber, P.SequenceNumber, 99999) AS ProductSequence
	FROM	#Temp_Orders O
			LEFT JOIN SupplierXProductSequencing S WITH (NOLOCK) ON O.OrderId = S.OrderId AND O.AcceptedCompanyId = S.SupplierCompanyId AND S.IsActive = 1
			LEFT JOIN SupplierXProductSequencing P WITH (NOLOCK) ON O.ProductTypeId = P.ProductId AND O.AcceptedCompanyId = P.SupplierCompanyId AND O.JobId = P.JobId AND P.IsActive = 1
	ORDER BY ProductSequence, OrderId
	SELECT DISTINCT OrderId AS Id ,COALESCE(OrderName,PoNumber,'') + '('+FuelType+')' AS
						[Name] FROM #Temp_Orders
END
GO

----------------------------------------------------------------------- 

CREATE OR ALTER   FUNCTION [dbo].[usf_GetDateDiffInDayHrMin]
(
    @StartDate    DATETIMEOFFSET(7),
    @EndDate    DATETIMEOFFSET(7)
)
RETURNS NVARCHAR(256)
AS
BEGIN
    DECLARE @Result NVARCHAR(256),
            @DiffInMinute int = DATEDIFF(MINUTE, @StartDate, @EndDate);
    
    SELECT @Result = 
    CASE    WHEN @DiffInMinute < 0 THEN CONCAT(0, ' Min')
	        WHEN @DiffInMinute < 60 THEN CONCAT(@DiffInMinute, ' Min')
            WHEN @DiffInMinute <= 1440 THEN CONCAT(CONCAT( @DiffInMinute / 60, ' Hr '), CONCAT(@DiffInMinute % 60, ' Min'))
            ELSE CONCAT(CONCAT(@DiffInMinute / 1440, ' Day '), CONCAT((@DiffInMinute % 1440) / 24, ' Hr')) END

    RETURN @Result
END
Go

---------------------------------------------------------------------
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[usp_GetBulkOrders]
@fileId Int
AS
BEGIN
	SELECT TOP(25) * FROM BulkOrderDetails WHERE FileId = @fileId AND IsOrderProcessed = 0 ORDER BY Id ASC
END
GO


GO
/****** Object:  StoredProcedure [dbo].[usp_GetBuyerInvoices]    Script Date: 4/27/2022 12:12:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_GetBuyerInvoices]   
@CompanyId int,
@BrandedCompanyId INT = 0,
@UserId int,      
@IsBuyerAdmin bit,      
@JobId int,      
@InvoiceTypeId int,      
@InvoiceFilter int,      
@StartDate datetimeoffset(7),      
@EndDate datetimeoffset(7),      
@CurrencyType int = 0,      
@CountryId int = 0,      
@GroupIds nvarchar(100),
@InvoiceTypeIdFilter nvarchar(150)= '',
@LocationIds nvarchar(max) = '', 
@VesselIds nvarchar(max) = '',   
@IsMarine BIT = 0,
@GlobalSearchText varchar(30) = NULL,      
@SortId int = 0,      
@SortDirection varchar(6) = 'desc',      
@PageSize int = 99999999,      
@PageNumber int = 1,
@InvoiceNumberSearchTypes [dbo].SEARCHTYPES READONLY,      
@PoNumberSearchTypes [dbo].SEARCHTYPES READONLY,
@BDRNumberSearchTypes [dbo].SEARCHTYPES READONLY,
@BolNumberSearchTypes [dbo].SEARCHTYPES READONLY,   
@DeliveryLevelPOSearchTypes [dbo].SEARCHTYPES READONLY,
@CarrierSearchTypes [dbo].SEARCHTYPES READONLY,   
@LiftDateSearchTypes [dbo].SEARCHTYPES READONLY,  
@JobSearchTypes [dbo].SEARCHTYPES READONLY,      
@SupplierSearchTypes [dbo].SEARCHTYPES READONLY,      
@FuelTypeSearchTypes [dbo].SEARCHTYPES READONLY,      
@DroppedGallonsSearchTypes [dbo].SEARCHTYPES READONLY,      
@PrePostValuesSearchTypes [dbo].SEARCHTYPES READONLY,
@TerminalSearchTypes [dbo].SEARCHTYPES READONLY,      
@AssetSearchTypes [dbo].SEARCHTYPES READONLY,      
@AmountSearchTypes [dbo].SEARCHTYPES READONLY,      
@DropDateSearchTypes [dbo].SEARCHTYPES READONLY,      
@DropTimeSearchTypes [dbo].SEARCHTYPES READONLY,      
@InvoiceDateSearchTypes [dbo].SEARCHTYPES READONLY,      
@DueDateSearchTypes [dbo].SEARCHTYPES READONLY, 
@VesselNameSearchTypes [dbo].SEARCHTYPES READONLY,
@StatusSearchTypes [dbo].SEARCHTYPES READONLY,      
--@LiftTicketNumberSearchTypes [dbo].SEARCHTYPES READONLY,      
@DropTicketNumberSearchTypes [dbo].SEARCHTYPES READONLY,      
@CreationMethodSearchTypes [dbo].SEARCHTYPES READONLY,      
@PickupAddressSearchTypes [dbo].SEARCHTYPES READONLY
AS      
BEGIN      
  DECLARE @InvoiceNumberSearchTypesValid int      
  SET @InvoiceNumberSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @InvoiceNumberSearchTypes)      
  DECLARE @PoNumberSearchTypesValid int      
  SET @PoNumberSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @PoNumberSearchTypes)     
  DECLARE @BolNumberSearchTypesValid int      
  SET @BolNumberSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @BolNumberSearchTypes)      
    DECLARE @CarrierSearchTypesValid int      
  SET @CarrierSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @CarrierSearchTypes)   
    DECLARE @LiftDateSearchTypesValid int      
  SET @LiftDateSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @LiftDateSearchTypes)  
  DECLARE @SupplierSearchTypesValid int      
  SET @SupplierSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @SupplierSearchTypes)      
  DECLARE @JobSearchTypesValid int      
  SET @JobSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @JobSearchTypes)      
  DECLARE @FuelTypeSearchTypesValid int      
  SET @FuelTypeSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @FuelTypeSearchTypes)      
  DECLARE @TerminalSearchTypesValid int      
  SET @TerminalSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @TerminalSearchTypes)      
  DECLARE @DropDateSearchTypesValid int      
  SET @DropDateSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @DropDateSearchTypes)      
  DECLARE @AssetSearchTypesValid int      
  SET @AssetSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @AssetSearchTypes)      
  DECLARE @AmountSearchTypesValid int      
  SET @AmountSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @AmountSearchTypes)      
  DECLARE @DropTimeSearchTypesValid int      
  SET @DropTimeSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @DropTimeSearchTypes)   
  DECLARE @VesselNameSearchTypesValid int      
  SET @VesselNameSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @VesselNameSearchTypes)
  DECLARE @StatusSearchTypesValid int      
  SET @StatusSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @StatusSearchTypes)      
  DECLARE @InvoiceDateSearchTypesValid int      
  SET @InvoiceDateSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @InvoiceDateSearchTypes)      
  DECLARE @DueDateSearchTypesValid int      
  SET @DueDateSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @DueDateSearchTypes)      
  DECLARE @DroppedGallonsSearchTypesValid int      
SET @DroppedGallonsSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @DroppedGallonsSearchTypes)      
  DECLARE @DropTicketNumberSearchTypesValid int      
  SET @DropTicketNumberSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @DropTicketNumberSearchTypes)      
  --DECLARE @LiftTicketNumberSearchTypesValid int      
  --SET @LiftTicketNumberSearchTypesValid = (SELECT      
  --  COUNT(*)      
  --FROM @LiftTicketNumberSearchTypes)      
  DECLARE @CreationMethodSearchTypesValid int      
  SET @CreationMethodSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @CreationMethodSearchTypes)      
  DECLARE @PickupAddressSearchTypesValid int      
  SET @PickupAddressSearchTypesValid = (SELECT      
    COUNT(*)      
  FROM @PickupAddressSearchTypes)      
  DECLARE @TblGroupCompanyIds TABLE (      
    CompanyId int PRIMARY KEY      
  );
  DECLARE @BDRNumberSearchTypesValid int
  SET @BDRNumberSearchTypesValid = (SELECT COUNT(*) FROM @BDRNumberSearchTypes);
  DECLARE @DeliveryLevelPOSearchTypesValid int
  SET @DeliveryLevelPOSearchTypesValid = (SELECT COUNT(*) FROM @DeliveryLevelPOSearchTypes);
  INSERT INTO @TblGroupCompanyIds      
    SELECT      
      *      
    FROM usf_GetGroupCompanyIds(@CompanyId, @GroupIds)      
	DECLARE @TblLocationIds TABLE (Id int);
	DECLARE @TblVesselIds TABLE (Id int);
	INSERT INTO @TblLocationIds SELECT VALUE FROM STRING_SPLIT(@LocationIds ,',');
	INSERT INTO @TblVesselIds SELECT VALUE FROM STRING_SPLIT(@VesselIds ,',');
  DECLARE @TblBOLDetails TABLE (      
    InvoiceHeaderId int,      
    BOLNo nvarchar(1000),      
    TerminalName nvarchar(max),      
    PickupAddress nvarchar(max),      
    PoNumber nvarchar(1000),      
    FuelType nvarchar(1000),      
    OrderId nvarchar(1000),      
    InvoiceId nvarchar(1000),      
    DroppedGallons nvarchar(1000),
	ConvertedQuantity nvarchar(1000),      
    DropDate nvarchar(1000),      
    DropTime nvarchar(1000)  ,  
	DropTicketNumber nvarchar(1000),  
	Carrier nvarchar(1000),  
	LiftDate nvarchar(1000),
	PrePostValues nvarchar(max)  
  );      
  INSERT INTO @TblBOLDetails      
    SELECT      
      *      
    FROM usf_getBolDetailsForBuyerInvoice(@CompanyId, @GroupIds, @InvoiceTypeId, @JobId,@StartDate, @EndDate,@InvoiceTypeIdFilter)      
  ;      
  WITH BuyerInvoices      
  AS (SELECT DISTINCT      
    --INV.Id,           
    BolDtls.InvoiceId AS Id,      
    --INV.OrderId  AS OrderId,           
    BolDtls.OrderId AS OrderId,      
    INV.DisplayInvoiceNumber AS InvoiceNumber,     
	INV.WaitingFor AS WaitingFor, 
    JBS.Name AS JobName,      
    JBS.Id AS JobId,      
    -- INV.PoNumber AS PoNumber,            
    BolDtls.PoNumber AS PoNumber,      
    --CASE WHEN IFD.PickupLocation = 2 THEN ISNULL(IFD.LiftTicketNumber,'--') ELSE ISNULL(IFD.BolNumber, '--') END AS BolNumber,          
    BolDtls.BOLNo AS BolNumber,  
 BolDtls.Carrier AS Carrier,  
 BolDtls.LiftDate AS LiftDate,  
    S.Name AS Supplier,      
    -- CASE WHEN INV.InvoiceTypeId = 5 THEN 'Dry Run Fee' WHEN INV.InvoiceTypeId = 9 THEN 'Balance Invoice' WHEN INV.InvoiceTypeId = 10 THEN 'Tank Rental Invoice' ELSE ISNULL(TFXP.NAME,PRD.Name) END AS FuelType,               
    BolDtls.FuelType AS FuelType,      
    --(            
    -- INV.BasicAmount - INV.TotalDiscountAmount +            
    -- (CASE WHEN INV.InvoiceTypeId = 5 THEN 0 ELSE ISNULL(INV.TotalFeeAmount, 0) END) +            
    -- (CASE WHEN (INV.InvoiceTypeId = 6 OR INV.InvoiceTypeId = 7) THEN 0             
    --  ELSE INV.TotalTaxAmount END)            
    --)  AS InvoiceAmount,            
    (      
    IH.TotalBasicAmount - IH.TotalDiscountAmount +      
    (CASE      
      WHEN INV.InvoiceTypeId = 5 THEN 0      
      ELSE ISNULL(IH.TotalFeeAmount, 0)      
    END) +      
    (CASE      
      WHEN (INV.InvoiceTypeId = 6 OR      
        INV.InvoiceTypeId = 7) THEN 0      
      ELSE IH.TotalTaxAmount      
    END)      
    ) AS InvoiceAmount,      
    -- CASE WHEN INV.InvoiceTypeId = 9 THEN '-' ELSE CONVERT(NVARCHAR(10), INV.DropEndDate, 101) END AS DropDate,            
    BolDtls.DropDate AS DropDate,      
    -- CASE WHEN INV.InvoiceTypeId = 9 THEN '-' ELSE FORMAT(INV.DropStartDate,'h:mm tt') + ' - ' + FORMAT(INV.DropEndDate,'h:mm tt') END AS DropTime,            
    BolDtls.DropTime AS DropTime,      
    CASE      
      WHEN IXS.StatusId = 10 THEN '--'      
      ELSE CONVERT(nvarchar(10), INV.UpdatedDate, 101)      
    END AS InvoiceDate,      
    CONVERT(nvarchar(10), INV.PaymentDueDate, 101) AS PaymentDueDate,      
    CASE      
      WHEN IXS.StatusId = 8 THEN IST.[Name] + ' - ' + A_USR.FirstName + ' ' + A_USR.LastName      
      ELSE IST.[Name]      
    END AS [Status],      
    IH.InvoiceNumberId,      
    -- ISNULL(MET.[Name],'--') AS TerminalName,            
    BolDtls.TerminalName AS TerminalName,      
    JXA.AssetId,      
    -- INV.DropEndDate,            
   -- INV.PaymentDueDate AS sPaymentDueDate,      
    --  CASE WHEN IXS.StatusId = 10 THEN NULL ELSE INV.CreatedDate END AS sInvoiceDate,            
    --INV.DroppedGallons AS DroppedGallons,          
    BolDtls.DroppedGallons AS DroppedGallons,
	BolDtls.ConvertedQuantity,
	--JBS.IsMarine  AS IsMarineLocation,
	CASE WHEN JBS.IsMarine IS NULL THEN CAST(0 AS BIT) ELSE CAST(JBS.IsMarine AS BIT) END AS IsMarineLocation,   
CASE WHEN  FRQ.UoM IS NULL THEN  0 ELSE FRQ.UoM END AS UoM,
--FRQ.UoM,
 (CASE WHEN FRQ.UoM = 2 THEN 'Litres' WHEN FRQ.UoM = 3 THEN 'Barrel' WHEN FRQ.UoM = 4 THEN 'MT' ELSE 'Gallons' END) AS UnitOfMeasurement,
	      
    CASE      
      WHEN IAD.CreationMethod = 2 THEN 'Mobile'      
      WHEN IAD.CreationMethod = 3 THEN 'Bulk Upload'      
      WHEN IAD.CreationMethod = 4 THEN 'TPD API'      
      ELSE 'TFX'        
    END AS CreationMethod,      
    --ISNULL(IAD.DropTicketNumber, '--') AS DropTicketNumber,      
 BolDtls.DropTicketNumber AS DropTicketNumber,  
    --ISNULL(IFD.LiftTicketNumber,'--') AS LiftTicketNumber,            
    -- CASE WHEN IFD.PickupLocation = 2 THEN 'Bulk Plant: ' + ISNULL(IFD.Address + ', ' + IFD.City+ ', ' + MST.Code + ' ' + IFD.ZipCode,'--') ELSE ISNULL(IFD.Address + ', ' + IFD.City+ ', ' + MST.Code + ' ' + IFD.ZipCode,'--') END AS [PickupAddress]      
  
   
       
    BolDtls.PickupAddress AS [PickupAddress],
	BolDtls.PrePostValues AS [PrePostValues],
	IH.Id AS [InvoiceHeaderId],
	(CASE WHEN AST.IsMarine = 1 THEN AST.[Name] ELSE '--' END) As [VesselName],
	(CASE WHEN JBS.IsMarine = 1 THEN BDR.[BDRNumber] ELSE '--' END) As [BDRNumber],
	 COALESCE(STUFF((Select ',' + DST.DeliveryLevelPO From DeliveryScheduleXTrackableSchedules DST
		INNER JOIN Invoices IV
		On IV.TrackableScheduleId=DST.ID and IV.InvoiceHeaderId=IH.Id Where IV.TrackableScheduleId IS NOT NULL
		AND DST.DeliveryLevelPO <>''
            FOR XML PATH(''), TYPE).value('text()[1]','nvarchar(500)')
           , 1, LEN(','), ''),'--') AS DeliveryLevelPO
  FROM dbo.Invoices INV      
  INNER JOIN InvoiceHeaderDetails IH      
    ON INV.InvoiceHeaderId = IH.Id AND INV.IsActive = 1 AND IH.IsActive = 1 
  INNER JOIN @TblBOLDetails BolDtls      
    ON BolDtls.InvoiceHeaderId = IH.Id      
  INNER JOIN dbo.InvoiceXInvoiceStatusDetails IXS      
    ON INV.Id = IXS.InvoiceId      
    AND IXS.IsActive = 1      
  INNER JOIN dbo.MstInvoiceStatuses IST      
    ON IXS.StatusId = IST.Id      
  INNER JOIN dbo.Orders ORD      
    ON INV.OrderId = ORD.Id      
  INNER JOIN dbo.FuelRequests FRQ      
    ON ORD.FuelRequestId = FRQ.Id      
  INNER JOIN Jobs JBS      
    ON FRQ.JobId = JBS.Id      
  INNER JOIN dbo.Companies S      
    ON S.Id = ORD.AcceptedCompanyId      
  INNER JOIN dbo.usf_GetUserJobIds(@UserId, @GroupIds) IDS      
    ON IDS.JobId = FRQ.JobId      
  LEFT JOIN dbo.JobXApprovalUsers A      
    ON JBS.Id = A.JobId      
    AND A.IsActive = 1      
  --LEFT JOIN dbo.MstProducts PRD ON FRQ.FuelTypeId = PRD.Id            
  --LEFT JOIN dbo.MstTfxProducts TFXP  ON TFXP.Id = PRD.TfxProductId            
  --LEFT JOIN InvoiceXBolDetails INVBOL ON INV.Id=INVBOL.InvoiceId            
  --LEFT JOIN dbo.InvoiceFtlDetails IFD ON IFD.Id = INVBOL.BolDetailId             
  --LEFT JOIN dbo.MstExternalTerminals MET ON IFD.TerminalId = MET.Id            
  LEFT JOIN dbo.Users A_USR      
    ON A.UserId = A_USR.Id      
  LEFT JOIN AssetDrops AD      
    ON INV.Id = AD.InvoiceId      
    AND AD.DropStatus = 0      
  LEFT JOIN JobXAssets JXA      
    ON AD.JobXAssetId = JXA.Id      
  LEFT JOIN InvoiceXAdditionalDetails IAD      
    ON INV.Id = IAD.InvoiceId   
  LEFT JOIN JobXAssets JXA1     
    ON JXA1.OrderId = ORD.Id AND JXA.RemovedBy IS NULL
  LEFT JOIN Assets AST 
    ON JXA1.AssetId = AST.Id AND AST.IsActive = 1
  LEFT JOIN BDRDetails(NOLOCK) BDR 
	ON BDR.InvoiceId = INV.Id AND BDR.IsActive = 1
  --LEFT JOIN MstStates MST ON MST.Id = IFD.StateId      
   LEFT JOIN DeliveryScheduleXTrackableSchedules(NOLOCK) DST 
	ON DST.Id = INV.TrackableScheduleId
  WHERE INV.InvoiceVersionStatusId = 1
  AND INV.WaitingFor != 9 -- Next marine drop
  AND INV.IsBuyPriceInvoice = 0      
  AND IXS.StatusId <> 10      
  AND (INV.OrderId IS NOT NULL      
  AND ORD.BuyerCompanyId IN (SELECT      
    CompanyId      
  FROM @TblGroupCompanyIds)      
  )      
  AND (@JobId = 0      
  OR FRQ.JobId = @JobId)      
  AND (      
  (@InvoiceTypeId IN (6, 7)      
  AND INV.InvoiceTypeId IN (6, 7))      
  OR (@InvoiceTypeId NOT IN (6, 7)      
  AND INV.InvoiceTypeId NOT IN (6, 7))      
  )      
  AND (INV.CreatedDate >= @StartDate      
  AND INV.CreatedDate < @EndDate)
  AND
  (@BrandedCompanyId <= 0 OR ORD.AcceptedCompanyId = @BrandedCompanyId)
  AND (      
  CASE      
    WHEN @InvoiceFilter = 0 THEN 1 --Invoice Filters            
    WHEN @InvoiceFilter IN (2, 11, 12, 1, 6, 5, 7, 3) AND      
      IXS.StatusId = @InvoiceFilter THEN 1      
    WHEN @InvoiceFilter = 4 AND      
      IXS.StatusId = 4 AND      
      ((SELECT      
        COUNT(IXIS.StatusId)      
      FROM InvoiceXInvoiceStatusDetails IXIS      
      WHERE IXIS.InvoiceId = INV.Id)      
      = 1 OR      
      2 = ANY (SELECT      
        IXIS.StatusId      
      FROM InvoiceXInvoiceStatusDetails IXIS      
      WHERE IXIS.InvoiceId = INV.Id)      
 ) THEN 1      
    WHEN @InvoiceFilter IN (13, 14) AND      
      (      
      4 = ANY (SELECT      
        IXIS.StatusId      
      FROM InvoiceXInvoiceStatusDetails IXIS      
      WHERE IXIS.InvoiceId = INV.Id      
      AND IXIS.IsActive = 1) AND      
      2 != ANY (SELECT      
        IXIS.StatusId      
      FROM InvoiceXInvoiceStatusDetails IXIS      
      WHERE IXIS.InvoiceId = INV.Id)      
      ) THEN 1      
    WHEN @InvoiceFilter = 15 AND      
      INV.ParentId IS NOT NULL THEN 1      
    WHEN @InvoiceFilter = 19 AND      
      INV.WaitingFor = 1 THEN 1      
    ELSE 0      
  END      
  ) = 1      
  AND (FRQ.FuelRequestTypeId <> 3      
  OR IXS.StatusId NOT IN (8, 4)      
  OR (@GroupIds = ''      
  AND dbo.usf_CheckInvoiceWorkflow(@UserId, @IsBuyerAdmin, IH.InvoiceNumberId, A.UserId, IXS.StatusId) = 1))      
  AND (      
  (@CountryId = 0      
  AND @CurrencyType = 0)      
  OR (JBS.CountryId = @CountryId      
  AND FRQ.Currency = @CurrencyType)      
  )
  AND (@LocationIds = '' OR (JBS.Id IN (Select Id from @TblLocationIds) AND JBS.IsMarine = @IsMarine))
  AND (@VesselIds = '' OR 0 < (SELECT Count(*) FROM AssetDrops AD INNER JOIN JobXAssets AJXA ON AJXA.Id = AD.JobXAssetId WHERE AJXA.AssetId IN (select Id from @TblVesselIds) AND AD.InvoiceId = INV.Id))
  ),      
  BuyerFinalInvoices      
  AS (SELECT      
    Id,      
    InvoiceNumber,      
    OrderId,   
 WaitingFor,  
    JobName,      
    JobId,      
    PoNumber,      
    BolNumber,  
 Carrier,  
 LiftDate,  
    Supplier,      
    FuelType,      
    InvoiceAmount,      
    DropDate,     
    DropTime,      
    InvoiceDate,      
    PaymentDueDate,      
    [Status],      
    InvoiceNumberId,      
    TerminalName,      
    --DropEndDate,            
    --sInvoiceDate,            
   -- sPaymentDueDate,      
    COUNT(DISTINCT AssetId) AS AssetCount,      
    DroppedGallons,      
    DropTicketNumber,      
    --LiftTicketNumber,            
    CreationMethod,      
    PickupAddress,
	PrePostValues,
	InvoiceHeaderId,
	[VesselName],
	[BDRNumber],
	[DeliveryLevelPO]
  FROM BuyerInvoices      
  GROUP BY Id,      
           InvoiceNumber,      
           OrderId,    
     WaitingFor,  
           JobName,      
           JobId,      
           PoNumber,      
           BolNumber,   
     Carrier,  
     LiftDate,  
           Supplier,      
           FuelType,      
           InvoiceAmount,      
           DropDate,      
           DropTime,      
 InvoiceDate,      
           PaymentDueDate,      
           [Status],      
           InvoiceNumberId,      
           TerminalName,      
           --DropEndDate,            
           --sInvoiceDate,            
        --   sPaymentDueDate,      
           DroppedGallons,      
           DropTicketNumber,      
           --LiftTicketNumber,            
           CreationMethod,      
           PickupAddress,
		   PrePostValues,
		   InvoiceHeaderId,
		   [VesselName],
		   [BDRNumber],
		   [DeliveryLevelPO])      
  SELECT      
    *,      
    [TotalCount] = COUNT(Id) OVER ()      
  FROM BuyerFinalInvoices      
  WHERE (      
  @InvoiceNumberSearchTypesValid = 0      
  OR (      
  @InvoiceNumberSearchTypesValid > 0      
  AND InvoiceNumber IN (SELECT      
    InvoiceNumber      
  FROM @InvoiceNumberSearchTypes      
  WHERE InvoiceNumber LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @PoNumberSearchTypesValid = 0      
  OR (      
  @PoNumberSearchTypesValid > 0      
  AND PoNumber IN (SELECT      
    PoNumber      
  FROM @PoNumberSearchTypes      
  WHERE PoNumber LIKE '%' + SearchVar + '%')      
  ))   
    
  AND (      
  @BolNumberSearchTypesValid = 0      
  OR (      
  @BolNumberSearchTypesValid > 0      
  AND BolNumber IN (SELECT      
    BolNumber      
  FROM @BolNumberSearchTypes      
  WHERE BolNumber LIKE '%' + SearchVar + '%')      
  ))  
  
    AND (      
  @CarrierSearchTypesValid = 0      
  OR (      
  @CarrierSearchTypesValid > 0      
  AND Carrier IN (SELECT      
    Carrier     
  FROM @CarrierSearchTypes      
  WHERE Carrier LIKE '%' + SearchVar + '%')      
  ))  
  
    AND (      
  @LiftDateSearchTypesValid = 0      
  OR (      
  @LiftDateSearchTypesValid > 0      
  AND LiftDate IN (SELECT      
    LiftDate      
  FROM @LiftDateSearchTypes      
  WHERE LiftDate LIKE '%' + SearchVar + '%')      
  ))  
  
  --AND      (             
  --                  @LiftTicketNumberSearchTypesValid = 0             
  --         OR       (             
  --                           @LiftTicketNumberSearchTypesValid > 0             
  --                  AND      LiftTicketNumber IN             
  --                           (             
  --                                  SELECT LiftTicketNumber             
  --                                  FROM   @LiftTicketNumberSearchTypes             
  --                                  WHERE  LiftTicketNumber LIKE '%' + SearchVar + '%')))             
  AND (      
  @PickupAddressSearchTypesValid = 0      
  OR (      
  @PickupAddressSearchTypesValid > 0      
  AND [PickupAddress] IN (SELECT      
    [PickupAddress]      
  FROM @PickupAddressSearchTypes      
  WHERE [PickupAddress] LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @CreationMethodSearchTypesValid = 0      
  OR (      
  @CreationMethodSearchTypesValid > 0      
  AND CreationMethod IN (SELECT      
    CreationMethod      
  FROM @CreationMethodSearchTypes      
  WHERE CreationMethod LIKE '%' + SearchVar + '%')      
  ))      
  --AND      (             
  --                  @LiftTicketNumberSearchTypesValid = 0             
  --         OR       (             
  --                           @LiftTicketNumberSearchTypesValid > 0             
  --                  AND      LiftTicketNumber IN             
  --                           (             
  --                                  SELECT LiftTicketNumber             
  --                                  FROM   @LiftTicketNumberSearchTypes             
  --                                  WHERE  LiftTicketNumber LIKE '%' + SearchVar + '%')))             
  AND (      
  @SupplierSearchTypesValid = 0      
  OR (      
  @SupplierSearchTypesValid > 0      
  AND Supplier IN (SELECT      
    Supplier      
  FROM @SupplierSearchTypes      
  WHERE Supplier LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @JobSearchTypesValid = 0      
  OR (      
  @JobSearchTypesValid > 0      
  AND JobName IN (SELECT      
    JobName      
  FROM @JobSearchTypes      
  WHERE JobName LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @FuelTypeSearchTypesValid = 0      
  OR (      
  @FuelTypeSearchTypesValid > 0      
  AND FuelType IN (SELECT      
    FuelType      
  FROM @FuelTypeSearchTypes      
  WHERE FuelType LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @TerminalSearchTypesValid = 0      
  OR (      
  @TerminalSearchTypesValid > 0      
  AND TerminalName IN (SELECT      
    TerminalName      
  FROM @TerminalSearchTypes      
  WHERE TerminalName LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @DropDateSearchTypesValid = 0      
  OR (      
  @DropDateSearchTypesValid > 0      
  AND DropDate IN (SELECT      
    DropDate      
  FROM @DropDateSearchTypes      
  WHERE DropDate LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @AssetSearchTypesValid = 0      
  OR (      
  @AssetSearchTypesValid > 0      
  AND AssetCount IN (SELECT      
    AssetCount      
  FROM @AssetSearchTypes      
  WHERE AssetCount LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @AmountSearchTypesValid = 0      
  OR (      
  @AmountSearchTypesValid > 0      
  AND InvoiceAmount IN (SELECT      
    InvoiceAmount      
  FROM @AmountSearchTypes      
  WHERE InvoiceAmount LIKE '%' + REPLACE(SearchVar, '$', '') + '%')      
  ))      
  AND (      
  @DropTimeSearchTypesValid = 0      
  OR (      
  @DropTimeSearchTypesValid > 0      
  AND DropTime IN (SELECT      
    DropTime      
  FROM @DropTimeSearchTypes      
  WHERE DropTime LIKE '%' + SearchVar + '%')      
  )) 
  
  AND (      
  @VesselNameSearchTypesValid = 0      
  OR (      
   @VesselNameSearchTypesValid > 0      
  AND [VesselName] IN (SELECT      
    [VesselName]      
  FROM @VesselNameSearchTypes      
  WHERE [VesselName] LIKE '%' + SearchVar + '%')      
  )) 
  AND (      
  @StatusSearchTypesValid = 0      
  OR (      
  @StatusSearchTypesValid > 0      
  AND [Status] IN (SELECT      
    [Status]      
  FROM @StatusSearchTypes      
  WHERE [Status] LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @InvoiceDateSearchTypesValid = 0      
  OR (      
  @InvoiceDateSearchTypesValid > 0      
  AND InvoiceDate IN (SELECT      
    InvoiceDate      
  FROM @InvoiceDateSearchTypes      
  WHERE InvoiceDate LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
 @DueDateSearchTypesValid = 0      
  OR (      
  @DueDateSearchTypesValid > 0      
  AND PaymentDueDate IN (SELECT      
    PaymentDueDate      
  FROM @DueDateSearchTypes      
  WHERE PaymentDueDate LIKE '%' + SearchVar + '%')      
  ))      
  AND (      
  @DroppedGallonsSearchTypesValid = 0      
  OR (      
  @DroppedGallonsSearchTypesValid > 0      
  AND DroppedGallons IN (SELECT      
    DroppedGallons      
  FROM @DroppedGallonsSearchTypes      
  WHERE DroppedGallons LIKE '%' + SearchVar + '%')      
  ))
  AND (
	@BDRNumberSearchTypesValid = 0 
	OR (
	@BDRNumberSearchTypesValid > 0 
	AND BDRNumber IN (SELECT BDRNumber
					FROM @BDRNumberSearchTypes
					WHERE BDRNumber LIKE '%' + SearchVar + '%')
	))
	 AND (
	@DeliveryLevelPOSearchTypesValid = 0 
	OR (
	@DeliveryLevelPOSearchTypesValid > 0 
	AND DeliveryLevelPO IN (SELECT DeliveryLevelPO
					FROM @DeliveryLevelPOSearchTypes
					WHERE DeliveryLevelPO LIKE '%' + SearchVar + '%')
	))
  AND (      
  @GlobalSearchText IS NULL      
  OR (      
  (InvoiceNumber LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  PoNumber LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  BolNumber LIKE '%' + @GlobalSearchText + '%')      
  --OR       (             
  --                                       LiftTicketNumber LIKE '%' + @GlobalSearchText+ '%')   
  
  OR (  
   Carrier LIKE '%' + @GlobalSearchText + '%'  
  )  
  OR (  
   LiftDate LIKE '%' + @GlobalSearchText + '%'  
  )  
  OR (      
  DropTicketNumber LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  CreationMethod LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  JobName LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  Supplier LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  FuelType LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  DroppedGallons LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  TerminalName LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  DropDate LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  AssetCount LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  InvoiceAmount LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  DropTime LIKE '%' + @GlobalSearchText + '%') 
  OR (      
  [VesselName] LIKE '%' + @GlobalSearchText + '%')  
  OR (      
  [Status] LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  InvoiceDate LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  PaymentDueDate LIKE '%' + @GlobalSearchText + '%')      
  OR (      
  PickupAddress LIKE '%' + @GlobalSearchText + '%')
  OR (
  [BDRNumber] LIKE '%' + @GlobalSearchText + '%')
   OR (
  [DeliveryLevelPO] LIKE '%' + @GlobalSearchText + '%')
  ))      
  ORDER BY CASE      
    --WHEN @SortId = 6             
    --                   AND      @SortDirection = 'asc' THEN DroppedGallons            
    WHEN @SortId = 12 AND      
      @SortDirection = 'asc' THEN AssetCount      
  END ASC,      
  CASE      
    WHEN @SortId = 13 AND      
      @SortDirection = 'asc' THEN InvoiceAmount      
  END ASC,      
 -- CASE      
    --WHEN @SortId = 10            
    --AND @SortDirection = 'asc' THEN DropEndDate             
    --WHEN @SortId = 12            
    --AND      @SortDirection = 'asc' THEN  sInvoiceDate            
  --  WHEN @SortId = 13 AND      
   --   @SortDirection = 'asc' THEN  sPaymentDueDate      
 -- END ASC,      
  CASE      
    WHEN @SortId = 0 AND      
      @SortDirection = 'asc' THEN InvoiceNumber      
    WHEN @SortId = 1 AND      
      @SortDirection = 'asc' THEN PoNumber
	WHEN @SortId = 2 AND      
      @SortDirection = 'asc' THEN [BDRNumber]
    WHEN @SortId = 3 AND      
      @SortDirection = 'asc' THEN BolNumber
	WHEN @SortId = 4 AND      
      @SortDirection = 'asc' THEN DeliveryLevelPO
	WHEN @SortId = 5 AND      
      @SortDirection = 'asc' THEN LiftDate
	WHEN @SortId = 6 AND      
      @SortDirection = 'asc' THEN Carrier
    WHEN @SortId = 7 AND      
      @SortDirection = 'asc' THEN JobName      
    WHEN @SortId = 8 AND      
      @SortDirection = 'asc' THEN Supplier      
    WHEN @SortId = 9 AND      
      @SortDirection = 'asc' THEN FuelType      
    WHEN @SortId = 10 AND      
      @SortDirection = 'asc' THEN DroppedGallons      
    WHEN @SortId = 11 AND      
      @SortDirection = 'asc' THEN PrePostValues      
    WHEN @SortId = 12 AND      
      @SortDirection = 'asc' THEN TerminalName      
    WHEN @SortId = 15 AND      
      @SortDirection = 'asc' THEN DropDate      
    WHEN @SortId = 16 AND      
      @SortDirection = 'asc' THEN DropTime      
    WHEN @SortId = 17 AND      
      @SortDirection = 'asc' THEN InvoiceDate      
    WHEN @SortId = 18 AND      
      @SortDirection = 'asc' THEN  PaymentDueDate 
	WHEN @SortId = 19 AND      
      @SortDirection = 'asc' THEN [VesselName]  
    WHEN @SortId = 20 AND      
      @SortDirection = 'asc' THEN [Status]      
    WHEN @SortId = 21 AND      
      @SortDirection = 'asc' THEN DropTicketNumber      
    --WHEN @SortId = 16            
    --AND      @SortDirection = 'asc' THEN LiftTicketNumber            
    WHEN @SortId = 22 AND      
      @SortDirection = 'asc' THEN CreationMethod      
  END ASC,      
  CASE      
    --WHEN @SortId = 6            
    --                   AND      @SortDirection = 'desc' THEN DroppedGallons              
    WHEN @SortId = 13 AND      
      @SortDirection = 'desc' THEN AssetCount      
       
  END DESC,      
  CASE      
    WHEN @SortId = 14 AND      
      @SortDirection = 'desc' THEN InvoiceAmount      
  END DESC,      
  --CASE      
    --WHEN @SortId = 10            
    --AND      @SortDirection = 'desc' THEN DropEndDate            
    --WHEN @SortId = 12            
    --AND      @SortDirection = 'desc' THEN sInvoiceDate             
   -- WHEN @SortId = 13 AND      
    --  @SortDirection = 'desc' THEN sPaymentDueDate      
 -- END DESC,      
  CASE      
    WHEN @SortId = 0 AND      
      @SortDirection = 'desc' THEN InvoiceNumber      
    WHEN @SortId = 1 AND      
      @SortDirection = 'desc' THEN PoNumber
	WHEN @SortId = 2 AND      
      @SortDirection = 'desc' THEN [BDRNumber]
    WHEN @SortId = 3 AND      
      @SortDirection = 'desc' THEN BolNumber   
	WHEN @SortId = 4 AND      
      @SortDirection = 'desc' THEN [DeliveryLevelPO]
    WHEN @SortId = 5 AND  
      @SortDirection = 'desc' THEN LiftDate  
    WHEN @SortId = 6 AND   
     @SortDirection = 'desc' THEN Carrier  
    WHEN @SortId = 7 AND      
      @SortDirection = 'desc' THEN JobName      
    WHEN @SortId = 8 AND      
      @SortDirection = 'desc' THEN Supplier      
    WHEN @SortId = 9 AND      
      @SortDirection = 'desc' THEN FuelType      
    WHEN @SortId = 10 AND      
      @SortDirection = 'desc' THEN DroppedGallons      
	WHEN @SortId = 11 AND      
      @SortDirection = 'desc' THEN PrePostValues
    WHEN @SortId = 12 AND      
      @SortDirection = 'desc' THEN TerminalName      
    WHEN @SortId = 15 AND      
      @SortDirection = 'desc' THEN DropDate      
    WHEN @SortId = 16 AND      
      @SortDirection = 'desc' THEN DropTime      
    WHEN @SortId = 17 AND      
      @SortDirection = 'desc' THEN InvoiceDate      
    WHEN @SortId = 18 AND      
      @SortDirection = 'desc' THEN PaymentDueDate  
	WHEN @SortId = 19 AND      
      @SortDirection = 'desc' THEN [VesselName]  
    WHEN @SortId = 20 AND      
      @SortDirection = 'desc' THEN [Status]      
    WHEN @SortId = 21 AND      
      @SortDirection = 'desc' THEN DropTicketNumber      
    --WHEN @SortId = 16            
    --AND      @SortDirection = 'desc' THEN LiftTicketNumber             
    WHEN @SortId = 22 AND      
      @SortDirection = 'desc' THEN CreationMethod     
    --WHEN @SortId = 18 AND      
    --  @SortDirection = 'desc' THEN Id      
  END DESC,
  CASE        
    WHEN @SortId = 24 AND        
      @SortDirection = 'desc' THEN InvoiceHeaderId        
        
  END DESC
  OFFSET (@PageNumber - 1) * @PageSize ROWS      
  FETCH NEXT @PageSize ROWS ONLY      
END
GO

/****** Object:  StoredProcedure [dbo].[usp_GetCompaniesDropDown]    Script Date: 28-04-2022 18:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Anant
-- Create date: 28-04-2022
-- Description:	Get all companies for drop down
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[usp_GetCompaniesDropDown] 
AS
BEGIN
	SELECT Id, Name FROM Companies ORDER BY ID DESC
END
GO

/****** Object:  StoredProcedure [dbo].[usp_GetFuelSurchargeTables]    Script Date: 4/22/2022 15:01:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[usp_GetFuelSurchargeTables]   
  @TableType  INT = 0,  
  @CustomerId  INT = NULL,  
  @TerminalId  INT = NULL,  
  @BulkPlantId  INT = NULL,  
  @SupplierId INT = 0,  
  @SourceRegionIds NVARCHAR(MAX) = NULL,
  @SelectedTerminals NVARCHAR(MAX) = NULL,
  @SelectedBulkPlants NVARCHAR(MAX) = NULL
AS   
BEGIN  
 SELECT DISTINCT   
  FS.Id AS Id,  
  FS.Name AS [Name]  
  FROM FuelSurchargeIndexes(NOLOCK) FS  
  LEFT JOIN FreightTablePickupLocations(NOLOCK) FL ON FL.FuelSurchargeIndexId = FS.Id  
  LEFT JOIN FreightTableCompanies(NOLOCK) FC ON FC.FuelSurchargeIndexId = FS.Id  
  LEFT JOIN FreightTableSourceRegions(NOLOCK) FTSR ON FS.Id = FTSR.FuelSurchargeIndexId  
  LEFT JOIN SourceRegions(NOLOCK) SR ON SR.Id = FTSR.SourceRegionId  
 WHERE FS.TableType = @TableType AND FS.IsActive = 1 and FS.StatusId = 2  
 AND FS.SupplierCompanyId = @SupplierId 
 AND 
 (
     @TableType != 2  OR 
     (
	   FC.AssignedCompanyId = @CustomerId
	 )
  )
 AND FTSR.SourceRegionId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SourceRegionIds,',')) 
 AND FL.TerminalId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SelectedTerminals,','))
 
 UNION 

  SELECT DISTINCT   
  FS.Id AS Id,  
  FS.Name AS [Name]  
  FROM FuelSurchargeIndexes(NOLOCK) FS  
  LEFT JOIN FreightTablePickupLocations(NOLOCK) FL ON FL.FuelSurchargeIndexId = FS.Id  
  LEFT JOIN FreightTableCompanies(NOLOCK) FC ON FC.FuelSurchargeIndexId = FS.Id  
  LEFT JOIN FreightTableSourceRegions(NOLOCK) FTSR ON FS.Id = FTSR.FuelSurchargeIndexId  
  LEFT JOIN SourceRegions(NOLOCK) SR ON SR.Id = FTSR.SourceRegionId  
 WHERE FS.TableType = @TableType AND FS.IsActive = 1 and FS.StatusId = 2  
 AND FS.SupplierCompanyId = @SupplierId 
 AND 
 (
     @TableType != 2  OR 
     (
	   FC.AssignedCompanyId = @CustomerId
	 )
  )  
 AND FTSR.SourceRegionId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SourceRegionIds,',')) 
 AND FL.BulkPlantId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SelectedBulkPlants,','))    
 ORDER  BY Name

END  
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetFreightRateTables]       
  @FreightRateRuleType INT = 0,      
  @TableType  INT = 0,      
  @CustomerId  INT = NULL,      
  @TerminalId  INT = NULL,      
  @BulkPlantId  INT = NULL,      
  @SupplierId INT = 0,      
  @FuelTypeId INT = NULL,      
  @SourceRegionIds NVARCHAR(MAX) = NULL,  
  @SelectedTerminals NVARCHAR(MAX) = NULL,  
  @SelectedBulkPlants NVARCHAR(MAX) = NULL  
AS       
BEGIN      
      
 SELECT DISTINCT       
  FR.Id AS Id,      
  FR.Name AS [Name]      
  FROM FreightRateRules(NOLOCK) FR      
  LEFT JOIN FreightTablePickupLocations(NOLOCK) FL ON FL.FreightRateRuleId = FR.Id      
  LEFT JOIN FreightTableCompanies(NOLOCK) FC ON FC.FreightRateRuleId = FR.Id      
  LEFT JOIN FreightRateFuelGroups(NOLOCK) FG ON FG.FreightRateRuleId = FR.Id      
  LEFT JOIN FuelGroupFuelTypes(NOLOCK) FT ON FT.FuelGroupId = FG.FuelGroupId      
  LEFT JOIN MstProducts(NOLOCK) TP ON TP.TfxProductId = FT.TfxProductId      
  LEFT JOIN FreightTableSourceRegions(NOLOCK) FTSR ON FR.Id = FTSR.FreightRateRuleId      
  LEFT JOIN SourceRegions(NOLOCK) SR ON SR.Id = FTSR.SourceRegionId      
 WHERE FR.FreightRateRuleType = @FreightRateRuleType and FR.TableType = @TableType      
 AND FR.IsActive = 1 and FR.[Status] = 2      
 AND FR.CreatedByCompanyId = @SupplierId AND FT.TfxProductId = @FuelTypeId /*TP.Id = @FuelTypeId*/     
 AND   
 (  
     @TableType != 2  OR   
     (  
    FC.AssignedCompanyId = @CustomerId  
  )  
  )  
     
 AND FTSR.SourceRegionId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SourceRegionIds,','))      
 AND FL.TerminalId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SelectedTerminals,','))      
 
 UNION

  SELECT DISTINCT       
  FR.Id AS Id,      
  FR.Name AS [Name]      
  FROM FreightRateRules(NOLOCK) FR      
  LEFT JOIN FreightTablePickupLocations(NOLOCK) FL ON FL.FreightRateRuleId = FR.Id      
  LEFT JOIN FreightTableCompanies(NOLOCK) FC ON FC.FreightRateRuleId = FR.Id      
  LEFT JOIN FreightRateFuelGroups(NOLOCK) FG ON FG.FreightRateRuleId = FR.Id      
  LEFT JOIN FuelGroupFuelTypes(NOLOCK) FT ON FT.FuelGroupId = FG.FuelGroupId      
  LEFT JOIN MstProducts(NOLOCK) TP ON TP.TfxProductId = FT.TfxProductId      
  LEFT JOIN FreightTableSourceRegions(NOLOCK) FTSR ON FR.Id = FTSR.FreightRateRuleId      
  LEFT JOIN SourceRegions(NOLOCK) SR ON SR.Id = FTSR.SourceRegionId      
 WHERE FR.FreightRateRuleType = @FreightRateRuleType and FR.TableType = @TableType      
 AND FR.IsActive = 1 and FR.[Status] = 2      
 AND FR.CreatedByCompanyId = @SupplierId AND FT.TfxProductId = @FuelTypeId /*TP.Id = @FuelTypeId*/     
 AND   
 (  
     @TableType != 2  OR   
     (  
    FC.AssignedCompanyId = @CustomerId  
  )  
 )  
     
 AND FTSR.SourceRegionId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SourceRegionIds,','))      
 AND FL.BulkPlantId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SelectedBulkPlants,','))      
 ORDER  BY Name

END  
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetAccessorialFeeTables]     
  @TableType  INT = 0,    
  @CustomerId  INT = NULL,    
  @TerminalId  INT = NULL,    
  @BulkPlantId  INT = NULL,    
  @SupplierId INT = 0,    
  @SourceRegionIds NVARCHAR(MAX) = NULL,  
  @SelectedTerminals NVARCHAR(MAX) = NULL,  
  @SelectedBulkPlants NVARCHAR(MAX) = NULL  
AS     
BEGIN    
    
 SELECT DISTINCT     
  AF.Id AS Id,    
  AF.Name AS [Name]    
  FROM AccessorialFees(NOLOCK) AF    
  LEFT JOIN FreightTablePickupLocations(NOLOCK) FL ON FL.AccessorialFeeId = AF.Id    
  LEFT JOIN FreightTableCompanies(NOLOCK) FC ON FC.AccessorialFeeId = AF.Id     
  LEFT JOIN FreightTableSourceRegions(NOLOCK) FTSR ON AF.Id = FTSR.AccessorialFeeId    
  LEFT JOIN SourceRegions(NOLOCK) SR ON SR.Id = FTSR.SourceRegionId    
 WHERE AF.TableType = @TableType AND AF.IsActive = 1 and AF.StatusId = 2    
 AND AF.SupplierCompanyId = @SupplierId    
 AND   
 (  
     @TableType != 2  OR   
     (  
    FC.AssignedCompanyId = @CustomerId  
  )  
  )  
   
 AND FTSR.SourceRegionId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SourceRegionIds,',')) 
 AND FL.TerminalId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SelectedTerminals,','))  
 
 UNION

 SELECT DISTINCT     
  AF.Id AS Id,    
  AF.Name AS [Name]    
  FROM AccessorialFees(NOLOCK) AF    
  LEFT JOIN FreightTablePickupLocations(NOLOCK) FL ON FL.AccessorialFeeId = AF.Id    
  LEFT JOIN FreightTableCompanies(NOLOCK) FC ON FC.AccessorialFeeId = AF.Id     
  LEFT JOIN FreightTableSourceRegions(NOLOCK) FTSR ON AF.Id = FTSR.AccessorialFeeId    
  LEFT JOIN SourceRegions(NOLOCK) SR ON SR.Id = FTSR.SourceRegionId    
 WHERE AF.TableType = @TableType AND AF.IsActive = 1 and AF.StatusId = 2    
 AND AF.SupplierCompanyId = @SupplierId    
 AND   
 (  
     @TableType != 2  OR   
     (  
    FC.AssignedCompanyId = @CustomerId  
  )  
  )  
   
 AND FTSR.SourceRegionId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SourceRegionIds,','))    
 AND FL.BulkPlantId in (SELECT TRIM(VALUE) FROM STRING_SPLIT(@SelectedBulkPlants,','))      
 ORDER BY [Name]

END    
GO

--exec [dbo].[usp_GetGallonsDeliveredCount]  0, null,null;
CREATE OR ALTER PROCEDURE [dbo].[usp_GetGallonsDeliveredCount] 
 @CompanyId INT  = 0,  
 @StartDate DATETIMEOFFSET = NULL,  
 @EndDate DATETIMEOFFSET = NULL
AS  
BEGIN

	DECLARE @TotalCount decimal  

	IF (@CompanyId = 0 AND @StartDate Is Null AND @EndDate IS NULL) --This will be default 99% call 
		BEGIN 
			SET @TotalCount = (SELECT CAST (ISNULL(SUM(ISNULL(INV.DroppedGallons, 0)),0) As DECIMAL) AS TotalOrderedDeliveredCount
								FROM Invoices INV WITH (NOLOCK)  
								WHERE
									INV.IsActive=1 AND INV.InvoiceVersionStatusId=1 AND INV.IsBuyPriceInvoice=0);
			SELECT @TotalCount AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount
		END
	ELSE 
		BEGIN
			--DROP TABLE IF EXISTS #AllInvoices; --CAST(Id As decimal) As Id, 
			SELECT TOP 0 DroppedGallons, CreatedDate
			INTO #AllInvoices
			FROM INVOICES WITH (NOLOCK);

			IF (@CompanyId > 0)  
					BEGIN  
						INSERT INTO #AllInvoices  
							SELECT INV.DroppedGallons,INV.CreatedDate   
							FROM	Orders ORD  WITH (NOLOCK)  
								JOIN Invoices INV  WITH (NOLOCK) ON ORD.Id = INV.OrderId  
							WHERE   
								INV.IsActive=1 AND INV.InvoiceVersionStatusId=1 AND INV.IsBuyPriceInvoice=0
								AND INV.OrderId IS NOT NULL AND  ORD.AcceptedCompanyId=@CompanyId    
					END  
			ELSE  
					BEGIN  
						INSERT INTO #AllInvoices  
							SELECT INV.DroppedGallons,INV.CreatedDate   
							FROM Invoices INV   WITH (NOLOCK) 
							WHERE   
								INV.IsActive=1 AND INV.InvoiceVersionStatusId=1 AND INV.IsBuyPriceInvoice=0
							ORDER BY INV.Id
					END  
  
			SET @TotalCount = (SELECT CAST(ISNULL(SUM(ISNULL(AI.DroppedGallons, 0)),0) As DECIMAL) AS TotalOrderedDeliveredCount
								FROM #AllInvoices AI)  
  
			IF (@StartDate IS NULL AND @EndDate IS NULL)
				SELECT @TotalCount AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount;
			ELSE
				BEGIN
					SELECT CAST(ISNULL(SUM(ISNULL(AI.DroppedGallons, 0)),0) As DECIMAL) AS TotalOrderedDeliveredCount, @TotalCount As TotalCount  
						FROM #AllInvoices AI
						WHERE 
							AI.CreatedDate >= (CASE WHEN @StartDate IS NOT NULL THEN  @StartDate ELSE CONVERT(DATETIME, -53690) END)
							AND
							AI.CreatedDate <= (CASE WHEN @EndDate IS NOT NULL THEN @EndDate ELSE GETDATE() END)
				END
		END	
END
Go

--exec [dbo].[usp_GetGallonsOrderedCount]  0, null ,null;
CREATE OR ALTER   PROCEDURE [dbo].[usp_GetGallonsOrderedCount]   
 @CompanyId INT ,  
 @StartDate DATETIMEOFFSET = NULL,  
 @EndDate DATETIMEOFFSET = NULL  
AS  
BEGIN 
	DECLARE @TotalCount decimal;

	IF (@CompanyId = 0 AND @StartDate Is Null AND @EndDate IS NULL) --This will be default 99% call 
		BEGIN 
			SET @TotalCount = (SELECT CAST (ISNULL(SUM(COALESCE(ORD.BrokeredMaxQuantity,FR.MaxQuantity,0)), 0) As Decimal)  
										FROM Orders ORD   
											JOIN FuelRequests FR ON ORD.FuelRequestId = FR.Id  
										WHERE   
											ORD.IsActive=1 AND ORD.ParentId IS NULL AND FR.QuantityTypeId <> 3);			

			SELECT @TotalCount AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount
		END
	ELSE
		BEGIN
			--DROP TABLE IF EXISTS #AllOrders;
			SELECT TOP 0 BrokeredMaxQuantity, AcceptedDate
			INTO #AllOrders
			FROM Orders ORD

			IF (@CompanyId > 0)  
					BEGIN  
					INSERT INTO #AllOrders  
						SELECT COALESCE(ORD.BrokeredMaxQuantity,FR.MaxQuantity,0) AS BrokeredMaxQuantity, ORD.AcceptedDate  
						FROM Orders ORD   
							JOIN FuelRequests FR ON ORD.FuelRequestId = FR.Id  
						WHERE   
							ORD.IsActive=1 AND ORD.ParentId IS NULL AND FR.QuantityTypeId <> 3
							AND ORD.AcceptedCompanyId = @CompanyId    
					END  
			ELSE  
					BEGIN  
					INSERT INTO #AllOrders  
						SELECT COALESCE(ORD.BrokeredMaxQuantity,FR.MaxQuantity,0) AS BrokeredMaxQuantity, ORD.AcceptedDate  
						FROM Orders ORD   
							JOIN FuelRequests FR ON ORD.FuelRequestId = FR.Id  
						WHERE   
							ORD.IsActive=1 AND ORD.ParentId IS NULL AND FR.QuantityTypeId <> 3   
					END
	
			SET @TotalCount = (SELECT CAST(ISNULL(SUM(ISNULL(AO.BrokeredMaxQuantity, 0)),0) As DECIMAL) FROM #AllOrders AO); 
				
			DECLARE @BrokeredMaxQuantity decimal;
			IF (@StartDate IS NOT NULL OR @EndDate IS NOT NULL)
				BEGIN
					SELECT @BrokeredMaxQuantity = CAST(ISNULL(SUM(ISNULL(AO.BrokeredMaxQuantity, 0)),0) As DECIMAL)
						FROM #AllOrders AO
						WHERE 
							AO.AcceptedDate >= (CASE WHEN @StartDate IS NOT NULL THEN  @StartDate ELSE CONVERT(DATETIME, -53690) END)
							AND
							AO.AcceptedDate <= (CASE WHEN @EndDate IS NOT NULL THEN @EndDate ELSE GETDATE() END);
				END
			ELSE
				SELECT @BrokeredMaxQuantity = @TotalCount;
				
			SELECT @BrokeredMaxQuantity AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount;
		END
END  
Go
GO

--exec usp_GetConsolidatedInvoicePdf 849058  
CREATE OR ALTER  PROCEDURE [dbo].[usp_GetConsolidatedInvoicePdf]     
 @InvoiceHeaderId INT        
AS        
BEGIN        
 ------------------------------ DROP LOCATION --------------------      
 DECLARE @True BIT = 1;        
 DECLARE @False BIT = 0;      
 DECLARE @DropSiteName NVARCHAR(128);        
 DECLARE @DropAddress NVARCHAR(128);        
 DECLARE @DropCity NVARCHAR(128);        
 DECLARE @DropStateCode NVARCHAR(128);        
 DECLARE @DropZipCode NVARCHAR(128);        
 DECLARE @IsDropLocationAvailable BIT = @False;        
 --Get drop location       
 SELECT TOP 1        
  @DropSiteName = CASE WHEN IDL.SiteName <> '' THEN IDL.SiteName ELSE NULL END,        
  @DropAddress = IDL.[Address],        
  @DropCity = IDL.City,        
  @DropStateCode = IDL.StateCode,        
  @DropZipCode = IDL.ZipCode,        
  @IsDropLocationAvailable = CASE WHEN IDL.[Address] IS NULL THEN @False ELSE @True END        
 FROM Invoices INV        
 JOIN Orders ORD ON INV.OrderId = ORD.Id       
 JOIN InvoiceDispatchLocations IDL ON INV.Id = IDL.InvoiceId AND IDL.LocationType = 2        
 WHERE INV.InvoiceHeaderId = @InvoiceHeaderId          
 ORDER BY IDL.Id DESC        
 ------------------- END ---------------------------------      
 ------------------- INVOICE HEADER -------------------------     
 DECLARE @PdfFooterCompanyIds TABLE (Id INT);     
 DECLARE @StrPdfFooterCompanyIds NVARCHAR(500);    
 SELECT @StrPdfFooterCompanyIds = [Description] FROM MstAppSettings WHERE [Key] = 'InvoicePdfFooter' AND IsActive = 1;    
 INSERT INTO @PdfFooterCompanyIds SELECT VALUE FROM STRING_SPLIT(@StrPdfFooterCompanyIds, ',') ORDER BY VALUE;    
 --PRINT @StrPdfFooterCompanyIds    
      
 SELECT DISTINCT TOP 1      
  ISNULL(ORD.ExternalBrokerId, ORD.BuyerCompanyId) AS BuyerCompanyId,        
  -- billing address      
  ISNULL(EBR.CompanyName, B_COM.[Name]) AS BuyerCompanyName,       
  IAD.BillingAddress,        
  IAD.BillingCity,   
  IAD.BillingStateName,  
  IAD.BillingCountryName,        
  IAD.BillingStateCode,        
  IAD.BillingZipCode,        
  -- po contact      
  IAD.PoContactName,        
  IAD.PoContactEmail,        
  IAD.PoContactPhoneNumber,  
       
  -- drop address      
  @DropAddress AS DropAddress,        
  @DropCity AS DropCity,        
  @DropStateCode AS DropStateCode,        
  @DropZipCode AS DropZipCode,        
  @IsDropLocationAvailable AS IsDropLocationAvailable,         
  --ship to      
  ISNULL(@DropSiteName, IAD.JobName) AS JobName,        
  IAD.DisplayJobID,        
  IAD.JobAddress,        
  IAD.JobCity,        
  IAD.JobStateCode,        
  IAD.JobZipCode,        
  -- supplier info      
  CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.CompanyName ELSE S_COM.[Name] END AS SupplierCompanyName,        
  CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.[Address] ELSE S_ADD.[Address] END AS SupplierAddress,        
  CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.City ELSE S_ADD.City END AS SupplierAddressCity,        
  CASE WHEN BOD.InvoicePreferenceId=2 THEN STB.Code ELSE S_MST.Code END AS SupplierAddressStateCode,        
  CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.ZipCode ELSE S_ADD.ZipCode END AS SupplierAddressZipCode,
  CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.CountryId ELSE S_ADD.CountryId END AS SupplierAddressCountryId,
  -- Commented below line for Impediment #30571 by Yash  
  --CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.PhoneNumber ELSE O_USR.PhoneNumber END AS SupplierPhoneNumber,    
  CASE WHEN BOD.InvoicePreferenceId=2 THEN EBR.PhoneNumber ELSE S_ADD.PhoneNumber END AS SupplierPhoneNumber,   
 -- INV.DisplayInvoiceNumber,      
 INV.DisplayInvoiceNumber AS DisplayInvoiceNumber,  
  INV.UpdatedDate,      
  INV.UoM,        
  INV.Currency,        
  INV.PaymentTermId,        
  TRM.[Name] AS PaymentTermName,        
  INV.NetDays,        
  FTL.Carrier,      
  INV.PaymentDueDate,      
  --LOGO.Data AS CompanyLogoData,      
  ISNULL(LOGO.Id, 0) AS CompanyLogoId,      
  LOGO.FilePath AS CompanyLogoPath,      
  IAD.IsJobSpecificBillToEnabled AS IsBillToEnabled,      
  IAD.BillToName,      
  IAD.BillToAddress,      
  IAD.BillToCity,      
  IAD.BillToZipCode,      
  IAD.BillToStateCode,      
  IAD.BillToStateName,      
  IAD.BillToCountryCode,      
  IAD.BillToCountryName,      
  IAD.CarrierOrderId,      
  SDB.AccountingCompanyId,    
   S_COM.Id AS SupplierCompanyId,    
  (SELECT [Value] FROM MstAppSettings MAS WHERE [Key] = 'InvoicePdfFooter' AND MAS.IsActive = 1 AND S_COM.Id IN(SELECT Id FROM @PdfFooterCompanyIds)) AS InvoiceFooterJson,    
  ISNULL(JBS.CountryId,0) AS JobCountryId,    
  STUFF((SELECT  ',' + ISNULL(DXT.FrDeliveryRequestId,'')    
            FROM Invoices IE    
   LEFT JOIN DeliveryScheduleXTrackableSchedules DXT ON IE.TrackableScheduleId=DXT.Id    
            WHERE  IE.InvoiceHeaderId=@InvoiceHeaderId    
    FOR XML PATH('')), 1, 1, '') AS DeliveryRequestId  ,  
 AST.Name AS Vessel,  
 OAD.Berth as Berth,  
 CASE WHEN JBS.IsMarine = 1 THEN BDR.BDRNumber ELSE '' END As BDRNumber  
 FROM InvoiceHeaderDetails IHD      
 INNER JOIN Invoices INV ON IHD.Id = INV.InvoiceHeaderId      
 LEFT JOIN InvoiceXAdditionalDetails IAD ON INV.Id = IAD.InvoiceId        
 LEFT JOIN Orders ORD ON INV.OrderId = ORD.Id        
 LEFT JOIN FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id      
 LEFT JOIN FuelRequestPricingDetails FRPD ON FRQ.Id = FRPD.FuelRequestId        
 LEFT JOIN Companies S_COM ON ORD.AcceptedCompanyId = S_COM.Id        
 LEFT JOIN CompanyAddresses S_ADD ON S_COM.Id = S_ADD.CompanyId AND S_ADD.IsDefault = 1 AND S_ADD.IsActive = 1        
 LEFT JOIN MstStates S_MST ON S_ADD.StateId = S_MST.Id       
 LEFT JOIN Companies B_COM ON ORD.BuyerCompanyId = B_COM.Id         
 LEFT JOIN ExternalBrokerOrderDetails BOD ON ORD.Id = BOD.OrderId        
 LEFT JOIN ExternalBrokers EBR ON ORD.ExternalBrokerId = EBR.Id        
 LEFT JOIN MstStates STB ON EBR.StateId = STB.Id        
 LEFT JOIN Users O_USR ON ORD.AcceptedBy = O_USR.Id        
 LEFT JOIN InvoiceXBolDetails InvBol ON InvBol.InvoiceHeaderId = IHD.Id      
 LEFT JOIN InvoiceFtlDetails FTL ON InvBol.BolDetailId = FTL.Id      
 LEFT JOIN MstPaymentTerms TRM ON INV.PaymentTermId = TRM.Id        
 LEFT JOIN Images LOGO ON S_COM.CompanyLogoId = LOGO.Id          
 LEFT JOIN SupplierXBuyerDetails SDB ON ORD.BuyerCompanyId =SDB.BuyerCompanyId AND ORD.AcceptedCompanyId = SDB.SupplierCompanyId AND  FRQ.JobId = SDB.JobId     
 LEFT JOIN DeliveryScheduleXTrackableSchedules DXT ON DXT.Id=INV.TrackableScheduleId    
 LEFT JOIN Jobs JBS ON FRQ.JobId=JBS.Id    
  LEFT JOIN OrderAdditionalDetails OAD ON OAD.OrderId = ORD.Id  
 LEFT JOIN JobXAssets JXA ON JXA.OrderId = ORD.Id  
 LEFT JOIN Assets AST ON AST.Id = JXA.AssetId  
 LEFT JOIN BDRDetails BDR ON BDR.InvoiceId = INV.Id  
 WHERE IHD.Id = @InvoiceHeaderId      
 ------------------------------- END ----------------------------      
 ---------------------------- LIFT INFO --------------------------------------      
 SELECT DISTINCT        
  FTL.LiftQuantity,      
  FTL.NetQuantity,    
  FTL.GrossQuantity,   
  FTL.DeliveredQuantity,  
  FTL.LiftTicketNumber,      
  FTL.LiftDate,      
  FTL.Carrier,      
  FTL.[SiteName] AS LocationName,      
  FTL.[Address] AS PickupAddress,        
  FTL.City AS PickupCity,        
  FTL.StateCode AS PickupStateCode,      
  FTL.CountryCode AS CountryCode,      
  FTL.ZipCode AS PickupZipCode,      
  CASE WHEN FTL.PickupLocation = 0 THEN 1 ELSE ISNULL(FTL.PickupLocation ,1) END AS PickupLocationType,      
  INV.OrderId,      
  FTL.FuelTypeId,        
  ISNULL(TFXP.NAME,MPD.Name)   AS FuelType,        
  ISNULL(FTL.TerminalName, T.Name) AS TerminalName,      
  FTL.ImageId,      
  INV.Id AS InvoiceId,    
  FTL.BadgeNumber,    
  FTL.IsBOLEditedForLfv,    
  FTL.Notes AS BolEditedNotes    
      
 FROM dbo.Invoices INV      
 INNER JOIN InvoiceHeaderDetails IH ON IH.Id = INV.InvoiceHeaderId      
 INNER JOIN InvoiceXBolDetails InvBol ON InvBol.InvoiceId = INV.Id      
 INNER JOIN InvoiceFtlDetails FTL ON InvBol.BolDetailId = FTL.Id       
 LEFT JOIN MstProducts MPD ON FTL.FuelTypeId = MPD.Id        
 LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = MPD.TfxProductId        
 LEFT JOIN MstExternalTerminals T ON FTL.TerminalId = T.Id      
 WHERE INV.InvoiceHeaderId = @InvoiceHeaderId AND FTL.LiftTicketNumber <> ''      
 ------------------------------- END ----------------------------      
 -------------------------------BOL INFO ------------------      
 SELECT DISTINCT        
   FTL.BolNumber,      
   FTL.GrossQuantity,      
   FTL.NetQuantity,    
   FTL.DeliveredQuantity,  
   FTL.Carrier,       
   FTL.[SiteName] AS LocationName,      
   FTL.[Address] AS PickupAddress,        
   FTL.City AS PickupCity,        
   FTL.StateCode AS PickupStateCode,      
   FTL.CountryCode AS CountryCode,      
   FTL.ZipCode AS PickupZipCode,      
   CASE WHEN FTL.PickupLocation = 0 THEN 1 ELSE ISNULL(FTL.PickupLocation ,1) END AS PickupLocationType,         
   FTL.FuelTypeId,      
   ISNULL(TFXP.NAME,MPD.Name)   AS FuelType,        
   ISNULL(FTL.TerminalName, T.Name) AS TerminalName,      
   INV.Id AS InvoiceId,    
   FTL.BadgeNumber,    
   FTL.IsBOLEditedForLfv,    
   FTL.Notes AS BolEditedNotes    
 FROM Invoices INV        
 INNER JOIN InvoiceHeaderDetails IH ON IH.Id = INV.InvoiceHeaderId      
 INNER JOIN InvoiceXBolDetails InvBol ON InvBol.InvoiceId = INV.Id      
 INNER JOIN InvoiceFtlDetails FTL ON InvBol.BolDetailId = FTL.Id      
 LEFT JOIN MstProducts MPD ON FTL.FuelTypeId = MPD.Id        
 LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = MPD.TfxProductId        
 LEFT JOIN MstExternalTerminals T ON FTL.TerminalId = T.Id       
 WHERE INV.InvoiceHeaderId = @InvoiceHeaderId AND FTL.BolNumber <> ''      
 ------------------------------- END ---------------------      
 ---------------------------- PICKUP LOCATION --------------------------------------      
 SELECT DISTINCT         
   ISNULL(FTL.[SiteName], FTL.TerminalName) AS LocationName,      
   FTL.[Address] AS PickupAddress,        
   FTL.City AS PickupCity,        
   FTL.StateCode AS PickupStateCode,      
   FTL.CountryCode AS CountryCode,      
   FTL.ZipCode AS PickupZipCode,      
   CASE WHEN FTL.PickupLocation = 0 THEN 1 ELSE ISNULL(FTL.PickupLocation ,1) END AS PickupLocationType,         
   FTL.FuelTypeId,      
   ISNULL(TFXP.NAME,MPD.Name)   AS FuelType,        
   ISNULL(FTL.TerminalName, T.Name) AS TerminalName      
 FROM Invoices INV        
 INNER JOIN InvoiceHeaderDetails IH ON IH.Id = INV.InvoiceHeaderId      
 INNER JOIN InvoiceXBolDetails InvBol ON InvBol.InvoiceHeaderId = IH.Id      
 INNER JOIN InvoiceFtlDetails FTL ON InvBol.BolDetailId = FTL.Id      
 LEFT JOIN MstProducts MPD ON FTL.FuelTypeId = MPD.Id        
 LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = MPD.TfxProductId       
 LEFT JOIN MstExternalTerminals T ON FTL.TerminalId = T.Id        
 WHERE INV.InvoiceHeaderId = @InvoiceHeaderId       
 ------------------------------- END ----------------------------      
 ------------------------------- INVOICE DETAILS -----------------------------------      
 SELECT DISTINCT INV.Id,        
   INV.OrderId,        
   INV.[Version],        
   INV.InvoiceVersionStatusId,        
   INV.InvoiceTypeId,        
   IH.InvoiceNumberId,        
  -- INV.DisplayInvoiceNumber,    
   INV.DisplayInvoiceNumber AS DisplayInvoiceNumber,  
   INV.ReferenceId,        
   CASE WHEN JBS.IsMarine =1 THEN ORD.PoNumber ELSE INV.PoNumber END AS PoNumber,        
   ISNULL(@DropSiteName, IAD.JobName) AS JobName,      
   INV.DropStartDate,        
   INV.DropEndDate,        
   INV.CreatedDate,        
   INV.UpdatedDate,        
   INV.DroppedGallons,        
   ISNULL(IFD.PricePerGallon, 0) AS PricePerGallon,        
   IFD.RackPrice,        
   INV.BasicAmount,        
   INV.TotalTaxAmount,        
   ISNULL(INV.TotalFeeAmount, 0) AS TotalFeeAmount,        
   INV.TotalDiscountAmount,        
   INV.PaymentDueDate,        
   INV.PaymentDate,        
   INV.IsWetHosingDelivery,        
   INV.IsOverWaterDelivery,        
   INV.QbInvoiceNumber,        
   INV.StateTax,        
   INV.FedTax,        
   INV.SalesTax,        
   INV.WaitingFor,        
   INV.ImageId,           
   IAD.SupplierAllowance,          
   ISNULL(INV.QuantityIndicatorTypeId, 1) AS QuantityIndicatorTypeId,         
   IAD.CxmlCheckOutDate,        
   IAD.Notes,      
   IAD.IsSurchargeApplicable,  
   IAD.IsFreightCostApplicable,  
   IAD.IsShowProductDescriptionOnInvoice,      
   IAD.SurchargePricingType,      
   IAD.SurchargePercentage,      
   IAD.SurchargeEIAPrice,        
   JSON_VALUE(IAD.CustomAttribute,'$.WBSNumber') AS WBSNumber,        
   Cast(IsNULL(JSON_VALUE(IAD.CustomAttribute,'$.IsTrueFillTax'), 0) AS BIT) AS IsTrueFillTax,        
   FRQ.PricingTypeId,        
   FRPD.DisplayPrice AS FuelRequestPPG,        
   INV.UoM,        
   INV.Currency,        
   MPD.ProductDisplayGroupId,        
   ISNULL(TFXP.NAME,MPD.Name)   AS ProductName,  
   ISNULL(TFXP.ProductDescription,'') AS SuperAdminProductDescription,        
   FRQ.FuelDescription AS ProductDescription,        
   ISNULL(CET.[Name], MET.[Name]) AS TerminalName,      
   ISNULL(MET.[Name],'--') AS PickupTerminal,        
   INV.PaymentTermId,        
   TRM.[Name] AS PaymentTermName,        
   INV.NetDays,        
   ORD.IsFTL,           
   CASE WHEN JBS.LocationType = 3 THEN @True ELSE @False END AS IsVariousFobOrigin,        
   JBS.IsApprovalWorkflowEnabled,        
   ISNULL(OXT.IsHidePricingEnabledForBuyer, @False) AS IsHidePricingEnabledForBuyer,        
   ISNULL(OXT.IsHidePricingEnabledForSupplier, @False) AS IsHidePricingEnabledForSupplier,        
   CASE WHEN BSD.OrderId IS NULL THEN @False ELSE @True END AS IsBuyAndSellOrder,        
   INV.IsBuyPriceInvoice,        
   ISNULL(FRQ.CreationTimeRackPPG + IFD.RackPrice, 0) AS BasePrice,        
   ISNULL(BSD.BrokerMarkUp, 0) AS BrokerMarkUp,        
   ISNULL(BSD.SupplierMarkUp, 0) AS SupplierMarkUp,        
   STN.Number AS StatementNumber,        
   STN.Id AS StatementId,       
   CASE WHEN BOD.OrderId IS NULL THEN @False ELSE @True END AS IsThirdPartyHardwareUsed,        
   CASE WHEN (SELECT TOP 1 Id FROM AssetDrops WHERE InvoiceId = INV.Id) IS NULL THEN @False ELSE @True END AS IsAssetDropAvailable,        
   FRQ.FuelTypeId,      
   MPD.ProductTypeId,      
   OInv.DisplayInvoiceNumber AS OriginalInvoiceNumber,      
   OInv.QbInvoiceNumber AS OriginalInvoiceQbNumber,      
   OIH.InvoiceNumberId AS OriginalInvoiceNumberId,      
   OInv.InvoiceTypeId AS OriginalInvoiceTypeId,         
   (SELECT TOP 1 CI.DisplayInvoiceNumber      
    FROM Invoices CI        
    INNER JOIN InvoiceXAdditionalDetails CIAD ON IAD.OriginalInvoiceId = CIAD.OriginalInvoiceId      
    WHERE CI.Id = INV.Id AND CI.InvoiceTypeId = 11) AS CreditInvoiceDisplayNumber,      
   IAD.TotalAllowance,      
   IAD.CreationMethod,      
   ISNULL(IAD.DropTicketNumber,'--') AS DropTicketNumber,         
   IXS.StatusId,      
   CASE WHEN (SELECT TOP 1 Id FROM InvoiceExceptions WHERE InvoiceId = INV.Id) IS NULL THEN @False ELSE @True END AS IsExceptionDdt ,     
   CASE WHEN ISNULL(IAD.ExceptionMessage,'') = '' THEN   IAD.DeliverySentToPDIOn    
   ELSE NULL    
   END AS PDIDetailsDate,      
   JBS.IsMarine AS IsMarineLocation,    
   INV.ConvertedQuantity,    
   INV.ConvertedPricing,    
   (CASE WHEN FRQ.UoM = 2 THEN 'Litres' WHEN FRQ.UoM = 3 THEN 'Barrel' WHEN FRQ.UoM = 4 THEN 'MetricTons' ELSE 'Gallons' END) AS FRUoM  ,  
   (CASE   
     WHEN FRQ.QuantityTypeId =1 THEN (select CONCAT(CAST(ROUND(FRQ.MaxQuantity,4) as decimal(18,4)), ''))  
  WHEN FRQ.QuantityTypeId = 2 THEN (select CONCAT(CAST(ROUND(FRQ.MinQuantity,4) as decimal(18,4)), '-', (CAST(ROUND(FRQ.MaxQuantity,4) as decimal(18,4)))))  
  WHEN FRQ.QuantityTypeId = 3 THEN 'Not Specified' END)  AS OrderQuantity,  
    LAST_VALUE(INV.DropEndDate ) OVER ( ORDER BY INV.DropEndDate ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) AS DisplayDropEndDate, -- for marine invoice,drop end date of last drop  
 LAST_VALUE(INV.DropStartDate ) OVER ( ORDER BY INV.DropStartDate DESC ROWS BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING) AS DisplayDropStartDate,-- for marine invoice, drop start date of first drop  
 INV.PaymentDueDate ,  
 ISNULL(CASE WHEN DST.DeliveryLevelPO=''THEN  '--'ELSE DST.DeliveryLevelPO END,'--') AS DeliveryLevelPO  
  FROM Invoices INV      
   JOIN InvoiceHeaderDetails IH ON INV.InvoiceHeaderId = IH.Id        
   JOIN InvoiceXAdditionalDetails IAD ON INV.Id = IAD.InvoiceId        
   JOIN dbo.InvoiceXInvoiceStatusDetails IXS ON INV.Id = IXS.InvoiceId AND IXS.IsActive = 1         
   JOIN Orders ORD ON INV.OrderId = ORD.Id        
   JOIN FuelRequests FRQ ON ORD.FuelRequestId = FRQ.Id      
   JOIN FuelRequestPricingDetails FRPD ON FRQ.Id = FRPD.FuelRequestId        
   JOIN MstProducts MPD ON FRQ.FuelTypeId = MPD.Id   
  LEFT JOIN DeliveryScheduleXTrackableSchedules DST ON DST.Id = INV.TrackableScheduleId      
   LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = MPD.TfxProductId       
   JOIN Jobs JBS ON FRQ.JobId = JBS.Id        
   JOIN Companies S_COM ON ORD.AcceptedCompanyId = S_COM.Id        
   JOIN CompanyAddresses S_ADD ON S_COM.Id = S_ADD.CompanyId AND S_ADD.IsDefault = 1 AND S_ADD.IsActive = 1        
   JOIN MstStates S_MST ON S_ADD.StateId = S_MST.Id        
   JOIN Companies B_COM ON ORD.BuyerCompanyId = B_COM.Id        
   JOIN MstPaymentTerms TRM ON INV.PaymentTermId = TRM.Id        
   JOIN Users O_USR ON ORD.AcceptedBy = O_USR.Id        
   LEFT JOIN Invoices OInv ON IAD.OriginalInvoiceId = OInv.Id      
   LEFT JOIN InvoiceHeaderDetails OIH ON OInv.InvoiceHeaderId = OIH.Id        
   LEFT JOIN MstExternalTerminals MET ON INV.TerminalId = MET.Id        
   LEFT JOIN ExternalBrokerOrderDetails BOD ON ORD.Id = BOD.OrderId        
   LEFT JOIN ExternalBrokerBuySellDetails BSD ON ORD.Id = BSD.OrderId        
   LEFT JOIN ExternalBrokers EBR ON ORD.ExternalBrokerId = EBR.Id        
   LEFT JOIN MstStates STB ON EBR.StateId = STB.Id        
   LEFT JOIN InvoiceXBolDetails INVBOL ON INV.Id=INVBOL.InvoiceId      
   LEFT JOIN dbo.InvoiceFtlDetails IFD ON IFD.Id = INVBOL.BolDetailId       
    LEFT JOIN MstExternalTerminals CET ON IFD.CityGroupTerminalId = CET.Id        
   LEFT JOIN OrderXTogglePricingDetails OXT ON ORD.Id = OXT.OrderId AND INV.InvoiceTypeId IN(6,7)        
   LEFT JOIN BillingStatementXInvoices BXI ON INV.Id = BXI.InvoiceId AND BXI.IsActive = 1        
   LEFT JOIN BillingStatements BST ON BXI.StatementId = BST.Id AND BST.IsGenerated = 1 AND BST.BillingScheduleId IS NOT NULL        
   LEFT JOIN StatementNumbers STN ON BST.StatementNumberId = STN.Id           
 WHERE INV.InvoiceHeaderId = @InvoiceHeaderId        
 ------------------------------- END ---------------------      
 EXEC [dbo].[usp_GetConsolidatedInvoicePdfFuelFees] @InvoiceHeaderId      
 EXEC [dbo].[usp_GetConsolidatedInvoicePdfTaxes] @InvoiceHeaderId      
 EXEC [dbo].[usp_GetConsolidatedPdfSpecialInstructions] @InvoiceHeaderId      
 EXEC [dbo].[usp_GetConsolidatedInvoicePdfAssetDrops] @InvoiceHeaderId      
END    
GO

-- =============================================    
-- Author:  Rahul S    
-- Create date: 26-Jul-2019    
-- Description: Returns fuel fees added in the invoice    
-- =============================================    
-- EXEC [dbo].[usp_GetConsolidatedInvoicePdfFuelFees] 33892  
CREATE OR ALTER PROCEDURE [dbo].[usp_GetConsolidatedInvoicePdfFuelFees]    
 @InvoiceHeaderId INT    
AS    
BEGIN    
 SELECT DISTINCT FFS.FeeTypeId,    
   FFS.FeeSubTypeId,    
   FFS.OtherFeeTypeId,    
   MFT.[Name] AS FeeTypeName,    
   CASE WHEN FFS.FeeTypeId <> 25 THEN ISNULL(MOT.[Name], FFS.FeeDetails)    
   WHEN FFS.FeeTypeId = 25 AND IAD.TankFrequencyId = 1 THEN FFS.FeeDetails + ' - Daily'    
   WHEN FFS.FeeTypeId = 25 AND IAD.TankFrequencyId = 2 THEN FFS.FeeDetails + ' - Weekly'    
   WHEN FFS.FeeTypeId = 25 AND IAD.TankFrequencyId = 3 THEN FFS.FeeDetails + ' - Biweekly'    
   WHEN FFS.FeeTypeId = 25 AND IAD.TankFrequencyId = 4 THEN FFS.FeeDetails + ' - Monthly'    
   ELSE ISNULL(MOT.[Name], FFS.FeeDetails) END AS OtherFeeName,    
   MSF.[Name] AS FeeSubTypeName,    
   FFS.Fee,    
   FFS.FeeSubQuantity,    
   FFS.TotalFee,    
   FFS.IncludeInPPG,    
   FFS.MinimumGallons,    
   FFS.FeeConstraintTypeId,    
   FFS.SpecialDate,    
   FFS.DiscountLineItemId,    
   MOT.Code AS OtherFeeCode,    
   FBQ.Id AS FeeByQuantityId,    
   FBQ.FeeTypeId AS FeeByQuantityTypeId,    
   FBQ.FeeSubTypeId AS FeeByQuantitySubTypeId,    
   FBQ.MinQuantity AS FeeByQuantityMinQuantity,    
   FBQ.MaxQuantity AS FeeByQuantityMaxQuantity,    
   FBQ.Fee AS FeeByQuantityFee,    
   FFS.UoM,    
   FFS.Currency,    
   FFS.StartTime,    
   FFS.EndTime,    
   FFS.WaiveOffTime,    
   MFT.TruckLoadCategoryId,  
   INV.Id AS InvoiceId,  
   INV.InvoiceTypeId,  
   INV.DroppedGallons,  
   IAD.IsSurchargeApplicable,  
 IAD.SurchargePricingType,  
 IAD.SurchargePercentage,  
 IAD.SurchargeEIAPrice,  
 FFS.Id AS FuelFeesId,  
 IAD.IsFreightCostApplicable,  
 IAD.FreightRateRuleType,
 IAD.Distance
 FROM Invoices INV    
 JOIN InvoiceHeaderDetails IH ON INV.InvoiceHeaderId = IH.Id    
 JOIN InvoiceXAdditionalDetails IAD ON INV.Id = IAD.InvoiceId    
 JOIN InvoiceXFees IXF ON INV.Id = IXF.InvoiceId    
 JOIN FuelFees FFS ON IXF.FuelRequestFeeId = FFS.Id    
 JOIN MstFeeTypes MFT ON FFS.FeeTypeId = MFT.Id    
 JOIN MstFeeSubTypes MSF ON FFS.FeeSubTypeId = MSF.Id     
 LEFT JOIN MstOtherFeeTypes MOT ON FFS.OtherFeeTypeId = MOT.Id    
 LEFT JOIN FeeByQuantities FBQ ON FFS.Id = FBQ.FuelFeesId     
  AND Abs(INV.DroppedGallons) > Abs(FBQ.MinQuantity)  
  AND Abs(INV.DroppedGallons) <= ISNULL(Abs(FBQ.MaxQuantity), Abs(INV.DroppedGallons))    
 WHERE INV.InvoiceHeaderId = @InvoiceHeaderId AND FFS.FeeTypeId != 4 AND FFS.TotalFee <> 0    
END  
GO
CREATE OR ALTER   PROCEDURE [dbo].[usp_GetSupplierOrderDetail] --EXEC [usp_GetSupplierOrderDetail] 8835,0,22            
 @OrderId INT,            
 @IsBrokeredOrder BIT = 0,            
 @CompanyId INT             
AS            
BEGIN            
DECLARE @ScheduleId INT = 0            
DECLARE @ScheduleName NVARCHAR(256) = ''            
DECLARE @True BIT = 1;            
DECLARE @False BIT = 0;            
DECLARE @IsBrokerVisible BIT = 1;        
DECLARE @IsCustomerContactExists BIT = 0;
IF EXISTS (SELECT 1 FROM OrderXUsers where OrderId = @OrderId)
BEGIN
SET @IsCustomerContactExists = 1;
END
DECLARE @ProductMappingDetail TABLE       
(      
 [MappingId]    INT,      
 [MyProductId]   NVARCHAR(500),      
 [BackOfficeProductId] NVARCHAR(500),      
 [DriverProductId]       NVARCHAR(500),      
 [CompanyId]    INT      
)      
INSERT INTO @ProductMappingDetail SELECT * FROM [dbo].[usf_GetAssignedProductMappings](@OrderId);      
IF EXISTS (SELECT 1 FROM Orders O INNER JOIN FuelRequests fr ON O.FuelRequestId = FR.id            
INNER JOIN FuelRequests CFR ON CFR.ParentId = FR.Id AND (CFR.FuelRequestTypeId = 3  OR CFR.FuelRequestTypeId = 7)         
INNER JOIN FuelRequestXStatuses S ON CFR.Id = S.FuelRequestId AND S.StatusId IN (2,3,6) AND S.IsActive = 1       
LEFT JOIN Orders COD ON COD.FuelRequestId = CFR.Id      
LEFT JOIN OrderXStatuses COST ON COD.Id = COST.OrderId AND COST.IsActive = 1           
WHERE O.Id = @OrderId AND (COD.Id is null OR COST.StatusId IN (1,4,5)))            
BEGIN            
SET @IsBrokerVisible = 0;            
END            
SELECT @ScheduleId = S.Id, @ScheduleName = S.BillingStatementId FROM  BillingScheduleXCustomerOrders O INNER JOIN BillingSchedules S ON O.BillingScheduleId = S.Id WHERE o.OrderId = @OrderId AND O.IsActive = 1 AND S.IsActive =1             
 ;WITH OrderDetailCTE AS(            
 SELECT            
  O.BuyerCompanyId,            
  O.Id,        
  O.[Name] as OrderName,         
  O.FuelRequestId,            
  J.Id AS JobId,            
  J.DisplayJobID,            
  J.StateId,            
  O.PoNumber,            
  COALESCE(O.TerminalId,FR.TerminalId,0) AS TerminalId,            
  ISNULL(O.CityGroupTerminalId, 0) AS CityGroupTerminalId,            
  ISNULL(T.Name,'--') AS TerminalName,            
  ISNULL(P.DisplayName,P.Name) AS FuelType,            
  JB.IsTaxExempted,            
  O.IsProFormaPo,            
  J.CompanyId AS JobCompanyId,            
  ISNULL(O.BrokeredMaxQuantity , FR.MaxQuantity) AS GallonsOrdered,            
  J.EndDate AS JobEndDate,            
  O.IsEndSupplier,      
  @IsCustomerContactExists AS IsCustomerContactExists,
  CASE WHEN O.DefaultInvoiceType = 2 THEN @True ELSE @False END AS IsDefaultInvoiceTypeManual,            
  FR.EstimateGallonsPerDelivery,            
  P.ProductDisplayGroupId AS TypeOfFuel,            
  P.ProductTypeId,            
  FR.FuelDescription AS ProductDescription,            
  FR.FuelTypeId,            
  P.TfxProductId AS TfxFuelTypeId,            
  --FR.PricingTypeId,            
  FR.OrderClosingThreshold,            
  J.Name AS JobName,            
  J.LocationType,            
  J.[Address], 
  J.AddressLine2, 
  J.AddressLine3,           
  J.City ,            
  MST.Code AS StateCode,            
  J.ZipCode ,            
  J.CountryId,            
  CNTRY.Code AS CountryCode,            
  FR.Currency,            
  O.IsFTL,            
  O.BrokeredMaxQuantity,            
  FR.UoM,            
  J.TimeZoneName,            
  --ISNULL(FR.SupplierCostTypeId, 1) AS SupplierCostTypeId,            
  --FR.SupplierCost,            
  --CASE WHEN O.IsActive = 1 AND FR.PricingTypeId = 4 AND FR.SupplierCost IS NOT NULL THEN @True ELSE @False END AS CanCreateInvoice,            
  OST.StatusId AS OrderStatus,            
  OST.UpdatedBy AS StatusUpdatedBy,            
  D.DriverId,            
  CASE WHEN DU.Id IS NULL THEN 'No Driver Assigned' ELSE DU.FirstName + ' ' +DU.LastName END AS DriverName,            
  FR.QuantityTypeId,            
  FR.FreightOnBoardTypeId,            
  FRP.FuelRequestId AS FrPricingDetailId,            
  ISNULL(FRD.PricingQuantityIndicatorTypeId,1) AS PricingQuantityIndicatorTypeId,            
  CASE WHEN CT.Id IS NOT NULL THEN CT.Name + ', ' + CT.StateCode ELSE CT.Name END AS CityGroupTerminalName,            
  --FRP.PricingSourceId,            
  --FRP.FeedTypeId,            
  --FRP.PricingSourceQuantityIndicatorTypeId,            
  --FRP.WeekendDropPricingDay,            
  --FRP.FuelClassTypeId,            
  FR.PaymentTermId AS FrPaymentTermId,            
  FR.NetDays AS FrNetDays,    
  OAD.OrderId AS AdditionalDetailId,            
  OAD.Allowance,            
  OAD.BOLInvoicePreferenceId,            
  OAD.IsDriverToUpdateBOL,            
  FRD.IsBolImageRequired,            
  FRD.IsDropImageRequired,            
  OAD.CarrierId,            
  OAD.LoadCode,            
  OAD.Notes,            
  OAD.SupplierContract,            
  CARR.Name AS CarrierName,            
  OAD.SupplierSourceId,            
  SS.Name AS SupplierSourceName,            
  FRD.DeliveryTypeId,      
  TP.OrderId AS OrderXTogglePricingDetailId,            
  TP.IsHidePricingEnabledForBuyer,            
  TP.IsHidePricingEnabledForSupplier,            
  V.PaymentTermId,            
  V.NetDays,            
  V.PaymentMethod,            
  PT.Name AS PaymentTermName,            
  BSD.OrderId AS ExternalBrokerBuySellDetailId,            
  BSD.ExternalBrokerId AS BuySellBrokerId,            
  O.ExternalBrokerId,            
  BSD.BrokerMarkUp,            
  BSD.SupplierMarkUp,            
  EBOD.ThirdPartyNozzleId,            
  EBOD.VendorId,            
  EBOD.CustomerNumber,            
  EBOD.ShipTo,            
  EBOD.Source,            
  EBOD.ProductCode,            
  EBOD.OrderId AS ExternalBrokerOrderDetailId,            
  EBOD.InvoicePreferenceId,            
  FR.MinQuantity,            
  FR.MaxQuantity,            
  FRD.StartDate,            
  FRD.EndDate,            
  FRD.StartTime,            
  FRD.EndTime,            
  FRD.PoContactId,            
  FRD.CustomAttribute,      
  FRD.OrderEnforcementId,      
  FRD.IsPrePostDipRequired,      
  DT.Name AS DeliveryTypeName,            
  CASE WHEN EXISTS (SELECT 1 FROM AssetDrops WHERE OrderId = @OrderId AND IsActive = 1) THEN @True ELSE @False END AS IsAssetHistoryAvailable,            
  CASE WHEN EXISTS (SELECT 1 FROM Invoices WHERE InvoiceTypeId != 5 AND InvoiceVersionStatusId = 1 AND OrderId = @OrderId) THEN @True ELSE @False END AS AnyInvoiceExists,            
  (    
 CASE WHEN J.IsMarine = 1 AND (FR.UoM = 3 OR FR.UoM = 4) THEN     
  CAST((SELECT ISNULL((SUM(INV.ConvertedQuantity)/ISNULL(O.[BrokeredMaxQuantity], CASE WHEN FR.MaxQuantity = 0 THEN 1 ELSE FR.MaxQuantity END) * 100), 0) FROM dbo.Invoices INV             
  WHERE INV.InvoiceVersionStatusId = 1 AND INV.IsActive = 1 AND INV.IsBuyPriceInvoice = 0 AND INV.OrderId = @OrderId) AS DECIMAL(18,2))    
 ELSE    
  CAST((SELECT ISNULL((SUM(INV.DroppedGallons)/ISNULL(O.[BrokeredMaxQuantity], CASE WHEN FR.MaxQuantity = 0 THEN 1 ELSE FR.MaxQuantity END) * 100), 0) FROM dbo.Invoices INV             
  WHERE INV.InvoiceVersionStatusId = 1 AND INV.IsActive = 1 AND INV.IsBuyPriceInvoice = 0 AND INV.OrderId = @OrderId) AS DECIMAL(18,2))      
 END    
 ) AS FuelDeliveredPercentage,            
  --CASE WHEN FR.PricingTypeId <> 2 OR FR.QuantityTypeId = 3 THEN null ELSE (ISNULL(O.[BrokeredMaxQuantity], FR.MaxQuantity) * FR.PricePerGallon) END AS OrderTotalAmount,            
  CASE WHEN PricingTypeId = 4 THEN dbo.usf_GetGlobalFuelCost(o.AcceptedCompanyId, P.TfxProductId, J.StateId, FR.Currency,FR.UoM) ELSE null END AS GlobalFuelCost,            
  --dbo.[usf_GetPricePerGallon](FR.PricePerGallon, FR.PricingTypeId, FR.RackAvgTypeId) AS DisplayPricePerGallon,            
  FRP.DisplayPrice AS DisplayPricePerGallon,            
  --FR.PricePerGallon,            
  --FR.RackAvgTypeId,            
  CASE WHEN CO.ID IS NULL THEN U.Id ELSE CU.Id END AS CustomerId,            
  CASE WHEN CO.ID IS NULL THEN U.Email ELSE CU.Email END AS CustomerEmail,            
  CASE WHEN CO.ID IS NULL THEN U.PhoneNumber ELSE CU.PhoneNumber END AS CustomerPhoneNumber,            
  C.Name AS CustomerCompany,            
  CASE WHEN CO.ID IS NULL THEN U.FirstName ELSE CU.FirstName END AS CustomerFirstName,             
  CASE WHEN CO.ID IS NULL THEN U.LastName ELSE CU.LastName END AS CustomerLastName,            
  SU.FirstName + ' ' + SU.LastName AS SupplierName,            
  SU.Email AS SupplierEmail,            
  SU.PhoneNumber AS SupplierPhoneNumber,            
  S.Name AS SupplierCompany,            
  ISNULL(BSEB.CompanyName, EB.CompanyName) AS ExternalBrokerCompany,            
  CASE WHEN @IsBrokeredOrder = 0 THEN  [dbo].[usf_GetCancelledParentOrder](O.Id) ELSE O.Id   END AS ParentOrderId,            
  CASE WHEN FR.FuelRequestTypeId = 2 THEN dbo.usf_GetParentFuelRequestTypeId(FR.Id) ELSE FR.FuelRequestTypeId END AS FuelRequestTypeId,            
  OAD.IsFuelSurcharge,
  OAD.IsFreightCost,
  OAD.FuelSurchagePricingType,            
  OAD.FileDetails,            
  OAD.PreferencesSettingId,--Added by Pravin for preference setting           
  --SupplierAssignedProductName = dbo.usf_GetSupplierAssignedProductName(@OrderId), --Added by Sharique for displaying the assign product name.          
  PMD.MyProductId,      
  PMD.DriverProductId,      
  PMD.BackOfficeProductId,         
  O.SignatureEnabled AS IsSignatureEnabled,            
  FRP.RequestPriceDetailId,            
  FRP.DisplayPriceCode AS PricingCodeDescription,              
  o.AcceptedCompanyId,  
  OAD.IsIncludePricingInExternalObj AS IsIncludePricing,  
  O.LeadRequestId,  
  O.IsActive,            
  FRD.IsDriverToUpdateBOL as FrIsDriverToUpdateBOL,            
  FRD.TruckLoadTypeId,            
  ORDGRP.GroupPoNumber,            
  ORDGRP.OrderGroupId,            
  FDL.Address AS PickupAddress,            
  FDL.City AS PickupCity,            
  FDL.StateCode AS PickupStateCode,            
  FDL.ZipCode AS PickupZipCode,     
  FDL.BulkPlantId,
  J.IsBillToEnabled,            
  COALESCE(JBA.Name,DF_BA.Name) AS BillToName,            
  COALESCE(JBA.Address,DF_BA.Address) AS BillToAddress,          
  COALESCE(JBA.AddressLine2,DF_BA.AddressLine2) AS BillToAddressLine2,
  COALESCE(JBA.AddressLine3,DF_BA.AddressLine3) AS BillToAddressLine3,  
  COALESCE(JBA.City,DF_BA.City) AS BillToCity,            
  COALESCE(JBA.ZipCode,DF_BA.ZipCode) BillToZipCode,            
  BILLTOMST.Id AS BillToStateId,            
  BILLTOMST.Code AS BillToStateCode,            
  BILLTOCNTRY.Id as BillToCountryId,            
  BILLTOCNTRY.Code as BillToCountryCode,   
  COALESCE(JBA.County,DF_BA.County) AS BillToCounty,
  COALESCE(JBA.StateName,DF_BA.StateName) AS BillToStateName,
  COALESCE(JBA.CountryName,DF_BA.CountryName) AS BillToCountryName, 
  COALESCE(JBA.Id,DF_BA.Id) AS BillingAddressId,         
  J.SiteInstructions,          
  J.IsRetailJob,     
  J.IsMarine as IsMarineLocation,  
  J.Latitude as Latitude,  
  J.Longitude as Longitude,  
  CASE WHEN EXISTS (SELECT 1 FROM JobCarrierDetails WHERE JobId = J.Id) THEN [dbo].[usf_GetJobCarrierDetail](J.Id,@CompanyId) ELSE null END AS CarrierCompanyName,      
  CAST(Case WHEN (FR.FuelRequestTypeId = 3 AND FRD.IsDispatchRetainedByCustomer =1) THEN 1 ELSE 0 END AS BIT) AS IsDispatchRetainedByCustomer,  
  JXA.AssetId AS VessleId,  
  OAD.Berth AS Berth,  
  AAD.IMONumber AS IMONumber,  
  AAD.Flag AS Flag,  
  JXA.Id AS JobXAssetId,  
  AST.[Name] AS Vessel,  
  OAD.IsManualBDNConfirmationRequired AS IsBDNConfirmationRequired,  
  OAD.IsManualInvoiceConfirmationRequired AS IsInvoiceConfirmationRequired,
  OAD.IsSupressPricingEnabled AS IsSupressOrderPricing,
  OAD.IsPDITaxRequired AS IsPdiTaxRequired,
  OAD.FreightPricingMethod,	
  OAD.FreightRateRuleType,	
  OAD.FreightRateTableType,	
  OAD.FreightRateRuleId,	
  OAD.FuelSurchargeTableType,	
  OAD.FuelSurchargeTableId,	
  OAD.AccessorialFeeTableType,	
  OAD.AccessorialFeeId
  FROM Orders O            
 INNER JOIN FuelRequests FR ON O.FuelRequestId = FR.Id            
 INNER JOIN Users U ON FR.CreatedBy = U.Id            
 INNER JOIN Companies C ON o.BuyerCompanyId = C.Id            
 INNER JOIN Users SU ON O.AcceptedBy = SU.Id            
 INNER JOIN Companies S ON SU.CompanyId = S.Id            
 INNER JOIN Jobs J ON FR.JobId = J.Id            
 INNER JOIN JobBudgets JB ON J.JobBudgetId = JB.Id            
 INNER JOIN MstStates MST ON J.StateId = MST.Id            
 INNER JOIN MstCountries CNTRY ON J.CountryId = CNTRY.Id            
 INNER JOIN MstProducts P ON FR.FuelTypeId = P.Id            
 LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = P.TfxProductId           
 INNER JOIN FuelRequestDetails FRD ON FR.Id = FRD.FuelRequestId            
 INNER JOIN MstDeliveryTypes DT ON FRD.DeliveryTypeId = DT.Id            
 INNER JOIN OrderXStatuses OST ON O.Id = OST.OrderId AND OST.IsActive = 1             
 LEFT JOIN MstExternalTerminals T ON O.TerminalId = T.Id            
 LEFT JOIN OrderXDrivers D ON O.Id = D.OrderId AND D.IsActive = 1            
 LEFT JOIN Users DU ON D.DriverId = DU.Id            
 LEFT JOIN FuelRequestPricingDetails FRP ON FR.Id = FRP.FuelRequestId             LEFT JOIN MstExternalTerminals CT ON FR.CityGroupTerminalId = CT.Id            
 LEFT JOIN OrderAdditionalDetails OAD ON O.Id = OAD.OrderId            
 LEFT JOIN Carriers CARR ON OAD.CarrierId = CARR.Id            
 LEFT JOIN SupplierSources SS ON OAD.SupplierSourceId = SS.Id            
 LEFT JOIN OrderGroupXOrders ORDGRP ON O.Id = ORDGRP.OrderId AND ORDGRP.IsActive = 1            
 LEFT JOIN ExternalBrokerBuySellDetails BSD ON O.Id = BSD.OrderId            
 LEFT JOIN ExternalBrokerOrderDetails EBOD ON O.Id = EBOD.OrderId            
 LEFT JOIN ExternalBrokers EB ON O.ExternalBrokerId = EB.Id            
 LEFT JOIN ExternalBrokers BSEB ON BSD.ExternalBrokerId = BSEB.Id            
 LEFT JOIN OrderDetailVersions V ON O.Id = V.OrderId AND V.IsActive = 1            
 LEFT JOIN MstPaymentTerms PT ON V.PaymentTermId = PT.Id            
 LEFT JOIN OrderXTogglePricingDetails TP ON O.Id = TP.OrderId            
 LEFT JOIN CounterOffers CO ON FR.Id = CO.FuelRequestId AND (CO.BuyerStatus = 2 OR CO.SupplierStatus = 2)            
 LEFT JOIN Users CU ON CO.BuyerId = CU.Id            
 LEFT JOIN FuelDispatchLocations FDL ON O.Id = FDL.OrderId AND FDL.IsActive = 1 AND FDL.DeliveryScheduleId IS NULL AND FDL.TrackableScheduleId IS NULL AND FDL.LocationType = 1            
 LEFT JOIN MstStates BILLTOMST ON J.BillToStateId = BILLTOMST.Id            
 LEFT JOIN MstCountries BILLTOCNTRY ON J.BillToCountryId = BILLTOCNTRY.Id        
 LEFT JOIN SupplierMappedProductDetails SMP ON SMP.IsActive = 1 AND O.AcceptedCompanyId = SMP.CompanyId AND SMP.TerminalId = T.Id AND SMP.FuelTypeId = P.TfxProductId      
 LEFT JOIN @ProductMappingDetail PMD ON SMP.Id = PMD.MappingId      
 LEFT JOIN JobXAssets JXA ON JXA.OrderId = O.Id AND JXA.RemovedBy IS NULL  
 LEFT JOIN AssetAdditionalDetails AAD ON JXA.AssetId = AAD.AssetId  
 LEFT JOIN Assets AST ON JXA.AssetId = AST.Id AND AST.IsActive = 1
 LEFT JOIN BillingAddresses DF_BA(NOLOCK) ON O.BuyerCompanyId = DF_BA.CompanyId AND DF_BA.IsActive = 1 AND DF_BA.IsDefault = 1
 LEFT JOIN BillingAddresses JBA ON J.BillingAddressId = JBA.Id  
 WHERE O.Id = @OrderId            
 )           
 SELECT (SELECT COUNT(JXA.Id) FROM JobXAssets JXA INNER JOIN Assets A ON JXA.AssetId = A.Id AND A.IsActive = 1             
   WHERE JXA.JobId = (SELECT DISTINCT JobId FROM OrderDetailCTE) AND JXA.RemovedBy IS NULL) AS AssignedAssetCount ,            
 @ScheduleId AS ScheduleId,            
 @ScheduleName AS ScheduleName,            
 @IsBrokerVisible AS IsBrokerVisible,            
 CASE WHEN (@IsBrokeredOrder = 1 OR ((FuelRequestTypeId = 3 OR  FuelRequestTypeId = 7)AND BuyerCompanyId = @CompanyId)) AND OrderStatus IN (1,4,5) THEN dbo.usf_GetBrokeredOrderStatusId(Id, OrderStatus, @CompanyId) ELSE OrderStatus END AS StatusId,  *     
 FROM OrderDetailCTE            
END  
Go

GO
/****** Object:  StoredProcedure [dbo].[usp_GetSupplierOrderDetail]    Script Date: 04/29/2022 12:50:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER     PROCEDURE [dbo].[usp_GetSupplierOrderDetail] --EXEC [usp_GetSupplierOrderDetail] 8835,0,22            
 @OrderId INT,            
 @IsBrokeredOrder BIT = 0,            
 @CompanyId INT             
AS            
BEGIN            
DECLARE @ScheduleId INT = 0            
DECLARE @ScheduleName NVARCHAR(256) = ''            
DECLARE @True BIT = 1;            
DECLARE @False BIT = 0;            
DECLARE @IsBrokerVisible BIT = 1;        
DECLARE @IsCustomerContactExists BIT = 0;
IF EXISTS (SELECT 1 FROM OrderXUsers where OrderId = @OrderId)
BEGIN
SET @IsCustomerContactExists = 1;
END
DECLARE @ProductMappingDetail TABLE       
(      
 [MappingId]    INT,      
 [MyProductId]   NVARCHAR(500),      
 [BackOfficeProductId] NVARCHAR(500),      
 [DriverProductId]       NVARCHAR(500),      
 [CompanyId]    INT      
)      
INSERT INTO @ProductMappingDetail SELECT * FROM [dbo].[usf_GetAssignedProductMappings](@OrderId);      
IF EXISTS (SELECT 1 FROM Orders O INNER JOIN FuelRequests fr ON O.FuelRequestId = FR.id            
INNER JOIN FuelRequests CFR ON CFR.ParentId = FR.Id AND (CFR.FuelRequestTypeId = 3  OR CFR.FuelRequestTypeId = 7)         
INNER JOIN FuelRequestXStatuses S ON CFR.Id = S.FuelRequestId AND S.StatusId IN (2,3,6) AND S.IsActive = 1       
LEFT JOIN Orders COD ON COD.FuelRequestId = CFR.Id      
LEFT JOIN OrderXStatuses COST ON COD.Id = COST.OrderId AND COST.IsActive = 1           
WHERE O.Id = @OrderId AND (COD.Id is null OR COST.StatusId IN (1,4,5)))            
BEGIN            
SET @IsBrokerVisible = 0;            
END            
SELECT @ScheduleId = S.Id, @ScheduleName = S.BillingStatementId FROM  BillingScheduleXCustomerOrders O INNER JOIN BillingSchedules S ON O.BillingScheduleId = S.Id WHERE o.OrderId = @OrderId AND O.IsActive = 1 AND S.IsActive =1             
 ;WITH OrderDetailCTE AS(            
 SELECT            
  O.BuyerCompanyId,            
  O.Id,        
  O.[Name] as OrderName,         
  O.FuelRequestId,            
  J.Id AS JobId,            
  J.DisplayJobID,            
  J.StateId,            
  O.PoNumber,            
  COALESCE(O.TerminalId,FR.TerminalId,0) AS TerminalId,            
  ISNULL(O.CityGroupTerminalId, 0) AS CityGroupTerminalId,            
  ISNULL(T.Name,'--') AS TerminalName,            
  ISNULL(P.DisplayName,P.Name) AS FuelType,            
  JB.IsTaxExempted,            
  O.IsProFormaPo,            
  J.CompanyId AS JobCompanyId,            
  ISNULL(O.BrokeredMaxQuantity , FR.MaxQuantity) AS GallonsOrdered,            
  J.EndDate AS JobEndDate,            
  O.IsEndSupplier,      
  @IsCustomerContactExists AS IsCustomerContactExists,
  CASE WHEN O.DefaultInvoiceType = 2 THEN @True ELSE @False END AS IsDefaultInvoiceTypeManual,            
  FR.EstimateGallonsPerDelivery,            
  P.ProductDisplayGroupId AS TypeOfFuel,            
  P.ProductTypeId,            
  FR.FuelDescription AS ProductDescription,            
  FR.FuelTypeId,            
  P.TfxProductId AS TfxFuelTypeId,            
  --FR.PricingTypeId,            
  FR.OrderClosingThreshold,            
  J.Name AS JobName,            
  J.LocationType,            
  J.[Address], 
  J.AddressLine2, 
  J.AddressLine3,           
  J.City ,            
  MST.Code AS StateCode,            
  J.ZipCode ,            
  J.CountryId,            
  CNTRY.Code AS CountryCode,            
  FR.Currency,            
  O.IsFTL,            
  O.BrokeredMaxQuantity,            
  FR.UoM,            
  J.TimeZoneName,            
  --ISNULL(FR.SupplierCostTypeId, 1) AS SupplierCostTypeId,            
  --FR.SupplierCost,            
  --CASE WHEN O.IsActive = 1 AND FR.PricingTypeId = 4 AND FR.SupplierCost IS NOT NULL THEN @True ELSE @False END AS CanCreateInvoice,            
  OST.StatusId AS OrderStatus,            
  OST.UpdatedBy AS StatusUpdatedBy,            
  D.DriverId,            
  CASE WHEN DU.Id IS NULL THEN 'No Driver Assigned' ELSE DU.FirstName + ' ' +DU.LastName END AS DriverName,            
  FR.QuantityTypeId,            
  FR.FreightOnBoardTypeId,            
  FRP.FuelRequestId AS FrPricingDetailId,            
  ISNULL(FRD.PricingQuantityIndicatorTypeId,1) AS PricingQuantityIndicatorTypeId,            
  CASE WHEN CT.Id IS NOT NULL THEN CT.Name + ', ' + CT.StateCode ELSE CT.Name END AS CityGroupTerminalName,            
  --FRP.PricingSourceId,            
  --FRP.FeedTypeId,            
  --FRP.PricingSourceQuantityIndicatorTypeId,            
  --FRP.WeekendDropPricingDay,            
  --FRP.FuelClassTypeId,            
  FR.PaymentTermId AS FrPaymentTermId,            
  FR.NetDays AS FrNetDays,    
  OAD.OrderId AS AdditionalDetailId,            
  OAD.Allowance,            
  OAD.BOLInvoicePreferenceId,            
  OAD.IsDriverToUpdateBOL,            
  FRD.IsBolImageRequired,            
  FRD.IsDropImageRequired,            
  OAD.CarrierId,            
  OAD.LoadCode,            
  OAD.Notes,            
  OAD.SupplierContract,            
  CARR.Name AS CarrierName,            
  OAD.SupplierSourceId,            
  SS.Name AS SupplierSourceName,            
  FRD.DeliveryTypeId,      
  TP.OrderId AS OrderXTogglePricingDetailId,            
  TP.IsHidePricingEnabledForBuyer,            
  TP.IsHidePricingEnabledForSupplier,            
  V.PaymentTermId,            
  V.NetDays,            
  V.PaymentMethod,            
  PT.Name AS PaymentTermName,            
  BSD.OrderId AS ExternalBrokerBuySellDetailId,            
  BSD.ExternalBrokerId AS BuySellBrokerId,            
  O.ExternalBrokerId,            
  BSD.BrokerMarkUp,            
  BSD.SupplierMarkUp,            
  EBOD.ThirdPartyNozzleId,            
  EBOD.VendorId,            
  EBOD.CustomerNumber,            
  EBOD.ShipTo,            
  EBOD.Source,            
  EBOD.ProductCode,            
  EBOD.OrderId AS ExternalBrokerOrderDetailId,            
  EBOD.InvoicePreferenceId,            
  FR.MinQuantity,            
  FR.MaxQuantity,            
  FRD.StartDate,            
  FRD.EndDate,            
  FRD.StartTime,            
  FRD.EndTime,            
  FRD.PoContactId,            
  FRD.CustomAttribute,      
  FRD.OrderEnforcementId,      
  FRD.IsPrePostDipRequired,      
  DT.Name AS DeliveryTypeName,            
  CASE WHEN EXISTS (SELECT 1 FROM AssetDrops WHERE OrderId = @OrderId AND IsActive = 1) THEN @True ELSE @False END AS IsAssetHistoryAvailable,            
  CASE WHEN EXISTS (SELECT 1 FROM Invoices WHERE InvoiceTypeId != 5 AND InvoiceVersionStatusId = 1 AND OrderId = @OrderId) THEN @True ELSE @False END AS AnyInvoiceExists,            
  (    
 CASE WHEN J.IsMarine = 1 AND (FR.UoM = 3 OR FR.UoM = 4) THEN     
  CAST((SELECT ISNULL((SUM(INV.ConvertedQuantity)/ISNULL(O.[BrokeredMaxQuantity], CASE WHEN FR.MaxQuantity = 0 THEN 1 ELSE FR.MaxQuantity END) * 100), 0) FROM dbo.Invoices INV             
  WHERE INV.InvoiceVersionStatusId = 1 AND INV.IsActive = 1 AND INV.IsBuyPriceInvoice = 0 AND INV.OrderId = @OrderId) AS DECIMAL(18,2))    
 ELSE    
  CAST((SELECT ISNULL((SUM(INV.DroppedGallons)/ISNULL(O.[BrokeredMaxQuantity], CASE WHEN FR.MaxQuantity = 0 THEN 1 ELSE FR.MaxQuantity END) * 100), 0) FROM dbo.Invoices INV             
  WHERE INV.InvoiceVersionStatusId = 1 AND INV.IsActive = 1 AND INV.IsBuyPriceInvoice = 0 AND INV.OrderId = @OrderId) AS DECIMAL(18,2))      
 END    
 ) AS FuelDeliveredPercentage,            
  --CASE WHEN FR.PricingTypeId <> 2 OR FR.QuantityTypeId = 3 THEN null ELSE (ISNULL(O.[BrokeredMaxQuantity], FR.MaxQuantity) * FR.PricePerGallon) END AS OrderTotalAmount,            
  CASE WHEN PricingTypeId = 4 THEN dbo.usf_GetGlobalFuelCost(o.AcceptedCompanyId, P.TfxProductId, J.StateId, FR.Currency,FR.UoM) ELSE null END AS GlobalFuelCost,            
  --dbo.[usf_GetPricePerGallon](FR.PricePerGallon, FR.PricingTypeId, FR.RackAvgTypeId) AS DisplayPricePerGallon,            
  FRP.DisplayPrice AS DisplayPricePerGallon,            
  --FR.PricePerGallon,            
  --FR.RackAvgTypeId,            
  CASE WHEN CO.ID IS NULL THEN U.Id ELSE CU.Id END AS CustomerId,            
  CASE WHEN CO.ID IS NULL THEN U.Email ELSE CU.Email END AS CustomerEmail,            
  CASE WHEN CO.ID IS NULL THEN U.PhoneNumber ELSE CU.PhoneNumber END AS CustomerPhoneNumber,            
  C.Name AS CustomerCompany,            
  CASE WHEN CO.ID IS NULL THEN U.FirstName ELSE CU.FirstName END AS CustomerFirstName,             
  CASE WHEN CO.ID IS NULL THEN U.LastName ELSE CU.LastName END AS CustomerLastName,            
  SU.FirstName + ' ' + SU.LastName AS SupplierName,            
  SU.Email AS SupplierEmail,            
  SU.PhoneNumber AS SupplierPhoneNumber,            
  S.Name AS SupplierCompany,            
  ISNULL(BSEB.CompanyName, EB.CompanyName) AS ExternalBrokerCompany,            
  CASE WHEN @IsBrokeredOrder = 0 THEN  [dbo].[usf_GetCancelledParentOrder](O.Id) ELSE O.Id   END AS ParentOrderId,            
  CASE WHEN FR.FuelRequestTypeId = 2 THEN dbo.usf_GetParentFuelRequestTypeId(FR.Id) ELSE FR.FuelRequestTypeId END AS FuelRequestTypeId,            
  OAD.IsFuelSurcharge,
  OAD.IsFreightCost,
  OAD.FuelSurchagePricingType,            
  OAD.FileDetails,            
  OAD.PreferencesSettingId,--Added by Pravin for preference setting           
  --SupplierAssignedProductName = dbo.usf_GetSupplierAssignedProductName(@OrderId), --Added by Sharique for displaying the assign product name.          
  PMD.MyProductId,      
  PMD.DriverProductId,      
  PMD.BackOfficeProductId,         
  O.SignatureEnabled AS IsSignatureEnabled,            
  FRP.RequestPriceDetailId,            
  FRP.DisplayPriceCode AS PricingCodeDescription,              
  o.AcceptedCompanyId,  
  OAD.IsIncludePricingInExternalObj AS IsIncludePricing,  
  O.LeadRequestId,  
  O.IsActive,            
  FRD.IsDriverToUpdateBOL as FrIsDriverToUpdateBOL,            
  FRD.TruckLoadTypeId,            
  ORDGRP.GroupPoNumber,            
  ORDGRP.OrderGroupId,            
  FDL.Address AS PickupAddress,            
  FDL.City AS PickupCity,            
  FDL.StateCode AS PickupStateCode,            
  FDL.ZipCode AS PickupZipCode,     
  FDL.BulkPlantId,
  J.IsBillToEnabled,            
  COALESCE(JBA.Name,DF_BA.Name) AS BillToName,            
  COALESCE(JBA.Address,DF_BA.Address) AS BillToAddress,          
  COALESCE(JBA.AddressLine2,DF_BA.AddressLine2) AS BillToAddressLine2,
  COALESCE(JBA.AddressLine3,DF_BA.AddressLine3) AS BillToAddressLine3,  
  COALESCE(JBA.City,DF_BA.City) AS BillToCity,            
  COALESCE(JBA.ZipCode,DF_BA.ZipCode) BillToZipCode,            
  BILLTOMST.Id AS BillToStateId,            
  BILLTOMST.Code AS BillToStateCode,            
  BILLTOCNTRY.Id as BillToCountryId,            
  BILLTOCNTRY.Code as BillToCountryCode,   
  COALESCE(JBA.County,DF_BA.County) AS BillToCounty,
  COALESCE(JBA.StateName,DF_BA.StateName) AS BillToStateName,
  COALESCE(JBA.CountryName,DF_BA.CountryName) AS BillToCountryName, 
  COALESCE(JBA.Id,DF_BA.Id) AS BillingAddressId,         
  J.SiteInstructions,          
  J.IsRetailJob,     
  J.IsMarine as IsMarineLocation,  
  J.Latitude as Latitude,  
  J.Longitude as Longitude,  
  CASE WHEN EXISTS (SELECT 1 FROM JobCarrierDetails WHERE JobId = J.Id) THEN [dbo].[usf_GetJobCarrierDetail](J.Id,@CompanyId) ELSE null END AS CarrierCompanyName,      
  CAST(Case WHEN (FR.FuelRequestTypeId = 3 AND FRD.IsDispatchRetainedByCustomer =1) THEN 1 ELSE 0 END AS BIT) AS IsDispatchRetainedByCustomer,  
  JXA.AssetId AS VessleId,  
  OAD.Berth AS Berth,  
  AAD.IMONumber AS IMONumber,  
  AAD.Flag AS Flag,  
  JXA.Id AS JobXAssetId,  
  AST.[Name] AS Vessel,  
  OAD.IsManualBDNConfirmationRequired AS IsBDNConfirmationRequired,  
  OAD.IsManualInvoiceConfirmationRequired AS IsInvoiceConfirmationRequired,
  OAD.IsSupressPricingEnabled AS IsSupressOrderPricing,
  OAD.IsPDITaxRequired AS IsPdiTaxRequired,
  OAD.FreightPricingMethod,	
  OAD.FreightRateRuleType,	
  OAD.FreightRateTableType,	
  OAD.FreightRateRuleId,	
  OAD.FuelSurchargeTableType,	
  OAD.FuelSurchargeTableId,	
  OAD.AccessorialFeeTableType,	
  OAD.AccessorialFeeId,
  OP.CreditCheckType AS CreditCheckTypeId
  FROM Orders O            
 INNER JOIN FuelRequests FR ON O.FuelRequestId = FR.Id            
 INNER JOIN Users U ON FR.CreatedBy = U.Id            
 INNER JOIN Companies C ON o.BuyerCompanyId = C.Id            
 INNER JOIN Users SU ON O.AcceptedBy = SU.Id            
 INNER JOIN Companies S ON SU.CompanyId = S.Id            
 INNER JOIN Jobs J ON FR.JobId = J.Id            
 INNER JOIN JobBudgets JB ON J.JobBudgetId = JB.Id            
 INNER JOIN MstStates MST ON J.StateId = MST.Id            
 INNER JOIN MstCountries CNTRY ON J.CountryId = CNTRY.Id            
 INNER JOIN MstProducts P ON FR.FuelTypeId = P.Id            
 LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = P.TfxProductId           
 INNER JOIN FuelRequestDetails FRD ON FR.Id = FRD.FuelRequestId            
 INNER JOIN MstDeliveryTypes DT ON FRD.DeliveryTypeId = DT.Id            
 INNER JOIN OrderXStatuses OST ON O.Id = OST.OrderId AND OST.IsActive = 1             
 LEFT JOIN MstExternalTerminals T ON O.TerminalId = T.Id            
 LEFT JOIN OrderXDrivers D ON O.Id = D.OrderId AND D.IsActive = 1            
 LEFT JOIN Users DU ON D.DriverId = DU.Id            
 LEFT JOIN FuelRequestPricingDetails FRP ON FR.Id = FRP.FuelRequestId             LEFT JOIN MstExternalTerminals CT ON FR.CityGroupTerminalId = CT.Id            
 LEFT JOIN OrderAdditionalDetails OAD ON O.Id = OAD.OrderId            
 LEFT JOIN Carriers CARR ON OAD.CarrierId = CARR.Id            
 LEFT JOIN SupplierSources SS ON OAD.SupplierSourceId = SS.Id            
 LEFT JOIN OrderGroupXOrders ORDGRP ON O.Id = ORDGRP.OrderId AND ORDGRP.IsActive = 1            
 LEFT JOIN ExternalBrokerBuySellDetails BSD ON O.Id = BSD.OrderId            
 LEFT JOIN ExternalBrokerOrderDetails EBOD ON O.Id = EBOD.OrderId            
 LEFT JOIN ExternalBrokers EB ON O.ExternalBrokerId = EB.Id            
 LEFT JOIN ExternalBrokers BSEB ON BSD.ExternalBrokerId = BSEB.Id            
 LEFT JOIN OrderDetailVersions V ON O.Id = V.OrderId AND V.IsActive = 1            
 LEFT JOIN MstPaymentTerms PT ON V.PaymentTermId = PT.Id            
 LEFT JOIN OrderXTogglePricingDetails TP ON O.Id = TP.OrderId            
 LEFT JOIN CounterOffers CO ON FR.Id = CO.FuelRequestId AND (CO.BuyerStatus = 2 OR CO.SupplierStatus = 2)            
 LEFT JOIN Users CU ON CO.BuyerId = CU.Id            
 LEFT JOIN FuelDispatchLocations FDL ON O.Id = FDL.OrderId AND FDL.IsActive = 1 AND FDL.DeliveryScheduleId IS NULL AND FDL.TrackableScheduleId IS NULL AND FDL.LocationType = 1            
 LEFT JOIN MstStates BILLTOMST ON J.BillToStateId = BILLTOMST.Id            
 LEFT JOIN MstCountries BILLTOCNTRY ON J.BillToCountryId = BILLTOCNTRY.Id        
 LEFT JOIN SupplierMappedProductDetails SMP ON SMP.IsActive = 1 AND O.AcceptedCompanyId = SMP.CompanyId AND SMP.TerminalId = T.Id AND SMP.FuelTypeId = P.TfxProductId      
 LEFT JOIN @ProductMappingDetail PMD ON SMP.Id = PMD.MappingId      
 LEFT JOIN JobXAssets JXA ON JXA.OrderId = O.Id AND JXA.RemovedBy IS NULL  
 LEFT JOIN AssetAdditionalDetails AAD ON JXA.AssetId = AAD.AssetId  
 LEFT JOIN Assets AST ON JXA.AssetId = AST.Id AND AST.IsActive = 1
 LEFT JOIN BillingAddresses DF_BA(NOLOCK) ON O.BuyerCompanyId = DF_BA.CompanyId AND DF_BA.IsActive = 1 AND DF_BA.IsDefault = 1
 LEFT JOIN BillingAddresses JBA ON J.BillingAddressId = JBA.Id 
 LEFT JOIN OnboardingPreferences OP ON OP.CompanyId = O.AcceptedCompanyId AND OP.IsActive =1 
 WHERE O.Id = @OrderId            
 )           
 SELECT (SELECT COUNT(JXA.Id) FROM JobXAssets JXA INNER JOIN Assets A ON JXA.AssetId = A.Id AND A.IsActive = 1             
   WHERE JXA.JobId = (SELECT DISTINCT JobId FROM OrderDetailCTE) AND JXA.RemovedBy IS NULL) AS AssignedAssetCount ,            
 @ScheduleId AS ScheduleId,            
 @ScheduleName AS ScheduleName,            
 @IsBrokerVisible AS IsBrokerVisible,            
 CASE WHEN (@IsBrokeredOrder = 1 OR ((FuelRequestTypeId = 3 OR  FuelRequestTypeId = 7)AND BuyerCompanyId = @CompanyId)) AND OrderStatus IN (1,4,5) THEN dbo.usf_GetBrokeredOrderStatusId(Id, OrderStatus, @CompanyId) ELSE OrderStatus END AS StatusId,  *     
 FROM OrderDetailCTE            
END  
Go

CREATE OR ALTER    PROCEDURE [usp_GetSupplierInvoicesAndDDTForDashboard] 
 @CompanyId    INT = 1293,   
 @CountryId   INT = 2,  
 @CurrencyType  INT = 2,  
 @GroupIds   NVARCHAR(100)  
AS  
BEGIN
	DECLARE @TimeGap INT = -1;
	SELECT @GroupIds = ISNULL(@GroupIds,'-1');
	--99% of the case call will be for individual company id from Dashboard
	--If Company id record exists in the timegap range, return the latest one
	--else process normally, which can take 5 to 8 seconds.
	--In case of concurrancy, possibilities of multiple records insert exists if multiple users make the call at same time. No effect on the output though.
	If (	@CompanyId > 0 
			AND Exists (select 1 FROM [RptInvoiceDDTForDashboard] WITH (NOLOCK)
						WHERE CompanyId = @CompanyId AND CurrencyType = @CurrencyType AND CountryId = @CountryId AND GroupIds = @GroupIds
								AND UpdatedDate > DateAdd(HOUR,@TimeGap, Cast (GetDate() As DateTimeOffset)))
		)
		BEGIN
			SELECT TOP 1 * FROM [RptInvoiceDDTForDashboard] WITH (NOLOCK)
			WHERE CompanyId = @CompanyId AND CurrencyType = @CurrencyType AND CountryId = @CountryId AND GroupIds = @GroupIds
			ORDER BY UpdatedDate DESC;
		END
	ELSE
		BEGIN
			DECLARE @TblGroupCompanyIds TABLE (CompanyId INT PRIMARY KEY);  
			INSERT INTO @TblGroupCompanyIds SELECT * from usf_GetGroupCompanyIds(@CompanyId,@GroupIds)
 
			DECLARE	  @BrokeredRequest INT  = 3 
				, @IsActiveStatus INT  = 1 
				, @DDTManualInvoiceType INT = 6  
				, @DDTMobileInvoiceType INT = 7 
				, @UnconfirmedInvoiceStatus INT  = 6
				, @ApprovedInvoiceStatus INT   = 3
				, @RejectedInvoiceStatus INT  = 4

				, @ReceivedInvoiceCount INT = 0  
				, @NotApprovedInvoiceCount INT = 0  
				, @CreatedInvoiceCount INT = 0 

				, @WaitingForPriceCount INT = 0  
				, @WaitingForPriceDDTCount INT = 0  
  
				, @ReceivedDDTsCount INT = 0  
				, @NotApprovedDDTsCount INT = 0  
				, @CreatedDDTsCount INT = 0  
		
			--DROP TABLE IF EXISTS #TempInvoices;
	
			--#TempInvoices
			SELECT	Inv.Id AS InvoiceId,   
					Inv.OrderId AS InvoiceOrderId,  
					IH.InvoiceNumberId AS InvoiceNumber,  
					Inv.ParentId AS ParentId,  
					Inv.InvoiceTypeId AS InvoiceTypeId,  
					InvStatus.StatusId AS InvoiceCurrentStatus,  
					Ord.BuyerCompanyId AS OrderBuyerCompanyId,  
					Ord.AcceptedCompanyId AS OrderAcceptedCompanyId,		   
					COM.Id AS InvoiceCreatedByCompanyId,  
					Inv.WaitingFor AS WaitingFor,   
					FR.FuelRequestTypeId AS FuelRequestTypeId	 
			INTO #TempInvoicesDDT  
			FROM 
				 Invoices Inv WITH (NOLOCK)  
				 INNER JOIN InvoiceHeaderDetails IH WITH (NOLOCK) ON IH.Id = INV.InvoiceHeaderId  
				 LEFT JOIN Orders Ord WITH (NOLOCK) ON Ord.Id = Inv.OrderId  
				 LEFT JOIN FuelRequests FR WITH (NOLOCK) ON FR.Id = Ord.FuelRequestId
				 LEFT JOIN Jobs JBS WITH (NOLOCK) ON JBS.Id = FR.JobId  
				 INNER JOIN Users USR WITH (NOLOCK) ON USR.Id = Inv.CreatedBy
				 --INNER JOIN UserXCompanies UXC ON USR.Id = UXC.UserId  
				 INNER JOIN Companies COM WITH (NOLOCK) ON COM.Id = USR.CompanyId 
				 INNER JOIN InvoiceXInvoiceStatusDetails InvStatus WITH (NOLOCK) ON Inv.Id = InvStatus.InvoiceId  
			WHERE
			InvStatus.IsActive = @IsActiveStatus
			AND Inv.InvoiceVersionStatusId = @IsActiveStatus AND Inv.IsBuyPriceInvoice != @IsActiveStatus   
			AND (@CountryId = 0 OR (Inv.Currency = @CurrencyType AND JBS.CountryId = @CountryId))  
			AND  
			(
				(Inv.OrderId IS NULL AND COM.Id IN (SELECT CompanyId FROM @TblGroupCompanyIds)) OR  
				(Inv.OrderId IS NOT NULL AND Ord.AcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)) OR  
				(FuelRequestTypeId = @BrokeredRequest and Ord.BuyerCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds))  
			)
			
			SELECT	@ReceivedInvoiceCount = COUNT(InvoiceId) FROM #TempInvoicesDDt
			WHERE	InvoiceOrderId IS NOT NULL AND FuelRequestTypeId = @BrokeredRequest and OrderBuyerCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)
				AND (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType)
	 
			SELECT	@NotApprovedInvoiceCount = COUNT(InvoiceId) FROM #TempInvoicesDDt
			WHERE	InvoiceCurrentStatus = @RejectedInvoiceStatus and [dbo].[usf_CheckInvoiceHasAnyReceivedStatus](InvoiceNumber) > 0  
				AND (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType)
	 
			SELECT @CreatedInvoiceCount = COUNT(InvoiceId) FROM #TempInvoicesDDt   
			WHERE   
				(  
				(InvoiceOrderId IS NULL and InvoiceCreatedByCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)) OR  
				(InvoiceOrderId IS NOT NULL and OrderAcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds))  
				)
				AND (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType)
     
			SELECT @WaitingForPriceCount = COUNT(InvoiceId) FROM #TempInvoicesDDt WHERE WaitingFor = 1 AND  
				(  
				(InvoiceOrderId IS NULL and InvoiceCreatedByCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)) OR  
				(InvoiceOrderId IS NOT NULL and OrderAcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds))  
				)
				AND (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType)
    	
			SELECT @ReceivedDDTsCount = COUNT(InvoiceId) FROM #TempInvoicesDDt
				WHERE InvoiceOrderId IS NOT NULL AND FuelRequestTypeId = @BrokeredRequest and OrderBuyerCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)
				AND ((InvoiceTypeId != @DDTManualInvoiceType OR InvoiceTypeId != @DDTMobileInvoiceType))
	
			SELECT @NotApprovedDDTsCount = COUNT(InvoiceId) FROM #TempInvoicesDDt
			WHERE InvoiceCurrentStatus = @RejectedInvoiceStatus and [dbo].[usf_CheckInvoiceHasAnyReceivedStatus](InvoiceNumber) > 0
				AND ((InvoiceTypeId != @DDTManualInvoiceType OR InvoiceTypeId != @DDTMobileInvoiceType))
   	 
			SELECT @CreatedDDTsCount = COUNT(InvoiceId) FROM #TempInvoicesDDt   
			WHERE   
				(  
				(InvoiceOrderId IS NULL and InvoiceCreatedByCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)) OR  
				(InvoiceOrderId IS NOT NULL and OrderAcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds))  
				)
				AND ((InvoiceTypeId = @DDTManualInvoiceType OR InvoiceTypeId = @DDTMobileInvoiceType))
   
			SELECT @WaitingForPriceDDTCount = COUNT(InvoiceId)
			FROM #TempInvoicesDDt
			WHERE WaitingFor = 1 AND  
				(  
				(InvoiceOrderId IS NULL and InvoiceCreatedByCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds)) OR  
				(InvoiceOrderId IS NOT NULL and OrderAcceptedCompanyId IN (SELECT CompanyId FROM @TblGroupCompanyIds))  
				)
				AND ((InvoiceTypeId != @DDTManualInvoiceType OR InvoiceTypeId != @DDTMobileInvoiceType))
		
			 
			SELECT @CompanyId As CompanyId,
				ISNULL(SUM ( CASE WHEN (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType) THEN 1 ELSE 0 END),0) As Total,
				ISNULL(SUM ( CASE WHEN InvoiceCurrentStatus = @UnconfirmedInvoiceStatus  AND (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType) THEN 1 ELSE 0 END),0) AS Unconfirmed,
				ISNULL(SUM ( CASE WHEN InvoiceCurrentStatus = @ApprovedInvoiceStatus  AND (InvoiceTypeId != @DDTManualInvoiceType AND InvoiceTypeId != @DDTMobileInvoiceType) THEN 1 ELSE 0 END),0) As Approved,
				@ReceivedInvoiceCount AS Received, @NotApprovedInvoiceCount AS NotApproved, 
				@CreatedInvoiceCount AS Created,@WaitingForPriceCount AS WaitingForPrice,

				ISNULL(SUM ( CASE WHEN ParentId IS NOT NULL THEN 1 ELSE 0 END),0) As InvoicesFromDDT,

				ISNULL(SUM ( CASE WHEN (InvoiceTypeId = @DDTManualInvoiceType OR InvoiceTypeId = @DDTMobileInvoiceType) THEN 1 ELSE 0 END),0) As TotalDDT,
				ISNULL(SUM ( CASE WHEN InvoiceCurrentStatus = @UnconfirmedInvoiceStatus  AND (InvoiceTypeId = @DDTManualInvoiceType OR InvoiceTypeId = @DDTMobileInvoiceType) THEN 1 ELSE 0 END),0) AS UnconfirmedDDT,
				ISNULL(SUM ( CASE WHEN InvoiceCurrentStatus = @ApprovedInvoiceStatus  AND (InvoiceTypeId = @DDTManualInvoiceType OR InvoiceTypeId = @DDTMobileInvoiceType) THEN 1 ELSE 0 END),0) As ApprovedDDT,
				@ReceivedDDTsCount AS ReceivedDDT,@NotApprovedDDTsCount AS NotApprovedDDT, @CreatedDDTsCount as CreatedDDT,  
				@WaitingForPriceDDTCount AS WaitingForPriceDDT,
				@CurrencyType As CurrencyType,
				@CountryId As CountryId,
				@GroupIds As GroupIds,
				Cast(GetDate() As DateTimeOffset) As UpdatedDate
			INTO #InvoiceDDTDashBoard
			FROM #TempInvoicesDDt;
		
			If (@CompanyId > 0)
				BEGIN
					INSERT INTO [RptInvoiceDDTForDashboard] SELECT * FROM #InvoiceDDTDashBoard;					
				END

			DELETE FROM [RptInvoiceDDTForDashboard] WHERE UpdatedDate < DateAdd(HOUR, (@TimeGap - 1), Cast (GetDate() As DateTimeOffset));

			SELECT * FROM #InvoiceDDTDashBoard;
		END
END
Go

CREATE OR ALTER   PROCEDURE [dbo].[usp_GetGallonsOrderedCount]   
 @CompanyId INT ,  
 @StartDate DATETIMEOFFSET = NULL,  
 @EndDate DATETIMEOFFSET = NULL  
AS  
BEGIN 
	DECLARE @TotalCount decimal;

	IF (@CompanyId = 0 AND @StartDate Is Null AND @EndDate IS NULL) --This will be default 99% call 
		BEGIN 
			SET @TotalCount = (	SELECT CAST (ISNULL(SUM(COALESCE(ORD.BrokeredMaxQuantity,FR.MaxQuantity,0)), 0) As Decimal)  
								FROM Orders ORD WITH (NOLOCK)  
									JOIN FuelRequests FR  WITH (NOLOCK) ON FR.Id = ORD.FuelRequestId 
								WHERE   
									ORD.IsActive=1 AND ORD.ParentId IS NULL AND FR.QuantityTypeId <> 3);			

			SELECT @TotalCount AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount
		END
	ELSE
		BEGIN
			SELECT @TotalCount = CAST(ISNULL(SUM(ISNULL(COALESCE(ORD.BrokeredMaxQuantity,FR.MaxQuantity,0), 0)),0) As DECIMAL)
			FROM Orders ORD  WITH (NOLOCK)  
				JOIN FuelRequests FR  WITH (NOLOCK) ON FR.Id = ORD.FuelRequestId
			WHERE   
				ORD.IsActive=1 AND ORD.ParentId IS NULL AND FR.QuantityTypeId <> 3
				AND (@CompanyId = 0 OR ORD.AcceptedCompanyId = @CompanyId )
	
			DECLARE @BrokeredMaxQuantity decimal;
			IF (@StartDate IS NULL AND @EndDate IS NULL)		
				SELECT @BrokeredMaxQuantity = @TotalCount;
			ELSE
				BEGIN
					SELECT @BrokeredMaxQuantity = CAST(ISNULL(SUM(ISNULL(COALESCE(ORD.BrokeredMaxQuantity,FR.MaxQuantity,0), 0)),0) As DECIMAL)
						FROM Orders ORD  WITH (NOLOCK)  
							JOIN FuelRequests FR  WITH (NOLOCK) ON FR.Id = ORD.FuelRequestId
						WHERE   
							ORD.IsActive=1 AND ORD.ParentId IS NULL AND FR.QuantityTypeId <> 3
							AND (@CompanyId = 0 OR ORD.AcceptedCompanyId = @CompanyId )
							AND ORD.AcceptedDate >= (CASE WHEN @StartDate IS NOT NULL THEN  @StartDate ELSE CONVERT(DATETIME, -53690) END)
							AND ORD.AcceptedDate <= (CASE WHEN @EndDate IS NOT NULL THEN @EndDate ELSE GETDATE() END);
				END
				
			SELECT @BrokeredMaxQuantity AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount;
		END
END
GO

CREATE OR ALTER PROCEDURE [dbo].[usp_GetGallonsDeliveredCount] 
 @CompanyId INT  = 0,  
 @StartDate DATETIMEOFFSET = NULL,  
 @EndDate DATETIMEOFFSET = NULL
AS  
BEGIN

	DECLARE @TotalCount decimal  

	IF (@CompanyId = 0 AND @StartDate Is Null AND @EndDate IS NULL) --This will be default 99% call 
		BEGIN 
			SET @TotalCount = (SELECT CAST (ISNULL(SUM(ISNULL(INV.DroppedGallons, 0)),0) As DECIMAL) AS TotalOrderedDeliveredCount
								FROM Invoices INV WITH (NOLOCK)  
								WHERE
									INV.IsActive=1 AND INV.InvoiceVersionStatusId=1 AND INV.IsBuyPriceInvoice=0);
			SELECT @TotalCount AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount
		END
	ELSE 
		BEGIN
						
			SELECT @TotalCount = CAST(ISNULL(SUM(ISNULL(INV.DroppedGallons, 0)),0) As DECIMAL)   
			FROM	Orders ORD  WITH (NOLOCK)  
				JOIN Invoices INV  WITH (NOLOCK) ON ORD.Id = INV.OrderId  
			WHERE   
				INV.IsActive=1 AND INV.InvoiceVersionStatusId=1 AND INV.IsBuyPriceInvoice=0
				AND INV.OrderId IS NOT NULL
				AND (@CompanyId = 0 OR ORD.AcceptedCompanyId=@CompanyId)  
					
			DECLARE @TotalOrderedDeliveredCount decimal
			IF (@StartDate IS NULL AND @EndDate IS NULL)
				SELECT @TotalOrderedDeliveredCount = @TotalCount;
			ELSE
				BEGIN
					SELECT @TotalOrderedDeliveredCount = CAST(ISNULL(SUM(ISNULL(INV.DroppedGallons, 0)),0) As DECIMAL)   
					FROM	Orders ORD  WITH (NOLOCK)  
						JOIN Invoices INV  WITH (NOLOCK) ON ORD.Id = INV.OrderId  
					WHERE   
						INV.IsActive=1 AND INV.InvoiceVersionStatusId=1 AND INV.IsBuyPriceInvoice=0
						AND INV.OrderId IS NOT NULL
						AND (@CompanyId = 0 OR ORD.AcceptedCompanyId = @CompanyId)   
						AND ORD.AcceptedDate >= (CASE WHEN @StartDate IS NOT NULL THEN  @StartDate ELSE CONVERT(DATETIME, -53690) END)
						AND ORD.AcceptedDate <= (CASE WHEN @EndDate IS NOT NULL THEN @EndDate ELSE GETDATE() END)
				END

			SELECT @TotalOrderedDeliveredCount AS TotalOrderedDeliveredCount, @TotalCount  As TotalCount;
		END	
END
GO
Go



IF NOT EXISTS (SELECT 1 FROM MstAppSettings WHERE [Key]='SAPFailedRequestRetryForCompanies')
BEGIN
	INSERT INTO [dbo].[MstAppSettings] ([Key], [Value], [Description], [IsActive], [UpdatedBy], [UpdatedDate])
		 VALUES ('SAPFailedRequestRetryForCompanies','','List of company ids separated by comma (,)', 1, 1, SYSDATETIMEOFFSET())
END
GO

IF EXISTS (SELECT 1 FROM ApiLogs WHERE [Url] = 'Location-Create' AND RESPONSE LIKE '%Invalid External Ref Id%' AND [MESSAGE]='1' AND RETRYCOUNT=0)
BEGIN
	UPDATE ApiLogs SET RETRYCOUNT=1 WHERE [Url] = 'Location-Create' AND RESPONSE LIKE '%Invalid External Ref Id%' AND [MESSAGE]='1' AND RETRYCOUNT=0
END
GO

GO
/****** Object:  UserDefinedFunction [dbo].[usf_getBolDetailsForInvoiceV2]    Script Date: 2022-04-28 10:37:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-----------------------------------------------------------------
--SELECT * FROM [dbo].[usf_getBolDetailsForInvoiceV2](6742,'',7,0,'2022-01-10','2022-01-18') where InvoiceHeaderId = 342440
Create or ALTER   FUNCTION [dbo].[usf_getBolDetailsForInvoiceV2]             
(  @CompanyId INT,            
   @GroupIds   NVARCHAR(100),          
   @InvoiceTypeId INT,         
   @OrderId INT,        
   @StartDate DATETIMEOFFSET(7),            
   @EndDate DATETIMEOFFSET(7)              
)
RETURNS @TempBOLDetails Table             
(            
	InvoiceHeaderId INT,            
	BOLNo nvarchar(1000),  
	BadgeNumber nvarchar(max),  
	TerminalName nvarchar(max),            
	PickupAddress nvarchar(max),            
	PricePerGallon nvarchar(1000),        
	PoNumber  nvarchar(1000),        
	FuelType nvarchar(1000),        
	OrderId nvarchar(1000),        
	InvoiceId nvarchar(1000),        
	DroppedGallons nvarchar(1000),
	ConvertedQuantity varchar(max),
	DropDate nvarchar(1000),        
	DropTime nvarchar(1000)  ,  
	DropTicketNumber nvarchar(1000),  
	Carrier nvarchar(1000),  
	LiftDate nvarchar(1000),  
	QbInvoiceNumber nvarchar(1000),
	DriverName nvarchar(1000),  
	NetQuantity	 decimal(18,8),
	GrossQuantity	 decimal(18,8),
	HasAttachments	 nvarchar(10),
	PrePostValues nvarchar(max) ,
	InvoiceDate nvarchar(256),
	CreationMethod nvarchar(256),
	PaymentDueDate nvarchar(256),
    StatusName nvarchar(100),
	TimeToInvoice INT
)
BEGIN 
	DECLARE @TblGroupCompanyIds TABLE (CompanyId INT PRIMARY KEY);                
	INSERT INTO @TblGroupCompanyIds SELECT * from usf_GetGroupCompanyIds(@CompanyId,@GroupIds)
	
	INSERT INTO @TempBOLDetails(InvoiceHeaderId,BOLNo,BadgeNumber,TerminalName,pickupAddress,pricePerGallon,PoNumber,FuelType,OrderId,InvoiceId,DroppedGallons,ConvertedQuantity,DropDate,DropTime,DropTicketNumber,Carrier,LiftDate,QbInvoiceNumber,DriverName,NetQuantity,GrossQuantity,HasAttachments,PrePostValues,InvoiceDate,CreationMethod,PaymentDueDate,StatusName, TimeToInvoice)
		SELECT InvoiceHeaderId,BOLNo,BadgeNumber,TerminalName,pickupAddress,pricePerGallon,PoNumber,FuelType,OrderId,InvoiceId,DroppedGallons,ConvertedQuantity,DropDate,DropTime,DropTicketNumber,Carrier,LiftDate,QbInvoiceNumber,DriverName,NetQuantity,GrossQuantity,HasAttachments,PrePostValues,InvoiceDate,CreationMethod,PaymentDueDate, StatusName, TimeToInvoice
		FROM  (SELECT  
				DISTINCT IHD.Id AS InvoiceHeaderId, 
				(CASE WHEN IFD.PickupLocation = 2 THEN IFD.LiftTicketNumber ELSE  IFD.BolNumber END) As  BOLNo,
				(CASE WHEN IFD.BadgeNumber = '' THEN NULL ELSE IFD.BadgeNumber END) As BadgeNumber,
				MET.[Name] AS [TerminalName],
				(CASE
					WHEN IFD.PickupLocation = 2 
					THEN
						CASE WHEN IFD.SiteName IS NULL THEN 'Bulk Plant: ' ELSE IFD.SiteName + ': ' END 
						+ ISNULL(IFD.Address + ', ' + IFD.City+ ', ' + IFD.StateCode + ' ' + IFD.ZipCode,'--')   
					ELSE ISNULL(IFD.Address + ', ' + IFD.City+ ', ' +   IFD.StateCode + ' ' + IFD.ZipCode,'--')
				END) As PickupAddress,
				CAST(CAST(IFD.PricePerGallon AS NUMERIC(18,4)) AS varchar(max)) AS PricePerGallon,
				(CASE WHEN INO.OrderId IS NULL THEN '--' ELSE INO.PoNumber END) As PONumber,
				(    
					CASE
					WHEN INO.OrderId IS NULL
					THEN '--'
					WHEN INO.InvoiceTypeId = 5
					THEN 'Dry Run Fee'
					WHEN INO.InvoiceTypeId = 9
					THEN 'Balance Invoice'
					WHEN INO.InvoiceTypeId = 10
					THEN 'Tank Rental Invoice' 
					ELSE ISNULL(TFXP.NAME,PRD.Name) 
					END
				) AS FuelType,
				Cast(INO.OrderId as varchar(100)) OrderId,
				INO.Id As InvoiceId,
				CASE WHEN JBS.IsMarine = 1 AND (INO.FRQ_UoM = 3 OR INO.FRQ_UoM = 4) THEN  
						(CAST(CAST(INO.DroppedGallons AS NUMERIC(18,2)) AS varchar(max)) + ' (' +
						(CAST(CAST(ISNULL(INO.ConvertedQuantity, 0) AS NUMERIC(18,2)) AS varchar(max)) + ' ' + 
								CASE WHEN INO.FRQ_UoM = 2 THEN 'Litres' WHEN INO.FRQ_UoM = 3 THEN 'Barrel' WHEN INO.FRQ_UoM = 4 THEN 'MT' ELSE 'Gallons' END  + ')')) 
					ELSE
						CAST(CAST(INO.DroppedGallons AS NUMERIC(18,2)) AS varchar(max))
					END AS DroppedGallons, 
				INO.ConvertedQuantity,
				(CASE WHEN INO.InvoiceTypeId = 9 THEN '-' ELSE CONVERT(NVARCHAR(10), INO.DropEndDate, 101) END) As DropDate,
				(CASE WHEN INO.InvoiceTypeId = 9 THEN '-' ELSE FORMAT(INO.DropStartDate,'h:mm tt') + ' - ' + FORMAT(INO.DropEndDate,'h:mm tt') END) As DropTime,
				CASE WHEN (IAD.DropTicketNumber = '' OR IAD.DropTicketNumber IS NULL) THEN '--' ELSE IAD.DropTicketNumber END AS DropTicketNumber,
				ISNULL(IFD.Carrier ,'--') AS Carrier,
				(CASE WHEN (IFD.LiftDate ='' OR IFD.LiftDate IS NULL) THEN NULL ELSE Concat(CONVERT(NVARCHAR(10), IFD.LiftDate, 101),' ',CONVERT(varchar(15),CAST(IFD.LiftStartTime AS TIME),100),'-',CONVERT(varchar(15),CAST(IFD.LiftEndTime AS TIME),100)) END) AS LiftDate,
				(CASE WHEN (INO.QbInvoiceNumber IS NULL) THEN '' ELSE INO.QbInvoiceNumber END) As QbInvoiceNumber,
				(CASE WHEN (INO.DriverId IS NULL) THEN '--' ELSE D_USR.FirstName + ' ' + D_USR.LastName END) As DriverName,
				--(CAST(CAST(SUM(ISNULL(IFD.NetQuantity, 0)) AS NUMERIC(18,2)) AS NVARCHAR(100))) AS TotalNetQty,
				--(CAST(CAST(SUM(ISNULL(IFD.GrossQuantity, 0)) AS NUMERIC(18,2)) AS NVARCHAR(100))) AS TotalGrossQty,
				IFD.NetQuantity,
				IFD.GrossQuantity,
				(CASE WHEN ((INO.ImageId IS NOT NULL AND INO.ImageId > 0) OR -- Drop Image
							 (IFD.ImageId IS NOT NULL AND IFD.ImageId > 0) OR -- BOL image
							 (IXAD.AdditionalImageId IS NOT NULL AND IXAD.AdditionalImageId > 0) OR -- Additional image
							 (INO.SignatureId IS NOT NULL AND INO.SignatureId > 0 AND SIMG.ImageId IS NOT NULL AND SIMG.ImageId > 0)) -- Signature image
						THEN 1
						ELSE 0 END
						) HasAttachments,
				(AST.Name + ': ' +   
					CAST(CAST(ASD.PreDip AS NUMERIC(18,2)) AS varchar(max)) +  
					CASE WHEN ASD.TankScaleMeasurement = 1 THEN 'cm' WHEN ASD.TankScaleMeasurement = 2 THEN 'in'   
					WHEN ASD.TankScaleMeasurement = 3 THEN 'G' WHEN ASD.TankScaleMeasurement = 4 THEN 'L'    
					ELSE '' END  
					+ ' - ' +   
					CAST(CAST(ASD.PostDip AS NUMERIC(18,2)) AS varchar(max)) +   
					CASE WHEN ASD.TankScaleMeasurement = 1 THEN 'cm'  WHEN ASD.TankScaleMeasurement = 2 THEN 'in'   
					WHEN ASD.TankScaleMeasurement = 3 THEN 'G' WHEN ASD.TankScaleMeasurement = 4 THEN 'L'    
					ELSE '' END +  
					' = ' +  
					CAST(CAST(ASD.DroppedGallons AS NUMERIC(18,2)) AS varchar(max)) +   
					CASE WHEN ASD.PostDip !=0 AND INO.UoM = 1 THEN 'G'  WHEN ASD.PostDip !=0 AND INO.UoM = 2 THEN 'L'   
					ELSE '' END) AS PrePostValues,
					INO.CreatedDate,
					INO.CompanyId,
					INO.AcceptedCompanyId,
					INO.BuyerCompanyId,
					INO.FuelRequestTypeId,
					INO.FRQ_ID,
					INO.InvoiceTypeId,
					CASE        
						WHEN IXS.StatusId = 10 THEN '--'        
						ELSE CONVERT(nvarchar(10), INO.UpdatedDate, 101)        
					END AS InvoiceDate, 
					CASE        
						WHEN IAD.CreationMethod = 2 THEN 'Mobile'        
						WHEN IAD.CreationMethod = 3 THEN 'Bulk Upload'
						WHEN IAD.CreationMethod = 4 THEN 'TPD API'
						ELSE 'Manual'        
					END AS CreationMethod,
					CONVERT(NVARCHAR(10), INO.PaymentDueDate, 101) AS PaymentDueDate,
					IST.[Name] as StatusName,
					DATEDIFF(MINUTE, ISNULL(IFD.LiftDate, INO.DropEndDate), INO.CreatedDate) AS TimeToInvoice
		FROM  
			dbo.InvoiceHeaderDetails(NOLOCK) IHD
			INNER JOIN (
					SELECT INV.*, 
							I_USR.CompanyId,
							ORD.AcceptedCompanyId,
							ORD.BuyerCompanyId,
							FRQ.FuelRequestTypeId,
							FRQ.ID AS FRQ_ID,
							FRQ.JobId,
							FRQ.FuelTypeId,
							FRQ.UoM AS FRQ_UoM
							FROM dbo.Invoices(NOLOCK) INV		
							INNER JOIN dbo.Users(NOLOCK) I_USR ON I_USR.Id = INV.CreatedBy
							LEFT OUTER JOIN ORDERS(NOLOCK) ORD ON ORD.ID = INV.OrderId
							LEFT JOIN dbo.FuelRequests(NOLOCK) FRQ ON FRQ.Id = ORD.FuelRequestId
							WHERE INV.InvoiceVersionStatusId = 1 AND		        
									(                
										(ORD.Id IS NULL AND I_USR.CompanyId IN(SELECT CompanyId FROM @TblGroupCompanyIds))                
										OR                
										(ORD.Id IS NOT NULL AND ORD.AcceptedCompanyId IN(SELECT CompanyId FROM @TblGroupCompanyIds))
										OR
										(FRQ.FuelRequestTypeId = 3 OR [dbo].[usf_GetParentFuelRequestTypeId](FRQ.Id) = 3)   
											AND ORD.BuyerCompanyId IN(SELECT CompanyId FROM @TblGroupCompanyIds)  
										OR        
										(	(FRQ.FuelRequestTypeId = 7 OR [dbo].[usf_GetParentFuelRequestTypeId](FRQ.Id) = 7)   
											AND ORD.BuyerCompanyId IN(SELECT CompanyId FROM @TblGroupCompanyIds))
									)
									AND
									(                
										ORD.Id = @OrderId                
										OR                
										(                
										 @OrderId = 0 AND INV.CreatedDate >= @StartDate AND INV.CreatedDate < @EndDate                
										)                
									)             
									AND                
									(                
										(@InvoiceTypeId IN (6, 7) AND INV.InvoiceTypeId IN (6, 7))                
										OR                
										(@InvoiceTypeId NOT IN (6, 7) AND INV.InvoiceTypeId NOT IN (6, 7))                
									)
						) AS INO ON IHD.Id = INO.InvoiceHeaderId
			LEFT JOIN dbo.Users(NOLOCK) D_USR ON D_USR.Id = INO.DriverId
			LEFT JOIN dbo.InvoiceXAdditionalDetails(NOLOCK) IAD ON INO.Id = IAD.InvoiceId
			LEFT JOIN dbo.InvoiceXInvoiceStatusDetails(Nolock) IXS ON INO.Id = IXS.InvoiceId AND IXS.IsActive = 1
			LEFT JOIN dbo.MstInvoiceStatuses(Nolock) IST ON IXS.StatusId = IST.Id
			LEFT JOIN dbo.InvoiceXBolDetails(NOLOCK) INVBOL  ON INO.Id =INVBOL.InvoiceId               
			LEFT JOIN dbo.InvoiceFtlDetails(NOLOCK) IFD ON IFD.Id = INVBOL.BolDetailId
			LEFT JOIN dbo.MstExternalTerminals(NOLOCK) MET ON MET.Id = IFD.TerminalId
			LEFT JOIN dbo.Jobs(NOLOCK) JBS ON JBS.Id = INO.JobId
			LEFT JOIN dbo.MstProducts(NOLOCK) PRD ON PRD.Id = INO.FuelTypeId
			LEFT JOIN dbo.MstTfxProducts(NOLOCK) TFXP  ON TFXP.Id = PRD.TfxProductId
			LEFT JOIN dbo.MstStates(NOLOCK) MST ON MST.Id = JBS.StateId
			LEFT JOIN dbo.Signatures(NOLOCK) SIMG ON SIMG.Id  = INO.SignatureId
			LEFT JOIN dbo.InvoiceXAdditionalDetails(NOLOCK) IXAD ON INO.Id = IXAD.InvoiceId
			LEFT JOIN dbo.AssetDrops(NOLOCK) ASD ON INO.Id = ASD.InvoiceId
			LEFT JOIN dbo.JobXAssets(NOLOCK) JXA ON  JXA.Id = ASD.JobXAssetId
			LEFT JOIN dbo.Assets(NOLOCK) AST ON AST.Id = JXA.AssetId) AS VBD		
		RETURN
END
GO
GO
/****** Object:  StoredProcedure [dbo].[usp_GetBolDetails]    Script Date: 2022-04-27 1:06:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC [usp_GetBolDetails] 621256 
Create or ALTER   PROCEDURE [dbo].[usp_GetBolDetails] 
	@InvoiceId INT  
AS  
BEGIN
	-- ADDED THIS VARIABLE AND TEMP TABLE TO HIDE BOL DETAILS OF INACTIVE INVOICE RECORDS
	DECLARE @InvoiceHeaderId INT
	SELECT @InvoiceHeaderId = InvoiceHeaderId FROM Invoices WHERE Id = @InvoiceId
	SELECT Id 
	INTO #ActiveInvoiceRecords
	FROM Invoices WHERE InvoiceHeaderId = @InvoiceHeaderId AND InvoiceVersionStatusId = 1
	SELECT DISTINCT
			FTL.BolNumber,  
			FTL.LiftQuantity,  
			FTL.LiftTicketNumber,  
			FTL.NetQuantity,  
			FTL.LiftDate, 
			ftl.LiftStartTime,
			ftl.LiftEndTime,
			FTL.GrossQuantity, 
			FTL.DeliveredQuantity,
			FTL.Carrier,  
			FTL.PickupLocation AS PickupLocationType,  
			ISNULL(FTL.Address,'--') AS Address,  
			ISNULL(FTL.City,'--') AS City,  
			ISNULL(FTL.ZipCode,'--') AS ZipCode,  
			ISNULL(FTL.StateCode,'--') AS StateCode,  
			CASE WHEN FTL.PickupLocation = 2 THEN ISNULL(FTL.SiteName,'--') ELSE ISNULL(FTL.TerminalName, '--') END AS TerminalName,  
			ISNULL(FTL.PricePerGallon,0) AS RackPrice,
			FTL.BadgeNumber
			FROM Invoices I  
			INNER JOIN InvoiceXBolDetails InvBol ON InvBol.InvoiceId in (SELECT Id FROM #ActiveInvoiceRecords)
			INNER JOIN InvoiceFtlDetails FTL ON InvBol.BolDetailId = FTL.Id  AND I.InvoiceVersionStatusId = 1
			LEFT JOIN MstExternalTerminals T ON FTL.TerminalId = T.Id
	 WHERE (I.Id = @InvoiceId  AND I.OrderId IS NOT NULL) OR
		   (I.Id = @InvoiceId  AND I.OrderId IS NULL AND (FTL.PricePerGallon IS NULL OR FTL.PricePerGallon <= 0))
END
GO

GO
/****** Object:  StoredProcedure [dbo].[usp_GetTerminalProductAssignmentGrid]    Script Date: 04/30/2022 1:35:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		yash b
-- Create date: 30th March 2022
-- Description:	Returns all the mapped products for a terminal
-- =============================================

--EXEC usp_GetTerminalProductAssignmentGrid 1,1
CREATE OR ALTER     PROCEDURE [dbo].[usp_GetTerminalProductAssignmentGrid]
	-- Add the parameters for the stored procedure here
	@CountryId nvarchar(10) = 1,
	@PricingSourceId INT = 1 
AS
BEGIN
      SET NOCOUNT ON;
      DECLARE @CountryCode nvarchar(10);
	   SET  @CountryCode = (SELECT CODE FROM MstCountries MC where MC.Id = @CountryId) 
	 ;WITH TerminalCTE AS
	 (
	   SELECT 
	   DISTINCT 
	        MET.Id AS TerminalId,
			MET.ControlNumber as TerminalControlNumber,
			MET.Name AS TerminalName
			FROM MstExternalTerminals MET	 
	        WHERE MET.IsActive =1 AND  MET.CountryCode = @CountryCode  AND MET.PricingSourceId = @PricingSourceId
			AND MET.ZipCode NOT IN ('00001','00002','00003') -- rack terminal filter
	 )
     SELECT
	  TCTE.TerminalId as TerminalId,
	  TCTE.TerminalControlNumber,
	  TCTE.TerminalName,
	  CASE WHEN COUNT(MP.Id) <=0 THEN null
	  ELSE
	  '['+ STRING_AGG(Convert(nvarchar(max), concat('{"Id":', MP.Id, ',"Name":"', isnull(MP.DisplayName,MP.Name),'"}')), ',')+']' end as AssignedProducts,
	 COUNT(MP.Id) as MappedProductCount
	 FROM TerminalCTE TCTE
	 LEFT JOIN MstProductMappings MPM ON TCTE.TerminalId = MPM.ExternalTerminalId AND MPM.IsActive = 1 
	 LEFT JOIN MstProducts MP ON MP.Id = MPM.ProductId    AND MPM.IsActive = 1 AND MP.IsActive = 1
	 WHERE (MP.PricingSourceId = @PricingSourceId OR MP.Id IS NULL)		 
	 GROUP by TCTE.TerminalId,TCTE.TerminalControlNumber,TCTE.TerminalName
	 ORDER BY TCTE.TerminalId DESC
END
Go
CREATE OR ALTER PROCEDURE [dbo].[usp_GetTPOOrderDetails] -- EXEC [usp_GetTPOOrderDetails] 55288  
 @OrderId INT  
AS  
BEGIN  
Select BUSR.Id AS UserId,  
BUSR.CompanyId as BuyerCompanyId,  
BCOM.Name AS BuyerCompanyName,  
BUSR.PhoneNumber,  
BUSR.Email,  
ORD.IsFTL,  
JB.Id AS JobId,  
JCUSR.Id AS OnsiteContactUserId,  
JCUSR.FirstName AS OnsiteFirstName,  
JCUSR.LastName AS OnsiteLastName,  
JBD.IsTaxExempted,  
FR.QuantityTypeId,  
FR.MinQuantity,  
FR.MaxQuantity,  
FRD.PricingQuantityIndicatorTypeId,  
IsNull(FR.FreightOnBoardTypeId, 0) AS FreightOnBoardTypeId,  
FRD.DeliveryTypeId,  
JSON_VALUE(FRD.CustomAttribute, '$.WBSNumber') AS WBSNumber,  
MP.ProductDisplayGroupId,  
FR.PaymentTermId,  
FR.NetDays,  
FRD.PaymentMethod,  
OAD.BOLInvoicePreferenceId,  
OAD.SupplierSourceId,  
SS.Name AS SupplierSourceName,  
OAD.SupplierContract,  
OAD.CarrierId,  
CR.Name AS Carrier,  
OAD.Notes,  
OAD.LoadCode,  
FR.OrderClosingThreshold,  
JBD.IsAssetTracked,  
JBD.IsAssetDropStatusEnabled,  
OAD.IsDriverToUpdateBOL,  
OAD.DRNotes,  
FRD.IsBolImageRequired,  
FRD.IsDropImageRequired,  
JB.SignatureEnabled,  
JBA.Address AS BillToAddress,  
JBA.AddressLine2 AS BillToAddressLine2,  
JBA.AddressLine3 AS BillToAddressLine3,  
JBA.City AS BillToCity,  
JBA.CountryId AS BillToCountryId,  
JBA.Name AS BillToName,  
JBA.StateId AS BillToStateId,  
JBA.ZipCode AS BillToZipCode,  
JBA.County AS BillToCounty,  
JBA.StateName AS BillToStateName,  
JBA.CountryName AS BillToCountryName,   
FR.FuelRequestTypeId,  
JB.LocationInventoryManagedBy,  
JB.IsMarine AS IsMarineLocation,  
CASE WHEN JB.IsMarine =1 THEN FR.UoM ELSE JB.UoM END AS UoM,  
OAD.IsManualBDNConfirmationRequired AS IsBDNConfirmationRequired,  
OAD.IsManualInvoiceConfirmationRequired AS IsInvoiceConfirmationRequired,  
OAD.InternationalWatersType as InternationalWatersType,  
OAD.Berth as Berth,  
JXA.AssetId as VessleId,  
AAD.IMONumber as IMONumber,  
AAD.Flag as Flag,  
OAD.IsSupressPricingEnabled as IsSuppressPricingEnabled,  
OAD.IsPDITaxRequired,
OAD.FreightPricingMethod
from Orders ORD  
INNER JOIN FuelRequests FR ON FR.id = ORD.FuelRequestId  
INNER JOIN FuelRequestDetails FRD ON FRD.FuelRequestId = ORD.FuelRequestId  
INNER JOIN Users BUSR ON BUSR.Id = FR.CreatedBy  
INNER JOIN Companies BCOM ON BCOM.Id = BUSR.CompanyId  
INNER JOIN Jobs JB ON JB.Id = FR.JobId  
INNER JOIN MstProducts MP ON MP.Id = FR.FuelTypeId  
INNER JOIN OrderAdditionalDetails OAD ON OAD.OrderId = ORD.Id  
LEFT JOIN JobBudgets JBD ON JBD.Id = JB.JobBudgetId  
LEFT JOiN JobXOnsiteContactPersons JCP ON JCP.JobId = JB.Id  
LEFT JOiN Users JCUSR ON JCUSR.Id = JCP.UserId  
LEFT JOIN Carriers CR ON CR.Id = OAD.CarrierId  
LEFT JOIN SupplierSources SS ON SS.Id = OAD.SupplierSourceId  
LEFT JOIN JobXAssets JXA ON JXA.OrderId = @OrderId  
LEFT JOIN AssetAdditionalDetails AAD ON AAD.AssetId =JXA.AssetId  
LEFT JOIN BillingAddresses JBA ON JB.BillingAddressId = JBA.Id   
Where ORD.Id = @OrderId  
END

Go

CREATE OR ALTER FUNCTION [DBO].[usf_GetDateDiffInDayHrMin]  
(  
    @StartDate    DATETIMEOFFSET(7),  
    @EndDate    DATETIMEOFFSET(7)  
)  
RETURNS NVARCHAR(256)  
AS  
BEGIN  
    DECLARE @Result NVARCHAR(256),
            @DiffInMinute int = DATEDIFF(MINUTE, @StartDate, @EndDate),
			@Day int = 0,
			@Hour int = 0,
			@Min int = 0

			if (@DiffInMinute > 0)
				BEGIN
					SELECT @Day = @DiffInMinute / 1440;
					SELECT @Hour = (@DiffInMinute % 1440) / 60;
					SELECT @Min = (@DiffInMinute % 14440) % 60;
				END

			SELECT @Result =		
					(CASE	WHEN @DiffInMinute <= 0 THEN '0 Min'
							ELSE
								CONCAT ( 
								(CASE WHEN @Day >= 2 THEN CONCAT ( @Day, ' Day ') WHEN @Day >= 1 THEN '1 Day ' ELSE '' END),
								(CASE WHEN @Hour >= 2 THEN CONCAT ( @Hour, ' Hr ') WHEN @Hour >= 1 THEN '1 Hr ' ELSE '' END),
								(CASE WHEN @Day >= 1 THEN '' WHEN @Min >= 2 THEN CONCAT ( @Min, ' Min ') WHEN @Min >= 1 THEN '1 Min ' ELSE '' END))
							END);
    RETURN @Result  
END
Go
Go
-- exec usp_GetDeliveryDetails 342961      
CREATE OR ALTER PROCEDURE [dbo].[usp_GetDeliveryDetails]      
 @InvoiceHeaderId INT          
 AS        
BEGIN        
 DECLARE @ThirdPartyId INT;      
 DECLARE @StaticOriginVendorId nvarchar(max);      
 SELECT @ThirdPartyId = Id FROM MstExternalThirdPartyCompanies WHERE CompanyName =  'PDI Mapping'      
 SELECT @StaticOriginVendorId = Value FROM MstAppSettings WHERE [KEY]='PDIEStaticVendorId'      
 IF @ThirdPartyId IS NULL      
 BEGIN      
  SET @ThirdPartyId = 2      
 END      
 SELECT        
 I.Id AS InvoiceId,       
 O.Id AS OrderId,      
 I.DisplayInvoiceNumber,      
 ISNULL(ES.TargetSupplierValue,'') AS VendorId,      
 ISNULL(EC.TargetCustomerValue,'') AS CustomerId,        
 ISNULL(ECL.TargetCustomerLocationValue,'') AS JobName,      
 CASE WHEN O.AcceptedCompanyId = O.BuyerCompanyId THEN ISNULL(ECL.TargetCustomerLocationValue,'')      
 ELSE ISNULL(EB.TargetBulkPlantValue,'') END AS SiteId,        
 I.DropEndDate,        
 ISNULL(TS.Date, I.DropEndDate) AS BusinessDate,        
 ISNULL(ECAR.TargetCarrierValue, '') AS Carrier,        
 --ISNULL(D.FirstName + ' ' + D.LastName, '') AS DriverName,      
 NULL AS DriverName,      
 EDM.TargetDriverValue, -- NEW PROPERTY      
 --NULL AS TargetDriverValue,      
 O.PoNumber,      
 B.LiftDate,        
 ROUND (TS.Quantity,2)AS OrderedQuantity,        
 CASE WHEN B.DeliveredQuantity IS NULL THEN ROUND (I.DroppedGallons, 2) ELSE ROUND (B.DeliveredQuantity, 2) END AS DroppedGallons,        
 ROUND (B.NetQuantity,2) AS NetQuantity ,        
 ROUND (B.GrossQuantity,2) AS GrossQuantity,        
 B.PickupLocation,        
 CASE WHEN B.PickupLocation = 2 THEN  ISNULL(EB.TargetBulkPlantValue,'')      
 ELSE ISNULL(ET.TargetTerminalValue,'') END  AS  SiteName,       
 COALESCE(B.BolNumber, B.LiftTicketNumber) AS BolNumber,        
 ISNULL(SBJ.CompanyOwnedLocation, 0) AS DestinationType,        
 CASE WHEN O.AcceptedCompanyId = O.BuyerCompanyId THEN 0      
 ELSE 1 END AS DestinationType,        
 ISNULL(EP.TargetProductValue,'') AS FuelType,        
 ISNULL(FRD.PricingQuantityIndicatorTypeId, 1) AS QuantityIndicatorTypeId,      
 ISNULL(OAD.BOLInvoicePreferenceId, 4) AS BOLInvoicePreferenceId, -- 4 - None      
 FRPD.RequestPriceDetailId,      
 J.IsMarine AS IsMarineLocation,      
 OAD.IsIncludePricingInExternalObj AS IsIncludePricing,      
 OAD.IsPDITaxRequired, -- NEW PROPERTY      
 CAST(I.ConvertedPricing AS DECIMAL(15,7)) AS ConvertedPricing, -- Keep format as DECIMAL(15,7) for PDI      
 CAST(B.PricePerGallon AS DECIMAL(15,7)) AS PricePerGallon,      
 FR.PricingTypeId,      
 FR.UoM,      
 I.InvoiceTypeId,      
 CI.DisplayInvoiceNumber AS OriginalInvoiceNumber,      
 FR.FuelTypeId AS FRFuelTypeId,      
 B.FuelTypeId AS BOLFuelTypeId,      
 @StaticOriginVendorId as StaticOriginVendorId -- static value from MstAppSettings      
 FROM    InvoiceHeaderDetails IH        
 INNER JOIN Invoices I ON IH.Id = I.InvoiceHeaderId        
 INNER JOIN InvoiceXAdditionalDetails IAD ON I.Id = IAD.InvoiceId        
 INNER JOIN Orders O ON I.OrderId = O.Id        
 INNER JOIN FuelRequests FR ON O.FuelRequestId = FR.Id        
 INNER JOIN FuelRequestDetails FRD ON FR.Id = FRD.FuelRequestId        
 INNER JOIN MstProducts P ON FR.FuelTypeId = P.Id        
 INNER JOIN Jobs J ON FR.JobId = J.Id         
 INNER JOIN Companies S ON O.AcceptedCompanyId = S.Id        
 INNER JOIN Companies C ON O.BuyerCompanyId = C.Id        
 INNER JOIN InvoiceXBolDetails IBOL ON I.Id = IBOL.InvoiceId        
 INNER JOIN InvoiceFtlDetails B ON IBOL.BolDetailId = B.Id  --AND B.NetQuantity IS NOT NULL AND B.GrossQuantity IS NOT NULL   
 LEFT JOIN Invoices CI ON IAD.OriginalInvoiceId = CI.Id        
 LEFT JOIN ExternalCustomerMappings EC ON C.Id = EC.CustomerId AND EC.CreatedByCompanyId = S.Id AND EC.ThirdPartyId = @ThirdPartyId      
 LEFT JOIN ExternalCustomerLocationMappings ECL ON J.Id = ECL.CustomerLocationId AND ECL.CreatedByCompanyId = S.Id  AND ECL.ThirdPartyId = @ThirdPartyId      
 LEFT JOIN ExternalProductMappings EP ON (P.TfxProductId = EP.TfxProductId OR P.Id = EP.OtherProductId) AND EP.CreatedByCompanyId = S.Id AND EP.ThirdPartyId = @ThirdPartyId      
    LEFT JOIN ExternalSupplierMappings ES ON S.Id = ES.SupplierId AND ES.CreatedByCompanyId = S.Id AND ES.ThirdPartyId = @ThirdPartyId      
 LEFT JOIN ExternalTerminalMappings ET ON B.TerminalId = ET.TerminalId  AND ET.CreatedCompanyId = S.Id AND ET.ThirdPartyId = @ThirdPartyId      
 LEFT JOIN BulkPlantLocations BL ON B.SiteName = BL.Name AND BL.CompanyId = S.Id AND BL.IsActive = 1      
    LEFT JOIN ExternalBulkPlantMappings EB ON BL.Id = EB.BulkPlantId AND EB.CreatedByCompanyId = S.Id        
 LEFT JOIN Users D ON I.DriverId = D.Id        
 LEFT JOIN ExternalDriverMappings EDM ON I.DriverId = EDM.DriverId -- NEW JOIN       
 LEFT JOIN DeliveryScheduleXTrackableSchedules TS ON I.TrackableScheduleId = TS.Id        
 LEFT JOIN SupplierXBuyerDetails SBJ ON O.AcceptedCompanyId = SBJ.SupplierCompanyId AND O.BuyerCompanyId = SBJ.BuyerCompanyId AND J.Id = SBJ.JobId AND SBJ.IsActive = 1        
 LEFT JOIN Companies CAR ON CAR.Name = B.Carrier      
 LEFT JOIN ExternalCarrierMappings ECAR ON CAR.Id = ECAR.CarrierId      
 LEFT JOIN OrderAdditionalDetails OAD(NOLOCK) ON O.Id = OAD.OrderId       
 LEFT JOIN FuelRequestPricingDetails FRPD(NOLOCK) ON O.FuelRequestId = FRPD.FuelRequestId      
 WHERE I.InvoiceHeaderId =  @InvoiceHeaderId AND I.InvoiceVersionStatusId = 1       
 AND I.DroppedGallons != 0      
 AND (OAD.BOLInvoicePreferenceId IS NOT NULL AND OAD.BOLInvoicePreferenceId = 6) -- PDI      
 AND B.IsActive = 1 and B.RecordHistory IS NULL
END      
GO

-- EXEC [dbo].[usp_GetDeliveryDetailsforSAP]  848585  
CREATE OR ALTER  PROCEDURE [dbo].[usp_GetDeliveryDetailsforSAP]        
@InvoiceHeaderId INT        
AS        
BEGIN        
 SELECT         
 --INV.PoNumber as TFXOrderNo,        
   DSTS.UoM AS UoM,        
 JBS.DisplayJobID AS LocationID,        
  ISNULL(IFD.BolNumber,IFD.LiftTicketNumber) AS LiftTicketNo,        
  INV.DisplayInvoiceNumber AS DropTicketNo,        
  CONVERT(NCHAR(8),ISNULL(IFD.LiftDate, INV.CreatedDate),112) AS LiftDate,        
 CASE WHEN IFD.LiftStartTime IS NULL THEN REPLACE(CONVERT(VARCHAR(8), IFD.CreatedDate, 108),':','')     
  ELSE REPLACE(CONVERT(VARCHAR(8), IFD.LiftStartTime, 108),':','') END AS LiftStartTime,    
 CASE WHEN IFD.LiftEndTime IS NULL THEN REPLACE(CONVERT(VARCHAR(8), IFD.CreatedDate, 108),':','')     
  ELSE REPLACE(CONVERT(VARCHAR(8), IFD.LiftEndTime, 108),':','') END AS LiftEndTime,    
 CASE WHEN IFD.DeliveredQuantity IS NULL THEN INV.DroppedGallons ELSE IFD.DeliveredQuantity END AS TotalDropQuantity,        
  ISNULL(SMP.MyProductId, MP.DisplayName) as ProductID,        
  --IFD.PricePerGallon AS Price,        
  ISNULL(CCM.CarrierAssignedCustomerId,'--') AS CustomerID ,        
  CASE         
     WHEN IFD.PickupLocation = 1 THEN         
   ISNULL(TCA.AssignedTerminalId,MET.Name)         
        WHEN IFD.PickupLocation = 2 THEN        
   ISNULL(TCAFORLIFT.AssignedTerminalId,BPL.Name)        
   END AS TerminalControl,     
   JSON_VALUE(DSTS.AdditionalInfo, '$.Sap_OrderNo') AS SAPOrdNumber,      
   JSON_VALUE(DSTS.AdditionalInfo, '$.FsTrailerDisplayId') AS TruckId,      
   JSON_VALUE(DSTS.AdditionalInfo, '$.UniqueOrderNo') AS TFXOrderNo,      
   CAST(JSON_VALUE(DSTS.AdditionalInfo, '$.DR_Price') AS DECIMAL(18, 8))  AS Price      
  FROM Jobs JBS        
  INNER JOIN FuelRequests FR ON JBS.Id = FR.JobId        
  INNER JOIN Orders ORD ON ORD.FuelRequestId = FR.Id        
  INNER JOIN Invoices INV ON INV.OrderId = ORD.Id        
  INNER JOIN InvoiceXBolDetails INVXBOL ON INVXBOL.InvoiceId = INV.Id        
  INNER JOIN InvoiceFtlDetails IFD ON INVXBOL.BolDetailId = IFD.Id AND IFD.NetQuantity IS NOT NULL AND IFD.GrossQuantity IS NOT NULL          
  INNER JOIN DeliveryScheduleXTrackableSchedules DSTS ON INV.TrackableScheduleId = DSTS.Id AND DSTS.FrDeliveryRequestId IS NOT NULL      
  LEFT JOIN MstExternalTerminals MET ON MET.Id = IFD.TerminalId        
  LEFT JOIN BulkPlantLocations(NOLOCK) BPL  ON IFD.SiteName=BPL.Name and BPL.IsActive = 1 and BPL.CompanyId=ORD.AcceptedCompanyId      
  LEFT JOIN MstProducts MP ON MP.Id = FR.FuelTypeId     
  LEFT JOIN MstTfxProducts MTP ON MP.TfxProductId = MTP.Id    
  LEFT JOIN SupplierMappedProductDetails SMP ON SMP.CompanyId = ORD.AcceptedCompanyId AND SMP.IsActive =1 AND SMP.TerminalId IS NULL AND SMP.FuelTypeId =  MTP.Id -- IN SMP we have Tfx ProductId         
  LEFT JOIN CarrierCustomerMappings CCM ON CCM.CarrierCompanyId = ORD.AcceptedCompanyId AND CCM.CarrierCustomerId = ORD.BuyerCompanyId AND CCM.IsActive = 1        
  LEFT  JOIN TerminalCompanyAlias(NOLOCK) TCA ON TCA.CreatedByCompanyId = ORD.AcceptedCompanyId AND TCA.IsActive = 1 AND IFD.TerminalId = TCA.TerminalId   AND TCA.TerminalId IS NOT NULL             
  LEFT  JOIN  dbo.TerminalCompanyAlias(NOLOCK) TCAFORLIFT ON TCAFORLIFT.CreatedByCompanyId = ORD.AcceptedCompanyId               
       AND BPL.Id = TCAFORLIFT.BulkPlantId AND  TCAFORLIFT.IsActive = 1 AND TCAFORLIFT.IsBulkPlant = 1 AND IFD.LiftTicketNumber  IS NOT NULL        
  WHERE INV.InvoiceHeaderId = @InvoiceHeaderId        
  AND INV.InvoiceVersionStatusId = 1        
END    
GO




GO
/****** Object:  StoredProcedure [dbo].[usp_GetSupplierInvoiceDetail]    Script Date: 5/2/2022 9:25:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER      PROCEDURE [dbo].[usp_GetSupplierInvoiceDetail] --EXEC [usp_GetSupplierInvoiceDetail] 399946,2929   EXEC [usp_DummyGetSupplierInvoiceDetail] 399946,2929,330862       
 @InvoiceId INT,        
 @CompanyId INT 
 
AS        
BEGIN        
DECLARE @AssetCount INT = 0        
DECLARE @StatementId INT = 0        
DECLARE @StatementNumber NVARCHAR(11)        
DECLARE @True BIT = 1;        
DECLARE @False BIT = 0;        
Declare @IsSingleBolInvoice BIT = 0;        
SELECT @AssetCount = COUNT(DISTINCT JobXAssetId) FROM AssetDrops WHERE InvoiceId = @InvoiceId AND DropStatus = 0         
SELECT @StatementId = S.Id, @StatementNumber = N.Number FROM  BillingStatementXInvoices I INNER JOIN BillingStatements S ON I.StatementId = S.Id        
INNER JOIN StatementNumbers N ON S.StatementNumberId = N.Id        
WHERE I.InvoiceId = @InvoiceId AND I.IsActive = 1 AND S.IsGenerated = 1        
---------------------------- Invoice Payment -------------------------------------------        
DECLARE @PaymentStatus INT = 0        
DECLARE @AmountPaid DECIMAL(18,8) = 0        
DECLARE @BalanceRemaining DECIMAL(18,8) = 0        
-- Added isSingleBolInvoice check to show Edit button on UI        
Select @IsSingleBolInvoice = CASE WHEN Count(INVS.Id) > 1 Then 0 Else 1 END from Invoices INVS        
INNER JOIN InvoiceXBolDetails IBL ON IBL.InvoiceHeaderId = INVS.InvoiceHeaderId        
Where INVS.Id = @InvoiceId        
SELECT @AmountPaid = ROUND(SUM(ISNULL(IPAY.AmountPaid, 0)), 2),        
    @BalanceRemaining = ROUND(MIN(ISNULL(IPAY.BalanceRemaining, 0)), 2)        
FROM Invoices INV          
INNER JOIN InvoiceHeaderDetails IH ON INV.InvoiceHeaderId = IH.Id        
INNER JOIN InvoicePayments IPAY ON IH.InvoiceNumberId = IPAY.InvoiceNumberId AND Inv.QbInvoiceNumber = IPay.QbInvoiceNumber        
WHERE INV.Id = @InvoiceId        
DECLARE @CreditInvoiceId INT        
DECLARE @CreditInvoiceDisplayNumber VARCHAR(256)        
SELECT @CreditInvoiceId = CI.Id, @CreditInvoiceDisplayNumber = CI.DisplayInvoiceNumber        
FROM Invoices INV          
INNER JOIN InvoiceXAdditionalDetails IAD ON INV.Id = IAD.InvoiceId        
INNER JOIN InvoiceXAdditionalDetails CIAD ON IAD.OriginalInvoiceId = CIAD.OriginalInvoiceId        
INNER JOIN Invoices CI ON CIAD.InvoiceId = CI.Id        
WHERE INV.Id = @InvoiceId and CI.InvoiceTypeId = 11        
SET @PaymentStatus = CASE WHEN @BalanceRemaining = 0 THEN 2 --Paid        
        WHEN @BalanceRemaining > 0 THEN 1 --Partially Paid        
        ELSE 0 -- Not Paid        
      END        
DECLARE @IsExceptionDdt BIT = (SELECT TOP 1 1 FROM InvoiceExceptions WHERE InvoiceId = @InvoiceId)        
SET @IsExceptionDdt = ISNULL(@IsExceptionDdt, 0)       
------------------------------      
declare @invoiceHeaderId INT=(select top 1 invoiceheaderId from invoices where id=@InvoiceId);      
declare @bolFIlaepath nvarchar(max)      
 set @bolFIlaepath=(      
select      
   distinct        
    stuff((      
      select  ' || ' +filePath from images i      
inner join InvoiceFtlDetails IFD on i.id=IFD.imageId      
inner join InvoiceXBolDetails IBD on IBD.BolDetailId=IFD.id      
where IBD.invoiceHeaderId=@invoiceHeaderId    
      and IFD.IsActive = 1
        order by filePath      
        for xml path('')      
    ),1,1,'')       
from images      
group by filePath)      
set @bolFIlaepath=LTRIM(right(@bolFIlaepath, len(@bolFIlaepath)-2));      
      
--------------------      
declare @dropImgpath varchar(max)      
 set @dropImgpath=(      
select      
   distinct        
    stuff((      
      select  ' || ' +i.filePath from images i      
inner join invoices inv on i.id=inv.imageId      
where inv.invoiceHeaderId=@invoiceHeaderId      
        order by i.filePath      
        for xml path('')      
    ),1,1,'')       
from images      
group by filePath)      
set @dropImgpath=LTRIM(right(@dropImgpath, len(@dropImgpath)-2));      
-------------------      
declare @sigImg varchar(max)      
 set @sigImg=(      
select      
   distinct        
    stuff((      
      select  ' || ' +i.filePath from images i   
   inner join Signatures s on s.imageId=i.id      
      inner join invoices inv on s.id=inv.SignatureId      
      where inv.invoiceHeaderId=@invoiceHeaderId      
 order by i.filePath      
        for xml path('')      
    ),1,1,'')       
from images      
group by filePath)      
set @sigImg=LTRIM(right(@sigImg, len(@sigImg)-2));      
      
----------------------      
declare @addImgpath varchar(max)      
 set @addImgpath=(      
select      
   distinct        
    stuff((      
      select  ' || ' +i.filePath from images i      
   inner join InvoiceXAdditionalDetails ixa on ixa.AdditionalImageId=i.id      
inner join invoices inv on ixa.invoiceId=inv.id      
where inv.invoiceHeaderId=@invoiceHeaderId      
        order by i.filePath      
        for xml path('')      
    ),1,1,'')       
from images      
group by filePath)      
set @addImgpath=LTRIM(right(@addImgpath, len(@addImgpath)-2));      
      


SELECT         
I.Id,        
I.InvoiceVersionStatusId,        
--I.DisplayInvoiceNumber,
I.DisplayInvoiceNumber AS DisplayInvoiceNumber,
I.ReferenceId,        
I.InvoiceTypeId,        
I.OrderId,        
I.ParentId,        
OXTP.IsHidePricingEnabledForBuyer,        
OXTP.IsHidePricingEnabledForSupplier,        
I.DroppedGallons,        
ISNULL(FTL.PricePerGallon,0) AS PricePerGallon,        
FR.Id AS FuelRequestId,        
I.StateTax,        
I.FedTax,        
I.SalesTax,        
I.BasicAmount,        
ISNULL(I.TotalFeeAmount, 0 ) AS TotalFeeAmount,        
--I.PaymentDueDate, 
(SELECT TOP 1 INV.PaymentDueDate FROM Invoices INV where INV.InvoiceHeaderId = I.InvoiceHeaderId ORDER BY INV.PaymentDueDate desc) as PaymentDueDate,
I.PaymentDate,        
IXS.StatusId,        
FTL.CityGroupTerminalId,        
I.TerminalId,        
I.CreatedDate,        
I.PaymentTermId,        
I.NetDays,        
ISNULL(AD.Notes, OAD.Notes) AS Notes,        
I.TotalTaxAmount,        
I.TotalDiscountAmount,        
I.WaitingFor,        
J.IsApprovalWorkflowEnabled,        
I.IsBuyPriceInvoice,        
I.Currency,        
I.UoM,        
(        
  IH.TotalBasicAmount - IH.TotalDiscountAmount +         
  (CASE WHEN I.InvoiceTypeId = 5 THEN 0 ELSE ISNULL(IH.TotalFeeAmount, 0) END) +        
  (CASE WHEN (I.InvoiceTypeId = 6 OR I.InvoiceTypeId = 7) THEN 0 ELSE IH.TotalTaxAmount END)        
) AS TotalInvoiceAmount,        
CASE WHEN EXISTS(SELECT 1 FROM Discounts D WHERE D.InvoiceId = @InvoiceId AND D.DealStatus = 1 AND D.CreatedCompanyId<> @CompanyId) THEN @True ELSE @False END AS IsPendingInvoiceAdjustment,        
I.PoNumber,        
I.ImageId,        
@dropImgpath AS ImagePath,      
--INVIMG.FilePath AS ImagePath,        
O.BuyerCompanyId,        
Sgn.ImageId AS SignImageId,        
CASE WHEN FuelRequestTypeId =2 THEN dbo.usf_GetParentFuelRequestTypeId(FR.Id) ELSE FuelRequestTypeId END AS FuelRequestTypeId,        
DriverComment,        
CASE WHEN ISNULL(I.ParentId,0) = 0 OR PIXS.StatusId = 1 THEN Child.Id ELSE Par.Id END AS LinkedInvoiceId,        
CASE WHEN ISNULL(I.ParentId,0) = 0 OR PIXS.StatusId = 1 THEN Child.InvoiceTypeId ELSE Par.InvoiceTypeId END AS LinkedInvoiceType,        
CASE WHEN ISNULL(I.ParentId,0) = 0 OR PIXS.StatusId = 1 THEN Child.DisplayInvoiceNumber ELSE Par.DisplayInvoiceNumber END AS LinkedInvoiceNumber,        
FR.FuelDescription AS NonStandardFuelDescription,        
CASE WHEN FR.ProductDisplayGroupId = 4 THEN P.ProductDisplayGroupId ELSE FR.ProductDisplayGroupId END AS FuelDisplayGroupId,        
FR.QuantityTypeId,        
FR.MaxQuantity,        
O.BrokeredMaxQuantity,        
ISNULL(J.UoM, 0) AS FuelQuantityUoM,        
FR.PricingTypeId,        
CT.Name + ', ' + CT.StateCode AS CityGroupTerminalName,        
FR.CreationTimeRackPPG AS RequestPricePerGallon,        
ISNULL(FTL.RackPrice, 0) AS RackPrice,        
AD.PaymentMethod,        
FR.OrderTypeId,        
FTL.BolNumber,        
FTL.LiftQuantity,        
FTL.LiftTicketNumber,        
FTL.NetQuantity,        
FTL.LiftDate,        
FTL.GrossQuantity,
FTL.DeliveredQuantity, 
FTL.Carrier,   
ISNULL(FTL.IsLiftFileValidated,0) AS IsLiftFileValidated,        
FTL.ImageId AS BolImageId,       
 @bolFIlaepath AS BolFilePath,      
--FTLIMG.FilePath AS BolFilePath,        
AD.AdditionalImageId AS AdditionalImageId,        
@addImgpath AS AdditionalImgFilePath,      
--AddImg.FilePath AS AdditionalImgFilePath,        
FR.IsPublicRequest,        
FRD.DeliveryTypeId,        
TS.Date AS ScheduleDate,        
TS.StartTime,        
TS.EndTime,        
TS.Quantity, 

I.TrackableScheduleId,        
iSNULL(O.IsFtl, @False) AS IsFtl,        
EB.CompanyName AS ExternalBrokerName,        
CASE WHEN BSD.OrderId IS NOT NULL THEN @True ELSE @False END AS IsBuyAndSellOrder,        
I.FilePath,        
BSD.BrokerMarkUp,        
BSD.SupplierMarkUp,        
JXU.UserId AS ApprovalUserId,        
J.TimeZoneName,        
I.DropEndDate,        
ISNULL(O.ExternalBrokerId, 0) AS ExternalBrokerId,        
JB.IsTaxExempted,        
I.DropStartDate,        
CASE WHEN I.DriverId IS NULL THEN '--' ELSE D.FirstName  + ' '+ D.LastName END AS DriverName,        
I.SupplierPreferredInvoiceTypeId,        
ISNULL(T.Name, '--') AS TerminalName,        
T.Latitude AS TerminalLatitude,        
T.Longitude AS TerminalLongitude,        
J.Latitude AS JobLatitude,        
J.Longitude AS JobLongitude,        
Sgn.Id AS SignatureId,        
@sigImg AS CustomerSignaturePath,      
--ISNULL(CustImg.FilePath,'') AS CustomerSignaturePath,        
Sgn.SignatoryAvailable,        
Sgn.Signatory,        
IST.Name AS StatusName,        
AD.AssetFilled,        
AD.SplitLoadChainId,        
O.AcceptedCompanyId,        
ISNULL(TFXP.NAME,P.Name) AS FuelType,        
FTL.Id AS FtlDetailId,        
ISNULL(CU.Email, Cust.Email) AS CustomerEmail,        
ISNULL(CU.PhoneNumber, Cust.PhoneNumber) AS CustomerPhoneNumber,        
CASE WHEN CU.Id IS NULL THEN Cust.FirstName + ' ' + Cust.LastName ELSE CU.FirstName + ' '+ CU.LastName END AS CustomerName,       
ISNULL(CU.Id, Cust.Id) CustomerId,    
ISNULL(CuCo.Name, CustCo.Name) AS BuyerCompanyName,        
@AssetCount AS AssetCount,        
@StatementId AS StatementId,        
@StatementNumber AS StatementNumber,        
@PaymentStatus AS PaymentStatus,        
ISNULL(@AmountPaid, 0) AS AmountPaid,        
ISNULL(@BalanceRemaining, 0) AS BalanceRemaining,        
FRPD.DisplayPrice,        
CASE WHEN OAD.BOLInvoicePreferenceId IN (2,3,5) THEN @True ELSE @False END AS SendDtnFile,        
CI.Id AS OriginalInvoiceId,        
CI.DisplayInvoiceNumber AS OriginalInvoiceNumber,        
@CreditInvoiceId AS CreditInvoiceId,        
@CreditInvoiceDisplayNumber AS CreditInvoiceDisplayNumber,        
AD.CreationMethod,        
AD.CarrierOrderId,        
I.DDTConversionReason,        
@IsExceptionDdt AS IsExceptionDdt,        
I.InvoiceHeaderId,        
@IsSingleBolInvoice AS IsSingleBolInvoice,        
OAD.PreferencesSettingId,    
OAD.IsSupressPricingEnabled AS IsSupressOrderPricing,    
--ISNULL(OPS.IsSupressOrderPricing, 0) AS IsSupressOrderPricing,        
I.BrokeredChainId,        
J.IsMarine AS IsMarineLocation,        
I.ConvertedQuantity AS ConvertedQuantity,        
J.CountryId as JobCountryId,       
OAD.IsManualBDNConfirmationRequired as IsBDNConfirmationRequired,    
OAD.IsManualInvoiceConfirmationRequired as IsInvoiceConfirmationRequired,
ISNULL(TS.DeliveryLevelPO,'') AS DeliveryLevelPO,
CASE WHEN J.IsMarine = 1 THEN BDR.BDRNumber ELSE '' END As BDRNumber
FROM Invoices I        
INNER JOIN InvoiceXInvoiceStatusDetails IXS ON I.Id = IXS.InvoiceId AND IXS.IsActive =1        
INNER JOIN MstInvoiceStatuses IST ON IXS.StatusId = IST.Id        
INNER JOIN InvoiceXAdditionalDetails AD ON I.Id = AD.InvoiceId        
LEFT JOIN Invoices CI ON AD.OriginalInvoiceId = CI.Id        
LEFT JOIN Orders O ON I.OrderId = O.Id        
LEFT JOIN OrderAdditionalDetails OAD ON O.Id = OAD.OrderId        
LEFT JOIN OrderXTogglePricingDetails OXTP ON O.Id = OXTP.OrderId        
LEFT JOIN FuelRequests FR ON O.FuelRequestId = FR.Id        
LEFT JOIN ExternalBrokerBuySellDetails BSD ON O.Id = BSD.OrderId        
LEFT JOIN ExternalBrokers EB ON BSD.ExternalBrokerId = EB.Id        
LEFT JOIN FuelRequestDetails FRD ON FR.Id=FRD.FuelRequestId  LEFT JOIN FuelRequestPricingDetails FRPD ON FR.Id=FRPD.FuelRequestId        
LEFT JOIN Users Cust ON FR.CreatedBy=Cust.Id        
LEFT JOIN Companies CustCo ON Cust.CompanyId =CustCo.Id        
LEFT JOIN CounterOffers CO ON FR.Id = CO.FuelRequestId AND (CO.BuyerStatus = 2 OR CO.SupplierStatus = 2)        
LEFT JOIN Users CU ON CO.BuyerId = CU.Id        
LEFT JOIN Companies CuCo ON CU.CompanyId = CuCo.Id        
LEFT JOIN MstProducts P ON FR.FuelTypeId = P.Id        
LEFT JOIN MstTfxProducts TFXP  ON TFXP.Id = P.TfxProductId         
LEFT JOIN Jobs J ON FR.JobId = J.Id        
LEFT JOIN JobBudgets JB ON J.JobBudgetId = JB.Id        
LEFT JOIN JobXApprovalUsers JXU ON J.Id = JXU.JobId AND JXU.IsActive = 1        
LEFT JOIN Invoices Par ON I.ParentId = Par.Id        
LEFT JOIN InvoiceXInvoiceStatusDetails PIXS ON Par.Id = PIXS.InvoiceId AND PIXS.IsActive =1        
LEFT JOIN MstInvoiceStatuses PST ON PIXS.StatusId = PST.Id        
LEFT JOIN Invoices Child ON I.Id = Child.ParentId AND Child.InvoiceVersionStatusId = 1        
LEFT JOIN DeliveryScheduleXTrackableSchedules TS ON I.TrackableScheduleId = TS.Id        
LEFT JOIN Users D ON I.DriverId = D.Id        
LEFT JOIN InvoiceXBolDetails InvBol ON I.Id = InvBol.InvoiceId        
LEFT JOIN InvoiceFtlDetails FTL ON InvBol.BolDetailId = FTL.Id        
LEFT JOIN Signatures Sgn ON I.SignatureId = Sgn.Id        
LEFT JOIN MstExternalTerminals T ON I.TerminalId = T.Id        
LEFT JOIN MstExternalTerminals CT ON FTL.CityGroupTerminalId = CT.Id        
LEFT JOIN Images INVIMG ON I.ImageId = INVIMG.Id        
LEFT JOIN Images FTLIMG ON FTL.ImageId = FTLIMG.Id        
LEFT JOIN Images AddImg ON AD.AdditionalImageId = AddImg.Id        
LEFT JOIN Images CustImg ON CustImg.Id = Sgn.ImageId        
LEFT JOIN InvoiceHeaderDetails IH ON I.InvoiceHeaderId = IH.Id         
LEFT JOIN OnboardingPreferences OPS ON OAD.PreferencesSettingId = OPS.Id 
LEFT JOIN BDRDetails BDR ON BDR.InvoiceId = I.Id
 WHERE I.Id = @InvoiceId        
END 
Go