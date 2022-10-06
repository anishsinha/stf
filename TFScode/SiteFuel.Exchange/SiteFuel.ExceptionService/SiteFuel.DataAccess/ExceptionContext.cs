using SiteFuel.DataAccess.Entities;
using SiteFuel.DataAccess.Extensions;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SiteFuel.DataAccess
{
    public class ExceptionContext : IDisposable
    {
        private ExceptionDataContext _context;

        public ExceptionContext()
        {
            _context = new ExceptionDataContext();
        }

        public ExceptionContext(string connectionString)
        {
            _context = new ExceptionDataContext(connectionString);
        }

        public ExceptionDataContext DataContext
        {
            get
            {
                return _context;
            }
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var str = ex.EntityValidationErrors.Select(t => t.DbValidationErrorsToString(t.ValidationErrors))
                .Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
                //Logger.LogManager.Logger.WriteException("SiteFuelUow", "Commit", str, ex);
                throw new DbEntityValidationException(str);
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                var str = ex.EntityValidationErrors.Select(t => t.DbValidationErrorsToString(t.ValidationErrors))
                .Aggregate(string.Empty, (current, next) => string.Format("{0}{1}{2}", current, Environment.NewLine, next));
                //Logger.LogManager.Logger.WriteException("SiteFuelUow", "Commit", str, ex);
                throw new DbEntityValidationException(str);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }
    }
}
