using System;
using SiteFuel.Exchange.Utilities;

namespace SiteFuel.Exchange.Core
{
    public class ApplicationContext : IContext
    {
        public D GetDomain<D>() where D : IDomain
        {
            return (D)Activator.CreateInstance(typeof(D));
        }

        public string ConnectionString
        {
            get
            {
                return ApplicationConstants.DatabaseConnection;
            }
        }
    }
}