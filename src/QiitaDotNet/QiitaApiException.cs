using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using QiitaDotNet.Serialization;

namespace QiitaDotNet;

/// <summary>
/// エラーレスポンスの型定義です。
/// </summary>
internal sealed record ErrorResponse(
    [property: JsonPropertyName("message")] string? Message,
    [property: JsonPropertyName("type")] string? Type);

/// <summary>
/// Qiita API v2 が失敗を示すレスポンスを返した場合にスローされる例外です。
/// </summary>
public sealed class QiitaApiException : HttpRequestException
{
    /// <summary>
    /// エラー詳細に含めるレスポンスボディのプレビュー最大文字数です。巨大なボディ全体を例外メッセージに含めないための上限です。
    /// </summary>
    private const int MaxBodyPreviewLength = 128;

    /// <summary>
    /// エラーの種別を示す文字列 (例: <c>not_found</c>) です。レスポンスボディに <c>type</c> プロパティが含まれない場合は <c>null</c> です。
    /// </summary>
    public string? ErrorType { get; }

    /// <summary>
    /// <see cref="QiitaApiException"/> クラスの新しいインスタンスを生成します。
    /// </summary>
    /// <param name="statusCode">HTTP ステータスコード。</param>
    /// <param name="errorType">エラーの種別を示す文字列。不明な場合は <c>null</c> 。</param>
    /// <param name="message">エラーメッセージ。</param>
    public QiitaApiException(HttpStatusCode statusCode, string? errorType, string message)
        : base(message, inner: null, statusCode)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// <see cref="QiitaApiException"/> クラスの新しいインスタンスを生成します。
    /// </summary>
    /// <param name="statusCode">HTTP ステータスコード。</param>
    /// <param name="errorType">エラーの種別を示す文字列。不明な場合は <c>null</c> 。</param>
    /// <param name="message">エラーメッセージ。</param>
    /// <param name="inner">内部例外。存在しない場合は <c>null</c> 。</param>
    public QiitaApiException(HttpStatusCode statusCode, string? errorType, string message, Exception? inner)
        : base(message, inner, statusCode)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// 失敗を示す <see cref="HttpResponseMessage"/> からインスタンスを生成します。
    /// </summary>
    /// <param name="response">失敗を示すレスポンス。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <exception cref="ArgumentNullException"><paramref name="response"/> が null です。</exception>
    /// <exception cref="OperationCanceledException">トークンによってリクエストがキャンセルされました。</exception>
    internal static async Task<QiitaApiException> FromResponseAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(response);

        string? errorMessage = null;
        string? errorType = null;
        string? errorDetails = null;

        // ボディ取得中の例外はハンドリングせず、そのまま呼び出し元に返します。
        var body = response.Content is not null
            ? await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)
            : null;

        // Qiita サーバーはエラー発生時に message と type を含む JSON オブジェクトを返します。
        // サーバーが正常に動作していればパースに失敗することはありませんが、
        // 障害発生に備えたフォールバックとして適当なエラーメッセージを生成して返します。
        if (!string.IsNullOrWhiteSpace(body))
        {
            try
            {
                var error = JsonSerializer.Deserialize(
                    body,
                    QiitaInternalJsonSerializerContext.Default.ErrorResponse);
                if (error?.Message is not null && error.Type is not null)
                {
                    errorMessage = error.Message;
                    errorType = error.Type;
                }
                else
                {
                    errorDetails = $"Invalid response body:\n{body.Truncate(MaxBodyPreviewLength)}";
                }
            }
            catch (JsonException)
            {
                errorDetails = $"Failed to parse response body:\n{body.Truncate(MaxBodyPreviewLength)}";
            }
        }
        else
        {
            errorDetails = "Empty response body.";
        }

        var method = response.RequestMessage?.Method?.ToString() ?? "UNKNOWN";
        var location = response.RequestMessage?.RequestUri?.ToString() ?? "unknown";
        var summary = $"{method} {location} failed with status code {(int)response.StatusCode} {response.StatusCode}";
        var message = errorDetails == null
            ? $"{summary}, type='{errorType}' and message='{errorMessage}'"
            : $"{summary}. {errorDetails}";

        return new QiitaApiException(response.StatusCode, errorType, message);
    }
}
