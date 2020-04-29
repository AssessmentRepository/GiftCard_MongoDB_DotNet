using GiftCards.DataLayer;
using GiftCards.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GiftCards.BusinessLayer
{
   public class GiftRepository : BaseRepository<Gift>, IGiftRepository
    {
        public GiftRepository(IMongoDBContext context) : base(context)
        {
        }
    
    }
}
