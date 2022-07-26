FROM devappcontainers.azurecr.io/rubyreloaded/auth-service-deps AS build

COPY ["/RubyReloaded.AuthService/src/API/API.csproj", "/RubyReloaded.AuthService/src/API/"]
COPY ["/RubyReloaded.AuthService/src/Domain/Domain.csproj", "/RubyReloaded.AuthService/src/Domain/"]
COPY ["/RubyReloaded.AuthService/src/Application/Application.csproj", "/RubyReloaded.AuthService/src/Application/"]
COPY ["/RubyReloaded.AuthService/src/Infrastructure/Infrastructure.csproj", "/RubyReloaded.AuthService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/RubyReloaded.AuthService/src/API/API.csproj"

COPY . .
WORKDIR /RubyReloaded.AuthService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
