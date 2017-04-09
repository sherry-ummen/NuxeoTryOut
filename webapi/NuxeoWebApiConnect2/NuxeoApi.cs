using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using NuxeoClient;
using NuxeoClient.Adapters;
using NuxeoClient.Wrappers;
using WebGrease.Css.Extensions;
using Task = System.Threading.Tasks.Task;

namespace NuxeoWebApiConnect2 {
    public static class NuxeoApi {

        private static Client _client = null;
        private static Random rnd = new Random();

        static NuxeoApi() {
            _client = new Client("http://localhost:8080/nuxeo/", new Authorization("Administrator", "Administrator"));
            _client.AddDefaultSchema("dublincore");
        }

        public static ChartData GetChartDataAll() {
            Dictionary<string, int> chartvalues = new Dictionary<string, int>();
            //Getting the document
            Task.Run(async () => {
                Document mainFolder = (Document)_client.DocumentFromUid("c1e1874e-142e-4aa4-acb4-c7f1fca1dfad").Get().Result;

                Adapter adapter = new SearchAdapter().SetSearchMode(SearchAdapter.SearchMode.NXQL)
                    .SetSearchQuery("SELECT * FROM DOCUMENT WHERE ecm:parentId = \"" + mainFolder.Uid + "\"");
                //.SetSearchQuery("SELECT * FROM DOCUMENT WHERE ecm:parentId = \"" + mainFolder.Uid + "\" AND dc:subjects IN ('art', 'sciences')");
                //.SetSearchQuery("SELECT * FROM DOCUMENT WHERE dc:subjects IN ('art', 'sciences')");

                Documents documents = (Documents)mainFolder.SetAdapter(adapter).Get().Result;

                foreach (var document in documents.Entries.Where(x => x.Properties.Keys.Any(z => z == "dc:nature"))) {
                    foreach (var value in document.Properties["dc:subjects"].Value<JToken>().Values<string>()) {
                        if (!chartvalues.ContainsKey(value)) {
                            chartvalues[value] = 1;
                        } else {
                            chartvalues[value] += 1;
                        }
                    }
                }

            }).Wait();
            chartvalues = chartvalues.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return new ChartData() {
                Labels = chartvalues.Keys,
                Data = chartvalues.Values,
                BackgroundColor = Lighten(Color.DodgerBlue, chartvalues.Count).ToArray()
            };

        }
        public static IEnumerable<string> Lighten(Color inColor, int count = 1, double inAmount = 10) {

            Color tempColor = inColor;
            List<string> colors = new List<string>();
            foreach (var i in Enumerable.Range(1, count)) {
                colors.Add(ColorTranslator.ToHtml(tempColor = Color.FromArgb(tempColor.A,
                    (int)(tempColor.R * 0.8), (int)(tempColor.G * 0.8), (int)(tempColor.B * 0.8))));
            }
            return colors;
        }
    }
}