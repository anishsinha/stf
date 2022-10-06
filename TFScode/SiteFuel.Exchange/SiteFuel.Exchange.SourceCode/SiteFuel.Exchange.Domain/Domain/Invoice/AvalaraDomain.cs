using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.AvalaraAuthenticationWebService;
using SiteFuel.Exchange.Domain.AvaTaxExciseWebService;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Domain.WebServices;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace SiteFuel.Exchange.Domain
{
    public static class AvalaraDomain
    {
        private static System.Net.CookieContainer _authCookies = new System.Net.CookieContainer();
        private static readonly Dictionary<UoM, string> uoMMappings = new Dictionary<UoM, string>
        {
            { UoM.Gallons, ApplicationConstants.GallonAbbreviation },
            { UoM.Litres, ApplicationConstants.LitreAbbreviation }
        };

        #region [  Invoke Process Transactions 5_27_0  ]

        public static AvalaraResponseViewModel InvokeProcessTransactions_5_27_0(AvalaraTaxInputViewModel inputViewModel)
        {
            var response = new AvalaraResponseViewModel();
            AvalaraAuthenticationService auth = new AvalaraAuthenticationService();
            auth.CookieContainer = new System.Net.CookieContainer();
            TransactionResultSummary_5_27_0 processTransactions_5_27_0Result = new TransactionResultSummary_5_27_0();

            try
            {
                auth.Url = AvalaraConfigSettings.LoginUrl;
                if (auth.Login(AvalaraConfigSettings.UserId, AvalaraConfigSettings.Password, AvalaraConfigSettings.CompanyName))
                {
                    _authCookies = auth.CookieContainer;
                    AvaTaxExcise taxDetermination = new AvaTaxExcise();
                    taxDetermination.CookieContainer = _authCookies;

                    taxDetermination.Url = AvalaraConfigSettings.TaxUrl;
                    //for (idx = 0; idx < 10; idx++)
                    {
                        //Define transaction
                        Transaction_5_27_0[] transactions = new Transaction_5_27_0[1];
                        Transaction_5_27_0 transactions_0 = new Transaction_5_27_0();
                        transactions_0.Company = AvalaraConfigSettings.CompanyName;
                        transactions_0.EffectiveDate = inputViewModel.EffectiveDate.Date;
                        transactions_0.InvoiceNumber = inputViewModel.InvoiceNumber.CropToLastChars(12);
                        transactions_0.InvoiceDate = inputViewModel.InvoiceDate.Date;
                        transactions_0.TransactionType = "BELOW";
                        transactions_0.TransportationModeCode = "J";
                        transactions_0.TitleTransferCode = "DEST";
                        transactions_0.Buyer = inputViewModel.BuyerCustomId;
                        transactions_0.Seller = inputViewModel.SellerCustomId;
                        transactions_0.SourceSystem = "POC";
                        transactions_0.ReportingCurrency = inputViewModel.Currency.ToString();
                        transactions_0.TransactionExchangeRates = GetTransactionExchangeRates(inputViewModel.CurrencyRates);
                        transactions_0.CustomString1 = inputViewModel.IsDirectTaxCompany.ToString(); // FOR DIRECT TAX
                        transactions_0.CustomString2 = inputViewModel.Exclusions == TaxExclusionType.NORA ? inputViewModel.Exclusions.ToString() : null;
                        transactions_0.CustomString3 = inputViewModel.JobId > 0 ? inputViewModel.JobId.ToString() : null; //For Veribage tax message at invoice PDF
                        //Define Transaction Lines
                        List<TransactionLine_5_27_0> transactionLines = new List<TransactionLine_5_27_0>();
                        TransactionLine_5_27_0 transactionLines_0 = new TransactionLine_5_27_0();
                        transactionLines_0.CustomString1 = inputViewModel.SupplierAllowance.ToString(); //FOR SUPPLIER ALLOWANCE
                        transactionLines_0.CustomString2 = inputViewModel.BuyerCompanyId > 0 ? inputViewModel.BuyerCompanyId.ToString() : null; //FOR Veribage tax message at invoice PDF
                        transactionLines_0.CustomString3 = inputViewModel.SupplierCompanyId > 0 ? inputViewModel.SupplierCompanyId.ToString() : null; //FOR Veribage tax message at invoice PDF
                        transactionLines_0.InvoiceLine = 1;
                        transactionLines_0.ProductCode = inputViewModel.ProductCode;
                        transactionLines_0.UnitPrice = inputViewModel.UnitPrice;
                        transactionLines_0.Currency = inputViewModel.Currency.ToString();
                        transactionLines_0.NetUnits = inputViewModel.NetUnitsDropped;
                        transactionLines_0.GrossUnits = inputViewModel.GrossUnitsDropped;
                        transactionLines_0.BilledUnits = inputViewModel.BilledUnitsDropped;

                        transactionLines_0.UnitOfMeasure = uoMMappings[inputViewModel.UoM];
                        transactionLines_0.OriginCountryCode = inputViewModel.OriginCountryCode;
                        transactionLines_0.OriginJurisdiction = inputViewModel.OriginJurisdiction;
                        if (!string.IsNullOrWhiteSpace(inputViewModel.OriginCounty))
                        {
                            transactionLines_0.OriginCounty = inputViewModel.OriginCounty.UpdateCountyName();
                        }
                        var originAddressLines = inputViewModel.OriginAddress.SplitAddress();
                        transactionLines_0.OriginAddress1 = originAddressLines[0];
                        transactionLines_0.OriginAddress2 = originAddressLines[1];
                        transactionLines_0.OriginCity = inputViewModel.OriginCity;
                        transactionLines_0.OriginPostalCode = inputViewModel.OriginPostalCode;
                        var destinationAddressLines = inputViewModel.DestinationAddress.SplitAddress();
                        transactionLines_0.DestinationAddress1 = destinationAddressLines[0];
                        transactionLines_0.DestinationAddress2 = destinationAddressLines[1];
                        transactionLines_0.DestinationCountryCode = inputViewModel.DestinationCountryCode;
                        transactionLines_0.DestinationJurisdiction = inputViewModel.DestinationJurisdiction;
                        if (!string.IsNullOrWhiteSpace(inputViewModel.DestinationCounty))
                        {
                            transactionLines_0.DestinationCounty = inputViewModel.DestinationCounty.UpdateCountyName();
                        }
                        transactionLines_0.DestinationCity = inputViewModel.DestinationCity;
                        transactionLines_0.DestinationPostalCode = inputViewModel.DestinationPostalCode;

                        transactionLines.Add(transactionLines_0);

                        // Add fee transaction line items
                        if (inputViewModel.FeeTransactionLines != null && inputViewModel.FeeTransactionLines.Count > 0)
                        {
                            var feesTransactions = GetAvalaraTransactionLines(inputViewModel.FeeTransactionLines, inputViewModel.Currency).ToList();
                            feesTransactions.ForEach(t => t.InvoiceLine = (t.InvoiceLine + 1));
                            transactionLines.AddRange(feesTransactions);
                        }

                        //Set Main Objects
                        transactions_0.TransactionLines = transactionLines.ToArray();
                        transactions[0] = transactions_0;
                        response.Request = transactions_0;
                        var request = SerializeObject(transactions);
                        LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0 ", "Request Xml", new Exception(request));
                        processTransactions_5_27_0Result = taxDetermination.ProcessTransactions_5_27_0(transactions);
                    }
                }
                else
                {
                    processTransactions_5_27_0Result = null;
                    LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0", "Authentication Failed!!" + AvalaraConfigSettings.UserId + " " + AvalaraConfigSettings.Password, new Exception("Authentication Failed!!"));
                    throw new Exception("Authentication Failed!!" + AvalaraConfigSettings.UserId + " " + AvalaraConfigSettings.Password);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0", ex.Message, ex);
                processTransactions_5_27_0Result = null;
                throw;
            }

            //fall back call to handle Avalara cache problem
            if (processTransactions_5_27_0Result.NumberSuccess <= 0 && processTransactions_5_27_0Result.NumberFailed > 0)
            {
                bool isNeedToCallServiceAgain = false;
                foreach (var item in processTransactions_5_27_0Result.TransactionResults)
                {
                    foreach (var error in item.TransactionErrors)
                    {
                        if (error.ErrorCode.Equals("-998"))
                        {
                            isNeedToCallServiceAgain = false; // No need to retry immediately. Job will try again.
                            LogManager.Logger.WriteDebug("AvalaraDomain", "ProcessTransactions_5_27_0", error.ErrorMessage);
                        }
                        else
                        {
                            LogManager.Logger.WriteException("AvalaraDomain", "ProcessTransactions_5_27_0", error.ErrorMessage, new Exception());
                        }
                    }
                }

                if (isNeedToCallServiceAgain)
                    return InvokeProcessTransactions_5_27_0(inputViewModel);
            }
            response.Result = processTransactions_5_27_0Result;
            return response;
        }

        public static BusinessEntityImportResultSummary_5_27_0 ImportBusinessLicense(List<TaxExemptionViewModel> viewModel)
        {
            AvalaraAuthenticationService auth = new AvalaraAuthenticationService();
            auth.CookieContainer = new System.Net.CookieContainer();
            BusinessEntityImportResultSummary_5_27_0 importBusinessLicenseResult = new BusinessEntityImportResultSummary_5_27_0();
            var publishMode = viewModel[0].IsUpdateRequest ? "UPDATE" : "INSERT";

            try
            {
                auth.Url = AvalaraConfigSettings.LoginUrl;
                if (auth.Login(AvalaraConfigSettings.UserId, AvalaraConfigSettings.Password, AvalaraConfigSettings.CompanyName))
                {
                    AvalaraImportService ava = new AvalaraImportService(AvalaraConfigSettings.TaxExemptionUrl);
                    BusinessEntity_5_27_0[] entities = new BusinessEntity_5_27_0[1];
                    BusinessEntity_5_27_0 businessEntity = new BusinessEntity_5_27_0();

                    List<BusinessAccount_5_27_0> accounts = new List<BusinessAccount_5_27_0>();
                    foreach (var license in viewModel)
                    {
                        BusinessAccount_5_27_0 businessAccount = new BusinessAccount_5_27_0();
                        businessAccount.EffectiveDate = DateTime.ParseExact(license.EffectiveDate.Date.ToString("yyyy/MM/dd"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        businessAccount.LicenseNumber = license.LicenseNumber;
                        if (license.ObsoleteDate.HasValue)
                        {
                            businessAccount.ObsoleteDate = DateTime.ParseExact(license.ObsoleteDate.Value.Date.ToString("yyyy/MM/dd"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                        }
                        businessAccount.BusinessType = license.BusinessType == (int)CompanyType.Buyer ? BusinessType.Buyer : BusinessType.Seller;
                        businessAccount.BusinessSubType = license.BusinessSubTypeVal;
                        businessAccount.CustomId = license.AccountCustomId;
                        businessAccount.Jurisdiction = license.Jurisdiction;
                        businessAccount.CountryCode = "USA";
                        businessAccount.BusinessAccountMappingId = Guid.NewGuid();
                        accounts.Add(businessAccount);
                    }

                    businessEntity.TradeName = viewModel[0].TradeName;
                    businessEntity.Address1 = viewModel[0].Address;
                    businessEntity.City = viewModel[0].City;
                    businessEntity.CountryCode = "USA";
                    businessEntity.Jurisdiction = viewModel[0].State.Code;
                    businessEntity.IdType = string.IsNullOrWhiteSpace(viewModel[0].IDType) ? BusinessEntityIdType.FEIN : (BusinessEntityIdType)Enum.Parse(typeof(BusinessEntityIdType), viewModel[0].IDType);
                    businessEntity.IdCode = viewModel[0].IDCode;
                    businessEntity.County = viewModel[0].County;
                    businessEntity.CustomId = viewModel[0].EntityCustomId;
                    businessEntity.EffectiveDate = DateTime.ParseExact(viewModel[0].CompanyEffectiveDate.Date.ToString("yyyy/MM/dd"), "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    businessEntity.LegalName = viewModel[0].LegalName;
                    businessEntity.BusinessAccounts = accounts.ToArray();
                    businessEntity.CustomIdPriority = viewModel[0].IsUpdateRequest ? 1 : 0;
                    businessEntity.IdCodeAndIdTypePriority = 0;
                    businessEntity.LegalNamePriority = 0;
                    businessEntity.PostalCode = viewModel[0].ZipCode;
                    entities[0] = businessEntity;
                    businessEntity.BusinessEntityMappingId = Guid.NewGuid();

                    _authCookies = auth.CookieContainer;
                    ava.CookieContainer = _authCookies;
                    importBusinessLicenseResult = ava.ImportBusinessEntities_5_27_0(entities, AvalaraConfigSettings.CompanyName, publishMode, false, "SFAPP");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AvalaraDomain", "ImportBusinessLicense", ex.Message, ex);
                importBusinessLicenseResult = null;
            }
            return importBusinessLicenseResult;
        }
        #endregion

        public static string SerializeObject<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }

        private static TransactionExchangeRate_5_27_0[] GetTransactionExchangeRates(List<CurrencyRateViewModel> currencyRates)
        {
            var response = currencyRates.Select(t => t.ToAvalaraViewModel());
            return response.ToArray();
        }

        public static AvalaraResponseViewModel InvokeProcessTransactions_5_27_0_For_FTL(AvalaraTaxInputViewModel inputViewModel)
        {
            var response = new AvalaraResponseViewModel();
            AvalaraAuthenticationService auth = new AvalaraAuthenticationService();
            auth.CookieContainer = new System.Net.CookieContainer();
            TransactionResultSummary_5_27_0 processTransactions_5_27_0Result = new TransactionResultSummary_5_27_0();

            try
            {
                auth.Url = AvalaraConfigSettings.LoginUrl;
                if (auth.Login(AvalaraConfigSettings.UserId, AvalaraConfigSettings.Password, AvalaraConfigSettings.CompanyName))
                {
                    _authCookies = auth.CookieContainer;
                    AvaTaxExcise taxDetermination = new AvaTaxExcise();
                    taxDetermination.CookieContainer = _authCookies;
                    taxDetermination.Url = AvalaraConfigSettings.TaxUrl;

                    //Define transaction
                    Transaction_5_27_0[] transactions = new Transaction_5_27_0[1];
                    Transaction_5_27_0 transactions_0 = new Transaction_5_27_0();
                    transactions_0.Company = AvalaraConfigSettings.CompanyName;
                    transactions_0.EffectiveDate = inputViewModel.EffectiveDate.Date;
                    transactions_0.InvoiceNumber = inputViewModel.InvoiceNumber.CropToLastChars(12);
                    transactions_0.InvoiceDate = inputViewModel.InvoiceDate.Date;
                    transactions_0.TransactionType = "BELOW";
                    transactions_0.TransportationModeCode = "J";
                    transactions_0.TitleTransferCode = "DEST";
                    transactions_0.Buyer = inputViewModel.BuyerCustomId;
                    transactions_0.Seller = inputViewModel.SellerCustomId;
                    transactions_0.SourceSystem = "POC";
                    transactions_0.ReportingCurrency = inputViewModel.Currency.ToString();
                    transactions_0.TransactionExchangeRates = GetTransactionExchangeRates(inputViewModel.CurrencyRates);
                    transactions_0.CustomString1 = inputViewModel.IsDirectTaxCompany.ToString(); // FOR DIRECT TAX
                    transactions_0.CustomString2 = inputViewModel.Exclusions == TaxExclusionType.NORA ? inputViewModel.Exclusions.ToString() : null;
                    transactions_0.CustomString3 = inputViewModel.JobId > 0 ? inputViewModel.JobId.ToString() : null; //For Veribage tax message at invoice PDF
                                                                                                                      //transactions_0.SaveTransactionInd = "Y";
                    if (inputViewModel.IsFobOrigin)
                    {
                        transactions_0.TitleTransferCode = "ORIG";
                        transactions_0.TransactionType = "RACK";
                    }

                    //Define Transaction Lines
                    List<TransactionLine_5_27_0> transactionLines = new List<TransactionLine_5_27_0>();
                    TransactionLine_5_27_0 transactionLines_0 = new TransactionLine_5_27_0();
                    transactionLines_0.CustomString1 = inputViewModel.SupplierAllowance.ToString(); //FOR SUPPLIER ALLOWANCE
                    transactionLines_0.CustomString2 = inputViewModel.BuyerCompanyId > 0 ? inputViewModel.BuyerCompanyId.ToString() : null; //FOR Veribage tax message at invoice PDF
                    transactionLines_0.CustomString3 = inputViewModel.SupplierCompanyId > 0 ? inputViewModel.SupplierCompanyId.ToString() : null; //FOR Veribage tax message at invoice PDF
                    transactionLines_0.InvoiceLine = 1;
                    transactionLines_0.ProductCode = inputViewModel.ProductCode;
                    transactionLines_0.UnitPrice = inputViewModel.UnitPrice;
                    transactionLines_0.Currency = inputViewModel.Currency.ToString();
                    transactionLines_0.NetUnits = inputViewModel.NetUnitsDropped;
                    transactionLines_0.GrossUnits = inputViewModel.GrossUnitsDropped;
                    transactionLines_0.BilledUnits = inputViewModel.BilledUnitsDropped;
                    transactionLines_0.UnitOfMeasure = uoMMappings[inputViewModel.UoM];
                    //transactionLines_0.BlendToProductCode = "B10";
                    transactionLines_0.OriginCountryCode = inputViewModel.OriginCountryCode;
                    transactionLines_0.OriginJurisdiction = inputViewModel.OriginJurisdiction;
                    if (!string.IsNullOrWhiteSpace(inputViewModel.OriginCounty))
                    {
                        transactionLines_0.OriginCounty = inputViewModel.OriginCounty.UpdateCountyName();
                    }
                    var originAddressLines = inputViewModel.OriginAddress.SplitAddress();
                    transactionLines_0.OriginAddress1 = originAddressLines[0];
                    transactionLines_0.OriginAddress2 = originAddressLines[1];
                    transactionLines_0.OriginCity = inputViewModel.OriginCity;
                    transactionLines_0.OriginPostalCode = inputViewModel.OriginPostalCode;
                    //transactionLines_0.OriginType = inputViewModel.OriginType;
                    //transactionLines_0.OriginOutCityLimitInd = inputViewModel.OriginOutCityLimitInd;
                    //transactionLines_0.OriginSpecialJurisdictionInd = inputViewModel.OriginSpecialJurisdictionInd;
                    transactionLines_0.DestinationCountryCode = inputViewModel.DestinationCountryCode;
                    transactionLines_0.DestinationJurisdiction = inputViewModel.DestinationJurisdiction;
                    if (!string.IsNullOrWhiteSpace(inputViewModel.DestinationCounty))
                    {
                        transactionLines_0.DestinationCounty = inputViewModel.DestinationCounty.UpdateCountyName();
                    }
                    var destinationAddressLines = inputViewModel.DestinationAddress.SplitAddress();
                    transactionLines_0.DestinationAddress1 = destinationAddressLines[0];
                    transactionLines_0.DestinationAddress2 = destinationAddressLines[1];
                    transactionLines_0.DestinationCity = inputViewModel.DestinationCity;
                    //transactionLines_0.DestinationOutCityLimitInd = inputViewModel.DestinationOutCityLimitInd;
                    //transactionLines_0.DestinationSpecialJurisdictionInd = inputViewModel.DestinationSpecialJurisdictionInd;
                    transactionLines_0.DestinationPostalCode = inputViewModel.DestinationPostalCode;

                    //transactionLines_0.SaleSpecialJurisdictionInd = inputViewModel.SaleSpecialJurisdictionInd;

                    transactionLines.Add(transactionLines_0);

                    if (inputViewModel.FeeTransactionLines != null && inputViewModel.FeeTransactionLines.Count > 0)
                    {
                        var feesTransactions = GetAvalaraTransactionLines(inputViewModel.FeeTransactionLines, inputViewModel.Currency).ToList();
                        feesTransactions.ForEach(t => t.InvoiceLine = (t.InvoiceLine + 1));
                        transactionLines.AddRange(feesTransactions);
                    }

                    //Set Main Objects
                    transactions_0.TransactionLines = transactionLines.ToArray();
                    transactions[0] = transactions_0;
                    response.Request = transactions_0;
                    var request = SerializeObject(transactions);
                    LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_For_FTL ", "Request Xml", new Exception(request));
                    processTransactions_5_27_0Result = taxDetermination.ProcessTransactions_5_27_0(transactions);

                }
                else
                {
                    processTransactions_5_27_0Result = null;
                    LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_For_FTL", "Authentication Failed!!" + AvalaraConfigSettings.UserId + " " + AvalaraConfigSettings.Password, new Exception("Authentication Failed!!"));
                    throw new Exception("Authentication Failed!!" + AvalaraConfigSettings.UserId + " " + AvalaraConfigSettings.Password);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_For_FTL", ex.Message, ex);
                processTransactions_5_27_0Result = null;
                throw;
            }

            //fall back call to handle Avalara cache problem
            if (processTransactions_5_27_0Result.NumberSuccess <= 0 && processTransactions_5_27_0Result.NumberFailed > 0)
            {
                bool isNeedToCallServiceAgain = false;
                foreach (var item in processTransactions_5_27_0Result.TransactionResults)
                {
                    foreach (var error in item.TransactionErrors)
                    {
                        if (error.ErrorCode.Equals("-998"))
                        {
                            //isNeedToCallServiceAgain = true;
                            LogManager.Logger.WriteDebug("AvalaraDomain", "InvokeProcessTransactions_5_27_0_For_FTL", error.ErrorMessage);
                        }
                        else
                        {
                            LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_For_FTL", error.ErrorMessage, new Exception());
                        }
                    }
                }

                if (isNeedToCallServiceAgain)
                    return InvokeProcessTransactions_5_27_0_For_FTL(inputViewModel);
            }
            response.Result = processTransactions_5_27_0Result;

            return response;
        }

        public static AvalaraResponseViewModel InvokeProcessTransactions_5_27_0_New(AvalaraTaxMultipleInputViewModel inputViewModel)
        {
            var response = new AvalaraResponseViewModel();
            AvalaraAuthenticationService auth = new AvalaraAuthenticationService();
            auth.CookieContainer = new System.Net.CookieContainer();
            TransactionResultSummary_5_27_0 processTransactions_5_27_0Result = new TransactionResultSummary_5_27_0();

            try
            {
                auth.Url = AvalaraConfigSettings.LoginUrl;
                if (auth.Login(AvalaraConfigSettings.UserId, AvalaraConfigSettings.Password, AvalaraConfigSettings.CompanyName))
                {
                    _authCookies = auth.CookieContainer;
                    AvaTaxExcise taxDetermination = new AvaTaxExcise();
                    taxDetermination.CookieContainer = _authCookies;
                    taxDetermination.Url = AvalaraConfigSettings.TaxUrl;

                    //Define transaction
                    Transaction_5_27_0[] transactions = GetAvalaraTransactions(inputViewModel);
                    var request = SerializeObject(transactions);
                    response.Request = transactions.FirstOrDefault();
                    LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_New ", "Request Xml", new Exception(request));
                    processTransactions_5_27_0Result = taxDetermination.ProcessTransactions_5_27_0(transactions);
                }
                else
                {
                    processTransactions_5_27_0Result = null;
                    LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_New", "Authentication Failed!!" + AvalaraConfigSettings.UserId + " " + AvalaraConfigSettings.Password, new Exception("Authentication Failed!!"));
                    throw new Exception("Authentication Failed!!" + AvalaraConfigSettings.UserId + " " + AvalaraConfigSettings.Password);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_New", ex.Message, ex);
                processTransactions_5_27_0Result = null;
                throw;
            }

            //fall back call to handle Avalara cache problem
            if (processTransactions_5_27_0Result.NumberSuccess <= 0 && processTransactions_5_27_0Result.NumberFailed > 0)
            {
                foreach (var item in processTransactions_5_27_0Result.TransactionResults)
                {
                    foreach (var error in item.TransactionErrors)
                    {
                        LogManager.Logger.WriteException("AvalaraDomain", "InvokeProcessTransactions_5_27_0_New", error.ErrorMessage, new Exception());
                    }
                }
            }
            //var request2 = SerializeObject(processTransactions_5_27_0Result);
            response.Result = processTransactions_5_27_0Result;
            return response;
        }

        private static Transaction_5_27_0[] GetAvalaraTransactions(AvalaraTaxMultipleInputViewModel inputViewModel)
        {
            Transaction_5_27_0[] transactions = new Transaction_5_27_0[1];
            Transaction_5_27_0 transactions_0 = new Transaction_5_27_0();
            transactions_0.Company = AvalaraConfigSettings.CompanyName;
            transactions_0.EffectiveDate = inputViewModel.EffectiveDate.Date;
            transactions_0.InvoiceNumber = inputViewModel.InvoiceNumber.CropToLastChars(12);
            transactions_0.InvoiceDate = inputViewModel.InvoiceDate.Date;
            transactions_0.TransactionType = "BELOW";
            transactions_0.TransportationModeCode = "J";
            transactions_0.TitleTransferCode = "DEST";
            transactions_0.Buyer = inputViewModel.BuyerCustomId;
            transactions_0.Seller = inputViewModel.SellerCustomId;
            transactions_0.SourceSystem = "POC";
            transactions_0.ReportingCurrency = inputViewModel.Currency.ToString();
            transactions_0.TransactionExchangeRates = GetTransactionExchangeRates(inputViewModel.CurrencyRates);
            transactions_0.CustomString1 = inputViewModel.IsDirectTaxCompany.ToString(); // FOR DIRECT TAX
            transactions_0.CustomString2 = inputViewModel.Exclusions == TaxExclusionType.NORA ? inputViewModel.Exclusions.ToString() : null;
            transactions_0.CustomString3 = inputViewModel.JobId > 0 ? inputViewModel.JobId.ToString() : null; //For Veribage tax message at invoice PDF
                                                                                                              //transactions_0.SaveTransactionInd = "Y";
            if (inputViewModel.IsFobOrigin)
            {
                transactions_0.TitleTransferCode = "ORIG";
                transactions_0.TransactionType = "RACK";
            }

            transactions_0.TransactionLines = GetAvalaraTransactionLines(inputViewModel.InputTransactionLines, inputViewModel.Currency);

            transactions[0] = transactions_0;
            return transactions;
        }

        private static TransactionLine_5_27_0[] GetAvalaraTransactionLines(List<AvalaraInputTransactionsViewModel> inputTransactionLines, Currency currency)
        {
            var linesLenth = inputTransactionLines.Count;
            TransactionLine_5_27_0[] transactionLines = new TransactionLine_5_27_0[linesLenth];

            for (var i = 0; i < linesLenth; i++)
            {
                var inputViewModel = inputTransactionLines[i];
                TransactionLine_5_27_0 transactionLines_0 = new TransactionLine_5_27_0();
                transactionLines_0.CustomString1 = inputViewModel.SupplierAllowance.ToString(); //FOR SUPPLIER ALLOWANCE
                transactionLines_0.CustomString2 = inputViewModel.BuyerCompanyId > 0 ? inputViewModel.BuyerCompanyId.ToString() : null; //FOR Veribage tax message at invoice PDF
                transactionLines_0.CustomString3 = inputViewModel.SupplierCompanyId > 0 ? inputViewModel.SupplierCompanyId.ToString() : null; //FOR Veribage tax message at invoice PDF
                transactionLines_0.InvoiceLine = i + 1;
                transactionLines_0.ProductCode = inputViewModel.ProductCode;
                transactionLines_0.UnitPrice = inputViewModel.UnitPrice;
                transactionLines_0.Currency = currency.ToString();
                transactionLines_0.NetUnits = inputViewModel.NetUnitsDropped;
                transactionLines_0.GrossUnits = inputViewModel.GrossUnitsDropped;
                transactionLines_0.BilledUnits = inputViewModel.BilledUnitsDropped;
                transactionLines_0.UnitOfMeasure = uoMMappings[inputViewModel.UoM];
                //transactionLines_0.BlendToProductCode = "B10";
                transactionLines_0.OriginCountryCode = inputViewModel.OriginCountryCode;
                transactionLines_0.OriginJurisdiction = inputViewModel.OriginJurisdiction;
                if (!string.IsNullOrWhiteSpace(inputViewModel.OriginCounty))
                {
                    transactionLines_0.OriginCounty = inputViewModel.OriginCounty.UpdateCountyName();
                }
                var originAddressLines = inputViewModel.OriginAddress.SplitAddress();
                transactionLines_0.OriginAddress1 = originAddressLines[0];
                transactionLines_0.OriginAddress2 = originAddressLines[1];
                transactionLines_0.OriginCity = inputViewModel.OriginCity;
                transactionLines_0.OriginPostalCode = inputViewModel.OriginPostalCode;
                //transactionLines_0.OriginType = inputViewModel.OriginType;
                //transactionLines_0.OriginOutCityLimitInd = inputViewModel.OriginOutCityLimitInd;
                //transactionLines_0.OriginSpecialJurisdictionInd = inputViewModel.OriginSpecialJurisdictionInd;
                transactionLines_0.DestinationCountryCode = inputViewModel.DestinationCountryCode;
                transactionLines_0.DestinationJurisdiction = inputViewModel.DestinationJurisdiction;
                if (!string.IsNullOrWhiteSpace(inputViewModel.DestinationCounty))
                {
                    transactionLines_0.DestinationCounty = inputViewModel.DestinationCounty.UpdateCountyName();
                }
                var destinationAddressLines = inputViewModel.DestinationAddress.SplitAddress();
                transactionLines_0.DestinationAddress1 = destinationAddressLines[0];
                transactionLines_0.DestinationAddress2 = destinationAddressLines[1];
                transactionLines_0.DestinationCity = inputViewModel.DestinationCity;
                //transactionLines_0.DestinationOutCityLimitInd = inputViewModel.DestinationOutCityLimitInd;
                //transactionLines_0.DestinationSpecialJurisdictionInd = inputViewModel.DestinationSpecialJurisdictionInd;
                transactionLines_0.DestinationPostalCode = inputViewModel.DestinationPostalCode;
                transactionLines_0.BillOfLadingNumber = inputViewModel.BillOfLaddingNumber;
                transactionLines_0.BillOfLadingDate = string.IsNullOrEmpty(inputViewModel.BillOfLaddingNumber) ? (DateTime?)null : inputViewModel.BillOfLaddingDate;
                //transactionLines_0.SaleSpecialJurisdictionInd = inputViewModel.SaleSpecialJurisdictionInd;

                transactionLines[i] = transactionLines_0;
            }
            return transactionLines;
        }

        public static string UpdateCountyName(this string countyName)
        {
            if (!string.IsNullOrEmpty(countyName))
            {
                var countyToExclude = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingAvalaraCountyName);
                var countyArray = countyToExclude.Trim().Split(',');
                foreach (var county in countyArray)
                {
                    countyName = Regex.Replace(countyName, county.Trim(), string.Empty, RegexOptions.IgnoreCase);
                }
            }
            countyName = !string.IsNullOrWhiteSpace(countyName)?countyName.Substring(0, Math.Min(30, countyName.Length)):string.Empty;
            return countyName.Trim();
        }
    }
}