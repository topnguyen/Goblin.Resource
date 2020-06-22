pushd Repository\Goblin.Service_Resource.Repository
dotnet ef migrations add %1 -v --context Service_Resource_DbContext
dotnet ef database update -v --context Service_Resource_DbContext
popd