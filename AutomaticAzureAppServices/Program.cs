using System;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.Common.Authentication.Models;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.ResourceManager.Models;
using Microsoft.Azure.Management.WebSites;
using Microsoft.Rest;
using Microsoft.Rest.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;

namespace AutomaticAzureAppServices
{
    class Program
    {
        private static ResourceManagementClient _resourceGroupClient;
        private static WebSiteManagementClient _websiteClient;
        private static AzureEnvironment _environment;
        static void Main(string[] args)
        {
            try
            {
                MainAsync().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }
        static async Task MainAsync()
        {
            // Set Environment - Choose between Azure public cloud, china cloud and US govt. cloud
            _environment = AzureEnvironment.PublicEnvironments[EnvironmentName.AzureCloud];

            // Get the credentials
            TokenCloudCredentials cloudCreds = GetCredsFromServicePrincipal();

            var tokenCreds = new TokenCredentials(cloudCreds.Token);
            // Use the creds to create the clients we need
            _resourceGroupClient = new ResourceManagementClient(_environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager), tokenCreds);
            _resourceGroupClient.SubscriptionId = cloudCreds.SubscriptionId;
            _websiteClient = new WebSiteManagementClient(_environment.GetEndpointAsUri(AzureEnvironment.Endpoint.ResourceManager), tokenCreds);
            _websiteClient.SubscriptionId = cloudCreds.SubscriptionId; 

            await ListResourceGroupsAndSites();

        }


        private static TokenCloudCredentials GetCredsFromServicePrincipal()
        {
            string subscription = ConfigurationManager.AppSettings["AzureSubscription"];
            string tenantId = ConfigurationManager.AppSettings["AzureTenantId"];
            string clientId = ConfigurationManager.AppSettings["AzureClientId"];
            string clientSecret = ConfigurationManager.AppSettings["AzureClientSecret"];

            // Quick check to make sure we're not running with the default app.config
            if (subscription[0] == '[')
            {
                throw new Exception("You need to enter your appSettings in app.config to run this sample");
            }

            var authority = String.Format("{0}{1}", _environment.Endpoints[AzureEnvironment.Endpoint.ActiveDirectory], tenantId);
            var authContext = new AuthenticationContext(authority);
            var credential = new ClientCredential(clientId, clientSecret);
            var authResult = authContext.AcquireToken(_environment.Endpoints[AzureEnvironment.Endpoint.ActiveDirectoryServiceEndpointResourceId], credential);
            return new TokenCloudCredentials(subscription, authResult.AccessToken);
        }

        static async Task ListResourceGroupsAndSites()
        {
            // Go through all the resource groups in the subscription
            IPage<ResourceGroup> rgListResult = await _resourceGroupClient.ResourceGroups.ListAsync();
            foreach (var rg in rgListResult)
            {
                Console.WriteLine(rg.Name);

                // Go through all the Websites in the resource group
                var siteListResult = await _websiteClient.WebApps.ListByResourceGroupWithHttpMessagesAsync(rg.Name);
                foreach (var site in siteListResult.Body)
                {
                    Console.WriteLine("    " + site.Name);


                    //await _websiteClient.WebApps.StopAsync(site.ResourceGroup, site.Name);  //Stop Service

                    await _websiteClient.WebApps.StartAsync(site.ResourceGroup, site.Name);  //Start Service

                }
            }
        }

    }
}