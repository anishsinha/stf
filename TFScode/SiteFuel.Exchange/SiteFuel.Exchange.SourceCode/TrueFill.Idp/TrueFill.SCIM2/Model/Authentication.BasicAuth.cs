namespace TrueFill.SCIM2.Model.Authentication
{
        public class BasicAuth : IAuthenticationModes
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
}
