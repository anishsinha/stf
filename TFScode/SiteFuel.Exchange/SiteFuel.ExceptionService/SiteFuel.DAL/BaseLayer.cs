using SiteFuel.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.DAL
{
    public class BaseLayer: IDisposable
    {
        private ExceptionContext _context;
        public BaseLayer()
        {
            _context = new ExceptionContext();
        }
        public BaseLayer(BaseLayer layer)
        {
            _context = layer.Context;
        }
        protected ExceptionContext Context
        {
            get { return _context; }
            set { _context = value; }
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
                // free managed resources  
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }
    }
}
