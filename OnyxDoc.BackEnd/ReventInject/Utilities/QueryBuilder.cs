using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReventInject.Utilities
{
    public class QueryBuilder
    {
        private static string _paramPrefix = "@";
        private static bool EnableNamedParams_;
        // Create a command
        private static Regex rxParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);

        public QueryBuilder(bool _EnableNamedParams = false)
        {
            EnableNamedParams_ = _EnableNamedParams;
        }

        public string BuildQuery(string sql, params object[] args)
        {
            return BuildSql(EnableNamedParams_, sql, args);
        }
        
        public static string BuildSql(bool EnableNamedParams, string sql, params object[] args)
        {
            // Perform named argument replacements
            if (EnableNamedParams)
            {
                var new_args = new List<object>();
                sql = ParametersHelper.ProcessParams(sql, args, new_args);
                args = new_args.ToArray();
            }

            // Perform parameter prefix replacements
            if (_paramPrefix != "@")
                sql = rxParamsPrefix.Replace(sql, m => _paramPrefix + m.Value.Substring(1));
            sql = sql.Replace("@@", "@");		   // <- double @@ escapes a single @

            int b = 0;
            foreach (var item in args)
            {
                sql = sql.Replace("@" + b, item.ToString());
                b++;
            }

            return sql;
        }
    }
}
