using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace X1APServer.ServiceTests
{
    public static class CommonUtils
    {
        public static bool ReflectiveEquals(object first, object second)
        {
            if (first == null && second == null)
            {
                return true;
            }
            if (first == null || second == null)
            {
                return false;
            }
            System.Type firstType = first.GetType();
            if (second.GetType() != firstType)
            {
                return false; // Or throw an exception
            }

            foreach (PropertyInfo propertyInfo in firstType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object defaultValue = null;
                    object firstValue = propertyInfo.GetValue(first, null);
                    object secondValue = propertyInfo.GetValue(second, null);

                    if (firstValue != null) defaultValue = GetDefault(firstValue.GetType());

                    if (((defaultValue != null && !defaultValue.Equals(firstValue)) || (defaultValue == null && firstValue != defaultValue)) && !object.Equals(firstValue, secondValue))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static object GetDefault(System.Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
