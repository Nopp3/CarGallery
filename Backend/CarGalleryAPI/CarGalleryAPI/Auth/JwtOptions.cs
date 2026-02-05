namespace CarGalleryAPI.Auth
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = "CarGalleryAPI";
        public string Audience { get; set; } = "CarGalleryFrontend";
        public string SigningKey { get; set; } = "";
        public int AccessTokenMinutes { get; set; } = 60;
    }
}
