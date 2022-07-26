using System;
using System.Collections.Generic;
using System.Text;

namespace Onyx.AuthService.Domain.Enums
{
    //global to Application
    public enum TestPermissions
    {
        ViewOrganizationProfile,
        EditOrganizationProfile,
        ViewAdminProfile,
        EditAdminProfile,
        ViewUserList,
        EditUser,
        CreateUser,
        ActivateUser,
        DeactivateUser,
        ViewConfiguration,
        EditConfiguration,
        CreateConfiguration,
     
    }


    public enum SuperAdminPermissions
    {
        ViewSuperAdminProfile,
        EditSuperAdminProfile,
        SetUpRoles,
        SetUpPermissions,
        ViewOrganizationList,
        ViewSubscriptionsList,
        ViewOrganization,
        EditOrganization,
        AtivateOrganization,
        DeactivateOrganizaion,
        ViewDashBoard,
        CreateOrganization,
        SetUpConfiguration,
        ViewConfiguration,
        EditConfiguration,
        ViewSettings,
        ViewReport,
        ExportReport,
        ViewAudit,
        ExportAudit,
        HelpSupportFeatures,
        ViewAllContractRequests,
        ViewOnlyMyContractRequests,
        ViewAllApprovalRequests,
        ViewOnlyMyApprovalRequests,
        ViewAllExecutedContacts,
        ViewOnlyMyExecutedContracts,
        //ViewOnlyMyExecutedPermits,
        //ViewAllExecutedPermits

    }

    public enum AdminPermissions
    {
        ResetPassword,
        ViewDashboard,
        CreateOrganization,
        EditOrganization,
        ViewOrganization,
        DisableOrganization,
        EditProfile,
        CreateUser,
        EditUser,
        ActivateUser,
        DeactivateUser,
        ViewUserList,
        CreateRoles,
        EditRoles,
        ViewRoles,
        ViewContract,
        InitiateContract,
        GenerateContract,
        ShareContract,
        EditContract,
        CreateDocumentTag,
        ShareRequest,
        ApproveAndRejectContractRequest,
        ViewMail,
        ViewExecutedContract,
        CreateExecutedContract,
        EditExecutedContract,
        ViewExecutedPermit,
        CreateExecutedPermit,
        EditExecutedPermit,
        RenewContract,
        TerminateContract,
        SignAndAcceptContract,
        Comment,
        ViewConfiguration,
        EditConfiguration,
        CreateConfiguration,
        ViewSettings,
        ViewAudit,
        ViewReport,
        ExportReport,
        ExportAudit,
        ViewAllContractRequests,
        ViewOnlyMyContractRequests,
        ViewAllApprovalRequests,
        ViewOnlyMyApprovalRequests,
        ViewAllExecutedContacts,
        ViewOnlyMyExecutedContracts,
        //ViewOnlyMyExecutedPermits,
        //ViewAllExecutedPermits
    }

    public enum PowerUsersPermissions
    {
        ResetPassword,
        ViewDashboard,
        EditProfile,
        ViewContract,
        InitiateContract,
        GenerateContract,
        ShareContract,
        EditContract,
        CreateDocumentTag,
        ShareRequest,
        ApproveAndRejectContractRequest,
        ViewMail,
        ViewExecutedContract,
        CreateExecutedContract,
        EditExecutedContract,
        ViewExecutedPermit,
        CreateExecutedPermit,
        EditExecutedPermit,
        RenewContract,
        TerminateContract,
        SignAndAcceptContract,
        Comment,
        ViewAllContractRequests,
        ViewOnlyMyContractRequests,
        ViewAllApprovalRequests,
        ViewOnlyMyApprovalRequests,
        ViewAllExecutedContacts,
        ViewOnlyMyExecutedContracts,
        //ViewOnlyMyExecutedPermits,
        //ViewAllExecutedPermits
    }

    public enum ExternalUserPermissions
    {

        ViewMail,
        OpenContract,
        AcceptContract,
        SignAndAcceptContract,
        RejectContract,
        CommentOnContract,
        HighLightaContract
    }

    public enum SupportPermissions
    {
        ViewOrganization,
        ViewUser,
        ViewConfiguration,
        ViewReports,
        EditProfile,
        ResetPassword
    }
}
