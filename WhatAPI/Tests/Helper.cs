using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    internal static class Helper
    {

        internal static bool AllPropertiesAreDefaultValues<T>(T classInstance)
        {
            foreach (var property in classInstance.GetType().GetProperties())
            {
                var name = property.Name;
                var value = property.GetValue(classInstance, null);
                if (!AreEqual(value, GetDefaultValue(property.PropertyType))) return false;
            }
            return true;
        }

        private static object GetDefaultValue(Type t)
        {
            if (t.IsValueType)
            {
                return Activator.CreateInstance(t);
            }
            else
            {
                return null;
            }
        }

        public static bool AreEqual<T>(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }

    }
}
