using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GQLG.Models.Meta;
using Newtonsoft.Json;

namespace GQLG.Models.Factories
{
    public static class ClassInfoFactory
    {
        public static ClassInfo Build(Type target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            var breadcrumb = new List<Type>();
            var classInfo = new ClassInfo()
            {
                Name = target.Name,
                Namespace = target.Namespace,
                Properties = CreateProperties(target, breadcrumb).ToArray()
            };

            return classInfo;
        }

        // Generate method to create JSON string of public properties
        public static string Create(Type target)
        {
            var properties = CreateProperties(target, new List<Type>());

            return JsonConvert.SerializeObject(properties, Formatting.Indented);
        }

        private static List<Meta.PropertyInfo> CreateProperties(Type target, List<Type> exclude)
        {
            return target.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p =>
                {
                    var childExclude = new List<Type>(exclude);
                    childExclude.Add(p.PropertyType);
                    return new Meta.PropertyInfo
                    {
                        Name = p.Name,
                        Type = GetTypeName(p.PropertyType),
                        IsNullable = IsNullable(p.PropertyType),
                        IsCollection = IsCollection(p.PropertyType),
                        IsPrimitive = IsPrimitive(p.PropertyType),
                        GenericArguments = GetGenericArguments(p.PropertyType),
                        Includes = !exclude.Contains(p.PropertyType) && !IsCollection(p.PropertyType) && !IsNullable(p.PropertyType) && !IsPrimitive(p.PropertyType) ? Includes(p.PropertyType, childExclude) : new List<Meta.PropertyInfo>()
                    };
                })
                .ToList();
        }

        private static List<Meta.PropertyInfo> Includes(Type propertyType, List<Type> exclude)
        {
            return CreateProperties(propertyType, exclude);
        }

        private static bool IsPrimitive(Type propertyType)
        {
            return 
                propertyType.IsPrimitive || 
                propertyType.Equals(typeof(string)) || 
                propertyType.Equals(typeof(DateTime)) ||
                propertyType.Equals(typeof(DateTimeOffset)) ||
                propertyType.Equals(typeof(TimeSpan)) ||
                (IsNullable(propertyType) &&
                    IsPrimitive(Nullable.GetUnderlyingType(propertyType)));
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
