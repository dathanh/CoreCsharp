using System;

namespace Database.Persistance
{
    public interface IWorkspace : IDisposable
    {
        Action ReleaseCallback { get; set; }
        void Commit();
    }
}
