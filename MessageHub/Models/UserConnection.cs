using System.Collections.Generic;

namespace MessageHub.Models
{
    public class UserConnection
    {
        public string ConnectionId { get; set; }
        public string Login { get; set; }
        public List<int> Subscribes { get; set; }
    }
}
