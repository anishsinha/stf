using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class LogMapper
    {
        public static AuditLog ToEntity(this AuditLogViewModel viewModel, AuditLog entity = null)
        {
            if (entity == null)
                entity = new AuditLog();

            entity.Id = viewModel.Id;
            entity.AuditEntityId = viewModel.AuditEntityId;
            entity.AuditEntityType = CropString(viewModel.AuditEntityType, 20);
            entity.AuditEventType = CropString(viewModel.AuditEventType, 20);
            entity.CallSite = CropString(viewModel.CallSite, 30);
            entity.CreatedBy = viewModel.CreatedBy;
            entity.CreatedDate = viewModel.CreatedDate;
            entity.JsonMessage = CropString(viewModel.JsonMessage, 100);
            entity.MachineName = CropString(viewModel.MachineName, 20);
            entity.RemoteAddress = CropString(viewModel.RemoteAddress, 20);
            entity.Message = CropString(viewModel.Message, 100);
            entity.Url = CropString(viewModel.Url, 20);

            return entity;
        }

        private static string CropString(string input, int cropLength)
        {
            if (!string.IsNullOrEmpty(input))
                return input.Substring(0, input.Length > cropLength ? cropLength : input.Length);
            return input;
        }
    }
}
