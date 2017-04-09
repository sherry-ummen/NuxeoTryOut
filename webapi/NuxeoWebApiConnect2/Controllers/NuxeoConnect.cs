using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json.Linq;

namespace NuxeoWebApiConnect2.Controllers {

    public class QueryPostData {
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Subjects { get; set; }
        public string Language { get; set; }
    }
    public class NuxeoConnectController : ApiController {

        [System.Web.Http.HttpGet]
        public ChartData ChartData() {
            return NuxeoApi.GetChartDataAll();
        }

        [System.Web.Http.HttpGet]
        public ChartData Post(string data) {
            return data != null ? NuxeoApi.Query(JObject.Parse(data).ToObject<QueryPostData>()) : null;
        }
    }
}
