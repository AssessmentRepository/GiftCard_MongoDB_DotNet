
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GiftCards.Entities
{
   public class ContactUs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ContactUsId { get; set; }
        public string Name { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
