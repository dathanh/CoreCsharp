namespace Database.Persistance.Tenants
{
    public class TenantWorkspace : WorkspaceBase<TenantDataContext>, ITenantWorkspace
    {
        public TenantWorkspace(TenantDataContext context)
            : base(context)
        {
        }
    }
}
