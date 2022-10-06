using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.Core.Logger;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain
{
    public class ReportDomain : BaseDomain
    {
        public ReportDomain() : base(ContextFactory.Current.ConnectionString)
        {
        }

        public ReportDomain(BaseDomain domain) : base(domain)
        {
        }

        public async Task<List<InvoiceReportExportViewModel>> GetInvoiceReportToExport(InvoiceReportFilter requestModel)
        {
            List<InvoiceReportExportViewModel> result = new List<InvoiceReportExportViewModel>();
            using (var tracer = new Tracer("ReportDomain", "GetInvoiceReportToExport"))
            {
                try
                {
                    var response = await ContextFactory.Current.GetDomain<StoredProcedureDomain>().GetInvoiceReportAsync(requestModel);
                    result = response.Select(t => new InvoiceReportExportViewModel
                    {
                        InvoiceNumber = t.InvoiceNumber?.Replace(",", ""),
                        InvoiceDate = t.InvoiceDate?.Replace(",", ""),
                        DeliveryAmount = t.DeliveryAmount.ToString()?.Replace(",", ""),
                        FuelAmount = t.FuelAmount.ToString()?.Replace(",", ""),
                        StateSalesTax = t.StateSalesTax.ToString()?.Replace(",", ""),
                        StateTax = t.StateTax.ToString()?.Replace(",", ""),
                        FederalTax = t.FederalTax.ToString()?.Replace(",", ""),
                        InvoiceAmount = t.InvoiceAmount.ToString()?.Replace(",", ""),
                        JobName = t.JobName?.Replace(",", ""),
                        Description = t.FuelType?.Replace(",", "")
                    }).ToList();
                }
                catch (Exception ex)
                {
                    LogManager.Logger.WriteException("ReportDomain", "GetInvoiceReportToExport", ex.Message, ex);
                }

                return result;
            }
        }

        public List<DropdownDisplayItem> GetJobsForSelectedSupplier(List<int> supplierCompanyIds, int buyerCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (supplierCompanyIds != null)
                {
                    response = Context.DataContext.Orders.Where(t => t.IsActive &&
                                            t.FuelRequest.Job.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                            t.BuyerCompanyId == buyerCompanyId && supplierCompanyIds.Contains(t.AcceptedCompanyId))
                                            .Select(t => new DropdownDisplayItem
                                            {
                                                Id = t.FuelRequest.Job.Id,
                                                Name = t.FuelRequest.Job.Name
                                            }).OrderByDescending(t => t.Id).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ReportOrderDomain", "GetJobsForSelectedSupplier", ex.Message, ex);
            }
            return response;
        }

        public List<DropdownDisplayItem> GetJobsForSelectedBuyer(List<int> buyerCompanyIds, int supplierCompanyId)
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                if (buyerCompanyIds != null)
                {
                    response = Context.DataContext.Orders.Where(t => t.IsActive &&
                                            t.FuelRequest.Job.JobXStatuses.FirstOrDefault(t1 => t1.IsActive).StatusId == (int)JobStatus.Open &&
                                            t.AcceptedCompanyId == supplierCompanyId && buyerCompanyIds.Contains(t.BuyerCompanyId))
                                            .Select(t => new DropdownDisplayItem
                                            {
                                                Id = t.FuelRequest.Job.Id,
                                                Name = t.FuelRequest.Job.Name
                                            }).OrderByDescending(t => t.Id).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("ReportOrderDomain", "GetJobsForSelectedBuyer", ex.Message, ex);
            }
            return response;
        }
    }
}
