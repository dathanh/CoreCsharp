using Framework.Service.StartUp;
using System;
using System.Linq;
using System.Threading;

namespace Framework.DomainModel.Entities.MongoDb.Mapping
{
    public class MongoDbMapperStartupTask : IStartupTask
    {
        public int Order => 0;

        public void Execute()
        {
            var types = this.GetType().Assembly.GetTypes();
            var profileTypes = types.Where(type => typeof(IMongoDbMappingProfile).IsAssignableFrom(type) && type.Name != "IMongoDbMappingProfile");
            foreach (var type in profileTypes)
            {
                var doneEvent = new ManualResetEvent(false);
                var thread = new MongoDbMapperProfileThread(doneEvent) { Profile = (IMongoDbMappingProfile)Activator.CreateInstance(type) };
                ThreadPool.QueueUserWorkItem(thread.ThreadPoolCallback);

            }
        }
    }
}