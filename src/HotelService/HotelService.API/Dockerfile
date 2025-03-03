#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/HotelService/HotelService.API/HotelService.API.csproj", "src/HotelService/HotelService.API/"]
COPY ["src/HotelService/HotelService.Application/HotelService.Application.csproj", "src/HotelService/HotelService.Application/"]
COPY ["src/HotelService/HotelService.Core/HotelService.Core.csproj", "src/HotelService/HotelService.Core/"]
COPY ["src/HotelService/HotelService.DataAccess/HotelService.DataAccess.csproj", "src/HotelService/HotelService.DataAccess/"]
RUN dotnet restore "./src/HotelService/HotelService.API/HotelService.API.csproj"
COPY . .
WORKDIR "/src/src/HotelService/HotelService.API"
RUN dotnet build "./HotelService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HotelService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelService.API.dll"]