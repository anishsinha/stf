using System;

namespace SiteFuel.Exchange.DataAccess.Entities
{
	public class QbLog
    {
		public int Id { get; set; }
        public int EntityType { get; set; }
        public string Ticket { get; set; }
        public string Response { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public string JsonMessage { get; set; }
    }
}

