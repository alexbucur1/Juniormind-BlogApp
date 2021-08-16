using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using BlogApp.Dotnet.ApplicationCore.Settings;

namespace BlogApp.Dotnet.Services
{
    public class AuthService : IAuthService
    {
        private readonly DiscoveryDocumentResponse _discoveryDocument;
        private readonly IOptions<AppSettings> _appSettings;

        public AuthService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            using var httpClient = new HttpClient();
            _discoveryDocument = httpClient.GetDiscoveryDocumentAsync($"{_appSettings.Value.LinksDocument}").Result;
        }

        public async Task<string> GetToken(ClientDTO client)
        {
            using var httpClient = new HttpClient();
            UriBuilder builder = new UriBuilder(_discoveryDocument.AuthorizeEndpoint);
            builder.Query = client.ClientInfoQuery;
            var result = httpClient.GetAsync(builder.Uri).Result;
            var uri = result.RequestMessage.RequestUri.ToString() + $"&email={client.Email}&password={client.Password}";
            var tokens = await httpClient.PostAsync(uri, result.Content);
            var tokensContent = await tokens.Content.ReadAsStringAsync();

            return tokensContent; 
        }
    }
}

