# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias (mejora el cache)
COPY ["MsProducts/MsProducts.csproj", "MsProducts/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "MsProducts/MsProducts.csproj"

# Copiar todo el código y compilar
COPY . .
WORKDIR "/src/MsProducts"
RUN dotnet publish "MsProducts.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MsProducts.dll"]
