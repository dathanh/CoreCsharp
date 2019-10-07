namespace Database.Persistance
{
    public interface IPersistenceService<TWorkspace> where TWorkspace : IWorkspace
    {
        TWorkspace CurrentWorkspace { get; }
    }
}
