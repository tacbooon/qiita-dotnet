using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Items;

/// <summary>
/// <see cref="QiitaDotNet.Items.IItemsClient"/> のテストです。
/// </summary>
public class ItemsClientTests
{
    [Test]
    public async Task ListAuthenticatedUserItemsAsync_WithoutArgs_BuildsCorrectRequest()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Items.ListAuthenticatedUserItemsAsync();
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/authenticated_user/items");
        }
    }

    [Test]
    [Arguments(1, DisplayName = "page is minimum (1)")]
    [Arguments(100, DisplayName = "page is maximum (100)")]
    public async Task ListAuthenticatedUserItemsAsync_WithPage_BuildsCorrectRequest(int page)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Items.ListAuthenticatedUserItemsAsync(page, null);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/authenticated_user/items?page={page}");
        }
    }

    [Test]
    [Arguments(1, DisplayName = "perPage is minimum (1)")]
    [Arguments(100, DisplayName = "perPage is maximum (100)")]
    public async Task ListAuthenticatedUserItemsAsync_WithPerPage_BuildsCorrectRequest(int perPage)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Items.ListAuthenticatedUserItemsAsync(null, perPage);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/authenticated_user/items?per_page={perPage}");
        }
    }

    [Test]
    public async Task ListAuthenticatedUserItemsAsync_WithAllParameters_BuildsCorrectRequest()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Items.ListAuthenticatedUserItemsAsync(page: 2, perPage: 30);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/authenticated_user/items?page=2&per_page=30");
        }
    }

    [Test]
    public async Task ListAuthenticatedUserItemsAsync_ResponseWithItems_ReturnsDeserializedItems()
    {
        var stub = new StubHandler { ResponseBody = ItemJson.ItemList };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Items.ListAuthenticatedUserItemsAsync();
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(2);
            await Assert.That(result[0].Id).IsEqualTo("c686397e4a0f4f11683d");
            await Assert.That(result[0].Title).IsEqualTo("Typical item title");
            await Assert.That(result[1].Id).IsEqualTo("b486397e4a0f4f11683e");
            await Assert.That(result[1].Title).IsEqualTo("Nulls item title");
        }
    }

    [Test]
    public async Task ListAuthenticatedUserItemsAsync_ResponseWithEmptyArray_ReturnsEmptyList()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Items.ListAuthenticatedUserItemsAsync();
        await Assert.That(result).IsEmpty();
    }

    [Test]
    public async Task ListAuthenticatedUserItemsAsync_ResponseWithNull_ReturnsEmptyList()
    {
        var stub = new StubHandler { ResponseBody = "null" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Items.ListAuthenticatedUserItemsAsync();
        await Assert.That(result).IsEmpty();
    }

    [Test]
    [Arguments(0, DisplayName = "page is below minimum (0)")]
    [Arguments(101, DisplayName = "page is above maximum (101)")]
    public async Task ListAuthenticatedUserItemsAsync_OutOfRangePage_ThrowsArgumentOutOfRangeExceptionWithoutSending(int page)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Items.ListAuthenticatedUserItemsAsync(page, null))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("page");
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    [Arguments(0, DisplayName = "perPage is below minimum (0)")]
    [Arguments(101, DisplayName = "perPage is above maximum (101)")]
    public async Task ListAuthenticatedUserItemsAsync_OutOfRangePerPage_ThrowsArgumentOutOfRangeExceptionWithoutSending(int perPage)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Items.ListAuthenticatedUserItemsAsync(null, perPage))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("perPage");
        await Assert.That(stub.LastRequest).IsNull();
    }
}
