using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Add {module}",
            $"View {module}",
            $"Edit {module}",
            $"Delete {module}",
        };
        }
     
        public static class Organization
        {
            //Organizations
            public const string ViewOrganization = "View Organization";
            public const string CreateOrganization = "Add Organization";
            public const string EditOrganization = "Edit Organization";
            public const string DisableOrganization = "Disable Organization";
            public const string DeleteOrganization = "Delete Organization";
        }
        public static class User
        {
            //Users
            public const string ViewUser = "View User";
            public const string CreateUser = "Add User";
            public const string EditUser = "Edit User";
            public const string DisableUser = "Disable User";
            public const string DeleteUser = "Delete User";
        }
        public static class Configuration
        {
            //Configurations
            public const string ViewConfiguration = "View Configuration";
            public const string CreateConfiguration = "Add Configuration";
            public const string EditConfiguration = "Edit Configuration";
            public const string DisableConfiguration = "Disable Configuration";
            public const string DeleteConfiguration = "Delete Configuration";
        }
        public static class Report
        {
            //Reports
            public const string ViewReport = "View Report";
            public const string CreateReport = "Add Report";
            public const string EditReport = "Edit Report";
            public const string DisableReport = "Disable Report";
            public const string DeleteReport = "Delete Report";
        }
        public static class Account
        {
            //Accounts
            public const string ViewAccount = "View Account";
            public const string CreateAccount = "Add Account";
            public const string EditAccount = "Edit Account";
            public const string DisableAccount = "Disable Account";
            public const string DeleteAccount = "Delete Account";
        }
        public static class Market
        {
            //Markets
            public const string ViewMarket = "View Market";
            public const string CreateMarket = "Add Market";
            public const string EditMarket = "Edit Market";
            public const string DisableMarket = "Disable Market";
            public const string DeleteMarket = "Delete Market";
        }
        public static class LoanRequest
        {
            //LoanRequests
            public const string ViewLoanRequest = "View LoanRequest";
            public const string CreateLoanRequest = "Add LoanRequest";
            public const string EditLoanRequest = "Edit LoanRequest";
            public const string DisableLoanRequest = "Disable LoanRequest";
            public const string DeleteLoanRequest = "Delete LoanRequest";

        }
        public static class Customer
        {
            //Customers
            public const string ViewCustomer = "View Customer";
            public const string CreateCustomer = "Add Customer";
            public const string EditCustomer = "Edit Customer";
            public const string DisableCustomer = "Disable Customer";
            public const string DeleteCustomer = "Delete Customer";
        }
        public static class Guarantor
        {
            //Guarantors
            public const string ViewGuarantor = "View Guarantor";
            public const string CreateGuarantor = "Add Guarantor";
            public const string EditGuarantor = "Edit Guarantor";
            public const string DisableGuarantor = "Disable Guarantor";
            public const string DeleteGuarantor = "Delete Guarantor";
        }


        public static class SalesPermissions
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class PowerUserPermissions
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }
        public static class SupportPermissions
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
        }

    }
}