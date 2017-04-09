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
using NuxeoClient;
using Authorization = NuxeoClient.Authorization;

namespace NuxeoWebApiConnect2.Controllers {

    public class JsonContent : HttpContent {

        private readonly MemoryStream _Stream = new MemoryStream();
        public JsonContent(object value) {

            Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var jw = new JsonTextWriter(new StreamWriter(_Stream));
            jw.Formatting = Formatting.Indented;
            var serializer = new JsonSerializer();
            serializer.Serialize(jw, value);
            jw.Flush();
            _Stream.Position = 0;

        }
        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context) {
            return _Stream.CopyToAsync(stream);
        }

        protected override bool TryComputeLength(out long length) {
            length = _Stream.Length;
            return true;
        }
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
        public IEnumerable<string> Coverage() {
            return Regions;
        }

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

        // POST api/values
        public void Post([FromBody]string value) {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/values/5
        public void Delete(int id) {
        }
    }
}
