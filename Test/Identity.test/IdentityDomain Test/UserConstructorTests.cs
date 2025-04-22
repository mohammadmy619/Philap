using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Domain.UserAgregate;
using Domain.UserAgregate.Exception;

namespace IdentityDomain_Test
{
    public class UserConstructorTests
    {
        [Fact]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            string userName = "testuser";
            string email = "test@example.com";
            string passwordHash = "hashedpassword123";

            // Act
            var user = new User(userName, email, passwordHash);

            // Assert
            Assert.Equal(userName, user.UserName);
            Assert.Equal(email, user.Email);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.NotNull(user.RoleIds); // یا _RoleIds اگر public است
            Assert.Empty(user.RoleIds); // لیست باید خالی باشد
        }

        [Theory]
        [InlineData(null, "test@example.com", "hashedpassword123")]
        [InlineData("", "test@example.com", "hashedpassword123")]
        [InlineData("  ", "test@example.com", "hashedpassword123")]
        public void Constructor_WithInvalidUserName_ThrowsException(string invalidUserName, string email, string passwordHash)
        {
            // Act
            Action user = () => new User(invalidUserName, email, passwordHash);


            // & Assert
            user.Should().Throw<UserNameIsNullException>("User name cannot be null or empty.");

        }

        [Theory]
        [InlineData("testuser", null, "hashedpassword123")]
        [InlineData("testuser", "", "hashedpassword123")]
        [InlineData("testuser", "  ", "hashedpassword123")]
        public void Constructor_WithInvalidEmail_ThrowsException(string userName, string invalidEmail, string passwordHash)
        {

            Action user = () => new User(userName, invalidEmail, passwordHash);
            // Act & Assert
            user.Should().Throw<EmailIsNullException>("Email cannot be null or empty.");
        }
        [Theory]
        [InlineData("invalid-email")] // Invalid format
        //[InlineData("user@domain")]   // Missing TLD
        //[InlineData("@domain.com")]   // Missing local part
        public void Constructor_WithInvalidEmailFormat_ThrowsInvalidEmailException(string invalidEmail)
        {
            // Arrange & Act
            Action act = () => new User("testuser", invalidEmail, "hashedpassword123");

            // Assert
            act.Should().Throw<InvalidEmailException>()
                .WithMessage("The provided email is not in a valid format.");
        }

        [Theory]
        [InlineData("testuser", "test@example.com", null)]
        [InlineData("testuser", "test@example.com", "")]
        [InlineData("testuser", "test@example.com", "  ")]
        public void Constructor_WithInvalidPasswordHash_ThrowsException(string userName, string email, string invalidPasswordHash)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new User(userName, email, invalidPasswordHash));
        }
    }
}

