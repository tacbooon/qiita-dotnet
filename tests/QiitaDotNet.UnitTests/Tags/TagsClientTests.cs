using QiitaDotNet.Tags;
using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Tags;

/// <summary>
/// <see cref="QiitaDotNet.Tags.ITagsClient"/> のテストです。
/// </summary>
public class TagsClientTests
{
    [Test]
    public async Task ListTagsAsync_WithoutArgs_BuildsCorrectRequest()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Tags.ListTagsAsync();
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/tags");
        }
    }

    [Test]
    [Arguments(1, DisplayName = "page is minimum (1)")]
    [Arguments(100, DisplayName = "page is maximum (100)")]
    public async Task ListTagsAsync_WithPage_BuildsCorrectRequest(int page)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Tags.ListTagsAsync(page, null);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/tags?page={page}");
        }
    }

    [Test]
    [Arguments(1, DisplayName = "perPage is minimum (1)")]
    [Arguments(100, DisplayName = "perPage is maximum (100)")]
    public async Task ListTagsAsync_WithPerPage_BuildsCorrectRequest(int perPage)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Tags.ListTagsAsync(null, perPage);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/tags?per_page={perPage}");
        }
    }

    [Test]
    [Arguments(TagSort.Count, "count", DisplayName = "sort is count")]
    [Arguments(TagSort.Name, "name", DisplayName = "sort is name")]
    public async Task ListTagsAsync_WithSort_BuildsCorrectRequest(TagSort sort, string expected)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Tags.ListTagsAsync(sort: sort);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/tags?sort={expected}");
        }
    }

    [Test]
    public async Task ListTagsAsync_WithAllParameters_BuildsCorrectRequest()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Tags.ListTagsAsync(page: 2, perPage: 30, sort: TagSort.Count);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/tags?page=2&per_page=30&sort=count");
        }
    }

    [Test]
    public async Task ListTagsAsync_ResponseWithTags_ReturnsDeserializedTags()
    {
        var stub = new StubHandler { ResponseBody = TagJson.TagList };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Tags.ListTagsAsync();
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(2);
            await Assert.That(result[0].Id).IsEqualTo("qiita");
            await Assert.That(result[1].Id).IsEqualTo("null_icon");
        }
    }

    [Test]
    public async Task ListTagsAsync_ResponseWithEmptyArray_ReturnsEmptyList()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Tags.ListTagsAsync();
        await Assert.That(result).IsEmpty();
    }

    [Test]
    public async Task ListTagsAsync_ResponseWithNull_ReturnsEmptyList()
    {
        var stub = new StubHandler { ResponseBody = "null" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Tags.ListTagsAsync();
        await Assert.That(result).IsEmpty();
    }

    [Test]
    [Arguments(0, DisplayName = "page is below minimum (0)")]
    [Arguments(101, DisplayName = "page is above maximum (101)")]
    public async Task ListTagsAsync_OutOfRangePage_ThrowsArgumentOutOfRangeExceptionWithoutSending(int page)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Tags.ListTagsAsync(page, null))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("page");
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    [Arguments(0, DisplayName = "perPage is below minimum (0)")]
    [Arguments(101, DisplayName = "perPage is above maximum (101)")]
    public async Task ListTagsAsync_OutOfRangePerPage_ThrowsArgumentOutOfRangeExceptionWithoutSending(int perPage)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Tags.ListTagsAsync(null, perPage))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("perPage");
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    public async Task ListTagsAsync_UndefinedSort_ThrowsArgumentOutOfRangeExceptionWithoutSending()
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Tags.ListTagsAsync(sort: (TagSort)999))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("sort");
        await Assert.That(stub.LastRequest).IsNull();
    }
}
