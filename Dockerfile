# Stage 1: build + publish
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY pokedex.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: runtime only (smaller image, no SDK)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "pokedex.dll"]
