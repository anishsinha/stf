using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteFuel.BAL;
using SiteFuel.DAL;
using SiteFuel.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Models;
using SiteFuel.PricingService.Controllers;
using SiteFuel.PricingService.Tests.Helper;
using SiteFuel.PricingService.Tests.TestHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SiteFuel.PricingService.Tests.Controllers
{

    /// <summary>
    /// ONLY ONE TIME OBJECT INITILIZATION
    //An instance of the fixture data is initialized
    //just before the first test in the class is run, and if it implements IDisposable,
    //is disposed after the last test in the class is run.
    // 1. To avoid multiple initialization of required property. It is good to have required property / entity in sepearte class.
    ///</summary>
    public class EntityFixture : IDisposable
    {
        //Todo : Why each Theory initilize constructor 
        // according to xUnit we have implement IClassFixture. https://stackoverflow.com/questions/46926852/xunit-constructor-runs-before-each-test
        public  List<MstPricingCode> MstPricingCodeEntities;
        public  List<ExternalPricingAxxis> ExternalPricingAxxisEntities;
        public List<PricingDetail> PricingDetailEntities;
        public List<CumulationDetail> CumulationDetailsEntities;

        public List<MstPricingConfig> MstPricingConfigEntities;

        public List<ExternalPricingOpis> ExternalPricingOpisEntities;
        public List<ExternalPricingPlatts> ExternalPricingPlattsEntities;
        //public Mock<PricingRepository> MockPricingRepository;MstProducts

        public List<MstProduct> MstProductsEntities;
        public List<MstTfxProduct> MstTfxProductsEntities;

        public List<MstExternalTerminal> MstExternalTerminalsEntities;

        public List<RequestPriceDetail> RequestPriceDetailsEntities;
        public EntityFixture()
        {
            MstPricingCodeEntities = new List<MstPricingCode>
            {
                new MstPricingCode {
                    Id=1,
                    Name="pricing name1",
                    PricingSourceId=1,
                    FeedTypeId=2,
                    FuelClassTypeId=3,
                    IsActive=true,
                    MstPricingSource=new MstPricingSource{Id=11,IsActive=true,Name="mstpriceSourceName1" },
                    PricingTypeId=2,
                    QuantityIndicatorId=12,
                    RackTypeId=4,
                    WeekendPricingTypeId=6  },
                 new MstPricingCode {
                    Id=2,
                    Name="pricing name2",
                    PricingSourceId=2,
                    FeedTypeId=3,
                    FuelClassTypeId=4,
                    IsActive=true,
                    MstPricingSource=new MstPricingSource{Id=12,IsActive=true,Name="mstpriceSourceName2" },
                    PricingTypeId=3,
                    QuantityIndicatorId=14,
                    RackTypeId=40,
                    WeekendPricingTypeId=60},

                 new MstPricingCode {
                    Id=3,
                    Name="pricing name2",
                    PricingSourceId=3,
                    FeedTypeId=3,
                    FuelClassTypeId=4,
                    IsActive=true,
                    MstPricingSource=new MstPricingSource{Id=12,IsActive=true,Name="mstpriceSourceName2" },
                    PricingTypeId=3,
                    QuantityIndicatorId=14,
                    RackTypeId=40,
                    WeekendPricingTypeId=60},

                 new MstPricingCode {
                    Id=4,
                    Name="pricing name2",
                    PricingSourceId=4,
                    FeedTypeId=3,
                    FuelClassTypeId=4,
                    IsActive=true,
                    MstPricingSource=new MstPricingSource{Id=12,IsActive=true,Name="mstpriceSourceName2" },
                    PricingTypeId=3,
                    QuantityIndicatorId=14,
                    RackTypeId=40,
                    WeekendPricingTypeId=60},

            };

            ExternalPricingAxxisEntities = new List<ExternalPricingAxxis>
            {
                new ExternalPricingAxxis{
                AvgPrice=23.7m,
                Currency=1,
                EffectiveDate=DateTime.Today,
                HighPrice=99.12m,
                Id=1,
                LowPrice=55.43m,
                MstExternalProduct = new MstExternalProduct{ },
                MstExternalTerminal = new MstExternalTerminal{},
                ProductCode="5M",
                ProductId=2,
                TerminalAbbreviation="terminal abbreviation",
                TerminalId=1,
                UpdatedDate=DateTime.Today}


            };
            // initialize data in the test database

            PricingDetailEntities = new List<PricingDetail>
            {
                new PricingDetail
                {
                    BasePrice = 36.12m,
                    BaseSupplierCost=12,
                    CityRackTerminalId=5,
                    Id=1,
                    FuelTypeId=2,
                    IsActive=true,
                    Margin=3m,
                    MarginTypeId=1,
                    MaxQuantity=2,
                    MinQuantity=3.4m,
                    MstPricingCode = new MstPricingCode{ PricingSourceId =0,FeedTypeId=0,FuelClassTypeId=0,Id=0,
                        IsActive=true,Name="pricing name0",PricingTypeId=0,QuantityIndicatorId=0,RackTypeId=0,WeekendPricingTypeId=0,
                        MstPricingSource=new MstPricingSource{ Id=0,IsActive=true,Name="source name0",UpdatedBy=0,
                                                                UpdatedDate=DateTimeOffset.MinValue } },
                    PricePerGallon=3.5m,
                    PricingCodeId=1,
                    RackAvgTypeId=2,
                    RequestPriceDetailId=0, //scenaro 1
                    RequestPriceDetails = new RequestPriceDetail{ PricingTypeId=0  }, //default scenario
                    SupplierCost=3.5m,
                    SupplierCostTypeId=12,
                    TerminalId=1
                },
                 new PricingDetail
                {
                    BasePrice = 36.12m,
                    BaseSupplierCost=12,
                    CityRackTerminalId=5,
                    Id=1,
                    FuelTypeId=2,
                    IsActive=true,
                    Margin=3m,
                    MarginTypeId=1,
                    MaxQuantity=2,
                    MinQuantity=3.4m,
                    MstPricingCode = new MstPricingCode{ PricingSourceId =1,FeedTypeId=1,FuelClassTypeId=1,Id=1,
                        IsActive=true,Name="pricing name1",PricingTypeId=1,QuantityIndicatorId=1,RackTypeId=1,WeekendPricingTypeId=1,
                        MstPricingSource=new MstPricingSource{ Id=1,IsActive=true,Name="source name1",UpdatedBy=1,
                                                                UpdatedDate=DateTimeOffset.MinValue } },
                    PricePerGallon=3.5m,
                    PricingCodeId=1,
                    RackAvgTypeId=2,
                    RequestPriceDetailId=2,
                    RequestPriceDetails = new RequestPriceDetail{ PricingTypeId=2},
                    SupplierCost=3.5m,
                    SupplierCostTypeId=12,
                    TerminalId=1
                },
                 new PricingDetail
                {
                    BasePrice = 36.12m,
                    BaseSupplierCost=12,
                    CityRackTerminalId=5,
                    Id=1,
                    FuelTypeId=2,
                    IsActive=true,
                    Margin=3m,
                    MarginTypeId=1,
                    MaxQuantity=0,
                    MinQuantity=3.4m,
                    MstPricingCode =  new MstPricingCode{ PricingSourceId =2,FeedTypeId=2,FuelClassTypeId=2,Id=2,
                        IsActive=true,Name="pricing name2",PricingTypeId=2,QuantityIndicatorId=2,RackTypeId=2,WeekendPricingTypeId=2,
                        MstPricingSource=new MstPricingSource{ Id=2,IsActive=true,Name="source name2",UpdatedBy=2,
                                                                UpdatedDate=DateTimeOffset.MinValue } },
                    PricePerGallon=3.5m,
                    PricingCodeId=1,
                    RackAvgTypeId=2,
                    RequestPriceDetailId=3,
                    RequestPriceDetails = new RequestPriceDetail{ PricingTypeId=3,TierTypeId=1},
                    SupplierCost=3.5m,
                    SupplierCostTypeId=12,
                    TerminalId=1
                },
                  new PricingDetail
                {
                    BasePrice = 36.12m,
                    BaseSupplierCost=12,
                    CityRackTerminalId=5,
                    Id=1,
                    FuelTypeId=2,
                    IsActive=true,
                    Margin=3m,
                    MarginTypeId=1,
                    MaxQuantity=0,
                    MinQuantity=3.4m,
                    MstPricingCode= new MstPricingCode { PricingSourceId =3,FeedTypeId=3,FuelClassTypeId=2,Id=3,
                        IsActive=true,Name="pricing name3",PricingTypeId=3,QuantityIndicatorId=3,RackTypeId=3,WeekendPricingTypeId=3,
                        MstPricingSource=new MstPricingSource{ Id=3,IsActive=true,Name="source name3",UpdatedBy=3,
                                                                UpdatedDate=DateTimeOffset.MinValue } },
                    PricePerGallon=3.5m,
                    PricingCodeId=1,
                    RackAvgTypeId=2,
                    RequestPriceDetailId=3,
                    RequestPriceDetails = new RequestPriceDetail{ PricingTypeId=3,TierTypeId=2},
                    SupplierCost=3.5m,
                    SupplierCostTypeId=12,
                    TerminalId=1
                },
                   new PricingDetail
                {
                    BasePrice = 36.12m,
                    BaseSupplierCost=12,
                    CityRackTerminalId=5,
                    Id=1,
                    FuelTypeId=2,
                    IsActive=true,
                    Margin=3m,
                    MarginTypeId=1,
                    MaxQuantity=0,
                    MinQuantity=3.4m,
                    MstPricingCode = new MstPricingCode { PricingSourceId =4,FeedTypeId=4,FuelClassTypeId=4,Id=4,
                        IsActive=true,Name="pricing name4",PricingTypeId=4,QuantityIndicatorId=4,RackTypeId=4,WeekendPricingTypeId=4,
                        MstPricingSource=new MstPricingSource{ Id=4,IsActive=true,Name="source name4",UpdatedBy=4,
                                                                UpdatedDate=DateTimeOffset.MinValue } },
                    PricePerGallon=3.5m,
                    PricingCodeId=1,
                    RackAvgTypeId=2,
                    RequestPriceDetailId=3,
                    RequestPriceDetails = new RequestPriceDetail{ PricingTypeId=3,TierTypeId=0},
                    SupplierCost=3.5m,
                    SupplierCostTypeId=12,
                    TerminalId=1
                },
                  new PricingDetail
                {
                    BasePrice = 36.12m,
                    BaseSupplierCost=12,
                    CityRackTerminalId=5,
                    Id=1,
                    FuelTypeId=2,
                    IsActive=true,
                    Margin=3m,
                    MarginTypeId=1,
                    MaxQuantity=2,
                    MinQuantity=3.4m,
                    MstPricingCode = new MstPricingCode { PricingSourceId =5,FeedTypeId=5,FuelClassTypeId=5,Id=5,
                        IsActive=true,Name="pricing name5",PricingTypeId=5,QuantityIndicatorId=5,RackTypeId=5,WeekendPricingTypeId=5,
                        MstPricingSource=new MstPricingSource{ Id=5,IsActive=true,Name="source name5",UpdatedBy=5,
                                                                UpdatedDate=DateTimeOffset.MinValue } },
                    PricePerGallon=3.5m,
                    PricingCodeId=1,
                    RackAvgTypeId=2,
                    RequestPriceDetailId=4,
                    RequestPriceDetails = new RequestPriceDetail{ PricingTypeId=4},
                    SupplierCost=3.5m,
                    SupplierCostTypeId=12,
                    TerminalId=1
                }
            };

          

            CumulationDetailsEntities = new List<CumulationDetail> { 
                new CumulationDetail{
                    Id=1,
                    CumulatedQuantity=4.55m,
                    IsActive=true,
                    RequestPriceDetailId=0,
                    EndDate=DateTime.Today
                },
                  new CumulationDetail{
                      Id=2,
                    CumulatedQuantity=14.53m,
                    IsActive=true,
                    RequestPriceDetailId=2,
                    EndDate=DateTime.Today
                },
                  new CumulationDetail{
                      Id=3,
                    CumulatedQuantity=94.51m,
                    IsActive=true,
                    RequestPriceDetailId=3,
                    EndDate=DateTime.Today
                },
                  
                  new CumulationDetail{
                      Id=4,
                    CumulatedQuantity=65.25m,
                    IsActive=true,
                    RequestPriceDetailId=4,
                    EndDate=DateTime.Today
                }

            };

            RequestPriceDetailsEntities = new List<RequestPriceDetail> {
            new RequestPriceDetail{ Id=0,CumulationTypeId=1, PricingTypeId=0,CumulationDetails =CumulationDetailsEntities },
             new RequestPriceDetail{ Id=2,CumulationTypeId=2, PricingTypeId=0,CumulationDetails =CumulationDetailsEntities },
             new RequestPriceDetail{ Id=3,CumulationTypeId=3, PricingTypeId=0,CumulationDetails =CumulationDetailsEntities },
             new RequestPriceDetail{ Id=4,CumulationTypeId=4, PricingTypeId=0,CumulationDetails =CumulationDetailsEntities }
            };

            MstPricingConfigEntities = new List<MstPricingConfig> {
            new MstPricingConfig{ 
                Description ="Pricing config description",Id=1,IsActive=true,Key="PricingDataSourcesUpdatedDate",UpdatedBy=1,UpdatedDate=DateTimeOffset.MinValue,Value=DateTime.Today.ToString() },
            new MstPricingConfig{
                Description ="Pricing config description1",Id=2,IsActive=true,Key="PricingDataLastUpdatedDate1",UpdatedBy=2,UpdatedDate=DateTimeOffset.MinValue,Value="price config value2" }
            };

            ExternalPricingOpisEntities = new List<ExternalPricingOpis>()
            {
                new ExternalPricingOpis
                {
                      Id= 0,
                      TerminalId= 2,
                      TerminalAbbreviation= null,
                      ProductId= 2,
                      Symbol= null,
                      LoadDate= DateTimeOffset.Parse("0001-01-01T00:00:00+00:00"),
                      FeedTypeId= 0,
                      ReportedDate= DateTimeOffset.Parse("0001-01-01T00:00:00+00:00"),
                      Price= 0.0m,
                      Unit= 0,
                      Currency= 0,
                      SupplierNumber= 0,
                      Supplier= null,
                      RackTypeId= 0,
                      SupplierBrand= null,
                      SupplierBrandId= 0,
                      PriceType= null,
                      PriceTypeId= 0,
                      LiftPoint= null,
                      ExternalProductId= 0,
                      SourceId= 0,
                      IsActualOPIS= false,
                      MstExternalTerminal=new MstExternalTerminal {},
                      MstProduct = new MstProduct {}
                     
                }

            };

            ExternalPricingPlattsEntities = new List<ExternalPricingPlatts>()
            {
                new ExternalPricingPlatts
                {
                    Id= 0,
                    TerminalId= 3,
                    TerminalAbbreviation= null,
                    ProductId= 3,
                    Symbol= null,
                    LoadDate= DateTimeOffset.Parse("0001-01-01T00:00:00+00:00"),
                    ReportedDate= DateTimeOffset.Parse("0001-01-01T00:00:00+00:00"),
                    Price= 0.0m,
                    Unit= 0,
                    Currency= 0,
                    SupplierNumber= 0,
                    Supplier= null,
                    LiftPoint= null,
                    ExternalProductId= 0,
                    SourceId= 0,
                    MstExternalTerminal = new MstExternalTerminal{ },
                    MstProduct = new MstProduct {}
                }
            };

            MstProductsEntities = new List<MstProduct>()
            {
                new MstProduct
                {
                    Id =1,
                    DisplayName="pName1",
                    IsActive=true,
                    MappedParentId=1,
                    MstPricingSource= new MstPricingSource{},
                    MstProductMappings = new List<MstProductMapping>(),
                    MstProductType=new MstProductType{ },
                    MstTfxProduct=new MstTfxProduct{ },
                    Name = "pName",
                    PricingSourceId=1,
                    ProductCode="2PC",
                    ProductDisplayGroupId=12,
                    ProductTypeId=3,
                    TfxProductId=2,
                    UpdatedBy=1,
                    UpdatedDate=DateTime.Today
                },
                 new MstProduct
                {
                    Id =2,
                    DisplayName="pName1",
                    IsActive=true,
                    MappedParentId=1,
                    MstPricingSource= new MstPricingSource{},
                    MstProductMappings = new List<MstProductMapping>(),
                    MstProductType=new MstProductType{ },
                    MstTfxProduct=new MstTfxProduct{ },
                    Name = "pName",
                    PricingSourceId=2,
                    ProductCode="2PC",
                    ProductDisplayGroupId=12,
                    ProductTypeId=3,
                    TfxProductId=2,
                    UpdatedBy=1,
                    UpdatedDate=DateTime.Today
                },
                  new MstProduct
                {
                    Id =3,
                    DisplayName="pName1",
                    IsActive=true,
                    MappedParentId=1,
                    MstPricingSource= new MstPricingSource{},
                    MstProductMappings = new List<MstProductMapping>(),
                    MstProductType=new MstProductType{ },
                    MstTfxProduct=new MstTfxProduct{ },
                    Name = "pName",
                    PricingSourceId=3,
                    ProductCode="2PC",
                    ProductDisplayGroupId=12,
                    ProductTypeId=3,
                    TfxProductId=2,
                    UpdatedBy=1,
                    UpdatedDate=DateTime.Today
                },
                   new MstProduct
                {
                    Id =4,
                    DisplayName="pName1",
                    IsActive=true,
                    MappedParentId=1,
                    MstPricingSource= new MstPricingSource{},
                    MstProductMappings = new List<MstProductMapping>(),
                    MstProductType=new MstProductType{ },
                    MstTfxProduct=new MstTfxProduct{ },
                    Name = "pName",
                    PricingSourceId=4,
                    ProductCode="2PC",
                    ProductDisplayGroupId=12,
                    ProductTypeId=3,
                    TfxProductId=2,
                    UpdatedBy=1,
                    UpdatedDate=DateTime.Today
                }


            };

            MstTfxProductsEntities = new List<MstTfxProduct>()
            {
                new MstTfxProduct
                {
                    Id =1,
                    IsActive=true,
                    MstProductType=new MstProductType{ },
                    Name = "pName",
                    ProductCode="2PC",
                    ProductDisplayGroupId=12,
                    ProductTypeId=3,
                    UpdatedBy=1,
                    UpdatedDate=DateTime.Today,
                    MstProducts = MstProductsEntities,
                    
                    
                }
            };

            MstExternalTerminalsEntities = new List<MstExternalTerminal>()
            {
                new MstExternalTerminal { ControlNumber= "C1",IsActive=true,Id=1,Name="Ext Terminal1"},
                new MstExternalTerminal { ControlNumber= "C2",IsActive=true,Id=2,Name="Ext Terminal2"},
                new MstExternalTerminal { ControlNumber= "C3",IsActive=true,Id=3,Name="Ext Terminal3"},
                 new MstExternalTerminal { ControlNumber= "C4",IsActive=false,Id=4,Name="Ext Terminal4"}
            };
        }
        public void Dispose()
        {
            // clean up test data from the database
            MstPricingCodeEntities = null;
            ExternalPricingAxxisEntities = null;
            PricingDetailEntities = null;
            CumulationDetailsEntities = null;

        }
    }

    /// <summary>
    /// This class having all test method of pricing controller
    /// </summary>
    public class PricingControllerTest : IClassFixture<EntityFixture>
    {

        readonly EntityFixture _entity;

        public PricingControllerTest(EntityFixture entity)
        {
            _entity = entity;
        }

        /// <summary>
        /// JsonFileData : you can control input and output of any method and define scenario. 
        /// How to identify scenario ? 
        /// Answer : need to deep drive respective method i.e GetTerminalPriceAsync and 
        /// carfully understand the behaviour of function.Example :Based on request , 
        /// PricingCodeId that is 1(exPricingSource.Axxis),2(PricingSource.OPIS),
        /// 3(PricingSource.PLATTS) and defult.method call different stored procedure.
        /// so need to capture all scenario while changing request field i.e PricingCodeId
        /// </summary>
        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "ReqAndRespGetTerminalPrice")]
        public async void Test_Service_GetTerminalPriceAsync(PriceRequestModel request,
            PricingResponseModel expectedPricingResponseModel)
        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mockDbSet = DbContextMock.GetQueryableMockDbSet<MstPricingCode>(_entity.MstPricingCodeEntities);

            var mDbContext = new Mock<DataContext>();
            mDbContext.Setup(c => c.MstPricingCodes).Returns(mockDbSet);

            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<PricingResponseModel>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(expectedPricingResponseModel);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            #endregion

            #region Act : perform the actual work of the test

            expectedPricingResponseModel = await pricingController.GetTerminalPriceAsync(request);

            #endregion

            #region Assert start – verify the result

            switch (request.PricingCodeId)
            {
                case 1:
                    Assert.True(request.TerminalId.HasValue && string.IsNullOrEmpty(expectedPricingResponseModel.Message),
                      "if TerminalId is null then message should be 'TerminalId field is required'.");
                    Assert.True(expectedPricingResponseModel.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=1 status should be Success");
                    break;
                case 2:
                    Assert.True(expectedPricingResponseModel.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=2 status should be Success");
                    break;
                case 3:
                    Assert.True(expectedPricingResponseModel.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=3 status should be Success");
                    break;
                case 4:
                    Assert.True(expectedPricingResponseModel.Status.ToString().Equals("Failed", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=4 status should be Success");



                    break;
                default:
                    break;
            }

            if (request.PricingCodeId == 1)
            {

            }

            #endregion
        }

        [Theory]
        [InlineData(int.MaxValue, "2017-3-1", int.MinValue, int.MaxValue)]
        [InlineData(int.MinValue, "2021-3-3", int.MaxValue, int.MinValue)]
        [InlineData(123, "2021-3-3", 456, 789)]
        public void Test_Request_Property(int pcId, string pdate, int pId, int tId)
        {

            //it will increase code coverage % since we are setting and getting each individual property.
            var request = new PriceRequestModel()
            {

                PricingCodeId = pcId,
                PriceDate = DateTime.Parse(pdate),
                ProductId = pId,
                TerminalId = tId

            };

            Assert.Equal(DateTime.Parse(pdate), request.PriceDate);
            Assert.Equal(pcId, request.PricingCodeId);
            Assert.Equal(pId, request.ProductId);
            Assert.Equal(tId, request.TerminalId);
        }

        [Fact]
        public void Property_name_exist_in_request_response()
        {
            PriceRequestModel request = new PriceRequestModel();
            PricingResponseModel response = new PricingResponseModel();

            Assert.True(HasProperty(request, "PriceDate"), "Property type or name has been changed");
            Assert.True(HasProperty(request, "PricingCodeId"), "Property type or name has been changed");
            Assert.True(HasProperty(request, "ProductId"), "Property type or name has been changed");

            Assert.True(HasProperty(response, "Status"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "EffectiveDate"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "PriceLastUpdatedDate"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "Price"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "AvgPrice"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "LowPrice"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "Message"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "HighPrice"), "Property type or name has been changed");
            Assert.True(HasProperty(response, "PricingTypeId"), "Property type or name has been changed");

            Assert.False(HasProperty(response, "PricingTypeIddd"), "Property type or name has been changed");

        }

        [Fact]
        public void Test_Response_Property()
        {

            //it will increase code coverage % since we are setting and getting each individual property.

            var response = new PricingResponseModel()
            {
                Status = 0,
                EffectiveDate = DateTime.Today,
                PriceLastUpdatedDate = DateTime.Today,
                Price = decimal.MaxValue,
                AvgPrice = 7.8m,
                LowPrice = 9.8m,
                Message = "Have a nice day!",
                HighPrice = 11.5m,
                PricingTypeId = 4
            };
            Assert.Equal("Success", response.Status.ToString());
            Assert.Equal(DateTime.Today, response.EffectiveDate);
            Assert.Equal(DateTime.Today, response.PriceLastUpdatedDate);
            Assert.Equal(decimal.MaxValue, response.Price);
            Assert.Equal(7.8m, response.AvgPrice);
            Assert.Equal(9.8m, response.LowPrice);
            Assert.Equal("Have a nice day!", response.Message);
            Assert.Equal(11.5m, response.HighPrice);
            Assert.Equal(4, response.PricingTypeId);

            Assert.False("Successssss" == response.Status.ToString());
            Assert.False(DateTime.Today.AddDays(1) == response.EffectiveDate);
            Assert.False(DateTime.Today.AddDays(2) == response.PriceLastUpdatedDate);
            Assert.False(4m == response.Price);
            Assert.False(7.9m == response.AvgPrice);
            Assert.False(9.9m == response.LowPrice);
            Assert.False("Have a nice day!!!!" == response.Message);
            Assert.False(11.6m == response.HighPrice);
            Assert.False(5 == response.PricingTypeId);


            Assert.True("Success" == response.Status.ToString());
            Assert.True(DateTime.Today == response.EffectiveDate);
            Assert.True(DateTime.Today == response.PriceLastUpdatedDate);
            Assert.True(decimal.MaxValue == response.Price);
            Assert.True(7.8m == response.AvgPrice);
            Assert.True(9.8m == response.LowPrice);
            Assert.True("Have a nice day!" == response.Message);
            Assert.True(11.5m == response.HighPrice);
            Assert.True(4 == response.PricingTypeId);
        }

        public bool HasProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 2)]
        public async void Test_Services_GetTerminalPriceAsync(int productId, int terminalId)
        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mockDbSet = DbContextMock.GetQueryableMockDbSet<ExternalPricingAxxis>(_entity.ExternalPricingAxxisEntities);

            var mDbContext = new Mock<DataContext>();
            mDbContext.Setup(c => c.ExternalPricingAxxis).Returns(mockDbSet);
            var response = new PricingResponseModel();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<PricingResponseModel>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(response);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            #endregion

            #region Act : perform the actual work of the test

            response = await pricingController.GetTerminalPriceAsync(productId, terminalId);

            #endregion

            #region Assert start – verify the result
            if (productId == 2)
            {

                Assert.True(response.Currency == 1, "Currency should be 1.");
                Assert.True(response.Status.ToString().Equals("Success"), "Status should be Success.");
            }
            else
            {
                Assert.True(response.Status.ToString().Equals("Failed"), "Status should be Failed.");
            }



            #endregion

        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "lstFuelPriceRequestModel")]
        public async void Test_Service_GetFuelPriceAsync(List<FuelPriceRequestModel> lstRequestModel,
            List<FuelPricingResponseModel> lstResponseModel)
        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            // GetFuelPriceAsync require 3 entites , so it has been mocked.
            var mockPricingDetail = DbContextMock.GetQueryableMockDbSet<PricingDetail>(_entity.PricingDetailEntities);
            var mockMstPricingCodes = DbContextMock.GetQueryableMockDbSet<MstPricingCode>(_entity.MstPricingCodeEntities);
            var mockCumulationDetails = DbContextMock.GetQueryableMockDbSet<CumulationDetail>(_entity.CumulationDetailsEntities);


            var mDbContext = new Mock<DataContext>();

            mDbContext.Setup(c => c.PricingDetails).Returns(mockPricingDetail);
            mDbContext.Setup(c => c.MstPricingCodes).Returns(mockMstPricingCodes);
            mDbContext.Setup(c => c.CumulationDetails).Returns(mockCumulationDetails);

            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            #endregion

            #region Act : perform the actual work of the test

            lstResponseModel = await pricingController.GetFuelPriceAsync(lstRequestModel);

            #endregion

            #region Assert start – verify the result


            DateTimeOffset dt = new DateTimeOffset();
            Assert.True(lstResponseModel.Count == 4);
            Assert.True(lstResponseModel[0].FuelCost == null, "FuelCost value is not correct");
            Assert.True(lstResponseModel[0].FuelCostTypeId == null, "FuelCostTypeId value is not correct");
            Assert.True(lstResponseModel[0].Message == null, "Message value is not correct");
            Assert.True(lstResponseModel[0].PriceLastUpdatedDate == dt, "PriceLastUpdatedDate value is not correct");
            Assert.True(lstResponseModel[0].PricePerGallon == -3.5m, "PricePerGallon value is not correct");
            Assert.True(lstResponseModel[0].PricingDate == dt, "PricingDate value is not correct");
            Assert.True(lstResponseModel[0].Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status value is not correct");
            Assert.True(lstResponseModel[0].TerminalPrice == 0, "TerminalPrice value is not correct");
            Assert.True(lstResponseModel[0].TierPricingDetails.CityGroupTerminalId == null, "CityGroupTerminalId value is not correct");
            Assert.True(lstResponseModel[0].TierPricingDetails.MaxQuantity == null, "MaxQuantity value is not correct");
            Assert.True(lstResponseModel[0].TierPricingDetails.MinQuantity == null, "MinQuantity value is not correct");
            Assert.True(lstResponseModel[0].TierPricingDetails.TerminalId == null, "TerminalId value is not correct");

            Assert.True(lstResponseModel[1].FuelCost == null, "FuelCost value is not correct");
            Assert.True(lstResponseModel[1].FuelCostTypeId == null, "FuelCostTypeId value is not correct");
            Assert.True(lstResponseModel[1].Message == null, "Message value is not correct");
            Assert.True(lstResponseModel[1].PriceLastUpdatedDate == dt, "PriceLastUpdatedDate value is not correct");
            Assert.True(lstResponseModel[1].PricePerGallon == 3.5m, "PricePerGallon value is not correct");
            Assert.True(lstResponseModel[1].PricingDate == dt, "PricingDate value is not correct");
            Assert.True(lstResponseModel[1].Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status value is not correct");
            Assert.True(lstResponseModel[1].TerminalPrice == 0, "TerminalPrice value is not correct");
            Assert.True(lstResponseModel[1].TierPricingDetails.CityGroupTerminalId == null, "CityGroupTerminalId value is not correct");
            Assert.True(lstResponseModel[1].TierPricingDetails.MaxQuantity == null, "MaxQuantity value is not correct");
            Assert.True(lstResponseModel[1].TierPricingDetails.MinQuantity == null, "MinQuantity value is not correct");
            Assert.True(lstResponseModel[1].TierPricingDetails.TerminalId == null, "TerminalId value is not correct");


            Assert.True(lstResponseModel[2].FuelCost == null, "FuelCost value is not correct");
            Assert.True(lstResponseModel[2].FuelCostTypeId == null, "FuelCostTypeId value is not correct");
            Assert.True(lstResponseModel[2].Message == null, "Message value is not correct");
            Assert.True(lstResponseModel[2].PriceLastUpdatedDate == dt, "PriceLastUpdatedDate value is not correct");
            Assert.True(lstResponseModel[2].PricePerGallon == 3.5m, "PricePerGallon value is not correct");
            Assert.True(lstResponseModel[2].PricingDate == dt, "PricingDate value is not correct");
            Assert.True(lstResponseModel[2].Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status value is not correct");
            Assert.True(lstResponseModel[2].TerminalPrice == 0, "TerminalPrice value is not correct");
            Assert.True(lstResponseModel[2].TierPricingDetails.CityGroupTerminalId == 5, "CityGroupTerminalId value is not correct");
            Assert.True(lstResponseModel[2].TierPricingDetails.MaxQuantity == 0, "MaxQuantity value is not correct");
            Assert.True(lstResponseModel[2].TierPricingDetails.MinQuantity == 94.51m, "MinQuantity value is not correct");
            Assert.True(lstResponseModel[2].TierPricingDetails.TerminalId == 1, "TerminalId value is not correct");


            Assert.True(lstResponseModel[3].FuelCost == 3.5m, "FuelCost value is not correct");
            Assert.True(lstResponseModel[3].FuelCostTypeId == 12, "FuelCostTypeId value is not correct");
            Assert.True(lstResponseModel[3].Message == null, "Message value is not correct");
            Assert.True(lstResponseModel[3].PriceLastUpdatedDate == dt, "PriceLastUpdatedDate value is not correct");
            Assert.True(lstResponseModel[3].PricePerGallon == 10.0m, "PricePerGallon value is not correct");
            Assert.True(lstResponseModel[3].PricingDate == dt, "PricingDate value is not correct");
            Assert.True(lstResponseModel[3].Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status value is not correct");
            Assert.True(lstResponseModel[3].TerminalPrice == 0, "TerminalPrice value is not correct");
            Assert.True(lstResponseModel[3].TierPricingDetails.CityGroupTerminalId == null, "CityGroupTerminalId value is not correct");
            Assert.True(lstResponseModel[3].TierPricingDetails.MaxQuantity == null, "MaxQuantity value is not correct");
            Assert.True(lstResponseModel[3].TierPricingDetails.MinQuantity == null, "MinQuantity value is not correct");
            Assert.True(lstResponseModel[3].TierPricingDetails.TerminalId == null, "TerminalId value is not correct");



            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "SalesCalculatorRequestModel")]
        public async void Test_Service_GetTerminalPricesForSalesCalculatorAsync(SalesCalculatorRequestModel inputModel)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.
            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            List<PricingData> storedProcReturns = new List<PricingData> {
                                            new PricingData()
                                                 {
                                                    TerminalId= 0,
                                                    Abbreviation = null,
                                                     Name = null,
                                                     City = null,
                                                     ZipCode = null,
                                                     StateCode = null,
                                                     PriceAvg = 0.0m,
                                                     Distance = 0.0d,
                                                     PricingDate =  DateTime.Today ,
                                                     TaxValue = 0.0m,
                                                     PriceLow = 0.0m,
                                                     PriceHigh = 0.0m,
                                                     Currency = 0,
                                                     Price = 0.0m,
                                                     PriceLoadDate = null,
                                                     ReportedDate = null,
                                                     TotalCount = 0,
                                                     RackTypeName = null,
                                                     FeedTypeName = null,
                                                     FuelClassTypeName = null,
                                                     PriceTypeName = null,
                                                     CurrencyName = null,
                                                     Amount = null,
                                                     Address = null
                                                 }
                                    };


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<PricingData>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            #endregion

            #region Act : perform the actual work of the test
            SalesCalculatorResponseModel response;
            response = await pricingController.GetTerminalPricesForSalesCalculatorAsync(inputModel);

            #endregion

            #region Assert start – verify the result

            //Axxis = 1,OPIS = 2,PLATTS = 3
            if (new[] { 1, 2, 3 }.Contains(inputModel.PricingSourceId) &&
                    response.Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase))
            {
                Assert.True(response.Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status should be Success");
                Assert.Null(response.Message);
                Assert.True(response.TerminalPrices[0].Abbreviation == storedProcReturns[0].Abbreviation, "Abbreviation must be equal");
                Assert.True(response.TerminalPrices[0].Address == storedProcReturns[0].Address, "Address must be equal");
                Assert.True(response.TerminalPrices[0].TerminalId == storedProcReturns[0].TerminalId, "TerminalId must be equal");
                Assert.True(response.TerminalPrices[0].Name == storedProcReturns[0].Name, "Name must be equal");
                Assert.True(response.TerminalPrices[0].City == storedProcReturns[0].City, "City must be equal");
                Assert.True(response.TerminalPrices[0].ZipCode == storedProcReturns[0].ZipCode, "ZipCode must be equal");
                Assert.True(response.TerminalPrices[0].StateCode == storedProcReturns[0].StateCode, "StateCode must be equal");
                Assert.True(response.TerminalPrices[0].PriceAvg == storedProcReturns[0].PriceAvg, "PriceAvg must be equal");
                Assert.True(response.TerminalPrices[0].Distance == storedProcReturns[0].Distance, "Distance must be equal");
                Assert.True(response.TerminalPrices[0].PricingDate == storedProcReturns[0].PricingDate, "PricingDate must be equal");
                Assert.True(response.TerminalPrices[0].TaxValue == storedProcReturns[0].TaxValue, "TaxValue must be equal");

                Assert.True(response.TerminalPrices[0].PriceLow == storedProcReturns[0].PriceLow, "PriceLow must be equal");
                Assert.True(response.TerminalPrices[0].PriceHigh == storedProcReturns[0].PriceHigh, "PriceHigh must be equal");
                Assert.True(response.TerminalPrices[0].Currency == storedProcReturns[0].Currency, "Currency must be equal");
                Assert.True(response.TerminalPrices[0].Price == storedProcReturns[0].Price, "Price must be equal");
                Assert.True(response.TerminalPrices[0].PriceLoadDate == storedProcReturns[0].PriceLoadDate, "PriceLoadDate must be equal");
                Assert.True(response.TerminalPrices[0].ReportedDate == storedProcReturns[0].ReportedDate, "ReportedDate must be equal");
                Assert.True(response.TerminalPrices[0].TotalCount == storedProcReturns[0].TotalCount, "TotalCount must be equal");
                Assert.True(response.TerminalPrices[0].RackTypeName == storedProcReturns[0].RackTypeName, "RackTypeName must be equal");
                Assert.True(response.TerminalPrices[0].FeedTypeName == storedProcReturns[0].FeedTypeName, "FeedTypeName must be equal");
                Assert.True(response.TerminalPrices[0].FuelClassTypeName == storedProcReturns[0].FuelClassTypeName, " FuelClassTypeName must be equal");
                Assert.True(response.TerminalPrices[0].PriceTypeName == storedProcReturns[0].PriceTypeName, "PriceTypeName must be equal");
                Assert.True(response.TerminalPrices[0].CurrencyName == storedProcReturns[0].CurrencyName, "CurrencyName must be equal");
                Assert.True(response.TerminalPrices[0].Amount == storedProcReturns[0].Amount, "Amount must be equal");
                Assert.True(response.TerminalPrices[0].Address == storedProcReturns[0].Address, "Address must be equal");
            }
            else
            {
                Assert.Equal("Failed", response.Status.ToString());
                Assert.Null(response.TerminalPrices);
                if (inputModel.PricingSourceId == 1)
                {
                    Assert.True(response.Message.ToString().Equals(@"ProductId field is required;SrcLatitude field is required;SrcLongitude is Required;RecordCount is required;CountryCode is required", StringComparison.CurrentCultureIgnoreCase), "Message should be equal");

                }

            }

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "CityRackPricesRequestModel")]
        public async void Test_Service_GetCityRackTerminalPricesAsync(CityRackPricesRequestModel inputModel)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            List<PricingData> storedProcReturns = new List<PricingData> {
                                            new PricingData()
                                                 {
                                                    TerminalId= 0,
                                                    Abbreviation = null,
                                                     Name = null,
                                                     City = null,
                                                     ZipCode = null,
                                                     StateCode = null,
                                                     PriceAvg = 0.0m,
                                                     Distance = 0.0d,
                                                     PricingDate =  DateTime.Today ,
                                                     TaxValue = 0.0m,
                                                     PriceLow = 0.0m,
                                                     PriceHigh = 0.0m,
                                                     Currency = 0,
                                                     Price = 0.0m,
                                                     PriceLoadDate = null,
                                                     ReportedDate = null,
                                                     TotalCount = 0,
                                                     RackTypeName = null,
                                                     FeedTypeName = null,
                                                     FuelClassTypeName = null,
                                                     PriceTypeName = null,
                                                     CurrencyName = null,
                                                     Amount = null,
                                                     Address = null
                                                 }
                                    };


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<PricingData>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            #endregion

            #region Act : perform the actual work of the test

            SalesCalculatorResponseModel response;
            response = await pricingController.GetCityRackTerminalPricesAsync(inputModel);

            #endregion

            #region Assert start – verify the result


            Assert.True(response.Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status should be Success");
            Assert.Null(response.Message);
            Assert.True(response.TerminalPrices[0].Abbreviation == storedProcReturns[0].Abbreviation, "Abbreviation must be equal");
            Assert.True(response.TerminalPrices[0].Address == storedProcReturns[0].Address, "Address must be equal");
            Assert.True(response.TerminalPrices[0].TerminalId == storedProcReturns[0].TerminalId, "TerminalId must be equal");
            Assert.True(response.TerminalPrices[0].Name == storedProcReturns[0].Name, "Name must be equal");
            Assert.True(response.TerminalPrices[0].City == storedProcReturns[0].City, "City must be equal");
            Assert.True(response.TerminalPrices[0].ZipCode == storedProcReturns[0].ZipCode, "ZipCode must be equal");
            Assert.True(response.TerminalPrices[0].StateCode == storedProcReturns[0].StateCode, "StateCode must be equal");
            Assert.True(response.TerminalPrices[0].PriceAvg == storedProcReturns[0].PriceAvg, "PriceAvg must be equal");
            Assert.True(response.TerminalPrices[0].Distance == storedProcReturns[0].Distance, "Distance must be equal");
            Assert.True(response.TerminalPrices[0].PricingDate == storedProcReturns[0].PricingDate, "PricingDate must be equal");
            Assert.True(response.TerminalPrices[0].TaxValue == storedProcReturns[0].TaxValue, "TaxValue must be equal");

            Assert.True(response.TerminalPrices[0].PriceLow == storedProcReturns[0].PriceLow, "PriceLow must be equal");
            Assert.True(response.TerminalPrices[0].PriceHigh == storedProcReturns[0].PriceHigh, "PriceHigh must be equal");
            Assert.True(response.TerminalPrices[0].Currency == storedProcReturns[0].Currency, "Currency must be equal");
            Assert.True(response.TerminalPrices[0].Price == storedProcReturns[0].Price, "Price must be equal");
            Assert.True(response.TerminalPrices[0].PriceLoadDate == storedProcReturns[0].PriceLoadDate, "PriceLoadDate must be equal");
            Assert.True(response.TerminalPrices[0].ReportedDate == storedProcReturns[0].ReportedDate, "ReportedDate must be equal");
            Assert.True(response.TerminalPrices[0].TotalCount == storedProcReturns[0].TotalCount, "TotalCount must be equal");
            Assert.True(response.TerminalPrices[0].RackTypeName == storedProcReturns[0].RackTypeName, "RackTypeName must be equal");
            Assert.True(response.TerminalPrices[0].FeedTypeName == storedProcReturns[0].FeedTypeName, "FeedTypeName must be equal");
            Assert.True(response.TerminalPrices[0].FuelClassTypeName == storedProcReturns[0].FuelClassTypeName, " FuelClassTypeName must be equal");
            Assert.True(response.TerminalPrices[0].PriceTypeName == storedProcReturns[0].PriceTypeName, "PriceTypeName must be equal");
            Assert.True(response.TerminalPrices[0].CurrencyName == storedProcReturns[0].CurrencyName, "CurrencyName must be equal");
            Assert.True(response.TerminalPrices[0].Amount == storedProcReturns[0].Amount, "Amount must be equal");
            Assert.True(response.TerminalPrices[0].Address == storedProcReturns[0].Address, "Address must be equal");




            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "TerminalPricesRequestModel")]
        public async void Test_Service_GetTerminalPricesForAuditAsync(TerminalPricesRequestModel inputModel)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            List<PricingData> storedProcReturns = new List<PricingData> {
                                            new PricingData()
                                                 {
                                                    TerminalId= 0,
                                                    Abbreviation = null,
                                                     Name = null,
                                                     City = null,
                                                     ZipCode = null,
                                                     StateCode = null,
                                                     PriceAvg = 0.0m,
                                                     Distance = 0.0d,
                                                     PricingDate =  DateTime.Today ,
                                                     TaxValue = 0.0m,
                                                     PriceLow = 0.0m,
                                                     PriceHigh = 0.0m,
                                                     Currency = 0,
                                                     Price = 0.0m,
                                                     PriceLoadDate = null,
                                                     ReportedDate = null,
                                                     TotalCount = 0,
                                                     RackTypeName = null,
                                                     FeedTypeName = null,
                                                     FuelClassTypeName = null,
                                                     PriceTypeName = null,
                                                     CurrencyName = null,
                                                     Amount = null,
                                                     Address = null
                                                 }
                                    };


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<PricingData>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            #endregion

            #region Act : perform the actual work of the test

            SalesCalculatorResponseModel response;
            response = await pricingController.GetTerminalPricesForAuditAsync(inputModel);

            #endregion

            #region Assert start – verify the result


            Assert.True(response.Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status should be Success");
            Assert.Null(response.Message);
            Assert.True(response.TerminalPrices[0].Abbreviation == storedProcReturns[0].Abbreviation, "Abbreviation must be equal");
            Assert.True(response.TerminalPrices[0].Address == storedProcReturns[0].Address, "Address must be equal");
            Assert.True(response.TerminalPrices[0].TerminalId == storedProcReturns[0].TerminalId, "TerminalId must be equal");
            Assert.True(response.TerminalPrices[0].Name == storedProcReturns[0].Name, "Name must be equal");
            Assert.True(response.TerminalPrices[0].City == storedProcReturns[0].City, "City must be equal");
            Assert.True(response.TerminalPrices[0].ZipCode == storedProcReturns[0].ZipCode, "ZipCode must be equal");
            Assert.True(response.TerminalPrices[0].StateCode == storedProcReturns[0].StateCode, "StateCode must be equal");
            Assert.True(response.TerminalPrices[0].PriceAvg == storedProcReturns[0].PriceAvg, "PriceAvg must be equal");
            Assert.True(response.TerminalPrices[0].Distance == storedProcReturns[0].Distance, "Distance must be equal");
            Assert.True(response.TerminalPrices[0].PricingDate == storedProcReturns[0].PricingDate, "PricingDate must be equal");
            Assert.True(response.TerminalPrices[0].TaxValue == storedProcReturns[0].TaxValue, "TaxValue must be equal");

            Assert.True(response.TerminalPrices[0].PriceLow == storedProcReturns[0].PriceLow, "PriceLow must be equal");
            Assert.True(response.TerminalPrices[0].PriceHigh == storedProcReturns[0].PriceHigh, "PriceHigh must be equal");
            Assert.True(response.TerminalPrices[0].Currency == storedProcReturns[0].Currency, "Currency must be equal");
            Assert.True(response.TerminalPrices[0].Price == storedProcReturns[0].Price, "Price must be equal");
            Assert.True(response.TerminalPrices[0].PriceLoadDate == storedProcReturns[0].PriceLoadDate, "PriceLoadDate must be equal");
            Assert.True(response.TerminalPrices[0].ReportedDate == storedProcReturns[0].ReportedDate, "ReportedDate must be equal");
            Assert.True(response.TerminalPrices[0].TotalCount == storedProcReturns[0].TotalCount, "TotalCount must be equal");
            Assert.True(response.TerminalPrices[0].RackTypeName == storedProcReturns[0].RackTypeName, "RackTypeName must be equal");
            Assert.True(response.TerminalPrices[0].FeedTypeName == storedProcReturns[0].FeedTypeName, "FeedTypeName must be equal");
            Assert.True(response.TerminalPrices[0].FuelClassTypeName == storedProcReturns[0].FuelClassTypeName, " FuelClassTypeName must be equal");
            Assert.True(response.TerminalPrices[0].PriceTypeName == storedProcReturns[0].PriceTypeName, "PriceTypeName must be equal");
            Assert.True(response.TerminalPrices[0].CurrencyName == storedProcReturns[0].CurrencyName, "CurrencyName must be equal");
            Assert.True(response.TerminalPrices[0].Amount == storedProcReturns[0].Amount, "Amount must be equal");
            Assert.True(response.TerminalPrices[0].Address == storedProcReturns[0].Address, "Address must be equal");




            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "TerminalRequestModel")]
        public async void Test_Service_GetClosestTerminalsAsync(TerminalRequestModel inputModel)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            List<TerminalDetails> storedProcReturns = new List<TerminalDetails> {
                                            new TerminalDetails()
                                                 {
                                                     Name = "Terminal name",
                                                    AvgPrice ="450",
                                                    Distance = 4.5,
                                                    Id = 123
                                                 }
                                    };


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<TerminalDetails>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            #endregion

            #region Act : perform the actual work of the test

            TerminalResponseModel response;
            response = await pricingController.GetClosestTerminalsAsync(inputModel);

            #endregion

            #region Assert start – verify the result

            Assert.True(response.Status.ToString().Equals("Success", StringComparison.CurrentCultureIgnoreCase), "Status should be Success");
            Assert.Null(response.Message);
            Assert.True(response.Terminals[0].AvgPrice == storedProcReturns[0].AvgPrice, "AvgPrice must be equal");
            Assert.True(response.Terminals[0].Distance == storedProcReturns[0].Distance, "Distance must be equal");
            Assert.True(response.Terminals[0].Id == storedProcReturns[0].Id, "Id must be equal");
            Assert.True(response.Terminals[0].Name == storedProcReturns[0].Name, "Name must be equal");

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "ProductDetailsRequestModel")]
        public async void Test_Service_GetAxxisProductDetailsAsync(ProductDetailsRequestModel inputModel)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            List<DropdownDisplayItem> storedProcReturns = new List<DropdownDisplayItem> {
                                            new DropdownDisplayItem()
                                                 {
                                                     Name = "dropdown1",
                                                     Id = 123
                                                 }
                                    };


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<DropdownDisplayItem>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            #endregion

            #region Act : perform the actual work of the test

            ProductDetailsResponseModel response;
            response = await pricingController.GetAxxisProductDetailsAsync(inputModel);

            #endregion

            #region Assert start – verify the result

            Assert.True(response.Status.ToString().Equals("Failed", StringComparison.CurrentCultureIgnoreCase), "Status should be Success");
            Assert.Null(response.Message);
            Assert.True(response.ProductDetails[0].Id == storedProcReturns[0].Id, "Id must be equal");
            Assert.True(response.ProductDetails[0].Name == storedProcReturns[0].Name, "Name must be equal");



            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "SourceBasedPriceRequestModel")]
        public async void Test_Service_GetLatestTerminalPriceAsync(SourceBasedPriceRequestModel request)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            PricingResponseModel storedProcReturns = new PricingResponseModel {
                EffectiveDate = DateTimeOffset.Parse("0001-01-01T00:00:00+00:00"),
                PriceLastUpdatedDate = DateTimeOffset.Parse("0001-01-01T00:00:00+00:00"),
                Currency = 0,
                Price = 0.0m,
                AvgPrice = 0.0m,
                LowPrice = 0.0m,
                HighPrice = 0.0m,
                PricingTypeId = 0,
                Status = 0,
                Message = null };


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            //stored proc call mocking.
            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<PricingResponseModel>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            #endregion

            #region Act : perform the actual work of the test

            PricingResponseModel response;
            response = await pricingController.GetLatestTerminalPriceAsync(request);

            #endregion

            #region Assert start – verify the result


            switch (request.PricingSourceId)
            {
                case 1:
                    Assert.True(request.TerminalId.HasValue && string.IsNullOrEmpty(response.Message),
                      "if TerminalId is null then message should be 'TerminalId field is required'.");
                    Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=1 status should be Success");
                    break;
                case 2:
                    Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=2 status should be Success");
                    break;
                case 3:
                    Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=3 status should be Success");
                    break;
                case 4:
                    Assert.True(response.Status.ToString().Equals("Failed", StringComparison.OrdinalIgnoreCase),
                        "for PricingCodeId=4 status should be Success");
                    Assert.True(response.Message.Equals("requested PricingSourceId is not supported.", StringComparison.OrdinalIgnoreCase), "Message should be, requested PricingSourceId is not supported");
                    break;
                default:
                    break;
            }

            #endregion
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async void Test_Service_GetLastUpdatedPricingDate(int requestPriceDetailId)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.


            //required mocking entity class PricingDetail included with MstPricingCode and MstPricingConfig
            var mockPricingDetail = DbContextMock.GetQueryableMockDbSet<PricingDetail>(_entity.PricingDetailEntities);
            var mockMstPricingConfig = DbContextMock.GetQueryableMockDbSet<MstPricingConfig>(_entity.MstPricingConfigEntities);

            var mDbContext = new Mock<DataContext>();
            //whenever PricingDetails and MstPricingConfig being access given return which mocked been used.
            mDbContext.Setup(c => c.PricingDetails).Returns(mockPricingDetail);
            mDbContext.Setup(c => c.MstPricingConfig).Returns(mockMstPricingConfig);


            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            DateTime response;



            if (requestPriceDetailId == 2)
            {
                await Assert.ThrowsAsync<InvalidCastException>(() => pricingController.GetLastUpdatedPricingDate(requestPriceDetailId));
            }
            else
            {
                response = await pricingController.GetLastUpdatedPricingDate(requestPriceDetailId);
                Assert.True(response == DateTime.Today);
            }
            #endregion

            #region Assert start – verify the result




            #endregion
        }

        [Theory]
        [InlineData(2, 1, 1, "2025-03-10")]
        [InlineData(2, 2, 2, "2017-3-1")]
        [InlineData(3, 3, 3, "2017-3-1")]
        [InlineData(4, 4, 4, "2017-3-1")]
        public async void Test_Service_IsCityRackPriceAvailable(int productId, int cityGroupTerminalId, int pricingSourceId, string effectiveDate)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.


            //required mocking entity class PricingDetail included with MstPricingCode and MstPricingConfig
            var mockExternalPricingAxxis = DbContextMock.GetQueryableMockDbSet<ExternalPricingAxxis>(_entity.ExternalPricingAxxisEntities);
            var mockMExternalPricingOpis = DbContextMock.GetQueryableMockDbSet<ExternalPricingOpis>(_entity.ExternalPricingOpisEntities);
            var mockExternalPricingPlatts = DbContextMock.GetQueryableMockDbSet<ExternalPricingPlatts>(_entity.ExternalPricingPlattsEntities);

            var mDbContext = new Mock<DataContext>();
            //whenever PricingDetails and MstPricingConfig being access given return which mocked been used.
            mDbContext.Setup(c => c.ExternalPricingAxxis).Returns(mockExternalPricingAxxis);
            mDbContext.Setup(c => c.ExternalPricingOpis).Returns(mockMExternalPricingOpis);
            mDbContext.Setup(c => c.ExternalPricingPlatts).Returns(mockExternalPricingPlatts);


            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            BooleanResponseModel response;


            DateTime? localeffectiveDate = DateTime.Parse(effectiveDate);
            if (pricingSourceId == 1)
            {
                // await Assert.ThrowsAsync<InvalidCastException>(() => pricingController.IsCityRackPriceAvailable(requestPriceDetailId));
                response = await pricingController.IsCityRackPriceAvailable(productId, cityGroupTerminalId, pricingSourceId, localeffectiveDate);
                Assert.True(response.Result);
                Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for pricingSourceId=1 status should be Success");
            }
            else if (pricingSourceId == 2)
            {
                response = await pricingController.IsCityRackPriceAvailable(productId, cityGroupTerminalId, pricingSourceId, localeffectiveDate);
                Assert.True(response.Result);
                Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for pricingSourceId=2 status should be Success");
            }
            else if (pricingSourceId == 3)
            {
                response = await pricingController.IsCityRackPriceAvailable(productId, cityGroupTerminalId, pricingSourceId, localeffectiveDate);
                Assert.True(response.Result);
                Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "for pricingSourceId=3 status should be Success");
            }
            else if (pricingSourceId == 4)
            {
                response = await pricingController.IsCityRackPriceAvailable(productId, cityGroupTerminalId, pricingSourceId, localeffectiveDate);
                Assert.False(response.Result);
                Assert.True(response.Status.ToString().Equals("Failed", StringComparison.OrdinalIgnoreCase),
                        "for pricingSourceId=4 status should be Success");
            }
            #endregion

            #region Assert start – verify the result




            #endregion
        }

        [Fact]
        public async void Test_Service_SyncAxxisPricingData()

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<int>
           (It.IsAny<string>())).ReturnsAsync(1);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            IntResponseModel response;
            response = await pricingController.SyncAxxisPricingData();


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Result == 1);
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "status should be Success");

            #endregion
        }

        [Fact]
        public async void Test_Service_SyncOpisPlattsPricingData()

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            List<SyncPricingResponse> storedProcreturns = new List<SyncPricingResponse> {
                new SyncPricingResponse { MstProductId = 0, ProductCode = "1x", ProductId = 2, ProductName = "pName1", RecordInserted = 5, SourceId = 5, TfxProductId = 111 }
            };
            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<SyncPricingResponse>
           (It.IsAny<string>())).ReturnsAsync(storedProcreturns);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            SyncPricingResponseModel response;
            response = await pricingController.SyncOpisPlattsPricingData();


            #endregion

            #region Assert start – verify the result

            Assert.True(response.PricingResponse.Count == 1);

            Assert.True(response.PricingResponse[0].MstProductId == storedProcreturns[0].MstProductId);
            Assert.True(response.PricingResponse[0].ProductCode == storedProcreturns[0].ProductCode);
            Assert.True(response.PricingResponse[0].ProductId == storedProcreturns[0].ProductId);
            Assert.True(response.PricingResponse[0].ProductName == storedProcreturns[0].ProductName);

            Assert.True(response.PricingResponse[0].RecordInserted == storedProcreturns[0].RecordInserted);
            Assert.True(response.PricingResponse[0].SourceId == storedProcreturns[0].SourceId);
            Assert.True(response.PricingResponse[0].TfxProductId == storedProcreturns[0].TfxProductId);
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "status should be Success");

            #endregion
        }

        public static IEnumerable<object[]> Keys
        {
            get
            {
                var testcase = new List<string> { "PricingDataSourcesUpdatedDate", "PricingDataLastUpdatedDate1", "key3" };
                yield return new object[] { testcase };
            }
        }

        [Theory]
        [MemberData(nameof(Keys))]
        public async void Test_Service_GetPricingConfig(List<string> keys)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);


            var mockMstPricingConfig = DbContextMock.GetQueryableMockDbSet<MstPricingConfig>(_entity.MstPricingConfigEntities);

            mDbContext.Setup(c => c.MstPricingConfig).Returns(mockMstPricingConfig);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            PricingConfigResponse response;
            response = await pricingController.GetPricingConfig(keys);


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Configs.Count == 2);
            bool exist = response.Configs.Any(x => x.Key == "PricingDataSourcesUpdatedDate");
            Assert.True(exist);
            exist = response.Configs.Any(x => x.Key == "PricingDataLastUpdatedDate1");
            Assert.True(exist);
            exist = response.Configs.Any(x => x.Key == "key3");
            Assert.False(exist);
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "ProductRequestModel")]
        public async void Test_Service_AddNewProduct(ProductRequestModel product)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            var mockMstProduct = DbContextMock.GetQueryableMockDbSet<MstProduct>(_entity.MstProductsEntities);

            mDbContext.Setup(c => c.MstProducts).Returns(mockMstProduct);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            IntResponseModel response;
            response = await pricingController.AddNewProduct(product);


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Result == 0); //added record id is 0..
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "ProductRequestModel")]
        public async void Test_Service_AddNewTfxProduct(ProductRequestModel product)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            var mockMstProduct = DbContextMock.GetQueryableMockDbSet<MstProduct>(_entity.MstProductsEntities);
            var mockMstTfxProducts = DbContextMock.GetQueryableMockDbSet<MstTfxProduct>(_entity.MstTfxProductsEntities);

            mDbContext.Setup(c => c.MstProducts).Returns(mockMstProduct);
            mDbContext.Setup(c => c.MstTfxProducts).Returns(mockMstTfxProducts);

            pricingRepository.Setup(p => p.SetEntityStateModified<MstProduct>(It.IsAny<MstProduct>()));

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            IntResponseModel response;
            response = await pricingController.AddNewTfxProduct(product);


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Result == 0); //added record id is 0..
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "ProductRequestModelForUpdate")]
        public async void Test_Service_UpdateTfxProduct(ProductRequestModel product)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            var mockMstProduct = DbContextMock.GetQueryableMockDbSet<MstProduct>(_entity.MstProductsEntities);
            var mockMstTfxProducts = DbContextMock.GetQueryableMockDbSet<MstTfxProduct>(_entity.MstTfxProductsEntities);

            mDbContext.Setup(c => c.MstProducts).Returns(mockMstProduct);
            mDbContext.Setup(c => c.MstTfxProducts).Returns(mockMstTfxProducts);

            pricingRepository.Setup(p => p.SetEntityStateModified<MstProduct>(It.IsAny<MstProduct>()));

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            IntResponseModel response;
            response = await pricingController.UpdateTfxProduct(product);


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Result == 1); //added record id is 0..
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public async void Test_Service_GetPricingConfigDetailst(int id)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            var mockMstPricingConfig = DbContextMock.GetQueryableMockDbSet<MstPricingConfig>(_entity.MstPricingConfigEntities);


            mDbContext.Setup(c => c.MstPricingConfig).Returns(mockMstPricingConfig);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            PricingConfigResponseModel response;
            response = await pricingController.GetPricingConfigDetails(id);


            #endregion

            #region Assert start – verify the result
            if (id == 0)
            {
                Assert.True(response.ConfigList.Count == 2);
            }
            else
            {
                Assert.True(response.Config.Id == 1);
                Assert.True(response.Config.IsActive);
                Assert.True(response.Config.Key == "PricingDataSourcesUpdatedDate");
                Assert.True(response.Config.Value == DateTime.Today.ToString());

            }
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "PricingConfigModel")]
        public async void Test_Service_EditPricingConfig(PricingConfigModel model)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(p => p.SetEntityStateModified<MstPricingConfig>(It.IsAny<MstPricingConfig>()));

            var mockMstPricingConfig = DbContextMock.GetQueryableMockDbSet<MstPricingConfig>(_entity.MstPricingConfigEntities);

            mDbContext.Setup(c => c.MstPricingConfig).Returns(mockMstPricingConfig);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            PricingConfigResponseModel response;
            response = await pricingController.EditPricingConfig(model);


            #endregion

            #region Assert start – verify the result
            if (model.Id <3)
            {
                Assert.True(response.Config.Id == 1 || response.Config.Id==2);
                Assert.True(response.Config.IsActive);
                Assert.True(response.Config.Key== "PricingDataSourcesUpdatedDate" || response.Config.Key == "PricingDataLastUpdatedDate1");
                Assert.Null(response.Config.Value);
                Assert.True(response.Config.UpdatedBy.Equals("0", StringComparison.OrdinalIgnoreCase),
                       "UpdatedBy should be 0");
            }
            else
            {
                Assert.Null(response.Config);
                Assert.Null(response.ConfigList);
                Assert.Null(response.Message);
            }
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "TerminalForFueltypesRequestModel")]
        public async void Test_Service_GetClosestTerminalsForFueltypesAsync(TerminalForFueltypesRequestModel model)

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            List<TerminalDetails> storedProcReturns = new List<TerminalDetails>()
            {
                new TerminalDetails{AvgPrice="4.5",Distance=25.5, Id=123,Name="parkland terminal1"},
                new TerminalDetails{AvgPrice="3.5",Distance=24.5, Id=124,Name="parkland terminal2"}

            };

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(p => p.SqlQueryToListAsync<TerminalDetails>
            (It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(storedProcReturns);

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);


            #endregion

            #region Act : perform the actual work of the test

            TerminalResponseModel response;
            response = await pricingController.GetClosestTerminalsForFueltypesAsync(model);


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Terminals.Count == 2);
            Assert.True(response.Terminals[0].AvgPrice== "4.5");
            Assert.True(response.Terminals[1].AvgPrice == "3.5");
            Assert.True(response.Terminals[0].Distance == 25.5);
            Assert.True(response.Terminals[1].Distance == 24.5);
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                       "status should be Success");

            #endregion
        }

        [Fact]        
        public async void Test_Service_GetAllTerminalsAsync()

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();

            var mockDbSet = DbContextMock.GetQueryableMockDbSet<MstExternalTerminal>(_entity.MstExternalTerminalsEntities);
            mDbContext.Setup(c => c.MstExternalTerminals).Returns(mockDbSet);

            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
           

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);


            #endregion

            #region Act : perform the actual work of the test

            List < DropdownDisplayItem > response;
            response = await pricingController.GetAllTerminalsAsync();


            #endregion

            #region Assert start – verify the result
            Assert.True(response.Count == 3);
            Assert.True(response.TrueForAll(xy=>xy.Name.Contains("Terminal")));

            #endregion
        }

        [Fact]
        public async void Test_Service_SyncActualOpisPricingData()

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            List<SyncPricingResponse> storedProcreturns = new List<SyncPricingResponse> {
                new SyncPricingResponse { MstProductId = 0, ProductCode = "1x", ProductId = 2, ProductName = "pName1", RecordInserted = 5, SourceId = 5, TfxProductId = 111 }
            };
            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(m => m.SqlQueryToListAsync<SyncPricingResponse>
           (It.IsAny<string>())).ReturnsAsync(storedProcreturns);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            SyncPricingResponseModel response;
            response = await pricingController.SyncActualOpisPricingData();


            #endregion

            #region Assert start – verify the result

            Assert.True(response.PricingResponse.Count == 1);

            Assert.True(response.PricingResponse[0].MstProductId == storedProcreturns[0].MstProductId);
            Assert.True(response.PricingResponse[0].ProductCode == storedProcreturns[0].ProductCode);
            Assert.True(response.PricingResponse[0].ProductId == storedProcreturns[0].ProductId);
            Assert.True(response.PricingResponse[0].ProductName == storedProcreturns[0].ProductName);

            Assert.True(response.PricingResponse[0].RecordInserted == storedProcreturns[0].RecordInserted);
            Assert.True(response.PricingResponse[0].SourceId == storedProcreturns[0].SourceId);
            Assert.True(response.PricingResponse[0].TfxProductId == storedProcreturns[0].TfxProductId);
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "status should be Success");

            #endregion
        }

        [Fact]
        public async void Test_Service_SyncDyedProductPricingFromClearProducts()

        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            var mDbContext = new Mock<DataContext>();
            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);

            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<int>
           (It.IsAny<string>())).ReturnsAsync(1);


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);



            #endregion

            #region Act : perform the actual work of the test

            IntResponseModel response;
            response = await pricingController.SyncDyedProductPricingFromClearProducts();


            #endregion

            #region Assert start – verify the result

            Assert.True(response.Result == 1);
            Assert.True(response.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                        "status should be Success");

            #endregion
        }
       
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 2)]
        [InlineData(5, 3)]
        public async void Test_Service_AssignNewTerminalForTierPricedOrder(int? terminalId, int requestPriceDetailsId)
        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.


            var mockPricingDetail = DbContextMock.GetQueryableMockDbSet<PricingDetail>(_entity.PricingDetailEntities);


            var mDbContext = new Mock<DataContext>();

            mDbContext.Setup(c => c.PricingDetails).Returns(mockPricingDetail);


            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(p => p.SetEntityStateModified<PricingDetail>(It.IsAny<PricingDetail>()));

            pricingRepository.Setup(bt => bt.BeginTransaction()).Returns(It.IsAny<DbContextTransaction>());
            pricingRepository.Setup(co => co.Commit(It.IsAny<DbContextTransaction>()));


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            #endregion

            #region Act : perform the actual work of the test

            BooleanResponseModel lstResponseModel;

           
            lstResponseModel = await pricingController.AssignNewTerminalForTierPricedOrder(terminalId, requestPriceDetailsId);


            #endregion

            #region Assert start – verify the result

            if(requestPriceDetailsId== terminalId.Value)
            {
                Assert.False(lstResponseModel.Result);
                Assert.True(lstResponseModel.Status.ToString().Equals("Failed", StringComparison.OrdinalIgnoreCase),
                           "status should be Failed");
                Assert.Null(lstResponseModel.Message);
            }
            else
            {
                Assert.True(lstResponseModel.Result);
                Assert.True(lstResponseModel.Status.ToString().Equals("Success", StringComparison.OrdinalIgnoreCase),
                           "status should be Success");
                Assert.True(lstResponseModel.Message.ToString().Equals("You have successfully assigned a different  terminal for " +
                    "this order; all future invoices will use this terminal when calculating PPG", StringComparison.OrdinalIgnoreCase),"should be equal");

            }



            #endregion
        }

       [Fact]
        public async void Test_Service_ResetCumulation()
        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.


            var mockRequestPriceDetail = DbContextMock.GetQueryableMockDbSet<RequestPriceDetail>(_entity.RequestPriceDetailsEntities);
           var mockCumulationDetails = DbContextMock.GetQueryableMockDbSet<CumulationDetail>(_entity.CumulationDetailsEntities);

            var mDbContext = new Mock<DataContext>();

            mDbContext.Setup(c => c.RequestPriceDetails).Returns(mockRequestPriceDetail);
            mDbContext.Setup(c => c.CumulationDetails).Returns(mockCumulationDetails);


            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<int>(It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(1);
            pricingRepository.Setup(m => m.ExecuteSqlCommand(It.IsAny<string>())).Returns(1);
            //pricingRepository.Setup(bt => bt.BeginTransaction()).Returns(It.IsAny<DbContextTransaction>());
            pricingRepository.Setup(co => co.Commit(It.IsAny<DbContextTransaction>()));


            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            #endregion

            #region Act : perform the actual work of the test

            bool response;


            response = await pricingController.ResetCumulation();


            #endregion

            #region Assert start – verify the result

            Assert.True(response);


            #endregion
        }

        [Theory]
        [JsonFileData(@"JsonFiles\PricingController\RequestAndResponse.json", "lstTerminalForFueltypesRequestModel")]
        public async void Test_Service_UpdateCumulationQtyPostInvoiceCreate(List<CumulationQuantityUpdateViewModel> cumulationQtyList)
        {

            #region Arrange : setup the testing objects and prepare the prerequisites for your test.

            

            var mDbContext = new Mock<DataContext>();
            var mockCumulationDetails = DbContextMock.GetQueryableMockDbSet<CumulationDetail>(_entity.CumulationDetailsEntities);
            mDbContext.Setup(c => c.CumulationDetails).Returns(mockCumulationDetails);


            var pricingRepository = new Mock<PricingRepository>(mDbContext.Object);
            pricingRepository.Setup(m => m.SqlQueryFirstOrDefaultAsync<int>(It.IsAny<string>(), It.IsAny<SqlParameter[]>())).ReturnsAsync(1);

            pricingRepository.Setup(bt => bt.BeginTransaction()).Returns(It.IsAny<DbContextTransaction>());
            pricingRepository.Setup(co => co.Commit(It.IsAny<DbContextTransaction>()));
            pricingRepository.Setup(p => p.SetEntityStateModified<CumulationDetail>(It.IsAny<CumulationDetail>()));

            PricingDomain pricingDomain = new PricingDomain(pricingRepository.Object);
            PricingController pricingController = new PricingController(pricingDomain);

            #endregion

            #region Act : perform the actual work of the test

            bool response;

            response = await pricingController.UpdateCumulationQtyPostInvoiceCreate(cumulationQtyList);

            #endregion

            #region Assert start – verify the result

            Assert.True(response);


            #endregion
        }

    }

}