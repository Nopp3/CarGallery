namespace CarGalleryAPI.Models
{
    public class User
    {
        public Guid id { get; set; }
        public int role_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
