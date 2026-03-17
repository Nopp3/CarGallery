using CarGalleryAPI.Data;
using Xunit;

namespace CarGalleryAPI.Tests.Data
{
    public class HashTests
    {
        [Fact]
        public void HashPassword_AndVerifyPassword_WithSamePassword_ReturnsTrue()
        {
            var password = "StrongPass!123";
            var hashed = Hash.HashPassword(password);

            var result = Hash.VerifyPassword(password, hashed);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_WithWrongPassword_ReturnsFalse()
        {
            var hashed = Hash.HashPassword("CorrectPassword!123");

            var result = Hash.VerifyPassword("WrongPassword!123", hashed);

            Assert.False(result);
        }

        [Fact]
        public void VerifyPassword_WithInvalidHash_ReturnsFalse()
        {
            var result = Hash.VerifyPassword("AnyPassword!123", "not-a-valid-bcrypt-hash");

            Assert.False(result);
        }
    }
}
