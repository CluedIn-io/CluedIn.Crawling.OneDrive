using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

using CluedIn.Core;
using CluedIn.Server.Common.WebApi.OAuth;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Provider.OneDrive.WebApi
{
    [RoutePrefix("api/" + OneDriveConstants.ProviderName + "/oauth")]
    public class OneDriveOAuthController : OAuthCluedInApiController, IOAuth2Authentication
    {
        private const string NoStateFoundInCluedIn = "No state found in CluedIn";
        private const string NoStateWasReturnedFromAuthenticator = "No state was returned from authenticator";

        public OneDriveOAuthController([NotNull] OneDriveProviderComponent component)
          : base(component)
        {
        }

        // This method will be invoked as a call-back from an authentication service (e.g., https://login.windows.net/).
        // It is not intended to be called directly, only as a redirect from the authorization request.
        // On completion, the method will cache the refresh token and access tokens, and redirect to the URL
        // specified in the state parameter.
        public async Task<HttpResponseMessage> Get(string code, string state)
        {
            using (var system = CreateRequestSystemExecutionContext())
            {
                // NOTE: In production, OAuth must be done over a secure HTTPS connection.
                if (Request.RequestUri.Scheme != "https" && !Request.RequestUri.IsLoopback)
                    return await Task.FromResult(Get("Endpoint is not HTTPS"));

                // Ensure there is a state value on the response.  If there is none, stop OAuth processing and display an error.
                if (state == null)
                    return await Task.FromResult(Get(NoStateWasReturnedFromAuthenticator));

                if (code == null)
                    return await Task.FromResult(Get("No code was returned from authenticator"));

                var stateObject = ValidateState(system, state);

                if (stateObject == null)
                {
                    system.Log.Error(new { code, state }, () => NoStateFoundInCluedIn);
                    return await Task.FromResult(Get(NoStateFoundInCluedIn));
                }

                throw new NotImplementedException("TODO: Implement this");
            }
        }

        /// <summary>Gets the specified error.</summary>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public HttpResponseMessage Get([NotNull] string error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            using (var system = CreateRequestSystemExecutionContext())
            {
                var response = Request.CreateResponse(HttpStatusCode.ExpectationFailed);

                system.Log.Warn(() => $"OneDrive received an error on OAuth : {error}");

                //I am not passed the state back, so I have nowhere to know where I cam from, hence the redirection to app.cluedin.net
                response.Content = new StringContent(
                    $"<script>window.location = \"https://app.{ConfigurationManager.AppSettings["Domain"]}/admin/#/administration/integration/error/onedrive\";</script>", Encoding.UTF8, "text/html");

                // Ensure there is a state value on the response.  If there is none, stop OAuth processing and display an error.
                return response;
            }
        }
    }
}
