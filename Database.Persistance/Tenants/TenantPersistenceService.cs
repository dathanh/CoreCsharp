using Framework.Service.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Database.Persistance.Tenants
{
    public class TenantPersistenceService : IPersistenceService<ITenantWorkspace>,
        ITenantPersistenceService
    {
        private ITenantWorkspace _tenantWorkspace;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDiagnosticService _diagnosticService;
        public TenantPersistenceService(IDeploymentService deploymentService, IHttpContextAccessor httpContextAccessor,
            IDiagnosticService diagnosticService)
        {
            DeploymentService = deploymentService;
            _httpContextAccessor = httpContextAccessor;
            _diagnosticService = diagnosticService;
            RefreshTenant();
        }

        public IDeploymentService DeploymentService { get; set; }

        public ITenantWorkspace CurrentWorkspace => _tenantWorkspace;

        public void RefreshTenant()
        {
            var dbContextOption = DeploymentService.GetDbContextOptionsBuilder();
            var context = new TenantDataContext(dbContextOption, _httpContextAccessor, _diagnosticService);
            _tenantWorkspace = new TenantWorkspace(context);
        }
    }
}
