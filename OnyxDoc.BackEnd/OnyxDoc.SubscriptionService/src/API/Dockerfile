FROM flowmono.azurecr.io/subscription-service-deps AS build

COPY ["/OnyxDoc.SubscriptionService/src/API/API.csproj", "/OnyxDoc.SubscriptionService/src/API/"]
COPY ["/OnyxDoc.SubscriptionService/src/Domain/Domain.csproj", "/OnyxDoc.SubscriptionService/src/Domain/"]
COPY ["/OnyxDoc.SubscriptionService/src/Application/Application.csproj", "/OnyxDoc.SubscriptionService/src/Application/"]
COPY ["/OnyxDoc.SubscriptionService/src/Infrastructure/Infrastructure.csproj", "/OnyxDoc.SubscriptionService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/OnyxDoc.SubscriptionService/src/API/API.csproj"

COPY . .
WORKDIR /OnyxDoc.SubscriptionService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
