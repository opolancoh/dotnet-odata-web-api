using System.Net;
using System.Text.Json;
using ODataExample.Web.Data;
using ODataExample.Web.Models;
using Xunit.Abstractions;

namespace ODataExample.Tests.IntegrationTests;

public class BookOdataIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const string BasePath = "/odata/books";

    public BookOdataIntegrationTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _httpClient = factory.CreateClient();
    }

    #region GetAll

    [Fact]
    public async Task GeAll_ShouldReturnCountMetadata()
    {
        var response = await _httpClient.GetAsync($"{BasePath}?$count=true");
        var payloadString = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(payloadString);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(DbSeeder.Books.Count, jsonElement.GetProperty("@odata.count").GetInt32());
    }

    [Fact]
    public async Task GeAll_ShouldReturnAllBooksWithAllProperties()
    {
        var response = await _httpClient.GetAsync($"{BasePath}");
        var payloadString = await response.Content.ReadAsStringAsync();
        var payloadDataObject = Utilities.ODataPayloadDeserializer<List<Book>>(payloadString);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(DbSeeder.Books.Count, payloadDataObject?.Count);

        // Check for returned properties and values
        var expectedItem = DbSeeder.Books.SingleOrDefault(x => x.Id == DbSeeder.BookId1);
        var actualItem = payloadDataObject?.SingleOrDefault(x => x.Id == DbSeeder.BookId1);
        Assert.NotNull(expectedItem);
        Assert.NotNull(actualItem);
        Assert.Equal(expectedItem?.Id, actualItem?.Id);
        Assert.Equal(expectedItem?.Title, actualItem?.Title);
        Assert.Equal(expectedItem?.PublishedOn, actualItem?.PublishedOn);
    }

    [Fact]
    public async Task GeAll_ShouldExpandReviews()
    {
        var response = await _httpClient.GetAsync($"{BasePath}?expand=reviews");
        var payloadString = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(payloadString);
        var jsonElementData = jsonElement.GetProperty("value");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Check single element
        var expectedElement = DbSeeder.Reviews.Single(x => x.Comment == "Comment 01");
        var actualElement = jsonElementData[0].GetProperty("Reviews")[0];

        Assert.Equal(expectedElement.Id, actualElement.GetProperty("Id").GetGuid());
        Assert.Equal(expectedElement.Comment, actualElement.GetProperty("Comment").GetString());
        Assert.Equal(expectedElement.Rating, actualElement.GetProperty("Rating").GetInt32());
    }

    [Theory]
    [MemberData(nameof(SelectedAndNotSelectedProperties))]
    public async Task GeAll_ShouldReturnAllBooksWithSelectedProperties(string[] selectedProperties, string[] notSelectedProperties)
    {
        var response = await _httpClient.GetAsync($"{BasePath}?$select={string.Join(",", selectedProperties)}");
        var payloadString = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(payloadString);
        var jsonElementData = jsonElement.GetProperty("value");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(DbSeeder.Books.Count, jsonElementData.GetArrayLength());

        // Check for selected or not selected properties
        var actualElement = jsonElementData[0];

        // Selected properties
        Assert.Equal(selectedProperties.Count(), actualElement.EnumerateObject().Count());
        Assert.All(selectedProperties, x => Assert.True(actualElement.TryGetProperty(x, out _)));

        // Not selected properties
        Assert.All(notSelectedProperties, x => Assert.False(actualElement.TryGetProperty(x, out _)));
    }

    [Theory]
    [MemberData(nameof(FilterConditions))]
    public async Task GeAll_ShouldFilterData(int expectedDataLength, string filter)
    {
        var response = await _httpClient.GetAsync($"{BasePath}?$filter={filter}");
        var payloadString = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(payloadString);
        var jsonElementData = jsonElement.GetProperty("value");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedDataLength, jsonElementData.GetArrayLength());
    }
    
    [Theory]
    [MemberData(nameof(PagingConditions))]
    public async Task GeAll_ShouldPagingData(int expectedDataLength, string pagingValue)
    {
        var response = await _httpClient.GetAsync($"{BasePath}?{pagingValue}");
        var payloadString = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(payloadString);
        var jsonElementData = jsonElement.GetProperty("value");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expectedDataLength, jsonElementData.GetArrayLength());
    }

    #endregion

    public static TheoryData<string[], string[]> SelectedAndNotSelectedProperties => new()
    {
        // 0: Selected properties | 1: Not selected properties
        {new[] {"Id", "Title", "PublishedOn"}, Array.Empty<string>()},
        {new[] {"Id"}, new[] {"Title", "PublishedOn"}},
        {new[] {"Title"}, new[] {"Id", "PublishedOn"}},
        {new[] {"PublishedOn"}, new[] {"Id", "Title"}},
        {new[] {"Id", "Title"}, new[] {"PublishedOn"}},
        {new[] {"Id", "PublishedOn"}, new[] {"Title"}},
        {new[] {"Title", "PublishedOn"}, new[] {"Id"}},
    };

    public static TheoryData<int, string> FilterConditions => new()
    {
        {1, "contains(title,'01')"},
        {2, "contains(title,'10')"},
        {4, "contains(title,'Book')"},
        {4, "startsWith(title,'B')"},
        {1, "title eq 'Book 101'"},
    };
    
    public static TheoryData<int, string> PagingConditions => new()
    {
        {2, "$top=2&$skip=1"},
        {3, "$top=3&$skip=0"},
        {2, "$top=3&$skip=2"},
        {4, "$top=4&$skip=0"},
        {0, "$top=4&$skip=4"},
    };
}