using QiitaDotNet.Users;

namespace QiitaDotNet;

/// <summary>
/// Qiita API v2 用のルートクライアントです。各リソースへの操作はサブクライアントを通じて行います。
/// </summary>
public sealed class QiitaClient : IQiitaClient
{
    private readonly ApiTransport _transport;
    private readonly Lazy<IUsersClient> _users;

    /// <summary>
    /// ユーザーリソース (<c>GET /api/v2/users</c> 系) を扱うサブクライアントを取得します。
    /// </summary>
    public IUsersClient Users => _users.Value;

    /// <summary>
    /// 指定した <see cref="HttpClient"/> を使って <see cref="QiitaClient"/> を初期化します。
    /// </summary>
    /// <param name="httpClient">API 呼び出しに使用する <see cref="HttpClient"/>。</param>
    /// <remarks>
    /// <para>引数で受け取った <paramref name="httpClient"/> の寿命管理は呼び出し元に委ねられます。</para>
    /// <para><paramref name="httpClient"/> に <see cref="HttpClient.BaseAddress"/> が未設定の場合、リクエスト送信時に <c>https://qiita.com/</c> をベースアドレスとして使用します。Qiita Team を使用する場合は <c>https://&lt;team-id&gt;.qiita.com/</c> を <see cref="HttpClient.BaseAddress"/> に設定してください。</para>
    /// <para>このクラスは認証を扱いません。アクセストークンを使用する場合は <see cref="Authentication.QiitaAccessTokenHandler"/> を組み込んだ <see cref="HttpClient"/> を外部で構成してからコンストラクタに渡してください。</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// using QiitaDotNet;
    /// using QiitaDotNet.Authentication;
    ///
    /// var handler = new QiitaAccessTokenHandler("&lt;access-token&gt;", new HttpClientHandler());
    /// var httpClient = new HttpClient(handler);
    /// var client = new QiitaClient(httpClient);
    /// </code>
    /// </example>
    public QiitaClient(HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        _transport = new ApiTransport(httpClient);
        _users = new Lazy<IUsersClient>(
            () => new UsersClient(_transport),
            LazyThreadSafetyMode.ExecutionAndPublication);
    }
}
