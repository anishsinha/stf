using SiteFuel.Exchange.ViewModels.Quickbooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
	public static class QbCompanyProfileMapper
	{
		public static QbCompanyProfile ToQbProfileModel(this DataAccess.Entities.QbCompanyProfile qbCompanyProfile, bool loadTerms = false, bool loadqwcFile = false)
		{
			var profile = new QbCompanyProfile
			{
				CompanyId = qbCompanyProfile.CompanyId,
				CreatedBy = qbCompanyProfile.CreatedBy,
				LastAccessedOn = qbCompanyProfile.LastAccessedOn,
				LoginToken = qbCompanyProfile.LoginToken,
				Password = qbCompanyProfile.Password,
				ProfileCreatedOn = qbCompanyProfile.ProfileCreatedOn,
				ProfileUpdatedOn = qbCompanyProfile.ProfileUpdatedOn,
				QbVersion = qbCompanyProfile.QbVersion,
				QwcXml = loadqwcFile ?  qbCompanyProfile.QwcXml : null,
				Username = qbCompanyProfile.Username,
				ExpenseAccountName = qbCompanyProfile.ExpenseAccountName,
				IncomeAccountName = qbCompanyProfile.IncomeAccountName,
                DiscountAccountName = qbCompanyProfile.DiscountAccountName,
                ClassRef = qbCompanyProfile.ClassRef,
				ItemPrefix = qbCompanyProfile.ItemPrefix,
				IsActive = qbCompanyProfile.IsActive,
                SyncStartDate = qbCompanyProfile.SyncStartDate,
				PaymentTerms = loadTerms? qbCompanyProfile.QbPaymentTerms.Select(x => new PaymentTerms
				{
					CreatedDate = x.CreatedDate,
					Id = x.Id,
					IsActive = x.IsActive,
					TermDays = x.TermDays,
					TermName = x.TermName
				}).ToList() : new List<PaymentTerms>()
			};
			return profile;
		}
	}
}
