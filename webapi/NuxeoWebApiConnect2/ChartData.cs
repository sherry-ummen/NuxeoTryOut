using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NuxeoWebApiConnect2 {
    public class ChartData {
        public IEnumerable<string> Labels { get; set; }
        public IEnumerable<int> Data { get; set; }

        public IEnumerable<string> BackgroundColor { get; set; }
        public IEnumerable<string> HoverBackgroundColor { get; set; }
    }
}