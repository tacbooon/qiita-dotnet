using System.Text.Json;
using QiitaDotNet.Models;

namespace QiitaDotNet.Tags;

/// <summary>
/// タグリソース (<c>/api/v2/tags</c>) 用サブクライアントインタフェースです。
/// </summary>
public interface ITagsClient
{
    /// <summary>
    /// タグ一覧を取得します (<c>GET /api/v2/tags</c>)。
    /// </summary>
    /// <param name="page">ページ番号 (1〜100)。省略時はサーバー既定値の 1。</param>
    /// <param name="perPage">1ページあたりの件数 (1〜100)。省略時はサーバー既定値の 20。</param>
    /// <param name="sort">ソート順。省略時はサーバー既定の新着順。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>タグの一覧。</returns>
    /// <exception cref="ArgumentException"><paramref name="page"/>、<paramref name="perPage"/>、または <paramref name="sort"/> が範囲外。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信。</exception>
    Task<IReadOnlyList<Tag>> ListTagsAsync(
        int? page = null,
        int? perPage = null,
        TagSort? sort = null,
        CancellationToken cancellationToken = default);
}
