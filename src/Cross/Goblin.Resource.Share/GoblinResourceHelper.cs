using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Goblin.Core.Constants;
using Goblin.Core.Settings;
using Goblin.Resource.Share.Models;
using Goblin.Resource.Share.Validators;

namespace Goblin.Resource.Share
{
    public static class GoblinResourceHelper
    {
        public static string Domain { get; set; } = string.Empty;

        public static string AuthorizationKey { get; set; } = string.Empty;

        public static readonly ISerializer JsonSerializer = new NewtonsoftJsonSerializer(GoblinJsonSetting.JsonSerializerSettings);

        private static IFlurlRequest GetRequest(long? loggedInUserId)
        {
            var request = Domain.WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey);

            if (loggedInUserId != null)
            {
                request = request.WithHeader(GoblinHeaderKeys.UserId, loggedInUserId);
            }

            request = request.ConfigureRequest(x => { x.JsonSerializer = JsonSerializer; });

            return request;
        }

        public static async Task<GoblinResourceFileModel> UploadAsync(GoblinResourceUploadFileModel model, CancellationToken cancellationToken = default)
        {
            ValidationHelper.Validate<GoblinResourceUploadFileModelValidator, GoblinResourceUploadFileModel>(model);

            try
            {
                var endpoint = GetRequest(model.LoggedInUserId).AppendPathSegment(GoblinResourceEndpoints.UploadFile);

                var fileModel = await endpoint
                    .PostJsonAsync(model, cancellationToken: cancellationToken)
                    .ReceiveJson<GoblinResourceFileModel>()
                    .ConfigureAwait(true);

                return fileModel;
            }
            catch (FlurlHttpException ex)
            {
                await FlurlHttpExceptionHelper.HandleErrorAsync(ex).ConfigureAwait(true);

                return null;
            }
        }

        public static async Task<GoblinResourceFileModel> GetAsync(GoblinResourceGetFileModel model, CancellationToken cancellationToken = default)
        {
            var endpoint = GetRequest(model.LoggedInUserId)
                .AppendPathSegment(GoblinResourceEndpoints.GetFile)
                .SetQueryParam("slug", model.Slug)
                .SetQueryParam(GoblinHeaderKeys.UserId, model.LoggedInUserId);

            var fileModel = await endpoint
                .GetJsonAsync<GoblinResourceFileModel>(cancellationToken: cancellationToken)
                .ConfigureAwait(true);

            return fileModel;
        }

        public static async Task DeleteAsync(GoblinResourceDeleteFileModel model, CancellationToken cancellationToken = default)
        {
            var endpoint = GetRequest(model.LoggedInUserId)
                .AppendPathSegment(GoblinResourceEndpoints.DeleteFile)
                .SetQueryParam("slug", model.Slug);

            await endpoint
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(true);
        }
    }
}