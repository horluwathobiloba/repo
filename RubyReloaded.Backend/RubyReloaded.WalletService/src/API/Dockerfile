FROM devappcontainers.azurecr.io/rubyreloaded/wallet-service-deps AS build

COPY ["/RubyReloaded.WalletService/src/API/API.csproj", "/RubyReloaded.WalletService/src/API/"]
COPY ["/RubyReloaded.WalletService/src/Domain/Domain.csproj", "/RubyReloaded.WalletService/src/Domain/"]
COPY ["/RubyReloaded.WalletService/src/Application/Application.csproj", "/RubyReloaded.WalletService/src/Application/"]
COPY ["/RubyReloaded.WalletService/src/Infrastructure/Infrastructure.csproj", "/RubyReloaded.WalletService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/RubyReloaded.WalletService/src/API/API.csproj"

COPY . .
WORKDIR /RubyReloaded.WalletService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
