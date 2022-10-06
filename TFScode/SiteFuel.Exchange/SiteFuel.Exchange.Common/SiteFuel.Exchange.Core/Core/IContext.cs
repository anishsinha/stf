namespace SiteFuel.Exchange.Core
{
    public interface IContext
    {
        D GetDomain<D>() where D : IDomain;

        string ConnectionString { get; }
    }

    public class ContextFactory
    {
        private static IContext _context;

        public static IContext Current
        {
            get
            {
                return _context;
            }
        }

        public static void Register(IContext context)
        {
            _context = context;
        }
    }

}
