using Newtonsoft.Json;
using RedWizardsHatWeb.Models;
using RedWizardsHatWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RedWizardsHatWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        public readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        private HttpClient SetupHttpClient()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // ServicePointManager.ServerCertificateValidationCallback +=
            //(sender, cert, chain, sslPolicyErrors) => { return true; };
            //var client = _clientFactory.CreateClient();
            var client = _clientFactory.CreateClient("HttpClientWithSSLUntrusted");


            return client;

        }

        public async Task<User> LoginAsync(string url, User objToCreate)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }
            var client = SetupHttpClient();
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User objToCreate)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if (objToCreate != null)
            {
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            var client = SetupHttpClient();
            HttpResponseMessage response = await client.SendAsync(requestMessage);

            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
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
