namespace QiitaDotNet.UnitTests;

/// <summary>
/// <see cref="QiitaClient"/> のテストです。
/// </summary>
public class QiitaClientTests
{
    [Test]
    public async Task Constructor_NullHttpClient_ThrowsArgumentNullException()
    {
        await Assert.That(() => new QiitaClient(null!))
            .ThrowsExactly<ArgumentNullException>()
            .WithParameterName("httpClient");
    }

    [Test]
    public async Task Users_ReturnsSameInstance()
    {
        using var httpClient = new HttpClient(new StubHandler());
        var client = new QiitaClient(httpClient);
        await Assert.That(client.Users).IsSameReferenceAs(client.Users);
    }

    [Test]
    public async Task Tags_ReturnsSameInstance()
    {
        using var httpClient = new HttpClient(new StubHandler());
        var client = new QiitaClient(httpClient);
        await Assert.That(client.Tags).IsSameReferenceAs(client.Tags);
    }
}
