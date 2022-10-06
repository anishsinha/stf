using System;
using System.Text;
using System.Web;
using TrueFill.SCIM2.Model.Authentication;

namespace TrueFill.SCIM2
{

    public class Authentication
    {

        #region enums
        public enum AuthenticationMode { BasicAuth, HTTPHeader, OAuth2 }

        #endregion

        #region Properties
        private readonly AuthenticationMode authenticationMode;
        public AuthenticationMode GetAuthenticationMode
        {
            get
            {
                return authenticationMode;
            }
        }

        #endregion 

        public Authentication(HttpRequest httpRequest, IAuthenticationModes iAuthenticationMode)
        {
            if (iAuthenticationMode is BasicAuth basicAuth)
            {
                if (IsBasicAuthValid(httpRequest, basicAuth))
                {
                    authenticationMode = AuthenticationMode.BasicAuth;
                }
                return; // call is authenticated
            }
            else if (iAuthenticationMode is HTTPHeader httpHeader)
            {
                if (IsHttpHeaderAuthValid(httpRequest, httpHeader))
                {
                    authenticationMode = AuthenticationMode.HTTPHeader;
                }
                return; // call is authenticated
            }
            else if (iAuthenticationMode is OAUth2)
            {
                //_authenticationMode = AuthenticationMode.OAuth2;
                throw new Exception("OAuth authentication is not yet implemented");
            }

        }

        private static bool IsHttpHeaderAuthValid(HttpRequest httpRequest, HTTPHeader httpHeader)
        {
            string bearerheader = httpRequest.Headers["Authorization"];
            if (bearerheader == null)
            {
                throw new HttpException(401, "no authorization");
            }
            else if (!bearerheader.StartsWith("Bearer "))
            {
                throw new HttpException(401, "not a bearer authorization");
            }
            else if (bearerheader != "Bearer " + httpHeader.Token)
            {
                throw new HttpException(401, "invalid token");
            }
            return true;
        }

        private static bool IsBasicAuthValid(HttpRequest httpRequest, BasicAuth basicAuth)
        {
            string basicrheader = httpRequest.Headers["Authorization"];
            if (basicrheader == null)
            {
                throw new HttpException(401, "no authorization");
            }
            else if (!basicrheader.StartsWith("Basic "))
            {
                throw new HttpException(401, "not a basic authorization");
            }
            else
            {
                var encoding = Encoding.GetEncoding("iso-8859-1");
                var byteArr = Convert.FromBase64String(basicrheader.Substring(6));
                string credentials = encoding.GetString(byteArr);
                if (credentials != string.Format("{0}:{1}", basicAuth.Username, basicAuth.Password))
                {
                    throw new HttpException(401, "invalid token");
                }
            }
            return true;
        }

    }
}
