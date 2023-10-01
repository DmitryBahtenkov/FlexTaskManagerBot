FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FTM.Telegram/FTM.Telegram.csproj", "FTM.Telegram/"]
COPY ["FTM.Infrastructure/FTM.Infrastructure.csproj", "FTM.Infrastructure/"]
COPY ["FTM.Domain/FTM.Domain.csproj", "FTM.Domain/"]
COPY ["Hors/Hors.csproj", "Hors/"]
RUN dotnet restore "FTM.Telegram/FTM.Telegram.csproj"
COPY . .
WORKDIR "/src/FTM.Telegram"
RUN dotnet build "FTM.Telegram.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FTM.Telegram.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FTM.Telegram.dll"]
