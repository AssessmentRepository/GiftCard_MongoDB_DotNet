using System;
using System.Collections.Generic;
using System.Text;

namespace GiftCards.Tests.Exceptions
{
   public class GiftNotFoundException :Exception
    {
        public string Messages;

        public GiftNotFoundException()
        {
            Messages = "Gift Not Found";
        }
        public GiftNotFoundException(string message)
        {
            Messages = message;
        }
    }
}
