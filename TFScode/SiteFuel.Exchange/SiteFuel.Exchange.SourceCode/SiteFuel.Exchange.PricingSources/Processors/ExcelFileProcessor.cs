using ExcelDataReader;
using SiteFuel.Exchange.PricingSources.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.PricingSources.Processors
{
    public class ExcelFileProcessor
    {
        private bool _startReading = false;
        public ExcelFileProcessor(List<string> filePaths)
        {
            if (filePaths != null)
            {
                Files = filePaths;
            }
        }
        public List<string> Files { get; set; } = new List<string>();
        public List<string> ProcessFiles()
        {
            var response = new List<string>();
            try
            {
                foreach (var filePath in Files)
                {
                    ExtractAndSaveData(filePath);
                    response.Add(filePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        private void ExtractAndSaveData(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var excelReader = ExcelReaderFactory.CreateReader(stream))
                {
                    using (var sqlConnection = DatabaseConnection.GetSqlConnection())
                    {
                        sqlConnection.Open();
                        using (var sqlTransaction = sqlConnection.BeginTransaction())
                        {
                            try
                            {
                                do
                                {
                                    while (excelReader.Read())
                                    {
                                        // Skip rows till header defined in excel file
                                        if (!_startReading)
                                        {
                                            _startReading = excelReader.GetString(0) == "Symbol";
                                            continue;
                                        }
                                        PricingHitory excelRow = GetData(excelReader);
                                        SaveData(excelRow, sqlConnection, sqlTransaction);
                                    }
                                }
                                while (excelReader.NextResult());
                                sqlTransaction.Commit();
                            }
                            catch
                            {
                                sqlTransaction.Rollback();
                                throw;
                            }
                        }
                    }
                }
            }
        }

        private static PricingHitory GetData(IExcelDataReader excelReader)
        {
            var rowObject = new PricingHitory();
            rowObject.Symbol = excelReader.GetString(0);
            rowObject.LoadDate = excelReader.IsDBNull(1) ? null : (DateTimeOffset?)excelReader.GetDateTime(1);
            rowObject.Feed = excelReader.GetString(2);
            rowObject.ReportedDate = excelReader.IsDBNull(3) ? null : (DateTimeOffset?)excelReader.GetDateTime(3);
            rowObject.Price = excelReader.IsDBNull(4) ? null : (decimal?)excelReader.GetDouble(4);
            rowObject.Unit = excelReader.GetString(5);
            rowObject.Currency = excelReader.GetString(6);
            rowObject.SupplierNumber = excelReader.IsDBNull(7) ? null : (int?)excelReader.GetDouble(7);
            rowObject.Supplier = excelReader.GetString(8);
            rowObject.SupplierBrand = excelReader.GetString(9);
            rowObject.PriceType = excelReader.GetString(10);
            rowObject.LiftPoint = excelReader.GetString(11);
            rowObject.Product_ID = excelReader.IsDBNull(12) ? null : (int?)excelReader.GetDouble(12);
            rowObject.ProductGroup = excelReader.GetString(13);
            rowObject.ProductDescription = excelReader.GetString(14);
            rowObject.Source = excelReader.GetString(15);
            rowObject.StateCode = excelReader.GetString(16);
            return rowObject;
        }

        private void SaveData(PricingHitory pricingHitory, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            var sqlQuery = $"INSERT INTO [dbo].[ExternalPricingHistories] (Symbol,LoadDate,Feed,ReportedDate,Price,Unit,Currency,SupplierNumber,Supplier,SupplierBrand,PriceType,LiftPoint,Product_ID,ProductGroup,ProductDescription,Source,StateCode) " +
                $"VALUES (@Symbol,@LoadDate,@Feed,@ReportedDate,@Price,@Unit,@Currency,@SupplierNumber,@Supplier,@SupplierBrand,@PriceType,@LiftPoint,@Product_ID,@ProductGroup,@ProductDescription,@Source,@StateCode)";

            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.Transaction = sqlTransaction;
            sqlCommand.CommandText = sqlQuery;
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

            sqlCommand.ExecuteNonQuery();
        }
    }
}
