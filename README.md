# ServiceStack.Text.TupleSerializer

Extension for [`ServiceStack.Text`](https://github.com/ServiceStack/ServiceStack.Text) to serialize [`Tuple`](http://msdn.microsoft.com/en-us/library/system.tuple%28v=vs.100%29.aspx) as a delimited string. This allows for more human readable values under specific situations while still leveraging the benefits of using a tuple.

Custom tuple serialization currently only applies to the json serializer. It works by assigning custom delegates to [`JsConfig<T>.SerializeFn`](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsConfig.cs) and [`JsConfig<T>.DeSerializeFn`](https://github.com/ServiceStack/ServiceStack.Text/blob/master/src/ServiceStack.Text/JsConfig.cs).

## Examples 

### Host Configuration

Configure all tuples in the ExampleCode namespace for all assemblies in the current app domain:
```c#
new TupleSerializerConfigurator()
  .WithAssemblies(AppDomain.CurrentDomain.GetAssemblies())
  .WithNamespaceFilter(ns => ns.StartsWith("ExampleCode"))
  .Configure();
```

Configure a string as a custom delimiter: (The default delimiter is the dash "-" character)
```c#
new TupleSerializerConfigurator()
  .WithAssemblies(AppDomain.CurrentDomain.GetAssemblies())
  .WithNamespaceFilter(ns => ns.StartsWith("ExampleCode"))
  .WithDelimiter(":")
  .Configure();
```

Configure explicit tuples: (useful for testing)
```c#
new TupleSerializerConfigurator()
   .WithTupleTypes(new List<Type>{typeof(Tuple<string, string>)})
   .Configure();
```

### Service Integration

#### Dtos

```c#
namespace ExampleCode
{
	public class ExchangeRate
    {
        public Tuple<string, string> CurrencyPair { get; set; }
        public double Rate { get; set; }
    }

    [Route("/exchangerates/{currencypair}", "GET")]
    public class ExchangeRateRequest : IGetReturn<ExchangeRate>
    {
        [ApiMember(Name = "currencypair", IsRequired = true, ParameterType = "path")]
        public Tuple<string, string> CurrencyPair { get; set; }
    }
}
```

#### Request URL

This will request for the latest EUR-USD exchange rate:
```
http://host:port/exchangerates/EUR-USD
```

#### Response Body

With ServiceStack.Text.TupleSerializer:
```JSON
{
  "CurrencyPair": "EUR-USD",
  "Rate": 1.35
}
```

Without ServiceStack.Text.TupleSerializer:
```JSON
{
  "CurrencyPair": {
    "Item1": "EUR",
    "Item2": "USD"
  },
  "Rate": 1.35
}
```

## Notes
* This implementation does not support nested tuples.
* `.WithNamespaceFilter()` only applies to public tuples found in assemblies passed using `.WithAssemblies()`. Any tuples explicitly identified `.WithTupleTypes() ` will not be filtered by namespace.
* Multiple calls to `.WithTupleTypes()` are additive and will not replace previously specified values.
* Both `.WithTupleTypes()` and `.WithAssemblies()` may be used at the same time, the results will be combined.
* `Configure()` should be called before serializing/deserializing anything with `ServiceStack.Text` or the custom methods may not register correctly with `JsConfig`.

## Using the Code

* [Install the NuGet Package](https://nuget.org/packages/ServiceStack.Text.TupleSerializer/)
* You can check out the code and run build.bat.
  * It will create NuGet packages you can consume in `.\ReleasePackages` or you can directly use the resulting binaries. 
* Build requirements
  * .Net 4.0
  * Powershell 2.0
