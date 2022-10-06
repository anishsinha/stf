using SiteFuel.Exchange.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SiteFuel.Exchange.Utilities;
using System.Web.Http.ModelBinding;
using SiteFuel.Exchange.Core.StringResources;

namespace SiteFuel.Exchange.Api.Mobile.Common
{
    public class CommonMethods
    {
        public ResponseViewModel GetErrorMessage(ModelStateDictionary modelState)
        {
            ResponseViewModel response = new ResponseViewModel(Status.Success);

            foreach (ModelState state in modelState.Values)
            {
                if (response.StatusCode != Status.Failed) //to get only first error to display
                {
                    foreach (ModelError error in state.Errors)
                    {
                        if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                        {
                            response.StatusCode = Status.Failed;
                            response.StatusMessage = error.ErrorMessage;
                            break;
                        }
                    }
                }
            }

            return response;
        }


        public bool ValidateDate(DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.Date >= DateTime.Now.Date;
        }

        public ResponseViewModel GetDateValidationResponse()
        {
            return new ResponseViewModel()
            {
                StatusCode = Status.Failed,
                StatusMessage = Resource.errMessageDateCannotBeLessThanTodaysDate
            };
        }
    }
}