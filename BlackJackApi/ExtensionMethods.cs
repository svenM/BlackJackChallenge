using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlackJackApi
{
    public class RandomMethods
    {
        //TODO sort in more appropriate classes
        public static T RandomEnumValue<T>() where T : System.Enum
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new Random().Next(v.Length));
        }
    }
}
