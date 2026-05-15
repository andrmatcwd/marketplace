namespace Marketplace.Web.Authorization;

public static class AppRoles
{
    public const string User       = "User";
    public const string Seller     = "Seller";
    public const string Moderator  = "Moderator";
    public const string Manager    = "Manager";
    public const string Admin      = "Admin";

    // Ordered lowest → highest (used when iterating to build role lookups)
    public static readonly string[] All = [User, Seller, Moderator, Manager, Admin];
}

public static class AppPolicies
{
    public const string SellerOrAbove    = "SellerOrAbove";
    public const string ModeratorOrAbove = "ModeratorOrAbove";
    public const string ManagerOrAbove   = "ManagerOrAbove";
}
