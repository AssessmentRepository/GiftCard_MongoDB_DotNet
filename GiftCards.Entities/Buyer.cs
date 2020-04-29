
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GiftCards.Entities
{
   public class Buyer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BuyerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int GiftId { get; set; }
    }
}
