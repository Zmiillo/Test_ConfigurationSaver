using System.Collections.Generic;

namespace CS.DataLibrary.Models.Dictionaries
{
    public class CSSubscribeType : BaseClass
    {
        public string Type { get; set; }

        public virtual ICollection<CSMessage> Messages { get; set; }
    }
}
