using System;
using System.Web.Optimization;

namespace SiteFuel.Exchange.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //Bundle Styles
            bundles.Add(new StyleBundle("~/Content/css/common")
               .Include(
               "~/Content/css/master-empty.css",
               "~/Content/css/bootstrap_4.3.1.min.css",
               //"~/Content/css/bootstrap.min.css",
               "~/Content/css/bootstrap-datetimepicker.min.css",
               "~/Content/css/font-awesome.min.css",
               "~/Content/css/glyphicon.css",
               "~/Content/css/jquery-ui.css",
               "~/Content/css/datatables.min.css",
               "~/Content/css/site.css",
               "~/Content/css/style.css",
               "~/Content/css/animate.css",
               "~/Content/css/master-dashboard.css",
               "~/Content/css/bootstrap-multiselect.css",
               "~/Content/css/jquery.toast.min.css",
               "~/Content/css/flags.css",
               "~/Content/css/fixedHeader.dataTables.min.css",
               "~/Content/css/select2.min.css",
               "~/Content/css/multi-email.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                //"~/Content/css/bootstrap.min.css"
                "~/Content/css/bootstrap_4.3.1.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/css/flags").Include(
               "~/Content/css/flags.css"));

            bundles.Add(new StyleBundle("~/Content/css/toast").Include(
               "~/Content/css/jquery.toast.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap-datetimepicker").Include(
                "~/Content/css/bootstrap-datetimepicker.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap-multiselect").Include(
                "~/Content/css/bootstrap-multiselect.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/font-awesome").Include(
                "~/Content/css/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/fullcalendar").Include(
                "~/Content/css/fullcalendar.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/master-dashboard").Include(
                "~/Content/css/master-dashboard.css"));

            bundles.Add(new StyleBundle("~/Content/css/master-empty").Include(
                "~/Content/css/master-empty.css"));

            bundles.Add(new StyleBundle("~/Content/css/master-error").Include(
                "~/Content/css/master-error.css"));

            bundles.Add(new StyleBundle("~/Content/css/master-onboarding").Include(
                "~/Content/css/master-onboarding.css"));

            bundles.Add(new StyleBundle("~/Content/css/select2").Include(
                "~/Content/css/select2.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/site").Include(
                "~/Content/css/site.css",
                "~/Content/css/style.css",
                "~/Content/css/animate.css"));

            bundles.Add(new StyleBundle("~/Content/css/datatables").Include(
                "~/Content/css/datatables.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/gridmvc").Include(
                "~/Content/css/gridmvc.css"));

            bundles.Add(new StyleBundle("~/Content/css/summernotes").Include(
                "~/Content/css/summernote.css",
                "~/Content/css/summernote-bs3.css"));

            bundles.Add(new StyleBundle("~/Content/css/icheck").Include(
                "~/Content/css/icheck.css"));

            bundles.Add(new StyleBundle("~/Content/css/multiple-emails").Include(
                "~/Content/css/multiple-emails.css"));

            bundles.Add(new StyleBundle("~/Content/css/deliverygroup").Include(
                "~/Content/css/delivery-group.css"));

            bundles.Add(new StyleBundle("~/Content/css/px-pagination").Include(
                "~/Content/css/px-pagination.css"));

            bundles.Add(new StyleBundle("~/Content/css/primeng").Include(
               "~/Content/css/primeicons.css",
               "~/Content/css/primeng.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/jquery-ui").Include(
                "~/Content/css/jquery-ui.css"));

            //Bundle Scripts
            bundles.Add(new StyleBundle("~/Content/js/jquery-ui").Include(
               "~/Content/js/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/Content/js/common").Include(
                //"~/content/js/jquery-{version}.js",
                "~/Content/js/jquery-3.4.1.min.js",
                "~/Content/js/popper.min.js",
                "~/Content/js/jquery.validate.min.js",
                "~/Content/js/jquery.validate.unobtrusive.min.js",
                "~/Content/js/mvcfoolproof.unobtrusive.min.js",
                "~/Content/js/requiredif-multiple.js",
                "~/Content/js/validate-email.js",
                "~/Content/js/validate-greaterthanzeroif.js",
                "~/Content/js/jquery-ui.js",
                //"~/Content/js/bootstrap.min.js",
                "~/Content/js/bootstrap_4.3.1.min.js",
                "~/Content/js/respond.min.js",
                "~/Content/js/moment.min.js",
                "~/Content/js/moment-with-locales.min.js",
                "~/Content/js/bootstrap-datetimepicker.min.js",
                //"~/Content/js/bootstrap-confirmation.min.js",
                "~/Content/js/bootstrap-confirmation_4.min.js",
                "~/Content/js/jquery.cookie.js",
                "~/Content/js/jquery.toast.min.js",
                "~/Content/js/jquery.flagstrap.min.js",
                "~/Content/js/site.js",
                "~/Content/js/master-dashboard.js",
                "~/Content/js/datatables.min.js",
                "~/Content/js/datatables.filter.js",
                "~/Content/js/datatables.sorting.time.js",
                "~/Content/js/datatables.customizestate.js",
                "~/Content/js/datatables.hide.empty.js",
                "~/Content/js/dataTables.fixedHeader.min.js",
                "~/Content/js/buttons.colVis.min.js",
                "~/Content/js/datatables.columnConfig.js",
                "~/Content/js/select2.min.js",
                "~/Content/js/bootstrap-multiselect.js",
                "~/Content/js/Custom/Breadcrumbs.js",
                "~/Scripts/Main/Utilities.js",
                "~/Content/js/Chart.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Content/js/address-goecoder").Include(
                "~/Content/js/address-goecoder.js",
                "~/Scripts/Main/Job/StateAutofill.js",
                "~/Scripts/Main/Job/zipvalidation.js"));

            bundles.Add(new ScriptBundle("~/Content/js/dispatch").Include(
                            "~/Content/js/address-goecoder.js",
                            "~/Scripts/Main/Job/StateAutofill.js",
                            "~/Scripts/Main/Job/zipvalidation.js",
                            "~/Content/js/common-supplier.js",
                            "~/Scripts/Main/Dispatch/Dispatch.js",
                            "~/Scripts/Main/Dispatch/dispatchCommon.js",
                            "~/Scripts/Main/Dispatch/deliveryGroup.js",
                            "~/Scripts/Main/DeliverySchedules/DeliverySchedule.js"));

            bundles.Add(new ScriptBundle("~/Content/js/stateautofill").Include(
                "~/Scripts/Main/Job/StateAutofill.js",
                "~/Scripts/Main/Job/zipvalidation.js"));

            bundles.Add(new ScriptBundle("~/Content/js/fullcalendar").Include(
                "~/Content/js/fullcalendar.min.js"));

            bundles.Add(new ScriptBundle("~/Script/Main/fuelSurcharge").Include(
                "~/Scripts/Main/FuelSurcharge/FuelSurcharge.js"));

            bundles.Add(new ScriptBundle("~/Script/Main/fuelSurchargeView").Include(
                "~/Scripts/Main/FuelSurcharge/FuelSurchargeView.js"));

            bundles.Add(new ScriptBundle("~/Script/Main/Order").Include(
                "~/Scripts/Main/Order/CreateOrder.js"));

            bundles.Add(new ScriptBundle("~/Script/Main/Email").Include(
               "~/Scripts/Main/Email/MultiEmail.js"));

            bundles.Add(new ScriptBundle("~/Script/Main/Job").Include(
                "~/Scripts/Main/Job/JobMap.js"));

            bundles.Add(new ScriptBundle("~/Content/js/toast").Include(
                "~/Content/js/jquery.toast.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/bootbox").Include(
               "~/Content/js/bootbox.js"));

            bundles.Add(new ScriptBundle("~/Content/js/bootstrap-confirmation").Include(
                //"~/Content/js/bootstrap-confirmation.min.js"
                "~/Content/js/bootstrap-confirmation_4.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Content/js/bootstrap").Include(
                //"~/content/js/bootstrap.min.js",
                "~/Content/js/bootstrap_4.3.1.min.js",
                "~/Content/js/respond.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/bootstrap-datetimepicker").Include(
                "~/Content/js/moment.min.js",
                "~/Content/js/moment-with-locales.min.js",
                "~/Content/js/bootstrap-datetimepicker.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/bootstrap-multiselect").Include(
                "~/Content/js/bootstrap-multiselect.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/image").Include(
                "~/Content/js/image.js"));

            bundles.Add(new ScriptBundle("~/Content/js/webnotification").Include(
                "~/Content/js/webnotification.js"));

            bundles.Add(new ScriptBundle("~/Content/js/scheduleLocation").Include(
                "~/Content/js/common-supplier.js",
                "~/Scripts/Main/DeliverySchedules/DeliverySchedule.js"
                 ));

            bundles.Add(new ScriptBundle("~/Content/js/jquery").Include(
                //"~/Content/js/jquery-{version}.js",
                "~/Content/js/jquery-3.4.1.min.js",
                "~/Content/js/popper.min.js",
                "~/Content/js/jquery.validate.min.js",
                "~/Content/js/jquery.validate.unobtrusive.min.js",
                "~/Content/js/mvcfoolproof.unobtrusive.min.js",
                "~/Content/js/requiredif-multiple.js",
                "~/Content/js/validate-email.js",
                "~/Content/js/validate-greaterthanzeroif.js"));

            bundles.Add(new ScriptBundle("~/Content/js/partial-collection").Include(
                "~/Content/js/partial.collection.js"));

            bundles.Add(new ScriptBundle("~/Scripts/js/news-feed").Include(
                "~/Scripts/Main/Newfeed/NewsfeedModule.js"));

            bundles.Add(new ScriptBundle("~/Content/js/quantityrange-validate").Include(
                "~/Content/js/quantityrange.validate.js"));

            bundles.Add(new ScriptBundle("~/Content/js/billingstatement").Include(
                "~/Content/js/billingstatement.js"));

            bundles.Add(new ScriptBundle("~/Content/js/jquery-pwstrength").Include(
                "~/Content/js/jquery.pwstrength.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/jquery-mask").Include(
                "~/Content/js/jquery.mask.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/jquery-maskMoney").Include(
                "~/Content/js/jquery.maskMoney.min.js"));

            bundles.Add(new StyleBundle("~/Content/js/master-dashboard").Include(
                "~/Content/js/master-dashboard.js"));

            bundles.Add(new StyleBundle("~/Content/js/moment").Include(
                "~/Content/js/moment.min.js",
                "~/Content/js/moment-with-locales.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/select2").Include(
                "~/Content/js/select2.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/site").Include(
                "~/Content/js/jquery.cookie.js",
                "~/Content/js/site.js",
                "~/Content/js/jquery.flagstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/datatables").Include(
                "~/Content/js/datatables.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/datatablesExternal").Include(
                "~/Content/js/datatables.min.js",
                "~/Content/js/datatables.filter.js",
                "~/Content/js/datatables.sorting.time.js",
                "~/Content/js/datatables.customizestate.js",
                "~/Content/js/datatables.hide.empty.js",
                "~/Content/js/dataTables.fixedHeader.min.js",
                "~/Content/js/buttons.colVis.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/unobtrusive-ajax").Include(
                "~/Content/js/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/gridmvc").Include(
                "~/Content/js/gridmvc.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/summernotes").Include(
                "~/Content/js/summernote.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/icheck").Include(
                "~/Content/js/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/Content/js/jquery-print").Include(
                "~/Content/js/jquery.print.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/Dashboard")
               .Include("~/Scripts/Main/Dashboard/DashboardGallonStat.js",
               "~/Scripts/Main/Dashboard/DashboardDropAverages.js",
               "~/Scripts/Main/Dashboard/DashboardCalendar.js",
               "~/Scripts/Main/Dashboard/CalendarEvents.js",
               "~/Scripts/Main/Dashboard/Base/dashboard-base.js"
               ));

            bundles.Add(new ScriptBundle("~/Scripts/Main/DeliverySchedules")
                .Include("~/Scripts/Main/DeliverySchedules/DriverNotification.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/BuyerDashboard")
               .Include("~/Scripts/Main/Dashboard/BuyerDashboardJobAvg.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/GlobalFuelCost")
              .Include("~/Scripts/Main/GlobalFuelCost/DashboardGlobalFuelCost.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/Invoice")
              .Include("~/Scripts/Main/Invoice/AssetDrop.js")
              .Include("~/Scripts/Main/Invoice/Invoice.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/InvoiceDetail")
              .Include("~/Scripts/Main/Invoice/InvoiceDetail.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/ServingStateAutofill")
              .Include("~/Scripts/Main/CompanyAddress/ServingStateAutofill.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/NextDeliverySchedules")
              .Include("~/Scripts/Main/DeliverySchedules/NextDeliverySchedules.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/Offer")
              .Include("~/Scripts/Main/Offer/Offers.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/Dashboard/Base").Include(
               "~/Scripts/Main/Dashboard/Base/dashboard-base.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Main/Ftl")
              .Include("~/Scripts/Main/Ftl/Ftl.js"));

            bundles.Add(new ScriptBundle("~/Content/js/multiple-emails").Include(
               "~/Content/js/multiple-emails.js"));

            bundles.Add(new ScriptBundle("~/Content/js/pricing").Include(
                "~/Content/js/pricing.js"));

            bundles.Add(new ScriptBundle("~/Content/js/px-pagination").Include(
                "~/Content/js/px-pagination.js"));

            bundles.Add(new ScriptBundle("~/Scripts/SendBird/SendBird").Include(
              "~/Scripts/SendBird/SendBird.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Angular8").Include(
               "~/Scripts/libs/runtime-es5.*",
               "~/Scripts/libs/polyfills-es5.*",
               //"~/Scripts/libs/styles-es5.*",
               //"~/Scripts/libs/vendor-es5.*",
               "~/Scripts/libs/main-es5.*"
               ));

            bundles.Add(new ScriptBundle("~/Content/js/supplierbranding").Include(
            "~/Content/js/supplierbranding.js"));
        }
    }
}
