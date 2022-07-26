using System.Collections.Generic;

namespace Upskillz_invoice_mgt_Domain.Common
{
    public static partial class Xtenxion
    {
        public static string ToStringItems<T>(this IEnumerable<T> items, string seperator = ",")
        {
            return items != null ? string.Join(seperator, items) : null;
        }
    }
}
