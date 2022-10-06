namespace TrueFill.SCIM2.Model
{
    public class CResources
    {
        public string[] Schemas { get; set; }
        public CResource[] Resources { get; set; }
        public CResources() { }
        public CResources(SchemasHelper.Schema schema)
        {
            Schemas = SchemasHelper.Get(schema);
        }
    }
}
