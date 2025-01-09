
dotnet ef migrations add MigrationWithMaterialType --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj

dotnet ef Database Update --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj
