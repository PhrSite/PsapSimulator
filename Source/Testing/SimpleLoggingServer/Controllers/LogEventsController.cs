/////////////////////////////////////////////////////////////////////////////////////
//  File:   LogEventsController.cs                                  21 Aug 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using I3V3.LogEvents;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Ng911Common;
using Ng911Lib.Utilities;
using System.Net;

namespace SimpleLoggingServer.Controllers;

[Route("/[controller]")]
[Consumes("application/json")]
[ApiController]
public class LogEventsController : ControllerBase
{
    [HttpPost]
    public void PostLogEvent([FromBody] I3LogEventContent Content)
    {
        if (Content == null || Content.content == null || Content.content.payload == null)
        {
            Response.StatusCode = 400;  // 400 Bad Request
            return;
        }

        // Get the raw JSON string from the payload of the POST request
        string strLogEvent = I3Jws.Base64UrlStringToJsonString(Content.content.payload);

        // Make sure that the received raw JSON string can at least be deserialized as the LogEvent base class
        LogEvent Le = JsonHelper.DeserializeFromString<LogEvent>(strLogEvent);
        if (Le == null)
        {
            Response.StatusCode = 400;
            return;
        }

        IPAddress ClientIp = Request.HttpContext.Connection.RemoteIpAddress;
        int Port = Request.HttpContext.Connection.RemotePort;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Received: {Le.logEventType} from {ClientIp}:{Port} at {TimeUtils.GetCurrentNenaTimestamp()}");
        Console.ResetColor();
        Console.WriteLine(strLogEvent);
        Console.WriteLine();

        // This line of code shows how to set the reason phrase of the response.
        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "LogEvent Successfully Logged";

        Response.StatusCode = 201;
    }
}
