namespace CS.DataLibrary.Models.Request
{
    public class AddConfigurationRequestModel
    {
        public int? FontId { get; set; }
        public int? ColorId { get; set; }
        public int? PositionX { get; set; }
        public int? PositionY { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Scale { get; set; }
        public int UserId { get; set; }
    }
}
