using LifeOrganizer.Domain.Entities;

namespace LifeOrganizer.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
