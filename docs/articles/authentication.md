# 認証

## アクセストークンの設定

@QiitaDotNet.Authentication.QiitaAccessTokenHandler を組み込んだ @System.Net.Http.HttpClient を @QiitaDotNet.QiitaClient のコンストラクタに渡してください。

アクセストークンは OAuth を利用した認可フローか、Qiita のユーザー管理画面から発行できます。詳細については [Qiita API v2 ドキュメント](https://qiita.com/api/v2/docs) を参照してください。

```csharp
using QiitaDotNet;
using QiitaDotNet.Authentication;

var handler = new QiitaAccessTokenHandler("<access-token>", new HttpClientHandler());
var httpClient = new HttpClient(handler);
var client = new QiitaClient(httpClient);

var users = await client.Users.ListUsersAsync();
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}: {user.Name}");
}
```

## 認証なしで利用する場合の制限事項

認証なしで Qiita API v2 を利用する場合は `GET` メソッドのエンドポイントだけを利用できます。名前が `Get*`, `List*`, `Search*` で始まるメソッドがこれに該当します。`DELETE`, `PATCH`, `POST`, `PUT` メソッドを利用する他のメソッドの呼び出しには認証が必要です。

また、認証なしでは API 呼び出しが IP アドレス当たり 60 件／時間に制限されます。グローバル IP アドレスは複数ユーザーで共有されることが多いため、最初の API 呼び出しから失敗する可能性もあります。認証を行った場合はユーザー当たり 1000 件/時間に制限が緩和されます。

詳細については [Qiita API v2 ドキュメント](https://qiita.com/api/v2/docs) を参照してください。

## 送信先の制限

アクセストークン漏洩を防ぐため、@QiitaDotNet.Authentication.QiitaAccessTokenHandler は送信先を既定で以下に制限しています。

- ホストは `qiita.com` と `*.qiita.com`（Qiita Team）のみ許可します。それ以外のホストにアクセストークンを送信しようとすると @System.InvalidOperationException をスローします。
- スキームは HTTPS のみ許可します。HTTP でアクセストークンを送信しようとすると @System.InvalidOperationException をスローします。

テスト目的で上記制限を緩和する場合のみ、以下のプロパティを `true` にしてください。

- @QiitaDotNet.Authentication.QiitaAccessTokenHandler.AllowCustomHosts: 信頼ホスト以外への送信を許可します。
- @QiitaDotNet.Authentication.QiitaAccessTokenHandler.AllowInsecureScheme: 平文 HTTP での送信を許可します。
