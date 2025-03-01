using CodeGeneration.Models.CodingUnits.Meta;
using System.Collections;
using System.Reflection;

namespace CodeGeneration.Services.Factories
{
    public class ReflectionFactory : IReflectionFactory
    {
        public Model Build(Type target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            var breadcrumb = new List<Type>();
            var classInfo = new Model()
            {
                Name = target.Name,
                Namespace = target.Namespace,
                Properties = CreateProperties(target, breadcrumb).ToArray()
            };

            return classInfo;
        }

        private List<Models.CodingUnits.Meta.Members.PropertyInfo> CreateProperties(Type target, List<Type> exclude)
        {
            return target.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p =>
                {
                    var childExclude = new List<Type>(exclude)
                    {
                        p.PropertyType
                    };

                    return new Models.CodingUnits.Meta.Members.PropertyInfo
                    {
                        Name = p.Name,
                        Type = new CodingUnit() { Name = GetTypeName(p.PropertyType) },
                        IsNullable = IsNullable(p.PropertyType),
                        IsCollection = IsCollection(p.PropertyType),
                        IsPrimitive = IsPrimitive(p.PropertyType),
                        GenericArguments = GetGenericArguments(p.PropertyType),
                        Includes = !exclude.Contains(p.PropertyType)
                            && !IsCollection(p.PropertyType)
                            && !IsNullable(p.PropertyType)
                            && !IsPrimitive(p.PropertyType)
                            ? Includes(p.PropertyType, childExclude)
                            : new List<Models.CodingUnits.Meta.Members.PropertyInfo>()
                    };
                })
                .ToList();
        }

        private List<Models.CodingUnits.Meta.Members.PropertyInfo> Includes(Type propertyType, List<Type> exclude)
        {
            return CreateProperties(propertyType, exclude);
        }

        private bool IsPrimitive(Type propertyType)
        {
            return
                propertyType.IsPrimitive ||
                propertyType.Equals(typeof(string)) ||
                propertyType.Equals(typeof(DateTime)) ||
                propertyType.Equals(typeof(DateTimeOffset)) ||
                propertyType.Equals(typeof(TimeSpan)) ||
                (IsNullable(propertyType) &&
                    IsPrimitive(Nullable.GetUnderlyingType(propertyType) ?? typeof(object)));
        }

        // Get the name of the type, without handling nullable types here
        private string GetTypeName(Type type)
        {
            if (IsNullable(type))
            {
                return Nullable.GetUnderlyingType(type)?.Name ?? type.Name;
            }
            return type.Name;
        }

        // Check if the type is nullable
        private bool IsNullable(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        // Check if the type is a collection
        private bool IsCollection(Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string);
        }

        // Get the list of generic arguments for the type
        private List<string> GetGenericArguments(Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments().Select(t => t.Name).ToList();
            }
            return new List<string>();
        }
    }
}
