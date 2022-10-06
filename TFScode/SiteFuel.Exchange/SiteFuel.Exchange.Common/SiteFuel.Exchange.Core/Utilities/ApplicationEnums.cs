using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SiteFuel.Exchange.Utilities
{
    public enum SystemUser
    {
        System = 1,
    }

    public enum ApplicationEnvironment
    {
        Dev = 0,
        Test = 1,
        QA = 2,
        Demo = 3,
        Prod = 4
    }

    public enum TaxExclusionType
    {
        None = 0,
        NORA = 1
    }

    public enum DropAddressStatus
    {
        UnKnown = 0,
        Pending = 1,
        InProgress = 2,
        Complete = 3
    }

    public enum FCMAppUserTypes
    {
        FuelRequestCreatedBy = 1,
        AssignedUser = 2,
        Driver = 3,
        OnsiteContact = 4,
        ApprovalUser = 5
    }

    public enum IndexType
    {
        None = 0,
        [Display(Name = "API")]
        API = 1,
        [Display(Name = "Manual")]
        Manual = 2
    }

    public enum DeliveryRequestFor
    {
        UnKnown = 0,
        Tank = 1,
        Order = 2,
        ProductType = 3
    }

    public enum FeatureTypes
    {
        QuickBooks = 1,
        BuyerQuestMapping = 2,
        ManageOffer = 3,
        RequestForQuotes = 4,
        TimeCard = 5,
        AccountGroup = 6,
        ManageSMSAlerts = 7,
        ProductMapping = 8,
        Messages = 9,
        APIDashboard = 10
    }

    public enum TankScaleMeasurement
    {
        None = 0,
        [Display(Name = "cm")]
        Cm = 1,
        [Display(Name = "in")]
        Inches = 2,
        [Display(Name = "Gallons")]
        Gallons = 3,
        [Display(Name = "Litres")]
        Litres = 4
    }

    public enum AssetType
    {
        [Display(Name = "Asset")]
        Asset = 1,
        [Display(Name = "Tank")]
        Tank = 2,
        [Display(Name = "Vessle")]
        Vessle = 3,
    }

    public enum TableTypes
    {
        [Display(Name = "Master")]
        Master = 1,
        [Display(Name = "Customer Specific")]
        CustomerSpecific = 2,
        [Display(Name = "Carrier Specific")]
        CarrierSpecific = 3
    }

    public enum AssignedCompanyType
    {
        Customer = 1,
        Carrier = 2
    }

    public enum NotificationCode
    {
        Start = 1,
        Reschedule = 2,
        Reassign = 3,
        Cancel = 4,
        NewScheduleAssign = 5,
        DriverArrived = 6,
        PickUpLocationChange = 7,
        SplitLoadAddressChange = 8,
        NewDeliveryGroupAssign = 9,
        DeliveryGroupModified = 10,
        DeliveryGroupDeleted = 11,
        SendBirdChat = 12,
        OptionalPickupRefresh = 13,
    }

    public enum DsbNotificationStatus
    {
        None = 0,
        UnRead,
        Read
    }

    public enum DsbNotificationType
    {
        None = 0,
        Otto = 1
    }

    public enum MobileDropStatus
    {
        //Use from Mobile 
        None = 0,
        UnplannedDropCompleted,
        DropDiversion
    }

    public enum DiversionType
    {
        None = 0,
        Partail = 1,
        Full
    }

    public enum NotificationStatus
    {
        Success = 0,
        Failed = 1,
        Accepted = 2,
        Queued = 3,
        Sending = 4,
        Sent = 5,
        Delivered = 6,
        Undelivered = 7,
        Receiving = 8,
        Received = 9,
        ReadWhatsAppOnly = 10
    }

    public enum GridName
    {
        BuyerAsset = 1,
        BuyerJob,
        BuyerFuelRequest,
        BuyerOrder,
        BuyerInvoice,
        BuyerInvoiceForOrder,
        BuyerDigitalDropTicket,
        BuyerDigitalDropTicketForOrder,
        SupplierFuelRequest,
        SupplierOrder,
        SupplierInvoice,
        SupplierDigitalDropTicket,
        BolSummaryGrid,
        ProductMappingGrid,
        TrailerSummaryGrid,
        TractorSummaryGrid,
        InvitedDriverSummaryGrid,
        DispatcherLocationGrid,
        DispatcherMustGoGrid,
        DispatcherShouldGoGrid,
        DispatcherCouldGoGrid,
        OnboardedDriverSummaryGrid,
        DispatcherSelectedDriverTrailerGrid,
        TankTypeGrid,
        ManageException,
        ApiDashboardGrid,
        NoDataExceptionGrid,
        SelfHauledPONumbersGrid,
        WholeSalebadgeNumbersGrid,
        CarrierJobAssignmentGrid = 30,
        LiftFileCarrierNamesGrid,
        SupplierJobGrid,
        LiftFileQuebecBillingBadgeGrid,
        BDNInvoice,
        MarinePortsGrid,
        MarineVesselsGrid,
        AdditiveProductsGrid,
        ReasonCategoryGrid,
        ReasonCodesGrid,
        HeldDeliveryReqGrid
    }

    public enum DealStatus
    {
        Pending = 1,
        Accepted = 2,
        Declined = 3
    }

    public enum FreightRateRuleType
    {
        Unknown = 0,
        [Display(Name = "P2P")]
        P2P,
        [Display(Name = "Range")]
        Range,
        [Display(Name = "Route")]
        Route
    }

    public enum FreightRateCalcPreferenceType
    {
        Unknown = 0,
        Round,
        Proportion
    }

    public enum FuelSurchagePricingType
    {
        Unknown = 0,
        [Display(Name = "Weekly")]
        Weekly = 1,
        [Display(Name = "Monthly")]
        Monthly = 2,
        [Display(Name = "Annualy")]
        Annualy = 3,
        [Display(Name = "Daily")]
        Daily = 4
    }

    public enum FuelSurchageArea
    {
        Unknown = 0,
        [Display(Name = "U.S.")]
        US,
        [Display(Name = "East Coast (PADD1)")]
        EastCoast_PADD1,
        [Display(Name = "New England (PADD 1A)")]
        NewEngland_PADD1A,
        [Display(Name = "Central Atlantic (PADD 1B)")]
        CentralAtlantic_PADD1B,
        [Display(Name = "Lower Atlantic (PADD 1C)")]
        LowerAtlantic_PADD1C,
        [Display(Name = "Midwest (PADD 2)")]
        Midwest_PADD2,
        [Display(Name = "Gulf Coast (PADD 3)")]
        GulfCoast_PADD3,
        [Display(Name = "Rocky Mountain (PADD 4)")]
        RockyMountain_PADD4,
        [Display(Name = "West Coast (PADD 5)")]
        WestCoast_PADD5,
        [Display(Name = "West Coast less California")]
        WestCoastlessCalifornia
    }

    public enum FuelGroupType
    {
        Unknown = 0,
        [Display(Name = "Standard")]
        Standard = 1,
        [Display(Name = "Custom")]
        Custom = 2
    }

    public enum FreightTableStatus
    {
        Unknown = 0,
        [Display(Name = "Draft")]
        Draft = 1,
        [Display(Name = "Published")]
        Published = 2,
        [Display(Name = "Archived")]
        Archived = 3
    }

    public enum MessageType
    {
        Success = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
    }

    public enum Status
    {
        Success = 0,
        Failed = 1,
        Warning = 2,
    }

    public enum DDTConversionReason
    {
        Nothing = 0,
        AvalaraProductNotMapped = 1,
        AutoCreditRebill = 2,
        AvalaraTaxFailed = 3,
        PDITaxFailed = 4
    }

    public enum OrderAdditionalUpdateType
    {
        SupplierAllowance = 1,
        InvoiceNotificationPreference = 2,
        Other = 3
    }

    public enum TankType
    {
        [Display(Name = "Below Ground")]
        BelowGround = 1,
        [Display(Name = "Above Ground")]
        AboveGround = 2,
    }

    public enum FillType
    {
        [Display(Name = "Percent")]
        Percent = 1,
        [Display(Name = "UoM")]
        UoM = 2,
    }

    public enum TankConstruction
    {
        [Display(Name = "Select")]
        Select = 0,
        [Display(Name = "Single Wall")]
        SingleWall = 1,
        [Display(Name = "Double Wall")]
        DoubleWall = 2,
    }

    public enum LocationManagedType
    {
        [Display(Name = "Not Specified")]
        NotSpecified = 0,
        [Display(Name = "Supplier Managed")]
        SupplierManaged = 1,
        [Display(Name = "Partial Carrier Managed")]
        PartialCarrierManaged = 2,
        [Display(Name = "Fully Carrier Managed")]
        FullyCarrierManaged = 3
    }

    public enum FreightPricingMethod
    {
        [Display(Name = "Manual")]
        Manual = 1,
        [Display(Name = "Auto")]
        Auto = 2,
    }

    public enum DipTestMethod
    {
        [Display(Name = "Select")]
        Select = 0,
        [Display(Name = "Veeder-Root")]
        VeederRoot = 1,
        [Display(Name = "INCON")]
        Incon = 2,
        [Display(Name = "e-Fax")]
        eFax = 3,
        [Display(Name = "Franklin Fuel System")]
        FranklinFuelSystem = 4,
        [Display(Name = "Sentinal")]
        Sentinal = 5,
        [Display(Name = "Manual")]
        Manual = 6,
        [Display(Name = "Pedigree")]
        Pedigree = 7,
        [Display(Name = "Skybitz")]
        Skybitz = 8,
        [Display(Name = "Insight360")]
        Insight360 = 9
    }

    public enum AuthStatus
    {
        Success = 0,
        Failed = 1,
        InvalidUser = 2,
        EmailNotConfirmed = 3,
        InActiveUser = 4,
        UnderImpersonation = 5,
        AccountLocked = 6,
        LoginFailed = 7,
        UnAuthorized = 8,
        TokenFailure = 9,
        TPOBuyerNotOnboarded = 10,
        NotOnboarded = 11,
        AcceptEULA = 12
    }

    public enum DropStatus
    {
        None = 0,
        NoFuelNeeded = 1,
        AssetNotAvailable = 2,
        UnplannedTankDrop = 3
    }

    public enum LoginSource
    {
        Web = 1,
        Mobile = 2,
        Idp = 3
    }

    public enum CompanyType
    {
        Unknown = 0,
        Buyer = 1,
        Supplier = 2,
        BuyerAndSupplier = 3,
        Carrier = 4,
        SupplierAndCarrier = 5,
        BuyerSupplierAndCarrier = 6
    }

    public enum UserRoles
    {
        CarrierAdmin = -2,
        BuyerAdmin = -1,
        SupplierAdmin = 0,
        SuperAdmin = 1,
        Admin = 2,
        Buyer = 3,
        Supplier = 4,
        Driver = 5,
        OnsitePerson = 6,
        AccountingPerson = 7,
        ReportingPerson = 8,
        InternalSalesPerson = 9,
        ExternalVendor = 10,
        Carrier = 11,
        AccountSpecialist = 12,
        Dispatcher = 13,
        Sales = 14
    }

    public enum NotificationType
    {
        Nothing = 0,
        Email = 1,
        Sms = 2,
        InApp = 3,
        EmailAndSms = 4
    }

    public enum AppType
    {
        Unknown = 0,
        WebApplication = 1,
        DriverApp = 2,
        BuyerApp = 3,
        ExternalApiCaller = 4,
        NFNBuyer = 5,
        NFNSupplier = 6,
        HandabandBuyer = 7,
        HandabandSupplier = 8
    }

    public enum MobileOSType
    {
        Unknown = 0,
        Android = 1,
        iOS = 2
    }

    public enum EventType
    {
        AdditionalUserAdded = 1,
        AdditionalUserUpdated,
        AdditionalUserOnboarded,
        UserRolesUpdated,
        CompanyInformationUpdated, //NotInUse
        CompanyAddressAdded, //NotInUse
        CompanyAddressUpdated, //NotInUse
        CompanyAddressDeleted, //NotInUse
        JobCreated,
        JobUpdated,
        JobDeleted, //NotInUse
        JobClosed, //NotInUse
        JobReopen, //NotInUse
        FuelRequestCreated,
        FuelRequestUpdated,
        FuelRequestDeleted, //NotInUse
        FuelRequestCancelled, //NotInUse
        FuelRequestExpired, //NotInUse
        FuelRequestDeclined, //NotInUse
        FuelRequestAccepted,
        CounterOfferCreated,
        CounterOfferDeclined, //NotInUse
        CounterOfferAccepted, //NotInUse
        CounterOfferCancelled, //NotInUse
        OrderCompleted, //NotInUse
        OrderClosed,
        OrderCancelled,
        DeliveryRequestCreated,
        DeliveryRequestUpdated,
        DeliveryRequestCancelled, //NotInUse
        DeliveryRequestCompleted, //NotInUse
        DeliveryRequestExpired, //NotInUse
        DeliveryRequestDeclined, //NotInUse
        DeliveryRequestAccepted, //NotInUse
        InvoiceCreated,
        CreateInvoiceFromDDT,
        DryRunInvoiceCreated,
        InvoiceUpdated,
        InvoiceUpdatedWithExceedingQuantity, //NotInUse
        InvoiceDeclined, //NotInUse
        InvoiceApproved,
        InvoicePaid, //NotInUse
        InvoicePayConfirmed, //NotInUse
        AssetCreated, //NotInUse
        AssetUpdated, //NotInUse
        AssetDeleted, //NotInUse
        AssetBulkUploaded, //NotInUse
        AssetAssigned, //NotInUse
        AssetRemoved, //NotInUse
        ExternalCompanyInvited,
        ExternalCompanyInviteUpdated,
        ExternalCompanyInviteOnboarded,
        BrokerFuelRequestCreated,
        BrokerFuelRequestCancelled,
        BrokerFuelRequestAccepted, //NotInUse
        DeliveryRequestReminder,
        OrderUpdated,
        DriverAssignedToDelivery,
        DriverRemovedFromDelivery,
        DriverAssignedToOrder,
        DriverRemovedFromOrder,
        OrderCanceledAndFuelRequestResubmitted,
        OrderClosedAndFuelRequestResubmitted,
        InvoiceApprovalReminder,
        InvoiceCreatedApprovalWorkflow,
        InvoiceApprovedApprovalWorkflow,
        JobAssignment,
        InvoiceAndDropTicketApprovalWorkFlowDisabled,
        NeedFuelIntimationCreatedUsingAdvertisementWidget,
        BrokerFuelRequestUpdated,
        InvoiceTaxValuesChanged,
        DeliveryRequestRescheduled,
        RequestForInvoiceDeletion, //NotInUse
        InvoiceDeletedBySuperAdmin, //NotInUse
        CancelInvoiceDeletionRequest, //NotInUse
        TPOUserInvitedForEULAAcceptance,
        DdtCreatedAsInvoiceIsWaitingForUpdatedPrice,
        LinkUnassignedDdtToOrder,
        InvoiceCreatedViaMobileDrop,
        UnassignedDdtCreate,
        InActiveDriverAssignedToOrder,
        FuelRequestToExpireWithExpirationDate,
        FuelRequestToExpireWithOrderStartDate,
        QuotationAwarded,
        QuotationNotAwarded,
        DDTCreateAsInvoiceWaitingForTaxes = 86,
        InvoiceGeneratedEstablishConnectionWithAvalara = 87,
        EmailToMonitorAxxisDataUpdates = 88,
        NewIncomingFuelRequest = 89,
        SendActivationLink = 90,
        ProFormaEnabledForOrder = 91,
        PoNumberChangedForSingleDeliveryOrder = 92,
        PoNumberChangedForMultipleDeliveryOrder = 93,
        DealCreatedForInvoice = 94,
        DealAcceptedForInvoice = 95,
        DealDeclinedForInvoice = 96,
        OrderPaymentTermsUpdated = 97,
        BillingStatementGenerated = 98,
        QuickBooksSyncReport = 99,
        BillingStatementUpdated = 100,
        InvoiceReportGenerated = 101,
        PdfEmailAttachment = 102,
        PoNumberChanged = 103,
        QuoteRequestCreated = 104,
        NewIncomingQuoteRequest = 105,
        QuoteRequestExpired = 106,
        FuelSurchargeStatusChangedForOrder = 107,
        CreditInvoiceCreated = 108,
        RebilledInvoiceCreated = 109,
        DriverOnWayToJob = 110,
        DriverArrivedJob = 111,
        DeliveryCompleted = 112,
        DdtCreatedAsInvoiceIsWaitingForExceptionApproval = 113,
        DdtPendingToInvoiceNotification = 114,
        DdtCreatedAsInvoiceIsWaitingForPrePostDipData = 115,
        DipTestNotUpdated = 116,
        TankDeliveryRequestCreated = 117,
        CarrierEmailOrderCreated = 118,
        DeliveryQuantityVarianceExceptionRaised = 119,
        BrokerDeliveryRequestCreated = 120,
        NominationAcknowledgement = 121,
        DeliveryScheduledOutsideDeliveryWindow = 122,
        TankHitRunOutLevel = 123,
        CarrierExceedsDelivery = 124,
        LocationAttributeChange = 125,
        DeliveryClosedSendBDN = 126,
        CarrierDeliveries = 127,
        InvitedUser = 128,
        InvitedUserAdded = 129,
        InvitedNewUser = 130,
    }

    public enum EventSubType
    {
        ContactUs = 0,
        EmailVerification = 1,
        ForgotPassword,
        AdditionalUserAdded_InvitedUser,
        AdditionalUserAdded_CompanyAdmin,
        AdditionalUserUpdated_InvitedUser,
        AdditionalUserUpdated_CompanyAdmin,
        AdditionalUserOnboarded_InvitedUser,
        AdditionalUserOnboarded_CompanyAdmin,
        UserRolesUpdated_OnboardedUser,
        UserRolesUpdated_CompanyAdmin,
        ExternalCompanyInvited,
        ExternalCompanyInviteUpdated,
        ExternalCompanyInviteOnboarded_Invitee,
        ExternalCompanyInviteOnboarded_Inviter,
        ExternalCompanyInviteOnboarded_SalesPeople,
        //JobCreated_Owner,
        //JobCreated_CompanyAdmin,
        //JobCreated_OnsitePerson,
        //JobCreated_WorkflowApprover,
        JobAssignment = 20,
        //JobUpdated_Admins,
        //JobUpdated_Approver,
        //JobUpdated_WorkflowApprover,
        FuelRequestCreated_Owner = 24,
        FuelRequestCreated_CompanyAdmin,
        FuelRequestCreated_Supplier,
        FuelRequestUpdated_Owner,
        FuelRequestUpdated_CompanyAdmin,
        FuelRequestAccepted_Owner,
        FuelRequestAccepted_OwnerCompanyAdmin,
        FuelRequestAccepted_Supplier,
        FuelRequestAccepted_SupplierCompanyAdmin,
        CounterOfferCreated_Buyer,
        CounterOfferCreated_BuyerCompanyAdmin,
        CounterOfferCreated_Supplier,
        CounterOfferCreated_SupplierCompanyAdmin,
        OrderClosed_Owner,
        OrderClosed_OwnerCompanyAdmin,
        OrderClosed_Buyer,
        OrderClosed_BuyerCompanyAdmin,
        OrderClosedAndFuelRequestResubmitted_Buyer,
        OrderCancelled_Owner,
        OrderCancelled_OwnerCompanyAdmin,
        OrderCancelled_Buyer,
        OrderCancelled_BuyerCompanyAdmin,
        OrderCanceledAndFuelRequestResubmitted_Buyer,
        InvoiceCreated_Supplier,
        InvoiceCreated_SupplierCompanyAdmin,
        InvoiceCreated_Buyer,
        InvoiceExceedingQuantityCreated_Buyer,
        InvoiceCreated_BuyerCompanyAdmin,
        InvoiceApproved_Supplier,
        InvoiceApproved_SupplierCompanyAdmin,
        InvoiceApproved_Buyer,
        InvoiceApproved_BuyerCompanyAdmin,
        InvoiceApproved_Buyer_AprovalWorkflow,
        InvoiceApproved_BuyerCompanyAdmin_AprovalWorkflow,
        InvoiceApproved_Supplier_AprovalWorkflow,
        DropTicketApproved_Supplier_AprovalWorkflow,
        InvoiceApproved_SupplierCompanyAdmin_AprovalWorkflow,
        DropTicketApproved_SupplierCompanyAdmin_AprovalWorkflow,
        InvoiceExceedingQuantityCreated_Supplier,
        InvoiceExceedingQuantityCreated_SupplierCompanyAdmin,
        InvoiceExceedingQuantityCreated_BuyerCompanyAdmin,
        InvoiceApprovalReminder_Buyer,
        InvoiceApprovalReminder_BuyerCompanyAdmin,
        DropTicketApprovalReminder_Buyer,
        DropTicketApprovalReminder_BuyerCompanyAdmin,
        InvoiceApprovalReminder_Supplier,
        InvoiceApprovalReminder_SupplierCompanyAdmin,
        DropTicketApprovalReminder_Supplier,
        DropTicketApprovalReminder_SupplierCompanyAdmin,
        DropTicketCreated_Supplier,
        DropTicketCreated_SupplierCompanyAdmin,
        DropTicketCreated_Buyer,
        DropTicketExceedingQuantityCreated_Buyer,
        DropTicketCreated_BuyerCompanyAdmin,
        DropTicketApproved_Supplier,
        DropTicketApproved_SupplierCompanyAdmin,
        DropTicketApproved_Buyer,
        DropTicketApproved_BuyerCompanyAdmin,
        DropTicketApproved_Buyer_AprovalWorkflow,
        DropTicketApproved_BuyerCompanyAdmin_AprovalWorkflow,
        DropTicketExceedingQuantityCreated_Supplier,
        DropTicketExceedingQuantityCreated_SupplierCompanyAdmin,
        DropTicketExceedingQuantityCreated_BuyerCompanyAdmin,
        InvoiceCreated_Buyer_AprovalWorkflow,
        InvoiceCreated_Supplier_AprovalWorkflow,
        InvoiceCreated_AprovalWorkflow_SupplierCompanyAdmin,
        InvoiceCreated_AprovalWorkflow_BuyerCompanyAdmin,
        DropTicketCreated_Buyer_AprovalWorkflow,
        DropTicketCreated_Supplier_AprovalWorkflow,
        DropTicketCreated_AprovalWorkflow_SupplierCompanyAdmin,
        DropTicketCreated_AprovalWorkflow_BuyerCompanyAdmin,
        BrokerFuelRequestCreated_Owner,
        BrokerFuelRequestCreated_CompanyAdmin,
        BrokerFuelRequestUpdated_Owner,
        BrokerFuelRequestUpdated_CompanyAdmin,
        BrokerFuelRequestCreated_Supplier,
        BrokerFuelRequestCancelled_Owner,
        BrokerFuelRequestCancelled_CompanyAdmin,
        DeliveryRequestCreated_Buyer,
        DeliveryRequestCreated_Supplier,
        DeliveryRequestUpdated_Buyer,
        DeliveryRequestUpdated_Supplier,
        DeliveryRequestCancelled_Supplier,
        DeliveryRequestCancelled_Buyer,
        DeliveryRequestCreated_BuyerCompanyAdmin,
        DeliveryRequestUpdated_BuyerCompanyAdmin,
        DeliveryRequestCreated_SupplierCompanyAdmin,
        DeliveryRequestUpdated_SupplierCompanyAdmin,
        DeliveryRequestCancelled_BuyerCompanyAdmin,
        DeliveryRequestAccepted_Buyer,
        DeliveryRequestAccepted_Supplier,
        DeliveryRequestAccepted_BuyerCompanyAdmin,
        DeliveryRequestAccepted_SupplierCompanyAdmin,
        DeliveryRequestDeclined_Buyer,
        DeliveryRequestDeclined_Supplier,
        DeliveryRequestDeclined_BuyerCompanyAdmin,
        DeliveryRequestDeclined_SupplierCompanyAdmin,
        DeliveryRequestReminder_Buyer,
        DeliveryRequestReminder_BuyerCompanyAdmin,
        DeliveryRequestReminder_Supplier,
        DeliveryRequestReminder_SupplierCompanyAdmin,
        OrderUpdated_Buyer,
        OrderUpdated_Supplier,
        DriverAssignedToDelivery_Driver,
        DriverAssignedToDelivery_Supplier,
        DriverAssignedToDelivery_SupplierAdmin,
        DriverRemovedFromDelivery_Driver,
        DriverRemovedFromDelivery_Supplier,
        DriverRemovedFromDelivery_SupplierAdmin,
        DriverAssignedToOrder_Driver,
        DriverAssignedToOrder_Supplier,
        DriverAssignedToOrder_SupplierAdmin,
        DriverRemovedFromOrder_Driver,
        DriverRemovedFromOrder_Supplier,
        DriverRemovedFromOrder_SupplierAdmin,
        SuperAdminCreatedNewUser,
        SuperAdminOnboardedNewCompany,
        InvoiceAndDropTicketApprovalWorkFlowDisabled_Approver,
        InvoiceAndDropTicketApprovalWorkFlowDisabled_Admins,
        NeedFuelIntimationCreated,
        InvoiceTaxValuesChanged_Buyer,
        InvoiceTaxValuesChanged_Supplier,
        DeliveryRequestRescheduled_Buyer,
        DeliveryRequestRescheduled_Supplier,
        DeliveryRequestRescheduled_BuyerCompanyAdmin,
        DeliveryRequestRescheduled_SupplierCompanyAdmin,
        InvoiceDeleteRequested_Supplier,
        CancelInvoiceDeleteRequest_Supplier,
        InvoiceDeleted_SuperAdmin,
        DeliveryCompletedForTPO_Buyer,
        DeliveryRequestCreatedForTPO_Buyer,
        DeliveryRequestCreatedForTPO_BuyerCompanyAdmin,
        DeliveryRequestUpdatedForTPO_Buyer,
        DeliveryRequestUpdatedForTPO_BuyerCompanyAdmin,
        DeliveryRequestCancelledForTPO_Buyer,
        DeliveryRequestCancelledForTPO_BuyerCompanyAdmin,
        TPOUserInvitedForEULAAcceptance,
        DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Supplier,
        DropTicketCreatedAsInvoiceIsWaitingForUpdatedPrice_Buyer,
        ProgressReport,
        LinkUnassignedDdtToOrder_Buyer,
        LinkUnassignedDdtToOrder_Supplier,
        LinkUnassignedDdtToOrderInvoiceGenerate_Buyer,
        LinkUnassignedDdtToOrderInvoiceGenerate_Supplier,
        CreateUnassignedDdt_Supplier,
        DropTicketApproved_Approver,
        InvoiceApproved_Approver,
        DriverNotOnboarded_Supplier,
        FuelRequestToExpireWithExpirationDate_SuperAdmin,
        FuelRequestToExpireWithOrderStartDate_SuperAdmin,
        QuotationAwarded_Supplier,
        QuotationNotAwarded_Supplier,
        DDTCreatedAsInvoiceIsWaitingForTaxes,
        InvoiceGeneratedEstablishConnectionWithAvalara,
        NewIncomingFuelRequest_SuperAdmin,
        QuickBooksSyncReport,
        DtnFileUploaded,
        TelFuelException,
        DeliveryClosedSendBDN,
        CarrierDeliveries,
        InvitedUserAdded_CompanyAdmin,
        InvitedUserAdded_InvitedUser,
        InvitedUserAdded_NewInvitedUser
    }

    public enum FuelRequestFilterType
    {
        All = 0,
        Draft = 1,
        Open = 2,
        Accepted = 3,
        Canceled = 4,
        Expired = 5,
        Missed = 7,
        Declined = 8,
        AboutToExpire = 9
    }

    public enum TruckStatus
    {
        Unknown = 0,
        Active = 1,
        InActive = 2,
        UnderMaintenance = 3
    }
    public enum LicenceRequirementStatus
    {
        Class1 = 0,
        Class3 = 1,
    }

    public enum AssetFilterType
    {
        All = 0,
        Assigned = 1,
        NotAssigned = 2,
    }

    public enum JobFilterType
    {
        All = 0,
        UnderBudget = 1,
        NoBudget = 2,
        OverBudget = 3,
        OpenJobs = 4,
        TotalBudget
    }

    public enum OrderFilterType
    {
        All = 0,
        Open = 1,
        Closed = 2,
        Canceled = 3,
        TotalDelivered = 4,
        FiftyPlusDelivered = 5,
        DeliveryRequest = 6
    }

    public enum InvoiceFilterType
    {
        All = 0,
        Unassigned = 1,
        Received = 2,
        Approved = 3,
        Rejected = 4,
        Paid = 5,
        Unconfirmed = 6,
        Confirmed = 7,
        NotApproved = 8,
        Dues = 9,
        WaitingForApproval = 10,
        InvoiceWaitingApproval = 11,
        DropTicketWaitingApproval = 12,
        InvoiceWaitingForApprovalRejected = 13,
        DropTicketWaitingForApprovalRejected = 14,
        InvoicesFromDropTicket = 15,
        BrokerInvoices = 16,
        SupplierInvoices = 17,
        JobWaitingForApproval = 18,
        DropTicketWaitingForPrice = 19,
    }

    public enum JobStatus
    {
        Draft = 1,
        Open = 2,
        Closed = 3,
        Pending = 4
    }

    public enum PrivateSupplierListType
    {
        Regular = 1,
        TPOCreated = 2
    }

    public enum TaxExemptionLicenseStatus
    {
        Draft = 1,
        Open = 2
    }

    public enum BudgetCalculationType
    {
        NoBudget = 1,
        Budget = 2,
        Fuel = 3
    }

    public enum WeekDay
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }

    public enum WeekNumber
    {
        Week1 = 1,
        Week2 = 2,
        Week3 = 3,
        Week4 = 4
    }

    public enum ProductDisplayGroups
    {
        CommonFuelType = 1,
        LessCommonFuelType = 2,
        OtherFuelType = 3,
        FuelTypesInYourArea = 4,
        FavoriteFuelType = 5,
        MarineFuelType = 6,
        AdditiveFuelType = 7
    }

    public enum SalesCalculatorRegionType
    {
        City = 1,
        State = 2
    }

    public enum FuelRequestStatus
    {
        Draft = 1,
        Open = 2,
        Accepted = 3,
        Canceled = 4,
        Expired = 5,
        CounterOfferAccepted = 6,
        Missed = 7,
        Declined = 8
    }

    public enum InvoiceVersionStatus
    {
        Active = 1,
        InActive = 2
    }

    public enum ApplicationTemplate
    {
        None = 0,
        TrueFill = 1,
        Handaband = 2,
        NationalFuelNetwork = 3
    }

    public enum QuantityType
    {
        [Display(Name = "Fixed")]
        SpecificAmount = 1,
        [Display(Name = "Range")]
        Range = 2,
        [Display(Name = "Not Specified")]
        NotSpecified = 3,
    }

    public enum MarginType
    {
        NoChange = 1,
        Percent = 2,
        SpecificAmount = 3,
        Edit = 4
    }

    public enum PricingType
    {
        [Display(Name = "Rack Avg")]
        RackAverage = 1,
        [Display(Name = "PPG")]
        PricePerGallon = 2,
        Tier = 3,
        [Display(Name = "Fuel Cost")]
        Suppliercost = 4,
        [Display(Name = "Rack Low")]
        RackLow = 5,
        [Display(Name = "Rack High")]
        RackHigh = 6
    }

    public enum FeeType
    {
        [Display(Name = "Delivery Fee")]
        DeliveryFee = 1,
        [Display(Name = "Wet Hose Fee")]
        WetHoseFee = 2,
        [Display(Name = "Over Water Fee")]
        OverWaterFee = 3,
        [Display(Name = "Dry Run Fee")]
        DryRunFee = 4,
        [Display(Name = "Additional Fee")]
        AdditionalFee = 5,
        [Display(Name = "Resale Fee")]
        ResaleFee = 6,
        [Display(Name = "Freight Fee")]
        FreightFee = 7,
        [Display(Name = "Minimum Gallon Fee")]
        UnderGallonFee = 8,
        [Display(Name = "Environmental Fee")]
        EnvironmentalFee = 9,
        [Display(Name = "Service Fee")]
        ServiceFee = 10,
        [Display(Name = "Load Fee")]
        LoadFee = 11,
        [Display(Name = "Surcharge Fee")]
        SurchargeFee = 12,
        [Display(Name = "CC Processing Fee")]
        ProcessingFee = 13,
        [Display(Name = "Other Fee")]
        OtherFee = 14,
        [Display(Name = "SubTotal")]
        SubTotal = 15,
        [Display(Name = "Stop Off Fee")]
        StopOffFee = 16,
        [Display(Name = "Demurrage Fee Terminal")]
        DemurrageFeeTerminal = 17,
        [Display(Name = "Demurrage Fee Destination")]
        DemurrageFeeDestination = 18,
        [Display(Name = "Demurrage Other")]
        DemurrageOther = 19,
        [Display(Name = "Pump Charge")]
        PumpCharge = 20,
        [Display(Name = "Split Tank")]
        SplitTank = 21,
        [Display(Name = "Retain")]
        Retain = 22,
        [Display(Name = "Tank Delivery Fee")]
        TankDeliveryFee = 23,
        [Display(Name = "Tank Pickup Fee")]
        TankPickupFee = 24,
        [Display(Name = "Tank Rental")]
        TankRental = 25,
        [Display(Name = "Surcharge Freight Fee")]
        SurchargeFreightFee = 26,
        [Display(Name = "Additive Fee")]
        AdditiveFee = 27,
        [Display(Name = "Freight Cost")]
        FreightCost = 28
    }

    public enum FeeSubType
    {
        NoFee = 1,
        FlatFee = 2,
        ByQuantity = 3,
        ByAssetCount = 4,
        HourlyRate = 5,
        DeliveryFee = 12,
        DeliveryFeeWeekends = 13,
        DeliveryFeeHolidays = 14,
        PerRoute = 15,
        PerGallon = 17,
        Percent = 18,
        ByDistance = 19
    }

    public enum PaymentTerms
    {
        NetDays = 1,
        DueOnReceipt = 2,
        PrePaidInFull = 3,
        [Display(Name = "1% 15 Days, Net 30")]
        Net30 = 4
    }

    public enum FreightFeeItemTypeInDTNFile
    {
        ITMD = 1,
        TXDL = 2,
        ITMF = 3
    }

    public enum CloseOrderType
    {
        OnGivenThreshold = 1,
        OnCompleted = 2
    }

    public enum OrderType
    {
        Hedge = 1,
        Spot = 2,
    }

    public enum DeliveryType
    {
        OneTimeDelivery = 1,
        MultipleDeliveries = 2,
    }

    public enum RackPricingType
    {
        PlusDollar = 1,
        MinusDollar = 2,
        PlusPercent = 3,
        MinusPercent = 4
    }

    public enum BroadcastType
    {
        Private = 1,
        Public = 2,
        All = 3
    }

    public enum OrderStatus
    {
        Open = 1,
        Closed = 2,
        Canceled = 3,
        PartiallyCanceled = 4,
        PartiallyClosed = 5,
        DrRecalled = 6
    }

    public enum InvoiceStatus
    {
        Unassigned = 1,
        Received = 2,
        Approved = 3,
        Rejected = 4,
        Paid = 5,
        Unconfirmed = 6,
        Confirmed = 7,
        WaitingForApproval = 8,
        Deleted = 9,
        Draft = 10,
        Canceled = 11,
        PartiallyPaid = 12,
        Credited = 13,
        CreditedAndRebilled = 14
    }

    public enum DateTimeFormat
    {
        Date = 1,
        DateTimeHHMM24 = 2,
        DateTimeHHMM12 = 3,
        DateTimeHHMMSS24 = 4,
        DateTimeHHMMSS12 = 5
    }

    public enum CalendarEventType
    {
        Order = 1,
        Invoice = 2,
        DeliverySchedule = 3
    }

    public enum CalendarDropType
    {
        DropScheduled = 1,
        DropCompleted = 2
    }

    public enum CounterOfferStatus
    {
        Pending = 1,
        Accepted = 2,
        Declined = 3,
        Cancelled = 4,
        Countered = 5,
        Open = 6
    }

    public enum ProductTypes
    {
        Unleaded = 1,
        Kerosene,
        Propane,
        Jet,
        ClearDiesel,
        RedDyeDiesel,
        Ethanol,
        Etherin,
        FuelOil,
        NonStandardFuel,
        ClearDiesel2,
        RedDyeDiesel2,
        ConventionalGas,
        MidgradeGas,
        PremiumGas,
        RegularGas,
        OtherGas,
        Marine,
        FurnaceOil,
        AVGas,
        RegularOxygenated,
        RegularConventional,
        PremiumOxygenated,
        PremiumConventional,
        MidGradeOxygenated,
        Additives
    }

    public enum SurchargeProductTypes
    {
        Unknown = 0,
        Gasoline = 1,
        Diesel = 5
    }

    public enum InvoiceType
    {
        MobileApp = 1,
        Manual = 2,
        NullOrder = 3,
        PrePaid = 4,
        DryRun = 5,
        DigitalDropTicketManual = 6,
        DigitalDropTicketMobileApp = 7,
        BuyPrice = 8,
        Balance = 9,
        TankRental = 10,
        CreditInvoice = 11,
        RebillInvoice = 12,
        PartialCredit = 13,
        All = 14
    }

    public enum FuelRequestType
    {
        All = 0,
        FuelRequest = 1,
        CounteredFuelRequest,
        BrokeredFuelRequest,
        ThirdPartyRequest,
        QuoteRequest,
        OfferFuelRequest,
        FreightOnlyRequest,
        MarineFuelRequest
    }

    public enum PageDisplayMode
    {
        Create = 1,
        Edit = 2,
        Details = 3,
        View = 4,
        None = 5
    }

    public enum RunningMeterMode
    {
        None = -1,
        No = 0,
        Yes = 1,
    }

    public enum DeliveryScheduleType
    {
        Weekly = 1,
        BiWeekly = 2,
        Monthly = 3,
        SpecificDates = 4
    }

    public enum DeliveryScheduleStatus
    {
        None = 0,
        Pending = 1,
        Suggested = 2,
        Accepted = 3,
        Declined = 4,
        Canceled = 5,
        Rescheduled = 6,
        Completed = 7,
        [Display(Name = "Completed - Late")]
        CompletedLate = 8,
        [Display(Name = "Rescheduled - Completed")]
        RescheduledCompleted = 9,
        [Display(Name = "Rescheduled - Late")]
        RescheduledLate = 10,
        Missed = 11,
        [Display(Name = "Rescheduled-Missed")]
        RescheduledMissed = 12,
        Expired = 13,
        New = 14,
        Modified = 15,
        Reassigned = 16,
        Assigned = 17,
        Unassigned = 18,
        Discontinued = 19,
        MissedAndRescheduled = 20,
        MissedAndCanceled = 21,
        [Display(Name = "Unplanned DropCompleted")]
        UnplannedDropCompleted = 22,
        Acknowledged = 23,
        [Display(Name = "PreLoadBol Completed")]
        PreLoadBolCompleted = 24,
        Diverted = 25
    }

    public enum ManiFolded
    {
        [Display(Name = "Select")]
        Select = -1,
        [Display(Name = "No")]
        No = 0,
        [Display(Name = "Yes")]
        Yes = 1,
    }

    public enum ViewInvoices
    {
        All = 1,
        Broker = 2,
        Supplier = 3
    }

    public enum SuperAdminFilterType
    {
        All = 0,
        Active = 1,
        InActive = 2
    }

    public enum CompanyFilterType
    {
        All = 0,
        Buyer = 1,
        Supplier = 2,
        BuyerAndSupplier = 3,
        Driver = 4,
    }

    public enum SiteFuelUserFilterType
    {
        All = 0,
        InternalSalesPerson = 1,
        AllSuperAdmin = 2,
        ActiveSuperAdmin = 3,
        InActiveSuperAdmin = 4,
        ExternalVendor = 5,
        AccountSpecialist = 6
    }

    public enum CompanyUsersFilterType
    {
        All = 0,
        Active = 1,
        InActive = 2
    }

    public enum CurrentUserImpersonationFlag
    {
        Impersonating,
        AlreadyImpersonated,
        ImpersonationDone
    }

    public enum RequestFuelFilterType
    {
        All = 0,
        CustomerContacted = 1,
        BusinessDone = 2
    }

    public enum RequestFuelStatusType
    {
        IsEmailRecieved = 0,
        IsCustomerContacted = 1,
        IsBusinessDone = 2
    }

    public enum AppMessageUserType
    {
        Sender = 1,
        Recipient = 2
    }

    public enum AppMessageStatus
    {
        Draft = 1,
        Sent = 2,
        Recieved = 3,
        Deleted = 4,
    }

    public enum AppMessageQueryType
    {
        Order = 1,
        Invoice = 2,
        DDT = 3,
        Dispatch = 4
    }

    public enum AppMessageFilterType
    {
        Inbox = 1,
        SentMails = 2,
        Important = 3,
        Drafts = 4,
        Deleted = 5,
    }

    public enum AppMessageMarkingType
    {
        Read = 1,
        Unread = 2,
        Important = 3,
        Unimportant = 4,
        Deleted = 5,
    }

    public enum AppMessageComposeType
    {
        Compose = 1,
        Draft = 2,
        Reply = 3,
        Forward = 4,
    }

    public enum TimeCardAction
    {
        ClockIn = 1,
        ClockOut,
        BreakStart,
        BreakEnd,
        LunchStart,
        LunchEnd,
        FuelDeliveryStart,
        FuelDeliveryEnd,
        PickUpFuelStart,
        PickUpFuelEnd
    }

    public enum DropsFilterType
    {
        TotalDrops = 1,
        DropsWithOverage = 2
    }

    public enum TrackableDeliveryScheduleStatus
    {
        [Display(Name = "New")]
        Pending = 1,
        [Display(Name = "New")]
        Suggested = 2,
        [Display(Name = "Scheduled")]
        Accepted = 3,
        [Display(Name = "Declined")]
        Declined = 4,
        [Display(Name = "Canceled")]
        Canceled = 5,
        [Display(Name = "Rescheduled")]
        Rescheduled = 6,
        [Display(Name = "Drop Completed")]
        Completed = 7,
        [Display(Name = "Drop Completed - Late")]
        CompletedLate = 8,
        [Display(Name = "Drop Completed")]
        RescheduledCompleted = 9,
        [Display(Name = "Drop Completed - Late")]
        RescheduledLate = 10,
        [Display(Name = "Missed")]
        Missed = 11,
        [Display(Name = "Rescheduled And Missed")]
        RescheduledMissed = 12,
        [Display(Name = "Expired")]
        Expired = 13,
        [Display(Name = "Scheduled")]
        New = 14,
        [Display(Name = "Scheduled & Modified")]
        Modified = 15,
        [Display(Name = "Reassigned")]
        Reassigned = 16,
        [Display(Name = "Assigned")]
        Assigned = 17,
        [Display(Name = "Unassigned")]
        Unassigned = 18,
        [Display(Name = "Discontinued")]
        Discontinued = 19,
        [Display(Name = "Missed And Rescheduled")]
        MissedAndRescheduled = 20,
        [Display(Name = "Missed And Canceled")]
        MissedAndCanceled = 21,
        [Display(Name = "Unplanned Drop Completed")]
        UnplannedDropCompleted = 22,
        [Display(Name = "Schedule Accepted")]
        Acknowledged = 23,
        [Display(Name = "PreLoadBol Completed")]
        PreLoadBolCompleted = 24,
        [Display(Name = "Missed(Diverted)")]
        Diverted = 25
    }

    public enum EnrouteDeliveryStatus
    {
        [Display(Name = "NA")]
        Unknown = 0,
        [Display(Name = "On The Way To Location")]
        OnTheWayToJob = 1,
        [Display(Name = "Driver Canceled")]
        DriverCanceled = 2,
        [Display(Name = "Delayed")]
        Delayed = 3,
        [Display(Name = "Completed Drop")]
        CompletedDrop = 4,
        [Display(Name = "Reassigned")]
        Reassigned = 5,
        [Display(Name = "Rescheduled")]
        Rescheduled = 6,
        [Display(Name = "Supplier Canceled")]
        SupplierCanceled = 7,
        [Display(Name = "Buyer Canceled")]
        BuyerCanceled = 8,
        [Display(Name = "Started And Delayed")]
        StartAndDelay = 9,
        [Display(Name = "Driver Uncanceled")]
        DriverUncanceled = 10,
        [Display(Name = "On The Way To Terminal")]
        OnTheWayToTerminal = 11,
        [Display(Name = "Arrived At Terminal")]
        ArrivedAtTerminal = 12,
        [Display(Name = "Waiting Before Drop Location")]
        WaitingBeforeDrop = 13,
        [Display(Name = "PickUp Canceled")]
        PickUpCanceled = 14,
        [Display(Name = "PickUp Started")]
        PickUpStarted = 15,
        [Display(Name = "PickUp Completed")]
        PickUpCompleted = 16,
        [Display(Name = "Waiting Before Fuel Pickup")]
        WaitingBeforeFuelPickup = 17,
        [Display(Name = "Arrived At Drop Location")]
        ArrivedAtJob = 18,
        [Display(Name = "Split Tank")]
        SplitTank = 19,
        [Display(Name = "Fuel Truck Retained")]
        FuelTruckRetain = 20,
        [Display(Name = "PreLoadBol Completed")]
        PreLoadBolCompleted = 21
    }

    public enum EntityType
    {
        Unknown = 0,
        Job = 1,
        Order = 2,
        Invoice = 3,
        DigitalDropTicket = 4,
        DeliverySchedule = 5,
        QuoteRequest = 6,
        FuelRequest = 7,
        Offer = 8,
        BillingStatement = 9
    }

    public enum NewsfeedEvent
    {
        Unknown = 0,
        BuyerOrderDeliveryScheduleAdded = 1,
        BuyerOrderDeliveryScheduleCanceled = 2,
        BuyerOrderClosed = 3,
        BuyerOrderMessage = 4,
        BuyerOrderModified = 5,
        BuyerOrderDeliveryScheduleModified = 6,
        BuyerOrderDeliveryScheduleRemoved = 7,
        BuyerOrderCanceled = 8,
        BuyerInvoiceAccepted = 9,
        BuyerInvoiceApproved = 10,
        BuyerInvoiceMessage = 11,
        BuyerInvoiceModified = 12,
        BuyerInvoiceRejected = 13,
        BuyerDigitalDropTicketAccepted = 14,
        BuyerDigitalDropTicketApproved = 15,
        BuyerDigitalDropTicketMessage = 16,
        BuyerDigitalDropTicketModified = 17,
        BuyerDigitalDropTicketRejected = 18,
        SupplierOrderAccepted = 19,
        SupplierOrderDeliveryScheduleAdded = 20,
        SupplierOrderDriverAssigned = 21,
        SupplierOrderBrokered = 22,
        SupplierOrderDeliveryScheduleCanceled = 23,
        SupplierOrderCanceled = 24,
        SupplierOrderClosed = 25,
        SupplierOrderMessage = 26,
        SupplierOrderModified = 27,
        SupplierOrderDeliveryScheduleModified = 28,
        SupplierOrderDriverReassigned = 29,
        SupplierOrderDeliveryScheduleRemoved = 30,
        SupplierInvoiceCreated = 31,
        SupplierInvoiceMessage = 32,
        SupplierInvoiceModified = 33,
        SupplierDigitalDropTicketCreated = 34,
        SupplierDigitalDropTicketMessage = 35,
        SupplierDigitalDropTicketModified = 36,
        SupplierDigitalDropTicketConvertedtoInvoice = 37,
        SystemOrderClosed = 38,
        SystemOrderDeliveryScheduleMissed = 39,
        SystemInvoiceCreated = 40,
        SystemInvoiceWaitingforApproval = 41,
        SystemDigitalDropTicketWaitingforApproval = 42,
        SystemDigitalDropTicketCreated = 43,
        BuyerCanceledSchedule = 44,
        SupplierCanceledSchedule = 45,
        BuyerReschedulesSchedule = 46,
        SupplierReschedulesSchedule = 47,
        SupplierDeliveryDriverReassigned = 48,
        NewJobCreated = 49,
        DraftJobCreated = 50,
        JobClosed = 51,
        JobReOpened = 52,
        JobModified = 53,
        EnabledAssetLevelTracking = 54,
        DisabledAssetLevelTracking = 55,
        EnabledResaleDetails = 56,
        DisabledResaleDetails = 57,
        EnabledInvoiceDDTWorkflowApproval = 58,
        DisabledInvoiceDDTWorkflowApproval = 59,
        EnabledSalesTaxExempt = 60,
        DisabledSalesTaxExempt = 61,
        EnabledAssetPictureRequiredDuringDrop = 62,
        DisabledAssetPictureRequiredDuringDrop = 63,
        AssetAdded = 64,
        AssetRemoved = 65,
        AssetReassigned = 66,
        AssetModified = 67,
        NewFuelRequestCreated = 68,
        FuelRequestExpireSoon = 69,
        FuelRequestExpired = 70,
        FuelRequestModified = 71,
        CounterOfferFromSupplier = 72,
        CounterOfferFromBuyer = 73,
        BuyerAcceptedCounterOffer = 74,
        BuyerDeclinedCounterOffer = 75,
        SupplierAcceptedCounterOffer = 76,
        SupplierDeclinedCounterOffer = 77,
        BuyerCanceledFuelRequest = 78,
        SupplierDeliveryDriverAssigned = 79,
        BuyerJobDeliveryScheduleAdded = 80,
        BuyerJobDeliveryScheduleModified = 81,
        BuyerJobDeliveryScheduleRemoved = 82,
        BuyerJobDeliveryScheduleCanceled = 83,
        BuyerJobRescheduledMissedDeliverySchedule = 84,
        SupplierJobDeliveryScheduleAdded = 85,
        SupplierJobDeliveryScheduleModified = 86,
        SupplierJobDeliveryScheduleRemoved = 87,
        SupplierJobDeliveryScheduleCanceled = 88,
        SupplierJobRescheduledMissedDeliverySchedule = 89,
        BuyerRenamedPONumber = 90,
        SupplierOrderDriverUnassigned = 91,
        SupplierDeliveryDriverUnassigned = 92,
        SupplierDigitalDropTicketDraftCreated = 93,
        SupplierDigitalDropTicketCanceled = 94,
        SupplierDigitalDropTicketDraftModified = 95,
        SupplierDigitalDropTicketDraftConverted = 96,
        SupplierOrderDigitalDropTicketCanceled = 97,
        SupplierOrderDigitalDropTicketDraftModified = 98,
        SupplierThirdPartyOrderCreated = 99,
        CounterOfferAcceptedByBuyerAndOrderCreated = 100,
        CounterOfferAcceptedBySupplierAndOrderCreated = 101,
        GlobalCostToCurrentCost = 102,
        CurrentCostToGlobalCost = 103,
        GlobalCostUpdate = 104,
        CurrentCostUpdate = 105,
        QuoteRequestCreated = 106,
        ReceivedQuote = 107,
        QuoteRequestCanceled = 108,
        QuoteRequestCompleted = 109,
        QuoteRequestExpired = 110,
        RFQDueDateModified = 111,
        QuotesNeededModified = 112,
        RFQNoteModified = 113,
        RFQAttachmentUploaded = 114,
        RFQAttachmentRemoved = 115,
        DDTCreatedwaitingForUpdatedPrice = 116,
        SubContractorAddedToAsset = 117,
        SubContractorUpdatedForAsset = 118,
        SubContractorRemovedFromAsset = 119,
        SubContractorBulkUploadAsset = 120,
        SupplierUnassignedDigitalDropTicketCreated = 121,
        SupplierAssignDigitalDropTicketToOrder = 122,
        AssetLevelTrackingEnableFromAssignDDTToOrder = 123,
        BuyerApprovedDDT = 124,
        DDTToInvoiceWaitingForPrice = 125,
        SupplierBrokeredOrderAutoCanceled = 126,
        SupplierOriginalOrderChosen = 127,
        SupplierAdaptedOrderChosen = 128,
        SystemDDTCreatedSingleDelivery = 129,
        SystemInvoiceCreatedSingleDelivery = 130,
        OrderClosedOnDateExpired = 131,
        SupplierAssignDDTToOrder = 132,
        SupplierDDTCreatedAfterDateExpiredSingleDelivery = 133,
        SupplierAssignDDTToOrderAfterDateExpired = 134,
        SupplierAssignInvoiceToOrderAfterDateExpired = 135,
        SupplierManualDDTCreatedBeforeDateExpiredSingleDelivery = 136,
        SupplierManualInvoiceCreatedBeforeDateExpiredSingleDelivery = 137,
        SupplierManualDDTCreatedAfterDateExpiredSingleDelivery = 138,
        SupplierManualInvoiceCreatedAfterDateExpiredSingleDelivery = 139,
        SupplierCurrentBrokeredOrderWithoutSchedulesCanceled = 140,
        SupplierCurrentBrokeredOrderWithSchedulesCanceled = 141,
        SupplierNextBrokeredOrderWithoutSchedulesAutoCanceled = 142,
        SupplierNextBrokeredOrderWithSchedulesAutoCanceled = 143,
        BuyerAwardedToSupplierOnQuote = 144,
        SupplierAssignDDTToOrderWhenApprovalWFEnabled = 145,
        BuyerApprovedDDTWaitingForPrice = 146,
        DriverDropsWaitingForApproval = 147,
        DriverDropsWaitingForUpdatedPrice = 148,
        SupplierSelectOrderCanceledNoSchedules = 149,
        SupplierCreatedDDTWaitingForApprovalSettingDDT = 150,
        SupplierCreatedDDTWaitingForUpdatedPrice = 151,
        BuyerEnabledHidePricing = 152,
        SupplierInvoiceGenerateOnLinkUnassignedDDTToOrder = 153,
        SupplierCreatedDDTWaitingForApprovalSettingInvoice = 154,
        SupplierAssignDDTToOrderWaitingForUpdatedPrice = 155,
        DDTCreatedWaitingForTaxes = 156,
        SupplierAssignDDTToOrderWaitingForTaxes = 157,
        DriverDropsWaitingForTaxes = 158,
        InvoiceGeneratedAfterEstablishConnectionWithAvalara = 159,
        DdtToInvoiceWaitingForTaxes = 160,
        EnabledProFormaPoForJob = 161,
        DisabledProFormaPoForJob = 162,
        EnabledProFormaPoForTPOOrder = 163,
        PoNumberChangedForProFormaOrder = 164,
        DealAccepted = 165,
        DealDeclined = 166,
        OfferAcceptOrderCreated = 167,
        DealCreated = 168,
        OrderPaymentTermsUpdated = 169,
        BalanceInvoiceCreated = 170,
        TankRentalInvoiceCreated = 171,
        FuelSurchargeEnabled = 172,
        FuelSurchargeDisabled = 173,
        CreditInvoiceCreated = 174,
        CreditAndRebilledInvoiceCreated = 175,
        SupplierSplitDropTicketUpdatedAutomatically = 176,
        InvoiceCreatedFromEddt = 177,
        DdtCreateFromEddt = 178,
        DdtWiatingForPriceCreatedFromEddt = 179,
        DdtWiatingForTaxCreatedFromEddt = 180,
        AutoInvoiceCreatedFromEddt = 181,
        AutoDdtCreateFromEddt = 182,
        AutoDdtWiatingForPriceCreatedFromEddt = 183,
        AutoDdtWiatingForTaxCreatedFromEddt = 184,
        SupplierCreatedDDTWaitingForPrePostDipData = 185,
        NominationCreated = 186,
        NominationModified = 187,
        OrderFieldsModified = 188
    }

    public enum TimeCardView
    {
        All = 0,
        Day = 1
    }

    public enum QueueMessageStatus
    {
        Pending = 0,
        InProcess = 1,
        Completed = 2,
        Failed = 3,
        //Can be used both for draft status i.e. before putting any queue in pending, first ensure all records are ready to be picked
        //e.g. for file upload ensure the file read and upload to blob is complete, then set it to pending status
        //Also, for temporarily putting any low/error state queue message from being put away to clear priority process in queue.
        //For now this will be backend status tracking done by technical maintenance team
        OnHold = 4,
        PartiallyCompletedWithErrors=5
    }

    public enum BulkOrderDetailsStatus
    {
        Pending = 0,
        InProcess,
        TPOValidation,
        TPOCreation,
        Completed,
        Failed
    }

    public enum OrderCancelReason
    {
        FuelNoLongerAvailable = 1,
        UnabletoMakeDeliveryWindow,
        BuyerChangedOrderTerms,
        ChangeInBusinessProfile,
        SupplierSpecifiedOtherReason,
        SupplierUnableToDeliverOnTime,
        SupplierChangedOrderTerms,
        NoLongerNeed,
        DuplicateOrder,
        BuyerSpecifiedOtherReason,
    }

    public enum QueueProcessType
    {
        [Display(Name = "TPO")]
        ThirdPartyOrderBulkUpload = 1,
        ExternalMeterDataUpload = 2,
        InvoiceCreated = 3,
        OrderCreated = 4,
        FuelRequestCreated = 5,
        OfferCreated = 6,
        DispatchLocation = 7,
        DtnFileGeneration = 8,
        TankRentalInvoice = 9,
        ReceivePayment = 10,
        [Display(Name = "TPD Invoice")]
        InvoiceBulkUpload = 11,
        CreditInvoiceCreation = 12,
        BrokerSplitInvoiceCreation = 13,
        RebillInvoiceCreation = 14,
        [Display(Name = "TPD Image(s)")]
        InvoiceImageUpload = 15,
        [Display(Name = "TPD Error(s)")]
        InvoiceUploadErrors = 16,
        [Display(Name = "Po Number")]
        PoNumberBulkUpload = 17,
        [Display(Name = "Tank")]
        TankBulkUpload = 18,
        [Display(Name = "Demand Capture")]
        DemandCaptureUpload = 19,
        [Display(Name = "Create Invoice")]
        CreateInvoiceUsingJsonFile = 20,
        [Display(Name = "Create Mobile Invoice")]
        CreateMobileInvoiceUsingJsonFile = 21,
        [Display(Name = "Tank Order Mapping")]
        CreateTankOrderMappingInFreightService = 22,
        [Display(Name = "Create Freight Only Order")]
        CreateFreightOnlyOrder = 23,
        TelaFuelServiceOrderAdd = 24,
        [Display(Name = "Product Mapping")]
        ProductMappingBulkUpload = 25,
        CloseFreightOnlyOrder = 26,
        EditBrokerInvoice = 27,
        BrokerInvoiceImageUpload = 28,
        CreateInvoiceForNoData = 29,
        ConvertBrokeredInvoiceForDipData = 30,
        [Display(Name = "Terminal Item Code Mapping")]
        TerminalItemCodeMappingBulkUpload = 31,
        PDIAPIDeliveryDetails = 32,
        FilldAPIDetails = 33,
        ConsolidationForDrCompletion = 34, // used for group consolidation + invoice confirmation + processing without Tax
        [Display(Name = "Asset")]
        AssetBulkUpload = 35,
        JobsBulkUpload = 36,
        DRCreation = 37,
        SAPAPIDeliveryDetails = 38,
        SAPOrderCreation = 39
    }

    public enum BlobContainerType
    {
        None = -1,
        Orderbulkupload = 0,
        ExternalMeterFeed = 1,
        MansfieldAMPInvoiceCsv = 2,
        JobAssetbulkupload = 3,
        QuoteRequest = 4,
        Quotation = 5,
        InvoiceBulkUpload = 6,
        SpecialInstructions = 7,
        InvoicePdfFiles = 8,
        PoNumberBulkUpload = 9,
        JobFilesUpload = 10,
        TankBulkUpload = 11,
        DriverProfilePhoto = 12,
        CreateInvoice = 13,
        CompanyProfile = 14,
        TankTypeDipChart = 15,
        BrandWebsite = 16,
        ProductMappingBulkUpload = 17,
        EditInvoice = 18,
        LiftDataProcessingPoFile = 19,
        LiftDataProcessingWholeSaleBadgeFile = 20,
        LiftDataProcessingCarrierNamesFile = 21,
        LiftDataProcessingQuebecBadgeFile = 22,
        JobsBulkUpload = 23,
        BDN = 24,
        CreditCheckApprovalFiles = 25
    }

    public enum OnboardedType
    {
        Direct = 1,
        Refferal,
        ThirdPartyOrderOnboarded
    }

    public enum SupplierCostTypes
    {
        GlobalCost = 1,
        SupplierCost
    }

    public enum OnboardingQuestionType
    {
        CompanySpecific = 1,
        ServiceSpecific = 2
    }

    public enum Trucks
    {
        Bobtail = 1,
        Transport = 2,
        TankWagon = 3
    }

    public enum ExternalSupplierType
    {
        Supplier = 1,
        Broker = 2,
        Wholesaler = 3,
        ChannelPartner = 4,
        Distributor = 5
    }

    public enum AccountType
    {
        Real = 1,
        SfxOwned = 2
    }

    public enum ExternalSupplierStatuses
    {
        New = 1,
        InProgress = 2,
        Completed = 3
    }

    public enum BrokeredOrderCustomer
    {
        Mansfield = 1,
    }

    public enum InvoicePreference
    {
        SendInvoiceDirectlyToCustomer = 1,
        SendInvoiceOnBehalfOfBroker = 2,
        SendBrokerDataFileToBroker = 3
    }

    public enum SupplierType
    {
        All = 0,
        Internal = 1,
        External = 2
    }

    public enum WaitingAction
    {
        Nothing = 0,
        [Display(Name = "Waiting for Updated Price")]
        UpdatedPrice = 1,

        [Display(Name = "Waiting for Avalara Tax")]
        AvalaraTax = 2,

        [Display(Name = "Waiting for Customer Approval")]
        CustomerApproval = 3,

        [Display(Name = "Waiting for BOL Details")]
        BolDetails = 4,

        [Display(Name = "Waiting for Exception Approval")]
        ExceptionApproval = 5,

        [Display(Name = "Waiting for Images")]
        Images = 6,

        [Display(Name = "Waiting for Pre Post Dip Data")]
        PrePostDipData = 7,

        [Display(Name = "Waiting for Inventory Verification")]
        InventoryVerification = 8,

        [Display(Name = "Waiting for Next Marine Drop")]
        NextMarineDrop = 9,

        [Display(Name = "Waiting for Other Drops")]
        AllDRCompletion = 10,

        [Display(Name = "Waiting for FILLD response")]
        FilldResponse = 11,

        [Display(Name = "Waiting for BDN Confirmation")]
        BDNConfirmation = 12,

        [Display(Name = "Waiting for Invoice Confirmation")]
        InvoiceConfirmation = 13,

        [Display(Name = "Waiting for PDIE Tax")]
        PDITaxes = 14
    }

    public enum QuoteRequestStatuses
    {
        Open = 1,
        Completed = 2,
        Expired = 3,
        Canceled = 4,
        Awarded = 6
    }

    public enum QuotationStatuses
    {
        Open = 1,
        Canceled = 4,
        Excluded = 5,
        Awarded = 6
    }

    public enum QuoteRequestFilterType
    {
        All = 0,
        Open = 1,
        Accepted = 2,
        Missed = 3,
        Declined = 4,
        Completed = 5,
        Expired = 6,
        Canceled = 7
    }

    public enum InvoiceCreationFrom
    {
        Default = 0,
        ManualSFX = 1,
        Amp = 2,
        Mobile = 3,
        AssignToOrder = 4
    }

    public enum OtherProductTaxPricingType
    {
        DollarOnTotalAmount = 1,
        PercentageOnTotalAmount = 2,
        DollarPerGallon = 3,
        PercentagePerGallon = 4
    }

    public enum FeeCategory
    {
        CommonFee = 1,
        OtherFee = 2,
        TankRental = 3
    }

    public enum FeeConstraintType
    {
        Weekend = 1,
        SpecialDate = 2
    }

    public enum Currency
    {
        None = 0,
        USD = 1,
        CAD = 2
    }
    public enum LocationType
    {
        PickUp = 1,
        Drop = 2
    }

    public enum PickupLocationType
    {
        None = 0,
        Terminal = 1,
        BulkPlant = 2
    }

    public enum Country
    {
        All = 0,
        USA = 1,
        CAN = 2,
        CAR = 4
    }

    public enum UoM
    {
        None = 0,
        [Display(Name = "Gallons", ShortName = "gallon")]
        Gallons = 1,
        [Display(Name = "Litres", ShortName = "litre")]
        Litres = 2,
        [Display(Name = "Barrel")]
        Barrels = 3,
        [Display(Name = "MetricTons", ShortName = "M/T")]
        MetricTons = 4
    }

    public enum RetainThresholdUoM
    {
        None = 0,
        [Display(Name = "Gallons", ShortName = "gallon")]
        Gallons = 1,
        [Display(Name = "Litres", ShortName = "litre")]
        Litres = 2
    }
    public enum DipTestUoM
    {
        Cm = 1,
        Inches = 2,
    }

    public enum DeleteDriverSchedule
    {
        Single = 1,
        Range = 2,
        Whole = 3
    }

    public enum InvoiceDeclinedReason
    {
        AmountOfFuelDeliveredDoesNotMatchOrder = 1,
        PricePerGallonDoesNotMatchOrder = 2,
        TheDriverCausedDamageDuringTheDelivery = 3,
        Other = 4,
        FeeWasNotAgreedUpon = 5
    }

    public enum AuditEventType
    {
        Unknown = 0,
        Insert = 1,
        Update = 2,
        Edit = 3,
        Delete = 4,
        CurrencyConversion = 5
    }

    public enum AuditEntityType
    {
        Unknown = 0,
        Job = 1,
        Order = 2,
        Notification = 3
    }

    public enum TaxType
    {
        Unknown = 0,
        Manual,
        Standard
    }

    public enum OfferLocationType
    {
        State = 1,
        City = 2
    }

    public enum OfferType
    {
        All = 0,
        Exclusive = 1,
        Market = 2,
        YourSuppliers = 3 // don't use anywhere - only used on buyer side for showing grid of buyer's suppliers
    }

    public enum OfferQuickUpdateType
    {
        Pricing = 1,
        Fees = 2
    }

    public enum OfferStatus
    {
        Draft = 1,
        Open = 2,
        Expired = 3
    }

    public enum OfferSearchEntityType
    {
        Suppliers = 1,
        FuelTypes = 2,
        PricingTypes = 3,
        States = 4
    }

    public enum OfferBuyerStatuses
    {
        Accepted = 1,
        Rejected = 2
    }

    public enum CompanyGroupType
    {
        Unknown = 0,
        Buyer = 1,
        Supplier = 2,
        BuyerAndSupplier = 3
    }

    public enum OfferPreference
    {
        Unknown = 0,
        ExclusiveOfferCustomerTier = 1,
        ExclusiveOfferCustomer = 2,
        MarketOffer = 3,
        State = 4,
        City = 5,
        PricingType = 6,
        FeeType = 7
    }

    public enum SingleDeliverySubTypes
    {
        [Display(Name = "Delivery Date")]
        DeliveryDate,
        [Display(Name = "Delivery Date Range")]
        DeliveryDateRange
    }

    public enum WebNotificationType
    {
        FuelRequestNotification = 1,
        OrderNotification,
        InvoiceNotification,
        OfferNotification,
        DispatchNotification = 5
    }

    public enum DispatchNotificationType
    {
        EnrouteStatus = 1,
        TerminalChange = 2,
        Reschedule = 3,
        CancelDelivery = 4
    }

    public enum SuperAdminDashboardTiles
    {
        [Display(Name = "Fuel Requests")]
        FuelRequests,
        [Display(Name = "Orders")]
        Orders
    }

    public enum SupplierDashboardTiles
    {
        //[Display(Name = "Calendar")]
        //Calendar,
        [Display(Name = "Dispatch")]
        Dispatch,
        [Display(Name = "Fuel Requests")]
        FuelRequests,
        [Display(Name = "Quote")]
        FuelRequestQuote,
        [Display(Name = "Invoices")]
        Invoices,
        //[Display(Name = "DropTickets")]
        //DropTickets,
        [Display(Name = "Global Fuel Cost")]
        GlobalFuelCost,
        [Display(Name = "Orders")]
        Orders,
        [Display(Name = "Gallon Stats")]
        GallonStats,
        [Display(Name = "Your Business")]
        YourBusiness,
        [Display(Name = "Drop Avg")]
        DropAverages
    }

    public enum BuyerDashboardTiles
    {
        [Display(Name = "Request For Quote")]
        RequestForQuote,
        [Display(Name = "Deliveries")]
        Deliveries,
        [Display(Name = "Fuel Requests")]
        FuelRequests,
        [Display(Name = "Invoices")]
        Invoices,
        [Display(Name = "Location Averages")]
        JobAverages,
        [Display(Name = "Orders")]
        Orders,
        [Display(Name = "Delivery Requests")]
        DeliveryRequests
    }
    public enum NewBuyerDashboardTiles
    {
        [Display(Name = "Deliveries")]
        Deliveries,
        [Display(Name = "Location Inventory")]
        LocationInventory,
        [Display(Name = "Invoices")]
        Invoices,
        [Display(Name = "Messages")]
        Messages,
    }
    public enum DriverDashboardTiles
    {
        [Display(Name = "Orders")]
        Orders
    }

    public enum FrequencyTypes
    {
        [Display(Name = "Daily")]
        Daily = 1,
        [Display(Name = "Weekly")]
        Weekly,
        [Display(Name = "Biweekly")]
        Biweekly,
        [Display(Name = "Monthly")]
        Monthly
    }

    public enum UpdateFrequencyTypes
    {
        Unknown = 0,
        Hour = 1,
        Day = 2
    }

    public enum TruckLoadTypes
    {
        [Display(Name = "Less Than Truck Load (LTL)")]
        LessTruckLoad = 1,
        [Display(Name = "Full Truck Load (FTL)")]
        FullTruckLoad = 2,
    }

    public enum FreightOnBoardTypes
    {
        [Display(Name = "Terminal (Origin)")]
        Terminal = 1,
        [Display(Name = "Destination")]
        Destination = 2
    }
    public enum CreditCheckTypes
    {
        [Display(Name = "Select Type")]
        None = 0,
        [Display(Name = "SAP")]
        SAP = 1,
        [Display(Name = "PDIE")]
        PDIE = 2,
    }

    public enum QuantityIndicatorTypes
    {
        [Display(Name = "Net")]
        Net = 1,
        [Display(Name = "Gross")]
        Gross = 2
    }

    public enum FuelClassTypes
    {
        [Display(Name = "Branded")]
        Branded = 1,
        [Display(Name = "Unbranded")]
        Unbranded = 2,
        [Display(Name = "Both (Branded & Unbranded)")]
        Both = 3
    }

    public enum InvoiceNotificationPreferenceTypes
    {
        [Display(Name = "Automatically send customer DDT & Invoice (No Billing File)")]
        SendInvoiceDDTWithoutBillingFile = 1,
        [Display(Name = "Send customer DDT & Invoice Billing File")]
        SendDDTWithBillingFile = 2,
        [Display(Name = "Send only Invoice Billing File")]
        SendOnlyBillingFile = 3,
        [Display(Name = "Send Invoice & Billing File")]
        SendInvoiceWithBillingFile = 5,
        [Display(Name = "None")]
        None = 4,
        [Display(Name = "Send Delivery Details To PDI")]
        SendPDIDeliveryDetails = 6
    }

    public enum PricingSource
    {
        [Display(Name = "Axxis")]
        Axxis = 1,
        [Display(Name = "OPIS")]
        OPIS = 2,
        [Display(Name = "PLATTS")]
        PLATTS = 3
    }

    public enum PricingSourceFeedTypes
    {
        [Display(Name = "Contract (10AM EST)")]
        Contract_10AM_EST = 1,
        [Display(Name = "Calendar (12PM EST)")]
        Calendar_12PM_EST = 2,
        [Display(Name = "End of Day (5PM EST)")]
        EOD_5PM_EST = 3,
        [Display(Name = "Previous Day (5PM EST)")]
        PreviousDay_5PM_EST = 4,
        [Display(Name = "Previous Day (10AM EST)")]
        PreviousDay_10AM_EST = 5
    }

    public enum JobLocationTypes
    {
        [Display(Name = "Location")]
        Location = 1,
        [Display(Name = "Geo Location")]
        GeoLocation = 2,
        [Display(Name = "Various")]
        Various = 3,
        [Display(Name = "Port")]
        Port = 4
    }

    public enum TruckLoadFeeCategories
    {
        OnlyLTL = 1,
        OnlyFTL = 2,
        FTLAndLTL = 3,
        FTLWaiver = 4, // fees to which waiver / embedded time is applicable
        OnlyOrder = 5
    }

    public enum DocumentName
    {
        PO = 1,
        Invoice = 2,
        BDR = 3,
        InvoiceSummary = 4,
        MarineTaxAffidavit = 5,
        CGInspection = 6,
        InspRequestVoucher = 7,
        BDNImage = 8,
    }

    public enum ActivationStatus
    {
        Created = 1,
        Active = 2,
        Inactive = 3,
        Deleted = 4
    }

    public enum Lookup
    {
        FuelSurchargeProduct = 1,
        FuelSurchargePeriod = 2,
        FuelSurchargeArea = 3,
    }

    public enum PaymentMethods
    {
        [Display(Name = "Select Method")]
        None = 0,
        [Display(Name = "Credit Card")]
        CreditCard = 1,
        [Display(Name = "Bank Check")]
        BankCheck = 2,
        [Display(Name = "ACH / Draft")]
        Draft = 3,
        [Display(Name = "Bank Wire")]
        BankWire = 4
    }

    public enum LoadQueueAttributes
    {
        [Display(Name = "Location Name")]
        LocationName = 1,
        [Display(Name = "Customer Name")]
        CustomerName = 2,
        [Display(Name = "Driver")]
        Driver = 3,
        [Display(Name = "Trailer Name")]
        TrailerName = 4
    }

    public enum DRQueueAttributes
    {
        [Display(Name = "Customer Name")]
        CustomerName = 1,
        [Display(Name = "Delivery Shift")]
        DeliveryShift = 2,
        [Display(Name = "Trailer Compatibility")]
        TrailerCompatibility = 3
    }

    public enum PaymentSource
    {
        Unknown = 0,
        QB = 1
    }

    public enum PaymentStatus
    {
        [Display(Name = "Not Paid")]
        NotPaid = 0,
        [Display(Name = "Partially Paid")]
        PartiallyPaid,
        [Display(Name = "Paid")]
        Paid
    }

    public enum CreationMethod
    {
        [Display(Name = "Manual")]
        SFX = 1,
        [Display(Name = "Mobile")]
        Mobile = 2,
        [Display(Name = "Bulk Upload")]
        BulkUploaded = 3,
        [Display(Name = "TPD API")]
        APIUpload = 4,
    }

    public enum FileType
    {
        SpecialInstruction = 1
    }

    public enum ExceptionResolution
    {
        Unknown = 0,
        ApproveBOLQuantity = 1,
        ApproveDroppedQuantity = 2,
        ApproveBOLMinusNetVariance = 3,
        ApproveDropTicket = 4,
        DiscardDropTicket = 5,
        CreateManualInvoice = 6,
        DiscardException = 7,
        AttachOrder = 8,
    }

    public enum ExceptionStatus
    {
        Unknown = 0,
        Raised = 1,
        Resolved = 2,
        AutoApproved = 3,
        Deleted = 4,
        AutoDiscard = 5
    }
    public enum ExceptionApprover
    {
        Unknown = 0,
        Self = 1,
        Buyer = 2,
        Supplier = 3,
        CarrierOrSupplier = 4

    }
    public enum WeekendDropPricingDay
    {
        [Display(Name = "Unknown")]
        Unknown = 0,
        [Display(Name = "Saturday Pricing Enabled")]
        Saturday = 7,
        [Display(Name = "Friday Pricing Enabled")]
        Friday = 6
    }

    public enum InvoiceImageType
    {
        //use Odd number for all types and even specific for Marine
        None = 0,
        [Display(Name = "Drop")]
        Drop = 1,
        [Display(Name = "Signature")]
        Signature = 3,
        [Display(Name = "BOL/Lift")]
        Bol = 5,
        [Display(Name = "Additional")]
        AdditionalImage = 7,
        [Display(Name = "Tax Affidavit")]
        TaxAffidavit = 2,
        [Display(Name = "CG Inspection")]
        CGInspection = 4,
        [Display(Name = "Request Inspection Voucher")]
        RequestInspectionVoucher = 6,
        [Display(Name = "BDN Image")]
        BDNImage = 8
    }

    public enum TableType
    {
        Unknown = 0,
        Direct = 1,
        Market = 2
    }

    public enum PriceType
    {
        Unknown = 0,
        PointToPoint = 1,
        DistanceRange = 2,
        QuantityRange = 3,
        PerGallonOrLitres = 4,
        Route = 5,
        Flat = 6
    }

    public enum WorkPreference
    {
        Unknown = 0,
        Calendar = 1,
        Shift = 2
    }

    public enum ScheduleQuantityType
    {
        [Display(Name = "Quantity")]
        Quantity = 1,
        [Display(Name = "Balance")]
        Balance = 2,
        [Display(Name = "Full Load")]
        FullLoad = 3,
        [Display(Name = "Small Compartment")]
        SmallCompartment = 4,
        [Display(Name = "Not Specified")]
        NotSpecified = 5
    }

    public enum DriverAcknowledgementStatus
    {
        [Display(Name = "Unknown")]
        Unknown = 0,
        [Display(Name = "Delivered")]
        Delivered = 1,
        [Display(Name = "Read")]
        Read = 2,
        [Display(Name = "Acknowledged")]
        Acknowledged = 3,
        [Display(Name = "Reacknowledged")]
        ReAcknowledgementNeeded = 4
    }

    public enum ExceptionType
    {
        Unknown = 0,
        DeliveredQuantityVariance = 1,
        DuplicateInvoice = 2,
        InvoiceApiException = 3,
        UnknownDeliveries = 4,
        MissingDeliveries = 5,
        UnmatchedDeliveryLocation = 6,
        OverFilledAsset = 7,
        UnassignedAsset = 8,
        ManageRetaion = 9
    }

    public enum OrderGroupType
    {
        [Display(Name = "Multi Products to Single Location")]
        MultiProducts = 1,
        [Display(Name = "Multi Term Pricing")]
        Tier = 2,
        [Display(Name = "Multi Products - Blended")]
        Blend = 3,

    }

    public enum OrderGroupFrequency
    {
        None = 0,
        Monthly = 1
    }

    public enum ProductCategory
    {
        None = 0,
        Gasoline = 1,
        Diesel = 2
    }

    public enum DeliveryReqPriority
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Must Go")]
        MustGo = 1,
        [Display(Name = "Should Go")]
        ShouldGo,
        [Display(Name = "Could Go")]
        CouldGo
    }

    public enum BrokeredDrCarrierStatus
    {
        None = 0,
        Pending = 1,
        Accepted = 2,
        Rejected = 3,
        Recalled = 4,
        Delivered = 5
    }

    public enum HeldDrStatus
    {
        New = 1,
        Pending = 2,
        Passed = 3,
        Incomplete = 4,
        ByPassed = 5
    }

    public enum DeliveryReqStatus
    {
        None = 0,
        Pending,
        Assigned,
        ScheduleCreated,
        Deleted,
        Draft,
        InUse
    }

    public enum TripStatus
    {
        None = 0,
        Added,
        Modified,
        Deleted
    }

    public enum DSBMethod
    {
        None = 0,
        Save = 1,
        DriverAssignment = 2,
        Publish = 3,
        Delete = 4,
        DragDrop = 5
    }

    public enum DeliveryGroupStatus
    {
        None = 0,
        Draft,
        Published,
        Deleted
    }
    public enum DemandSourceType
    {
        None = 0,
        Email = 1,
        FileUpload = 2,
        Manual = 3,
    }

    public enum TractorStatus
    {
        Active = 1,
        InActive = 2,
        UnderMaintenance = 3
    }
    public enum TrailerTypeStatus
    {
        [Display(Name = "L")]
        Lead = 1,
        [Display(Name = "P")]
        Pup = 2,
        [Display(Name = "Q")]
        Quad = 3,
        [Display(Name = "T")]
        Tandem = 4,
        [Display(Name = "Tri")]
        Tridem = 5
    }
    public enum LicenceRequirement
    {
        [Display(Name = "Class 1")]
        Class1 = 1,
        [Display(Name = "Class 3")]
        Class3 = 2,
    }

    public enum OnboardingPreferencePricingMethod
    {
        [Display(Name = "Rack Based")]
        Rack = 1,
        [Display(Name = "City Based")]
        City = 2,
        [Display(Name = "Fixed")]
        Fixed = 3,
        [Display(Name = "Fuel Cost")]
        FuelCost = 4,
        [Display(Name = "Multiple (Inconsistent)")]
        Multiple = 5,
    }

    public enum OrderCreationMethod
    {
        Unknown = 0,
        FromFuelRequest = 1,
        FromTPO = 2,
        FromOffer = 3,
        FromQuotes = 4
    }

    public enum ScheduleBuilderView
    {
        None = 0,
        Driver = 1,
        Trailer = 2
    }
    public enum TaxPricingTypes
    {
        [Display(Name = "$ - On Total Amount")]
        TotalAmountBased = 1,
        [Display(Name = "% - On Total Amount")]
        PercentBased = 2,
        [Display(Name = "$ - Per Gallon")]
        AmountPerGallon = 3,
        [Display(Name = "% - Per Gallon")]
        PercentPerGallon = 4,
        [Display(Name = "$ - Per Litre")]
        AmountPerLitre = 5,
        [Display(Name = "% - Per Litre")]
        PercentPerLitre = 6,
    }

    public enum AutoDrStatus
    {
        None = 0,//Manual Dr Creation
        Create = 1,//Auto Dr creation
        Update = 2,//Auto Update Dr Quantity
        CreateAndUpdate = 3,//Auto Created and AutoUpdated Dr
        Delete = 4
    }

    public enum DriverScheduleType
    {
        Planned = 1,
        UnPlanned = 2,
        All = 0
    }
    public enum OrderEnforcement
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Enforce Order Level Values")]
        EnforceOrderLevelValues = 1,
        [Display(Name = "Manage Exception")]
        ManageException = 2,
        [Display(Name = "No Enforcement")]
        NoEnforcement = 3
    }

    public enum TrackableScheduleType
    {
        PickupAndDrop = 0,
        PickupOnly = 1,
        DropOnly = 2
    }

    public enum APIResultType
    {
        Total = 3,
        Success = 0,
        Exception = 1
    }
    public enum ReqResType
    {
        Response = 1,
        Request = 2,

    }

    public enum DRSource
    {
        Manual = 1,
        Auto = 2,
        PreLoad = 3,
        API = 4,
        MissedDR = 5,
        BuyerApp = 6,
        Recreated = 7,
        Forecasting = 8,
        OttoForecasting = 9,
    }
    public enum ScheduleTypes
    {
        Weekly = 1,
        BiWeekly = 2,
        Monthly = 3
    }

    public enum LFVParameterType
    {
        None = 0,
        Input,
        Output
    }

    public enum LFVRecordStatus
    {
        None = 0,
        Clean,
        PendingMatch,
        NoMatch,
        IgnoreMatch,
        UnMatched,
        ActiveExceptions,
        ReprocessSubmitted,
        Duplicate,
        PartialMatch,
        ForcedIgnore
    }
    public enum LocationInventoryManagedBy
    {
        Telapoint = 1
    }

    public enum NoDataExceptionApproval
    {
        [Display(Name = "Add Pre & Post Dip")]
        AddPreAndPostDip = 1,
        [Display(Name = "Upload Images")]
        UploadImages = 2,
        [Display(Name = "BOL Details")]
        BOLDetails = 3,
        [Display(Name = "Request For Waiver")]
        RequestForWaiver = 4,
        [Display(Name = "Request For Waiver Sent")]
        RequestForWaiverSent = 5,
        [Display(Name = "Accept Without Data")]
        AcceptWithoutData = 6
    }
    public enum ProductSequencingCreationMethod
    {
        Unknown = 0,
        Account = 1,
        Job = 2
    }

    public enum ProductSequenceType
    {
        None = 0,
        Product = 1,
        Order = 2
    }

    public enum TierPricingType
    {
        Unknown = 0,
        [Display(Name = "Volume Based")]
        VolumeBased = 1,
        [Display(Name = "Delivery Quantity Based")]
        DeliveryQuantityBased = 2
    }

    public enum CumulationType
    {
        Unknown = 0,
        [Display(Name = "Weekly")]
        Weekly = 1,
        [Display(Name = "Biweekly")]
        BiWeekly = 2,
        [Display(Name = "Monthly")]
        Monthly = 3,
        SpecificDates = 4
    }
    public enum ForcastingSettingLevel
    {
        Account = 1,
        Job = 2,
        Tank = 3
    }
    public enum ForcastingServicePreferance
    {
        Buyer = 1,
        Supplier = 2
    }
    public enum InventoyUOM
    {
        UOM = 1,
        Percentage = 2,
    }
    public enum RateOfConsumsionUOM
    {
        Hours = 1,
        Days = 2,

    }

    public enum MessageCodes
    {
        Start = 1,
        Reschedule = 2,
        Reassign = 3,
        Cancel = 4,
        NewScheduleAssign = 5,
        DriverArrived = 6,
        PickUpLocationChange = 7,
        SplitLoadAddressChange = 8,
        NewDeliveryGroupAssign = 9,
        DeliveryGroupModified = 10,
        DeliveryGroupDeleted = 11,
        SendBirdChat = 12,
        InvalidTankMakeModel = 13,
        DipValueGreaterThanMaxCapacity = 14,
        TankMakeModelRequired = 15,
        ScaleMeasurementRequired = 16,
        DipValueRequired = 17,
        DipValSmallerThanMinValOfDipChart = 18,
        DipValMoreThanMaxValOfDipChart = 19,
        FuelCapacityRequired = 20,
        DeliveryGroupMissed = 21,
        DeliveryGroupUnassigned = 22
    }

    public enum PDIApiResponse
    {
        Accepted = 0,
        AcceptedWithWarnings = 1,
        RejectedWithErrors = 2
    }
    public enum PDIDestinationType
    {
        CompanyOwnedLocation = 0,
        CustomerLocation = 1

    }
    public enum PDIOriginType
    {
        Terminal = 0,
        BulkPlant = 1

    }
    public enum ExternalThirdPartyCompanies
    {
        ThirdParty = 1,
        PDI = 2,
        Filld = 3
    }
    public enum PDIOrderStatus
    {
        Quote = 0,
        Open = 1,
        Picked = 2,
        Shipped = 3,
        Dispatched = 4,
        ReleasedForBilling = 5,
        Billed = 6,
        CancelledAsQuote = 7,
        CancelledAsOrder = 8,
        ExpiredQuote = 9,
        ReleasedForDispatch = 10,
        ReleasedForPicking = 12,
        AcceptedBySite = 13,
        RejectedBySite = 14,
        Delivered = 15,
        Pending = 16,
        Unknown = 99
    }
    public enum SelctedSalesTab
    {
        Priority = 1,
        Tanks,
        Location
    }
    public enum DsbLoadQueueStatus
    {
        New = 0,
        InProgress,
        Success,
        Failed
    }
    public enum DsbLoadQueueNotifyStatus
    {
        New = 0,
        Read
    }

    public enum TfxModule
    {
        None,
        SupplierWallyboardLocation,
        SupplierWallyboardLoads,
        SupplierWallyboardSales,
        SupplierWallyboardSalesPriority,
        SupplierWallyboardSalesTanks,
        BuyerWallyboardLocation,
        BuyerWallyboardLoads,
        BuyerWallyboardSales,
        BuyerWallyboardSalesPriority,
        BuyerWallyboardSalesTanks,
        BuyerWallyboardSalesLocation,
        AssignedByMeDeliveryRequests,
        DSBShift
    }

    public enum InventoryDataCaptureType
    {
        [Display(Name = "Not Specified")]
        NotSpecified = 0,
        [Display(Name = "Connected")]
        Connected = 1,
        [Display(Name = "Manual")]
        Manual = 2,
        [Display(Name = "Call-in")]
        CallIn = 3,
        [Display(Name = "Mixed")]
        Mixed = 4,
    }

    public enum SourcingRequestStatus
    {
        [Display(Name = "Submitted")]
        Submitted = 1,
        [Display(Name = "Modified")]
        Modified = 2,
        [Display(Name = "Accepted")]
        Accepted = 3,
        [Display(Name = "Accepted And Modified")]
        AcceptedAndModified = 4,
        [Display(Name = "PO Created")]
        OrderCreated = 5,
        [Display(Name = "Lost")]
        Lost = 6
    }

    public enum CarrierXDeliveryFailureRequestType
    {
        TPDAPI = 1,

    }

    public enum DestinedForInternationalWaters
    {
        [Display(Name = "Unknown")]
        Unknown = 0,
        [Display(Name = "Yes")]
        Yes = 1,
        [Display(Name = "No")]
        No = 2,
    }
    public enum FleetType
    {
        [Display(Name = "Less Than Truck Load (LTL)")]
        LessTruckLoad = 1,
        [Display(Name = "Full Truck Load (FTL)")]
        FullTruckLoad = 2,
        DEF = 3,
        [Display(Name = "Fuel Assets")]
        FuelAsset = 4
    }
    public enum ServiceOfferingType
    {
        FTL = 1,
        LTL = 2,
        [Display(Name = "LTL Wet Hosing")]
        LTLWetHosing = 3,
        DEF,
        [Display(Name = "DEF Wet Hosing")]
        DEFWetHosing
    }

    public enum ServiceAreaType
    {
        [Display(Name = "State Wide")]
        StateWide = 1,
        [Display(Name = "Zip Wide")]
        ZipWide = 2
    }
    public enum InvitedCompanyType
    {
        [Display(Name = "Carrier (Freight Only)")]
        Carrier = 4,
        [Display(Name = "Supplier (Fuel + Freight)")]
        Supplier = 2,
        [Display(Name = "Supplier/Carrier")]
        SupplierAndCarrier = 5
    }

    public enum FuelTrailerAssetType
    {
        [Display(Name = "FTL")]
        FTL = 1,
        [Display(Name = "LTL")]
        LTL = 2
    }

    public enum DefTrailerAssetType
    {
        [Display(Name = "Box Truck")]
        BoxTruck = 1,
        [Display(Name = "Tank Wagon")]
        TankWagon = 2,
        [Display(Name = "Stake Bed")]
        StakeBed = 3,
        [Display(Name = "Transport")]
        Transport = 4,
    }
    public enum SourcingRequestDisplayStatus
    {
        [Display(Name = "All")]
        All = 1,
        [Display(Name = "New")]
        New = 2,
        [Display(Name = "WIP")]
        WIP = 3,
        [Display(Name = "Sourced")]
        Sourced = 4,
        [Display(Name = "Lost")]
        Lost = 5,
    }

    public enum PaymentDueDateType
    {
        None = 0,
        InvoiceCreationDate = 1,
        DeliveryDate = 2
    }
    public enum EbolMatchStatus
    {
        NoMatch = 0,
        Match = 1,
        Override = 2
    }

    public enum RegionFavProductType
    {
        None = 0,
        ProductType = 1,
        FuelType = 2
    }

    /// <summary>
    /// Used for Jobs identification to be put on hold while continous/trigger jobs through App settings
    /// </summary>
    public enum JobsOnHold
    {
        None = 0,
        DeliveryAlerts = 1,
        Pricing = 2,
        Notifications = 3,
        WaitingForTax = 4,
        DSBQueue = 5,
        SAPOrder = 6,
        QuickBookWorkflow = 7,
        PDIApi = 8,
        FileUpload = 9,
        FailedAPIReprocess = 10
    }

    public enum EditPropertyType
    {
        CreateTPO,
        UpdateTPO,
        UpdateLocOnTheFly,
        UpdateFuelSurchargeFrieghtFee,
        UpdateFrieghtRate,
        UpdateAccessorialFees,
        PO,
        ProformaPO,
        OrderName,
        PaymentTerms
    }
}
