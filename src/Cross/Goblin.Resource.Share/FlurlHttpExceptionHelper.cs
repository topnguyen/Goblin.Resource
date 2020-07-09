using System;
using System.Threading.Tasks;
using Flurl.Http;
using Goblin.Core.Errors;

namespace Goblin.Resource.Share
{
    public static class FlurlHttpExceptionHelper
    {
        public static async Task HandleErrorAsync(FlurlHttpException ex)
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
}