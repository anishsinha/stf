using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Domain.Mappers;
using SiteFuel.Exchange.Logger;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace SiteFuel.Exchange.Domain
{
    public static class AuditLogger
    {
        private static readonly System.Timers.Timer _aTimer;
        private static ConcurrentQueue<AuditLogViewModel> _auditLogs = new ConcurrentQueue<AuditLogViewModel>();
        private const int intervalTime = 20000;

        static AuditLogger()
        {
            _aTimer = new System.Timers.Timer();
            _aTimer.Elapsed += new ElapsedEventHandler(SyncAuditLog);
            _aTimer.Interval = intervalTime;
            _aTimer.Enabled = true;
            _aTimer.Start();
        }

        public static void AddAuditLog(UserContext userContext, AuditLogViewModel auditLog)
        {
            try
            {
                var request = HttpContext.Current?.Request;
                if (userContext != null)
                {
                    auditLog.CreatedBy = userContext.Id;
                }
                if (request != null)
                {
                    auditLog.MachineName = Environment.MachineName;
                    auditLog.Url = request.Url.AbsolutePath;
                    auditLog.RemoteAddress = request.UserHostAddress;
                }

                _auditLogs.Enqueue(auditLog);
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuditLogger", "AddAuditLog", ex.Message, ex);
            }
        }

        private static void SyncAuditLog(object sender, ElapsedEventArgs e)
        {
            try
            {
                var logDomain = new ExceptionLogDomain();
                if (!_auditLogs.IsEmpty)
                {
                    var auditLogs = new List<AuditLog>();
                    while (_auditLogs.TryPeek(out AuditLogViewModel log))
                    {
                        auditLogs.Add(log.ToEntity());
                        _auditLogs.TryDequeue(out AuditLogViewModel removedlog);
                    }
                    logDomain.AddAuditLogs(auditLogs);
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("AuditLogger", "SyncAuditLog", ex.Message, ex);
            }
        }

    }
}
