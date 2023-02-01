using Newtonsoft.Json;
using RedWizardsHatWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RedWizardsHatWeb.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;

        }

        private HttpClient SetupHttpClient(string token)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // ServicePointManager.ServerCertificateValidationCallback +=
            //(sender, cert, chain, sslPolicyErrors) => { return true; };
            //var client = _clientFactory.CreateClient();
            var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");
            if (token?.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return client;

        }

        public async Task<bool> CreateAsync(string url, T objectToCreate, string token = "")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if (objectToCreate != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(objectToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = SetupHttpClient(token);

            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> DeleteAsync(string url, int Id, string token = "")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            var client = SetupHttpClient(token);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url, string token = "")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var client = SetupHttpClient(token);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<T> GetAsync(string url, int Id, string token = "")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var client = SetupHttpClient(token);
            HttpResponseMessage response = await client.SendAsync(requestMessage);
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate, string token = "")
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Patch, url);
            if (objToUpdate != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = SetupHttpClient(token);
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
