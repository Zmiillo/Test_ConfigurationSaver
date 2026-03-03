using DataLibrary.Models.Request;
using System.Collections.Generic;

namespace CS.DataLibrary.Models.Dictionaries
{
    public class CSFont : BaseClass
    {
        public string Name { get; set; }
        public string FontColor { get; set; }

        public virtual ICollection<CSUserConfiguration> UserConfigurations { get; set; }

        public CSFont()
        { }
        public CSFont(DictionaryRequestModel requestModel)
        {
            Id = requestModel.Id;
            Name = requestModel.ValueOne;
            FontColor = requestModel.ValueTwo;
            IsDeleted = false;
        }
    }
}
