using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace QiitaDotNet;

/// <summary>
/// Qiita API v2 の HTTPS 通信を担う内部実装クラスです。
/// </summary>
internal sealed class ApiTransport
{
    /// <summary>
    /// <see cref="HttpClient.BaseAddress"/> が設定されていないときに使うフォールバック URI です。
    /// </summary>
    private static readonly Uri FallbackBaseAddress = new("https://qiita.com/", UriKind.Absolute);

    /// <summary>
    /// すべてのパスに自動付与される API プレフィックスです。
    /// </summary>
    private const string PathPrefix = "/api/v2";

    private readonly HttpClient _httpClient;

    /// <summary>
    /// 指定された <see cref="HttpClient"/> を使用して <see cref="ApiTransport"/> を初期化します。
    /// </summary>
    /// <param name="httpClient">API 呼び出しに使用する <see cref="HttpClient"/>。</param>
    internal ApiTransport(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _httpClient = httpClient;
    }

    /// <summary>
    /// GET リクエストを送信し、JSON レスポンスをデシリアライズします。
    /// </summary>
    /// <typeparam name="T">レスポンスの型。</typeparam>
    /// <param name="path">パス (例: <c>/users</c>)。</param>
    /// <param name="query">クエリパラメータ。</param>
    /// <param name="responseType">デシリアライズに使う型情報。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>デシリアライズ結果。ボディが JSON の null の場合は <c>null</c>。</returns>
    /// <exception cref="ArgumentException">引数が無効。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信</exception>
    internal async Task<T?> GetJsonAsync<T>(
        string path,
        QueryBuilder query,
        JsonTypeInfo<T> responseType,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(
                HttpMethod.Get, path, query, content: null, cancellationToken)
            .ConfigureAwait(false);

        await using var stream = await response.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync(stream, responseType, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// POST リクエストを JSON ボディ付きで送信し、JSON レスポンスをデシリアライズします。
    /// </summary>
    /// <typeparam name="TRequest">リクエストボディの型。</typeparam>
    /// <typeparam name="TResponse">レスポンスの型。</typeparam>
    /// <param name="path">パス (例: <c>/users</c>)。</param>
    /// <param name="body">リクエストボディ。</param>
    /// <param name="requestType">シリアライズに使う型情報。</param>
    /// <param name="responseType">デシリアライズに使う型情報。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>デシリアライズ結果。ボディが JSON の null の場合は <c>null</c>。</returns>
    /// <exception cref="ArgumentNullException">引数が null。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信</exception>
    internal async Task<TResponse?> PostJsonAsync<TRequest, TResponse>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestType,
        JsonTypeInfo<TResponse> responseType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var content = JsonContent.Create(body, requestType);
        using var response = await SendAsync(
                HttpMethod.Post, path, query: null, content, cancellationToken)
            .ConfigureAwait(false);

        await using var stream = await response.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync(stream, responseType, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// PATCH リクエストを JSON ボディ付きで送信し、JSON レスポンスをデシリアライズします。
    /// </summary>
    /// <typeparam name="TRequest">リクエストボディの型。</typeparam>
    /// <typeparam name="TResponse">レスポンスの型。</typeparam>
    /// <param name="path">パス (例: <c>/users</c>)。</param>
    /// <param name="body">リクエストボディ。</param>
    /// <param name="requestType">シリアライズに使う型情報。</param>
    /// <param name="responseType">デシリアライズに使う型情報。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>デシリアライズ結果。ボディが JSON の null の場合は <c>null</c>。</returns>
    /// <exception cref="ArgumentNullException">引数が null。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信</exception>
    internal async Task<TResponse?> PatchJsonAsync<TRequest, TResponse>(
        string path,
        TRequest body,
        JsonTypeInfo<TRequest> requestType,
        JsonTypeInfo<TResponse> responseType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        var content = JsonContent.Create(body, requestType);
        using var response = await SendAsync(
                HttpMethod.Patch, path, query: null, content, cancellationToken)
            .ConfigureAwait(false);

        await using var stream = await response.Content
            .ReadAsStreamAsync(cancellationToken)
            .ConfigureAwait(false);

        return await JsonSerializer.DeserializeAsync(stream, responseType, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// PUT リクエストを送信します。Qiita API v2 の PUT は送受信ともにボディがありません。
    /// </summary>
    /// <param name="path">パス (例: <c>/users</c>)。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <exception cref="ArgumentNullException">引数が null。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信</exception>
    internal async Task PutAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(
                HttpMethod.Put, path, query: null, content: null, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// DELETE リクエストを送信します。Qiita API v2 の DELETE は送受信ともにボディがありません。
    /// </summary>
    /// <param name="path">パス (例: <c>/users</c>)。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <exception cref="ArgumentNullException">引数が null。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信</exception>
    internal async Task DeleteAsync(
        string path,
        CancellationToken cancellationToken = default)
    {
        using var response = await SendAsync(
                HttpMethod.Delete, path, query: null, content: null, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <summary>
    /// リクエストの組み立てと送信を行います。
    /// </summary>
    private async Task<HttpResponseMessage> SendAsync(
        HttpMethod method,
        string path,
        QueryBuilder? query,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var baseAddress = _httpClient.BaseAddress ?? FallbackBaseAddress;
        var uri = new Uri(
            $"{baseAddress.ToString().TrimEnd('/')}{PathPrefix}{path}{query}",
            UriKind.Absolute);
        using var request = new HttpRequestMessage(method, uri);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (content is not null)
        {
            request.Content = content;
        }

        // ResponseContentRead を使うことでヘッダだけでなくボディ全体を読み込むまでここで待機します。
        // これにより、Timeout と MaxResponseContentBufferSize の設定がボディ読み込み完了まで効きます。
        // Qiita API のレスポンスボディは通常小さいため、バッファリングしても問題ありません。
        var response = await _httpClient
            .SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken)
            .ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            using (response)
            {
                throw await QiitaApiException
                    .FromResponseAsync(response, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        return response;
    }
}
