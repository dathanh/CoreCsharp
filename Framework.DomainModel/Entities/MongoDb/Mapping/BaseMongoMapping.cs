using MongoDB.Bson.Serialization;

namespace Framework.DomainModel.Entities.MongoDb.Mapping
{
    public class BaseMongoMapping : IMongoDbMappingProfile
    {
        public void Execute()
        {
            BsonClassMap.RegisterClassMap<Entity>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(c => c.Id));
                cm.GetMemberMap(c => c.CreatedOn).SetElementName("CreatedDate");
                cm.GetMemberMap(c => c.LastTime).SetElementName("LastModifiedDate");
                cm.UnmapProperty(c => c.CreatedById);
                cm.UnmapProperty(c => c.LastUserId);
                cm.UnmapProperty(c => c.CreatedBy);
                cm.UnmapProperty(c => c.LastUser);
            });

            BsonClassMap.RegisterClassMap<MongoEntity>(cm =>
            {
                cm.AutoMap();
                cm.GetMemberMap(c => c.CreatedByName).SetElementName("CreatedBy");
                cm.GetMemberMap(c => c.LastModifiedByName).SetElementName("LastModifiedBy");
            });
        }
    }
}