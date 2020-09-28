using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER
{
    class ConversationUserLink
    {
        public UserProfile participant { get; set; }
        public Conversation conversation { get; set; }
    }
}
