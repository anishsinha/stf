using SiteFuel.Exchange.Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SiteFuel.Exchange.Utilities
{
    public static class EnumHelperMethods
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            string response = string.Empty;
            try
            {
                var enumMember = enumValue.GetType()
                                    .GetMember(enumValue.ToString())
                                    .FirstOrDefault();

                if(enumMember != null)
                {
                   response = enumMember.GetCustomAttribute<DisplayAttribute>()?
                                    .GetName() ?? enumValue.ToString();
                }
                else
                {
                    response = enumValue.ToString();
                }
                                    
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("EnumHelperMethods", "GetDisplayName", ex.Message, ex);
            }
            return response;
        }

        public static string GetDisplayShortName(this Enum enumValue)
        {
            string response = string.Empty;
            try
            {
                var enumMember = enumValue.GetType()
                                    .GetMember(enumValue.ToString())
                                    .FirstOrDefault();

                if (enumMember != null)
                {
                    response = enumMember.GetCustomAttribute<DisplayAttribute>()?
                                    .GetShortName() ?? enumValue.ToString();
                }
                else
                {
                    response = enumValue.ToString();
                }

            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("EnumHelperMethods", "GetDisplayShortName", ex.Message, ex);
            }
            return response;
        }

        public static List<DropdownDisplayItem> EnumToList<T>()
        {
            var response = new List<DropdownDisplayItem>();
            try
            {
                var enumType = typeof(T);
                foreach (var item in Enum.GetValues(typeof(T)))
                {
                    var ddl = new DropdownDisplayItem();

                    var member = enumType.GetMember(item.ToString());
                    var attributes = member[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                    var displayName = attributes.Length > 0 ? ((DisplayAttribute)attributes[0]).Name : item.ToString();

                    if (displayName.ToLower() != "none" && displayName.ToLower() != "unknown")
                    {
                        ddl.Id = (int)item;
                        ddl.Name = displayName;
                        response.Add(ddl);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.Logger.WriteException("EnumHelperMethods", "EnumToList", ex.Message, ex);
            }

            return response;
        }

        public static ProductCategory GetProductCategoryType(this int value)
        {
            if (value == (int)ProductTypes.Unleaded || value == (int)ProductTypes.ConventionalGas || value == (int)ProductTypes.MidgradeGas || value == (int)ProductTypes.PremiumGas || value == (int)ProductTypes.RegularGas || value == (int)ProductTypes.OtherGas)
                return ProductCategory.Gasoline;
            else if (value == (int)ProductTypes.ClearDiesel || value == (int)ProductTypes.ClearDiesel2 || value == (int)ProductTypes.RedDyeDiesel || value == (int)ProductTypes.RedDyeDiesel2)
                return ProductCategory.Diesel;

            return ProductCategory.None;
        }

        public static int GetActualStatus(this int value)
        {
            switch (value)
            {
                case 1:
                    return (int)EnrouteDeliveryStatus.OnTheWayToJob;

                case 2:
                    return (int)EnrouteDeliveryStatus.OnTheWayToTerminal;

                case 3:
                    return (int)EnrouteDeliveryStatus.ArrivedAtTerminal;

                case 4:
                    return (int)EnrouteDeliveryStatus.ArrivedAtJob;

                case 5:
                    return (int)EnrouteDeliveryStatus.FuelTruckRetain;

                default:
                    break;
            }
            

            return (int)EnrouteDeliveryStatus.Unknown;
        }
    }
}
