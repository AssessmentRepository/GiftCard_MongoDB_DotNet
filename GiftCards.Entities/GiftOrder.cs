using System;
using System.Collections.Generic;
using System.Text;

namespace GiftCards.Entities
{
  public  class GiftOrder
    {
      public string GiftOrderId { get; set; }
      public Gift OrderedGift { get; set; }
      public  Buyer GiftBuyer { get; set; }
    }
}
