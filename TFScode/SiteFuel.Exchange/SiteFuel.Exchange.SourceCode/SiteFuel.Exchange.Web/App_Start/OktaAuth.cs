using Microsoft.AspNet.Identity;
using Sustainsys.Saml2.Owin;
using System.Configuration;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2;
using Sustainsys.Saml2.WebSso;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Owin;
using SiteFuel.Exchange.Domain;
using System;

namespace SiteFuel.Exchange.Web
{
    public class OktaAuth
    {
        public static void OktaAuthentication(IAppBuilder app)
        {
            HelperDomain helperDomain = new HelperDomain();
            var idpS = helperDomain.GeActiveIdentityProvider(); //1293 - Parkland
            if (idpS != null && idpS.Count>0)
            {
                //https://stackoverflow.com/questions/26166826/usecookieauthentication-vs-useexternalsignincookie
                app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie); // Passive
                foreach (var idp in idpS)
                {
                    app.UseSaml2Authentication(CreateSaml2Options(helperDomain.GetServerUrl(), idp.IdentityProviderIssuer, idp.IdentityProviderSsoUri, idp.Certificate));
                }
            }
        }


        private static Saml2AuthenticationOptions CreateSaml2Options(string serverUrl, string IdentityProviderIssuer, string IdentityProviderSsoUri, string certificate)
        {
            var applicationBaseUri = new Uri(serverUrl);
            var saml2BaseUri = new Uri(applicationBaseUri, "saml2");
            var identityProviderIssuer = IdentityProviderIssuer;
            var identityProviderSsoUri = new Uri(IdentityProviderSsoUri);

            var Saml2Options = new Saml2AuthenticationOptions(false)
            {
                SPOptions = new SPOptions
                {
                    EntityId = new EntityId(saml2BaseUri.AbsoluteUri),
                    ReturnUrl = applicationBaseUri

                }
            };

            var identityProvider = new IdentityProvider(new EntityId(identityProviderIssuer), Saml2Options.SPOptions)
            {
                AllowUnsolicitedAuthnResponse = true,
                Binding = Saml2BindingType.HttpRedirect,
                SingleSignOnServiceUrl = identityProviderSsoUri
            };

            byte[] bytes = Encoding.ASCII.GetBytes(certificate);
            identityProvider.SigningKeys.AddConfiguredKey(
                new X509Certificate2(bytes));

            Saml2Options.IdentityProviders.Add(identityProvider);

            return Saml2Options;
        }

    }
}