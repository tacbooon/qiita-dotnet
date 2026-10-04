using QiitaDotNet.Tags;
using QiitaDotNet.Users;

namespace QiitaDotNet;

/// <summary>
/// Qiita API v2 へのアクセスを提供するルートクライアントインタフェースです。
/// </summary>
public interface IQiitaClient
{
    /// <summary>
    /// タグリソース (<c>GET /api/v2/tags</c> 系) を扱うサブクライアントを取得します。
    /// </summary>
    ITagsClient Tags { get; }

    /// <summary>
    /// ユーザーリソース (<c>GET /api/v2/users</c> 系、<c>GET /api/v2/authenticated_user</c>) を扱うサブクライアントを取得します。
    /// </summary>
    IUsersClient Users { get; }
}
