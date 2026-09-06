$ErrorActionPreference = "Stop"

dotnet publish "NPC Maker.csproj" -r win-x64 -c Release -o dist/win
dotnet publish "NPC Maker.csproj" -r linux-x64 -c Release -o dist/linux