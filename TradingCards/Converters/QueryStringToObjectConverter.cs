using System.ComponentModel;

namespace TradingCards.Converters;

public class QueryStringToObjectConverter
{
    public static object Convert(IQueryCollection query, Type type) 
    {
        var obj = Activator.CreateInstance(type)!;
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            // Look for matching query string key (case-insensitive)
            var queryValue = query.FirstOrDefault(q => q.Key.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
            if (queryValue.Value.Count > 0)
            {
                var value = queryValue.Value[0];

                // Get the type converter for the property type
                var converter = TypeDescriptor.GetConverter(property.PropertyType);

                if (converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        // If the property is nullable, get the underlying type
                        if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                        {
                            property.SetValue(obj, converter.ConvertFrom(value));
                        }
                        else
                        {
                            // Convert non-nullable values
                            property.SetValue(obj, converter.ConvertFrom(value));
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle conversion error (e.g., invalid format)
                        Console.WriteLine($"Error converting value for {property.Name}: {ex.Message}");
                    }
                }
            }
        }

        return obj;
    }
}
