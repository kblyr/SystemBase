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

    static bool TryGetClaim<T>(this ClaimsPrincipal principal, string claimType, TryParse<T> tryParse, out T value) where T : struct
    {
        if (principal.TryGetClaim(claimType, out string valueString))
        {
            var result = tryParse(valueString);
            value = result.value;
            return result.status;
        }

        value = default;
        return false;
    }

    public static bool TryGetClaimBoolean(this ClaimsPrincipal principal, string claimType, out bool output) => principal.TryGetClaim(claimType, (value) => new (bool.TryParse(value, out bool outputValue), outputValue), out output);
    public static bool TryGetClaimByte(this ClaimsPrincipal principal, string claimType, out byte output) => principal.TryGetClaim(claimType, (value) => new (byte.TryParse(value, out byte outputValue), outputValue), out output);
    public static bool TryGetClaimInt16(this ClaimsPrincipal principal, string claimType, out short output) => principal.TryGetClaim(claimType, (value) => new (short.TryParse(value, out short outputValue), outputValue), out output);
    public static bool TryGetClaimInt32(this ClaimsPrincipal principal, string claimType, out int output) => principal.TryGetClaim(claimType, (value) => new (int.TryParse(value, out int outputValue), outputValue), out output);
    public static bool TryGetClaimInt32(this ClaimsPrincipal principal, string claimType, out long output) => principal.TryGetClaim(claimType, (value) => new (long.TryParse(value, out long outputValue), outputValue), out output);
    public static bool TryGetClaimSingle(this ClaimsPrincipal principal, string claimType, out float output) => principal.TryGetClaim(claimType, (value) => new (float.TryParse(value, out float outputValue), outputValue), out output);
    public static bool TryGetClaimDouble(this ClaimsPrincipal principal, string claimType, out double output) => principal.TryGetClaim(claimType, (value) => new (double.TryParse(value, out double outputValue), outputValue), out output);
    public static bool TryGetClaimDecimal(this ClaimsPrincipal principal, string claimType, out decimal output) => principal.TryGetClaim(claimType, (value) => new (decimal.TryParse(value, out decimal outputValue), outputValue), out output);
    public static bool TryGetClaimDateTime(this ClaimsPrincipal principal, string claimType, out DateTime output) => principal.TryGetClaim(claimType, (value) => new (DateTime.TryParse(value, out DateTime outputValue), outputValue), out output);
    public static bool TryGetClaimDateTimeOffset(this ClaimsPrincipal principal, string claimType, out DateTimeOffset output) => principal.TryGetClaim(claimType, (value) => new (DateTimeOffset.TryParse(value, out DateTimeOffset outputValue), outputValue), out output);

    delegate (bool status, T value) TryParse<T>(string value);
}