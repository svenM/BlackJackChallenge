using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi.Domain
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ExcelValueAttribute : Attribute
    {
        public string ExcelValue { get; set; }
        public ExcelValueAttribute(string value)
        {
            ExcelValue = value;
        }
    }
}
