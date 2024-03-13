namespace RoleBasedAuthSample.Extension;

public static class Permissions
{
 
        public const int Read = 1;// "Read";
        public const int Write =2;// "Write";
        public const int Update =3;// "Update";
        public const int Delete =4;// "Delete";
        public const int Create =5;// "Create";
        public const int Execute =6;//"Execute";
        public const int Admin =7;// "Admin";
        // Custom Permissions
        public const int UserManagement = 10;// "UserManagement";
        public const int ContentManagement =11;// "ContentManagement";
        public const int SettingsManagement =12;// "SettingsManagement";
 
}