using Microsoft.EntityFrameworkCore;
using System;

namespace Database.Persistance
{
    public abstract class WorkspaceBase<TContext> : IWorkspace where TContext : DbContext
    {
        public WorkspaceBase(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; private set; }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Action ReleaseCallback { get; set; }

        ~WorkspaceBase()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (ReleaseCallback != null)
                {
                    ReleaseCallback();
                }

                Context.Dispose();
            }
        }
    }
}
