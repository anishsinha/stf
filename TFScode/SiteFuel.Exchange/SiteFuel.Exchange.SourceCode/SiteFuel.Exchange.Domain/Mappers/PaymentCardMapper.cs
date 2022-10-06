using SiteFuel.Exchange.DataAccess.Entities;
using SiteFuel.Exchange.Utilities;
using SiteFuel.Exchange.ViewModels;
using Stripe;

namespace SiteFuel.Exchange.Domain.Mappers
{
    public static class PaymentCardMapper
    {
        public static PaymentViewModel ToPaymentViewModel(this CompanyXStripeCard entity, PaymentViewModel viewModel = null)
        {
            if (viewModel == null)
                viewModel = new PaymentViewModel(Status.Success);

            viewModel.Id = entity.Id;
            viewModel.UserId = entity.UserId;
            viewModel.CompanyId = entity.CompanyId;
            viewModel.CardToken = entity.StripeCardToken;
            viewModel.AddedBy = $"{entity.User.FirstName} {entity.User.LastName}";

            string stripeSecretKey = string.Empty;
            var environment = (ApplicationEnvironment)ConfigHelperMethods.GetConfigSetting(ApplicationConstants.Environment).GetValue<int>();
            if (environment == ApplicationEnvironment.Prod)
            {
                stripeSecretKey = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingLiveStripePrivateKey);
            }
            else
            {
                stripeSecretKey = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingTestStripePrivateKey);
            }

            var tokenService = new StripeTokenService(stripeSecretKey);
            StripeToken token = tokenService.Get(entity.StripeCardToken);

            viewModel.Card = new PaymentCardViewModel
            {
                NameOnCard = token.StripeCard.Name,
                CardNumber = $"{"XXXXXXXXXXXX"}{token.StripeCard.Last4}",
                IsPrimary = entity.IsPrimary
            };

            viewModel.Address = token.StripeCard.AddressLine1;
            viewModel.City = token.StripeCard.AddressCity;
            viewModel.State = new StateViewModel(Status.Success) { Code = token.StripeCard.AddressState };
            viewModel.Country = new CountryViewModel(Status.Success) { Code = token.StripeCard.AddressCountry };
            viewModel.ZipCode = token.StripeCard.AddressZip;

            return viewModel;
        }

        public static CompanyXStripeCard ToCompanyUserXStripeCardEntity(this PaymentViewModel viewModel, CompanyXStripeCard entity = null)
        {
            if (entity == null)
                entity = new CompanyXStripeCard();

            var tokenOptions = new StripeTokenCreateOptions();
            tokenOptions.Card = new StripeCreditCardOptions()
            {
                Name = viewModel.Card.NameOnCard,
                Number = viewModel.Card.CardNumber,
                Cvc = viewModel.Card.Cvv,
                ExpirationMonth = viewModel.Card.ExpirationMonth,
                ExpirationYear = viewModel.Card.ExpirationYear,
                AddressLine1 = viewModel.Address,
                AddressCity = viewModel.City,
                AddressState = viewModel.State.Code,
                AddressCountry = viewModel.Country.Code,
                AddressZip = viewModel.ZipCode,
            };

            string stripeSecretKey = string.Empty;
            var environment = (ApplicationEnvironment)ConfigHelperMethods.GetConfigSetting(ApplicationConstants.Environment).GetValue<int>();
            if (environment == ApplicationEnvironment.Prod)
            {
                stripeSecretKey = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingLiveStripePrivateKey);
            }
            else
            {
                stripeSecretKey = ApplicationDomain.ApplicationSettings.GetApplicationSettingValue<string>(ApplicationConstants.KeyAppSettingTestStripePrivateKey);
            }

            var tokenService = new StripeTokenService(stripeSecretKey);
            var stripeToken = tokenService.Create(tokenOptions);

            entity.UserId = viewModel.UserId;
            entity.CompanyId = viewModel.CompanyId;
            entity.StripeCardToken = stripeToken.Id;
            entity.IsPrimary = viewModel.Card.IsPrimary;

            return entity;
        }
    }
}
