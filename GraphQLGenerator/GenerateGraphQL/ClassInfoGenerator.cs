using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace GQLG
{
    public static class ClassInfoGenerator
    {
        // Generate method to create JSON string of public properties
        public static string Generate(Type target)
        {
            var properties = target.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                   .Select(p => new PropertyInfo
                                   {
                                       Name = p.Name,
                                       Type = GetTypeName(p.PropertyType),
                                       IsNullable = IsNullable(p.PropertyType),
                                       IsCollection = IsCollection(p.PropertyType),
                                       GenericArguments = GetGenericArguments(p.PropertyType)
                                   })
                                   .ToList();

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }

        // Get the name of the type, without handling nullable types here
        private static string GetTypeName(Type type)
        {
            if (IsNullable(type))
            {
                return Nullable.GetUnderlyingType(type).Name;
            }
            return type.Name;
        }

        // Check if the type is nullable
        private static bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        // Check if the type is a collection
        private static bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        // Get the list of generic arguments for the type
        private static List<string> GetGenericArguments(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments().Select(t => t.Name).ToList();
            }
            return new List<string>();
        }
    }
}
