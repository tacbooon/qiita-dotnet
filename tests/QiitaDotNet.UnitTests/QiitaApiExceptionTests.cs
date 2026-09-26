using System.Net;

namespace QiitaDotNet.UnitTests;

/// <summary>
/// <see cref="QiitaApiException"/> のテストです。
/// </summary>
public class QiitaApiExceptionTests
{
    [Test]
    public async Task Constructor_SetsProperties()
    {
        var ex = new QiitaApiException(HttpStatusCode.NotFound, "not_found", "Not found");
        using (Assert.Multiple())
        {
            await Assert.That(ex.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
            await Assert.That(ex.ErrorType).IsEqualTo("not_found");
            await Assert.That(ex.Message).IsEqualTo("Not found");
        }
    }

    [Test]
    public async Task Constructor_WithInnerException_SetsInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var ex = new QiitaApiException(HttpStatusCode.BadGateway, errorType: null, "Failed", inner);
        using (Assert.Multiple())
        {
            await Assert.That(ex.InnerException).IsSameReferenceAs(inner);
            await Assert.That(ex.ErrorType).IsNull();
        }
    }

    [Test]
    public async Task CanBeCaughtAsHttpRequestException()
    {
        var ex = new QiitaApiException(HttpStatusCode.Unauthorized, "unauthorized", "Unauthorized");
        using (Assert.Multiple())
        {
            await Assert.That(ex).IsAssignableTo<HttpRequestException>();
            await Assert.That(ex).IsTypeOf<QiitaApiException>();
        }
    }
}
