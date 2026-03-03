using DataLibrary.Models.Request;
using System.Collections.Generic;

namespace CS.DataLibrary.Models.Dictionaries
{
    public class CSColor : BaseClass
    {
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public virtual ICollection<CSUserConfiguration> UserConfigurations { get; set; }

        public CSColor()
        { }
        public CSColor(DictionaryRequestModel requestModel)
        {
            Id = requestModel.Id;
            PrimaryColor = requestModel.ValueOne;
            SecondaryColor = requestModel.ValueTwo;
            IsDeleted = false;
        }
    }
}
