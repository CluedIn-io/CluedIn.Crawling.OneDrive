/*using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using CluedIn.Core;
using CluedIn.Server.Common.WebApi.OAuth;
using CluedIn.Crawling.OneDrive.Core;

namespace CluedIn.Provider.OneDrive.WebApi
{
    [Authorize(Roles = "Admin, OrganizationAdmin")]
    [RoutePrefix("api/providers/" + OneDriveConstants.ProviderName)]
    public class OneDriveController : OAuthCluedInApiController
    {
        public OneDriveController([NotNull] OneDriveProviderComponent component) : base(component)
        {
        }

        // GET: Authenticate and Fetch Data
        public async Task<HttpResponseMessage> Get(string authError)
        {
            using (var context = CreateRequestExecutionContext(UserPrincipal))
            {
                if (authError != null)
                {
                    // Tell the OAuth provider where to redirect to once you have the code.
                    var redirectUri = new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/api/" + OneDriveConstants.ProviderName + "/oauth");

                    var state = GenerateState(context, UserPrincipal.Identity.UserId, redirectUri.AbsoluteUri, context.Organization.Id);

                    throw new NotImplementedException($"TODO: Implement state processing... {state}");
                }

                return await Task.FromResult(Request.CreateResponse(HttpStatusCode.OK, "OneDrive Provider Crawled"));
            }
        }
    }
}

*/
