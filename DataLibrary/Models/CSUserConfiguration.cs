using CS.DataLibrary.Models.Dictionaries;
using CS.DataLibrary.Models.Request;

namespace CS.DataLibrary.Models
{
    public class CSUserConfiguration : BaseClass
    {
        public int FontId { get; set; }
        public int ColorId { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Scale { get; set; }
        public int UserId { get; set; }

        public virtual CSFont Font { get; set; }
        public virtual CSColor Color { get; set; }
        public virtual CSUser User { get; set; }

        public CSUserConfiguration()
        {
            FontId = 1;
            ColorId = 1;
            PositionX = 0;
            PositionY = 0;
            Scale = 100;
            Width = 300;
            Height = 200;
            IsDeleted = false;
        }

        public CSUserConfiguration(AddConfigurationRequestModel mappingConfiguration)
        {
            FontId = mappingConfiguration.FontId ?? 1;
            ColorId = mappingConfiguration.ColorId ?? 1;
            PositionX = mappingConfiguration.PositionX ?? 0;
            PositionY = mappingConfiguration.PositionY ?? 0;
            Scale = mappingConfiguration.Scale ?? 100;
            Width = mappingConfiguration.Width ?? 300;
            Height = mappingConfiguration.Height ?? 200;
            UserId = mappingConfiguration.UserId;
            IsDeleted = false;
        }

        public CSUserConfiguration(EditConfigurationRequestModel mappingConfiguration)
        {
            FontId = mappingConfiguration.FontId;
            ColorId = mappingConfiguration.ColorId;
            PositionX = mappingConfiguration.PositionX;
            PositionY = mappingConfiguration.PositionY;
            Scale = mappingConfiguration.Scale;
            Width = mappingConfiguration.Width;
            Height = mappingConfiguration.Height;
            Id = mappingConfiguration.Id;
            IsDeleted = false;
        }
    }
}
