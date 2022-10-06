using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.ViewModels
{
    [Serializable]
    public class QueueMessageFatalException : Exception
    {
        public List<string> ErrorInfos { get; set; }
        public QueueMessageFatalException()
        {
        }

        public QueueMessageFatalException(string message, List<string>errorInfos) : base(message)
        {
            ErrorInfos = errorInfos;
        }

        public QueueMessageFatalException(string message) : base(message)
        {
        }

        public QueueMessageFatalException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected QueueMessageFatalException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
