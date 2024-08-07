dotnet ef migrations add Initial --startup-project ..\DevOps.Api\DevOps.Api.csproj
dotnet ef database update --startup-project ..\DevOps.Api\DevOps.Api.csproj
