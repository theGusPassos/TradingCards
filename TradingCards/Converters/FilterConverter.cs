using Microsoft.AspNetCore.Mvc.ModelBinding;
using TradingCards.Controllers.Filters;

namespace TradingCards.Converters
{
    public class FilterConverter(FilterTypeRegistry filterTypeRegistry) : IModelBinder
    {
        readonly FilterTypeRegistry filterTypeRegistry = filterTypeRegistry;

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var queryString = bindingContext.HttpContext.Request.Query;

            if (queryString.TryGetValue("type", out var type))
            {
                var filterType = filterTypeRegistry.GetType(type.ToString());
                bindingContext.Result = ModelBindingResult.Success((FilterBase)QueryStringToObjectConverter.Convert(queryString, filterType));
            }
            else
            {
                if (queryString.TryGetValue("name", out var name))
                {
                    bindingContext.Result = ModelBindingResult.Success(new FilterBase
                    {
                        Name = name
                    });
                }
                else
                {
                    throw new Exception("invalid filter");
                }
            }

            return Task.CompletedTask;
        }
    }
}
