FROM mcr.microsoft.com/dotnet/core/sdk:5.0.100-preview.3 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MS.Underfloor.Api.dll"]