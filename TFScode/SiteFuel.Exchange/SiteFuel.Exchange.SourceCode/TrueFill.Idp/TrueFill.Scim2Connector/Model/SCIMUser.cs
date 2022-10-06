using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System.Data;
using TrueFill.SCIM2.Model;

namespace TrueFill.SCIM2Service
{
    public class SCIMUser : CResource
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Smurf { get; set; }

        public SCIMUser()
        {
            Schemas = SchemasHelper.Get(SchemasHelper.Schema.User);
        }

        public SCIMUser(User r)
        {
            Schemas = SchemasHelper.Get(SchemasHelper.Schema.User);
            Id = r.Id;
            Username = r.Email;
            Smurf = "";
        }

        public SCIMUser(UserViewModel r)
        {
            Schemas = SchemasHelper.Get(SchemasHelper.Schema.User);
            Id = r.Id;
            Username = r.Email;
            Smurf = "";
        }
        public SCIMUser(DataRow r)
        {
            Schemas = SchemasHelper.Get(SchemasHelper.Schema.User);
            Id = (int)r["id"];
            Username = r["Username"].ToString();
            Smurf = r["Smurf"].ToString();
        }

    }

}