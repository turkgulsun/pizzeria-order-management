﻿FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pizzeria.Console/Pizzeria.Console.csproj", "Pizzeria.Console/"]
RUN dotnet restore "Pizzeria.Console/Pizzeria.Console.csproj"
COPY . .
WORKDIR "/src/Pizzeria.Console"
RUN dotnet build "Pizzeria.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pizzeria.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pizzeria.Console.dll"]
