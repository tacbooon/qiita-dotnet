using System.Text.Json;
using QiitaDotNet.Models;

namespace QiitaDotNet.Users;

/// <summary>
/// ユーザーリソース (<c>/api/v2/users</c>) 用サブクライアントインタフェースです。
/// </summary>
public interface IUsersClient
{
    /// <summary>
    /// ユーザーを作成日時の降順で取得します (<c>GET /api/v2/users</c>)。
    /// </summary>
    /// <param name="page">ページ番号 (1〜100)。省略時はサーバー既定値の 1。</param>
    /// <param name="perPage">1ページあたりの件数 (1〜100)。省略時はサーバー既定値の 20。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>ユーザーの一覧。</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="page"/> または <paramref name="perPage"/> が範囲外。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信。</exception>
    Task<IReadOnlyList<User>> ListUsersAsync(
        int? page = null,
        int? perPage = null,
        CancellationToken cancellationToken = default);
}
