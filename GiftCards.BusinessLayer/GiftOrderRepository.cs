using GiftCards.DataLayer;
using GiftCards.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiftCards.BusinessLayer
{
   public class GiftOrderRepository : BaseRepository<GiftOrder>, IGiftOrderRepository
    {
        public GiftOrderRepository(IMongoDBContext context) : base(context)
        {
        }
    
    }
}
