namespace TradingCards.Converters
{
    public class FilterTypeRegistry
    {
        readonly Dictionary<string, Type> mappings = [];

        public void Register<T>(string type) => mappings.Add(type, typeof(T));
        public Type GetType(string type) => mappings[type];
    }
}
