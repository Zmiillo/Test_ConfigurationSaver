using CS.DataLibrary.Models.Dictionaries;

namespace CS.DataLibrary.Models
{
    public class CSMessage
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SubscribeId { get; set; }

        public virtual CSSubscribeType SubscribeType { get; set; }
    }
}
