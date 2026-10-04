using System.Text.Json;
using QiitaDotNet.Models;

namespace QiitaDotNet.Users;

/// <summary>
/// ユーザーリソース (<c>/api/v2/users</c>、<c>/api/v2/authenticated_user</c>) 用サブクライアントインタフェースです。
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
    /// <exception cref="ArgumentException"><paramref name="page"/> または <paramref name="perPage"/> が範囲外。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信。</exception>
    Task<IReadOnlyList<User>> ListUsersAsync(
        int? page = null,
        int? perPage = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 指定したユーザーを取得します (<c>GET /api/v2/users/:user_id</c>)。
    /// </summary>
    /// <param name="userId">ユーザー ID。</param>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>ユーザー。</returns>
    /// <remarks>
    /// <para>ユーザー ID は大文字・小文字を区別せずに照合されます。戻り値の <see cref="User.Id"/> はサーバー側で正規化された表記となります。例えば、引数に <c>qiita</c> を指定しても <c>Qiita</c> が返る可能性があります。</para>
    /// </remarks>
    /// <exception cref="ArgumentException"><paramref name="userId"/> が null、空文字、空白文字のみ、または <c>.</c>、<c>..</c>。</exception>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信。</exception>
    Task<User> GetUserAsync(
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// アクセストークンに紐付く認証中のユーザーを取得します (<c>GET /api/v2/authenticated_user</c>)。
    /// </summary>
    /// <param name="cancellationToken">キャンセル用トークン。</param>
    /// <returns>認証中のユーザー。</returns>
    /// <remarks>
    /// <para>このエンドポイントの呼び出しには認証が必要です。アクセストークンを設定していない場合は 401 エラー (<see cref="QiitaApiException"/>) となります。</para>
    /// </remarks>
    /// <exception cref="HttpRequestException">ネットワークエラー。</exception>
    /// <exception cref="JsonException">成功レスポンスのボディが不正な JSON。</exception>
    /// <exception cref="OperationCanceledException">キャンセルまたはタイムアウト。</exception>
    /// <exception cref="QiitaApiException">エラーレスポンスを受信。</exception>
    Task<AuthenticatedUser> GetAuthenticatedUserAsync(
        CancellationToken cancellationToken = default);
}
