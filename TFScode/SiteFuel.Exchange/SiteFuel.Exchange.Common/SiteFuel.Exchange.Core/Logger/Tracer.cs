using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Core.Logger
{
    public class Tracer : IDisposable
    {
        protected bool disposed = false;
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Tracer(string className, string methodName)
        {
            ClassName = className;
            MethodName = methodName;
            StartTime = DateTime.Now;
        }

        private void EndTracer()
        {
            EndTime = DateTime.Now;
            LogManager.Logger.WriteTrace(ClassName, MethodName, $"Elapsed Time:{(EndTime - StartTime).TotalMilliseconds}ms");
        }
        public void Dispose()
        {
            EndTracer();
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // called via myClass.Dispose(). 
                    // OK to use any private object references
                }
                // Release unmanaged resources.
                // Set large fields to null.                
                disposed = true;
            }
        }

        ~Tracer()
        {
            if (!disposed)
            {
                Dispose(false);
            }
        }
    }
}
