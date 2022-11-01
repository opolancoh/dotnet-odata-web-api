using System.Text.Json;

namespace ODataExample.Tests;

public static class Utilities
{
    public static T? ODataPayloadDeserializer<T>(string json)
    {
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
        var jsonElementData = jsonElement.GetProperty("value");
        return jsonElementData.Deserialize<T>();
    }
}