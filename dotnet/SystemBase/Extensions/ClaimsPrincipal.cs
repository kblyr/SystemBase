using System.Security.Claims;

namespace SystemBase;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetClaim(this ClaimsPrincipal principal, string claimType, out string value)
    {
        if (!principal.HasClaim(_ => _.Type == claimType))
        {
            value = "";
            return false;
        }

        value = principal.Claims.Single(_ => _.Type == claimType).Value;
        return true;
    }

    public static bool TryGetClaimInt32(this ClaimsPrincipal principal, string claimType, out int value)
    {
        if (principal.TryGetClaim(claimType, out string rawValue) && int.TryParse(rawValue, out value))
        {
            return true;
        }

        value = 0;
        return false;
    }
}