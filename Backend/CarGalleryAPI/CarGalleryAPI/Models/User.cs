
namespace CarGalleryAPI.Models
{
    public enum Roles
    {
        Admin = 1,
        User = 2
    }

    public class User
    {
        public Guid id { get; set; }
        public int role_id { get; set; }
        public string username { get; set; }
        public string? email { get; set; }
        public string password { get; set; }
    }

    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
