using System.Collections.Generic;

namespace NuxeoWebApiConnect2 {
    public class ChartData {
        public IEnumerable<string> Labels { get; set; } = new List<string>();
        public IEnumerable<int> Data { get; set; } = new List<int>();

        public IEnumerable<string> BackgroundColor { get; set; } = new List<string>();
        public IEnumerable<string> HoverBackgroundColor { get; set; } = new List<string>();
    }
}