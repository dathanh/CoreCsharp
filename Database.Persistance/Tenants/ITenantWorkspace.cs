namespace Database.Persistance.Tenants
{
    public interface ITenantWorkspace : IWorkspace
    {
        TenantDataContext Context { get; }
    }
}
