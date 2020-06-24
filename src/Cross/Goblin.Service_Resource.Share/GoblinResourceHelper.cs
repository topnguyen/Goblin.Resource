using System;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using Goblin.Core.Errors;
using Goblin.Service_Resource.Share.Models;

namespace Goblin.Service_Resource.Share
{
    public static class GoblinResourceHelper
    {
        public static string Domain { get; set; } = string.Empty;

        public static async Task<GoblinResourceFileModel> UploadAsync(GoblinResourceUploadFileModel model,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var fileModel = await Domain
                    .AppendPathSegment(GoblinResourceEndpoints.UploadFile)
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinResourceFileModel>()
                    .ConfigureAwait(true);

                return fileModel;
            }
            catch (FlurlHttpException ex)
            {
                var goblinErrorModel = await ex.GetResponseJsonAsync<GoblinErrorModel>().ConfigureAwait(true);

                if (goblinErrorModel != null)
                {
                    throw new GoblinException(goblinErrorModel);
                }

                var responseString = await ex.GetResponseStringAsync().ConfigureAwait(true);

                var message = responseString ?? ex.Message;

                throw new Exception(message);
            }
        }

        public static async Task<GoblinResourceFileModel> GetAsync(string slug, CancellationToken cancellationToken = default)
        {
            var fileModel = await Domain
                .AppendPathSegment(GoblinResourceEndpoints.GetFile)
                .SetQueryParam("slug", slug)
                .GetJsonAsync<GoblinResourceFileModel>(cancellationToken: cancellationToken)
                .ConfigureAwait(true);

            return fileModel;
        }

        public static async Task DeleteAsync(string slug, CancellationToken cancellationToken = default)
        {
            await Domain
                .AppendPathSegment(GoblinResourceEndpoints.DeleteFile)
                .SetQueryParam("slug", slug)
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(true);
        }
    }
}