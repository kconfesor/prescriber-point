using System.Security.Claims;

namespace PrescriberPoint.Journal.WebApi.Extensions;

public static class ClaimsPrincipalExtensions {
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal) {
        return int.Parse(claimsPrincipal.FindFirstValue(CustomClaimTypes.UserId)!);
    }
}