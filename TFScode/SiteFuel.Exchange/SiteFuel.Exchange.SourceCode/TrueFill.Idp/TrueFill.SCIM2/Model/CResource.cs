namespace TrueFill.SCIM2.Model
{
    public class CResource
    {
        public string[] Schemas { get; set; }
        public CResource() { }
        public void SetSchema(SchemasHelper.Schema schema)
        {
            Schemas = SchemasHelper.Get(schema);
        }
    }
}
