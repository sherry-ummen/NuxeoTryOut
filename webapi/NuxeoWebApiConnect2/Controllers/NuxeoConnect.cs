using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using NuxeoClient;

namespace NuxeoWebApiConnect2.Controllers {
    public class NuxeoConnectController : ApiController {
        private Client _client = null;
        public NuxeoConnectController() {
            _client = new Client("http://localhost:8080/nuxeo/", new Authorization("Administrator", "Administrator"));
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
