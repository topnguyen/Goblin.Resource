namespace Goblin.Service_Resource.Share
{
    public static class Endpoints
    {
        /// <summary>
        ///     Upload file endpoint <br />
        ///     HTTP Method: PUT
        /// </summary>
        public const string UploadFile = "/files";
        
        /// <summary>
        ///     Get file information endpoint <br />
        ///     Add "slug" parameter in Query String <br />
        ///     HTTP Method: GET
        /// </summary>
        public const string GetFile = "/files";

        /// <summary>
        ///     Delete a file endpoint <br />
        ///     Add "slug" parameter in Query String <br />
        ///     HTTP Method: DELETE
        /// </summary>
        public const string DeleteFile = "/files";
    }
}