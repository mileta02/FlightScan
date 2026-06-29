FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY FlightScan.slnx .
COPY FlightScan.Api/FlightScan.Api.csproj FlightScan.Api/
COPY FlightScan.Application/FlightScan.Application.csproj FlightScan.Application/
COPY FlightScan.Core/FlightScan.Core.csproj FlightScan.Core/
COPY FlightScan.Infrastructure/FlightScan.Infrastructure.csproj FlightScan.Infrastructure/
COPY FlightScan.Tests/FlightScan.Tests.csproj FlightScan.Tests/

RUN dotnet restore FlightScan.slnx

FROM restore AS build
COPY . .
RUN dotnet publish FlightScan.Api/FlightScan.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "FlightScan.Api.dll"]
