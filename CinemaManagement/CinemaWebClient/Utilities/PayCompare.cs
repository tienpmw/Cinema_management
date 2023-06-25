using System.Globalization;

namespace CinemaWebClient.Utilities
{
    public class PayCompare : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return 1;
            if (y == null) return -1;
            var Compare = CompareInfo.GetCompareInfo("es-US");
            return Compare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
