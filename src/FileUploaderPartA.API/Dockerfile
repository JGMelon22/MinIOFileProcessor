FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/FileUploaderPartA.API/FileUploaderPartA.API.csproj", "src/FileUploaderPartA.API/"]
COPY ["src/FileUploaderPartA.Application/FileUploaderPartA.Application.csproj", "src/FileUploaderPartA.Application/"]
COPY ["src/FileUploaderPartA.Core/FileUploaderPartA.Core.csproj", "src/FileUploaderPartA.Core/"]
COPY ["src/FileUploaderPartA.Infrastructure/FileUploaderPartA.Infrastructure.csproj", "src/FileUploaderPartA.Infrastructure/"]
RUN dotnet restore "src/FileUploaderPartA.API/FileUploaderPartA.API.csproj"
COPY . .
WORKDIR "/src/src/FileUploaderPartA.API"
RUN dotnet build "FileUploaderPartA.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileUploaderPartA.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileUploaderPartA.API.dll"]
