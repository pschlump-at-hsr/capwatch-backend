FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY *.sln ./
COPY CapWatchBackend.Application/*.csproj ./CapWatchBackend.Application/
COPY CapWatchBackend.DataAccess.MongoDB/*.csproj ./CapWatchBackend.DataAccess.MongoDB/
COPY CapWatchBackend.Domain/*.csproj ./CapWatchBackend.Domain/
COPY CapWatchBackend.WebApi/*.csproj ./CapWatchBackend.WebApi/
COPY CapWatchBackend.WebApi.Tests/*.csproj ./CapWatchBackend.WebApi.Tests/

RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

EXPOSE 80

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CapWatchBackend.WebApi.dll"]