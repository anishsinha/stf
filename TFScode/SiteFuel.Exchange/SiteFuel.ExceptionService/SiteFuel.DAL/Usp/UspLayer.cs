using Microsoft.SqlServer.Server;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models.ApiModels;
using SiteFuel.Models.CompanyException;
using SiteFuel.Models.CustomerException;
using SiteFuel.Models.InvoiceException;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DAL.Usp
{
    public class UspLayer : BaseLayer
    {
        public UspLayer()
        {
        }
        public UspLayer(BaseLayer baseLayer) : base(baseLayer)
        {
        }

        public async Task<List<UspGetCompanyException>> GetCompanyExceptions(int ownerCompanyId, int timeout = 30)
        {
            var inputmodel = new { OwnerCompanyId = ownerCompanyId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetCompanyExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<UspGetCompanyException>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<UspGetCustomerException>> GetCustomerExceptions(int ownerCompanyId, int enabledForCompanyId, int timeout = 30)
        {
            var inputmodel = new { OwnerCompanyId = ownerCompanyId, EnabledForCompanyId = enabledForCompanyId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetCustomerExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<UspGetCustomerException>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }


        public async Task<List<RaisedExceptionModel>> GetRaisedExceptions(string exceptionTypeIds, int companyId, bool isBuyerCompany, int timeout = 30)
        {
            var inputmodel = new { ExceptionTypeIds= exceptionTypeIds, CompanyId= companyId, IsBuyerCompany= isBuyerCompany };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetRaisedExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<RaisedExceptionModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<EnabledExceptionModel>> GetEnabledExceptions(int ownerCompanyId, int enabledForCompanyId, List<BrokeredOrdersModel> brokers, int timeout = 30)
        {
            List<EnabledExceptionModel> response = new List<EnabledExceptionModel>();
            DataSet resultSet = new DataSet();
            using (var conn = Context.DataContext.Database.Connection)
            {
                conn.Open();
                var command = conn.CreateCommand() as SqlCommand;
                command.CommandText = "dbo.usp_GetEnabledExceptions";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@OwnerCompanyId",
                    SqlDbType = SqlDbType.Int,
                    Value = ownerCompanyId
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@EnabledForCompanyId",
                    SqlDbType = SqlDbType.Int,
                    Value = enabledForCompanyId
                });
                command.Parameters.Add(new SqlParameter()
                {
                    ParameterName = "@BrokeredUsers",
                    SqlDbType = SqlDbType.Structured,
                    Value = ConvertToDataTable(brokers, ownerCompanyId, enabledForCompanyId)
                });
                command.CommandTimeout = timeout;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(resultSet);
                if (resultSet.Tables.Count > 0 && resultSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i <= resultSet.Tables[0].Rows.Count - 1; i++)
                    {
                        response.Add(new EnabledExceptionModel() { ExceptionTypeId = (int)resultSet.Tables[0].Rows[i].ItemArray[0],
                            OwnerCompanyId = (int)resultSet.Tables[0].Rows[i].ItemArray[1],
                            ApproverCompanyId = (int)resultSet.Tables[0].Rows[i].ItemArray[2],
                            Threshold = (decimal)resultSet.Tables[0].Rows[i].ItemArray[3]
                        });
                    }
                }
            };
            return response;
        }

        public DataTable ConvertToDataTable(List<BrokeredOrdersModel> dataList, int ownerCompanyId, int enabledForCompanyId)
        {
            DataTable convertedTable = new DataTable();

            convertedTable.Columns.Add("BuyerCompanyId");
            convertedTable.Columns.Add("SupplierCompanyId");
            convertedTable.Columns.Add("ExceptionTypeId");
            if (dataList != null)
            {
                foreach (var item in dataList)
                {
                    if (!((item.BuyerCompanyId == ownerCompanyId || item.BuyerCompanyId == enabledForCompanyId) && (item.SupplierCompanyId == ownerCompanyId || item.SupplierCompanyId == enabledForCompanyId)))
                    {
                        var row = convertedTable.NewRow();
                        row["BuyerCompanyId"] = item.BuyerCompanyId;
                        row["SupplierCompanyId"] = item.SupplierCompanyId;
                        row["ExceptionTypeId"] = (int)ExceptionType.DeliveredQuantityVariance;
                        convertedTable.Rows.Add(row);
                    }
                }
            }
            return convertedTable;
        }

        public async Task<List<GeneratedExceptionApprovalModel>> GetDeliveredQuantityVarianceExceptions(int approvalCompanyId, int timeout = 30)
        {
            var inputmodel = new { ApprovalCompanyId = approvalCompanyId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetDeliveredQuantityVarianceExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<GeneratedExceptionApprovalModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<DeliveredQuantityVarianceExceptionModel>> GetBuyerApprovalExceptions(int supplierCompanyId, string exceptionTypes, int timeout = 30)
        {
            var inputmodel = new { SupplierCompanyId = supplierCompanyId, ExceptionTypeIds = exceptionTypes };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetBuyerApprovalExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<DeliveredQuantityVarianceExceptionModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<GeneratedExceptionApprovalModel>> GetSupplierApprovalExceptions(int buyerCompanyId, string exceptionTypes, int timeout = 30)
        {
            var inputmodel = new { BuyerCompanyId = buyerCompanyId, ExceptionTypeIds = exceptionTypes };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetSupplierApprovalExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<GeneratedExceptionApprovalModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<AutoApprovalExceptionModel>> GetAutoApprovalExceptions(string holidayList, bool isSaturdayOff, int timeout = 30)
        {
            var inputmodel = new { HolidayList = holidayList ?? string.Empty, IsSaturdayOff = isSaturdayOff };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetAutoApprovalExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<AutoApprovalExceptionModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }
        public async Task<List<GeneratedExceptionApprovalModel>> GetExceptionsForApproval(int approvalCompanyId, string exceptionTypes, int timeout = 30)
        {
            var inputmodel = new { ApprovalCompanyId = approvalCompanyId, ExceptionTypeIds = exceptionTypes };
            var input = SqlHelperMethods.GetStoredProcedure("usp_GetGeneratedExceptions", inputmodel);

            Context.DataContext.Database.CommandTimeout = timeout;
            var response = await Context.DataContext.Database.SqlQuery<GeneratedExceptionApprovalModel>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }

        public async Task<List<int>> GetExceptionIdsForAutoRejection(int exceptionTypeId, int statusId)
        {
            var inputmodel = new { ExceptionTypeId = exceptionTypeId, StatusId = statusId };
            var input = SqlHelperMethods.GetStoredProcedure("usp_getExceptionIdsforAutoRejection", inputmodel);
            Context.DataContext.Database.CommandTimeout = 30;
            var response = await Context.DataContext.Database.SqlQuery<int>(input.Query, input.Params.ToArray()).ToListAsync();
            return response;
        }
    }
}
