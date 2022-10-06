using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrueFill.SCIM2Service
{
    public class Auth
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccessTokenEndPointURI { get; set; }
        public string AuthorizationEndpointURI { get; set; }
        public string ClientID { get; set; }
        public string ClientSecret  { get; set; }
        public string BearerToken { get; set; }
    }
}