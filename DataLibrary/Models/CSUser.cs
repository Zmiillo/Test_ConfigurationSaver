using System.Collections.Generic;

namespace CS.DataLibrary.Models
{
    public class CSUser : BaseClass
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Subscribes { get; set; }

        public virtual ICollection<CSUserConfiguration> UserConfigurations { get; set; }
    }
}
