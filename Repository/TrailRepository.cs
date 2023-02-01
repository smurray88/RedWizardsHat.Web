using RedWizardsHatWeb.Models;
using RedWizardsHatWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedWizardsHatWeb.Repository
{
    public class TrailRepository : Repository<Trail>, ITrailRepository
    {
        public readonly IHttpClientFactory _clientFactory;

        public TrailRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
