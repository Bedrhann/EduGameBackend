# 1. Aşama: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyalarını kopyala ve restore et (Cache optimizasyonu için önce csproj'lar)
COPY ["EduGame.API/EduGame.API.csproj", "EduGame.API/"]
COPY ["EduGame.Core/EduGame.Core.csproj", "EduGame.Core/"]
COPY ["EduGame.Data/EduGame.Data.csproj", "EduGame.Data/"]
COPY ["EduGame.Service/EduGame.Service.csproj", "EduGame.Service/"]

RUN dotnet restore "EduGame.API/EduGame.API.csproj"

# Tüm kodları kopyala
COPY . .
WORKDIR "/src/EduGame.API"
RUN dotnet build "EduGame.API.csproj" -c Release -o /app/build

# Yayınlama (Publish)
FROM build AS publish
RUN dotnet publish "EduGame.API.csproj" -c Release -o /app/publish

# 2. Aşama: Runtime (Çalıştırma - Daha küçük imaj boyutu için)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EduGame.API.dll"]