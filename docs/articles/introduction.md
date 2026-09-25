# はじめに

QiitaDotNet は [Qiita API v2](https://qiita.com/api/v2/docs) を .NET から呼び出すためのクライアントライブラリです。

## 対象フレームワーク

- `net10.0`

## 機能

### 認証

アクセストークンを利用した Bearer 認証と、認証なしでのリクエストをサポートします。詳細については [認証](authentication.md) を参照してください。

### リクエスト

現時点でサポートしているリクエストは以下の通りです。

| サブクライアント                                          | メソッド                          | HTTP メソッド | エンドポイント                   |
|-----------------------------------------------------------|-----------------------------------|---------------|----------------------------------|
| @QiitaDotNet.Users.IUsersClient                           | `ListUsersAsync`                  | `GET`         | `/api/v2/users`                  |

## ライセンス

このプロジェクトは [MIT License](https://opensource.org/licenses/MIT) に基づいて公開されています。詳細については [LICENSE](https://github.com/tacbooon/qiita-dotnet/blob/main/LICENSE) を参照してください。
