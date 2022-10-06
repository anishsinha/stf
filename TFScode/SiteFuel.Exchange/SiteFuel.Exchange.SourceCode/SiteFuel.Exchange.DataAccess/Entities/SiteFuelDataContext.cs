namespace SiteFuel.Exchange.DataAccess.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    public partial class SiteFuelDataContext : DbContext
    {
        public SiteFuelDataContext() : base("name=DatabaseConnection")
        {
        }
        public virtual DbSet<QbInvoiceMapping> QbInvoiceMappings { get; set; }
        public virtual DbSet<CarrierCustomerMapping> CarrierCustomerMappings { get; set; }
        public virtual DbSet<QbEntityMapping> QbEntityMappings { get; set; }
        public virtual DbSet<QbResponse> QbResponses { get; set; }
        public virtual DbSet<QbCompanyProfile> QbProfiles { get; set; }
        public virtual DbSet<QbRequest> QbRequests { get; set; }
        public virtual DbSet<AccountingWorkflow> QbWorkflows { get; set; }
        public virtual DbSet<AppLocation> AppLocations { get; set; }
        public virtual DbSet<AppMessage> AppMessages { get; set; }
        public virtual DbSet<AppMessageXUserStatus> AppMessageXUserStatuses { get; set; }
        public virtual DbSet<AssetAdditionalDetail> AssetAdditionalDetails { get; set; }
        public virtual DbSet<AssetDropRequest> AssetDropRequests { get; set; }
        public virtual DbSet<AssetDrop> AssetDrops { get; set; }
        public virtual DbSet<AssetDuplicate> AssetDuplicates { get; set; }
        public virtual DbSet<MstApplicationTemplate> MstApplicationTemplates { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyAddress> CompanyAddresses { get; set; }
        public virtual DbSet<CompanyBlacklist> CompanyBlacklists { get; set; }
        public virtual DbSet<CompanyFavoriteFuel> CompanyFavoriteFuels { get; set; }
        public virtual DbSet<CompanyXAdditionalUserInvite> CompanyXAdditionalUserInvites { get; set; }
        public virtual DbSet<CompanyXMobileAppTheme> CompanyXMobileAppThemes { get; set; }
        public virtual DbSet<CompanyXStripeCard> CompanyXStripeCards { get; set; }
        public virtual DbSet<CounterOffer> CounterOffers { get; set; }
        public virtual DbSet<CreditAppDetail> CreditAppDetails { get; set; }
        public virtual DbSet<CreditAppDocument> CreditAppDocuments { get; set; }
        public virtual DbSet<DebugLog> DebugLogs { get; set; }
        public virtual DbSet<DeliverySchedule> DeliverySchedules { get; set; }
        public virtual DbSet<DeliveryScheduleXDriver> DeliveryScheduleXDrivers { get; set; }
        public virtual DbSet<DeliveryScheduleXTrackableSchedule> DeliveryScheduleXTrackableSchedules { get; set; }
        public virtual DbSet<DifferentFuelPrice> DifferentFuelPrices { get; set; }
        public virtual DbSet<EnrouteDeliveryHistory> EnrouteDeliveryHistories { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public virtual DbSet<NotificationLog> NotificationLogs { get; set; }
        public virtual DbSet<FeeByQuantity> FeeByQuantities { get; set; }
        public virtual DbSet<FuelFee> FuelRequestFees { get; set; }
        public virtual DbSet<FuelRequest> FuelRequests { get; set; }
        public virtual DbSet<FuelRequestDetail> FuelRequestDetails { get; set; }
        public virtual DbSet<FuelRequestXStatus> FuelRequestXStatuses { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImpersonationActivityLog> ImpersonationActivityLogs { get; set; }
        public virtual DbSet<ImpersonationHistory> ImpersonationHistories { get; set; }
        public virtual DbSet<InvoiceNumber> InvoiceNumbers { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceXAdditionalDetail> InvoiceXAdditionalDetails { get; set; }
        public virtual DbSet<InvoiceXDeclineReason> InvoiceXDeclineReasons { get; set; }
        public virtual DbSet<InvoiceXInvoiceStatusDetail> InvoiceXInvoiceStatusDetails { get; set; }
        public virtual DbSet<InvoiceXSpecialInstruction> InvoiceXSpecialInstructions { get; set; }
        public virtual DbSet<InvoiceXAccessorialFee> InvoiceXAccessorialFees { get; set; }
        public virtual DbSet<InvoiceFtlDetail> InvoiceFtlDetails { get; set; }
        public virtual DbSet<PreLoadBolDetail> PreLoadBolDetails { get; set; }
        public virtual DbSet<JobBudget> JobBudgets { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<CarrierEmailSetting> CarrierEmailSettings { get; set; }
        public virtual DbSet<JobCarrierDetail> JobCarrierDetails { get; set; }
        public virtual DbSet<JobXApprovalUser> JobXApprovalUsers { get; set; }
        public virtual DbSet<JobXAsset> JobXAssets { get; set; }
        public virtual DbSet<JobXStatus> JobXStatuses { get; set; }
        public virtual DbSet<MstAdvertisementTierPricing> MstAdvertisementTierPricings { get; set; }
        public virtual DbSet<MstAlertType> MstAlertTypes { get; set; }
        public virtual DbSet<MstAppMessageStatus> MstAppMessageStatuses { get; set; }
        public virtual DbSet<MstAppMessageUserType> MstAppMessageUserTypes { get; set; }
        public virtual DbSet<MstAppSetting> MstAppSettings { get; set; }
        public virtual DbSet<MstAppType> MstAppTypes { get; set; }
        public virtual DbSet<MstBudgetAllocationType> MstBudgetAllocationTypes { get; set; }
        public virtual DbSet<MstBudgetCalculationType> MstBudgetCalculationTypes { get; set; }
        public virtual DbSet<MstBudgetType> MstBudgetTypes { get; set; }
        public virtual DbSet<MstBusinessSubType> MstBusinessSubTypes { get; set; }
        public virtual DbSet<MstBusinessTenure> MstBusinessTenures { get; set; }
        public virtual DbSet<MstLookup> MstLookup { get; set; }
        public virtual DbSet<MstLookupType> MstLookupType { get; set; }
        public virtual DbSet<MstCity> MstCities { get; set; }
        public virtual DbSet<MstCompanySize> MstCompanySizes { get; set; }
        public virtual DbSet<MstCompanyType> MstCompanyTypes { get; set; }
        public virtual DbSet<MstCompanyIdentifier> MstCompanyIdentifiers { get; set; }
        public virtual DbSet<MstCompanyTypeXRole> MstCompanyTypeXRoles { get; set; }
        public virtual DbSet<MstCompanyUserRoleXEventType> MstCompanyUserRoleXEventTypes { get; set; }
        public virtual DbSet<MstCounterOfferStatus> MstCounterOfferStatuses { get; set; }
        public virtual DbSet<MstCountry> MstCountries { get; set; }
        public virtual DbSet<MstDeliveryFeeType> MstDeliveryFeeTypes { get; set; }
        public virtual DbSet<MstDeliveryScheduleStatus> MstDeliveryScheduleStatuses { get; set; }
        public virtual DbSet<MstDeliveryScheduleType> MstDeliveryScheduleTypes { get; set; }
        public virtual DbSet<MstDeliveryType> MstDeliveryTypes { get; set; }
        public virtual DbSet<MstEnrouteDeliveryStatus> MstEnrouteDeliveryStatuses { get; set; }
        public virtual DbSet<MstEntityType> MstEntityTypes { get; set; }
        public virtual DbSet<MstEventType> MstEventTypes { get; set; }
        public virtual DbSet<MstExchangeModule> MstExchangeModules { get; set; }
        public virtual DbSet<MstExternalProduct> MstExternalProducts { get; set; }
        public virtual DbSet<MstExternalTerminal> MstExternalTerminals { get; set; }
        public virtual DbSet<MstFeeSubType> MstFeeSubTypes { get; set; }
        public virtual DbSet<MstFeeType> MstFeeTypes { get; set; }
        public virtual DbSet<MstFeeXFeeSubType> MstFeeXFeeSubTypes { get; set; }
        public virtual DbSet<MstFuelQuantity> MstFuelQuantities { get; set; }
        public virtual DbSet<MstFuelRequestStatus> MstFuelRequestStatuses { get; set; }
        public virtual DbSet<MstFuelRequestType> MstFuelRequestTypes { get; set; }
        public virtual DbSet<MstInvoiceDeclineReason> MstInvoiceDeclineReasons { get; set; }
        public virtual DbSet<MstInvoiceStatus> MstInvoiceStatuses { get; set; }
        public virtual DbSet<MstInvoiceType> MstInvoiceTypes { get; set; }
        public virtual DbSet<MstInvoiceVersionStatus> MstInvoiceVersionStatuses { get; set; }
        public virtual DbSet<MstJobStatus> MstJobStatuses { get; set; }
        public virtual DbSet<MstMarginType> MstMarginTypes { get; set; }
        public virtual DbSet<MstMathOperator> MstMathOperators { get; set; }
        public virtual DbSet<MstMobileAppThemeDetail> MstMobileAppThemeDetails { get; set; }
        public virtual DbSet<MstMobileAppTheme> MstMobileAppThemes { get; set; }
        public virtual DbSet<MstNewsfeedEvent> MstNewsfeedEvents { get; set; }
        public virtual DbSet<MstNewsfeedMessage> MstNewsfeedMessages { get; set; }
        public virtual DbSet<MstOnboardedType> MstOnboardedTypes { get; set; }
        public virtual DbSet<MstOnboardingQuestion> MstOnboardingQuestions { get; set; }
        public virtual DbSet<MstOnboardingQuestionType> MstOnboardingQuestionTypes { get; set; }
        public virtual DbSet<MstOrderCancelationReason> MstOrderCancelationReasons { get; set; }
        public virtual DbSet<MstOrderStatus> MstOrderStatuses { get; set; }
        public virtual DbSet<MstOrderType> MstOrderTypes { get; set; }
        public virtual DbSet<MstPaymentTerm> MstPaymentTerms { get; set; }
        public virtual DbSet<MstPhoneType> MstPhoneTypes { get; set; }
        public virtual DbSet<MstPricingType> MstPricingTypes { get; set; }
        public virtual DbSet<MstProcessType> MstProcessTypes { get; set; }
        public virtual DbSet<MstProductDisplayGroup> MstProductDisplayGroups { get; set; }
        public virtual DbSet<MstProductMapping> MstProductMappings { get; set; }
        public virtual DbSet<MstProduct> MstProducts { get; set; }
        public virtual DbSet<MstTfxProduct> MstTfxProducts { get; set; }
        public virtual DbSet<MstProductType> MstProductTypes { get; set; }
        public virtual DbSet<MstQuantityType> MstQuantityTypes { get; set; }
        public virtual DbSet<MstRackAvgPricingType> MstRackAvgPricingTypes { get; set; }
        public virtual DbSet<MstRole> MstRoles { get; set; }
        public virtual DbSet<MstRoleXInvoiceDeclineReason> MstRoleXInvoiceDeclineReasons { get; set; }
        public virtual DbSet<MstServiceArea> MstServiceAreas { get; set; }
        public virtual DbSet<MstState> MstStates { get; set; }
        public virtual DbSet<MstSupplierQualification> MstSupplierQualifications { get; set; }
        public virtual DbSet<MstTaxExemptLicenseStatus> MstTaxExemptLicenseStatuses { get; set; }
        public virtual DbSet<MstTaxPricingType> MstTaxPricingTypes { get; set; }
        public virtual DbSet<MstTimeCardAction> MstTimeCardActions { get; set; }
        public virtual DbSet<MstWeekDay> MstWeekDays { get; set; }
        public virtual DbSet<MstFeeConstraintType> MstFeeConstraintTypes { get; set; }
        public virtual DbSet<MstFeedType> MstFeedTypes { get; set; }
        public virtual DbSet<MstPricingSource> MstPricingSources { get; set; }
        public virtual DbSet<Newsfeed> Newsfeeds { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<OnboardingQuestionAnswer> OnboardingQuestionAnswers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderAdditionalDetail> OrderAdditionalDetails { get; set; }
        public virtual DbSet<OrderBadgeDetail> OrderBadgeDetails { get; set; }
        public virtual DbSet<FuelRequestPricingDetail> FuelRequestPricingDetails { get; set; }
        public virtual DbSet<OrderDetailVersion> OrderDetailVersions { get; set; }
        public virtual DbSet<OrderTaxDetail> OrderTaxDetails { get; set; }
        public virtual DbSet<OrderVersionXDeliverySchedule> OrderVersionXDeliverySchedules { get; set; }
        public virtual DbSet<OrderXCancelationReason> OrderXCancelationReasons { get; set; }
        public virtual DbSet<OrderXDriver> OrderXDrivers { get; set; }
        public virtual DbSet<OrderXStatus> OrderXStatuses { get; set; }
        public virtual DbSet<OrderXTogglePricingDetail> OrderXTogglePricingDetails { get; set; }
        public virtual DbSet<PaymentDiscount> PaymentDiscounts { get; set; }
        public virtual DbSet<PrivateSupplierList> PrivateSupplierLists { get; set; }
        public virtual DbSet<ProductTypeCompatibilityMapping> ProductTypeCompatibilityMappings { get; set; }
        public virtual DbSet<MstBlendProductTypeMapping> MstBlendProductTypeMapping { get; set; }
        public virtual DbSet<QueueMessage> QueueMessages { get; set; }
        public virtual DbSet<QueueResult> QueueResults { get; set; }
        public virtual DbSet<WebNotification> WebNotifications { get; set; }
        public virtual DbSet<MstNotificationType> MstNotificationTypes { get; set; }
        public virtual DbSet<RequestFuel> RequestFuels { get; set; }
        public virtual DbSet<RequestPrice> RequestPrices { get; set; }
        public virtual DbSet<ResaleCustomer> ResaleCustomers { get; set; }
        public virtual DbSet<ResaleFee> ResaleFees { get; set; }
        public virtual DbSet<Resale> Resales { get; set; }
        public virtual DbSet<SpecialInstruction> SpecialInstructions { get; set; }
        public virtual DbSet<Spill> Spills { get; set; }
        public virtual DbSet<SAPAutoIdentifier> SAPAutoIdentifiers { get; set; }
        public virtual DbSet<SAPOrderStatus> SAPOrderStatus { get; set; }
        public virtual DbSet<Subcontractor> Subcontractors { get; set; }
        public virtual DbSet<SupplierAddressXSetting> SupplierAddressXSettings { get; set; }
        public virtual DbSet<SupplierAddressXWorkingHour> SupplierAddressXWorkingHours { get; set; }
        public virtual DbSet<SourceRegion> SourceRegions { get; set; }
        public virtual DbSet<SourceRegionCarrier> SourceRegionCarriers { get; set; }
        public virtual DbSet<SourceRegionAddress> SourceRegionAddresses { get; set; }
        public virtual DbSet<SourceRegionPickupLocation> SourceRegionPickupLocations { get; set; }
        public virtual DbSet<TaxDetail> TaxDetails { get; set; }
        public virtual DbSet<TaxExemptLicens> TaxExemptLicenses { get; set; }
        public virtual DbSet<TimeCardEntry> TimeCardEntries { get; set; }
        public virtual DbSet<UserCode> UserCodes { get; set; }
        public virtual DbSet<UserFilterSetting> UserFilterSettings { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<UserXInvite> UserXInvites { get; set; }
        public virtual DbSet<UserXNotificationSetting> UserXNotificationSettings { get; set; }
        public virtual DbSet<MstSupplierCostTypes> MstSupplierCostTypes { get; set; }
        public virtual DbSet<CurrentCost> CurrentCosts { get; set; }
        public virtual DbSet<FuelRequestCurrentCost> FuelRequestCurrentCosts { get; set; }
        public virtual DbSet<BillingAddress> BillingAddresses { get; set; }
        public virtual DbSet<MstExternalMeterService> MstExternalMeterServices { get; set; }
        public virtual DbSet<ExternalSupplier> ExternalSuppliers { get; set; }
        public virtual DbSet<ExternalSupplierAddress> ExternalSupplierAddresses { get; set; }
        public virtual DbSet<ExternalSupplierAddressTruckType> ExternalSupplierAddressTruckTypes { get; set; }
        public virtual DbSet<ExternalSupplierStatus> ExternalSupplierStatuses { get; set; }
        public virtual DbSet<ExternalSupplierNote> ExternalSupplierNotes { get; set; }
        public virtual DbSet<MstAccountType> MstAccountTypes { get; set; }
        public virtual DbSet<AssetSubcontractor> AssetSubcontractors { get; set; }
        public virtual DbSet<ExternalDropDetail> ExternalDropDetails { get; set; }
        public virtual DbSet<MstQuoteRequestStatus> MstQuoteRequestStatuses { get; set; }
        public virtual DbSet<Quotation> Quotations { get; set; }
        public virtual DbSet<QuoteRequest> QuoteRequests { get; set; }
        public virtual DbSet<QuoteRequestStatus> QuoteRequestStatuses { get; set; }
        public virtual DbSet<ExternalBroker> ExternalBrokers { get; set; }
        public virtual DbSet<ExternalBrokerOrderDetail> ExternalBrokerOrderDetails { get; set; }
        public virtual DbSet<ExternalBrokerBuySellDetail> ExternalBrokerBuySellDetails { get; set; }
        public virtual DbSet<AssetContractNumber> AssetContractNumbers { get; set; }
        public virtual DbSet<QuoteRequestDocument> QuoteRequestDocuments { get; set; }
        public virtual DbSet<ConvertedSupplier> ConvertedSuppliers { get; set; }
        public virtual DbSet<MstExternalSupplierType> MstExternalSupplierTypes { get; set; }
        public virtual DbSet<MstThirdPartyNozzle> MstThirdPartyNozzles { get; set; }
        public virtual DbSet<StateCurrentCost> StateCurrentCosts { get; set; }
        public virtual DbSet<QbPaymentTerm> QbPaymentTerms { get; set; }
        public virtual DbSet<MstOtherFeeType> MstOtherFeeTypes { get; set; }
        public virtual DbSet<MstFeeCategory> MstFeeCategories { get; set; }
        public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<MstEventGroup> MstEventGroups { get; set; }
        public virtual DbSet<MstNotificationTemplate> MstNotificationTemplates { get; set; }
        public virtual DbSet<NotificationTemplateMapping> NotificationTemplateMappings { get; set; }
        public virtual DbSet<Signature> Signatures { get; set; }
        public virtual DbSet<MstTierType> MstTierTypes { get; set; }
        public virtual DbSet<MstOfferType> MstOfferTypes { get; set; }
        public virtual DbSet<MstOfferStatus> MstOfferStatuses { get; set; }
        public virtual DbSet<MstBuyerOfferStatus> MstBuyerOfferStatuses { get; set; }
        public virtual DbSet<OfferTierMapping> OfferTierMappings { get; set; }
        public virtual DbSet<OfferBuyerStatus> OfferBuyerStatuses { get; set; }
        public virtual DbSet<OfferPricing> OfferPricings { get; set; }
        public virtual DbSet<OfferPricingExclusion> OfferPricingExclusions { get; set; }
        public virtual DbSet<OfferPricingItem> OfferPricingDetails { get; set; }
        public virtual DbSet<OfferUpdateCommand> OfferUpdateHistories { get; set; }
        public virtual DbSet<OfferLocationExclusion> OfferLocationExclusions { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<DiscountLineItem> DiscountLineItems { get; set; }
        public virtual DbSet<CompanyGroup> CompanyGroups { get; set; }
        public virtual DbSet<CompanyGroupXCompany> CompanyGroupXCompanies { get; set; }
        public virtual DbSet<CompanyExternalIdentification> CompanyExternalIdentifications { get; set; }
        public virtual DbSet<MstFeature> MstFeatures { get; set; }
        public virtual DbSet<CompanyFeature> CompanyFeatures { get; set; }
        public virtual DbSet<OfferQuickUpdatePreference> OfferQuickUpdatePreferences { get; set; }
        public virtual DbSet<QbLog> QbLogs { get; set; }
        public virtual DbSet<UserPageSetting> UserPageSettings { get; set; }
        public virtual DbSet<MstFrequencyType> MstFrequencyTypes { get; set; }
        public virtual DbSet<BillingSchedule> BillingSchedules { get; set; }
        public virtual DbSet<BillingScheduleXCustomerOrder> BillingScheduleXCustomerOrders { get; set; }
        public virtual DbSet<BillingStatement> BillingStatements { get; set; }
        public virtual DbSet<BillingStatementXInvoice> BillingStatementXInvoices { get; set; }
        public virtual DbSet<StatementNumber> StatementNumbers { get; set; }
        public virtual DbSet<FuelDispatchLocation> FuelDispatchLocations { get; set; }
        public virtual DbSet<Carrier> Carriers { get; set; }
        public virtual DbSet<InvoiceDispatchLocation> InvoiceDispatchLocations { get; set; }
        public virtual DbSet<DirectTax> DirectTaxes { get; set; }
        public virtual DbSet<SupplierSource> SupplierSources { get; set; }
        public virtual DbSet<FuelRequestTankRentalFrequency> FuelRequestTankRentalFrequencies { get; set; }
        public virtual DbSet<TankDetail> TankDetails { get; set; }
        public virtual DbSet<CompanySetting> CompanySettings { get; set; }
        public virtual DbSet<FuelSurchargeTable> FuelSurchargeTables { get; set; }
        public virtual DbSet<FuelSurchargeIndex> FuelSurchargeIndexes { get; set; }
        public virtual DbSet<FuelSurchargeGeneratedTable> FuelSurchargeGeneratedTables { get; set; }
        public virtual DbSet<AccessorialFee> AccessorialFees { get; set; }
        public virtual DbSet<FuelGroup> FuelGroups { get; set; }
        public virtual DbSet<FuelGroupProductType> FuelGroupProductTypes { get; set; }
        public virtual DbSet<FuelGroupFuelType> FuelGroupFuelTypes { get; set; }
        public virtual DbSet<EIAPriceUpdate> EIAPriceUpdates { get; set; }
        public virtual DbSet<EIAAreaMapping> EIAAreaMappings { get; set; }
        public virtual DbSet<InvoicePayment> InvoicePayments { get; set; }
        public virtual DbSet<InvoiceException> InvoiceExceptions { get; set; }
        public virtual DbSet<TaxExclusion> TaxExclusions { get; set; }
        public virtual DbSet<UserGridConfiguration> UserGridConfigurations { get; set; }
        public virtual DbSet<DeliveryGroup> DeliveryGroups { get; set; }
        public virtual DbSet<InvoiceHeaderDetail> InvoiceHeaderDetails { get; set; }
        public virtual DbSet<InvoiceXBolDetail> InvoiceXBolDetails { get; set; }
        public virtual DbSet<ForcastingServiceXCarrier> ForcastingServiceXCarriers { get; set; }
        public virtual DbSet<OrderGroup> OrderGroups { get; set; }
        public virtual DbSet<OrderGroupXOrder> OrderGroupXOrders { get; set; }
        public virtual DbSet<TermOrderGroupHistory> TermOrderGroupDropDetails { get; set; }
        public virtual DbSet<OnboardingPreference> OnboardingPreferences { get; set; }
        public virtual DbSet<BuyerXOnboardingPreference> BuyerXOnboardingPreferences { get; set; }
        public virtual DbSet<SupplierXBuyerDetails> SupplierXBuyerDetails { get; set; }
        public virtual DbSet<SupplierXBuyerSetting> SupplierXBuyerSettings { get; set; }
        public virtual DbSet<FavoriteSideMenu> FavoriteSideMenu { get; set; }
        public virtual DbSet<MstOPISProduct> MstOPISProducts { get; set; }
        public virtual DbSet<BulkPlantLocation> BulkPlantLocations { get; set; }
        public virtual DbSet<SupplierMappedProductDetails> SupplierMappedProductDetails { get; set; }
        public virtual DbSet<SupplierMappedProductXFuelType> SupplierMappedProductXFuelTypes { get; set; }
        public virtual DbSet<SupplierInvitationDetails> SupplierInvitationDetails { get; set; }
        public virtual DbSet<APIDebugLog> APIDebugLogs { get; set; }
        public virtual DbSet<ApiLog> ApiLogs { get; set; }
        public virtual DbSet<TerminalCompanyAlias> TerminalCompanyAliases { get;set;}
        public virtual DbSet<CarrierMapping> CarrierMappings { get; set; }
        public virtual DbSet<ScheduleXBrokerOrderDetail> ScheduleXBrokerOrderDetails { get; set; }
        public virtual DbSet<LiftFileValidationParameter> LiftFileValidationParameters { get; set; }
        public virtual DbSet<LiftFileValidationRecord> LiftFileValidationRecords { get; set; }
        public virtual DbSet<LiftFileDetail> LiftFileDetails { get; set; }
        public virtual DbSet<LiftFilePONumbers> LiftFilePONumbers { get; set; }
        public virtual DbSet<MasterFuelTypeForTFX> MasterFuelTypeForTFXs { get; set; }
        public virtual DbSet<MasterJdeXrefToVendorFile> MasterJdeXrefToVendorFiles { get; set; }
        public virtual DbSet<SupplierXProductSequencing> SupplierXProductSequencing { get; set; }
        public virtual DbSet<LiftFileWholesaleBadges> LiftFileWholesaleBadges { get; set; }
        public virtual DbSet<MstTerminalSupplier> MstTerminalSuppliers { get; set; }
        public virtual DbSet<MstTerminalItemDescription> MstTerminalItemDescriptions { get; set; }
        public virtual DbSet<TerminalItemCodeMapping> TerminalItemCodeMappings { get; set; }
        public virtual DbSet<EBolDetails> EBolDetails { get; set; }
        public virtual DbSet<EBolDetailsStaging> EBolDetailsStaging { get; set; }
        public virtual DbSet<LiftFileBadgeManagementDetail> LiftFileBadgeManagementDetails { get; set; }
        public virtual DbSet<InvoiceTierPricingDetail> InvoiceTierPricingDetails { get; set; }
        public virtual DbSet<ForcastingServiceSetting> ForcastingServiceSettings { get; set; }
        public virtual DbSet<ForcastingPreference> ForcastingPreferences { get; set; }
        public virtual DbSet<MstExternalThirdPartyCompanies> MstExternalThirdPartyCompanies { get; set; }
        public virtual DbSet<ExternalCustomerMappings> ExternalCustomerMappings { get; set; }
        public virtual DbSet<ExternalCustomerLocationMappings> ExternalCustomerLocationMappings { get; set; }
        public virtual DbSet<ExternalSupplierMappings> ExternalSupplierMappings { get; set; }
        public virtual DbSet<ExternalProductMappings> ExternalProductMappings { get; set; }
        public virtual DbSet<ExternalTerminalMappings> ExternalTerminalMappings { get; set; }
        public virtual DbSet<ExternalCompaniesRoleAccessMapping> ExternalCompaniesRoleAccessMapping { get; set; }
        public virtual DbSet<ExternalCarrierMappings> ExternalCarrierMappings { get; set; }
        public virtual DbSet<ExternalDriverMappings> ExternalDriverMappings { get; set; }
        public virtual DbSet<ExternalBulkPlantMappings> ExternalBulkPlantMappings { get; set; }
        public virtual DbSet<Acknowledgement> Acknowledgements { get; set; }
        public virtual DbSet<LiftFileCarrierNames> LiftFileCarrierNames { get; set; }
        public virtual DbSet<MstGravityConversion> MstGravityConversions { get; set; }
        public virtual DbSet<PushNotificationLog> PushNotificationLogs { get; set; }
        public virtual DbSet<LiftFileQuebecBillingBadges> LiftFileQuebecBillingBadges { get; set; }
        public virtual DbSet<ExternalProductTypeMappings> ExternalProductTypeMappings { get; set; }
        public virtual DbSet<ExternalTruckMappings> ExternalTruckMappings { get; set; }
        public virtual DbSet<ExternalRegionMappings> ExternalRegionMappings { get; set; }
        public virtual DbSet<ExternalScheduleMappings> ExternalScheduleMappings { get; set; }
        public virtual DbSet<ExternalAssetDropDetails> ExternalAssetDropDetails { get; set; }
        public virtual DbSet<DsbLoadQueueDetails> DsbLoadQueueDetails { get; set; }
        public virtual DbSet<LiftFileHoldRecords> LiftFileHoldRecords { get; set; }
        public virtual DbSet<LeadRequest> LeadRequests { get; set; }
        public virtual DbSet<LeadCustomerInformation> LeadCustomerInformations { get; set; }
        public virtual DbSet<LeadAddressDetail> LeadAddressDetails { get; set; }
        public virtual DbSet<LeadFuelFee> LeadFuelFees { get; set; }
        public virtual DbSet<LeadFuelDetail> LeadFuelDetails { get; set; }
        public virtual DbSet<LeadFuelDeliveryDetail> LeadFuelDeliveryDetails { get; set; }
        public virtual DbSet<LeadAdditionalDetail> LeadAdditionalDetails { get; set; }
        public virtual DbSet<LeadRequestXOrder> LeadRequestXOrders { get; set; }
        public virtual DbSet<LeadPricingDetail> LeadPricingDetails { get; set; }
        public virtual DbSet<FreightRateRule> FreightRateRules { get; set; }
        public virtual DbSet<FreightRateRouteTable> FreightRateRouteTables { get; set; }
        public virtual DbSet<FreightRateRangeTable> FreightRateRangeTables { get; set; }
        public virtual DbSet<FreightRatePtoPTable> FreightRatePtoPTables { get; set; }
        public virtual DbSet<FreightRatePtoPLocation> FreightRatePtoPLocations { get; set; }
        public virtual DbSet<FreightRatePtoPFuelGroup> FreightRatePtoPFuelGroups { get; set; }
        public virtual DbSet<FreightRateFuelGroup> FreightRateFuelGroups { get; set; }
        public virtual DbSet<FreightTableCompany> FreightTableCompanies { get; set; }
        public virtual DbSet<FreightTablePickupLocation> FreightTablePickupLocations { get; set; }
        public virtual DbSet<FreightTableSourceRegion> FreightTableSourceRegions { get; set; }
        public virtual DbSet<BDRDetail> BDRDetails { get; set; }
        public virtual DbSet<LeadNote> LeadNotes { get; set; }
        public virtual DbSet<MstCountryAsGroup> MstCountryAsGroups { get; set; }
        public virtual DbSet<CarrierDeliveryXUserSetting> CarrierDeliveryXUserSettings { get; set; }
        public virtual DbSet<CarrierXDeliveryFailure> CarrierXDeliveryFailures { get; set; }
        public virtual DbSet<ThirdPartyCompanyInvites> ThirdPartyCompanyInvites { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<FleetInformation> FleetInformations { get; set; }
        public virtual DbSet<CompanyXServingLocation> CompanyXServingLocations { get; set; }
        public virtual DbSet<InvitedUser> InvitedUsers{ get; set; }
        public virtual DbSet<CompanyToken> CompanyTokens { get; set; }
        public virtual DbSet<LeadRequestPriceDetails> LeadRequestPriceDetails { get; set; }
        public virtual DbSet<LeadCumulationDetail> LeadCumulationDetails { get; set; }
        public virtual DbSet<CompanyXCreators> CompanyXCreators { get; set; }
        public virtual DbSet<ReasonCategory> ReasonCategories { get; set; }
        public virtual DbSet<ReasonCodeDetail> ReasonCodes { get; set; }
        public virtual DbSet<ExternalIdentityProvider> ExternalIdentityProviders { get; set; }
        public virtual DbSet<IdentityProvider> IdentityProviders { get; set; }
        public virtual DbSet<CompanyIdentityService> CompanyIdentityServices { get; set; }
        public virtual DbSet<BulkOrderDetails> BulkOrderDetails { get; set; }
        public virtual DbSet<EBOLProductMappings> EBOLProductMappings { get; set; }
        public virtual DbSet<LoadOptimizationUser> LoadOptimizationUsers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FreightRatePtoPFuelGroup>()
               .Property(e => e.RateValue)
               .HasPrecision(18, 8);

            modelBuilder.Entity<FreightRateRangeTable>()
                .Property(e => e.RateValue)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FreightRateRouteTable>()
                .Property(e => e.RateValue)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AppLocation>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AppLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AppMessage>()
                .HasMany(e => e.AppMessageXUserStatuses)
                .WithRequired(e => e.AppMessage)
                .HasForeignKey(e => e.MessageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AppMessage>()
                .HasMany(e => e.Users)
                .WithMany(e => e.AppMessages)
                .Map(m => m.ToTable("AppMessageXDraftRecipients").MapLeftKey("MessageId").MapRightKey("RecipientId"));

            modelBuilder.Entity<AppMessage>()
                .HasMany(e => e.AppMessages1)
                .WithMany(e => e.AppMessages)
                .Map(m => m.ToTable("AppMessageXReplies").MapLeftKey("MessageId").MapRightKey("ReplyMessageId"));

            modelBuilder.Entity<AssetAdditionalDetail>()
                .Property(e => e.FuelCapacity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDropRequest>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDropRequest>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDropRequest>()
                .Property(e => e.QuantityRequired)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.MeterStartReading)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.MeterEndReading)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.DroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.PostDip)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.PreDip)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDuplicate>()
                .Property(e => e.FuelCapacity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Asset>()
                .HasOptional(e => e.AssetAdditionalDetail)
                .WithRequired(e => e.Asset)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.AssetDropRequests)
                .WithRequired(e => e.Asset)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Spills)
                .WithRequired(e => e.Asset)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.AssetDuplicates)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyBlacklists)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.AddedByCompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyBlacklists1)
                .WithRequired(e => e.Company1)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyFavoriteFuels)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyXAdditionalUserInvites)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyXMobileAppThemes)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CompanyXStripeCards)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CreditAppDetails)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CreditAppDocuments)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.AcceptedCompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Orders1)
                .WithRequired(e => e.BuyerCompany)
                .HasForeignKey(e => e.BuyerCompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.PrivateSupplierLists)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Companies1)
                .WithMany(e => e.Companies)
                .Map(m => m.ToTable("CompanyXInvites").MapLeftKey("BuyerCompanyId").MapRightKey("SupplierCompanyId"));

            //modelBuilder.Entity<Company>()
            //    .HasMany(e => e.MstProducts)
            //    .WithMany(e => e.Companies)
            //    .Map(m => m.ToTable("CompanyXNonStandardProducts").MapLeftKey("CompanyId").MapRightKey("ProductId"));

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Notifications)
                .WithMany(e => e.Companies)
                .Map(m => m.ToTable("NotificationXCompanies").MapLeftKey("CompanyId").MapRightKey("NotificationId"));

            modelBuilder.Entity<Company>()
                .HasMany(e => e.PrivateSupplierLists1)
                .WithMany(e => e.Companies)
                .Map(m => m.ToTable("PrivateSupplierListXSupplierCompanies").MapLeftKey("SupplierCompanyId").MapRightKey("PrivateSupplierListId"));

            modelBuilder.Entity<CompanyAddress>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<CompanyAddress>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<CompanyAddress>()
                .HasOptional(e => e.SupplierAddressXSetting)
                .WithRequired(e => e.CompanyAddress)
                .WillCascadeOnDelete();

            modelBuilder.Entity<CompanyAddress>()
                .HasMany(e => e.SupplierAddressXWorkingHours)
                .WithRequired(e => e.CompanyAddress)
                .HasForeignKey(e => e.AddressId);

            modelBuilder.Entity<CompanyAddress>()
                .HasMany(e => e.Users)
                .WithMany(e => e.CompanyAddresses)
                .Map(m => m.ToTable("CompanyAddressXUsers").MapLeftKey("CompanyAddressId").MapRightKey("UserId"));

            modelBuilder.Entity<CompanyAddress>()
                .HasMany(e => e.MstProductTypes)
                .WithMany(e => e.CompanyAddresses)
                .Map(m => m.ToTable("SupplierAddressXProductTypes").MapLeftKey("AddressId").MapRightKey("ProductTypeId"));

            modelBuilder.Entity<CompanyAddress>()
                .HasMany(e => e.MstSupplierQualifications)
                .WithMany(e => e.CompanyAddresses)
                .Map(m => m.ToTable("SupplierAddressXQualifications").MapLeftKey("AddressId").MapRightKey("QualificationId"));

            modelBuilder.Entity<CompanyAddress>()
                .HasMany(e => e.MstStates)
                .WithMany(e => e.CompanyAddresses1)
                .Map(m => m.ToTable("SupplierAddressXServingStates").MapLeftKey("AddressId").MapRightKey("StateId"));

            modelBuilder.Entity<CompanyXAdditionalUserInvite>()
                .HasMany(e => e.Jobs)
                .WithMany(e => e.CompanyXAdditionalUserInvites)
                .Map(m => m.ToTable("AdditionalUserXJobs").MapLeftKey("AdditionalUserId").MapRightKey("JobId"));

            modelBuilder.Entity<CompanyXAdditionalUserInvite>()
                .HasMany(e => e.MstRoles)
                .WithMany(e => e.CompanyXAdditionalUserInvites)
                .Map(m => m.ToTable("CompanyXAdditionalUserInviteRoles").MapLeftKey("CompanyXAdditionalUserInviteId").MapRightKey("RoleId"));

            modelBuilder.Entity<DebugLog>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<DeliverySchedule>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<DeliverySchedule>()
                .Property(e => e.EndTime)
                .HasPrecision(0);

            modelBuilder.Entity<DeliverySchedule>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<DeliverySchedule>()
                .HasMany(e => e.DeliveryScheduleXDrivers)
                .WithRequired(e => e.DeliverySchedule)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliverySchedule>()
                .HasMany(e => e.DeliveryScheduleXTrackableSchedules)
                .WithRequired(e => e.DeliverySchedule)
                .HasForeignKey(e => e.DeliveryScheduleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliverySchedule>()
                .HasMany(e => e.OrderVersionXDeliverySchedules)
                .WithRequired(e => e.DeliverySchedule)
                .HasForeignKey(e => e.DeliveryRequestId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DeliverySchedule>()
                .HasMany(e => e.FuelRequests)
                .WithMany(e => e.DeliverySchedules)
                .Map(m => m.ToTable("FuelRequestXDeliverySchedules").MapLeftKey("DeliveryScheduleId").MapRightKey("FuelRequestId"));

            modelBuilder.Entity<DeliveryScheduleXTrackableSchedule>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<DeliveryScheduleXTrackableSchedule>()
                .Property(e => e.EndTime)
                .HasPrecision(0);

            modelBuilder.Entity<DeliveryScheduleXTrackableSchedule>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<DeliveryScheduleXTrackableSchedule>()
                .HasMany(e => e.DeliverySchedules)
                .WithOptional(e => e.DeliveryScheduleXTrackableSchedule)
                .HasForeignKey(e => e.RescheduledTrackableId);

            modelBuilder.Entity<DifferentFuelPrice>()
                .Property(e => e.MinQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<DifferentFuelPrice>()
                .Property(e => e.MaxQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<DifferentFuelPrice>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<DifferentFuelPrice>()
                .Property(e => e.Margin)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExceptionLog>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<FeeByQuantity>()
                .Property(e => e.MinQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FeeByQuantity>()
                .Property(e => e.MaxQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FeeByQuantity>()
                .Property(e => e.Fee)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FeeByQuantity>()
                .Property(e => e.Margin)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderTaxDetail>()
                .Property(e => e.TaxRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelFee>()
                .Property(e => e.Fee)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelFee>()
                .Property(e => e.Margin)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelFee>()
                .Property(e => e.MinimumGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelFee>()
                .Property(e => e.FeeSubQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelFee>()
                .Property(e => e.TotalFee)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelFee>()
                .HasMany(e => e.ResaleFees)
                .WithRequired(e => e.FuelRequestFee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FuelFee>()
                .HasMany(e => e.FuelRequests)
                .WithMany(e => e.FuelRequestFees)
                .Map(m => m.ToTable("FuelRequestXFees").MapLeftKey("FuelRequestFeeId").MapRightKey("FuelRequestId"));

            modelBuilder.Entity<FuelFee>()
                .HasMany(e => e.Invoices)
                .WithMany(e => e.FuelRequestFees)
                .Map(m => m.ToTable("InvoiceXFees").MapLeftKey("FuelRequestFeeId").MapRightKey("InvoiceId"));

            modelBuilder.Entity<FuelFee>()
                .HasMany(e => e.AccessorialFees)
                .WithMany(e => e.FuelFees)
                .Map(m => m.ToTable("AccessorialFeeXFees").MapLeftKey("FuelFeeId").MapRightKey("AccessorialFeeId"));

            modelBuilder.Entity<FuelFee>()
               .HasMany(e => e.LeadRequests)
               .WithMany(e => e.FuelFees)
               .Map(m => m.ToTable("LeadXFees").MapLeftKey("FuelFeeId").MapRightKey("LeadRequestId"));

            modelBuilder.Entity<BillingStatement>()
                .HasMany(e => e.BillingStatementXInvoices)
                .WithRequired(e => e.BillingStatement)
                .HasForeignKey(e => e.StatementId);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.BillingStatementXInvoices)
                .WithRequired(e => e.Invoice)
                .HasForeignKey(e => e.InvoiceId);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.MinQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.MaxQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.OverageAllowedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.HedgeDroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.HedgeDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.SpotDroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.SpotDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.CreationTimeRackPPG)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.AssetDropRequests)
                .WithRequired(e => e.FuelRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.CounterOffers)
                .WithRequired(e => e.FuelRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.FuelRequests1)
                .WithOptional(e => e.FuelRequest1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<FuelRequest>()
                .HasOptional(e => e.FuelRequestDetail)
                .WithRequired(e => e.FuelRequest);

            modelBuilder.Entity<FuelRequest>()
                .HasOptional(e => e.FuelRequestPricingDetail)
                .WithRequired(e => e.FuelRequest);

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.FuelRequestXStatuses)
                .WithRequired(e => e.FuelRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.FuelRequest)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.PrivateSupplierLists)
                .WithMany(e => e.FuelRequests)
                .Map(m => m.ToTable("FuelRequestXPrivateSupplierLists").MapLeftKey("FuelRequestId").MapRightKey("PrivateSupplierListId"));

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.ResaleCustomers)
                .WithMany(e => e.FuelRequests)
                .Map(m => m.ToTable("FuelRequestXResaleCustomers").MapLeftKey("FuelRequestId").MapRightKey("ResaleCustomerId"));

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.Resales)
                .WithMany(e => e.FuelRequests)
                .Map(m => m.ToTable("FuelRequestXResales").MapLeftKey("FuelRequestId").MapRightKey("ResaleId"));

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.SpecialInstructions)
                .WithMany(e => e.FuelRequests)
                .Map(m => m.ToTable("FuelRequestXSpecialInstructions").MapLeftKey("FuelRequestId").MapRightKey("SpecialInstructionId"));

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.MstSupplierQualifications)
                .WithMany(e => e.FuelRequests)
                .Map(m => m.ToTable("FuelRequestXSupplierQualifications").MapLeftKey("FuelRequestId").MapRightKey("SupplierQualificationId"));

            modelBuilder.Entity<FuelRequest>()
                .HasMany(e => e.Users)
                .WithMany(e => e.FuelRequests1)
                .Map(m => m.ToTable("SupplierXDeclinedFuelRequests").MapLeftKey("FuelRequestId").MapRightKey("SupplierId"));

            modelBuilder.Entity<FuelRequestDetail>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<FuelRequestDetail>()
                .Property(e => e.EndTime)
                .HasPrecision(0);

            modelBuilder.Entity<Image>()
                .HasMany(e => e.Companies)
                .WithOptional(e => e.Image)
                .HasForeignKey(e => e.CompanyLogoId);

            modelBuilder.Entity<InvoiceNumber>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<InvoiceNumber>()
                .HasMany(e => e.InvoiceHeaderDetails)
                .WithRequired(e => e.InvoiceNumber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceHeaderDetail>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.InvoiceHeader)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StatementNumber>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<StatementNumber>()
                .HasMany(e => e.BillingStatements)
                .WithRequired(e => e.StatementNumber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.DroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<PreLoadBolDetail>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.StateTax)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.FedTax)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.SalesTax)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BasicAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.TotalTaxAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.RackPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<PreLoadBolDetail>()
               .Property(e => e.RackPrice)
               .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.TotalFeeAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.Invoices1)
                .WithOptional(e => e.Invoice1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Invoice>()
                .HasOptional(e => e.InvoiceXAdditionalDetail)
                .WithRequired(e => e.Invoice);

            modelBuilder.Entity<Invoice>()
               .HasMany(e => e.InvoiceDispatchLocation)
               .WithRequired(e => e.Invoice);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceXDeclineReasons)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceXInvoiceStatusDetails)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.InvoiceXSpecialInstructions)
                .WithRequired(e => e.Invoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invoice>()
                .HasMany(e => e.TaxDetails)
                .WithMany(e => e.Invoices)
                .Map(m => m.ToTable("InvoiceXTaxDetails").MapLeftKey("InvoiceId").MapRightKey("TaxDetailsId"));

            modelBuilder.Entity<InvoiceXAdditionalDetail>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceXAdditionalDetail>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceXAdditionalDetail>()
                .Property(e => e.SurchargeEIAPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceXAdditionalDetail>()
                .Property(e => e.SurchargePercentage)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.Budget)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.Gallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.HedgeAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.SpotAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .HasMany(e => e.MstBudgetAllocationTypes)
                .WithMany(e => e.JobBudgets)
                .Map(m => m.ToTable("JobBudgetXBudgetAllocationTypes").MapLeftKey("JobBudgetId").MapRightKey("BudgetAllocationTypeId"));

            modelBuilder.Entity<Job>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.HedgeDroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.HedgeDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.SpotDroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.SpotDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobXApprovalUsers)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.JobXStatuses)
                .WithRequired(e => e.Job)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Jobs2)
                .Map(m => m.ToTable("UserXJobs").MapLeftKey("JobId").MapRightKey("UserId"));

            modelBuilder.Entity<Job>()
                .HasMany(e => e.ResaleCustomers)
                .WithMany(e => e.Jobs)
                .Map(m => m.ToTable("JobXResaleCustomers").MapLeftKey("JobId").MapRightKey("ResaleCustomerId"));

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Subcontractors)
                .WithMany(e => e.Jobs)
                .Map(m => m.ToTable("JobXSubcontractors").MapLeftKey("JobId").MapRightKey("SubContractorId"));

            modelBuilder.Entity<Job>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithMany(e => e.Jobs)
                .Map(m => m.ToTable("JobXTaxExemptLicenses").MapLeftKey("JobId").MapRightKey("TaxExemptLicenseId"));

            modelBuilder.Entity<Job>()
                .HasMany(e => e.Users1)
                .WithMany(e => e.Jobs3)
                .Map(m => m.ToTable("JobXOnsiteContactPersons").MapLeftKey("JobId").MapRightKey("UserId"));

            modelBuilder.Entity<JobXAsset>()
                .HasMany(e => e.AssetDrops)
                .WithRequired(e => e.JobXAsset)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstAdvertisementTierPricing>()
                .Property(e => e.Amount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<MstAppMessageStatus>()
                .HasMany(e => e.AppMessageXUserStatuses)
                .WithRequired(e => e.MstAppMessageStatus)
                .HasForeignKey(e => e.AppMessageStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstAppMessageUserType>()
                .HasMany(e => e.AppMessageXUserStatuses)
                .WithRequired(e => e.MstAppMessageUserType)
                .HasForeignKey(e => e.UserTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstAppType>()
                .HasMany(e => e.AppLocations)
                .WithRequired(e => e.MstAppType)
                .HasForeignKey(e => e.AppTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstAppType>()
                .HasMany(e => e.CompanyXMobileAppThemes)
                .WithRequired(e => e.MstAppType)
                .HasForeignKey(e => e.AppTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProductType>()
                .HasMany(e => e.Assets)
                .WithOptional(e => e.MstProductType)
                .HasForeignKey(e => e.FuelType);

            modelBuilder.Entity<MstBudgetCalculationType>()
                .HasMany(e => e.JobBudgets)
                .WithRequired(e => e.MstBudgetCalculationType)
                .HasForeignKey(e => e.BudgetCalculationTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstBudgetType>()
                .HasMany(e => e.JobBudgets)
                .WithOptional(e => e.MstBudgetType)
                .HasForeignKey(e => e.BudgetTypeId);

            modelBuilder.Entity<MstBusinessSubType>()
                .Property(e => e.Jurisdiction)
                .IsUnicode(false);

            modelBuilder.Entity<MstBusinessSubType>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithRequired(e => e.MstBusinessSubType)
                .HasForeignKey(e => e.BusinessSubType)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MstBusinessTenure>()
            //    .HasMany(e => e.Companies)
            //    .WithRequired(e => e.MstBusinessTenure)
            //    .HasForeignKey(e => e.BusinessTenureId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MstCompanySize>()
            //    .HasMany(e => e.Companies)
            //    .WithRequired(e => e.MstCompanySize)
            //    .HasForeignKey(e => e.CompanySizeId)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCompanyType>()
                .HasMany(e => e.Companies)
                .WithRequired(e => e.MstCompanyType)
                .HasForeignKey(e => e.CompanyTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCompanyType>()
                .HasMany(e => e.MstCompanyTypeXRoles)
                .WithRequired(e => e.MstCompanyType)
                .HasForeignKey(e => e.CompanyTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCompanyType>()
                .HasMany(e => e.MstCompanyUserRoleXEventTypes)
                .WithRequired(e => e.MstCompanyType)
                .HasForeignKey(e => e.CompanyTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCompanyType>()
                .HasMany(e => e.MstOrderCancelationReasons)
                .WithRequired(e => e.MstCompanyType)
                .HasForeignKey(e => e.CompanyTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCounterOfferStatus>()
                .HasMany(e => e.CounterOffers)
                .WithOptional(e => e.MstCounterOfferStatus)
                .HasForeignKey(e => e.BuyerStatus);

            modelBuilder.Entity<MstCounterOfferStatus>()
                .HasMany(e => e.CounterOffers1)
                .WithOptional(e => e.MstCounterOfferStatus1)
                .HasForeignKey(e => e.SupplierStatus);

            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.CompanyAddresses)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.MstStates)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.MstCountryAsGroups)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountryAsGroup>()
                .HasMany(e => e.MstStates)
                .WithOptional(e => e.MstCountryAsGroup)
                .HasForeignKey(e => e.CountryGroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.MstCountryAsGroups)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountryAsGroup>()
                .HasMany(e => e.MstStates)
                .WithOptional(e => e.MstCountryAsGroup)
                .HasForeignKey(e => e.CountryGroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstDeliveryScheduleStatus>()
                .HasMany(e => e.DeliveryScheduleXTrackableSchedules)
                .WithRequired(e => e.MstDeliveryScheduleStatus)
                .HasForeignKey(e => e.DeliveryScheduleStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstDeliveryScheduleType>()
                .HasMany(e => e.DeliverySchedules)
                .WithRequired(e => e.MstDeliveryScheduleType)
                .HasForeignKey(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstDeliveryType>()
                .HasMany(e => e.FuelRequestDetails)
                .WithRequired(e => e.MstDeliveryType)
                .HasForeignKey(e => e.DeliveryTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstEnrouteDeliveryStatus>()
                .HasMany(e => e.EnrouteDeliveryHistories)
                .WithRequired(e => e.MstEnrouteDeliveryStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstEventType>()
                .HasMany(e => e.MstCompanyUserRoleXEventTypes)
                .WithRequired(e => e.MstEventType)
                .HasForeignKey(e => e.EventTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstEventType>()
                .HasMany(e => e.Notifications)
                .WithRequired(e => e.MstEventType)
                .HasForeignKey(e => e.EventTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstEventType>()
                .HasMany(e => e.UserXNotificationSettings)
                .WithRequired(e => e.MstEventType)
                .HasForeignKey(e => e.EventTypeId);

            modelBuilder.Entity<MstExternalProduct>()
                .HasMany(e => e.MstProductMappings)
                .WithRequired(e => e.MstExternalProduct)
                .HasForeignKey(e => e.ExternalProductId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalTerminal>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<MstExternalTerminal>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<MstExternalTerminal>()
                .HasMany(e => e.FuelRequests)
                .WithOptional(e => e.MstExternalTerminal)
                .HasForeignKey(e => e.TerminalId);

            modelBuilder.Entity<MstExternalTerminal>()
                .HasMany(e => e.InvoiceBolDetails)
                .WithOptional(e => e.MstExternalTerminal)
                .HasForeignKey(e => e.TerminalId);

            modelBuilder.Entity<MstExternalTerminal>()
               .HasMany(e => e.PreLoadBolDetails)
               .WithOptional(e => e.MstExternalTerminal)
               .HasForeignKey(e => e.TerminalId);

            modelBuilder.Entity<MstExternalTerminal>()
                .HasMany(e => e.Jobs)
                .WithOptional(e => e.MstExternalTerminal)
                .HasForeignKey(e => e.TerminalId);

            modelBuilder.Entity<MstExternalTerminal>()
                .HasMany(e => e.MstProductMappings)
                .WithRequired(e => e.MstExternalTerminal)
                .HasForeignKey(e => e.ExternalTerminalId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalTerminal>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.MstExternalTerminal)
                .HasForeignKey(e => e.TerminalId);

            modelBuilder.Entity<MstExternalTerminal>()
                .HasMany(e => e.RequestPrices)
                .WithRequired(e => e.MstExternalTerminal)
                .HasForeignKey(e => e.TerminalId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFeeSubType>()
                .HasMany(e => e.FeeByQuantities)
                .WithRequired(e => e.MstFeeSubType)
                .HasForeignKey(e => e.FeeSubTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFeeSubType>()
                .HasMany(e => e.FuelRequestFees)
                .WithRequired(e => e.MstFeeSubType)
                .HasForeignKey(e => e.FeeSubTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFeeSubType>()
                .HasMany(e => e.MstFeeXFeeSubTypes)
                .WithRequired(e => e.MstFeeSubType)
                .HasForeignKey(e => e.FeeSubTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFeeType>()
                .HasMany(e => e.FeeByQuantities)
                .WithRequired(e => e.MstFeeType)
                .HasForeignKey(e => e.FeeTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFeeType>()
                .HasMany(e => e.FuelRequestFees)
                .WithRequired(e => e.MstFeeType)
                .HasForeignKey(e => e.FeeTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFeeType>()
                .HasMany(e => e.MstFeeXFeeSubTypes)
                .WithRequired(e => e.MstFeeType)
                .HasForeignKey(e => e.FeeTypeId)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MstFuelQuantity>()
            //    .HasMany(e => e.Companies)
            //    .WithRequired(e => e.MstFuelQuantity)
            //    .HasForeignKey(e => e.FuelQuantityId)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFuelRequestStatus>()
                .HasMany(e => e.FuelRequestXStatuses)
                .WithRequired(e => e.MstFuelRequestStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstFuelRequestType>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.MstFuelRequestType)
                .HasForeignKey(e => e.FuelRequestTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstInvoiceDeclineReason>()
                .HasMany(e => e.InvoiceXDeclineReasons)
                .WithRequired(e => e.MstInvoiceDeclineReason)
                .HasForeignKey(e => e.DeclineReasonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstInvoiceDeclineReason>()
                .HasMany(e => e.MstRoleXInvoiceDeclineReasons)
                .WithRequired(e => e.MstInvoiceDeclineReason)
                .HasForeignKey(e => e.InvoiceDeclineReasonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstInvoiceStatus>()
                .HasMany(e => e.InvoiceXInvoiceStatusDetails)
                .WithRequired(e => e.MstInvoiceStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstInvoiceType>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.MstInvoiceType)
                .HasForeignKey(e => e.InvoiceTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstInvoiceType>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.MstInvoiceType)
                .HasForeignKey(e => e.DefaultInvoiceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstInvoiceVersionStatus>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.MstInvoiceVersionStatus)
                .HasForeignKey(e => e.InvoiceVersionStatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstJobStatus>()
                .HasMany(e => e.JobXStatuses)
                .WithRequired(e => e.MstJobStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstMarginType>()
                .HasMany(e => e.DifferentFuelPrices)
                .WithOptional(e => e.MstMarginType)
                .HasForeignKey(e => e.MarginTypeId);

            modelBuilder.Entity<MstMarginType>()
                .HasMany(e => e.FeeByQuantities)
                .WithOptional(e => e.MstMarginType)
                .HasForeignKey(e => e.MarginTypeId);

            modelBuilder.Entity<MstMarginType>()
                .HasMany(e => e.FuelRequestFees)
                .WithOptional(e => e.MstMarginType)
                .HasForeignKey(e => e.MarginTypeId);

            modelBuilder.Entity<MstMobileAppTheme>()
                .HasMany(e => e.CompanyXMobileAppThemes)
                .WithRequired(e => e.MstMobileAppTheme)
                .HasForeignKey(e => e.ThemeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstMobileAppTheme>()
                .HasMany(e => e.MstMobileAppThemeDetails)
                .WithRequired(e => e.MstMobileAppTheme)
                .HasForeignKey(e => e.ThemeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstOrderCancelationReason>()
                .HasMany(e => e.OrderXCancelationReasons)
                .WithRequired(e => e.MstOrderCancelationReason)
                .HasForeignKey(e => e.ReasonId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstOrderStatus>()
                .HasMany(e => e.OrderXStatuses)
                .WithRequired(e => e.MstOrderStatus)
                .HasForeignKey(e => e.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstOrderType>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.MstOrderType)
                .HasForeignKey(e => e.OrderTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPaymentTerm>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.MstPaymentTerm)
                .HasForeignKey(e => e.PaymentTermId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPaymentTerm>()
                .HasMany(e => e.Invoices)
                .WithRequired(e => e.MstPaymentTerm)
                .HasForeignKey(e => e.PaymentTermId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPaymentTerm>()
               .HasMany(e => e.QuoteRequests)
               .WithRequired(e => e.MstPaymentTerm)
               .HasForeignKey(e => e.PaymentTermId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPhoneType>()
                .HasMany(e => e.CompanyAddresses)
                .WithRequired(e => e.MstPhoneType)
                .HasForeignKey(e => e.PhoneTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPricingType>()
                .HasMany(e => e.DifferentFuelPrices)
                .WithRequired(e => e.MstPricingType)
                .HasForeignKey(e => e.PricingTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPricingType>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.MstPricingType)
                .HasForeignKey(e => e.PricingTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstPricingType>()
                .HasMany(e => e.Resales)
                .WithRequired(e => e.MstPricingType)
                .HasForeignKey(e => e.PricingTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProcessType>()
                .HasMany(e => e.QueueMessages)
                .WithRequired(e => e.MstProcessType)
                .HasForeignKey(e => e.ProcessTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProductDisplayGroup>()
                .HasMany(e => e.MstProducts)
                .WithRequired(e => e.MstProductDisplayGroup)
                .HasForeignKey(e => e.ProductDisplayGroupId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProduct>()
                .HasMany(e => e.CompanyFavoriteFuels)
                .WithRequired(e => e.MstProduct)
                .HasForeignKey(e => e.FuelTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProduct>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.MstProduct)
                .HasForeignKey(e => e.FuelTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProduct>()
                .HasMany(e => e.MstProductMappings)
                .WithRequired(e => e.MstProduct)
                .HasForeignKey(e => e.ProductId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProduct>()
                .HasMany(e => e.RequestPrices)
                .WithRequired(e => e.MstProduct)
                .HasForeignKey(e => e.ProductId)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<MstProductType>()
                .HasMany(e => e.MstProducts)
                .WithRequired(e => e.MstProductType)
                .HasForeignKey(e => e.ProductTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstQuantityType>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.MstQuantityType)
                .HasForeignKey(e => e.QuantityTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstRackAvgPricingType>()
                .HasMany(e => e.DifferentFuelPrices)
                .WithOptional(e => e.MstRackAvgPricingType)
                .HasForeignKey(e => e.RackAvgTypeId);

            modelBuilder.Entity<MstRackAvgPricingType>()
                .HasMany(e => e.Resales)
                .WithOptional(e => e.MstRackAvgPricingType)
                .HasForeignKey(e => e.RackAvgTypeId);

            modelBuilder.Entity<MstRole>()
                .HasMany(e => e.MstCompanyTypeXRoles)
                .WithRequired(e => e.MstRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstRole>()
                .HasMany(e => e.MstCompanyUserRoleXEventTypes)
                .WithRequired(e => e.MstRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstRole>()
                .HasMany(e => e.MstRoleXInvoiceDeclineReasons)
                .WithRequired(e => e.MstRole)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstRole>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithRequired(e => e.MstRole)
                .HasForeignKey(e => e.BusinessType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstRole>()
                .HasMany(e => e.Users)
                .WithMany(e => e.MstRoles)
                .Map(m => m.ToTable("UserXRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.AssetAdditionalDetails)
                .WithOptional(e => e.MstState)
                .HasForeignKey(e => e.LicensePlateStateId);

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.CompanyAddresses)
                .WithRequired(e => e.MstState)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.MstState)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.MstExternalTerminals)
                .WithRequired(e => e.MstState)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithRequired(e => e.MstState)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstTaxExemptLicenseStatus>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithRequired(e => e.MstTaxExemptLicenseStatus)
                .HasForeignKey(e => e.Status)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstTaxPricingType>()
                .HasMany(e => e.TaxDetails)
                .WithOptional(e => e.MstTaxPricingType)
                .HasForeignKey(e => e.TaxPricingTypeId);

            modelBuilder.Entity<MstTimeCardAction>()
                .HasMany(e => e.TimeCardEntries)
                .WithRequired(e => e.MstTimeCardAction)
                .HasForeignKey(e => e.ActionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstWeekDay>()
                .HasMany(e => e.DeliverySchedules)
                .WithRequired(e => e.MstWeekDay)
                .HasForeignKey(e => e.WeekDayId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstWeekDay>()
                .HasMany(e => e.SupplierAddressXWorkingHours)
                .WithRequired(e => e.MstWeekDay)
                .HasForeignKey(e => e.WeekDayId);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.AssetDrops)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasOptional(e => e.OrderAdditionalDetail)
                .WithRequired(e => e.Order);

            modelBuilder.Entity<SourceRegion>()
               .HasOptional(e => e.SourceRegionAddress)
               .WithRequired(e => e.SourceRegion);

            modelBuilder.Entity<FuelSurchargeIndex>()
              .HasMany(e => e.FuelSurchargeGeneratedTables)
              .WithRequired(e => e.FuelSurchargeIndex)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<FuelSurchargeGeneratedTable>()
               .Property(e => e.FuelSurchargePercentage)
               .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeGeneratedTable>()
              .Property(e => e.PriceRangeStartValue)
              .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeGeneratedTable>()
              .Property(e => e.PriceRangeEndValue)
              .HasPrecision(18, 8);

            modelBuilder.Entity<OrderAdditionalDetail>()
                .Property(e => e.Allowance)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.DeliveryScheduleXTrackableSchedules)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.EnrouteDeliveryHistories)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Orders1)
                .WithOptional(e => e.Order1)
                .HasForeignKey(e => e.ParentId);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderDeliverySchedules)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasOptional(e => e.OrderXCancelationReason)
                .WithRequired(e => e.Order);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.FuelDispatchLocations)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderXDrivers)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.OrderXStatuses)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SourceRegion>()
               .HasMany(e => e.SourceRegionCarrier)
               .WithRequired(e => e.SourceRegion)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<SourceRegion>()
              .HasMany(e => e.SourceRegionPickupLocation)
              .WithRequired(e => e.SourceRegion)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasOptional(e => e.OrderXTogglePricingDetail)
                .WithRequired(e => e.Order);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Spills)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.TaxExemptLicenses)
                .WithMany(e => e.Orders)
                .Map(m => m.ToTable("OrderXTaxExemptLicenses").MapLeftKey("OrderId").MapRightKey("TaxExemptLicenseId"));

            modelBuilder.Entity<Order>()
                .Property(e => e.BrokeredMaxQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<QueueMessage>()
                .HasMany(e => e.QueueResults)
                .WithRequired(e => e.QueueMessage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RequestPrice>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<RequestPrice>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<RequestPrice>()
                .HasMany(e => e.RequestFuels)
                .WithRequired(e => e.RequestPrice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ResaleFee>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Resale>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Resale>()
                .HasMany(e => e.ResaleFees)
                .WithRequired(e => e.Resale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SpecialInstruction>()
                .HasMany(e => e.InvoiceXSpecialInstructions)
                .WithRequired(e => e.SpecialInstruction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SupplierAddressXWorkingHour>()
                .Property(e => e.StartTime)
                .HasPrecision(0);

            modelBuilder.Entity<SupplierAddressXWorkingHour>()
                .Property(e => e.EndTime)
                .HasPrecision(0);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.ProductCategory)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.TaxRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.TaxAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.SalesTaxBaseAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.TradingTaxAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxDetail>()
                .Property(e => e.LicenseNumber)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.EntityCustomId)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.IDType)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.IDCode)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.TradeName)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.LegalName)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.Jurisdiction)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.County)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.LicenseNumber)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.LicensePercentage)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TaxExemptLicens>()
                .Property(e => e.AccountCustomId)
                .IsUnicode(false);

            modelBuilder.Entity<TaxExemptLicens>()
                .HasMany(e => e.TaxDetails)
                .WithOptional(e => e.TaxExemptLicens)
                .HasForeignKey(e => e.LicenseId);

            modelBuilder.Entity<TimeCardEntry>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TimeCardEntry>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<User>()
                .HasMany(e => e.AppLocations)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.AppMessageXUserStatuses)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.AssetDrops)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.DroppedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CompanyBlacklists)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AddedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CompanyBlacklists1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.RemovedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CompanyFavoriteFuels)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AddedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CompanyFavoriteFuels1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.RemovedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CompanyXAdditionalUserInvites)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.InvitedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CompanyXStripeCards)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CounterOffers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.BuyerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CounterOffers1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.SupplierId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CreditAppDetails)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.From)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.CreditAppDocuments)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AddedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DeliverySchedules)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DeliveryScheduleXDrivers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.DriverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.DeliveryScheduleXTrackableSchedules)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.DriverId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.EnrouteDeliveryHistories)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.FuelRequests)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Invoices)
                .WithOptional(e => e.Driver)
                .HasForeignKey(e => e.DriverId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Invoices1)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.InvoiceXInvoiceStatusDetails)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UpdatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Jobs)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Jobs1)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.PoContactId);

            modelBuilder.Entity<User>()
               .HasMany(e => e.FuelRequestDetails)
               .WithOptional(e => e.PoContactUser)
               .HasForeignKey(e => e.PoContactId);

            modelBuilder.Entity<User>()
                .HasMany(e => e.JobXApprovalUsers)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Notifications)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.TriggeredBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AcceptedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrderDeliverySchedule)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrderXCancelationReasons)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CanceledBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrderXDrivers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.DriverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.OrderXStatuses)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UpdatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PrivateSupplierLists)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AddedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.PrivateSupplierLists1)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.UpdatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.TimeCardEntries)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.DriverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserCodes)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserTokens)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserXInvites)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.InvitedBy);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.BillingAddresses)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<MstPhoneType>()
               .HasMany(e => e.BillingAddresses)
               .WithRequired(e => e.MstPhoneType)
               .HasForeignKey(e => e.PhoneTypeId);

            modelBuilder.Entity<BillingAddress>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<BillingAddress>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<MstOnboardingQuestionType>()
                .HasMany(e => e.MstOnboardingQuestions)
                .WithRequired(e => e.MstOnboardingQuestionType)
                .HasForeignKey(e => e.QuestionTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.OnboardingQuestionAnswers)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<MstOnboardingQuestion>()
                .HasMany(e => e.OnboardingQuestionAnswers)
                .WithRequired(e => e.MstOnboardingQuestion)
                .HasForeignKey(e => e.OnboardingQuestionId);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.HaulerPricingMatrices)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<HaulerPricingMatrix>()
                .Property(e => e.MinGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<HaulerPricingMatrix>()
                .Property(e => e.MaxGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<HaulerPricingMatrix>()
                .Property(e => e.Price)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.CurrentCosts)
                .WithRequired(e => e.SupplierCompany)
                .HasForeignKey(e => e.SupplierCompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalMeterService>()
                .HasMany(e => e.Orders)
                .WithOptional(e => e.MstExternalMeterService)
                .HasForeignKey(e => e.ExternalMeterServiceId);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.TaxExclusions)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.DirectTaxes)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<MstAccountType>()
                .HasMany(e => e.Companies)
                .WithRequired(e => e.MstAccountType)
                .HasForeignKey(e => e.AccountTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ExternalSupplierAddress>()
              .HasMany(e => e.MstSupplierQualifications)
              .WithMany(e => e.ExternalSupplierAddresses)
              .Map(m => m.ToTable("ExternalSupplierAddressQualifications").MapLeftKey("AddressId").MapRightKey("QualificationId"));

            modelBuilder.Entity<ExternalSupplierAddress>()
              .HasMany(e => e.MstProductTypes)
              .WithMany(e => e.ExternalSupplierAddresses)
              .Map(m => m.ToTable("ExternalSupplierAddressProductTypes").MapLeftKey("AddressId").MapRightKey("ProductTypeId"));

            modelBuilder.Entity<ExternalSupplierAddress>()
                 .HasMany(e => e.MstStates)
                 .WithMany(e => e.ExternalSupplierAddresses)
                 .Map(m => m.ToTable("ExternalSupplierAddressServingStates").MapLeftKey("AddressId").MapRightKey("StateId"));

            modelBuilder.Entity<CurrentCost>()
                .Property(e => e.Cost)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalSupplier>()
               .HasMany(e => e.ExternalSupplierAddresses)
               .WithRequired(e => e.ExternalSupplier)
               .HasForeignKey(e => e.ExternalSupplierId)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<ExternalSupplier>()
                .HasMany(e => e.ExternalSupplierStatuses)
                .WithRequired(e => e.ExternalSupplier)
                .HasForeignKey(e => e.ExternalSupplierId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ExternalSupplier>()
                .HasMany(e => e.ExternalSupplierNotes)
                .WithRequired(e => e.ExternalSupplier)
                .HasForeignKey(e => e.ExternalSupplierId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ExternalSupplierAddress>()
               .HasMany(e => e.ExternalSupplierAddressTruckTypes)
               .WithRequired(e => e.ExternalSupplierAddress)
               .HasForeignKey(e => e.AddressId)
               .WillCascadeOnDelete(true);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.AssetSubcontractors)
                .WithRequired(e => e.Asset)
                .HasForeignKey(e => e.AssetId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Subcontractor>()
                .HasMany(e => e.AssetSubcontractors)
                .WithRequired(e => e.Subcontractor)
                .HasForeignKey(e => e.SubcontractorId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.AssetContractNumbers)
                .WithRequired(e => e.Asset)
                .HasForeignKey(e => e.AssetId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ExternalDropDetail>()
                .Property(e => e.DropStartLatitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalDropDetail>()
                .Property(e => e.DropStartLongitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalDropDetail>()
                .Property(e => e.DropEndLatitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalDropDetail>()
                .Property(e => e.DropEndLongitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.ExternalDropDetails)
                .WithRequired(e => e.Order)
                .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<DeliveryScheduleXTrackableSchedule>()
                .HasMany(e => e.ExternalDropDetails)
                .WithOptional(e => e.DeliveryScheduleXTrackableSchedule)
                .HasForeignKey(e => e.TrackableScheduleId);

            modelBuilder.Entity<ExternalSupplierAddress>()
               .Property(e => e.Latitude)
               .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalSupplierAddress>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalBrokerOrderDetail>()
                .HasRequired(t => t.Order)
                .WithOptional(t => t.ExternalBrokerOrderDetail);

            modelBuilder.Entity<ExternalBrokerBuySellDetail>()
                .HasRequired(t => t.Order)
                .WithOptional(t => t.ExternalBrokerBuySellDetail);

            modelBuilder.Entity<ExternalBrokerBuySellDetail>()
                .Property(e => e.BrokerMarkUp)
                .HasPrecision(18, 8);

            modelBuilder.Entity<ExternalBrokerBuySellDetail>()
                .Property(e => e.SupplierMarkUp)
                .HasPrecision(18, 8);

            modelBuilder.Entity<User>()
               .HasMany(e => e.Quotations)
               .WithRequired(e => e.User)
               .HasForeignKey(e => e.CreatedBy);

            modelBuilder.Entity<User>()
                .HasMany(e => e.QuoteRequests)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstOrderType>()
                .HasMany(e => e.QuoteRequests)
                .WithRequired(e => e.MstOrderType)
                .HasForeignKey(e => e.OrderTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstProduct>()
               .HasMany(e => e.QuoteRequests)
               .WithRequired(e => e.MstProduct)
               .HasForeignKey(e => e.FuelTypeId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstTfxProduct>()
               .HasMany(e => e.MstProducts)
                .WithOptional(e => e.MstTFXProduct)
                .HasForeignKey(e => e.TfxProductId);

            modelBuilder.Entity<QuoteRequest>()
                .Property(e => e.Quantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<QuoteRequest>()
                 .HasMany(e => e.DeclinedUsers)
                 .WithMany(e => e.DeclinedQuoteRequests)
                 .Map(m => m.ToTable("SupplierDeclinedQuoteRequests").MapLeftKey("QuoteRequestId").MapRightKey("SupplierId"));

            modelBuilder.Entity<QuoteRequest>()
                .HasMany(e => e.MstSupplierQualifications)
                .WithMany(e => e.QuoteRequests)
                .Map(m => m.ToTable("QuoteRequestQualifications").MapLeftKey("QuoteRequestId").MapRightKey("QualificationId"));

            modelBuilder.Entity<Quotation>()
                .HasMany(e => e.QuotationFees)
                .WithMany(e => e.Quotations)
                .Map(m => m.ToTable("QuotationFees").MapLeftKey("QuotationId").MapRightKey("FeeId"));

            modelBuilder.Entity<Quotation>()
                .Property(e => e.SupplierCost)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Quotation>()
                .Property(e => e.PricePerGallon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<QuoteRequest>()
                .HasMany(e => e.PrivateSupplierLists)
                .WithMany(e => e.QuoteRequests)
                .Map(m => m.ToTable("QuoteRequestPrivateSupplierLists").MapLeftKey("QuoteRequestId").MapRightKey("PrivateSupplierListId"));

            modelBuilder.Entity<MstExternalSupplierType>()
                .HasMany(e => e.ExternalSupplier)
                .WithRequired(e => e.MstExternalSupplierType)
                .HasForeignKey(e => e.CompanyTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstExternalSupplierType>()
                .HasMany(e => e.ConvertedSupplier)
                .WithRequired(e => e.MstExternalSupplierType)
                .HasForeignKey(e => e.Type)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.ExternalSupplierAddresses1)
                .WithRequired(e => e.MstState)
                .HasForeignKey(e => e.StateId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MstCountry>()
                .HasMany(e => e.ExternalSupplierAddresses)
                .WithRequired(e => e.MstCountry)
                .HasForeignKey(e => e.CountryId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(x => x.OfferTierMappings)
                .WithRequired(x => x.SupplierCompany)
                .HasForeignKey(x => x.SupplierCompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasOptional(e => e.CompanySetting)
                .WithRequired(e => e.Company);

            modelBuilder.Entity<AccountingWorkflow>()
                .HasMany(e => e.QbRequests)
                .WithRequired(x => x.AccountingWorkflow)
                .HasForeignKey(x => x.AccountingWorkflowId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<QbCompanyProfile>()
                .HasMany(e => e.QbPaymentTerms)
                .WithRequired(x => x.QbCompanyProfile)
                .HasForeignKey(x => x.QbProfileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CurrencyRate>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.BaseHedgeDroppedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.BaseSpotDroppedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.BaseSpotDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelRequest>()
                .Property(e => e.BaseHedgeDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BaseDroppedQuntity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BasePrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.StateTax)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BaseFedTax)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
            .Property(e => e.BaseSalesTax)
            .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
            .Property(e => e.BaseStateTax)
            .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BaseBasicAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BaseTotalTaxAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BaseRackPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.BaseTotalFeeAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceXAdditionalDetail>()
                .Property(e => e.SupplierAllowance)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceXAdditionalDetail>()
                .Property(e => e.TotalAllowance)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.NetQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.GrossQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.DeliveredQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<PreLoadBolDetail>()
               .Property(e => e.NetQuantity)
               .HasPrecision(18, 8);

            modelBuilder.Entity<PreLoadBolDetail>()
                .Property(e => e.GrossQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.BaseHedgeDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.BaseSpotDroppedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.BaseSpotDroppedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Job>()
                .Property(e => e.BaseHedgeDroppedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
               .Property(e => e.BaseHedgeAmount)
               .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
               .Property(e => e.BaseSpotAmount)
               .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
               .Property(e => e.BasePrice)
               .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<JobBudget>()
                .Property(e => e.BaseBudget)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderTaxDetail>()
                .Property(e => e.BaseTaxRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderTaxDetail>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Quotation>()
                .Property(e => e.BasePrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Quotation>()
                .Property(e => e.BaseSupplierCost)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Quotation>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Resale>()
                .Property(e => e.ExchangeRate)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Resale>()
                .Property(e => e.BasePrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OfferUpdateCommand>()
                .Property(e => e.UpdatedAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.GrossGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .Property(e => e.NetGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeTable>()
                .Property(e => e.PriceRangeEndValue)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeTable>()
                .Property(e => e.PriceRangeStartValue)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeTable>()
                .Property(e => e.FuelSurchargePercentage)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeIndex>()
                .Property(e => e.IndexPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeIndex>()
                .Property(e => e.PriceStartValue)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeIndex>()
                .Property(e => e.PriceEndValue)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeIndex>()
                .Property(e => e.PriceInterval)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeIndex>()
                .Property(e => e.SurchargeStartPercent)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelSurchargeIndex>()
                .Property(e => e.SurchargeInterval)
                .HasPrecision(18, 8);

            modelBuilder.Entity<EIAPriceUpdate>()
                .Property(e => e.Price)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoicePayment>()
                .Property(e => e.AmountPaid)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoicePayment>()
                .Property(e => e.BalanceRemaining)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoicePayment>()
                .Property(e => e.TotalAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
                .HasMany(x => x.InvoiceExceptions)
                .WithRequired(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceHeaderDetail>()
                .Property(e => e.TotalDroppedGallons)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceHeaderDetail>()
                .Property(e => e.TotalBasicAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceHeaderDetail>()
                .Property(e => e.TotalFeeAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceHeaderDetail>()
                .Property(e => e.TotalTaxAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceHeaderDetail>()
                .Property(e => e.TotalDiscountAmount)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderGroupXOrder>()
                .Property(e => e.BlendPercentage)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderGroupXOrder>()
                .Property(e => e.MinVolume)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderGroupXOrder>()
                .Property(e => e.MaxVolume)
                .HasPrecision(18, 8);

            modelBuilder.Entity<OrderGroupXOrder>()
                .Property(e => e.TotalDropped)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TermOrderGroupHistory>()
                .Property(e => e.MinVolume)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TermOrderGroupHistory>()
                .Property(e => e.MaxVolume)
                .HasPrecision(18, 8);

            modelBuilder.Entity<TermOrderGroupHistory>()
                .Property(e => e.TotalDropped)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.OnboardingPreferences)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OnboardingPreference>()
                .HasMany(e => e.BuyerXOnboardingPreferences)
                .WithRequired(e => e.OnboardingPreference)
                .HasForeignKey(e => e.OnboardingPreferenceId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceFtlDetail>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<PreLoadBolDetail>()
               .Property(e => e.Latitude)
               .HasPrecision(18, 8);

            modelBuilder.Entity<PreLoadBolDetail>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelDispatchLocation>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<FuelDispatchLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceDispatchLocation>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceDispatchLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<BulkPlantLocation>()
                .Property(e => e.Latitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<BulkPlantLocation>()
                .Property(e => e.Longitude)
                .HasPrecision(18, 8);

            modelBuilder.Entity<DeliveryScheduleXTrackableSchedule>()
               .HasMany(e => e.ScheduleXBrokerOrderDetails)
               .WithRequired(e => e.TrackableSchedule)
               .HasForeignKey(e => e.TrackableScheduleId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
               .HasMany(e => e.ScheduleXBrokerOrderDetails)
               .WithRequired(e => e.Order)
               .HasForeignKey(e => e.OrderId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<LiftFileValidationRecord>()
                .Property(e => e.CorrectedQty)
                .HasPrecision(18, 8);

            modelBuilder.Entity<LiftFileValidationRecord>()
                .Property(e => e.Gross)
                .HasPrecision(18, 8);

            modelBuilder.Entity<LiftFileValidationRecord>()
                .Property(e => e.Temp)
                .HasPrecision(18, 8);

            modelBuilder.Entity<LiftFileValidationRecord>()
               .Property(e => e.Density)
               .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceTierPricingDetail>()
               .Property(e => e.PricePerGallon)
               .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceTierPricingDetail>()
               .Property(e => e.Quantity)
               .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceTierPricingDetail>()
               .Property(e => e.TierMinQuantity)
               .HasPrecision(18, 8);

            modelBuilder.Entity<InvoiceTierPricingDetail>()
               .Property(e => e.TierMaxQuantity)
               .HasPrecision(18, 8);

            modelBuilder.Entity<MstGravityConversion>()
                .Property(e => e.Gravity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<MstGravityConversion>()
                .Property(e => e.GallonsPerMetricTon)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.Gravity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<AssetDrop>()
                .Property(e => e.ConvertedQuantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<LiftFileBadgeManagementDetail>()
                .Property(e => e.CustomerNumber)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
              .Property(e => e.Gravity)
              .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
              .Property(e => e.ConvertedQuantity)
              .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
              .Property(e => e.ConversionFactor)
              .HasPrecision(18, 8);

            modelBuilder.Entity<Invoice>()
              .Property(e => e.ConvertedPricing)
              .HasPrecision(18, 8);

            modelBuilder.Entity<CompanyAddress>()
              .HasMany(e => e.CompanyXServingLocations)
              .WithRequired(e => e.CompanyAddress)
              .HasForeignKey(e => e.AddressId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvitedUser>()
                .HasMany(e => e.MstRoles)
                .WithMany(e => e.InvitedUsers)
                .Map(m => m.ToTable("InvitedUserXRoles").MapLeftKey("InvitedUserId").MapRightKey("RoleId"));

            modelBuilder.Entity<User>()
                .HasMany(e => e.InvitedUsers)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.InvitedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.InvitedUsers)
                .WithRequired(e => e.Company)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.FleetInformations)
                .WithRequired(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<MstState>()
                .HasMany(e => e.CompanyXServingLocations)
                .WithRequired(e => e.MstState)
                .HasForeignKey(e => e.StateId);

            modelBuilder.Entity<Order>()
               .HasMany<User>(s => s.OrderXUsers)
               .WithMany(c => c.OrderXUsers)
               .Map(cs =>
               {
                   cs.MapLeftKey("OrderId");
                   cs.MapRightKey("UserId");
                   cs.ToTable("OrderXUsers");
               });
        }
    }
}
