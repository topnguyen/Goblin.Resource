# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .

COPY Goblin.Core/Goblin.Core/*.csproj ./Goblin.Core/Goblin.Core/
COPY Goblin.Core/Goblin.Core.Web/*.csproj ./Goblin.Core/Goblin.Core.Web/

COPY src/Cross/Goblin.Resource.Core/*.csproj ./src/Cross/Goblin.Resource.Core/
COPY src/Cross/Goblin.Resource.Mapper/*.csproj ./src/Cross/Goblin.Resource.Mapper/
COPY src/Cross/Goblin.Resource.Share/*.csproj ./src/Cross/Goblin.Resource.Share/

COPY src/Repository/Goblin.Resource.Contract.Repository/*.csproj ./src/Repository/Goblin.Resource.Contract.Repository/
COPY src/Repository/Goblin.Resource.Repository/*.csproj ./src/Repository/Goblin.Resource.Repository/

COPY src/Service/Goblin.Resource.Contract.Service/*.csproj ./src/Service/Goblin.Resource.Contract.Service/
COPY src/Service/Goblin.Resource.Service/*.csproj ./src/Service/Goblin.Resource.Service/

COPY src/Web/Goblin.Resource/*.csproj ./src/Web/Goblin.Resource/

RUN dotnet restore

# copy everything else and build app

COPY Goblin.Core/Goblin.Core/. ./Goblin.Core/Goblin.Core/
COPY Goblin.Core/Goblin.Core.Web/. ./Goblin.Core/Goblin.Core.Web/

COPY src/Cross/Goblin.Resource.Core/. ./src/Cross/Goblin.Resource.Core/
COPY src/Cross/Goblin.Resource.Mapper/. ./src/Cross/Goblin.Resource.Mapper/
COPY src/Cross/Goblin.Resource.Share/. ./src/Cross/Goblin.Resource.Share/

COPY src/Repository/Goblin.Resource.Contract.Repository/. ./src/Repository/Goblin.Resource.Contract.Repository/
COPY src/Repository/Goblin.Resource.Repository/. ./src/Repository/Goblin.Resource.Repository/

COPY src/Service/Goblin.Resource.Contract.Service/. ./src/Service/Goblin.Resource.Contract.Service/
COPY src/Service/Goblin.Resource.Service/. ./src/Service/Goblin.Resource.Service/

COPY src/Web/Goblin.Resource/. ./src/Web/Goblin.Resource/

WORKDIR /source
RUN dotnet publish -c release -o /publish --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /publish
COPY --from=build /publish ./
ENTRYPOINT ["dotnet", "Goblin.Resource.dll"]