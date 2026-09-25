using QiitaDotNet.Users;

namespace QiitaDotNet;

/// <summary>
/// Qiita API v2 へのアクセスを提供するルートクライアントインタフェースです。
/// </summary>
public interface IQiitaClient
{
    /// <summary>
    /// ユーザーリソース (<c>GET /api/v2/users</c> 系) を扱うサブクライアントを取得します。
    /// </summary>
    IUsersClient Users { get; }
}
