using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace BlogApp.Dotnet.API.IntegrationTests.Helpers
{
    public static class HttpClients
    {
        public static HttpClient GetAuthorizedClient(WebApplicationFactory<Startup> factory, string role, string userID)
        {
            return factory.WithWebHostBuilder(builder =>
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator>(x => new FakePolicyEvaluator(role));
                services.AddControllers(o => o.Filters.Add(new CustomAuthorizeFilter(role, userID))); 
            }))
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
        }


        public static HttpClient GetUnauthorizedClient(WebApplicationFactory<Startup> factory)
        {
            return factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}
