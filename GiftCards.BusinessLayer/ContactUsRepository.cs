using GiftCards.DataLayer;
using GiftCards.Entities;

namespace GiftCards.BusinessLayer
{
    public class ContactUsRepository : BaseRepository<ContactUs>, IContactUsRepository
    {
        public ContactUsRepository(IMongoDBContext context) : base(context)
        {
        }
    }
}
