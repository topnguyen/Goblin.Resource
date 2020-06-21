pushd Repository\Goblin.Service_Resource.Repository
dotnet ef migrations add %1 -v --context DbContext
dotnet ef database update -v --context DbContext
popd