FROM devappcontainers.azurecr.io/onyx/auth-service-deps AS build

COPY ["/Onyx.AuthService/src/API/API.csproj", "/Onyx.AuthService/src/API/"]
COPY ["/Onyx.AuthService/src/Domain/Domain.csproj", "/Onyx.AuthService/src/Domain/"]
COPY ["/Onyx.AuthService/src/Application/Application.csproj", "/Onyx.AuthService/src/Application/"]
COPY ["/Onyx.AuthService/src/Infrastructure/Infrastructure.csproj", "/Onyx.AuthService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/Onyx.AuthService/src/API/API.csproj"

COPY . .
WORKDIR /Onyx.AuthService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
