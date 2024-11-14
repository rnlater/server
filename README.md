
dotnet ef migrations add InitMigrationWithAuth --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj

dotnet ef migrations add MigrationWithLearningGame --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj

dotnet ef Database Update --project src/Infrastructure/Infrastructure.csproj --startup-project src/Endpoint/Endpoint.csproj
