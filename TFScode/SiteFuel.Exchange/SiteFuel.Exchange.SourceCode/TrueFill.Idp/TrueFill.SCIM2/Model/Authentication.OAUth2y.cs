namespace TrueFill.SCIM2.Model.Authentication
{
    public class OAUth2 : IAuthenticationModes
    {
        public string Access_token_endpoint_URI { get; set; }
        public string Authorization_endpoint_URI { get; set; }
        public string Client_ID { get; set; }
        public string Client_Secret { get; set; }
    }
}
