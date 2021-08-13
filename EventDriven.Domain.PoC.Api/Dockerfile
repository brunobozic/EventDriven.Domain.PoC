#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://+:5000;https://+:5001
EXPOSE 5000
Expose 5001

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