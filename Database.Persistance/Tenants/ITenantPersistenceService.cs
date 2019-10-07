namespace Database.Persistance.Tenants
{
    public interface ITenantPersistenceService : IPersistenceService<ITenantWorkspace>
    {
        void RefreshTenant();
    }
}
