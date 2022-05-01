FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TripService/TripService.csproj", "TripService/"]
RUN dotnet restore "TripService/TripService.csproj"
COPY . .
WORKDIR "/src/TripService"
RUN dotnet build "TripService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TripService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TripService.dll"]