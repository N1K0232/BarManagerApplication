using System;
using System.Net.Http;

namespace BarApplication.APIClient.Identity.Core
{
    public class IdentityClient : IIdentityClient
    {
        private readonly HttpClient httpClient;

        public IdentityClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing && httpClient != null)
            {
                httpClient.Dispose();
            }
        }
    }
}