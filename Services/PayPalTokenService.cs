using Newtonsoft.Json;

namespace Sips.Services
{
    public class PayPalTokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;


        public PayPalTokenService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration;
        }

        public string GetAccessToken()
        {
            var secretKey = _configuration["PayPal:SecretKey"];
            var payPalClient = _configuration["PayPal:ClientId"];

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post,
                                   "https://api-m.sandbox.paypal.com/v1/oauth2/token");
            tokenRequest.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
              Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{payPalClient}:" +
                                                                      $"{secretKey}")));

            tokenRequest.Content = new FormUrlEncodedContent(new[]
            {
           new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

            var response = _httpClient.SendAsync(tokenRequest).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to retrieve access token. Status code: " +
                                    $" {response.StatusCode}");
            }
            var responseContent = response.Content.ReadAsStringAsync().Result;
            dynamic responseData = JsonConvert.DeserializeObject(responseContent);
            return responseData.access_token;
        }
    }

}
