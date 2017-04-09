using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuxeoClient;
using Authorization = NuxeoClient.Authorization;

namespace NuxeoWebApiConnect2.Controllers {

    public class QueryPostData {
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<string> Subjects { get; set; }
        public string Language { get; set; }
    }

    public class Test {
        public IEnumerable<string> Regions { get; set; }
    }

    public class NuxeoConnectController : ApiController {

        private static string[] Nature = new[]
            {"application", "article", "contract", "bankAccountDetails", "booklet", "letter", "loanApplication"};  // TODO : Fetch these from Nuxeo. But how I don know yet.

        private static string[] Subjects = new[] {
            "art","sciences","daily life", "human sciences", "society", "technology"
        };

        private static string[] Regions = new[] {
            "asia", "africa", "europe", "antartica"
        };

        [System.Web.Http.HttpGet]
        public ChartData ChartData() {
            return NuxeoApi.GetChartDataAll();
        }

        // GET api/values
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id) {
            return "value";
        }

        [System.Web.Http.HttpGet]
        // POST api/values
        public ChartData Post(string data) {
            return data != null ? NuxeoApi.Query(JObject.Parse(data).ToObject<QueryPostData>()) : null;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        public void Delete(int id) {
        }
    }
}
