using Microsoft.EntityFrameworkCore;

namespace Database.Persistance.Tenants
{
    public interface IDeploymentService
    {
        DbContextOptions<TenantDataContext> GetDbContextOptionsBuilder();
    }
}
