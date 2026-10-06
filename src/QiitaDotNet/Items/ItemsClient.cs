using System.Text.Json;
using QiitaDotNet.Models;
using QiitaDotNet.Serialization;

namespace QiitaDotNet.Items;

/// <summary>
/// <see cref="IItemsClient"/> の既定実装です。
/// </summary>
/// <remarks>
/// <para><see cref="QiitaClient"/> が生成して公開することを想定しています。</para>
/// </remarks>
internal sealed class ItemsClient : IItemsClient
{
    private readonly ApiTransport _transport;

    /// <summary>
    /// 記事リソースを操作するためのサブクライアントを初期化します。
    /// </summary>
    /// <param name="transport">API 呼び出しに使用するトランスポート。</param>
    internal ItemsClient(ApiTransport transport)
    {
        ArgumentNullException.ThrowIfNull(transport);
        _transport = transport;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<Item>> ListAuthenticatedUserItemsAsync(
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
                    "/authenticated_user/items",
                    query,
                    QiitaJsonSerializerContext.Default.ListItem,
                    cancellationToken)
                .ConfigureAwait(false)
            ?? [];
    }
}
