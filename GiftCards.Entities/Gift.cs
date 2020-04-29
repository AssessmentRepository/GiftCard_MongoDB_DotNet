
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GiftCards.Entities
{
   public class Gift
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GiftId { get; set; }
        public string GiftName{ get; set; }
        public int Ammount { get; set; }
        public string ContactUsId { get; set; }
    }
}
