using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.PricingSources.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    public class CsvFileProcessor
    {
        private readonly int _maxLines = 100;
        private readonly string _delimiter = ",";

        static readonly string sqlQUery = "INSERT INTO [dbo].[ExternalPricingHistories] (Symbol,LoadDate,Feed,ReportedDate,Price,Unit,Currency,SupplierNumber,Supplier,SupplierBrand,PriceType,LiftPoint,Product_ID,ProductGroup,ProductDescription,Source,StateCode,FileId)"
             + "VALUES (@Symbol,CAST(@LoadDate AS DATE),@Feed,@ReportedDate,@Price,@Unit,@Currency,@SupplierNumber,@Supplier,@SupplierBrand,@PriceType,@LiftPoint,@Product_ID,@ProductGroup,@ProductDescription,@Source,@StateCode,@FileId);";

        static readonly string opisSqlQuery = "INSERT INTO [dbo].[ExternalOPISPricingHistories] (ProductIndicator,ProductType,LoadDate,State,City,Supplier,BrandIndicator,Terms,GrossPrice,UniqueMarker,OctaneLevel,ActualProduct,RVP,DieselBlend,BioType,FileId,ReportedDate, ProductDescription, ProductCode)"
         + "VALUES (@ProductIndicator,@ProductType, CAST(@LoadDate AS DATE), @State, @City, @Supplier, @BrandIndicator, @Terms, @GrossPrice, @UniqueMarker, @OctaneLevel, @ActualProduct, @RVP, @DieselBlend, @BioType, @FileId, @ReportedDate, @ProductDescription, @ProductCode);";

        public List<int> ProcessFiles(List<PendingFile> PendingFiles)
        {
            var response = new List<int>();
            try
            {
                foreach (var file in PendingFiles.Where(x => x.IsSaved))
                {
                    var startTime = DateTimeOffset.Now;
                    var inserted = ProcessFileData(file.FullPath, file.ID);
                    if (inserted > 0)
                    {
                        response.Add(file.ID);
                    }
                    var endTime = DateTimeOffset.Now;
                    var logInfo = "Processed[Time Taken " + (endTime - startTime).TotalSeconds
                        + " Seconds][Record Inserted: " + inserted + "]: " + file.FullPath;
                    LogManager.Logger.WriteInfo("CsvFileProcessor", "ProcessFiles", logInfo);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CsvFileProcessor", "ProcessFiles", ex.Message, ex);
            }
            return response;
        }

        private int ProcessFileData(string filePath, int fileId)
        {
            int totalInserted = 0;
            bool searchHeader = true;
            var compareInfo = CultureInfo.CurrentCulture.CompareInfo;
            int skipLines = 0, currentLineCount = 0;
            var headerColumnList = new List<string>();

            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    string debugLineInfo = string.Empty;
                    try
                    {
                        DateTimeOffset reportedDate = DateTimeOffset.Now;
                        do
                        {
                            var lines = File.ReadLines(filePath).Skip(skipLines).Take(_maxLines).ToList();
                            if (searchHeader)
                            {
                                headerColumnList = GetHeaderColumns(compareInfo, _maxLines, ref lines);
                                searchHeader = false;
                            }
                            foreach (var line in lines)
                            {
                                if (line.Contains("Gas 87 (Low RVP; not 7.8, 7.0)"))
                                {
                                    continue;
                                }
                                debugLineInfo = line;
                                var priceObj = GetLineData(headerColumnList, line);
                                if (priceObj != null)
                                {
                                    priceObj.FileId = fileId;
                                    priceObj.ReportedDate = reportedDate;
                                    SaveData(priceObj, sqlConnection, sqlTransaction);
                                    totalInserted++;
                                }
                            }
                            skipLines += _maxLines;
                            currentLineCount = lines.Count;
                        }
                        while (currentLineCount > 0);
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        var errorMessage = $"{filePath} : {debugLineInfo} : {ex.Message}";
                        LogManager.Logger.WriteException("CsvFileProcessor", "ProcessFileData", $"{errorMessage}", ex);
                        EmailProcessor.ProcessEmail("Syncing Failed : " + errorMessage);
                    }
                }
            }
            return totalInserted;
        }

        /// <summary>
        /// Processes the opis file data.
        /// </summary>
        /// <param name="fileData">The file path.</param>
        /// <param name="fileId">The file identifier.</param>
        /// <returns><see cref="int"/>Return count for the processed file.</returns>
        public int ProcessOPISFileData(string fileData, int fileId, List<OPISProductMappingInfo> productMappingInfo)
        {
            int totalInsertedCount = 0;
            bool searchHeader = true;
            var compareInfo = CultureInfo.CurrentCulture.CompareInfo;
            int skipLines = 0, currentLineCount = 0;
            var headerColumnList = new List<string>();

            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                sqlConnection.Open();
                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    string debugLineInfo = string.Empty;
                    try
                    {
                        var reportedDate = DateTimeOffset.Now;
                        do
                        {
                            var lines = fileData.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Skip(skipLines).Take(_maxLines).ToList();
                            if (searchHeader)
                            {
                                headerColumnList = this.GetHeaderColumns(compareInfo, _maxLines, ref lines, true);
                                searchHeader = false;
                            }
                            foreach (var line in lines)
                            {
                                debugLineInfo = line;
                                var opisPricingData = this.GetOPISLineData(headerColumnList, line);
                                if (opisPricingData != null)
                                {
                                    this.BindOPISData(fileId, productMappingInfo, reportedDate, opisPricingData);
                                    this.SaveOPISData(opisPricingData, sqlConnection, sqlTransaction);
                                    totalInsertedCount++;
                                }
                            }
                            skipLines += _maxLines;
                            currentLineCount = lines.Count();
                        }
                        while (currentLineCount > 0);
                        sqlTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaction.Rollback();
                        var errorMessage = $"{fileId} : {debugLineInfo} : {ex.Message}";
                        LogManager.Logger.WriteException("CsvFileProcessor", "ProcessOPISFileData", $"{errorMessage}", ex);
                        EmailProcessor.ProcessEmail("Syncing Failed : " + errorMessage);
                        LogManager.Logger.WriteException("CsvFileProcessor", "ProcessOPISFileData", "Service Type: SiteFuel.Exchange.OPISFileService", ex);
                    }
                }
            }
            return totalInsertedCount;
        }

        /// <summary>
        /// Binds the opis data.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="productMappingInfo">The product mapping information.</param>
        /// <param name="reportedDate">The reported date.</param>
        /// <param name="opisPricingData">The opis pricing data.</param>
        private void BindOPISData(int fileId, List<OPISProductMappingInfo> productMappingInfo, DateTimeOffset reportedDate, OPISPricingHistory opisPricingData)
        {
            opisPricingData.FileId = fileId;
            opisPricingData.ReportedDate = reportedDate;
            opisPricingData.ProductType = opisPricingData?.ProductType ?? string.Empty;
            opisPricingData.DieselBlend = opisPricingData?.DieselBlend ?? string.Empty;
            var opisProductDetails = this.GetOPISProductDetails(opisPricingData.ProductIndicator, opisPricingData.ProductType, opisPricingData.ActualProduct, opisPricingData.DieselBlend, productMappingInfo);
            opisPricingData.ProductDescription = opisProductDetails?.ProductDescription ?? string.Empty;
            opisPricingData.ProductCode = opisProductDetails?.ProductCode ?? string.Empty;
        }

        private List<string> GetHeaderColumns(CompareInfo compareInfo, int maxLines, ref List<string> lines, bool isOPISFile = false)
        {
            List<string> headerColumnList;
            // Skip lines till header lines and get on data lines
            var headerString = isOPISFile ? GetOPISHeaderColumn(compareInfo, lines) : GetHeaderColumn(compareInfo, lines);
            if (string.IsNullOrWhiteSpace(headerString))
            {
                throw new InvalidDataException("Header information is missing for this file");
            }
            headerColumnList = headerString.Split(new string[] { _delimiter }, StringSplitOptions.None).ToList();
            var headerIndex = lines.IndexOf(headerString);
            lines = lines.Skip(headerIndex + 1).Take(maxLines).ToList();
            return headerColumnList;
        }

        private static string GetHeaderColumn(CompareInfo compareInfo, List<string> lines)
        {
            var header = lines.FirstOrDefault(t => compareInfo.IndexOf(t, "Symbol", CompareOptions.OrdinalIgnoreCase) >= 0
                                    && compareInfo.IndexOf(t, "Load Date", CompareOptions.OrdinalIgnoreCase) >= 0);

            return header;
        }

        /// <summary>
        /// Gets the OPIS header column.
        /// </summary>
        /// <param name="compareInfo">The compare information.</param>
        /// <param name="lines">The lines.</param>
        /// <returns></returns>
        private static string GetOPISHeaderColumn(CompareInfo compareInfo, List<string> lines)
        {
            var header = lines.FirstOrDefault(t => compareInfo.IndexOf(t, "Product Indicator", CompareOptions.OrdinalIgnoreCase) >= 0
                                    && compareInfo.IndexOf(t, "Date(Daily)", CompareOptions.OrdinalIgnoreCase) >= 0);

            return header;
        }

        private PricingHitory GetLineData(List<string> headerColums, string lineString)
        {
            PricingHitory pricingHistory = new PricingHitory();
            try
            {
                var lineData = lineString.Split(new string[] { _delimiter }, StringSplitOptions.None);
                foreach (var columnName in headerColums)
                {
                    var colIndex = headerColums.IndexOf(columnName);
                    var colData = lineData[colIndex];
                    if (colData.Length == 0)
                    {
                        continue;
                    }

                    switch (columnName)
                    {
                        case "Symbol":
                            pricingHistory.Symbol = colData;
                            break;
                        case "Load Date":
                            pricingHistory.LoadDate = Convert.ToDateTime(colData);
                            break;
                        case "Feed":
                            pricingHistory.Feed = colData;
                            break;
                        case "Timestamp":
                            pricingHistory.ReportedDate = Convert.ToDateTime(colData);
                            break;
                        case "Price":
                            pricingHistory.Price = Convert.ToDecimal(colData);
                            break;
                        case "Unit":
                            pricingHistory.Unit = colData;
                            break;
                        case "Currency":
                            pricingHistory.Currency = colData;
                            break;
                        case "Supplier Number":
                            pricingHistory.SupplierNumber = Convert.ToInt32(colData);
                            break;
                        case "Supplier":
                            pricingHistory.Supplier = colData;
                            break;
                        case "Supplier Brand":
                            pricingHistory.SupplierBrand = colData;
                            break;
                        case "Price Type":
                            pricingHistory.PriceType = colData;
                            break;
                        case "Lift Point":
                            pricingHistory.LiftPoint = colData;
                            break;
                        case "Product_ID":
                            pricingHistory.Product_ID = Convert.ToInt32(colData);
                            break;
                        case "Product Group (JDE)":
                            pricingHistory.ProductGroup = colData;
                            break;
                        case "Product Description":
                            pricingHistory.ProductDescription = colData;
                            break;
                        case "Source":
                            pricingHistory.Source = colData;
                            break;
                        case "State":
                        case "Province":
                            pricingHistory.StateCode = colData;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CsvFileProcessor", "GetLineData", $"LineData[{lineString}] => " + ex.Message, ex);
                pricingHistory = null;
            }
            return pricingHistory;
        }

        /// <summary>
        /// Gets the OPIS line data.
        /// </summary>
        /// <param name="headerColums">The header colums.</param>
        /// <param name="lineString">The line string.</param>
        /// <returns><see cref="OPISPricingHistory"/>Return the pricing history details.</returns>
        private OPISPricingHistory GetOPISLineData(List<string> headerColums, string lineString)
        {
            OPISPricingHistory opisPricingHistory = null;
            try
            {
                var lineData = lineString.Split(new string[] { _delimiter }, StringSplitOptions.None);
                if (headerColums.Count > 0 && lineData.Count() > 0 && !string.IsNullOrWhiteSpace(lineString))
                {
                    opisPricingHistory = new OPISPricingHistory();

                    foreach (var columnName in headerColums)
                    {
                        var colIndex = headerColums.IndexOf(columnName);
                        var colData = lineData[colIndex].Trim();
                        if (colData.Length == 0)
                        {
                            continue;
                        }

                        switch (columnName.Trim())
                        {
                            case "Product Indicator":
                                opisPricingHistory.ProductIndicator = colData;
                                break;
                            case "Product Type":
                                opisPricingHistory.ProductType = colData;
                                break;
                            case "Date(Daily)":
                                opisPricingHistory.LoadDate = DateTime.ParseExact(colData, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                break;
                            case "State":
                                opisPricingHistory.State = colData;
                                break;
                            case "City":
                                opisPricingHistory.City = colData;
                                break;
                            case "Supplier Low/High/Average":
                                opisPricingHistory.Supplier = colData;
                                break;
                            case "Brand Indicator":
                                opisPricingHistory.BrandIndicator = colData;
                                break;
                            case "Terms":
                                opisPricingHistory.Terms = colData;
                                break;
                            case "Gross Price":
                                opisPricingHistory.GrossPrice = Convert.ToDecimal(colData);
                                break;
                            case "Unique Marker":
                                opisPricingHistory.UniqueMarker = colData;
                                break;
                            case "Octane Level":
                                opisPricingHistory.OctaneLevel = Convert.ToInt32(colData);
                                break;
                            case "Actual Product":
                                opisPricingHistory.ActualProduct = colData;
                                break;
                            case "RVP":
                                opisPricingHistory.RVP = Convert.ToDecimal(colData);
                                break;
                            case "Diesel Blend":
                                opisPricingHistory.DieselBlend = colData;
                                break;
                            case "Bio Type":
                                opisPricingHistory.BioType = colData;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("CsvFileProcessor", "GetOPISLineData", $"LineData[{lineString}] => " + ex.Message, ex);
                opisPricingHistory = null;
            }
            return opisPricingHistory;
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <param name="pricingHitory">The pricing hitory.</param>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="sqlTransaction">The SQL transaction.</param>
        private void SaveData(PricingHitory pricingHitory, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.CommandText = sqlQUery;
            sqlCommand.CommandType = System.Data.CommandType.Text;

            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Symbol", pricingHitory.Symbol));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@LoadDate", pricingHitory.LoadDate));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Feed", pricingHitory.Feed));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@ReportedDate", pricingHitory.ReportedDate));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Price", pricingHitory.Price));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Unit", pricingHitory.Unit));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Currency", pricingHitory.Currency));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@SupplierNumber", pricingHitory.SupplierNumber));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Supplier", pricingHitory.Supplier));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@SupplierBrand", pricingHitory.SupplierBrand));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@PriceType", pricingHitory.PriceType));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@LiftPoint", pricingHitory.LiftPoint));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Product_ID", pricingHitory.Product_ID));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProductGroup", pricingHitory.ProductGroup));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProductDescription", pricingHitory.ProductDescription));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@Source", pricingHitory.Source));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@StateCode", pricingHitory.StateCode));
            sqlCommand.Parameters.Add(SqlParam.GetParameter("@FileId", pricingHitory.FileId));

            sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Saves the opis data.
        /// </summary>
        /// <param name="opisPricingHistory">The opis pricing history model.</param>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="sqlTransaction">The SQL transaction.</param>
        private void SaveOPISData(OPISPricingHistory opisPricingHistory, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            try
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.Transaction = sqlTransaction;
                sqlCommand.CommandText = opisSqlQuery;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProductIndicator", opisPricingHistory.ProductIndicator));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProductType", opisPricingHistory.ProductType));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@LoadDate", opisPricingHistory.LoadDate));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@State", opisPricingHistory.State));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@City", opisPricingHistory.City));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@Supplier", opisPricingHistory.Supplier));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@BrandIndicator", opisPricingHistory.BrandIndicator));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@Terms", opisPricingHistory.Terms));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@GrossPrice", opisPricingHistory.GrossPrice));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@UniqueMarker", opisPricingHistory.UniqueMarker));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@OctaneLevel", opisPricingHistory.OctaneLevel));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@ActualProduct", opisPricingHistory.ActualProduct));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@RVP", opisPricingHistory.RVP));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@DieselBlend", opisPricingHistory.DieselBlend));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@BioType", opisPricingHistory.BioType));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@FileId", opisPricingHistory.FileId));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@ReportedDate", opisPricingHistory.ReportedDate));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProductDescription", opisPricingHistory.ProductDescription));
                sqlCommand.Parameters.Add(SqlParam.GetParameter("@ProductCode", opisPricingHistory.ProductCode));

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
               LogManager.Logger.WriteException("CsvFileProcessor", "SaveOPISData", $"Exception occured => " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the opis product mapped details.
        /// </summary>
        /// <param name="productGroup">The product group.</param>
        /// <param name="productType">Type of the product.</param>
        /// <param name="actualProduct">The actual product.</param>
        /// <param name="dieselBlend">The diesel blend.</param>
        /// <param name="productMappingInfo">The product mapping information.</param>
        /// <returns><see cref="OPISProductMappingInfo"/>Return the product description and product code.</returns>
        private OPISProductMappingInfo GetOPISProductDetails(string productGroup, string productType, string actualProduct, string dieselBlend, List<OPISProductMappingInfo> productMappingInfo)
        {
              var productMapdetails = productMappingInfo.Where(x => x.ProductGroup == productGroup && x.ProductType == productType && x.ProductName == actualProduct && x.DieselBlend == dieselBlend)
                                    .Select(p => new OPISProductMappingInfo
                                    {
                                        ProductCode = p.ProductCode,
                                        ProductDescription = p.ProductDescription
                                    }).FirstOrDefault();
            
              return productMapdetails;
        }

        /// <summary>
        /// Gets the opis product mapping information.
        /// </summary>
        /// <returns><see cref="List{OPISProductMappingInfo}"/>List of product mapping</returns>
        public List<OPISProductMappingInfo> GetOPISProductMappingInfo()
        {
            var opisProductMappingInfo = new List<OPISProductMappingInfo>();
            using (var sqlConnection = DatabaseConnection.GetSqlConnection())
            {
                var sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "usp_GetOPISProductMappingInfo";
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlConnection.Open();

                using (var reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var productMappingInfo = new OPISProductMappingInfo()
                        {
                            ProductGroup = reader["ProductGroup"].ToString(),
                            ProductType = reader["ProductType"].ToString(),
                            ProductName = reader["ActualProductName"].ToString(),
                            ProductDescription = reader["ProductDescription"].ToString(),
                            ProductCode = reader["ProductCode"].ToString(),
                            DieselBlend = reader["DieselBlend"].ToString(),
                        };

                        opisProductMappingInfo.Add(productMappingInfo);
                    }
                }
            }

            return opisProductMappingInfo;
        }
    }
}
