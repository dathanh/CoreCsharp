namespace Framework.Service.StartUp
{
    public interface IStartupTask
    {
        int Order { get; }
        void Execute();
    }
}
