using System.Text.Json.Serialization;
using System.Text.Json; 

namespace Keycloak
{
    public class KeycloakAuthService
    {
        private readonly HttpClient httpClient; 
        private readonly IConfiguration configuration;
        public KeycloakAuthService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient; 
            this.configuration = configuration; 
        }

        public async Task<TokenResponse> LoginAsync(string username, string password)
        {
            var tokenEndpoint = $"{configuration["Keycloak:BaseUrl"]}/realms/{configuration["Keycloak:Realm"]}/protocol/openid-connect/token";

            var requestBody = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", configuration["Keycloak:ClientId"] },
            { "client_secret", configuration["Keycloak:ClientSecret"] }, 
            { "username", username },
            { "password", password }
        };

            var content = new FormUrlEncodedContent(requestBody);

            var response = await httpClient.PostAsync(tokenEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to authenticate: {response.ReasonPhrase}");
            }
            throw new Exception("failed to authenticate");
        } 
    }
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
    
}