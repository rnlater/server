
dotnet ef migrations add MigrationWithLearningList --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj

dotnet ef Database Update --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj
