using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace RPAManagement
{
    public class RpaManager : IRpaManager
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<RpaManager> _logger;

        public RpaManager(IHttpClientFactory clientFactory, ILogger<RpaManager> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;

        }

        /// <summary>
        /// Retrieves an authentication token from UiPath Orchestrator.
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="accessKey">The access key.</param>
        /// <returns>The authentication token.</returns>
        public async Task<string> GetAuthenticationToken(string clientId, string accessKey)
        {
            try
            {
                _logger.LogInformation("Getting authentication token...");

                string authenticateUrl = "here-goes-the-URL";
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                JObject reqBody = new(
                    new JProperty("grant_type", "refresh_token"));
                HttpContent content = new StringContent(reqBody.ToString(), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(authenticateUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(responseContent);
                    var output = data.SelectToken("token")?.Value<string>();
                    return output ?? "";
                }
                else
                {
                    return $"StatusCode: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting authentication token.");
                return $"Error: {ex.Message}";
            }
        }

        /// <summary>
        /// Retrieves the release key and organization unit ID by name from UiPath Orchestrator.
        /// </summary>
        /// <param name="releaseKeyUrl">The URL to retrieve the release key.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>A dictionary containing the release key and organization unit ID.</returns>
        public async Task<Dictionary<string, string>> GetReleaseKeyByName(string releaseKeyUrl, string accessToken)
        {
            try
            {
                _logger.LogInformation("Getting Release Key...");

                string getReleaseKeyUrl = releaseKeyUrl;
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync(getReleaseKeyUrl, HttpCompletionOption.ResponseContentRead);

                var respDict = new Dictionary<string, string>();

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic data = JObject.Parse(responseContent);
                    respDict.Add("ReleaseKey", Convert.ToString(data.value[0].Key.Value));
                    respDict.Add("OrganizationUnitId", Convert.ToString(data.value[0].OrganizationUnitId.Value));
                    return respDict;
                }
                else
                {
                    return new Dictionary<string, string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting authentication token.");
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Starts the RPA process with the specified release key, organization unit ID, and additional arguments.
        /// </summary>
        /// <param name="url">The URL to start the RPA process.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="releaseKey">The release key.</param>
        /// <param name="organizationUnitId">The organization unit ID.</param>
        /// <returns>The response from the RPA process start request.</returns>
        public async Task<string> StartRpaUsingWithArguments(string url, string accessToken, string releaseKey, string organizationUnitId)
        {
            try
            {
                _logger.LogInformation("Starting RPA...");
                string startJobUrl = url;
                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.TryAddWithoutValidation("X-UIPATH-OrganizationUnitId", organizationUnitId);

                Guid tempCallId = Guid.NewGuid();

                JObject reqBody = new JObject(
                    new JProperty("startInfo", new JObject(
                        new JProperty("ReleaseKey", releaseKey),
                        new JProperty("Strategy", "ModernJobsCount"),
                        new JProperty("JobsCount", 1),
                        new JProperty("InputArguments",
                            new JObject(
                                new JProperty("CallerID", tempCallId)
                            )
                        )
                    ))
                );
                HttpContent content = new StringContent(reqBody.ToString(), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(startJobUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"StatusCode: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting authentication token.");
                return $"StatusCode: Error";
            }
        }
    }
}
