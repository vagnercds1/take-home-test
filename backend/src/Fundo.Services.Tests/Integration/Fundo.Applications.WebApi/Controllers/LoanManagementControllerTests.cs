using Fundo.Applications.Domain.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Fundo.Services.Tests.Integration
{
    /*************************************
      
     1 - In the real case, the repository layer should be mocked and all endpoints should be tested.

     *************************************/

    public class LoanManagementControllerTests : 
        IClassFixture<WebApplicationFactory<Fundo.Applications.WebApi.Startup>>,
        IClassFixture<WebApplicationFactory<Fundo.Applications.WebApiSecurity.Startup>>
    {
        private readonly HttpClient _authClient;
        private readonly HttpClient _clientLoan;
 
        public LoanManagementControllerTests(WebApplicationFactory<Fundo.Applications.WebApi.Startup> factoryLoan,
                                             WebApplicationFactory<Fundo.Applications.WebApiSecurity.Startup> factoryAuth)
        { 
            _clientLoan = factoryLoan.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _authClient = factoryAuth.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private async Task<string> AuthenticateAsync()
        {
            // Arrange: 
            var loginRequest = new RequestLogin
            {
                User = "john@test.com",
                Password = "1234"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json"
            );

            // Act: 
            var response = await _authClient.PostAsync("/api/loan/login", content);

            // Assert: 
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ResponseLogin>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return responseObject.Token;
        }

        #region PostLoanAsync

        [Fact]
        public async Task PostLoanAsync_ShouldReturn200_WhenRequestIsValid()
        {
            // Arrange:
            var token = await AuthenticateAsync();
            _clientLoan.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestLoan = new
            {
                Amount = 1000.0,
                Term = 12,
                ApplicantId = "d25eda0b-c474-465c-b32b-2e3575aa9811"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestLoan),
                Encoding.UTF8,
                "application/json"
            );

            // Act: 
            var response = await _clientLoan.PostAsync("/api/loan", content);

            // Assert:
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Created successfully", responseContent);
        }

        [Fact]
        public async Task PostLoanAsync_ShouldReturn400_WhenRequestIsNotValid()
        {
            // Arrange:
            var token = await AuthenticateAsync();
            _clientLoan.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestLoan = new
            {
                Amount = 1000.0,
                Term = 12,
                ApplicantId = "12345"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(requestLoan),
                Encoding.UTF8,
                "application/json"
            );

            // Act: 
            var response = await _clientLoan.PostAsync("/api/loan", content);

            // Assert:
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Applicant not Exists", responseContent);
        }

        [Fact]
        public async Task PostLoanAsync_ShouldReturn401_WhenNotAuthenticated()
        {
            // Arrange: 
            var content = new StringContent(
                JsonSerializer.Serialize(new RequestLoan()),
                Encoding.UTF8,
                "application/json"
            );

            // Act: 
            var response = await _clientLoan.PostAsync("/api/loan", content);

            // Assert:
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion

        #region GetLoansAsync

        [Fact]
        public async Task GetLoansAsync_ShouldReturn200_WhenLoansExist()
        {
            // Arrange:
            var token = await AuthenticateAsync();
        
            _clientLoan.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act: 
            var response = await _clientLoan.GetAsync("/api/loan/loans");

            // Assert:
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetLoansAsync_ShouldReturn401_WhenNotAuthenticated()
        {
            // Act: 
            var response = await _clientLoan.GetAsync("/api/loan/loans");

            // Assert:
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        }

        #endregion
    }
}
