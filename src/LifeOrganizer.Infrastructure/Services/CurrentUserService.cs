using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LifeOrganizer.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LifeOrganizer.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public Guid? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var sub = user?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                   ?? user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }
}
