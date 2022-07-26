FROM devappcontainers.azurecr.io/onyx/contract-service-deps AS build

COPY ["/Onyx.ContractService/src/API/API.csproj", "/Onyx.ContractService/src/API/"]
COPY ["/Onyx.ContractService/src/Domain/Domain.csproj", "/Onyx.ContractService/src/Domain/"]
COPY ["/Onyx.ContractService/src/Application/Application.csproj", "/Onyx.ContractService/src/Application/"]
COPY ["/Onyx.ContractService/src/Infrastructure/Infrastructure.csproj", "/Onyx.ContractService/src/Infrastructure/"]
COPY ["/ReventInject/ReventInject.csproj", "/ReventInject/ReventInject.csproj"]

RUN dotnet restore "/Onyx.ContractService/src/API/API.csproj"

COPY . .
WORKDIR /Onyx.ContractService/src/API

RUN dotnet build "API.csproj" -c Release -o /app/build
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.15-buster-slim AS final
ENV TZ Africa/Lagos
WORKDIR /app
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "API.dll"]
