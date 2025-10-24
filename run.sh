dotnet nuget locals all --clear
rm -rf obj bin
dotnet restore
dotnet build
dotnet run