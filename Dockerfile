FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Tandem.Api/Tandem.Api.csproj", "src/Tandem.Api/"]
COPY ["src/Tandem.Application/Tandem.Application.csproj", "src/Tandem.Application/"]
COPY ["src/Tandem.Domain/Tandem.Domain.csproj", "src/Tandem.Domain/"]
COPY ["src/Tandem.Infrastructure/Tandem.Infrastructure.csproj", "src/Tandem.Infrastructure/"]
RUN dotnet restore "src/Tandem.Api/Tandem.Api.csproj"
COPY . .
WORKDIR "/src/src/Tandem.Api"
RUN dotnet build "Tandem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tandem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tandem.Api.dll"]

