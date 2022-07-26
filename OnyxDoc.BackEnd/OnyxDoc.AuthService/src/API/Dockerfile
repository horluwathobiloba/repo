FROM flowmono.azurecr.io/auth-service-deps AS build

COPY ["/OnyxDoc.AuthService/src/API/API.csproj", "/OnyxDoc.AuthService/src/API/"]
COPY ["/OnyxDoc.AuthService/src/Domain/Domain.csproj", "/OnyxDoc.AuthService/src/Domain/"]
COPY ["/OnyxDoc.AuthService/src/Application/Application.csproj", "/OnyxDoc.AuthService/src/Application/"]
COPY ["/OnyxDoc.AuthService/src/Infrastructure/Infrastructure.csproj", "/OnyxDoc.AuthService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/OnyxDoc.AuthService/src/API/API.csproj"

COPY . .
WORKDIR /OnyxDoc.AuthService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
