FROM flowmono.azurecr.io/document-service-deps AS build

COPY ["/OnyxDoc.DocumentService/src/API/API.csproj", "/OnyxDoc.DocumentService/src/API/"]
COPY ["/OnyxDoc.DocumentService/src/Domain/Domain.csproj", "/OnyxDoc.DocumentService/src/Domain/"]
COPY ["/OnyxDoc.DocumentService/src/Application/Application.csproj", "/OnyxDoc.DocumentService/src/Application/"]
COPY ["/OnyxDoc.DocumentService/src/Infrastructure/Infrastructure.csproj", "/OnyxDoc.DocumentService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/OnyxDoc.DocumentService/src/API/API.csproj"

COPY . .
WORKDIR /OnyxDoc.DocumentService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
