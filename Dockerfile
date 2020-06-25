# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .

COPY Goblin.Core/Goblin.Core/*.csproj ./Goblin.Core/Goblin.Core/
COPY Goblin.Core/Goblin.Core.Web/*.csproj ./Goblin.Core/Goblin.Core.Web/

COPY src/Cross/Goblin.Service_Resource.Core/*.csproj ./src/Cross/Goblin.Service_Resource.Core/
COPY src/Cross/Goblin.Service_Resource.Mapper/*.csproj ./src/Cross/Goblin.Service_Resource.Mapper/
COPY src/Cross/Goblin.Service_Resource.Share/*.csproj ./src/Cross/Goblin.Service_Resource.Share/

COPY src/Repository/Goblin.Service_Resource.Contract.Repository/*.csproj ./src/Repository/Goblin.Service_Resource.Contract.Repository/
COPY src/Repository/Goblin.Service_Resource.Repository/*.csproj ./src/Repository/Goblin.Service_Resource.Repository/

COPY src/Service/Goblin.Service_Resource.Contract.Service/*.csproj ./src/Service/Goblin.Service_Resource.Contract.Service/
COPY src/Service/Goblin.Service_Resource.Service/*.csproj ./src/Service/Goblin.Service_Resource.Service/

COPY src/Web/Goblin.Service_Resource/*.csproj ./src/Web/Goblin.Service_Resource/

COPY src/Test/Goblin.Service_Resource.Test/*.csproj ./src/Test/Goblin.Service_Resource.Test/

RUN dotnet restore

# copy everything else and build app

COPY Goblin.Core/Goblin.Core/. ./Goblin.Core/Goblin.Core/
COPY Goblin.Core/Goblin.Core.Web/. ./Goblin.Core/Goblin.Core.Web/

COPY src/Cross/Goblin.Service_Resource.Core/. ./src/Cross/Goblin.Service_Resource.Core/
COPY src/Cross/Goblin.Service_Resource.Mapper/. ./src/Cross/Goblin.Service_Resource.Mapper/
COPY src/Cross/Goblin.Service_Resource.Share/. ./src/Cross/Goblin.Service_Resource.Share/

COPY src/Repository/Goblin.Service_Resource.Contract.Repository/. ./src/Repository/Goblin.Service_Resource.Contract.Repository/
COPY src/Repository/Goblin.Service_Resource.Repository/. ./src/Repository/Goblin.Service_Resource.Repository/

COPY src/Service/Goblin.Service_Resource.Contract.Service/. ./src/Service/Goblin.Service_Resource.Contract.Service/
COPY src/Service/Goblin.Service_Resource.Service/. ./src/Service/Goblin.Service_Resource.Service/

COPY src/Web/Goblin.Service_Resource/. ./src/Web/Goblin.Service_Resource/

COPY src/Test/Goblin.Service_Resource.Test/. ./src/Test/Goblin.Service_Resource.Test/

WORKDIR /source
RUN dotnet publish -c release -o /publish --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /publish
COPY --from=build /publish ./
ENTRYPOINT ["dotnet", "Goblin.Service_Resource.dll"]