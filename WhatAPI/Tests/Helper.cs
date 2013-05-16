using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    public static class Helper
    {

        public static int GetRandomIntFromRange(int smallest, int largest, params int[] exclude)
        {
            var random = new Random();
            while (true)
            {
                var value = random.Next(smallest, largest + 1);
                if (!exclude.Contains(value)) return value;
            }
        }

        public static int GetRandomElementFromArray(int[] values)
        {
            var random = new Random();
            return values[random.Next(0, values.Length + 1)];
        }

        public static bool AllPropertiesAreDefaultValues<T>(T classInstance)
        {
            foreach (var property in classInstance.GetType().GetProperties())
            {
                var name = property.Name;
                var value = property.GetValue(classInstance, null);
                if (!Equals(value, GetDefaultValue(property.PropertyType))) return false;
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

        private static bool Equals<T>(T a, T b)
        {
            return EqualityComparer<T>.Default.Equals(a, b);
        }

    }
}
