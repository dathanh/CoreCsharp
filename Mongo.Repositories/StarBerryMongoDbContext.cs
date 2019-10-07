using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Mongo.Repositories
{
    public class StarBerryMongoDbContext
    {
        private readonly MongoClient _mongoClient;
        private readonly MongoServer _mongoServer;
        private readonly MongoDatabase _mongoDatabase;
        private readonly IConfiguration _configuration;
        public StarBerryMongoDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var databaseName = configuration["StarBerryMongoDatabaseName"];
            var hostTemplate = configuration["StarBerryMongoConnectionString"];
            var mongoHost = hostTemplate.Replace("{DB_NAME}", databaseName);
            var settings = MongoClientSettings.FromUrl(new MongoUrl(mongoHost));
            // settings.WriteConcern = WriteConcern.Unacknowledged;
            _mongoClient = new MongoClient(settings);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase(databaseName);

        }
        public StarBerryMongoDbContext(string url, string databaseName)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(url));
            // settings.WriteConcern = WriteConcern.Unacknowledged;
            _mongoClient = new MongoClient(settings);
            _mongoServer = _mongoClient.GetServer();
            _mongoDatabase = _mongoServer.GetDatabase(databaseName);

        }
        public MongoDatabase StarBerryMongoDatabase => _mongoDatabase;

        //public MongoCollection<Car> Cars
        //{
        //    get
        //    {
        //        return StarBerryMongoDatabase.GetCollection<Car>("cars");
        //    }
        //}
    }
}
