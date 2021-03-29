# https://medium.com/@oluwabukunmi.aluko/dockerize-asp-net-core-web-app-with-multiple-layers-projects-part1-2256aa1b0511

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY *.sln .
COPY CapWatchBackend.Application/*.csproj ./CapWatchBackend.Application/
COPY CapWatchBackend.DataAccess.MongoDB/*.csproj ./CapWatchBackend.DataAccess.MongoDB/
COPY CapWatchBackend.Domain/*.csproj ./CapWatchBackend.Domain/
COPY CapWatchBackend.WebApi/*.csproj ./CapWatchBackend.WebApi/

RUN dotnet restore

COPY CapWatchBackend.Application/. ./CapWatchBackend.Application/
COPY CapWatchBackend.DataAccess.MongoDB/. ./CapWatchBackend.DataAccess.MongoDB/
COPY CapWatchBackend.Domain/. ./CapWatchBackend.Domain/
COPY CapWatchBackend.WebApi/. ./CapWatchBackend.WebApi/

WORKDIR /app/CapWatchBackend.WebApi
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/sdk:5.0
WORKDIR /app

COPY --from=build-env /app/CapWatchBackend.WebApi/out .
ENTRYPOINT ["dotnet", "CapWatchBackend.WebApi.dll"]