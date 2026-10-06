using System.Text.Json;
using QiitaDotNet.Models;

namespace QiitaDotNet.Items;

/// <summary>
/// 記事リソース (<c>/api/v2/items</c>、<c>/api/v2/authenticated_user/items</c>) 用サブクライアントインタフェースです。
/// </summary>
public interface IItemsClient
{
    /// <summary>
    /// 認証中のユーザーの記事を作成日時の降順で取得します (<c>GET /api/v2/authenticated_user/items</c>)。
    /// </summary>
    /// <param name="page">ページ番号 (1〜100)。省略時はサーバー既定値の 1。</param>
    /// <param name="perPage">1ページあたりの件数 (1〜100)。省略時はサーバー既定値の 20。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>認証中のユーザーの記事一覧。</returns>
    /// <remarks>
    /// <para>このエンドポイントの呼び出しには認証が必要です。アクセストークンを設定していない場合は 401 エラー (<see cref="QiitaApiException"/>) となります。</para>
    /// </remarks>
    /// <exception cref="ArgumentException"><paramref name="page"/> または <paramref name="perPage"/> が範囲外。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信。</exception>
    Task<IReadOnlyList<Item>> ListAuthenticatedUserItemsAsync(
        int? page = null,
        int? perPage = null,
        CancellationToken cancellationToken = default);
}
