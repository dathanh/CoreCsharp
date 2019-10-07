using Database.Persistance.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectName.Services.Deployment
{
    public class WebDeploymentService : IDeploymentService
    {
        private readonly IConfiguration _configuration;
        public WebDeploymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbContextOptions<TenantDataContext> GetDbContextOptionsBuilder()
        {
            var optionsBuilder = new DbContextOptionsBuilder<TenantDataContext>();
            int.TryParse(_configuration["ConnectionStringTimeOut"], out int connectionTimeOut);
            if (connectionTimeOut == 0)
            {
                connectionTimeOut = 180;
            }
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(_configuration.GetConnectionString("ProjectName"), opt => opt.CommandTimeout(connectionTimeOut));
            return optionsBuilder.Options;
        }
    }
}
