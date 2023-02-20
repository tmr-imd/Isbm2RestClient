using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Isbm2Client.Extensions;

public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, object> source) where T : class, new()
    {
        T someObject = new();

        foreach (var item in source)
        {
            var property = typeof(T).GetProperty(item.Key);

            if ( property is not null )
                property.SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    public static Dictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    {
        Type objectType = source.GetType();

        PropertyInfo[] properties = objectType.GetProperties( bindingAttr );

        return properties.ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null) ?? throw new NullReferenceException($"Property {propInfo.Name} cannot have a null value")
        );
    }
}
