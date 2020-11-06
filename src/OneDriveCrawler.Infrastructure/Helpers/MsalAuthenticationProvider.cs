using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.Crawling.OneDriveCrawler.Infrastructure.Helpers
{
    public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private static MsalAuthenticationProvider _singleton;
        private IPublicClientApplication _clientApplication;
        private string[] _scopes;
        private string _username;
        private SecureString _password;
        private string _userId;
        private string _tenant;
        private string _secret;
        private bool _appAuth;

        private MsalAuthenticationProvider(IPublicClientApplication clientApplication, string[] scopes, string username, SecureString password, string tenant, string secret, bool appAuth)
        {
            _clientApplication = clientApplication;
            _scopes = scopes;
            _username = username;
            _password = password;
            _userId = null;
            _tenant = tenant;
            _secret = secret;
            _appAuth = appAuth;
        }

        public static MsalAuthenticationProvider GetInstance(IPublicClientApplication clientApplication, string[] scopes, string username, SecureString password, string tenant, string secret, bool appAuth)
        {
            if (_singleton == null)
            {
                _singleton = new MsalAuthenticationProvider(clientApplication, scopes, username, password, tenant, secret, appAuth);
            }

            return _singleton;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            var accessToken = await GetTokenAsync();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> GetTokenAsync()
        {
            if (_appAuth)
            {
                var formContent = new FormUrlEncodedContent(new[]
{
                    new KeyValuePair<string, string>("client_id", _username),
                    new KeyValuePair<string, string>("client_secret", _secret),
                    new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

                var myHttpClient = new HttpClient();
                var response = await myHttpClient.PostAsync($"https://login.microsoftonline.com/{_tenant}/oauth2/v2.0/token", formContent);

                var json = response.Content.ReadAsStringAsync().Result;

                var responseModel = JsonConvert.DeserializeObject<Response>(json);

                return responseModel.access_token;

            }

            if (!string.IsNullOrEmpty(_userId))
            {
                try
                {
                    var account = await _clientApplication.GetAccountAsync(_userId);

                    if (account != null)
                    {
                        var silentResult = await _clientApplication.AcquireTokenSilent(_scopes, account).ExecuteAsync();
                        return silentResult.AccessToken;
                    }
                }
                catch (MsalUiRequiredException) { }
            }

            var result = await _clientApplication.AcquireTokenByUsernamePassword(_scopes, _username, _password).ExecuteAsync();
            _userId = result.Account.HomeAccountId.Identifier;
            return result.AccessToken;

        }
    }

    public class Response
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
}
