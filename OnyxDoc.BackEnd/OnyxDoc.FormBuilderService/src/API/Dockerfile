FROM flowmono.azurecr.io/formbuilder-service-deps AS build

COPY ["/OnyxDoc.FormBuilderService/src/API/API.csproj", "/OnyxDoc.FormBuilderService/src/API/"]
COPY ["/OnyxDoc.FormBuilderService/src/Domain/Domain.csproj", "/OnyxDoc.FormBuilderService/src/Domain/"]
COPY ["/OnyxDoc.FormBuilderService/src/Application/Application.csproj", "/OnyxDoc.FormBuilderService/src/Application/"]
COPY ["/OnyxDoc.FormBuilderService/src/Infrastructure/Infrastructure.csproj", "/OnyxDoc.FormBuilderService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/OnyxDoc.FormBuilderService/src/API/API.csproj"

COPY . .
WORKDIR /OnyxDoc.FormBuilderService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
