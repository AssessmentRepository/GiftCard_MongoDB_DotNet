using GiftCards.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiftCards.DataLayer
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> AllContactUs();
        Task<IEnumerable<GiftOrder>> ViewGiftCardOrders();
        Task<IEnumerable<Gift>> ViewAllGifts();
        Task<Buyer> PlaceOrderAsync(Buyer Buyer, string GiftId);
        Task<IEnumerable<Buyer>> GetAllBuyerAsync();
        Task<IEnumerable<Gift>> SearchGift(string Giftname);

        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(string id);
        Task Create(TEntity obj);
    }
}
