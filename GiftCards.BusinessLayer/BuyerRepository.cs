using GiftCards.DataLayer;
using GiftCards.Entities;


namespace GiftCards.BusinessLayer
{
    public class BuyerRepository : BaseRepository<Buyer>, IBuyerRepository
    {
        public BuyerRepository(IMongoDBContext context) : base(context)
        {
        }
    }
}
