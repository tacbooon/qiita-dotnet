using System.Net.Http.Headers;

namespace QiitaDotNet.Authentication;

/// <summary>
/// アクセストークンによる認証ヘッダ (<c>Authorization: Bearer &lt;token&gt;</c>) を付与する
/// <see cref="DelegatingHandler"/> です。
/// </summary>
/// <remarks>
/// <para>本ライブラリの <see cref="QiitaClient"/> はトークンを直接受け取らず、このハンドラーを組み込んだ <see cref="HttpClient"/> を受け取る設計を採用しています。</para>
/// <para>CWE-201 の対策として、送信先ホストは既定で Qiita 公式サーバー (<see cref="DefaultTrustedHosts"/>) のみを許可します。テスト目的でそれ以外のホストを使う場合は <see cref="AllowCustomHosts"/> に <c>true</c> を設定してください。</para>
/// <para>CWE-319 の対策として、既定では HTTPS 以外の要求にはトークンを付与せず送信前に拒否します。テスト目的で HTTP を使いたい場合は <see cref="AllowInsecureScheme"/> に <c>true</c> を設定してください。</para>
/// </remarks>
public sealed class QiitaAccessTokenHandler : DelegatingHandler
{
    private readonly string _accessToken;

    /// <summary>
    /// 既定でトークン送信を許可するホストの一覧です。
    /// </summary>
    /// <remarks>
    /// <para>リストには <c>qiita.com</c> (Qiita) と <c>*.qiita.com</c> (Qiita Team) が含まれています。</para>
    /// </remarks>
    public static IReadOnlyList<string> DefaultTrustedHosts { get; } = ["qiita.com", "*.qiita.com"];

    /// <summary>
    /// <see cref="DefaultTrustedHosts"/> 以外のホストへのトークン送信を許可するかどうかです。既定値は <c>false</c> です。
    /// </summary>
    /// <remarks>
    /// <para>テスト用のカスタムホストを使う場合のみ <c>true</c> にしてください。実運用では <c>false</c> のままにしてください。</para>
    /// </remarks>
    public bool AllowCustomHosts { get; set; }

    /// <summary>
    /// 平文 HTTP でのトークン送信を許可するかどうかです。既定値は <c>false</c> です。
    /// </summary>
    /// <remarks>
    /// <para>テスト用のローカルサーバー等でのみ <c>true</c> にしてください。実運用では <c>false</c> のまま HTTPS のみを使用してください。</para>
    /// </remarks>
    public bool AllowInsecureScheme { get; set; }

    /// <summary>
    /// 指定したアクセストークンを使ってハンドラーを初期化します。
    /// </summary>
    /// <param name="accessToken">Qiita のアクセストークンです。</param>
    public QiitaAccessTokenHandler(string accessToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        _accessToken = accessToken;
    }

    /// <summary>
    /// 指定したアクセストークンとインナーハンドラーを使ってハンドラーを初期化します。
    /// </summary>
    /// <param name="accessToken">Qiita のアクセストークンです。</param>
    /// <param name="innerHandler">内側の <see cref="HttpMessageHandler"/> です。</param>
    public QiitaAccessTokenHandler(string accessToken, HttpMessageHandler innerHandler)
        : base(innerHandler ?? throw new ArgumentNullException(nameof(innerHandler)))
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        _accessToken = accessToken;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException">URI スキーマが HTTPS 以外、またはホストが信頼対象外。</exception>
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var host = request.RequestUri?.Host;
        if (string.IsNullOrEmpty(host) || (!AllowCustomHosts && !IsDefaultHost(host)))
        {
            throw new InvalidOperationException(
                $"Bearer token transmission to untrusted host is blocked: '{request.RequestUri}'." +
                " Set AllowCustomHosts to true only to use custom endpoints for testing.");
        }

        if (!AllowInsecureScheme && !IsSecureScheme(request.RequestUri?.Scheme))
        {
            throw new InvalidOperationException(
                $"Bearer token transmission requires HTTPS: '{request.RequestUri}'." +
                " Set AllowInsecureScheme to true only to use plaintext HTTP for testing.");
        }

        request.Headers.Authorization ??= new AuthenticationHeaderValue("Bearer", _accessToken);
        return base.SendAsync(request, cancellationToken);
    }

    private static bool IsDefaultHost(string host) =>
        host.Equals("qiita.com", StringComparison.OrdinalIgnoreCase) ||
        host.EndsWith(".qiita.com", StringComparison.OrdinalIgnoreCase);

    private static bool IsSecureScheme(string? scheme) =>
        string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
}
