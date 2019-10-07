using System;
using System.Threading;

namespace Framework.DomainModel.Entities.MongoDb.Mapping
{
    public class MongoDbMapperProfileThread
    {
        private readonly ManualResetEvent _doneEvent;
        public IMongoDbMappingProfile Profile { get; set; }

        public MongoDbMapperProfileThread(ManualResetEvent doneEvent)
        {
            _doneEvent = doneEvent;
        }
        public void ThreadPoolCallback(Object threadContext)
        {
            Profile.Execute();
            _doneEvent.Set();
        }
    }
}