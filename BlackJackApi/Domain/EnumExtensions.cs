using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlackJackApi.Domain
{
    public static class EnumExtensions
    {

        public static T? GetEnumFromValue<T>(string value) where T : struct, IConvertible
        {
            foreach (var val in Enum.GetValues(typeof(T)))
            {
                FieldInfo field = typeof(T).GetField(val.ToString());
                var attr = field.GetCustomAttribute<ExcelValueAttribute>();
                if(attr.ExcelValue == value)
                {
                    return (T)val;
                }
            }
            return null;
        }
    }
}
