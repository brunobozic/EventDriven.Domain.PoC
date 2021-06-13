#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["EventDriven.Domain.PoC.Api/EventDriven.Domain.PoC.Api.csproj", "EventDriven.Domain.PoC.Api/"]
RUN dotnet restore "EventDriven.Domain.PoC.Api/EventDriven.Domain.PoC.Api.csproj"
COPY . .
WORKDIR "/src/EventDriven.Domain.PoC.Api"
RUN dotnet build "EventDriven.Domain.PoC.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventDriven.Domain.PoC.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventDriven.Domain.PoC.Api.dll"]