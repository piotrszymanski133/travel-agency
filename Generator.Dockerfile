FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Generator/Generator.csproj", "Generator/"]
COPY ["CommonComponents/CommonComponents.csproj", "CommonComponents/"]
RUN dotnet restore "Generator/Generator.csproj"
COPY . .
WORKDIR "/src/Generator"
RUN dotnet build "Generator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Generator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Generator.dll"]