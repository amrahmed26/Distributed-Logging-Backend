namespace DistributedLogging.api.Auth.Constants
{
    public class CustomClaimTypes
    {
        public const string Name = "Name";
        public const string Permission = "Permission";
        public const string Email = "Email";
    }

    public static class AccountPermissions
    {
        public const string Create = "create-users";
        public const string Edit = "edit-users";
        public const string View = "view-users";
        public const string Delete = "delete-users";
        public const string Publish = "publish-users";
        public const string EditPermission = "edit-permissions-users";
    }

  
}
