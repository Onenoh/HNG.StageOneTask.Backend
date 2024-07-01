
using HNG.StageOneTask.BackendC_.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;

namespace HNG.StageOneTask.BackendC_.Implementations
{
    public class GreetingService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "http://ip-api.com";
        public GreetingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GreetingResponseModel> Greet(HttpContext context, string visitor_Name, CancellationToken token)
        {
            var ipAddress = GetClientIpAdress(context);
            var location = await GetClientLocation(ipAddress, token);
            var city = location["city"]?.ToString() ?? "Unknown";
            var greetingResponse = $"Hello, {visitor_Name}!, the temperature is {GetTemperature()} in {city}.";

            var response = new GreetingResponseModel
            {
                Client_ip = ipAddress,
                Location = city,
                Greeting = greetingResponse
            };
            return response;
        }
        private static string GetClientIpAdress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "8.8.8.8"; 
            }
            return ipAddress ?? "Unknown";
        }

        private async Task<JObject> GetClientLocation(string clientIp, CancellationToken token)
        {
            var fields = "city";
            var route = $"{BASE_URL}/json/{clientIp}?fields={fields}";
            try
            {
                var response = await _httpClient.GetStringAsync(route);
                var locationData = JObject.Parse(response);
                return locationData;
            }
            catch (HttpRequestException)
            {
                return new JObject { { "error", "Unable to fetch location data" } };
            }
            catch (JsonException)
            {
                return new JObject { { "error", "Invalid location data format" } };
            }
        }
        private string GetTemperature()
        {
            var temperatureC = Random.Shared.Next(-20, 55);
            var temperatureF = 32 + (int)(temperatureC / 0.5556);
            return $"{temperatureC}°C / {temperatureF}°F";
        }
    }
}
