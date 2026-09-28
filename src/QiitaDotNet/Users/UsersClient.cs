using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;

namespace QiitaDotNet.Users;

/// <summary>
/// <see cref="IUsersClient"/> の既定実装です。
/// </summary>
/// <remarks>
/// <para><see cref="QiitaClient"/> が生成して公開することを想定しています。</para>
/// </remarks>
internal sealed class UsersClient : IUsersClient
{
    private readonly ApiTransport _transport;

    /// <summary>
    /// ユーザーリソースを操作するためのサブクライアントを初期化します。
    /// </summary>
    /// <param name="transport">API 呼び出しに使用するトランスポート。</param>
    internal UsersClient(ApiTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<User>> ListUsersAsync(
        int? page = null,
        int? perPage = null,
        CancellationToken cancellationToken = default)
    {
        Pagination.Validate(page, perPage);

        var query = new QueryBuilder()
            .AddIfNotNull("page", page)
            .AddIfNotNull("per_page", perPage);

        return await _transport
                .GetJsonAsync(
                    "/users",
                    query,
                    QiitaJsonSerializerContext.Default.ListUser,
                    cancellationToken)
                .ConfigureAwait(false)
            ?? [];
    }

    /// <inheritdoc />
    public async Task<User> GetUserAsync(
        string userId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        if (userId is "." or "..")
        {
            throw new ArgumentException("userId must not be '.' or '..'.", nameof(userId));
        }

        var path = $"/users/{Uri.EscapeDataString(userId)}";
        var user = await _transport
                .GetJsonAsync(
                    path,
                    new QueryBuilder(),
                    QiitaJsonSerializerContext.Default.User,
                    cancellationToken)
                .ConfigureAwait(false);

        return user ?? throw new JsonException("Response body was null.");
    }
}
