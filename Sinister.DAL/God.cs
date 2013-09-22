using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Sinister.DAL
{
    public class God
    {
        public static string MigrationConnectionString { get; set; }
    }
    public static class Extensions
    {
        public static string GetDisplayName(this PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayNameAttribute), true);
            if (atts.Length == 0)
                return "";
            return (atts[0] as DisplayNameAttribute).DisplayName;
        }


    }
}
