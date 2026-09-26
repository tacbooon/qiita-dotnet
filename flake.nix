{
  description = "Dev shell for qiita-dotnet";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixpkgs-unstable";
  };

  outputs =
    {
      self,
      nixpkgs
    }:
    let
      forAllSystems = nixpkgs.lib.genAttrs [
        "aarch64-darwin"
        "aarch64-linux"
        "x86_64-linux"
      ];
    in
    {
      devShells = forAllSystems (
        system:
        let
          projectName = "qiita-dotnet";
          pkgs = import nixpkgs {
            inherit system;
            config.allowUnfree = true;
          };
        in
        {
          default = pkgs.mkShell {
            packages = with pkgs; [
              dotnetCorePackages.sdk_10_0
            ];

            DOTNET_CLI_TELEMETRY_OPTOUT = "1";
            DOTNET_NOLOGO = "true";
            DOTNET_ROOT = "${pkgs.dotnetCorePackages.sdk_10_0}/share/dotnet";

            shellHook = ''
              # 一時ディレクトリのパスが長すぎて mac で UNIX ソケットのパス長上限を超える問題のワークアラウンド
              export TMP="/tmp/${projectName}-''${UID:-$(id -u)}"
              export TMPDIR="$TMP";
              export TEMP="$TMP"
              export TEMPDIR="$TMP"
              mkdir -p -m 700 "$TMP" || exit 1
              chmod 700 "$TMP" || exit 1

              # .NET 関連ツールの復元を行うがエラーログだけを出力し、通常ログは破棄
              dotnet tool restore > /dev/null
            '';
          };
        }
      );
    };
}
