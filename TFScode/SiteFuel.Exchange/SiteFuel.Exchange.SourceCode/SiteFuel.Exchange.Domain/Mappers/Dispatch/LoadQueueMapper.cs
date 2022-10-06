using SiteFuel.Exchange.Core.StringResources;
using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.ViewModels;
using System.Collections.Generic;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class LoadQueueMapper
    {
        public static List<DsbLoadQueueDetails> ToEntity(this List<DsbLoadQueueViewModel> viewModel)
        {
            var loadQueueDetails = new List<DsbLoadQueueDetails>();
            if (viewModel != null)
            {
                foreach (var item in viewModel)
                {
                    loadQueueDetails.Add(item.ToEntity());
                }
            }
            return loadQueueDetails;
        }
        public static DsbLoadQueueDetails ToEntity(this DsbLoadQueueViewModel viewModel)
        {
            var loadQueueDetails = new DsbLoadQueueDetails();
            loadQueueDetails.ScheduleBuilderId = viewModel.ScheduleBuilderId;
            loadQueueDetails.Date = viewModel.Date;
            loadQueueDetails.RegionId = viewModel.RegionId;
            loadQueueDetails.ShiftId = viewModel.ShiftId;
            loadQueueDetails.ShiftIndex = viewModel.ShiftIndex;
            loadQueueDetails.DriverColIndex = viewModel.DriverColIndex;
            loadQueueDetails.TrailerInfo = viewModel.TrailerInfo;
            loadQueueDetails.TfxDriverId = viewModel.TfxDriverId;
            loadQueueDetails.DeliveryRequestInfo = viewModel.DeliveryRequestInfo;
            loadQueueDetails.DriverColJsonInfo = viewModel.DriverColJsonInfo;
            loadQueueDetails.TfxUserId = viewModel.TfxUserId;
            loadQueueDetails.TfxCompanyId = viewModel.TfxCompanyId;
            loadQueueDetails.UserLanguage = viewModel.UserLanguage;
            loadQueueDetails.CreatedBy = viewModel.TfxUserId;
            return loadQueueDetails;
        }
        public static List<DsbLoadQueueStatusViewModel> ToStatusEntity(this List<DsbLoadQueueViewModel> viewModel)
        {
            var loadQueueDetails = new List<DsbLoadQueueStatusViewModel>();
            if (viewModel != null)
            {
                foreach (var item in viewModel)
                {
                    loadQueueDetails.Add(item.ToStatusEntity());
                }
            }
            return loadQueueDetails;
        }
        public static DsbLoadQueueStatusViewModel ToStatusEntity(this DsbLoadQueueViewModel viewModel)
        {
            var loadQueueStatus = new DsbLoadQueueStatusViewModel();
            loadQueueStatus.ShiftIndex = viewModel.ShiftIndex;
            loadQueueStatus.DriverColIndex = viewModel.DriverColIndex;
            loadQueueStatus.TfxDriverName = viewModel.TfxDriverName;
            loadQueueStatus.TfxUserId = viewModel.TfxUserId;
            loadQueueStatus.TfxCompanyId = viewModel.TfxCompanyId;
            loadQueueStatus.Messages = new List<LoadQueueStatus>();
            var statusMessage = string.Format(Resource.valMessageLoadQueueSuccess, loadQueueStatus.TfxDriverName, loadQueueStatus.DriverColIndex, loadQueueStatus.ShiftIndex);
            loadQueueStatus.Messages.Add(new LoadQueueStatus { StatusMessage = statusMessage });
            return loadQueueStatus;
        }
        public static List<DsbLoadQueueValidateViewModel> ToValidateEntity(this List<DsbLoadQueueDetails> viewModel)
        {
            var loadQueueDetails = new List<DsbLoadQueueValidateViewModel>();
            if (viewModel != null)
            {
                foreach (var item in viewModel)
                {
                    loadQueueDetails.Add(item.ToValidateEntity());
                }
            }
            return loadQueueDetails;
        }
        public static DsbLoadQueueValidateViewModel ToValidateEntity(this DsbLoadQueueDetails entity)
        {
            var loadQueueDetails = new DsbLoadQueueValidateViewModel();
            loadQueueDetails.ShiftIndex = entity.ShiftIndex;
            loadQueueDetails.DriverColIndex = entity.DriverColIndex;
            if (entity.TrailerInfo != null)
            {
                loadQueueDetails.TrailerInfo = entity.TrailerInfo;
            }
            loadQueueDetails.TfxDriverId = entity.TfxDriverId;
            if (entity.DeliveryRequestInfo != null)
            {
                loadQueueDetails.DeliveryRequestInfo = entity.DeliveryRequestInfo;
            }
            loadQueueDetails.TfxUserId = entity.TfxUserId;
            loadQueueDetails.TfxCompanyId = entity.TfxCompanyId;
            return loadQueueDetails;
        }
        public static List<DsbLoadQueueViewModel> ToEntity(this List<DsbLoadQueueDetails> viewModel)
        {
            var loadQueueDetails = new List<DsbLoadQueueViewModel>();
            if (viewModel != null)
            {
                foreach (var item in viewModel)
                {
                    loadQueueDetails.Add(item.ToEntity());
                }
            }
            return loadQueueDetails;
        }
        public static DsbLoadQueueViewModel ToEntity(this DsbLoadQueueDetails viewModel)
        {
            var loadQueueDetails = new DsbLoadQueueViewModel();
            loadQueueDetails.Id = viewModel.Id;
            loadQueueDetails.ScheduleBuilderId = viewModel.ScheduleBuilderId;
            loadQueueDetails.Date = viewModel.Date;
            loadQueueDetails.RegionId = viewModel.RegionId;
            loadQueueDetails.ShiftId = viewModel.ShiftId;
            loadQueueDetails.ShiftIndex = viewModel.ShiftIndex;
            loadQueueDetails.DriverColIndex = viewModel.DriverColIndex;
            loadQueueDetails.TrailerInfo = viewModel.TrailerInfo;
            loadQueueDetails.TfxDriverId = viewModel.TfxDriverId;
            loadQueueDetails.DeliveryRequestInfo = viewModel.DeliveryRequestInfo;
            loadQueueDetails.DriverColJsonInfo = viewModel.DriverColJsonInfo;
            loadQueueDetails.TfxUserId = viewModel.TfxUserId;
            loadQueueDetails.TfxCompanyId = viewModel.TfxCompanyId;
            loadQueueDetails.UserLanguage = viewModel.UserLanguage;
            loadQueueDetails.CreatedBy = viewModel.TfxUserId;
            return loadQueueDetails;
        }
    }
}
