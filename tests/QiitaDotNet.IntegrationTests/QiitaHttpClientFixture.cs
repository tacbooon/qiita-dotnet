using QiitaDotNet.Authentication;
using TUnit.Core.Interfaces;

// ネットワークエラーやサーバーエラー発生に備えて、全てのテストに対して既定のタイムアウト時間を設けます。
[assembly: Timeout(10_000)]

// 製品サーバーへの負荷を抑制するため、テストは並列実行せず、逐次実行します。
[assembly: NotInParallel]

namespace QiitaDotNet.IntegrationTests;

/// <summary>
/// テストに使用する <see cref="QiitaClient"/> を生成するフィクスチャです。
/// </summary>
/// <remarks>
/// <para>環境変数 <c>QIITA_ACCESS_TOKEN</c> にアクセストークンを設定した場合は認証を有効化します。未設定の場合は認証なしで動作します。</para>
/// </remarks>
public sealed class QiitaHttpClientFixture : IAsyncInitializer, IAsyncDisposable
{
    /// <summary>アクセストークンをテストに渡すための環境変数です。</summary>
    public const string AccessTokenEnvironmentVariableName = "QIITA_ACCESS_TOKEN";

    /// <summary>テストで使用する <see cref="HttpClient"/> を取得します。テスト側で破棄しないでください。</summary>
    public HttpClient HttpClient { get; private set; } = null!;

    /// <summary>テストで使用する <see cref="QiitaClient"/> を取得します。</summary>
    public QiitaClient Client { get; private set; } = null!;

    /// <summary>テストで使用する <see cref="QiitaClient"/> を生成します。</summary>
    public Task InitializeAsync()
    {
        var token = Environment.GetEnvironmentVariable(AccessTokenEnvironmentVariableName);
        HttpMessageHandler innerHandler = new HttpClientHandler();
        HttpMessageHandler handler = string.IsNullOrWhiteSpace(token)
            ? innerHandler
            : new QiitaAccessTokenHandler(token, innerHandler);
        HttpClient = new HttpClient(handler);
        Client = new QiitaClient(HttpClient);
        return Task.CompletedTask;
    }

    /// <summary>共有していた <see cref="HttpClient"/> を破棄します。</summary>
    public ValueTask DisposeAsync()
    {
        HttpClient.Dispose();
        return ValueTask.CompletedTask;
    }
}
