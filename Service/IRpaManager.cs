namespace RPAManagement
{
    /// <summary>
    /// Interface for managing RPA operations using UiPath Orchestrator.
    /// </summary>
    public interface IRpaManager
    {
        // UiPath Orchestrator Methods

        /// <summary>
        /// Retrieves an authentication token from UiPath Orchestrator.
        /// </summary>
        /// <param name="clientId">The client ID.</param>
        /// <param name="accessKey">The access key.</param>
        /// <returns>The authentication token.</returns>
        Task<string> GetAuthenticationToken(string clientId, string accessKey);

        /// <summary>
        /// Retrieves the release key and organization unit ID by name from UiPath Orchestrator.
        /// </summary>
        /// <param name="releaseKeyUrl">The URL to retrieve the release key.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>A dictionary containing the release key and organization unit ID.</returns>
        Task<Dictionary<string, string>> GetReleaseKeyByName(string releaseKeyUrl, string accessToken);

        /// <summary>
        /// Starts the RPA process with the specified release key, organization unit ID, and additional arguments.
        /// </summary>
        /// <param name="url">The URL to start the RPA process.</param>
        /// <param name="accessToken">The access token.</param>
        /// <param name="releaseKey">The release key.</param>
        /// <param name="organizationUnitId">The organization unit ID.</param>
        /// <returns>The response from the RPA process start request.</returns>
        Task<string> StartRpaUsingWithArguments(string url, string accessToken, string releaseKey, string organizationUnitId);
    }
}
