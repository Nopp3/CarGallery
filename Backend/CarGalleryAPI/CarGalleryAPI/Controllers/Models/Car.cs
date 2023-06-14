namespace CarGalleryAPI.Controllers.Models
{
    public class Car
    {
        public Guid id { get; set; }
        public Guid user_id { get; set; }
        public int fuel_id { get; set; }
        public int body_id { get; set; }
        public int brand_id { get; set; }
        public string model { get; set; }
        public int productionYear { get; set; }
        public int engine { get; set; }
        public int horsePower { get; set; }
        public string imagePath { get; set; }
    }
}
