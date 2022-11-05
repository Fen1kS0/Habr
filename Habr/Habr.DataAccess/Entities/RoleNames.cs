namespace Habr.DataAccess.Entities;

public class RoleNames
{
    public const string Administrator = "Administrator";
    public const string User = "User";

    public static IEnumerable<string> AllRoles => typeof(RoleNames)
        .GetFields()
        .Select(x => x.GetValue(null))
        .OfType<string>();
}