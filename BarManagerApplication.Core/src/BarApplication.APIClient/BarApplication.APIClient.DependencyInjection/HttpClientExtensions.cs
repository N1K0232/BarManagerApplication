using BarApplication.APIClient.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace BarApplication.APIClient.DependencyInjection;

public static class HttpClientExtensions
{
    public static IHttpClientBuilder AddIdentityClient(this IServiceCollection services)
    {
        var httpClientBuilder = services.AddHttpClient<IIdentityClient, IdentityClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://localhost:44388/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return new IdentityClient(httpClient);
        });

        return httpClientBuilder;
    }
}