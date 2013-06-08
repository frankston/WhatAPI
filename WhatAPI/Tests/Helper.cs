using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    internal static class Helper
    {

        private static Random random = new Random((int)DateTime.Now.Ticks);

        internal static int GetRandomIntFromRange(int smallest, int largest, params int[] exclude)
        {
            var random = new Random();
            while (true)
            {
                var value = random.Next(smallest, largest + 1);
                if (!exclude.Contains(value)) return value;
            }
        }

        internal static int GetRandomElementFromArray(int[] values)
        {
            var random = new Random();
            return values[random.Next(0, values.Length + 1)];
        }

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

        internal static string RandomCharString(int length)
        {
            char[] c = new char[length];
            for (int i = 0; i < length; i++) c[i] = (char)random.Next(97, 122);
            return new string(c);
        }

        internal static int GetRandomIntFromRange(int min, int max)
        {
            return random.Next(min, max + 1);
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
