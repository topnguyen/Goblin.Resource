namespace Goblin.Service_Resource.Share
{
    public static class Service_Resource_Endpoints
    {
        public const string Base = "~/sample";

        public const string AddEndpoint = Base; // POST
        public const string GetEndpoint = Base + "/{id}";// GET
        public const string UpdateEndpoint = Base + "/{id}"; // PUT
        public const string DeleteEndpoint = Base + "/{id}"; // Delete
        public const string GetPagedEndpoint = Base; //GET
    }
}
