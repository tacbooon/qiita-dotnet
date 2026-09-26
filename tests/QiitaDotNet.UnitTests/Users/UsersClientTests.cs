using QiitaDotNet.UnitTests.TestData;

namespace QiitaDotNet.UnitTests.Users;

/// <summary>
/// <see cref="QiitaDotNet.Users.IUsersClient"/> のテストです。
/// </summary>
public class UsersClientTests
{
    [Test]
    public async Task ListUsersAsync_WithoutArgs_BuildsCorrectRequest()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync();
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/users");
        }
    }

    [Test]
    [Arguments(1, DisplayName = "page is minimum (1)")]
    [Arguments(100, DisplayName = "page is maximum (100)")]
    public async Task ListUsersAsync_WithPage_BuildsCorrectRequest(int page)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync(page, null);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/users?page={page}");
        }
    }

    [Test]
    [Arguments(1, DisplayName = "perPage is minimum (1)")]
    [Arguments(100, DisplayName = "perPage is maximum (100)")]
    public async Task ListUsersAsync_WithPerPage_BuildsCorrectRequest(int perPage)
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync(null, perPage);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo($"https://qiita.com/api/v2/users?per_page={perPage}");
        }
    }

    [Test]
    public async Task ListUsersAsync_WithAllParameters_BuildsCorrectRequest()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await client.Users.ListUsersAsync(page: 2, perPage: 30);
        using (Assert.Multiple())
        {
            await Assert.That(stub.LastRequest?.Method).IsEqualTo(HttpMethod.Get);
            await Assert.That(stub.LastRequest?.RequestUri?.ToString())
                .IsEqualTo("https://qiita.com/api/v2/users?page=2&per_page=30");
        }
    }

    [Test]
    public async Task ListUsersAsync_ResponseWithUsers_ReturnsDeserializedUsers()
    {
        var stub = new StubHandler { ResponseBody = UserJson.UserList };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Users.ListUsersAsync();
        using (Assert.Multiple())
        {
            await Assert.That(result).Count().IsEqualTo(2);
            await Assert.That(result[0].Id).IsEqualTo("typical");
            await Assert.That(result[1].Id).IsEqualTo("with_nulls");
        }
    }

    [Test]
    public async Task ListUsersAsync_ResponseWithEmptyArray_ReturnsEmptyList()
    {
        var stub = new StubHandler { ResponseBody = "[]" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Users.ListUsersAsync();
        await Assert.That(result).IsEmpty();
    }

    [Test]
    public async Task ListUsersAsync_ResponseWithNull_ReturnsEmptyList()
    {
        var stub = new StubHandler { ResponseBody = "null" };
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        var result = await client.Users.ListUsersAsync();
        await Assert.That(result).IsEmpty();
    }

    [Test]
    [Arguments(0, DisplayName = "page is below minimum (0)")]
    [Arguments(101, DisplayName = "page is above maximum (101)")]
    public async Task ListUsersAsync_OutOfRangePage_ThrowsArgumentOutOfRangeExceptionWithoutSending(int page)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Users.ListUsersAsync(page, null))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("page");
        await Assert.That(stub.LastRequest).IsNull();
    }

    [Test]
    [Arguments(0, DisplayName = "perPage is below minimum (0)")]
    [Arguments(101, DisplayName = "perPage is above maximum (101)")]
    public async Task ListUsersAsync_OutOfRangePerPage_ThrowsArgumentOutOfRangeExceptionWithoutSending(int perPage)
    {
        var stub = new StubHandler();
        using var httpClient = new HttpClient(stub);
        var client = new QiitaClient(httpClient);
        await Assert.That(async () => await client.Users.ListUsersAsync(null, perPage))
            .ThrowsExactly<ArgumentOutOfRangeException>()
            .WithParameterName("perPage");
        await Assert.That(stub.LastRequest).IsNull();
    }
}
