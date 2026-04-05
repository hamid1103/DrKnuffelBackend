using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Text;

namespace DrKnuffelBackEnd.Tests
{
    [TestClass]
    public class UserRegistrationTests
    {
        private WebApplicationFactory<Program> factory;
        private HttpClient client;

        [TestInitialize]
        public void Setup()
        {
            factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string>
                        {
                            { "ConnectionStrings:DefaultConnection", "Server=tcp:avansict2242721.database.windows.net,1433;Initial Catalog=db2242721;Persist Security Info=False;User ID=MY_APPLICATION_USER;Password=W@chtwoord159;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" }
                        });
                    });
                });

            client = factory.CreateClient();
        }

        [TestMethod]
        public async Task Register_PasswordTooShort_ReturnsBadRequest()
        {
            // Arrange
            var json = """
            {
                "email": "test1@test.com",
                "password": "Short1!"
            }
            """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/account/register", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_PasswordWithoutDigit_ReturnsBadRequest()
        {
            // Arrange
            var json = """
            {
                "email": "test2@test.com",
                "password": "NoDigitPassword!"
            }
            """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/account/register", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_PasswordWithoutUppercase_ReturnsBadRequest()
        {
            // Arrange
            var json = """
            {
                "email": "test3@test.com",
                "password": "lowercase123!"
            }
            """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/account/register", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [TestMethod]
        public async Task Register_PasswordWithoutSpecialChar_ReturnsBadRequest()
        {
            // Arrange
            var json = """
            {
                "email": "test4@test.com",
                "password": "Valid12345"
            }
            """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/account/register", content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}