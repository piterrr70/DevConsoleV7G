
using System;
using UnityEngine;

namespace V7G.Console
{
    public static class GenericExtension
    {

        public static bool IsInt<T>(this T inputValue, out int value)
        {
            return int.TryParse(inputValue as string, out value);
        }

        public static bool IsFloat<T>(this T inputValue, out float value)
        {
            return float.TryParse(inputValue as string, out value);
        }

        public static Type GetTypeOfThisValue<T>(this T inputValue)
        {
            if ((inputValue as string).Split(',').Length == 3)
            {
                return typeof(Vector3);
            }
            if (int.TryParse(inputValue as string, out var _))
            {
                return typeof(int);
            }
            if(float.TryParse(inputValue as string, out var _))
            {
                return typeof(float);
            }            
            return typeof(string);
        }

        public static Vector3 StringToVector3(this string value, char separator)
        {
            string[] sValue = value.Split(separator);

            if (sValue.Length == 3)
            {
                return new Vector3(float.Parse(sValue[0]), float.Parse(sValue[1]), float.Parse(sValue[2]));
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}