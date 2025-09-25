# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias (mejora el cache)
COPY ["MsTransactions/MsTransactions.csproj", "MsTransactions/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "MsTransactions/MsTransactions.csproj"

# Copiar todo el código y compilar
COPY . .
WORKDIR "/src/MsTransactions"
RUN dotnet publish "MsTransactions.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MsTransactions.dll"]
