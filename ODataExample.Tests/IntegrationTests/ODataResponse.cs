using Newtonsoft.Json;
using ODataExample.Web.Models;

namespace ODataExample.Tests.IntegrationTests;

public record ODataResponse
{
    [JsonProperty("@odata.context")]
    public string Context { get; set; }
    
    [JsonProperty("@odata.count")]
    public string Count { get; set; }
    
    public List<Book> Value { get; set; }
}