namespace TradingCards.Converters;

public class TypeRegistry
{
    readonly Dictionary<string, Type> mappings = [];
    public void Register<T>(string type) => mappings.Add(type.ToLower(), typeof(T));
    public Type GetType(string type) => mappings[type.ToLower()];
}

public class CardTypeRegistry : TypeRegistry
{
}

public class FilterTypeRegistry : TypeRegistry
{
}


