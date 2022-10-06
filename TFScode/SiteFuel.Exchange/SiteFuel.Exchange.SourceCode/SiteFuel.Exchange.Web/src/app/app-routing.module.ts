import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    {
        path: "Settings/Profile",
        loadChildren: () => import('./company-addresses/lazy-loading/company-addresses.module').then(mod => mod.CompanyAddressesModule)
    },
    {
        path: "Supplier/Region",
        loadChildren: () => import('./company-addresses/lazy-loading/company-addresses.module').then(mod => mod.CompanyAddressesModule)
    },
    {
        path: "SuperAdmin/SuperAdmin",
        loadChildren: () => import('./company-addresses/lazy-loading/company-addresses.module').then(mod => mod.CompanyAddressesModule)
    },
    {
        path: 'Supplier/Invoice',
        loadChildren: () => import('./invoice/lazy-loading/invoice.module').then(mod => mod.InvoiceModule)
    },
    {
        path: "Supplier/OrderGroup/View",
        loadChildren: () => import('./order/lazy-loading/order.module').then(mod => mod.OrderModule)
    },
    {
        path: "Buyer/OrderGroup/View",
        loadChildren: () => import('./order/lazy-loading/order.module').then(mod => mod.OrderModule)
    },
    {
        path: "Carrier/Dashboard",
        loadChildren: () => import('./carrier/carrier.module').then(mod => mod.CarrierModule)
    },
    {
        path: "Carrier/ScheduleBuilder",
        loadChildren: () => import('./carrier/carrier.module').then(mod => mod.CarrierModule)
    },
    {
        path: "Carrier/ScheduleBuilder/Index",
        loadChildren: () => import('./carrier/carrier.module').then(mod => mod.CarrierModule)
    },
    {
        path: "Carrier/ScheduleBuilder/DeliveryRequests",
        loadChildren: () => import('./delivery-request-display/delivery-request-display.module').then(mod => mod.DeliveryRequestDisplayModule)
    },
    {
        path: "Carrier/SelfServiceAlias/View",
        loadChildren: () => import('./self-service-alias/self-service-alias.module').then(mod => mod.SelfServiceAliasModule)
    },
    {
        path: "Carrier/Tractor/View",
        loadChildren: () => import('./tractor/tractor.module').then(mod => mod.TractorModule)
    },
    {
        path: "Carrier/Freight/View",
        loadChildren: () => import('./trailer/trailer.module').then(mod => mod.TrailerModule)
    },
    {
        path: "Dispatcher",
        loadChildren: () => import('./dispatcher/dispatcher.module').then(mod => mod.DispatcherModule)
    },
    {
        path: "Settings/Profile/CarrierCompanies",
        loadChildren: () => import('./carrier-companies/lazy-loading/carrier-companies.module').then(mod => mod.CarrierCompaniesModule)
    },
    {
        path: "Supplier/Job/View",
        loadChildren: () => import('./carrier-companies/lazy-loading/carrier-companies.module').then(mod => mod.CarrierCompaniesModule)
    },
    //driver Schedule  
    {
        path: "Settings/Profile/DriverManagement",
        loadChildren: () => import('./driver/driver.module').then(mod => mod.DriverModule)
    },
    {
        path: "Home/TPDAPIDashboard",
        loadChildren: () => import('./tpd-api-dashboard/tpd-api-dashboard.module').then(mod => mod.TpdApiDashboardModule)
    },
    {
        path: "Supplier/Location",
        loadChildren: () => import('./location/lazy-loading/location.module').then(mod => mod.LocationModule)
    },
    {
        path: "Buyer/Job/BuyerWallyBoard",
        loadChildren: () => import('./buyer-wally-board/buyer-wally-board.module').then(mod => mod.BuyerWallyBoardModule)
    },
    {
        path: "SuperAdmin/SuperAdmin/GetTerminalSupplier",
        loadChildren: () => import('./terminalSupplierMaster/terminal-supplier-master.module').then(mod => mod.TerminalSupplierMasterModule)
    },
    {
        path: "Buyer/Dashboard",
        loadChildren: () => import('./buyer-dashboard/buyer-dashboard.module').then(mod => mod.BuyerDashboardModule)
    },
    {
        path: "Carrier/ScheduleBuilder/DeliveryRequestsReport",
        loadChildren: () => import('./carrier/delivery-request-report/delivery-request-report.module').then(mod => mod.DeliveryRequestReportModule)
    },
    {
        path: "Supplier/LiftFile",
        loadChildren: () => import('./lfv-dashboard/lfv-dashboard.module').then(mod => mod.LfvDashboardModule)
    },
    {
        path: "Supplier/FuelSurcharge",
        loadChildren: () => import('./fuelsurcharge/fuelsurcharge.module').then(mod => mod.FuelsurchargeModule)
    },
    {
        path: "Supplier/FreightRate",
        loadChildren: () => import('./freightRates/freight.rate.module').then(mod => mod.FreightRateRulesModule)
    },
    {
        path: "SalesUser",
        loadChildren: () => import('./sales-user/sales-user.module').then(mod => mod.SalesUserModule)
    },   
    {
        path: "Supplier/AccessorialFees",
        loadChildren: () => import('./accessorial-fees/accessorial-fees.module').then(mod => mod.AccessorialFeesModule)
    },
    {
        path: "SuperAdmin/SuperAdmin/MarinePortsAndVessels",
        loadChildren: () => import('./marine-ports-vessels/marine-ports-vessels.module').then(m => m.MarinePortsVesselsModule)
    },
    {
        path: "Supplier/Job/MarinePortsAndVessels",
        loadChildren: () => import('./marine-ports-vessels/marine-ports-vessels.module').then(m => m.MarinePortsVesselsModule)
    },
    {
        path: "Invitation",
        loadChildren: () => import('./invitation/invitation.module').then(mod => mod.InvitationModule)
    },
    {
        path: "Supplier/ThirdPartyNetwork",
        loadChildren: () => import('./third-party-network/third-party-network.module').then(mod => mod.ThirdPartyNetworkModule)
    },
    {
        path: "Carrier/Calendar",
        loadChildren: () => import('./calendar/tfcalendar.module').then(mod => mod.TfcalendarModule)
    },
    {
        path: "SuperAdmin/SuperAdmin/CreateTerminals",
        loadChildren: () => import('./superadmin-create-terminals/superadmin-create-terminal.module').then(m => m.SuperadminCreateTerminalModule)
    },
    {
        path: "Supplier/FuelGroup/Create",
        loadChildren: () => import('./create-fuel-group/create-fuel-group.module').then(mod => mod.CreateFuelGroupModule)
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes, { enableTracing: false })
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
