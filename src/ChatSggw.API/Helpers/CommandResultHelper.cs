using Neat.CQRSLite.Contract.Commands;
using Neat.CQRSLite.Contract.Helpers;
using System.Net;
using System.Net.Http;

namespace ChatSggw.API.Helpers
{
    public static class CommandResultHelper
    {
        public static HttpResponseMessage ProceesForResult(this ICommand command, 
            Assistant assistant, HttpRequestMessage request)
        {
            var commandResult = assistant.Do(command);

            return commandResult.WasSuccessful()
                ? request.CreateResponse(HttpStatusCode.OK, "ok")
                : request.CreateResponse(HttpStatusCode.BadRequest, commandResult.ValidationErrors);
        }
    }
}