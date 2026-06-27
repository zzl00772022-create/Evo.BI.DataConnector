FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Evo.BI.DataConnector.csproj", "./"]
RUN dotnet restore "Evo.BI.DataConnector.csproj"

COPY . .
RUN dotnet publish "Evo.BI.DataConnector.csproj" \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Evo.BI.DataConnector.dll"]
