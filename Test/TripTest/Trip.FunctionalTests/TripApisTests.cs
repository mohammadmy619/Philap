using Application.Trips.CreateTrip;
using Domain.TripAggregate;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace Trip.FunctionalTests
{
    public class TripApisTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public TripApisTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        #region [Tests]

        [Fact]
        public async Task Create_Trip_Should_Return_OkStatus()
        {
            // Arrange
            var createCommand = new CreateTripCommand(
                LeaderId: Guid.NewGuid(),
                TravelStartDate: DateTime.UtcNow.AddDays(10),
                TravelEndDate: DateTime.UtcNow.AddDays(15),
                LocationName: $"Tehran - {Guid.NewGuid()}",
                PriceAmount: 1500.50m,
                PriceCurrency: "USD",
                TripStatus: TripStatus.Active // یا هر مقدار Enum که دارید
            );

            var content = new StringContent(
                JsonConvert.SerializeObject(createCommand),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PostAsync("api/trip", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = JsonConvert.DeserializeObject<CreateTripResponse>(responseContent);
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result?.TripId);
        }

        [Fact]
        public async Task Get_Trip_By_Valid_Id_Should_Return_Ok()
        {
            // Arrange: First create a trip to fetch
            var createdTrip = await CreateSampleTripAsync();
            var tripId = createdTrip.TripId;

            // Act
            var response = await _httpClient.GetAsync($"api/trip/{tripId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GetTripResponse>(responseContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(tripId, result?.TripId);
            Assert.Equal("qom - Sample", result?.LocationName); // یا مقدار مورد انتظار
        }

        [Fact]
        public async Task Get_Trip_By_Invalid_Id_Should_Return_NotFound()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await _httpClient.GetAsync($"api/trip/{nonExistentId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_Trips_With_Pagination_Should_Return_Ok()
        {
            // Arrange: Ensure some data exists
            await CreateSampleTripAsync();

            // Act
            var response = await _httpClient.GetAsync("api/trip?PageNumber=1&PageSize=10");
            var responseContent = await response.Content.ReadAsStringAsync();

            // فرض: خروجی لیست به صورت آرایه‌ای از GetTripsResponse است
            var result = JsonConvert.DeserializeObject<List<GetTripsResponse>>(responseContent);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            // اختیاری: Assert.True(result.Count > 0);
        }

        [Fact]
        public async Task Update_Trip_Should_Return_NoContent()
        {
            // Arrange: Create a trip first
            var createdTrip = await CreateSampleTripAsync();

            var updateCommand = new UpdateTripCommand(
                TripId: createdTrip.TripId,
                LeaderId: Guid.NewGuid(), // یا همان LeaderId قبلی
                TravelStartDate: DateTime.UtcNow.AddDays(20),
                TravelEndDate: DateTime.UtcNow.AddDays(25),
                LocationName: $"Updated Location - {Guid.NewGuid()}",
                PriceAmount: 2000.00m,
                PriceCurrency: "EUR",
                TripStatus: TripStatus.Active
            );

            var content = new StringContent(
                JsonConvert.SerializeObject(updateCommand),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _httpClient.PutAsync($"api/trip/{updateCommand.TripId}", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Optional: Verify update by fetching again
            var getResponse = await _httpClient.GetAsync($"api/trip/{updateCommand.TripId}");
            var updatedContent = await getResponse.Content.ReadAsStringAsync();
            var updatedTrip = JsonConvert.DeserializeObject<GetTripResponse>(updatedContent);

            Assert.Equal(updateCommand.LocationName, updatedTrip?.LocationName);
            Assert.Equal(updateCommand.PriceAmount, updatedTrip?.PriceAmount);
        }

        #endregion [Tests]


        #region [Private Helpers]

        private async Task<CreateTripResponse> CreateSampleTripAsync()
        {
            var command = new CreateTripCommand(
                LeaderId: Guid.NewGuid(),
                TravelStartDate: DateTime.UtcNow.AddDays(1),
                TravelEndDate: DateTime.UtcNow.AddDays(2),
                LocationName: $"qom - Sample",
                PriceAmount: 1000.00m,
                PriceCurrency: "USD",
                TripStatus: TripStatus.Active
            );

            var content = new StringContent(
                JsonConvert.SerializeObject(command),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("api/trip", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreateTripResponse>(responseString)!;
        }

        #endregion [Private Helpers]
    }


    #region [Test DTOs / Responses]
    // این کلاس‌ها باید دقیقاً منطبق با Responseهای برگشتی از هندلرهای شما باشند

    public class CreateTripResponse
    {
        [JsonProperty("tripId")]
        public Guid TripId { get; set; }

        // سایر فیلدهایی که هنگام Create برمی‌گردند
    }

    public class GetTripResponse
    {
        [JsonProperty("tripId")]
        public Guid TripId { get; set; }

        [JsonProperty("leaderId")]
        public Guid LeaderId { get; set; }

        [JsonProperty("travelStartDate")]
        public DateTime TravelStartDate { get; set; }

        [JsonProperty("travelEndDate")]
        public DateTime TravelEndDate { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; } = null!;

        [JsonProperty("priceAmount")]
        public decimal PriceAmount { get; set; }

        [JsonProperty("priceCurrency")]
        public string PriceCurrency { get; set; } = null!;

        [JsonProperty("tripStatus")]
        public TripStatus TripStatus { get; set; }
    }

    // برای لیست تریپ‌ها، معمولاً همان ساختار GetTripResponse استفاده می‌شود
    public class GetTripsResponse : GetTripResponse { }

    #endregion
}