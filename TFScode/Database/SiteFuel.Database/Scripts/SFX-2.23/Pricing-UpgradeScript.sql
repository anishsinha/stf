
GO
/****** Object:  StoredProcedure [dbo].[usp_GetClosestTerminals]    Script Date: 04/28/2022 4:00:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE  [dbo].[usp_GetClosestTerminals]
	    @CountryId    INT = 0, 
        @FuelTypeId   INT = 0, 
        @JobLatitude  FLOAT = 0, 
        @JobLongitude FLOAT = 0,		
		@PricingCode INT,
		@Terminal     NVARCHAR(MAX) = '',
		@CompanyCountryId    INT = 1,
		@IsSuppressPricing BIT = 0
AS
BEGIN
DECLARE @ExternalProductId INT = 0
DECLARE @PricingSource INT
--DECLARE @OriginalCountyCode NVARCHAR(MAX)
DECLARE @TerminalCount INT = 20
--SELECT @OriginalCountyCode = Code FROM MstCountries WHERE Id=@CountryId
SELECT @PricingSource = PricingSourceId FROM MstPricingCodes WHERE Id = @PricingCode
IF EXISTS (SELECT 1 FROM [dbo].[MstPricingConfigs] WHERE [KEY]='TerminalCountInDropDown')
BEGIN
	SELECT @TerminalCount = CAST(ISNULL(VALUE,20) AS int) FROM [dbo].[MstPricingConfigs] WHERE [KEY]='TerminalCountInDropDown'
END
IF(@PricingSource = 1)
BEGIN
		--IF(@IsSuppressPricing = 0)
		--BEGIN
		--	SELECT @ExternalProductId = ExternalProductId FROM MstProductMappings WHERE ProductId = @FuelTypeId and IsActive = 1
		--	SELECT DISTINCT TOP (@TerminalCount) MET.Id, 
		--						   MET.Name, 
		--						   DBO.Usf_calculatedistance(MET.Latitude, MET.Longitude, @JobLatitude, @JobLongitude, @CompanyCountryId) AS Distance
		--	FROM   MstExternalTerminals MET 
		--		   JOIN ExternalPricingAxxis MPD 
		--			 ON MET.Abbreviation = MPD.TerminalAbbreviation 
		--		   JOIN MstProductMappings MPM 
		--			 ON MET.Id = MPM.ExternalTerminalId AND MPM.IsActive = 1
		--	WHERE  MET.Name LIKE '%'+@Terminal+'%' AND MPD.ProductId = @ExternalProductId AND MPM.ProductId = @FuelTypeId 
		--	--AND MET.CountryCode=@OriginalCountyCode
		--	ORDER  BY Distance
		--END
		--ELSE
		--BEGIN
			SELECT @ExternalProductId = ExternalProductId FROM MstProductMappings WHERE ProductId = @FuelTypeId and IsActive = 1
			SELECT DISTINCT TOP (@TerminalCount) MET.Id, 
								   MET.Name, 
								   DBO.Usf_calculatedistance(MET.Latitude, MET.Longitude, @JobLatitude, @JobLongitude, @CompanyCountryId) AS Distance
			FROM   MstExternalTerminals MET 
				   --JOIN ExternalPricingAxxis MPD ON MET.Abbreviation = MPD.TerminalAbbreviation 
				   JOIN MstProductMappings MPM 
					 ON MET.Id = MPM.ExternalTerminalId AND MPM.IsActive = 1
			WHERE  MET.Name LIKE '%'+@Terminal+'%' 
			--AND MPD.ProductId = @ExternalProductId 
			AND MPM.ProductId = @FuelTypeId 
			--AND MET.CountryCode=@OriginalCountyCode
			ORDER  BY Distance
		--END
END
ELSE
BEGIN
	SELECT DISTINCT TOP (@TerminalCount) MET.Id, 
						   MET.Name, 
						   DBO.Usf_calculatedistance(MET.Latitude, MET.Longitude, @JobLatitude, @JobLongitude, @CompanyCountryId) AS Distance
	FROM   MstExternalTerminals MET        
	WHERE  MET.Name LIKE '%'+@Terminal+'%' 
			--AND MET.CountryCode=@OriginalCountyCode
			AND MET.ControlNumber <> '-'
	ORDER  BY Distance
END
END
Go

GO
/****** Object:  StoredProcedure [dbo].[usp_GetClosestTerminalsForMultipleProducts]    Script Date: 04/28/2022 4:44:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--EXEC [dbo].[usp_GetClosestTerminalsForMultipleProducts] 1,'1,2,3',0,0,7  
CREATE OR ALTER PROCEDURE  [dbo].[usp_GetClosestTerminalsForMultipleProducts]  
     @CountryId    INT = 0,   
        @FuelTypeId   NVARCHAR(100),   
        @JobLatitude  FLOAT = 0,   
        @JobLongitude FLOAT = 0,    
  @PricingCode INT,  
  @Terminal     NVARCHAR(MAX) = '',  
  @CompanyCountryId INT = 1,  
  @IsSuppressPricing BIT = 0  
AS  
BEGIN  
 DECLARE @ExternalProductId INT = 0  
 DECLARE @PricingSource INT  
 DECLARE @OriginalCountyCode NVARCHAR(MAX)   
 DECLARE @ExternalProductIds TABLE (Id INT);  
 DECLARE @FuelTypeIds TABLE (Id INT);  
 DECLARE @TerminalCount INT = 20  
 SELECT @OriginalCountyCode = Code FROM MstCountries WHERE Id=@CountryId  
 SELECT @PricingSource = PricingSourceId FROM MstPricingCodes WHERE Id = @PricingCode  
   
 IF EXISTS (SELECT 1 FROM [dbo].[MstPricingConfigs] WHERE [KEY]='TerminalCountInDropDown')  
 BEGIN  
  SELECT @TerminalCount = CAST(ISNULL(VALUE,20) AS int) FROM [dbo].[MstPricingConfigs] WHERE [KEY]='TerminalCountInDropDown'  
 END  
 INSERT INTO @FuelTypeIds SELECT VALUE FROM STRING_SPLIT(@FuelTypeId, ',') ORDER BY VALUE;  
   
 IF(@PricingSource = 1)  
	 BEGIN    
	 INSERT INTO @ExternalProductIds   
		SELECT DISTINCT ExternalProductId  
		FROM MstProductMappings   
		WHERE ProductId IN (SELECT ID FROM @FuelTypeIds)  and IsActive = 1
	  --IF(@IsSuppressPricing = 0) -- EXISTING LOGIC WHEN SUPPRESS PRICING IS DISABLED  
		 -- BEGIN  
		 -- SELECT DISTINCT TOP (@TerminalCount) MET.Id,   
			--	  MET.Name,   
			--	  DBO.Usf_calculatedistance(MET.Latitude, MET.Longitude, @JobLatitude, @JobLongitude, ISNULL(@CompanyCountryId, @CountryId)) AS Distance  
		 -- FROM   MstExternalTerminals MET   
			--  JOIN ExternalPricingAxxis MPD   
			-- ON MET.Abbreviation = MPD.TerminalAbbreviation   
			--  JOIN MstProductMappings MPM   
			-- ON MET.Id = MPM.ExternalTerminalId  and MPM.IsActive = 1
			----INNER JOIN @ExternalProductIds EPD on mpm.ExternalProductId = EPD.Id  
		 -- WHERE  MET.Name LIKE '%'+ @Terminal +'%'   
		 --  AND MPD.ProductId IN (SELECT ID FROM @ExternalProductIds)  
		 --  AND MPM.ProductId IN (SELECT ID FROM @FuelTypeIds)  
		 --  AND MET.CountryCode=@OriginalCountyCode  
		 -- ORDER  BY Distance  
		 -- END  
	  --ELSE -- DONT CHECK PRICING WHEN SUPPRESS PRICING IS SET  
	 -- BEGIN  
	  SELECT DISTINCT TOP (@TerminalCount) MET.Id,   
			  MET.Name,   
			  DBO.Usf_calculatedistance(MET.Latitude, MET.Longitude, @JobLatitude, @JobLongitude, ISNULL(@CompanyCountryId, @CountryId)) AS Distance  
	  FROM   MstExternalTerminals MET   
		  --JOIN ExternalPricingAxxis MPD ON MET.Abbreviation = MPD.TerminalAbbreviation   
		  JOIN MstProductMappings MPM   
		 ON MET.Id = MPM.ExternalTerminalId  and MPM.IsActive = 1
		--INNER JOIN @ExternalProductIds EPD on mpm.ExternalProductId = EPD.Id  
	  WHERE  MET.Name LIKE '%'+ @Terminal +'%'   
	   --AND MPD.ProductId IN (SELECT ID FROM @ExternalProductIds)  
	   AND MPM.ProductId IN (SELECT ID FROM @FuelTypeIds)  
	   AND MET.CountryCode=@OriginalCountyCode  
	  ORDER  BY Distance  
	 -- END  
	 END  
 ELSE  
 BEGIN  
 SELECT DISTINCT TOP 10 MET.Id,   
         MET.Name,   
         DBO.Usf_calculatedistance(MET.Latitude, MET.Longitude, @JobLatitude, @JobLongitude, @CountryId) AS Distance  
 FROM   MstExternalTerminals MET          
 WHERE  MET.Name LIKE '%'+@Terminal+'%'
 --AND MET.CountryCode=@OriginalCountyCode 
 AND MET.ControlNumber <> '-'  
 ORDER  BY Distance  
 END  
END  
Go
