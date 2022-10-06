using SiteFuel.Exchange.Logger;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Lookups.V1;

namespace SiteFuel.Exchange.SmsManager
{
    public class Sms
    {
        public Sms()
        {
        }

        public static Sms GetClient()
        {
            return new Sms();
        }

        public string Send(ApplicationEventSmsNotificationViewModel model)
        {
            var response = string.Empty;
            try
            {
                TwilioClient.Init(model.TwilioAccountSid, model.TwilioAuthToken);
                model.Url = model.Url + "Sms/UpdateSmsStatus";
                foreach (var sendTo in model.To)
                {
                    var sendPhoneNumber = GetFormattedPhoneNumber(sendTo);
                    var smsResponse = MessageResource.Create(
                        body: model.SmsText,
                        from: new Twilio.Types.PhoneNumber(model.TwilioFromPhoneNumber),
                        statusCallback: new Uri(model.Url),
                        to: new Twilio.Types.PhoneNumber(sendPhoneNumber)
                    );

                    response = smsResponse.Sid;

                    LogManager.Logger.WriteDebug("Sms", "Send", "SmsSid: " + smsResponse.Sid + " Url: " + model.Url + "");
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("Sms", "Send", ex.Message, ex);
            }
            return response;
        }

        public bool IsPhoneNumberValid(ApplicationEventSmsNotificationViewModel model)
        {
            var response = false;
            try
            {
                TwilioClient.Init(model.TwilioAccountSid, model.TwilioAuthToken);
                foreach (var sendTo in model.To)
                {
                    var sendPhoneNumber = GetFormattedPhoneNumber(sendTo);
                    var phoneNumber = PhoneNumberResource.Fetch(
                        pathPhoneNumber: new Twilio.Types.PhoneNumber(sendPhoneNumber)
                    );

                    //var phoneNumber = PhoneNumberResource.Fetch(
                    //        countryCode: "US",
                    //        pathPhoneNumber: new Twilio.Types.PhoneNumber(sendTo)
                    //    );

                    response = true;
                    LogManager.Logger.WriteDebug("Sms", "IsPhoneNumberValid", "PhoneNumber: " + phoneNumber.PhoneNumber + " Url: " + phoneNumber.Url + "");
                }
            }
            catch
            {
                //LogManager.Logger.WriteException("Sms", "IsPhoneNumberValid", ex.Message, ex);
            }
            return response;
        }

        private string GetFormattedPhoneNumber(string phoneNumber)
        {
            string formattedPhoneNumber = phoneNumber.Replace("-", "");
            formattedPhoneNumber = formattedPhoneNumber.Replace("(", "");
            formattedPhoneNumber = formattedPhoneNumber.Replace(")", "");
            formattedPhoneNumber = formattedPhoneNumber.Replace(".", "");
            formattedPhoneNumber = formattedPhoneNumber.Replace(" ", "");
            return formattedPhoneNumber;
        }
    }
}

