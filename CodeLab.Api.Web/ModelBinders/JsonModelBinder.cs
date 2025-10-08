using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CodeLab.Api.Web.ModelBinders;

public class JsonModelBinder<T> : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var valueProviderResult = context.ValueProvider.GetValue(context.ModelName);
        if (valueProviderResult == ValueProviderResult.None) return Task.CompletedTask;

        var value = valueProviderResult.FirstValue;
        if (string.IsNullOrEmpty(value)) return Task.CompletedTask;

        var obj = JsonSerializer.Deserialize<T>(value);
        context.Result = ModelBindingResult.Success(obj);
        return Task.CompletedTask;
    }
}