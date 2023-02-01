using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedWizardsHatWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://127.0.0.1:5001/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/v1/nationalparks/";
        public static string TrailAPIPath = APIBaseUrl + "api/v1/trails/";
        public static string UsersAPIPath = APIBaseUrl + "api/v1/users/";
    }
}
