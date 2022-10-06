using Mono.Reflection;
using Moq;
using NUnit.Framework;
using SiteFuel.Exchange.Core;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using TrueFill.SCIM2Service;

namespace TrueFill.Scim2Service.Tests
{

 

    public class Scim2ControllerTests 
    {
       
        public string authJson = @"{
  ""Username"": ""ParklandOkta"",
  ""Password"": ""4fb95c08-8a0e-4ba6-b053-ac0b4bcf82ff"",
  ""AccessTokenEndPointURI"":""uri"" ,
  ""AuthorizationEndpointURI"": ""url"",
  ""CNewMethodlientID"": """",
  ""ClientSecret"": """",
  ""BearerToken"": """"
}";

        string createNewUserOktaJson = @"{
                ""RootElement"": {
                    ""schemas"": [ ""urn:ietf:params:scim:schemas:core:2.0:User"" ],
         ""userName"": ""anish@truefill.com"", ""name"": { ""givenName"": ""anish"", ""familyName"": ""sinha"" },
         ""emails"": [ { ""primary"": true, ""value"": ""anish@truefill.com"", ""type"": ""work"" } ],
         ""displayName"": ""anish sinha"", ""locale"": ""en-US"", ""externalId"": ""00u3s8hma9ZnrTBfp5d7"", ""groups"": [],
         ""password"": ""qb46IkyQ"", ""active"": true }";



[Test]
        public void ProcessScimRequest_GetUsers()
        {

            Scim2Controller scim2Controller;
            HttpContext httpContext;
            string qureyString = "startIndex=1&count=100";
            string endPoint = "http://localhost.com/Scim2Connector.aspx";
           
            GetMockHttpContext(out scim2Controller, out httpContext, endPoint, qureyString);
            scim2Controller.ProcessScimRequest(httpContext);

        }

        [Test]
        public void ProcessScimRequest_GetUsersFilter()
        {

            Scim2Controller scim2Controller;
            HttpContext httpContext;
            string endPoint = "http://localhost.com/Scim2Connector.aspx";
            string qureyString = "filter=abc@truefill.com&startIndex=1&count=100";
            GetMockHttpContext(out scim2Controller, out httpContext, endPoint, qureyString);
            scim2Controller.ProcessScimRequest(httpContext);   
        }


        [Test]
        public void ProcessScimRequest_NewUser_POST()
        {

            //Scim2Controller scim2Controller;
            //HttpContext httpContext;
            //string endPoint = "http://localhost.com/Scim2Connector.aspx";
            //string qureyString = "filter=abc@truefill.com&startIndex=1&count=100";
            //GetMockHttpContext(out scim2Controller, out httpContext, endPoint, qureyString);

            //typeof(HttpRequest).GetField("_httpMethod", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(httpContext.Request, "POST");

            //byte[] byteArray = System.Text.Encoding.ASCII.GetBytes(createNewUserOktaJson);

            //MemoryStream MemoryStream = ToStream(byteArray);
            //Stream stream = ToStream(byteArray);
            //MemoryStream.CopyTo(stream);

            //// System.Web.HttpInputStream ff;


            //typeof(HttpRequest).GetField("_inputStream", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(httpContext.Request, stream);

            //scim2Controller.ProcessScimRequest(httpContext);
        }

        public MemoryStream ToStream(byte[] bytes)
        {
            return new MemoryStream(bytes)
            {
                Position = 0
            };
        }

        private void GetMockHttpContext(out Scim2Controller scim2Controller, out HttpContext httpContext, string endPoint, string qureyString)
        {
            Mock<ScimDomain> scimDomain = new Mock<ScimDomain>(); 
            scimDomain.Setup(c => c.GetBasicCredentials()).Returns(authJson); //--------------- mocking GetBasicCredentials

            List<SCIMUser> response = new List<SCIMUser>();
            response.Add(new SCIMUser() { Id = 1, Username = "testUserName1" });
            response.Add(new SCIMUser() { Id = 2, Username = "testUserName2" });

            scimDomain.Setup(c => c.GetUsers(1293, 1)).Returns(response);  //--------------- mocking GetUsers

            if (qureyString.Contains("filter"))
            {
                UserViewModel uVM = new UserViewModel();
                uVM.Id = 5;
                uVM.Email = "abc@bc.com";
                scimDomain.Setup(c => c.GetUserByEmail("abc@bc.com")).Returns(uVM); //--------------- mocking GetUserByEmail
            }

            var context = new Mock<IContext>();
            scim2Controller = new Scim2Controller(scimDomain.Object, context.Object);
           

            var httpReq = new HttpRequest("", endPoint, qureyString);

            //---------------Headers mocking
            NameValueCollection headers = httpReq.Headers;            

            byte[] bytes = Encoding.ASCII.GetBytes("ParklandOkta:4fb95c08-8a0e-4ba6-b053-ac0b4bcf82ff");
            string key = Convert.ToBase64String(bytes);

            Type t = headers.GetType();
            const BindingFlags nonPublicInstanceMethod = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance;

            t.InvokeMember("MakeReadWrite", nonPublicInstanceMethod, null, headers, null);
            t.InvokeMember("InvalidateCachedArrays", nonPublicInstanceMethod, null, headers, null);

            // eg. add Basic Authorization header
            t.InvokeMember("BaseRemove", nonPublicInstanceMethod, null, headers, new object[] { "Authorization" });
            t.InvokeMember("BaseAdd", nonPublicInstanceMethod, null, headers,
                new object[] { "Authorization", new ArrayList { "Basic " + key } });

            t.InvokeMember("MakeReadOnly", nonPublicInstanceMethod, null, headers, null);

            httpContext = new HttpContext(httpReq,
                                 new HttpResponse(new StringWriter(new StringBuilder("Some response text!"))));
            httpContext.RewritePath(httpReq.FilePath,"/USERS","");

            //--------------QueryString mocking

            NameValueCollection queryString = httpContext.Request.QueryString;

            Type f = queryString.GetType();
            const BindingFlags nonPublicInstanceMethod1 = BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance;

            f.InvokeMember("MakeReadWrite", nonPublicInstanceMethod, null, queryString, null);
            f.InvokeMember("InvalidateCachedArrays", nonPublicInstanceMethod, null, queryString, null);

            // eg. add Basic Authorization header
            f.InvokeMember("BaseRemove", nonPublicInstanceMethod1, null, queryString, new object[] { "startIndex" });
            f.InvokeMember("BaseAdd", nonPublicInstanceMethod1, null, queryString,
                new object[] { "startIndex", new ArrayList {  "1" } });

            f.InvokeMember("BaseRemove", nonPublicInstanceMethod1, null, queryString, new object[] { "count" });
            f.InvokeMember("BaseAdd", nonPublicInstanceMethod1, null, queryString,
                new object[] { "count", new ArrayList { "100" } });

            if (qureyString.Contains("filter"))
            {

                f.InvokeMember("BaseRemove", nonPublicInstanceMethod1, null, queryString, new object[] { "filter" });
                f.InvokeMember("BaseAdd", nonPublicInstanceMethod1, null, queryString,
                    new object[] { "filter", new ArrayList { "userName eq abc@truefill.com" } });
            }

            f.InvokeMember("MakeReadOnly", nonPublicInstanceMethod1, null, queryString, null);
        }
    }
}