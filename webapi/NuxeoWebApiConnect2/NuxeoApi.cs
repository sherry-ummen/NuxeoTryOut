using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json.Linq;
using NuxeoClient;
using NuxeoClient.Adapters;
using NuxeoClient.Wrappers;
using NuxeoWebApiConnect2.Controllers;
using Task = System.Threading.Tasks.Task;

namespace NuxeoWebApiConnect2 {
    public static class NuxeoApi {

        private static Client _client = null;
        private static Random rnd = new Random();
        private static string _uuid = "c1e1874e-142e-4aa4-acb4-c7f1fca1dfad";
        static NuxeoApi() {
            _client = new Client("http://localhost:8080/nuxeo/", new Authorization("Administrator", "Administrator"));
            _client.AddDefaultSchema("dublincore");
        }

        public static ChartData GetChartDataAll() {
            Documents documents = null;
            //Getting the document
            Task.Run(async () => {
                Document mainFolder = (Document)_client.DocumentFromUid(_uuid).Get().Result;

                Adapter adapter = new SearchAdapter().SetSearchMode(SearchAdapter.SearchMode.NXQL)
                    .SetSearchQuery("SELECT * FROM DOCUMENT WHERE ecm:parentId = \"" + mainFolder.Uid + "\"")
                    .SetPageSize(Int32.MaxValue.ToString() /*This will blow up. Get pagination logic done*/);

                documents = (Documents)mainFolder.SetAdapter(adapter).Get().Result;
            }).Wait();
            return GetChartData(documents);
        }

        public static ChartData Query(QueryPostData data) {
            Documents documents = null;
            //Getting the document
            Task.Run(async () => {
                Document mainFolder = (Document)_client.DocumentFromUid(_uuid).Get().Result;

                Adapter adapter = new SearchAdapter().SetSearchMode(SearchAdapter.SearchMode.NXQL)
                .SetSearchQuery($"SELECT * FROM DOCUMENT WHERE ecm:parentId =\"{mainFolder.Uid}\" AND " +
                                $"dc:subjects IN ({ArraySqlToQueryString(data.Subjects)}) AND " +
                                $"dc:coverage IN ({ArraySqlToQueryString(data.Regions)})")
                .SetPageSize(Int32.MaxValue.ToString() /*This will blow up. Get pagination logic done*/);

                documents = (Documents)mainFolder.SetAdapter(adapter).Get().Result;
            }).Wait();
            return GetChartData(documents);
        }

        private static ChartData GetChartData(Documents documents) {
            if (documents == null) return new ChartData();
            Dictionary<string, int> chartvalues = new Dictionary<string, int>();
            foreach (var document in documents.Entries.Where(x => x.Properties.Keys.Any(z => z == "dc:nature"))) {
                string value = document.Properties["dc:nature"].Value<string>();
                if (!chartvalues.ContainsKey(value)) {
                    chartvalues[value] = 1;
                } else {
                    chartvalues[value] += 1;
                }
            }
            chartvalues = chartvalues.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            return new ChartData() {
                Labels = chartvalues.Keys,
                Data = chartvalues.Values,
                BackgroundColor = Lighten(Color.DodgerBlue, chartvalues.Count).ToArray()
            };
        }

        private static string ArraySqlToQueryString(IEnumerable<string> values) {
            return string.Join(",", values.Select(x => $"'{x}'"));
        }

        private static IEnumerable<string> Lighten(Color inColor, int count = 1, double inAmount = 10) {

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