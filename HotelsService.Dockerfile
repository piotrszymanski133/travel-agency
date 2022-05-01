FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HotelsService/HotelsService.csproj", "HotelsService/"]
COPY ["CommonComponents/CommonComponents.csproj", "CommonComponents/"]
RUN dotnet restore "HotelsService/HotelsService.csproj"
COPY . .
WORKDIR "/src/HotelsService"
RUN dotnet build "HotelsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelsService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelsService.dll"]