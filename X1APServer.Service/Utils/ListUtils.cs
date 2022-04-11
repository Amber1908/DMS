using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.Service.Utils
{
    public static class ListUtils
    {
        public static T FindMaxValueItem<T>(List<T> list, Converter<T, int> converter)
        {
            if (list.Count == 0)
            {
                return default(T);
            }
            int maxValue = int.MinValue;
            T maxValItem = default(T);
            foreach (T item in list)
            {
                int value = converter(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxValItem = item;
                }
            }
            return maxValItem;
        }

        public static T FindMaxValueItem<T>(List<T> list, Converter<T, DateTime> converter)
        {
            if (list.Count == 0)
            {
                return default(T);
            }
            DateTime maxValue = DateTime.MinValue;
            T maxValItem = default(T);
            foreach (T item in list)
            {
                DateTime value = converter(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxValItem = item;
                }
            }
            return maxValItem;
        }
    }
}
