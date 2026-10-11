$ErrorActionPreference = "Stop"

dotnet publish "NPC Maker" -r win-x64 -c Release -o dist/win
dotnet publish "NPC Maker" -r linux-x64 -c Release -o dist/linux
#dotnet publish "NPC Maker" -r osx-x64 -c Release -o dist/osx-x64
#dotnet publish "NPC Maker" -r osx-arm64 -c Release -o dist/osx-arm