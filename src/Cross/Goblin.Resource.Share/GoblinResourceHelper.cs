using System;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using Goblin.Core.Constants;
using Goblin.Core.Errors;
using Goblin.Core.Settings;
using Goblin.Resource.Share.Models;

namespace Goblin.Resource.Share
{
    public static class GoblinResourceHelper
    {
        public static string Domain { get; set; } = string.Empty;

        public static string AuthorizationKey { get; set; } = string.Empty;

        public static readonly ISerializer JsonSerializer =
            new NewtonsoftJsonSerializer(GoblinJsonSetting.JsonSerializerSettings);

        public static async Task<GoblinResourceFileModel> UploadAsync(GoblinResourceUploadFileModel model,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = Domain
                    .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                    .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                    .AppendPathSegment(GoblinResourceEndpoints.UploadFile);

                var fileModel = await endpoint
                    .ConfigureRequest(x =>
                    {
                        x.JsonSerializer = JsonSerializer;
                    })
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

        public static async Task<GoblinResourceFileModel> GetAsync(GoblinResourceGetFileModel model,
            CancellationToken cancellationToken = default)
        {
            var endpoint = Domain
                .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                .AppendPathSegment(GoblinResourceEndpoints.GetFile)
                .SetQueryParam("slug", model.Slug)
                .SetQueryParam(GoblinHeaderKeys.UserId, model.LoggedInUserId);

            var fileModel = await endpoint
                .ConfigureRequest(x =>
                {
                    x.JsonSerializer = JsonSerializer;
                })
                .GetJsonAsync<GoblinResourceFileModel>(cancellationToken: cancellationToken)
                .ConfigureAwait(true);

            return fileModel;
        }

        public static async Task DeleteAsync(GoblinResourceDeleteFileModel model,
            CancellationToken cancellationToken = default)
        {
            var endpoint = Domain
                .WithHeader(GoblinHeaderKeys.Authorization, AuthorizationKey)
                .WithHeader(GoblinHeaderKeys.UserId, model.LoggedInUserId)
                .AppendPathSegment(GoblinResourceEndpoints.DeleteFile)
                .SetQueryParam("slug", model.Slug);

            await endpoint
                .ConfigureRequest(x =>
                {
                    x.JsonSerializer = JsonSerializer;
                })
                .DeleteAsync(cancellationToken)
                .ConfigureAwait(true);
        }
    }
}