using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.WorkFlowService.Domain.Enums
{
    //global to Application
    public enum TestPermissions
    {
        //Organization Module Permissions
        CreateOrganization,
        EditOrganization,
        ViewOrganization,
        DisableOrganization,
        DeleteOrganization,
        //Staff Module Permissions
        CreateStaff,
        EditStaff,
        ViewStaff,
        DisableStaff,
        DeleteStaff,
        //Configuration Module Permissions
        EditConfiguration,
        ViewConfiguration,
        DisableConfiguration,
        //Reports Module Permissions
        ViewReports,
        //Accounts Module Permissions
        ViewAccount,
        AddAccount,
        //Customers Module Permissions
        AddCustomers,
        EditCustomers,
        ViewCustomers,
        DisableCustomers,
        AddGuarantors,
        EditGuarantors,
        ViewGuarantors,
        DisableGuarantors,
        //Market Permissions
        AddMarkets,
        EditMarkets,
        ViewMarkets,
        DisableMarkets,
        //Loan Request Permissions
        AddLoanRequest,
        EditLoanRequest,
        ViewLoanRequest,
        ApproveLoanRequest,
        RejectLoanRequest,
        ProcessLoanRequest,
        ReturnLoanRequest,
        DisburseLoan,
        //Loan RepaymentPermissions
        AddLoanRepayment,
        EditLoanRepayment,
        ViewLoanRepayment,
        ApproveLoanRepayment,
        RejectLoanRepayment,


    }
   public enum SuperAdminPermissions
    {
      CreateOrganization,
      EditOrganization,
      ViewOrganization,
      DisableOrganization,
      CreateStaff,
      EditStaff,
      ViewStaff,
      DisableStaff,
      EditConfiguration,
      ViewConfiguration,
      DisableConfiguration,
      ViewReports,
      ViewAccounts, 
      ViewCustomers,
      ViewGuarantors,
      ViewMarkets,
    }

    //within application
    public enum AdminPermissions
    {
        CreateOrganization,
        EditOrganization,
        ViewOrganization,
        DisableOrganization,
        CreateStaff,
        EditStaff,
        ViewStaff,
        DisableStaff,
        EditConfiguration,
        ViewConfiguration,
        DisableConfiguration,
        ViewReports,
        ViewAccounts,
        ViewCustomers,
        ViewGuarantors,
        ViewMarkets,
    }

    public enum SalesPermissions
    {
        ViewOrganization,
        ViewStaff,
        ViewConfiguration,
        CreateLoan,
        EditLoan,
        ViewLoan,
        ViewCustomers,
        ViewGuarantors,
        CreateLoanRepayment,
        EditLoanRepayment,
        ViewReports,
        ViewAccounts,
        ViewMarkets,
    }
    public enum PowerUserPermissions
    {
        AddAccount,
        ViewAccounts,
        ViewOrganization,
        ViewStaff,
        EditConfiguration,
        ViewConfiguration,
        ViewLoan,
        ViewCustomers,
        ViewGuarantors,
        AddMarket,
        ViewMarkets,
        ApproveLoanRequest,
        RejectLoanRequest,
        DisburseLoan,
        ApproveLoanRepayment,
        RejectLoanRepayment,
        ViewReports
       
    }

    public enum SupportPermissions
    {
        
        ViewOrganization,
        ViewStaff,
        ViewConfiguration,
        ViewLoan,
        ViewReports,
        ViewCustomers,
        ViewGuarantors,
        ViewMarkets
    }
}
