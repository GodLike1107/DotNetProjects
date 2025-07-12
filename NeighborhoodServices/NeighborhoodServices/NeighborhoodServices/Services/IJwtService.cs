using NeighborhoodServices.API.Models;

namespace NeighborhoodAPI.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
