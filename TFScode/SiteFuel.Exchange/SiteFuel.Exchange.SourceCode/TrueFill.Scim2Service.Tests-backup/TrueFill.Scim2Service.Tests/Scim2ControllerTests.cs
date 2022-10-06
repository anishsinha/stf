using Moq;
using SiteFuel.Exchange.Core;
using System;
using TrueFill.SCIM2Service;
using Xunit;
using System.Web;

namespace TrueFill.Scim2Service.Tests
{

    public class EntityFixture : IDisposable
    {

        public string auth = @"{
  ""Username"": ""ParklandOkta"",
  ""Password"": ""4fb95c08-8a0e-4ba6-b053-ac0b4bcf82ff"",
  ""AccessTokenEndPointURI"":""uri"" ,
  ""AuthorizationEndpointURI"": ""url"",
  ""ClientID"": """",
  ""ClientSecret"": """",
  ""BearerToken"": """"
}";

        public EntityFixture()
        {

        }

        public void Dispose()
        {


        }




    }

    public class Scim2ControllerTests : IClassFixture<EntityFixture>
    {
        readonly EntityFixture _entity;

        public Scim2ControllerTests(EntityFixture entity)
        {
            _entity = entity;
        }

        [Fact]
        public void Test1()
        {
            int i = 10;
            Assert.False(10 ==10);
        }
    }
}