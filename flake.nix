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
              dotnet tool restore > /dev/null
            '';
          };
        }
      );
    };
}
