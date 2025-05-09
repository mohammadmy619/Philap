using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Domain.UserAgregate;
using Domain.UserAgregate.Exception;
using Domain.Services;
using Infrastructure.Services.Externals;
using Moq;

namespace IdentityDomain_Test
{
    public class UserConstructorTests
    {

        public UserConstructorTests()
        {
                
        }

        [Fact]
        public void Constructor_WithValidParameters_InitializesProperties()
        {
            // Arrange
            string userName = "testuser";
            string email = "test@example.com";
            string passwordHash = "hashedpassword123";
            var mockEmailService = new Mock<IEmailService>();
            // Act
            var user = new User(userName, email, passwordHash, mockEmailService.Object);

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
            //Areange
            var mockEmailService = new Mock<IEmailService>();

            // Act
            Action user = () => new User(invalidUserName, email, passwordHash, mockEmailService.Object);


            // & Assert
            user.Should().Throw<UserNameIsNullException>("User name cannot be null or empty.");

        }

        [Theory]
        [InlineData("testuser", null, "hashedpassword123")]
        [InlineData("testuser", "", "hashedpassword123")]
        [InlineData("testuser", "  ", "hashedpassword123")]
        public void Constructor_WithInvalidEmail_ThrowsException(string userName, string invalidEmail, string passwordHash)
        {
            //Areange
            var mockEmailService = new Mock<IEmailService>();



            Action user = () => new User(userName, invalidEmail, passwordHash, mockEmailService.Object);
            // Act & Assert
            user.Should().Throw<EmailIsNullException>("Email cannot be null or empty.");
        }
        [Theory]
        [InlineData("invalid-email")] // Invalid format
   
        public void Constructor_WithInvalidEmailFormat_ThrowsInvalidEmailException(string invalidEmail)
        {
            // Arrange & Act
            //Areange
            var mockEmailService = new Mock<IEmailService>();

            Action act = () => new User("testuser", invalidEmail, "hashedpassword123", mockEmailService.Object);

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
            //Areange
            var mockEmailService = new Mock<IEmailService>();


            // Act & Assert
            Assert.Throws<ArgumentException>(() => new User(userName, email, invalidPasswordHash,mockEmailService.Object));
        }
    }
}

