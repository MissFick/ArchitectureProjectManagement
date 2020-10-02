using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Authorization
    {
        public static AuthorizationOptions SetupAuthorizationPolicies(this AuthorizationOptions options)
        {
            //https://docs.asp.net/en/latest/security/authorization/policies.html

            
            options.AddCloudscribeLoggingDefaultPolicy();




            options.AddPolicy(
                "ServerAdminPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins");

                });

             // probably best to not let anyone change the main admin policy from the UI
            options.AddPolicy(
                "AdminPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins", "Administrators");
                });

            // you could comment this out if you want admins from any site to be able
            // to edit globally shared country state data
            // by commenting this out the policy could be managed per tenant from the UI
            // but it is probably best to only let this be managed from the master tenant
            options.AddPolicy(
                "CoreDataPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins");
                });

            // probably best and recommended to not let this policy be managed from the UI
            // since this policy controls who can manage policies from the UI
            options.AddPolicy(
                "PolicyManagementPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Administrators");
                });

            //
            options.AddPolicy(
                "UserLookupPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("ServerAdmins", "Administrators", "Draughtsman");
                });

            options.AddPolicy(
                "RoleLookupPolicy",
                authBuilder =>
                {
                    authBuilder.RequireRole("Role Administrators", "Administrators", "Draughtsman");
                });

            // add other policies here 

            options.AddPolicy(
               "CreateProjectPolicy",
               authBuilder =>
               {
                   authBuilder.RequireRole("Administrator","Draughtsman");
               });

            options.AddPolicy(
                "DraughtsmanPolicy",
                authBuilder =>
            {
                authBuilder.RequireRole("Draughtsman");
            });

            options.AddPolicy(
                "PropertyOwnerPolicy",
                authBuilder =>
            {
                authBuilder.RequireRole("Property Owner");
            });

            return options;
        }

    }
}
