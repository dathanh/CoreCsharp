using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Mongo.Repositories
{
    public class ProjectNameMongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly MongoServer _mongoServer;
        private readonly MongoDatabase _mongoDatabase;
        private readonly IConfiguration _configuration;
        public ProjectNameMongoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var databaseName = configuration["ProjectNameMongoDatabaseName"];
            var hostTemplate = configuration["ProjectNameMongoConnectionString"];
            var mongoHost = hostTemplate.Replace("{DB_NAME}", databaseName);
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoHost));
            // settings.WriteConcern = WriteConcern.Unacknowledged;
            _mongoClient = new MongoClient(settings);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase(databaseName);

        }
        public ProjectNameMongoDbContext(string url, string databaseName)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(url));
            // settings.WriteConcern = WriteConcern.Unacknowledged;
            _mongoClient = new MongoClient(settings);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase(databaseName);

        }
        public MongoDatabase ProjectNameMongoDatabase => _mongoDatabase;

        //public MongoCollection<Car> Cars
        //{
        //    get
        //    {
        //        return ProjectNameMongoDatabase.GetCollection<Car>("cars");
        //    }
        //}
    }
}
