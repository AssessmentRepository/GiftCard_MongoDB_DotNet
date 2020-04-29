using GiftCards.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GiftCards.DataLayer
{
  public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoDBContext _mongoContext;
        protected IMongoCollection<TEntity> _dbCollection;

        protected BaseRepository(IMongoDBContext context)
        {
            _mongoContext = context;
            _dbCollection = _mongoContext.GetCollection<TEntity>(typeof(TEntity).Name);
        }

      
        public async Task<IEnumerable<TEntity>> AllContactUs()
        {
            var all = await _dbCollection.FindAsync(Builders<TEntity>.Filter.Empty, null);
            return await all.ToListAsync();
        }
        public async Task Create(TEntity obj)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> Get(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAll()
        {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<Buyer>> GetAllBuyerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Buyer> PlaceOrderAsync(Buyer Buyer, string GiftId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Gift>> SearchGift(string Giftname)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Gift>> ViewAllGifts()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GiftOrder>> ViewGiftCardOrders()
        {
            throw new NotImplementedException();
        }
    }
}
