FROM devappcontainers.azurecr.io/rubyreloaded/subscription-service-deps AS build

COPY ["/RubyReloaded.SubscriptionService/src/API/API.csproj", "/RubyReloaded.SubscriptionService/src/API/"]
COPY ["/RubyReloaded.SubscriptionService/src/Domain/Domain.csproj", "/RubyReloaded.SubscriptionService/src/Domain/"]
COPY ["/RubyReloaded.SubscriptionService/src/Application/Application.csproj", "/RubyReloaded.SubscriptionService/src/Application/"]
COPY ["/RubyReloaded.SubscriptionService/src/Infrastructure/Infrastructure.csproj", "/RubyReloaded.SubscriptionService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/RubyReloaded.SubscriptionService/src/API/API.csproj"

COPY . .
WORKDIR /RubyReloaded.SubscriptionService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
